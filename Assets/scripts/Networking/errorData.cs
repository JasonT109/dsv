using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class errorData : NetworkBehaviour
{
    [SyncVar]
    public float error_bilgeLeak = 1f;
    [SyncVar]
    public float error_batteryLeak = 1f;
    [SyncVar]
    public float error_electricLeak = 1f;
    [SyncVar]
    public float error_oxygenExt = 1f;
    [SyncVar]
    public float error_vhf = 1f;
    [SyncVar]
    public float error_forwardSonar = 1f;
    [SyncVar]
    public float error_depthSonar = 1f;
    [SyncVar]
    public float error_doppler = 1f;
    [SyncVar]
    public float error_gps = 1f;
    [SyncVar]
    public float error_cpu = 1f;
    [SyncVar]
    public float error_vidhd = 1f;
    [SyncVar]
    public float error_datahd = 1f;
    [SyncVar]
    public float error_tow = 1f;
    [SyncVar]
    public float error_radar = 1f;
    [SyncVar]
    public float error_sternLights = 1f;
    [SyncVar]
    public float error_bowLights = 1f;
    [SyncVar]
    public float error_portLights = 1f;
    [SyncVar]
    public float error_bowThruster = 1f;
    [SyncVar]
    public float error_hyrdaulicRes = 1f;
    [SyncVar]
    public float error_starboardLights = 1f;
    [SyncVar]
    public float error_runningLights = 1f;
    [SyncVar]
    public float error_ballastTank = 1f;
    [SyncVar]
    public float error_hydraulicPump = 1f;
    [SyncVar]
    public float error_oxygenPump = 1f;
}
