using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class gliderErrorData : NetworkBehaviour
{
    [SyncVar]
    public float error_thruster_l = 1f;
    [SyncVar]
    public float error_thruster_r = 1f;
    [SyncVar]
    public float error_vertran_l = 1f;
    [SyncVar]
    public float error_vertran_r = 1f;
    [SyncVar]
    public float error_jet_l = 1f;
    [SyncVar]
    public float error_jet_r = 1f;
    [SyncVar]
    public float thruster_heat_l = 48;
    [SyncVar]
    public float thruster_heat_r = 52f;
    [SyncVar]
    public float vertran_heat_l = 36f;
    [SyncVar]
    public float vertran_heat_r = 39f;
    [SyncVar]
    public float jet_heat_l = 36f;
    [SyncVar]
    public float jet_heat_r = 39f;
    [SyncVar]
    public float error_panel_l = 1f;
    [SyncVar]
    public float error_panel_r = 1f;

    [SyncVar]
    public float error_pressure = 1f;
    [SyncVar]
    public float error_structural = 1f;
    [SyncVar]
    public float error_grapple = 1f;
    [SyncVar]
    public float error_system = 1f;

}
