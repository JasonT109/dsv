using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Meg.Networking;

public class NetworkManagerCustom : MonoBehaviour
{
    
    // Constants
    // ------------------------------------------------------------

    /** Timeout for interactive startup attempts (seconds). */
    public const float DefaultStartupTimeout = 2;

    /** Timeout for automatic session startup attempts (seconds). */
    public const float AutoStartupTimeout = 10;

    /** Name of the password screen. */
    public const string LoginScene = "offline_scene";

    /** Name of the gliders screen. */
    public const string GliderScene = "screen_gliders";

    /** Name of the main submarine screen. */
    public const string BigSubScene = "screen_subs";

    /** Name of the scene containing DCC screens. */
    public const string DccScene = "screen_dcc";


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

    /** Scene to start up in when hosting/joining a session. */
    public string Scene { get; private set; }

    /** Whether session will be automatically started. */
    public bool AutoStart
        { get { return ShouldAutoStart(); } }


    // Private Properties
    // ------------------------------------------------------------

    /** The underlying networking manager. */
    private Meg.Networking.NetworkManager _manager;

    /** Number of previous scene loads. */
    private static int _loadCount;

    /** Whether a network session is active for this instance. */
    private bool HasSession
        { get { return NetworkClient.active || NetworkServer.active || _manager.matchMaker != null; } }

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
        Debug.Log(string.Format("NetworkManagerCustom.Awake(id:{0})", Id));

        // Get default scene from the UNET network manager.
        _manager = GetComponent<Meg.Networking.NetworkManager>();

        // Get default connection state.
        Host = Configuration.Get("server-ip", Network.player.ipAddress);
        Port = Configuration.Get("server-port", Port);
        Scene = Configuration.Get("network-scene", _manager.onlineScene);

        // Configure UNET network manager with default connection.
        _manager.networkAddress = Host;
        _manager.networkPort = Port;
        
        // Modify UNET configuration to better tolerate packet loss, etc.
        // _manager.connectionConfig.NetworkDropThreshold = 95;
    }

    /** Startup. */
    private void Start()
    {
        Debug.Log(string.Format("NetworkManagerCustom.Start(id:{0})", Id));

        // Schedule inital automatic startup attempt.
        ScheduleNextAttempt();
    }

    /** Updating. */
    private void Update()
    {
        // Spawn server object if needed.
        // if (IsHost)
        //    EnsureServerObjectExists();

        // Periodically attempt automatic startup.
        if (!HasSession && CanAttemptStartup())
            AttemptAutoStartup();
    }

    /** Called when a level load occurs. */
    private void OnLevelWasLoaded()
    {
        _loadCount++;
        Debug.Log(string.Format("NetworkManagerCustom.OnLevelWasLoaded(id:{0}): loadCount is now {1}", Id, _loadCount));
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Set the scene to start when joining a session. */
    public void SetScene(string value)
    {
        Debug.Log(string.Format("NetworkManagerCustom.SetScene(id:{0}, {1}", Id, value));
        Scene = value;
        _manager.onlineScene = Scene;
    }

    /** Attempt to start up a client session. */
    public bool StartClient()
    {
        if (!CanAttemptStartup())
            return false;

        Debug.Log(string.Format("NetworkManagerCustom.StartClient(id:{0}): Starting client session - connecting to host at {1}:{2}, scene '{3}'..", Id, Host, Port, Scene));
        _manager.onlineScene = Scene;
        _manager.StartClient();

        ScheduleNextAttempt();
        return true;
    }

    /** Attempt to start up a server (host) session. */
    public bool StartServer()
    {
        if (!CanAttemptStartup())
            return false;

        Debug.Log(string.Format("NetworkManagerCustom.StartClient(id:{0}): Starting server session - scene '{1}'..", Id, Scene));
        EnsureServerObjectExists();
        _manager.onlineScene = Scene;
        _manager.StartHost();

        ScheduleNextAttempt();
        return true;
    }


    // Private Methods
    // ------------------------------------------------------------

    private static string Id
        { get { return Configuration.Instance.CurrentId; } }

    /** Whether autostartup can be attempted. */
    private bool ShouldAutoStart()
    {
        var role = Configuration.Get("network-role", "");
        if (string.IsNullOrEmpty(role))
            return !IsLoginScreen();

        // Can't autoload more than once.
        if (_loadCount > 1)
            return false;

        return true;
    }

    /** Attempt automatic session startup. */
    private void AttemptAutoStartup()
    {
        // Check if we have an existing session.
        if (HasSession || _loadCount > 1 || !CanAttemptStartup())
            return;

        // Check if we should start a client or server session immediately.
        // If we're not in the login screen and no session exists, must be in the editor.
        // In that case we should start up a local server automatically.
        var attempted = false;
        var role = Configuration.Get("network-role", "").ToLower();
        if (!IsLoginScreen())
            attempted = StartServer();
        else if (role == "client")
            attempted = StartClient();
        else if (role == "server" || role == "host")
            attempted = StartServer();

        // Schedule the next auto-attempt (if we made one.)
        if (attempted)
            ScheduleNextAttempt(AutoStartupTimeout);
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
        { return SceneManager.GetActiveScene().name == LoginScene; }

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
        Debug.Log(string.Format("NetworkManagerCustom.EnsureServerObjectExists(id:{0}): Spawning server object.", Id));
        var toId = player.GetComponent<NetworkIdentity>();
        var conn = toId.connectionToClient;
        var sd = (GameObject) Instantiate(ServerObject, new Vector3(200, 0, 0), Quaternion.identity);
        NetworkServer.SpawnWithClientAuthority(sd, conn);
    }

}
