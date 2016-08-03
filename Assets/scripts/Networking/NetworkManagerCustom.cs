using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkManagerCustom : MonoBehaviour
{
    public NetworkManager manager;
    public GameObject ServerObject;
    public GameObject serverButton;
    public GameObject clientButton;
    public TextMesh connectionText;
    public TextMesh numConsText;
    public TextMesh ip;
    public string connectionInfo = "localHost";
    public string[] connectionList = { "192.168.3.137", "192.168.7.83" };
    public int connectionPort = 25001;

    public GameObject cycleIpUpButton;
    public GameObject cycleIpDownButton;

    private buttonControl sb;
    private buttonControl cb;
    private GameObject g;
    private bool canChangeValue = true;
    private int conListNum;

    private debugObject debugObject;


    void Awake()
    {
        manager = GetComponent<NetworkManager>();
        manager.networkAddress = connectionInfo;
        manager.networkPort = connectionPort;

        if (!debugObject)
            debugObject = ObjectFinder.Find<debugObject>();
    }

    void Start()
    {
        if (!debugObject)
            return;

        serverButton = debugObject.serverButton;
        clientButton = debugObject.clientButton;
        connectionText = debugObject.connectionText;
        numConsText = debugObject.numConsText;
        ip = debugObject.ipText;
        ip.text = (connectionInfo + " : " + connectionPort);
        sb = serverButton.GetComponent<buttonControl>();
        cb = clientButton.GetComponent<buttonControl>();
        connectionText.text = "Disconnected";
        cycleIpUpButton = debugObject.ipUpButton;
        cycleIpDownButton = debugObject.ipDownButton;
    }

    void Update()
    {
        if (!debugObject)
            return;

        connectionText = debugObject.connectionText;
        numConsText = debugObject.numConsText;
        ip = debugObject.ipText;

        if (cycleIpDownButton.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            conListNum--;
            if (conListNum < 0)
            {
                conListNum = connectionList.Length - 1;
            }
            canChangeValue = false;
            StartCoroutine(Wait(0.2f));
        }

        if (cycleIpUpButton.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            conListNum++;
            if (conListNum > connectionList.Length - 1)
            {
                conListNum = 0;
            }
            canChangeValue = false;
            StartCoroutine(Wait(0.2f));
        }

        connectionInfo = connectionList[conListNum];
        manager.networkAddress = connectionInfo;
        ip.text = (connectionInfo + " : " + connectionPort);
        connectionText = debugObject.connectionText;
        numConsText = debugObject.numConsText;

        if (clientButton && serverButton)
        {
            if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
            {
               if (cb.active && canChangeValue)
               {
                   // Debug.Log("Starting client");
                   canChangeValue = false;
                   manager.StartClient();
                   StartCoroutine(Wait(1.0f));
               }
               if (sb.active && canChangeValue)
               {
                   // Debug.Log("Starting server");
                   canChangeValue = false;
                   manager.StartHost();
                   StartCoroutine(Wait(1.0f));
               }
            }
            if (NetworkServer.active && NetworkServer.localClientActive)
            {
                g = GameObject.FindWithTag("ServerData");
                if (g == null)
                {
                    Spawn();
                }
                if (NetworkServer.localClientActive)
                {
                    connectionText.text = ("Hosting: " + manager.networkAddress + " : " + manager.networkPort);
                    numConsText.text = manager.numPlayers.ToString();
                }
            }
            if (NetworkClient.active)
            {
                connectionText.text = ("Client: " + manager.networkAddress + " : " + manager.networkPort);
                numConsText.text = manager.numPlayers.ToString();
            }
        }
    }

    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canChangeValue = true;
    }

    public void Spawn()
    {
        // As this is spawned only once by the host we can assume there will only be one player in the scene.
        var player = GameObject.FindWithTag("Player");
        
        if (player != null)
        {
            var toId = player.GetComponent<NetworkIdentity>();
            var conn = toId.connectionToClient;
            var sd = (GameObject) Instantiate(ServerObject, new Vector3(200, 0, 0), Quaternion.identity);
            NetworkServer.SpawnWithClientAuthority(sd, conn);
        }

    }

    public void StartClient()
    {
        Debug.Log("Starting client");
        canChangeValue = false;
        manager.StartClient();

        if (canChangeValue)
        {
            Debug.Log("Starting client");
            canChangeValue = false;
            manager.StartClient();
        }
    }

    public void StartServer()
    {
        if (NetworkServer.active && NetworkServer.localClientActive)
        {
            g = GameObject.FindWithTag("ServerData");
            if (g == null)
            {
                Spawn();
            }
        }
        
        if (canChangeValue)
        {
            Debug.Log("Starting Server");
            canChangeValue = false;
            manager.StartHost();
        }
    }
}
