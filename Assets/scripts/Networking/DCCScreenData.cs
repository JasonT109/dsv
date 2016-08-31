using System;
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

    [SyncVar]
    public int DCCquadScreen0 = 0;
    [SyncVar]
    public int DCCquadScreen1 = 1;
    [SyncVar]
    public int DCCquadScreen2 = 2;
    [SyncVar]
    public int DCCquadScreen3 = 3;
    [SyncVar]
    public int DCCquadScreen4 = 4;
    [SyncVar]
    public int DCCfullscreen = 0;
    [SyncVar]
    public int DCCcommsContent = 0;
    [SyncVar]
    public int DCCquadcycle = 0;

    [SyncVar]
    public bool DCCvesselNameInTitle;
    [SyncVar]
    public bool DCCcommsUseSliders;

    /** A synchronized list of Stations. */
    public class SyncListStation : SyncListStruct<Station> { };

    /** Synchronization information for the various DCC stations. */
    public SyncListStation Stations = new SyncListStation();

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

        public Station(Station other)
        {
            Id = other.Id;
            Screen3 = other.Screen3;
            Screen4 = other.Screen4;
            Screen5 = other.Screen5;
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
        // Get the given station's current state.
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
        // Get the given station's current state.
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
        Stations.Add(new Station { Id = 0 });
    }

    /** Update a station by id. */
    private void SetStation(int id, Station station)
    {
        // Ensure enough stations exist in the synchronized list.
        while (Stations.Count <= id)
            Stations.Add(new Station { Id = Stations.Count });

        // Update station's data in the synchronized list.
        if (id >= 0 && id < Stations.Count)
            Stations[id] = station;
    }

    /** Return a station for the given id. */
    private Station GetStation(int id)
    {
        if (id >= 0 && id < Stations.Count)
            return Stations[id];

        return new Station { Id = id };
    }

}
