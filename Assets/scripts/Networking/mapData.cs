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
        Mode2D = 1,
        ModeSubSchematic = 2   // Used in the DCC strategy table.
    };


    [Header("Mode")]

    [SyncVar]
    public Mode mapMode = Mode.Mode3D;


    [Header("Coordinates")]

    [SyncVar]
    public float latitude = 18.553059f;

    [SyncVar]
    public float longitude = 112.244285f;

    [SyncVar]
    public float mapScale = 1f;


    [Header("Layers")]

    [SyncVar]
    public int acidLayer;

    [SyncVar]
    public int waterLayer = 1;

    [SyncVar]
    public int mapLayerAlerts = 1;

    [SyncVar]
    public int mapLayerContours = 1;

    [SyncVar]
    public int mapLayerGrid = 1;

    [SyncVar]
    public int mapLayerDepths = 1;

    [SyncVar]
    public int mapLayerLabels = 1;

    [SyncVar]
    public int mapLayerSatellite = 1;

    [SyncVar]
    public int mapLayerShipping = 1;

    [SyncVar]
    public int mapLayerTemperatures = 1;


    [Header("Events")]

    [SyncVar]
    public float initiateMapEvent;

    [SyncVar]
    public string mapEventName;


    public bool IsMap2D
        { get { return mapMode == Mode.Mode2D; } }

    public bool IsMap3D
        { get { return mapMode == Mode.Mode3D; } }

    public bool IsSubSchematic
        { get { return mapMode == Mode.ModeSubSchematic; } }

}