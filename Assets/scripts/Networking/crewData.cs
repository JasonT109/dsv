using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using Meg.Networking;
using Meg.Graphics;

public class crewData : NetworkBehaviour
{
    
    // Structures
    // ------------------------------------------------------------

    /** Structure for tracking a crew's current state. */
    [Serializable]
    public struct Crew
    {
        public int Id;
        public string Name;
        public float HeartRate;
        public float BodyTemp;
        public float HeartStrengthMin;
        public float HeartStrengthMax;
        public float HeartPattern;
        public float RespirationRate;
        public float ETCO2Min;
        public float ETCO2Max;
        public float ETCO2Pattern;
        public float SPO2Min;
        public float SPO2Max;
        public float SPO2Pattern;
        public float ABPMin;
        public float ABPMax;
        public float PAPMin;
        public float PAPMax;
        public bool MonitorLeds;
        public bool MonitorGraphs;
        public bool Photo;
        public bool Offline;

        public Crew(Crew other)
        {
            Id = other.Id;
            Name = other.Name;
            HeartRate = other.HeartRate;
            BodyTemp = other.BodyTemp;
            HeartStrengthMin = other.HeartStrengthMin;
            HeartStrengthMax = other.HeartStrengthMax;
            HeartPattern = other.HeartPattern;
            RespirationRate = other.RespirationRate;
            ETCO2Min = other.ETCO2Min;
            ETCO2Max = other.ETCO2Max;
            ETCO2Pattern = other.ETCO2Pattern;
            SPO2Min = other.SPO2Min;
            SPO2Max = other.SPO2Max;
            SPO2Pattern = other.SPO2Pattern;
            ABPMin = other.ABPMin;
            ABPMax = other.ABPMax;
            PAPMin = other.PAPMin;
            PAPMax = other.PAPMax;
            MonitorLeds = other.MonitorLeds;
            MonitorGraphs = other.MonitorGraphs;
            Photo = other.Photo;
            Offline = other.Offline;
        }

        public Crew(int id)
        {
            Id = id;
            Name = "Unknown";
            HeartRate = 86f;
            BodyTemp = 36.5f;
            HeartStrengthMin = 0f;
            HeartStrengthMax = 100f;
            HeartPattern = 0f;
            RespirationRate = 20f;
            ETCO2Min = 0f;
            ETCO2Max = 38f;
            ETCO2Pattern = 0f;
            SPO2Min = 0f;
            SPO2Max = 98f;
            SPO2Pattern = 0f;
            ABPMin = 82f;
            ABPMax = 125f;
            PAPMin = 10f;
            PAPMax = 26f;
            MonitorLeds = true;
            MonitorGraphs = true;
            Photo = true;
            Offline = false;
        }
    };

    /** Class definition for a synchronized list of crew states. */
    public class SyncListCrews : SyncListStruct<Crew> { };



    // Synchronization
    // ------------------------------------------------------------

    #region Synchronization

    [Header("Synchronization")]

    /** Synchronized list for holding crew state. */
    public SyncListCrews Crews = new SyncListCrews();

    #endregion



    // Properties
    // ------------------------------------------------------------

    #region PublicProperties

    /** Return the number of known crews. */
    public int CrewCount
        { get { return Crews.Count; } }

    /** Regular expression used to match server data keys. */
    public readonly Regex DataKeyPattern = new Regex(
        @"^(crew)(\w+)(\d+)$", RegexOptions.IgnoreCase);

    #endregion


    // Members
    // ------------------------------------------------------------

    /** Number of registered crew members. */
    private int _registeredCrewCount;


    // Unity Methods
    // ------------------------------------------------------------

    #region UnityMethods

    /** Serverside initialization logic. */
    public override void OnStartServer()
    {
        base.OnStartServer();
        InitializeCrews();
    }

