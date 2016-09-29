using System;
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
        None = -1,
        Mode3D = 0,
        Mode2D = 1,
        ModeSubSchematic = 2   // Used in the DCC strategy table.
    };


    [Header("Mode")]

    [SyncVar]
    public Mode mapMode = Mode.Mode3D;

    [SyncVar]
    public bool mapInteractive = true;

    [SyncVar]
    public bool mapTopDown = true;

    [SyncVar]
    public bool mapUseOldIndicators = false;


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
    public int acidLayerCount = 4;

    [SyncVar]
    public float acidLayerOpacity = 0.5f;

    [SyncVar]
    public float acidLayerFadeTime = 0.25f;

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


    [Header("Lines")]

    /** Synchronized list for holding map lines. */
    public SyncListLines Lines = new SyncListLines();

    /** Synchronized list for holding line progress percentages. */
    public SyncListFloat LinePercentages = new SyncListFloat();


    public bool IsMap2D
        { get { return mapMode == Mode.Mode2D; } }

    public bool IsMap3D
        { get { return mapMode == Mode.Mode3D; } }

    public bool IsSubSchematic
        { get { return mapMode == Mode.ModeSubSchematic; } }


    // Enumerations
    // ------------------------------------------------------------

    /** Possible line styles. */
    public enum LineStyle
    {
        Normal
    }


    // Structures
    // ------------------------------------------------------------

    /** Structure representing a line on the map. */
    [Serializable]
    public struct Line
    {
        public int Id;
        public LineStyle Style;
        public Color Color;
        public Vector3[] Points;
        public float Progress;
    }

    /** Class definition for a synchronized list of crew states. */
    public class SyncListLines : SyncListStruct<Line> { };


    // Unity Methods
    // ------------------------------------------------------------

    #region UnityMethods

    /** Serverside initialization logic. */
    public override void OnStartServer()
    {
        base.OnStartServer();

        // Default to top-down map in gliders / evac ship.
        if (serverUtils.IsGlider())
            mapTopDown = true;

        /*
        var line = new Line
        {
            Id = 0,
            Color = Color.cyan,
            Points = new[] {new Vector3(0, 0, 0), new Vector3(100, 0, 0), new Vector3(100, 100, 0), new Vector3(0, 100, 0), new Vector3(0, 0, 0), },
        };

        Lines.Add(line);
        LinePercentages.Add(0f);
        */
    }

    /*
    private void Update()
    {
        if (!isServer || LinePercentages.Count <= 0)
            return;

        LinePercentages[0] = Mathf.Repeat(Time.time * 25f, 100f);
    }
    */

    #endregion


}