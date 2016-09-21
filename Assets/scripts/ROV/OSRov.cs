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
    public bool RoVLaunched;
    [SyncVar]
    public bool RovLostSignal;

    public int RovState;
    //0 = not launched
    //1 = launched
    //2 = eaten by a shark **SPOILER ALERT**

    // Use this for initialization
    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(RoVLaunched && RovState != 1)
        {
            RovState = 1;
            //launched state
        }
    }
}