    /** Update. */
    private void Update()
    {
        while (CrewCount > _registeredCrewCount)
        {
            _registeredCrewCount++;
            RegisterCrewParams(_registeredCrewCount);
        }
    }
    #endregion


    // Public Methods
    // ------------------------------------------------------------

    /** Add a crew. */
    [Server]
    public void AddCrew(Crew crew)
    {
        // Check if crew already exists in the list.
        // If so, just overwrite the existing crew with this one.
        if (crew.Id > 0 && crew.Id <= Crews.Count)
        {
            SetCrew(crew.Id, crew);
            return;
        }

        // Assign crew an id based on the highest existing id in list.
        var id = Crews.Count + 1;
        crew.Id = id;

        // Add crew to the synchronized list.
        Crews.Add(crew);
    }


    // Setters and Getters
    // ------------------------------------------------------------

    /** Return a crew for the given id. */
    public Crew GetCrew(int id)
    {
        if (id >= 1 && id <= Crews.Count)
            return Crews[id - 1];

        return new Crew { Id = id };
    }

    /** Whether the given server data key relates to crew state. */
    public bool IsCrewKey(string valueName)
        { return valueName.StartsWith("crew"); }

    /** Updates a server shared value. */
    public void SetServerData(string valueName, float value, bool add = false)
    {
        // Parse value into crew id and parameter name.
        int id; string parameter;
        if (!TryParseKey(valueName, out id, out parameter))
            return;

        // Apply the appropriate change.
        switch (parameter.ToLower())
        {
            case "heartrate":
                SetCrew(id, new Crew(GetCrew(id)) { HeartRate = value });
                break;
            case "bodytemp":
                SetCrew(id, new Crew(GetCrew(id)) { BodyTemp = value });
                break;
            case "heartpattern":
                SetCrew(id, new Crew(GetCrew(id)) { HeartPattern = value });
                break;
            case "heartstrengthmin":
                SetCrew(id, new Crew(GetCrew(id)) { HeartStrengthMin = value });
                break;
            case "heartstrengthmax":
                SetCrew(id, new Crew(GetCrew(id)) { HeartStrengthMax = value });
                break;
            case "respirationrate":
                SetCrew(id, new Crew(GetCrew(id)) { RespirationRate = value });
                break;
            case "etco2min":
                SetCrew(id, new Crew(GetCrew(id)) { ETCO2Min = value });
                break;
            case "etco2max":
                SetCrew(id, new Crew(GetCrew(id)) { ETCO2Max = value });
                break;
            case "etco2pattern":
                SetCrew(id, new Crew(GetCrew(id)) { ETCO2Pattern = value });
                break;
            case "spo2min":
                SetCrew(id, new Crew(GetCrew(id)) { SPO2Min = value });
                break;
            case "spo2max":
                SetCrew(id, new Crew(GetCrew(id)) { SPO2Max = value });
                break;
            case "spo2pattern":
                SetCrew(id, new Crew(GetCrew(id)) { SPO2Pattern = value });
                break;
            case "abpmin":
                SetCrew(id, new Crew(GetCrew(id)) { ABPMin = value });
                break;
            case "abpmax":
                SetCrew(id, new Crew(GetCrew(id)) { ABPMax = value });
                break;
            case "papmin":
                SetCrew(id, new Crew(GetCrew(id)) { PAPMin = value });
                break;
            case "papmax":
                SetCrew(id, new Crew(GetCrew(id)) { PAPMax = value });
                break;
            case "monitorleds":
                SetCrew(id, new Crew(GetCrew(id)) { MonitorLeds = value > 0 });
                break;
            case "monitorgraphs":
                SetCrew(id, new Crew(GetCrew(id)) { MonitorGraphs = value > 0 });
                break;
            case "photo":
                SetCrew(id, new Crew(GetCrew(id)) { Photo = value > 0 });
                break;
            case "offline":
                SetCrew(id, new Crew(GetCrew(id)) { Offline = value > 0 });
                break;
        }
    }

