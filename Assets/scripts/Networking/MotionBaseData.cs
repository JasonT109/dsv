using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using Meg.Networking;

public class MotionBaseData : NetworkBehaviour
{
    [SyncVar]
    public int MotionComPort;
    [SyncVar]
    public bool MotionComPortOpen;

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

    //TODO SyncVar
    public bool MotionBaseHost = true;
    [SyncVar]
    public bool DecoupleMotionBase = false;


    public override void OnStartServer()
    {
        base.OnStartServer();

        if (!Manager)
            return;

        // Retrieve the COM port specified in login screen and save it as a parameter.
        var match = Regex.Match(Manager.ComPort, @"^.*(\d+)$");
        if (match.Success)
            MotionComPort = int.Parse(match.Groups[1].Value);
        else
            MotionComPort = 0;
    }

    private ArduinoManager _manager;
    private ArduinoManager Manager
    {
        get
        {
            if (!_manager)
                _manager = FindManager();

            return _manager;
        }
    }

    private static ArduinoManager FindManager()
    {
        var go = GameObject.FindGameObjectWithTag("ArduinoManager");
        return go ? go.GetComponent<ArduinoManager>() : null;
    }
}
