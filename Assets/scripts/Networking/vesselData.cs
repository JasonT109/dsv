using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using Meg.Networking;

public class vesselData : NetworkBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** The number of predefined vessels (can't be removed.) */
    public const int BaseVesselCount = 6;

    /** Id of the 'meg' vessel. */
    public const int MegId = 5;

    /** Id of the 'intercept pin' vessel. */
    public const int InterceptId = 6;

    /** Default name for an unknown vessel. */
    public string Unknown = "Unknown";

    /** Scaling factor for converting sub world space into vessel XY space. */
    public const float WorldToVesselScale = 0.001f;

    /** Scaling factor for converting vessel XY space into sub world space. */
    public const float VesselToWorldScale = 1000f;


    // Enumerations
    // ------------------------------------------------------------

    /** Types of vessel icon. */
    public enum Icon
    {
        Normal = 0,
        Warning = 1
    }


    // Structures
    // ------------------------------------------------------------

    /** Structure for tracking a vessel's current state. */
    [System.Serializable]
    public struct Vessel
    {
        public int Id;
        public string Name;
        public Vector3 Position;
        public float Speed;
        public bool OnMap;
        public bool OnSonar;
        public Icon Icon;

        public float Depth
            { get { return Position.z; } }

        public Vessel(Vessel vessel)
        {
            Id = vessel.Id;
            Name = vessel.Name;
            Position = vessel.Position;
            Speed = vessel.Speed;
            OnMap = vessel.OnMap;
            OnSonar = vessel.OnSonar;
            Icon = vessel.Icon;
        }
    };

    /** Class definition for a synchronized list of vessel states. */
    public class SyncListVessels : SyncListStruct<Vessel> { };



    // Synchronization
    // ------------------------------------------------------------

    #region Synchronization

    [Header("Synchronization")]

    /** Id of the vessel currently being simulated. */
    [SyncVar]
    public int PlayerVessel = 1;

    /** Synchronized list for holding vessel state. */
    public SyncListVessels Vessels = new SyncListVessels();

    #endregion



    // Properties
    // ------------------------------------------------------------
    
    #region PublicProperties

    /** Return the player vessel's name. */
    public string PlayerVesselName
    {
        get { return GetName(PlayerVessel); }
        set { SetName(PlayerVessel, value); }
    }

    /** Return the number of known vessels. */
    public int VesselCount
        { get { return Vessels.Count; } }

    /** Regular expression used to match server data keys. */
    public readonly Regex DataKeyPattern = new Regex(
        @"^(v|vessel|meg|intercept)(\d+)(\w+)$", RegexOptions.IgnoreCase);

    #endregion




    // Unity Methods
    // ------------------------------------------------------------

    #region UnityMethods

    /** Serverside initialization logic. */
    public override void OnStartServer()
    {
        base.OnStartServer();
        InitializeVessels();
    }

    /** Physics update. */
    void FixedUpdate()
    {
        // Force player vessel's data to match the sub's world position. 
        if (PlayerVessel > 0)
            SetPosition(PlayerVessel, WorldToVesselSpace(SubPosition));
    }

    #endregion


    // Public Methods
    // ------------------------------------------------------------

    /** Add a vessel. */
    [Server]
    public void AddVessel(Vessel vessel)
    {
        // Add vessel to the synchronized list.
        Vessels.Add(vessel);
        var id = vessel.Id;

        // Register vessel dynamic server parameters.
        serverUtils.RegisterServerValue(string.Format("vessel{0}visible", id), 
            new serverUtils.ParameterInfo { maxValue = 1, type = serverUtils.ParameterType.Bool });
        serverUtils.RegisterServerValue(string.Format("vessel{0}onmap", id), 
            new serverUtils.ParameterInfo { maxValue = 1, type = serverUtils.ParameterType.Bool });
        serverUtils.RegisterServerValue(string.Format("vessel{0}onsonar", id), 
            new serverUtils.ParameterInfo { maxValue = 1, type = serverUtils.ParameterType.Bool });
        serverUtils.RegisterServerValue(string.Format("vessel{0}posx", id),
            new serverUtils.ParameterInfo { minValue = -5000, maxValue = 5000 });
        serverUtils.RegisterServerValue(string.Format("vessel{0}posy", id),
            new serverUtils.ParameterInfo { minValue = -5000, maxValue = 5000 });
        serverUtils.RegisterServerValue(string.Format("vessel{0}posz", id),
            new serverUtils.ParameterInfo { minValue = -5000, maxValue = 5000 });
        serverUtils.RegisterServerValue(string.Format("vessel{0}depth", id),
            new serverUtils.ParameterInfo { minValue = -5000, maxValue = 5000 });
        serverUtils.RegisterServerValue(string.Format("vessel{0}speed", id),
            serverUtils.DefaultParameterInfo);
        serverUtils.RegisterServerValue(string.Format("vessel{0}velocity", id),
            serverUtils.DefaultParameterInfo);
        serverUtils.RegisterServerValue(string.Format("vessel{0}icon", id),
            new serverUtils.ParameterInfo { maxValue = (int) Icon.Warning, type = serverUtils.ParameterType.Int });
    }

    /** Update a vessel by id. */
    public void SetVessel(int id, Vessel vessel)
    {
        if (id >= 1 && id <= Vessels.Count)
            Vessels[id - 1] = vessel;
    }

    /** Return a vessel for the given id. */
    public Vessel GetVessel(int id)
    {
        if (id >= 1 && id <= Vessels.Count)
            return Vessels[id - 1];

        return new Vessel { Id = id, Name = Unknown };
    }


    // Setters and Getters
    // ------------------------------------------------------------

    /** Whether the given server data key relates to vessel state. */
    public bool IsVesselKey(string valueName)
        { return DataKeyPattern.IsMatch(valueName); }

    /** Updates a server shared value. */
    public void SetServerData(string valueName, float value, bool add = false)
    {
        // Parse value into vessel id and parameter name.
        var id = 0; var parameter = "";
        if (!TryParseKey(valueName, out id, out parameter))
            return;

        // Apply the appropriate change.
        switch (parameter)
        {
            case "vis":
            case "visible":
                SetVisible(id, value > 0);
                break;
            case "onmap":
                SetOnMap(id, value > 0);
                break;
            case "onsonar":
                SetOnSonar(id, value > 0);
                break;
            case "posx":
                var px = GetPosition(id);
                SetPosition(id, new Vector3(value, px.y, px.z));
                break;
            case "posy":
                var py = GetPosition(id);
                SetPosition(id, new Vector3(py.x, value, py.z));
                break;
            case "posz":
            case "depth":
                var pz = GetPosition(id);
                SetPosition(id, new Vector3(pz.x, pz.y, value));
                break;
            case "speed":
            case "velocity":
                SetSpeed(id, value);
                break;
            case "icon":
                SetIcon(id, (Icon) Mathf.RoundToInt(value));
                break;
        }
    }

    /** Returns a server shared value. */
    public float GetServerData(string valueName, float defaultValue)
    {
        // Parse value into vessel id and parameter name.
        var id = 0; var parameter = "";
        if (!TryParseKey(valueName, out id, out parameter))
            return defaultValue;

        // Apply the appropriate change.
        switch (parameter)
        {
            case "vis":
            case "visible":
            case "onmap":
                return GetVessel(id).OnMap ? 1 : 0;
            case "onsonar":
                return GetVessel(id).OnSonar ? 1 : 0;
            case "posx":
                return GetVessel(id).Position.x;
            case "posy":
                return GetVessel(id).Position.y;
            case "posz":
            case "depth":
                return GetVessel(id).Position.z;
            case "speed":
            case "velocity":
                return GetVessel(id).Speed;
            case "icon":
                return (int) GetIcon(id);
            default:
                return defaultValue;
        }
    }

    /** Set a vessel's current position (1-based index). */
    public void SetPosition(int id, Vector3 position)
    {
        SetVessel(id, new Vessel(GetVessel(id)) { Position = position });

        // Update sub's transform if affecting the player vessel.
        if (id == PlayerVessel)
            SubPosition = VesselToWorldSpace(position);
    }

    /** Return a vessel's position. */
    public Vector3 GetPosition(int id)
        { return GetVessel(id).Position;  }

    /** Set a vessel's depth. */
    public void SetDepth(int id, float depth)
    {
        var p = GetPosition(id);
        SetVessel(id, new Vessel(GetVessel(id)) { Position = new Vector3(p.x, p.y, depth)});
    }

    /** Return a vessel's depth. */
    public float GetDepth(int id)
        { return GetVessel(id).Position.z; }

    /** Set a vessel's current position and nominal speed (1-based index). */
    public void SetState(int id, Vector3 position, float speed)
    {
        SetVessel(id, new Vessel(GetVessel(id)) { Position = position, Speed = speed });

        // Update sub's transform if affecting the player vessel.
        if (id == PlayerVessel)
            SubPosition = VesselToWorldSpace(position);
    }

    /** Set a vessel's name (1-based index). */
    public void SetName(int id, string value)
        { SetVessel(id, new Vessel(GetVessel(id)) { Name = value }); }

    /** Return a vessel's current name. */
    public string GetName(int id)
        { return GetVessel(id).Name; }

    /** Set a vessel's visibility (1-based index). */
    public void SetVisible(int id, bool visible)
        { SetVessel(id, new Vessel(GetVessel(id)) { OnMap = visible, OnSonar = visible }); }

    /** Set a vessel's map visibility (1-based index). */
    public void SetOnMap(int id, bool visible)
        { SetVessel(id, new Vessel(GetVessel(id)) { OnMap = visible }); }

    /** Set a vessel's sonar visibility (1-based index). */
    public void SetOnSonar(int id, bool visible)
        { SetVessel(id, new Vessel(GetVessel(id)) { OnSonar = visible }); }

    /** Return a vessel's current map visibility. */
    public bool IsOnMap(int id)
        { return GetVessel(id).OnMap; }

    /** Return a vessel's current sonar visibility. */
    public bool IsOnSonar(int id)
        { return GetVessel(id).OnSonar; }

    /** Set a vessel's icon (1-based index). */
    public void SetIcon(int id, Icon icon)
        { SetVessel(id, new Vessel(GetVessel(id)) { Icon = icon }); }

    /** Return a vessel's current warning status. */
    public Icon GetIcon(int id)
        { return GetVessel(id).Icon; }

    /** Set a vessel's speed (1-based index). */
    public void SetSpeed(int id, float speed)
        { SetVessel(id, new Vessel(GetVessel(id)) { Speed = speed }); }

    /** Return a vessel's current speed. */
    public float GetSpeed(int id)
        { return GetVessel(id).Speed; }
    
    /** Return a vessel's current state as an [x,y,z,speed] tuple (1-based index). */
    public float[] GetData(int id)
    {
        // Check if a valid vessel id has been supplied.
        var index = id - 1;
        if (index < 0 || index >= Vessels.Count)
            return new float[4];

        var state = Vessels[index];
        var result = new float[4];
        result[0] = state.Position.x;
        result[1] = state.Position.y;
        result[2] = state.Position.z;
        result[3] = state.Speed;

        return result;
    }
    

    // Coordinates and Spaces
    // ------------------------------------------------------------

    /** Return a vessel's position in sub space. */
    public Vector3 GetWorldPosition(int id)
        { return VesselToWorldSpace(GetPosition(id)); }

    /** Return a vessel's position in long-range sonar space. */
    public Vector2 GetSonarPosition(int id, SonarData.Type type)
        { return VesselToSonarSpace(GetPosition(id), type); }

    /** Convert a point in vessel space to sub space. */
    public Vector3 VesselToWorldSpace(Vector3 p)
        { return new Vector3(p.x * VesselToWorldScale, -p.z, p.y * VesselToWorldScale); }

    /** Convert a point in sub space to vessel space. */
    public Vector3 WorldToVesselSpace(Vector3 world)
        { return new Vector3(world.x * WorldToVesselScale, world.z * WorldToVesselScale, -world.y); }

    /** Convert a point in vessel space to the player vessel's local space. */
    public Vector3 VesselToPlayerSpace(Vector3 p)
        { return p - GetPosition(PlayerVessel); }

    /** Convert a point in vessel space to the player vessel's local space. */
    public Vector3 PlayerToVesselSpace(Vector3 p)
        { return p + GetPosition(PlayerVessel); }

    /** Convert a point in vessel space to sonar space (normalized to 0..1 according to maximum range). */
    public Vector2 VesselToSonarSpace(Vector3 p, SonarData.Type type)
    {
        // Check that sonar exists.
        if (!serverUtils.SonarData)
            return Vector3.zero;

        // Convert sonar maximum range from metres into vessel space.
        var range = serverUtils.SonarData.GetConfigForType(type).MaxRange * WorldToVesselScale;
        if (range <= 0)
            return Vector3.zero;

        // Compute normalized position, relative to player.
        return VesselToPlayerSpace(p) / range;
    }

    /** Convert a point in sonar space to vessel space. */
    public Vector3 SonarToVesselSpace(Vector3 sonar, SonarData.Type type)
    {
        // Check that sonar exists.
        if (!serverUtils.SonarData)
            return GetPosition(PlayerVessel);

        // Get sonar maximum range in metres.
        var range = serverUtils.SonarData.GetConfigForType(type).MaxRange;
        if (range <= 0)
            return GetPosition(PlayerVessel);

        // Convert from normalized sonar space to vessel space.
        return PlayerToVesselSpace(sonar * range);
    }

    /** Set the player sub's world velocity. */
    public void SetPlayerWorldVelocity(Vector3 velocity)
        { serverUtils.SubControl.SetWorldVelocity(velocity); }

    /** Return the distance between two vessels in meters. */
    public float GetDistance(int from, int to)
    {
        var a = GetPosition(from); a.x *= 1000; a.y *= 1000;
        var b = GetPosition(to); b.x *= 1000; b.y *= 1000;
        return Vector3.Distance(a, b);
    }

    /** Return a vessel's current position as a latitude/longitude pair (1-based index). */
    public Vector2 GetLatLong(int vessel)
    {
        var position = GetPosition(vessel);
        var dx = position.x * 1000;
        var dy = position.y * 1000;

        double latitude = serverUtils.GetServerData("latitude");
        double longitude = serverUtils.GetServerData("longitude");

        latitude = latitude + (dy / Conversions.EarthRadius) * (180 / Math.PI);
        longitude = longitude + (dx / Conversions.EarthRadius) * (180 / Math.PI) / Math.Cos(latitude * Math.PI / 180);

        return new Vector2((float)longitude, (float)latitude);
    }


    // Private Properties
    // ------------------------------------------------------------

    /** Set the sub's worldspace position. */
    private Vector3 SubPosition
    {
        get { return serverUtils.ServerObject.transform.position; }
        set { serverUtils.ServerObject.transform.position = value; }
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Initialize the vessel state list. */
    [Server]
    private void InitializeVessels()
    {
        // Populate predefined vessels that always exist in the simulation.
        AddVessel(new Vessel() { Id = 1, Name = GetNameFromConfig(1), Position = new Vector3(-1f, -0.5f, 2000f), OnMap = true, OnSonar = true });
        AddVessel(new Vessel() { Id = 2, Name = GetNameFromConfig(2), Position = new Vector3(-2f, 2f, 8900f), OnMap = true, OnSonar = true });
        AddVessel(new Vessel() { Id = 3, Name = GetNameFromConfig(3), Position = new Vector3(2f, -2f, 7300f), OnMap = true, OnSonar = true });
        AddVessel(new Vessel() { Id = 4, Name = GetNameFromConfig(4), Position = new Vector3(0f, 0f, 7700f), OnMap = true, OnSonar = true });
        AddVessel(new Vessel() { Id = 5, Name = GetNameFromConfig(5), Position = new Vector3(0f, -2.5f, 8200f), OnMap = true, OnSonar = true, Icon = Icon.Warning });
        AddVessel(new Vessel() { Id = 6, Name = GetNameFromConfig(6), Position = new Vector3(2f, 2f, 8200f), OnMap = true, OnSonar = true });
    }

    /** Get a vessel's name from configuration. */
    private string GetNameFromConfig(int vessel)
    {
        // Retrieve vessel name from configuration.
        var vesselNames = Configuration.GetJson("vessel-names");
        if (vesselNames && vesselNames.IsArray && vessel <= vesselNames.Count)
            return vesselNames[vessel - 1].str;

        return Unknown;
    }

    /** Parse a server data value key into vessel id and parameter key. */
    private bool TryParseKey(string valueName, out int id, out string parameter)
    {
        // Check if a value name was supplied.
        id = 0; parameter = "";
        if (string.IsNullOrEmpty(valueName))
            return false;

        // Pattern match on incoming key to determine what to change.
        var key = valueName.ToLower();
        var match = DataKeyPattern.Match(key);
        if (!match.Success)
            return false;

        // Parse the match into vessel id and state key.
        var type = match.Groups[1].Value;
        id = int.Parse(match.Groups[2].Value);
        parameter = match.Groups[3].Value;

        // Handle special cases for the Meg and Intercept vessels.
        if (type == "meg")
            id = MegId;
        else if (type == "intercept")
            id = InterceptId;

        // Successfully parsed key into components.
        return true;
    }

}
