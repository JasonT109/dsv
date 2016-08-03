using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkManagerCustom : MonoBehaviour
{
    
    // Constants
    // ------------------------------------------------------------

    /** Timeout for interactive startup attempts (seconds). */
    private const float DefaultStartupTimeout = 2;

    /** Timeout for automatic session startup attempts (seconds). */
    private const float AutoStartupTimeout = 10;

    /** Name of the password screen. */
    private const string LoginScreenScene = "offline_scene";


    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** The server data object. */
    public GameObject ServerObject;


    [Header("Configuration")]

    /** Server host to connect to. */
    public string Host = "localhost";

    /** Server port to connect to. */
    public int Port = 25001;
    
    /** The UNET networking manager. */
    public NetworkManager UNet { get; private set; }


    // Private Properties
    // ------------------------------------------------------------

    /** Whether a network session is active for this instance. */
    private bool HasSession
        { get { return NetworkClient.active || NetworkServer.active || UNet.matchMaker != null; } }

    /** Whether this instance is acting as a host. */
    private static bool IsHost
        { get { return NetworkServer.active && NetworkServer.localClientActive; } }


    // Members
    // ------------------------------------------------------------

    /** Time at which next start attempt can be made. */
    private float _nextStartTime;

    /** The server game object instance. */
    private GameObject _serverObject;


    // Unity Methods
    // ------------------------------------------------------------

    /** Preinitialization. */
    private void Awake()
    {
        // Get default connection state.
        Host = Configuration.Get("server-ip", Network.player.ipAddress);
        Port = Configuration.Get("server-port", Port);

        // Configure UNET network manager with default connection.
        UNet = GetComponent<NetworkManager>();
        UNet.networkAddress = Host;
        UNet.networkPort = Port;
    }

    /** Startup. */
    private void Start()
    {
        // Perform an inital automatic startup attempt.
        if (!HasSession && CanAttemptStartup())
            AttemptAutoStartup();
    }

    /** Updating. */
    private void Update()
    {
        // Spawn server object if needed.
        if (IsHost)
            EnsureServerObjectExists();

        // Periodically attempt automatic startup.
        if (!HasSession && CanAttemptStartup())
            AttemptAutoStartup();
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Attempt automatic session startup. */
    private void AttemptAutoStartup()
    {
        // Check if we have an existing session.
        if (HasSession || !CanAttemptStartup())
            return;

        // Check if we should start a client or server session immediately.
        var attempted = false;
        if (Configuration.Has("client"))
            attempted = StartClient();
        else if (Configuration.Has("server"))
            attempted = StartServer();
        else if (Configuration.Has("host"))
            attempted = StartServer();
        else if (!IsLoginScreen())
            attempted = StartServer();

        // Schedule the next auto-attempt (if we made one.)
        if (attempted)
            ScheduleNextAttempt(AutoStartupTimeout);
    }

    /** Start up a client session. */
    public bool StartClient()
    {
        if (!CanAttemptStartup())
            return false;
        
        Debug.Log(string.Format("Starting client session - connecting to host at {0}:{1}..", Host, Port));
        UNet.StartClient();
        ScheduleNextAttempt();
        return true;
    }

    /** Start up a server (host) session. */
    public bool StartServer()
    {
        if (!CanAttemptStartup())
            return false;

        Debug.Log("Starting server session..");
        EnsureServerObjectExists();
        UNet.StartHost();
        ScheduleNextAttempt();
        return true;
    }

    /** Whether a startup attempt can be made at present. */
    private bool CanAttemptStartup()
        { return Time.time >= _nextStartTime; }

    /** Schedule the next possible time for a startup attempt. */
    private void ScheduleNextAttempt()
        { _nextStartTime = Time.time + DefaultStartupTimeout; }

    /** Schedule the next possible time for a startup attempt. */
    private void ScheduleNextAttempt(float timeout)
        { _nextStartTime = Time.time + timeout; }

    /** Check if we're in the initial password screen. */
    private bool IsLoginScreen()
        { return SceneManager.GetActiveScene().name == LoginScreenScene; }

    /** Try to ensure the server object exists. */
    private void EnsureServerObjectExists()
    {
        // Check if we're acting as the host.
        if (!IsHost)
            return;

        // Check if server object exists.
        if (_serverObject)
            return;

        // Look for server object in scene, and spawn it if needed.
        _serverObject = GameObject.FindWithTag("ServerData");
        if (_serverObject == null)
            SpawnServerObject();
    }

    /** Spawn the server object from the host. */
    private void SpawnServerObject()
    {
        // Check if we're acting as the host.
        if (!IsHost)
            return;

        // As this is spawned only once by the host, we can assume there will only be one player in the scene.
        var player = GameObject.FindWithTag("Player");
        if (!player)
            return;

        // Spawn the server object.
        Debug.Log("Spawning server object.");
        var toId = player.GetComponent<NetworkIdentity>();
        var conn = toId.connectionToClient;
        var sd = (GameObject) Instantiate(ServerObject, new Vector3(200, 0, 0), Quaternion.identity);
        NetworkServer.SpawnWithClientAuthority(sd, conn);
    }

}
