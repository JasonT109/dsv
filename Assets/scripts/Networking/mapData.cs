using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Meg.Networking;

public class mapData : NetworkBehaviour
{

    /** Default floor depth to assume. */
    public const float DefaultFloorDepth = 11050;

    /** Map modes. */
    public enum Mode
    {
        Mode3D = 0,
        Mode2D = 1
    };

    [Header("Coordinates")]

    [SyncVar]
    public float latitude = 18.553059f;

    [SyncVar]
    public float longitude = 112.244285f;

    [SyncVar]
    public float mapScale = 1f;


    [Header("Map Events")]

    [SyncVar]
    public float initiateMapEvent;

    [SyncVar]
    public string mapEventName;

    [SyncVar]
    public int acidLayer;

    [SyncVar]
    public int waterLayer = 1;

    [SyncVar]
    public Mode mapMode = Mode.Mode3D;


    public bool IsMap2D
        { get { return mapMode == Mode.Mode2D; } }

    public bool IsMap3D
        { get { return mapMode == Mode.Mode3D; } }

}