using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class batteryData : NetworkBehaviour
{
    [SyncVar]
    public float bank1;
    [SyncVar]
    public float bank2;
    [SyncVar]
    public float bank3;
    [SyncVar]
    public float bank4;
    [SyncVar]
    public float bank5;
    [SyncVar]
    public float bank6;
    [SyncVar]
    public float bank7;
}
