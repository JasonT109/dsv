using UnityEngine;
using System.Collections;

public class connectNetwork : MonoBehaviour {
	
	public string IP = "127.0.0.1";
	public int Port = 25001;

    public GameObject serverButton;
    public GameObject clientButton;

    public TextMesh connectionText;

    public buttonControl sb;
    public buttonControl cb;

    private bool canChangeValue = true;

    void Start()
    {
        sb = serverButton.GetComponent<buttonControl>();
        cb = clientButton.GetComponent<buttonControl>();
        connectionText.text = "Disconnected";
    }
	
    void Update()
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            connectionText.text = "Disconnected";

            if (cb.active && canChangeValue)
            {
                canChangeValue = false;
                Network.Connect(IP, Port);
                StartCoroutine(Wait(0.2f));
            }
            if (sb.active && canChangeValue)
            {
                canChangeValue = false;
                Network.InitializeServer(10, Port, false);
                StartCoroutine(Wait(0.2f));
            }
        }
        else {
            if (Network.peerType == NetworkPeerType.Client)
            {
                connectionText.text = "Connected";

                if (!cb.active && canChangeValue)
                {
                    canChangeValue = false;
                    Network.Disconnect(250);
                    StartCoroutine(Wait(0.2f));
                }

            }
            if (Network.peerType == NetworkPeerType.Server)
            {
                connectionText.text = ("Connections: " + Network.connections.Length);

                if (!sb.active && canChangeValue)
                {
                    canChangeValue = false;
                    Network.Disconnect(250);
                    StartCoroutine(Wait(0.2f));
                }
            }
        }
    }

    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canChangeValue = true;
    }
}
