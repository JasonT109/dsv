using UnityEngine;
using System.Collections;
using Meg.Networking;
using UnityEngine.Networking;

public class dockingData : NetworkBehaviour
{
    [SyncVar]
    public int docking1 = 0;

    [SyncVar]
    public int docking2 = 0;

    [SyncVar]
    public int docking3 = 0;

    [SyncVar]
    public int docking4 = 0;

    [SyncVar]
    public bool dockinggaugeon;

    [SyncVar]
    public float dockinggaugevalue = 0;
}
