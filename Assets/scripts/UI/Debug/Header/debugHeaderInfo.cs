using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class debugHeaderInfo : MonoBehaviour
{

    public Text Role;
    public Text IP;

    public float UpdateInterval = 1;

    private float _nextUpdateTime;
    
    public Color ClientColor = Color.green;
    public Color HostColor = Color.yellow;

    private void Update()
    {
        if (Time.time < _nextUpdateTime)
            return;

        if (!NetworkManager.singleton)
            return;

        _nextUpdateTime = Time.time + UpdateInterval;

        if (NetworkServer.active)
        {
            Role.text = "SERVER";
            Role.color = HostColor;
            IP.text = string.Format("{0}:{1} ({2} connection{3})",
                NetworkManager.singleton.networkAddress,
                NetworkManager.singleton.networkPort,
                NetworkServer.connections.Count,
                NetworkServer.connections.Count != 1 ? "s" : "");
        }
        else if (NetworkClient.active)
        {
            Role.text = "CLIENT";
            Role.color = ClientColor;
            IP.text = string.Format("{0}:{1}",
                NetworkManager.singleton.networkAddress,
                NetworkManager.singleton.networkPort);
        }
        else
        {
            Role.text = "DISCONNECTED";
        }

    }
}
