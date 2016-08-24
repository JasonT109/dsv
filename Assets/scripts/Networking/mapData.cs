using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Meg.Networking;

public class mapData : NetworkBehaviour
{

    /** Default floor depth to assume. */
    public const float DefaultFloorDepth = 11050;


    [Header("Coordinates")]

    [SyncVar]
    public float latitude = 18.553059f;

    [SyncVar]
    public float longitude = 112.244285f;

    [Header("Map Events")]

    [SyncVar]
    public float initiateMapEvent;

    [SyncVar]
    public string mapEventName;

    [SyncVar]
    public int acidLayer;

}