using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class oxygenData : NetworkBehaviour
{
    [SyncVar]
    public float oxygenTank1;
    [SyncVar]
    public float oxygenTank2;
    [SyncVar]
    public float oxygenTank3;
    [SyncVar]
    public float oxygenTank4;
    [SyncVar]
    public float oxygenTank5;
    [SyncVar]
    public float oxygenTank6;
    [SyncVar]
    public float oxygenTank7;
}
