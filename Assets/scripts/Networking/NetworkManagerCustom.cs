using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkManagerCustom : MonoBehaviour
{
    public NetworkManager manager;
    public GameObject ServerObject;
    public GameObject debugObj;
    public GameObject serverButton;
    public GameObject clientButton;
    public TextMesh connectionText;
    public TextMesh numConsText;
    public TextMesh ip;
    public string connectionInfo = "localHost";
    public string[] connectionList = new string[] { "192.168.3.137", "192.168.7.83" };
    public int connectionPort = 25001;

    public GameObject cycleIpUpButton;
    public GameObject cycleIpDownButton;

    private buttonControl sb;
    private buttonControl cb;
    private GameObject g;
    private bool canChangeValue = true;
    private int conListNum = 0;

    // Runtime variable
    bool showServer = false;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
        manager.networkAddress = connectionInfo;
        manager.networkPort = connectionPort;
        
    }

    void Start()
    {
        debugObj = GameObject.FindWithTag("Debug");

        if (debugObj)
        {
            serverButton = debugObj.GetComponent<debugObject>().serverButton;
            clientButton = debugObj.GetComponent<debugObject>().clientButton;
            connectionText = debugObj.GetComponent<debugObject>().connectionText;
            numConsText = debugObj.GetComponent<debugObject>().numConsText;
            ip = debugObj.GetComponent<debugObject>().ipText;
            ip.text = (connectionInfo + " : " + connectionPort.ToString());
            sb = serverButton.GetComponent<buttonControl>();
            cb = clientButton.GetComponent<buttonControl>();
            connectionText.text = "Disconnected";
            cycleIpUpButton = debugObj.GetComponent<debugObject>().ipUpButton;
            cycleIpDownButton = debugObj.GetComponent<debugObject>().ipDownButton;
        }
    }

    void Update()
    {
        if (debugObj == null)
        {
            debugObj = GameObject.FindWithTag("Debug");
            if (debugObj)
            {
                serverButton = debugObj.GetComponent<debugObject>().serverButton;
                clientButton = debugObj.GetComponent<debugObject>().clientButton;
                connectionText = debugObj.GetComponent<debugObject>().connectionText;
                numConsText = debugObj.GetComponent<debugObject>().numConsText;
                ip = debugObj.GetComponent<debugObject>().ipText;
                cycleIpUpButton = debugObj.GetComponent<debugObject>().ipUpButton;
                cycleIpDownButton = debugObj.GetComponent<debugObject>().ipDownButton;
                cycleIpUpButton = debugObj.GetComponent<debugObject>().ipUpButton;
                cycleIpDownButton = debugObj.GetComponent<debugObject>().ipDownButton;
            }
        }

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
        ip.text = (connectionInfo + " : " + connectionPort.ToString());

        if (!debugObj)
        {
            debugObj = GameObject.FindWithTag("Debug");
            if (debugObj)
            {
                serverButton = debugObj.GetComponent<debugObject>().serverButton;
                clientButton = debugObj.GetComponent<debugObject>().clientButton;
                connectionText = debugObj.GetComponent<debugObject>().connectionText;
                numConsText = debugObj.GetComponent<debugObject>().numConsText;
                sb = serverButton.GetComponent<buttonControl>();
                cb = clientButton.GetComponent<buttonControl>();
            }
        }
        else if (debugObj && clientButton && serverButton)
        {
            if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
            {
                //connectionText.text = "Disconnected";
               if (cb.active && canChangeValue)
               {
                   //Debug.Log("Starting client");
                   canChangeValue = false;
                   manager.StartClient();
                   StartCoroutine(Wait(1.0f));
               }
               if (sb.active && canChangeValue)
               {
                   //Debug.Log("Starting server");
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
        //as this is spawned only once by the host we can assume there will only be one player in the scene
        GameObject player = GameObject.FindWithTag("Player");
        
        if (player != null)
        {
            NetworkIdentity toId = player.GetComponent<NetworkIdentity>();
            var conn = toId.connectionToClient;
            GameObject sd = (GameObject)Instantiate(ServerObject, new Vector3(200, 0, 0), Quaternion.identity);
            //NetworkServer.Spawn(sd);
            NetworkServer.SpawnWithClientAuthority(sd, conn);
        }

    }

    public void StartClient()
    {
        Debug.Log("Starting client");
        canChangeValue = false;
        manager.StartClient();
        if(canChangeValue)
        {
            Debug.Log("Starting client");
            canChangeValue = false;
            manager.StartClient();
            //StartCoroutine(Wait(1.0f));
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
        
        if(canChangeValue)
        {
            Debug.Log("Starting Server");
            canChangeValue = false;
            manager.StartHost();
            //StartCoroutine(Wait(1.0f));
        }
    }
}