    /** Returns a server shared value. */
    public float GetServerData(string valueName, float defaultValue)
    {
        // Parse value into crew id and parameter name.
        int id; string parameter;
        if (!TryParseKey(valueName, out id, out parameter))
            return defaultValue;

        // Apply the appropriate change.
        switch (parameter.ToLower())
        {
            case "heartpattern":
                return GetCrew(id).HeartPattern;
            case "heartrate":
                return GetCrew(id).HeartRate;
            case "bodytemp":
                return GetCrew(id).BodyTemp;
            case "bodytempdecimals":
                return GetCrew(id).BodyTemp - Mathf.FloorToInt(GetCrew(id).BodyTemp);
            case "heartstrengthmin":
                return GetCrew(id).HeartStrengthMin;
            case "heartstrengthmax":
                return GetCrew(id).HeartStrengthMax;
            case "respirationrate":
                return GetCrew(id).RespirationRate;
            case "etco2min":
                return GetCrew(id).ETCO2Min;
            case "etco2max":
                return GetCrew(id).ETCO2Max;
            case "etco2pattern":
                return GetCrew(id).ETCO2Pattern;
            case "spo2min":
                return GetCrew(id).SPO2Min;
            case "spo2max":
                return GetCrew(id).SPO2Max;
            case "spo2pattern":
                return GetCrew(id).SPO2Pattern;
            case "abpmin":
                return GetCrew(id).ABPMin;
            case "abpmax":
                return GetCrew(id).ABPMax;
            case "papmin":
                return GetCrew(id).PAPMin;
            case "papmax":
                return GetCrew(id).PAPMax;
            case "monitorleds":
                return GetCrew(id).MonitorLeds ? 1 : 0;
            case "monitorgraphs":
                return GetCrew(id).MonitorGraphs ? 1 : 0;
            case "photo":
                return GetCrew(id).Photo ? 1 : 0;
            case "offline":
                return GetCrew(id).Offline ? 1 : 0;
            default:
                return defaultValue;
        }
    }

    /** Returns a server shared value. */
    public string GetServerDataAsText(string valueName)
    {
        // Parse value into crew id and parameter name.
        int id; string parameter;
        if (!TryParseKey(valueName, out id, out parameter))
            return "no value";

        // Return the appropriate parameter value.
        switch (parameter.ToLower())
        {
            case "name":
                return GetCrew(id).Name;
            default:
                var value = GetServerData(valueName, serverUtils.Unknown);
                return (value == serverUtils.Unknown) ? "no value" : value.ToString("n1");
        }
    }


    // Load / Save
    // ------------------------------------------------------------

    /** Save state to JSON. */
    public JSONObject Save()
    {
        var json = new JSONObject();
        var crewsJson = new JSONObject(JSONObject.Type.ARRAY);
        foreach (var v in Crews)
            crewsJson.Add(SaveCrew(v));

        json.AddField("Crew", crewsJson);
        return json;
    }

    /** Load state from JSON. */
    [Server]
    public void Load(JSONObject json)
    {
        // Reinitialize crews to default states.
        InitializeCrews();

        // Load in crew data from file.
        var crewsJson = json.GetField("Crew");
        if (crewsJson == null || crewsJson.IsNull)
            return;
        for (var i = 0; i < crewsJson.Count; i++)
            AddCrew(LoadCrew(crewsJson[i]));
    }

