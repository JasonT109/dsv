using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Meg.Networking;

public class mapData : NetworkBehaviour
{
    [SyncVar]
    public float initiateMapEvent;
    [SyncVar]
    public string mapEventName;
}