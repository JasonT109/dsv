using System;
using System.Collections.Generic;
using System.Linq;
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

    /** Default vessel color on map. */
    public static readonly Color DefaultColorOnMap = new Color(0.196f, 0.643f, 0.878f, 1);

    /** Default vessel color on sonar. */
    public static readonly Color DefaultColorOnSonar = new Color(0.196f, 0.643f, 0.878f, 1);



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
    [Serializable]
    public struct Vessel
    {
        public int Id;
        public string Name;
        public Vector3 Position;
        public float Speed;
        public bool OnMap;
        public bool OnSonar;
        public Icon Icon;
        public Color ColorOnSonar;
        public Color ColorOnMap;

        public float Depth
            { get { return Position.z; } }

        public vesselMovement Movement
            { get { return serverUtils.VesselMovements.GetVesselMovement(Id); } }

        public Vessel(Vessel vessel)
        {
            Id = vessel.Id;
            Name = vessel.Name;
            Position = vessel.Position;
            Speed = vessel.Speed;
            OnMap = vessel.OnMap;
            OnSonar = vessel.OnSonar;
            Icon = vessel.Icon;
            ColorOnSonar = vessel.ColorOnSonar;
            ColorOnMap = vessel.ColorOnMap;
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

    /** Return the last vessels's id. */
    public int LastVessel
        { get { return Vessels.Count; } }

    /** Regular expression used to match server data keys. */
    public readonly Regex DataKeyPattern = new Regex(
        @"^(v|vessel|meg|intercept)(\d+)(\w+)$", RegexOptions.IgnoreCase);

    #endregion

    
    // Private Properties
    // ------------------------------------------------------------

    /** The player vessel's worldspace position. */
    private static Vector3 PlayerPosition
    {
        get { return serverUtils.ServerObject.transform.position; }
        set { serverUtils.ServerObject.transform.position = value; }
    }

    /** The player vessel's worldspace position. */
    private Vector3 PlayerWorldVelocity
    {
        get { return GetPlayerWorldVelocity(); }
        set { SetPlayerWorldVelocity(value); }
    }

    
    // Members
    // ------------------------------------------------------------

    /** Initial state of each vessel. */
    private readonly List<Vessel> _initialVesselStates = new List<Vessel>();

    /** Initial player world position. */
    private Vector3 _initialPlayerPosition;



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
            SetPosition(PlayerVessel, WorldToVesselSpace(PlayerPosition));
    }

    #endregion


    // Public Methods
    // ------------------------------------------------------------

    /** Add a vessel. */
    [Server]
    public void AddVessel(Vessel vessel)
    {
        // Assign vessel an id based on the highest existing id in list.
        var id = Vessels.Count + 1;
        vessel.Id = id;

        // Add vessel to the synchronized list.
        Vessels.Add(vessel);

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

    /** Remove the last vessel (if possible). */
    [Server]
    public void RemoveLastVessel()
    {
        if (Vessels.Count > BaseVesselCount)
            Vessels.RemoveAt(Vessels.Count - 1);
    }

    /** Clear extra vessels (leaving just the base vessel set). */
    [Server]
    public void ClearExtraVessels()
    {
        while (Vessels.Count > BaseVesselCount)
            RemoveLastVessel();
    }

    /** Returns whether a vessel can be removed. */
    public bool CanRemove(Vessel vessel)
        { return CanRemove(vessel.Id); }

    /** Returns whether a vessel can be removed. */
    public bool CanRemove(int id)
        { return id > BaseVesselCount; }



    // Setters and Getters
    // ------------------------------------------------------------

    /** Return a vessel for the given id. */
    public Vessel GetVessel(int id)
    {
        if (id >= 1 && id <= Vessels.Count)
            return Vessels[id - 1];

        return new Vessel { Id = id, Name = Unknown };
    }

    /** Whether the given server data key relates to vessel state. */
    public bool IsVesselKey(string valueName)
        { return DataKeyPattern.IsMatch(valueName); }

    /** Updates a server shared value. */
    public void SetServerData(string valueName, float value, bool add = false)
    {
        // Parse value into vessel id and parameter name.
        int id; string parameter;
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
        int id; string parameter;
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

        // If affecting the player vessel, update worldspace position.
        if (id == PlayerVessel)
            PlayerPosition = VesselToWorldSpace(position);
    }

    /** Return a vessel's position. */
    public Vector3 GetPosition(int id)
        { return GetVessel(id).Position;  }

    /** Set a vessel's depth. */
    public void SetDepth(int id, float depth)
    {
        var p = GetPosition(id);
        SetPosition(id, new Vector3(p.x, p.y, depth));
    }

    /** Return a vessel's depth. */
    public float GetDepth(int id)
        { return GetVessel(id).Position.z; }

    /** Set a vessel's current position and nominal speed (1-based index). */
    public void SetState(int id, Vector3 position, float speed)
    {
        SetPosition(id, position);
        SetSpeed(id, speed);
    }

    /** Set a vessel's name (1-based index). */
    public void SetName(int id, string value)
        { SetVessel(id, new Vessel(GetVessel(id)) { Name = value }); }

    /** Return a vessel's current name. */
    public string GetName(int id)
        { return GetVessel(id).Name; }

    /** Return a debug name for the given vessel. */
    public string GetDebugName(int id)
    {
        var name = string.Format("{0}: {1}", id, GetName(id).ToUpper());
        if (id == MegId)
            name += " (MEG)";
        else if (id == InterceptId)
            name += " (INT)";

        return name;
    }

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

    /** Set a vessel's sonar color (1-based index). */
    public void SetColorOnSonar(int id, Color color)
        { SetVessel(id, new Vessel(GetVessel(id)) { ColorOnSonar = color }); }

    /** Return a vessel's current sonar color. */
    public Color GetColorOnSonar(int id)
        { return GetVessel(id).ColorOnSonar; }

    /** Set a vessel's map color (1-based index). */
    public void SetColorOnMap(int id, Color color)
        { SetVessel(id, new Vessel(GetVessel(id)) { ColorOnMap = color }); }

    /** Return a vessel's current map color. */
    public Color GetColorOnMap(int id)
        { return GetVessel(id).ColorOnMap; }

    /** Set a vessel's speed (1-based index). */
    public void SetSpeed(int id, float speed)
        { SetVessel(id, new Vessel(GetVessel(id)) { Speed = speed }); }

    /** Return a vessel's current speed. */
    public float GetSpeed(int id)
        { return GetVessel(id).Speed; }
    
    

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
        var range = serverUtils.SonarData.GetConfigForType(type).MaxRange * WorldToVesselScale;
        if (range <= 0)
            return GetPosition(PlayerVessel);

        // Convert from normalized sonar space to vessel space.
        return PlayerToVesselSpace(sonar * range);
    }

    /** Set the player sub's world velocity. */
    public void SetPlayerWorldVelocity(Vector3 velocity)
        { serverUtils.SubControl.SetWorldVelocity(velocity); }

    /** Set the player sub's world velocity. */
    public Vector3 GetPlayerWorldVelocity()
        { return serverUtils.SubControl.GetWorldVelocity(); }

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

    /** Capture the initial state of all vessels. */
    public void CaptureInitialState()
    {
        _initialPlayerPosition = PlayerPosition;
        _initialVesselStates.Clear();
        foreach (Vessel vessel in Vessels)
            _initialVesselStates.Add(vessel);
    }

    /** Reset vessel states to initial values. */
    public void ResetToInitialState()
    {
        // Reset vessels to the recorded state.
        foreach (var vessel in _initialVesselStates)
            SetVessel(vessel.Id, vessel);

        // Reset player's world position and velocity.
        PlayerPosition = _initialPlayerPosition;
        PlayerWorldVelocity = Vector3.zero;
    }



    // Load / Save
    // ------------------------------------------------------------

    /** Save state to JSON. */
    public JSONObject Save()
    {
        var json = new JSONObject();
        json.AddField("PlayerVessel", PlayerVessel);

        var vesselsJson = new JSONObject(JSONObject.Type.ARRAY);
        foreach (var v in Vessels)
            vesselsJson.Add(SaveVessel(v));

        json.AddField("Vessels", vesselsJson);
        return json;
    }

    /** Load state from JSON. */
    [Server]
    public void Load(JSONObject json)
    {
        var vesselsJson = json.GetField("Vessels");
        Vessels.Clear();
        for (var i = 0; i < vesselsJson.Count; i++)
            AddVessel(LoadVessel(vesselsJson[i]));

        // Reset player's world position and velocity.
        PlayerVessel = (int) json.GetField("PlayerVessel").i;
        PlayerPosition = VesselToWorldSpace(GetPosition(PlayerVessel));
        PlayerWorldVelocity = Vector3.zero;
    }

    /** Save a vessel state to JSON. */
    private JSONObject SaveVessel(Vessel vessel)
    {
        var json = new JSONObject();
        json.AddField("Id", vessel.Id);
        json.AddField("Name", vessel.Name);
        json.AddField("Position", vessel.Position);
        json.AddField("Speed", vessel.Speed);
        json.AddField("OnMap", vessel.OnMap);
        json.AddField("OnSonar", vessel.OnSonar);
        json.AddField("Icon", (int) vessel.Icon);

        return json;
    }

    /** Load vessel state from JSON. */
    private Vessel LoadVessel(JSONObject json)
    {
        var vessel = new Vessel();
        json.GetField(ref vessel.Id, "Id");
        json.GetField(ref vessel.Name, "Name");
        json.GetField(ref vessel.Position, "Position");
        json.GetField(ref vessel.Speed, "Speed");
        json.GetField(ref vessel.OnMap, "OnMap");
        json.GetField(ref vessel.OnSonar, "OnSonar");

        int icon = 0;
        json.GetField(ref icon, "Icon");
        vessel.Icon = (Icon) icon;

        return vessel;
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Update a vessel by id. */
    private void SetVessel(int id, Vessel vessel)
    {
        // Ensure enough vessels exist in the synchronized list.
        // while (Vessels.Count < id)
        //    Vessels.Add(new Vessel());

        // Update vessel's data in the synchronized list.
        if (id >= 1 && id <= Vessels.Count)
            Vessels[id - 1] = vessel;
    }

    /** Initialize the vessel state list. */
    [Server]
    private void InitializeVessels()
    {
        // Populate predefined vessels from configuration.
        var vessels = Configuration.GetJson("vessels");
        if (vessels.IsArray)
            for (var i = 0; i < vessels.Count; i++)
                AddVesselFromConfig(vessels[i]);
    }

    /** Get a vessel's name from configuration. */
    [Server]
    private void AddVesselFromConfig(JSONObject json)
    {
        var vessel = new Vessel
        {
            Name = Unknown,
            ColorOnMap = DefaultColorOnMap,
            ColorOnSonar = DefaultColorOnSonar
        };

        json.GetField(ref vessel.Name, "name");
        json.GetField(ref vessel.ColorOnMap, "coloronmap");
        json.GetField(ref vessel.ColorOnSonar, "coloronsonar");
        json.GetField(ref vessel.Position, "position");
        json.GetField(ref vessel.OnMap, "onmap");
        json.GetField(ref vessel.OnSonar, "onsonar");

        var iconName = "normal";
        try
        {
            json.GetField(ref iconName, "icon");
            vessel.Icon = (Icon) Enum.Parse(typeof(Icon), iconName, true);
        }
        catch (Exception)
            { Debug.LogWarning("Unrecognized vessel icon in config: '" + iconName + "'."); }

        AddVessel(vessel);
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