    /** Save a crew state to JSON. */
    private JSONObject SaveCrew(Crew crew)
    {
        var json = new JSONObject();
        json.AddField("Id", crew.Id);
        json.AddField("Name", crew.Name);
        json.AddField("HeartRate", crew.HeartRate);
        json.AddField("BodyTemp", crew.BodyTemp);
        json.AddField("HeartStrengthMin", crew.HeartStrengthMin);
        json.AddField("HeartStrengthMax", crew.HeartStrengthMax);
        json.AddField("HeartPattern", crew.HeartPattern);
        json.AddField("RespirationRate", crew.RespirationRate);
        json.AddField("ETCO2Min", crew.ETCO2Min);
        json.AddField("ETCO2Max", crew.ETCO2Max);
        json.AddField("ETCO2Pattern", crew.ETCO2Pattern);
        json.AddField("SPO2Min", crew.SPO2Min);
        json.AddField("SPO2Max", crew.SPO2Max);
        json.AddField("SPO2Pattern", crew.SPO2Pattern);
        json.AddField("ABPMin", crew.ABPMin);
        json.AddField("ABPMax", crew.ABPMax);
        json.AddField("PAPMin", crew.PAPMin);
        json.AddField("PAPMax", crew.PAPMax);
        json.AddField("MonitorLeds", crew.MonitorLeds);
        json.AddField("MonitorGraphs", crew.MonitorGraphs);
        json.AddField("Photo", crew.Photo);
        json.AddField("Offline", crew.Offline);
        return json;
    }

    /** Load crew state from JSON. */
    private Crew LoadCrew(JSONObject json, int id = 0)
    {
        var crew = new Crew(id);

        json.GetField(ref crew.Id, "Id");
        json.GetField(ref crew.Name, "Name");
        json.GetField(ref crew.HeartRate, "HeartRate");
        json.GetField(ref crew.BodyTemp, "BodyTemp");
        json.GetField(ref crew.HeartStrengthMin, "HeartStrengthMin");
        json.GetField(ref crew.HeartStrengthMax, "HeartStrengthMax");
        json.GetField(ref crew.HeartPattern, "HeartPattern");
        json.GetField(ref crew.RespirationRate, "RespirationRate");
        json.GetField(ref crew.ETCO2Min, "ETCO2Min");
        json.GetField(ref crew.ETCO2Max, "ETCO2Max");
        json.GetField(ref crew.ETCO2Pattern, "ETCO2Pattern");
        json.GetField(ref crew.SPO2Min, "SPO2Min");
        json.GetField(ref crew.SPO2Max, "SPO2Max");
        json.GetField(ref crew.SPO2Pattern, "SPO2Pattern");
        json.GetField(ref crew.ABPMin, "ABPMin");
        json.GetField(ref crew.ABPMax, "ABPMax");
        json.GetField(ref crew.PAPMin, "PAPMin");
        json.GetField(ref crew.PAPMax, "PAPMax");
        json.GetField(ref crew.MonitorLeds, "MonitorLeds");
        json.GetField(ref crew.MonitorGraphs, "MonitorGraphs");
        json.GetField(ref crew.Offline, "Offline");
        return crew;
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Initialize the crew state list. */
    [Server]
    private void InitializeCrews()
    {
        // Remove any existing crews.
        Crews.Clear();

        // Populate predefined crews from configuration.
        var crews = Configuration.GetJson("crew");
        if (crews.IsArray)
            for (var i = 0; i < crews.Count; i++)
                AddCrewFromConfig(i + 1, crews[i]);
    }

    /** Get a crew's name from configuration. */
    [Server]
    private void AddCrewFromConfig(int id, JSONObject json)
    {
        var crew = LoadCrew(json, id);
        AddCrew(crew);
    }

    /** Update a crew by id. */
    private void SetCrew(int id, Crew crew)
    {
        // Ensure enough crews exist in the synchronized list.
        // while (Crews.Count < id)
        //    Crews.Add(new Crew());

        // Update crew's data in the synchronized list.
        if (id >= 1 && id <= Crews.Count)
            Crews[id - 1] = crew;
    }

    /** Parse a server data value key into crew id and parameter key. */
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

        // Parse the match into crew id and state key.
        parameter = match.Groups[2].Value;
        id = int.Parse(match.Groups[3].Value);

        // Successfully parsed key into components.
        return true;
    }

