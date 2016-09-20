using UnityEngine;
using UnityEngine.Networking;
using Meg.Networking;

public class OSRov : NetworkBehaviour
{
    [SyncVar]
    public float RovLightSBoard;
    [SyncVar]
    public float RovLightPort;
    [SyncVar]
    public float RovLightBow;

    [SyncVar]
    public bool RoVBootUp;
    [SyncVar]
    public bool RovLostSignal;

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}