using UnityEngine;
using UnityEngine.Networking;
using Meg.Networking;

public class MotionBaseData : NetworkBehaviour
{
    [SyncVar]
    public float MotionBasePitch;
    [SyncVar]
    public float MotionBaseYaw;
    [SyncVar]
    public float MotionBaseRoll;
    [SyncVar]
    public float MotionDampen;
    [SyncVar]
    public bool MotionSafety = true;
    [SyncVar]
    public bool MotionHazard = false;
    [SyncVar]
    public float MotionSlerpSpeed = 2f;
    [SyncVar]
    public float MotionHazardSensitivity = 15f;
    [SyncVar]
    public bool MotionHazardEnabled = true;

    //todo syncvar these
    [SyncVar]
    public float MotionPitchMax = 37f;
    [SyncVar]
    public float MotionRollMax = 37f;
    [SyncVar]
    public float MotionYawMax = 37f;


    // Use this for initialization
    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
