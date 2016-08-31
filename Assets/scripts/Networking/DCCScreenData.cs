using System;
using Meg.DCC;
using UnityEngine;
using UnityEngine.Networking;

public class DCCScreenData : NetworkBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** Undefined station id. */
    public const int Unknown = -1;

    /** Minimum station id. */
    public const int MinStationId = 0;

    /** Maximum station id. */
    public const int MaxStationId = 10;


    // Properties
    // ------------------------------------------------------------

    /** A synchronized list of Stations. */
    public class SyncListStation : SyncListStruct<Station> { };

    /** Synchronization information for the various DCC stations. */
    public SyncListStation Stations = new SyncListStation();

    [SyncVar]
    public int DCCcommsContent = 0;

    [SyncVar]
    public bool DCCvesselNameInTitle;

    [SyncVar]
    public bool DCCcommsUseSliders;


    /** 
     * Represents the screen state for a given DCC station. 
     * A DCC Station is a set of screens for a single seated user.
     */
    [Serializable]
    public struct Station
    {
        public int Id;
        public DCCWindow.contentID Screen3;
        public DCCWindow.contentID Screen4;
        public DCCWindow.contentID Screen5;

        public DCCWindow.contentID Quad0;
        public DCCWindow.contentID Quad1;
        public DCCWindow.contentID Quad2;
        public DCCWindow.contentID Quad3;
        public DCCWindow.contentID Quad4;

        public int QuadFullScreen;
        public int QuadCycle;

        public Station(int id)
        {
            Id = id;
            Screen3 = 0;
            Screen4 = 0;
            Screen5 = 0;
            Quad0 = DCCWindow.contentID.lifesupport;
            Quad1 = DCCWindow.contentID.instruments;
            Quad2 = DCCWindow.contentID.thrusters;
            Quad3 = DCCWindow.contentID.diagnostics;
            Quad4 = DCCWindow.contentID.none;
            QuadFullScreen = 0;
            QuadCycle = 0;
        }

        public Station(Station other)
        {
            Id = other.Id;
            Screen3 = other.Screen3;
            Screen4 = other.Screen4;
            Screen5 = other.Screen5;
            Quad0 = other.Quad0;
            Quad1 = other.Quad1;
            Quad2 = other.Quad2;
            Quad3 = other.Quad3;
            Quad4 = other.Quad4;
            QuadFullScreen = other.QuadFullScreen;
            QuadCycle = other.QuadCycle;
        }
    }


    // Static Properties
    // ------------------------------------------------------------

    /** The station ID that this instance belongs to. */
    public static int StationId { get { return GetStationId(); } }


    // Static Members
    // ------------------------------------------------------------

    /** The station ID that this instance belongs to. */
    private static int _stationId = Unknown;


    // Unity Methods
    // ------------------------------------------------------------

    #region UnityMethods

    /** Initialization. */
    private void Awake()
    {
        InitStationId();
    }

    /** Serverside initialization logic. */
    public override void OnStartServer()
    {
        base.OnStartServer();
        InitializeStations();
    }

    #endregion


    // Setters and Getters
    // ------------------------------------------------------------

    /** Set the station ID that this instance belongs to. */
    public static void SetStationId(int id)
        { _stationId = Mathf.Clamp(id, MinStationId, MaxStationId); }

    /** Set the station ID that this instance belongs to. */
    public static void SetStationId(string value)
    {
        int stationId;
        if (int.TryParse(value, out stationId))
            SetStationId(stationId);
    }

    /** Updates a server shared station value. */
    public void SetScreenContent(DCCScreenID._screenID id, DCCWindow.contentID value, int stationId)
    {
        var station = GetStation(stationId);
        switch (id)
        {
            case DCCScreenID._screenID.screen3:
                SetStation(stationId, new Station(station) { Screen3 = value});
                break;
            case DCCScreenID._screenID.screen4:
                SetStation(stationId, new Station(station) { Screen4 = value });
                break;
            case DCCScreenID._screenID.screen5:
                SetStation(stationId, new Station(station) { Screen5 = value });
                break;
        }
    }

    /** Returns a server shared station value. */
    public DCCWindow.contentID GetScreenContent(DCCScreenID._screenID id, int stationId)
    {
        var station = GetStation(stationId);
        switch (id)
        {
            case DCCScreenID._screenID.screen3:
                return station.Screen3;
            case DCCScreenID._screenID.screen4:
                return station.Screen4;
            case DCCScreenID._screenID.screen5:
                return station.Screen5;
            default:
                return 0;
        }
    }

    /** Set quad screen content by position */
    public void SetQuadContent(DCCScreenContentPositions.positionID id, DCCWindow.contentID value, int stationId)
    {
        var station = GetStation(stationId);
        switch (id)
        {
            case DCCScreenContentPositions.positionID.topLeft:
                SetStation(stationId, new Station(station) { Quad0 = value });
                break;
            case DCCScreenContentPositions.positionID.topRight:
                SetStation(stationId, new Station(station) { Quad1 = value });
                break;
            case DCCScreenContentPositions.positionID.bottomLeft:
                SetStation(stationId, new Station(station) { Quad2 = value });
                break;
            case DCCScreenContentPositions.positionID.bottomRight:
                SetStation(stationId, new Station(station) { Quad3 = value });
                break;
            case DCCScreenContentPositions.positionID.middle:
                SetStation(stationId, new Station(station) { Quad4 = value });
                break;
        }
    }

    /** Set quad screen content by position */
    public DCCWindow.contentID GetQuadContent(DCCScreenContentPositions.positionID id, int stationId)
    {
        var station = GetStation(stationId);
        switch (id)
        {
            case DCCScreenContentPositions.positionID.topLeft:
                return station.Quad0;
            case DCCScreenContentPositions.positionID.topRight:
                return station.Quad1;
            case DCCScreenContentPositions.positionID.bottomLeft:
                return station.Quad2;
            case DCCScreenContentPositions.positionID.bottomRight:
                return station.Quad3;
            case DCCScreenContentPositions.positionID.middle:
                return station.Quad4;
            default:
                return 0;
        }
    }

    /** Set quad screen content by position */
    public void SetQuadCycle(int value, int stationId)
    {
        var station = GetStation(stationId);
        SetStation(stationId, new Station(station) { QuadCycle = value });
    }

    /** Return quad cycle setting */
    public int GetQuadCycle(int stationId)
        { return GetStation(stationId).QuadCycle; }

    /** Set quad screen content by position */
    public void SetQuadFullScreen(int value, int stationId)
    {
        var station = GetStation(stationId);
        SetStation(stationId, new Station(station) { QuadFullScreen = value });
    }

    /** Return quad cycle setting */
    public int GetQuadFullScreen(int stationId)
        { return GetStation(stationId).QuadFullScreen; }

    /** Return a quad position for the given string. */
    public static DCCScreenContentPositions.positionID GetPositionForName(string position)
    {
        switch (position)
        {
            case "DCCquadScreen0":
                return DCCScreenContentPositions.positionID.topLeft;
            case "DCCquadScreen1":
                return DCCScreenContentPositions.positionID.topRight;
            case "DCCquadScreen2":
                return DCCScreenContentPositions.positionID.bottomLeft;
            case "DCCquadScreen3":
                return DCCScreenContentPositions.positionID.bottomRight;
            case "DCCquadScreen4":
                return DCCScreenContentPositions.positionID.middle;
            case "hidden":
                return DCCScreenContentPositions.positionID.hidden;
            default:
                return DCCScreenContentPositions.positionID.hidden;
        }
    }

    /** Return a position name for the given id. */
    public static string GetNameForPosition(DCCScreenContentPositions.positionID id)
    {
        switch (id)
        {
            case DCCScreenContentPositions.positionID.topLeft:
                return "DCCquadScreen0";
            case DCCScreenContentPositions.positionID.topRight:
                return "DCCquadScreen1";
            case DCCScreenContentPositions.positionID.bottomLeft:
                return "DCCquadScreen2";
            case DCCScreenContentPositions.positionID.bottomRight:
                return "DCCquadScreen3";
            case DCCScreenContentPositions.positionID.middle:
                return "DCCquadScreen4";
            case DCCScreenContentPositions.positionID.hidden:
                return "hidden";
            default:
                return "hidden";
        }
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Initialize the station id. */
    private static void InitStationId()
    {
        if (_stationId < 0)
            SetStationId(Configuration.Get("dcc-station-id", 0));
    }

    /** Return the current station id. */
    private static int GetStationId()
    {
        // Initialize default station id on demand.
        if (_stationId < 0)
            InitStationId();

        return _stationId;
    }

    /** Initialize the station state list. */
    [Server]
    private void InitializeStations()
    {
        // Remove any existing stations.
        Stations.Clear();

        // Add a default station.
        Stations.Add(new Station(0));
    }

    /** Update a station by id. */
    private void SetStation(int id, Station station)
    {
        // Ensure enough stations exist in the synchronized list.
        while (Stations.Count <= id)
            Stations.Add(new Station(id));

        // Update station's data in the synchronized list.
        if (id >= 0 && id < Stations.Count)
            Stations[id] = station;
    }

    /** Return a station for the given id. */
    private Station GetStation(int id)
    {
        if (id >= 0 && id < Stations.Count)
            return Stations[id];

        return new Station(id);
    }

}