    /** Register server values for a given crew member. */
    private void RegisterCrewParams(int id)
    {
        // Register crew dynamic server parameters.
        serverUtils.RegisterServerValue(string.Format("crewabpmin{0}", id), new serverUtils.ParameterInfo { description = "Minimum ambulatory blood pressure." });
        serverUtils.RegisterServerValue(string.Format("crewabpmax{0}", id), new serverUtils.ParameterInfo { maxValue = 200, description = "Maximum ambulatory blood pressure." });
        serverUtils.RegisterServerValue(string.Format("crewbodytemp{0}", id), new serverUtils.ParameterInfo { description = "Current body temperaturefor crew member 1." });
        serverUtils.RegisterServerValue(string.Format("crewetco2min{0}", id), new serverUtils.ParameterInfo { description = "Minimum end-tidal CO2 level." });
        serverUtils.RegisterServerValue(string.Format("crewetco2max{0}", id), new serverUtils.ParameterInfo { description = "Maximum end-tidal CO2 level." });
        serverUtils.RegisterServerValue(string.Format("crewetco2pattern{0}", id), new serverUtils.ParameterInfo { maxValue = 5, description = "Pattern to use for end-tidal CO2 graph." });
        serverUtils.RegisterServerValue(string.Format("crewheartpattern{0}", id), new serverUtils.ParameterInfo { maxValue = 5, description = "Pattern to use for heartrate graph." });
        serverUtils.RegisterServerValue(string.Format("crewheartrate{0}", id), new serverUtils.ParameterInfo { maxValue = 200, description = "Current heartrate for crew member 1." });
        serverUtils.RegisterServerValue(string.Format("crewheartstrengthmin{0}", id), new serverUtils.ParameterInfo { description = "Minimum amplitude of heartrate signal (y axis)." });
        serverUtils.RegisterServerValue(string.Format("crewheartstrengthmax{0}", id), new serverUtils.ParameterInfo { description = "Maximum amplitude of heartrate signal (y axis)." });
        serverUtils.RegisterServerValue(string.Format("crewmonitorleds{0}", id), new serverUtils.ParameterInfo { maxValue = 1, type = serverUtils.ParameterType.Bool, description = "Whether LEDs are on in the Medical bay monitor screen." });
        serverUtils.RegisterServerValue(string.Format("crewmonitorgraphs{0}", id), new serverUtils.ParameterInfo { maxValue = 1, type = serverUtils.ParameterType.Bool, description = "Whether graphs are on in the Medical bay monitor screen." });
        serverUtils.RegisterServerValue(string.Format("crewpapmin{0}", id), new serverUtils.ParameterInfo { description = "Minimum pulmonary arterial pressure." });
        serverUtils.RegisterServerValue(string.Format("crewpapmax{0}", id), new serverUtils.ParameterInfo { description = "Maximum pulmonary arterial pressure." });
        serverUtils.RegisterServerValue(string.Format("crewrespirationrate{0}", id), new serverUtils.ParameterInfo { description = "Current resipiration rate for crew member 1" });
        serverUtils.RegisterServerValue(string.Format("crewspo2min{0}", id), new serverUtils.ParameterInfo { description = "Minimum Oxygen saturation level." });
        serverUtils.RegisterServerValue(string.Format("crewspo2max{0}", id), new serverUtils.ParameterInfo { description = "Maximum Oxygen saturation level." });
        serverUtils.RegisterServerValue(string.Format("crewspo2pattern{0}", id), new serverUtils.ParameterInfo { maxValue = 5, description = "Pattern to use for oxygen saturation graph." });
        serverUtils.RegisterServerValue(string.Format("crewphoto{0}", id), new serverUtils.ParameterInfo { maxValue = 1, type = serverUtils.ParameterType.Bool, description = "Display crew photo?" });
        serverUtils.RegisterServerValue(string.Format("crewoffline{0}", id), new serverUtils.ParameterInfo { maxValue = 1, type = serverUtils.ParameterType.Bool, description = "Crew member is offline?" });
    }


}

