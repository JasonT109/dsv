using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Meg.Networking;

public class mapData : NetworkBehaviour
{

    /** Default floor depth to assume. */
    public const float DefaultFloorDepth = 11050;

    public const int MaxLinePointCount = 50;

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


    // Properties
    // ------------------------------------------------------------

    #region PublicProperties

    /** Regular expression used to match server data keys. */
    public readonly Regex LineDataKeyPattern = new Regex(
        @"^(mapline)(\w+)(\d+)$", RegexOptions.IgnoreCase);

    public bool IsMap2D
        { get { return mapMode == Mode.Mode2D; } }

    public bool IsMap3D
        { get { return mapMode == Mode.Mode3D; } }

    public bool IsSubSchematic
        { get { return mapMode == Mode.ModeSubSchematic; } }

    #endregion


    // Enumerations
    // ------------------------------------------------------------

    /** Possible line styles. */
    public enum LineStyle
    {
        None = -1,
        Normal = 0,
        Dashed = 1
    }

    /** Possible point styles. */
    public enum PointStyle
    {
        None = -1,
        Circle = 0,
        Diamond = 1
    }


    // Structures
    // ------------------------------------------------------------

    /** Structure representing a line on the map. */
    [Serializable]
    public struct Line
    {
        public int Id;
        public string Name;
        public LineStyle Style;
        public float Width;
        public Color Color;
        public PointStyle PointStyle;
        public Vector3[] Points;

        public Line(Line other)
        {
            Id = other.Id;
            Name = other.Name;
            Style = other.Style;
            Color = other.Color;
            Width = other.Width;
            PointStyle = other.PointStyle;
            if (other.Points != null)
                Points = other.Points.Clone() as Vector3[];
            else
                Points = null;
        }

        public Line(int id)
        {
            Id = id;
            Name = "Line";
            Style = LineStyle.Normal;
            Color = Color.white;
            PointStyle = PointStyle.None;
            Points = new Vector3[0];
            Width = 0.1f;
        }

    }

    /** Class definition for a synchronized list of line states. */
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
    }

    #endregion


    // Public Methods
    // ------------------------------------------------------------

    /** Add a line. */
    [Server]
    public void AddLine(Line line)
    {
        // Check if line already exists in the list.
        // If so, just overwrite the existing line with this one.
        if (line.Id > 0 && line.Id <= Lines.Count)
        {
            SetLine(line.Id, line);
            return;
        }

        // Assign line an id based on the highest existing id in list.
        var id = Lines.Count + 1;
        line.Id = id;

        // Add line to the synchronized list.
        Lines.Add(line);
        LinePercentages.Add(100f);

        // Register line dynamic server parameters.
        serverUtils.RegisterServerValue(string.Format("maplinepercent{0}", id), 
            new serverUtils.ParameterInfo { description = "Progress percentage for a line on the map." });
    }

    /** Remove the last line (if possible). */
    [Server]
    public void RemoveLastLine()
    {
        if (Lines.Count > 0)
            Lines.RemoveAt(Lines.Count - 1);

        while (LinePercentages.Count > Lines.Count)
            LinePercentages.RemoveAt(LinePercentages.Count - 1);
    }

    /** Clear all lines. */
    [Server]
    public void ClearLines()
    {
        Lines.Clear();
        LinePercentages.Clear();
    }

    /** Update a line by id. */
    [Server]
    public void SetLine(int id, Line line)
    {
        // Update line's data in the synchronized list.
        if (id >= 1 && id <= Lines.Count)
            Lines[id - 1] = line;
    }

    public static LineStyle LineStyleForName(string name)
        { return (LineStyle) Enum.Parse(typeof(LineStyle), name); }

    public static PointStyle PointStyleForName(string name)
        { return (PointStyle) Enum.Parse(typeof(PointStyle), name); }


    // Setters and Getters
    // ------------------------------------------------------------

    /** Return the number of map lines. */
    public int LineCount
        { get { return Lines.Count; } }

    /** Return the last map line's id. */
    public int LastLine
        { get { return Lines.Count; } }

    /** Return a line for the given id. */
    public Line GetLine(int id)
    {
        if (id >= 1 && id <= Lines.Count)
            return Lines[id - 1];

        return new Line { Id = id };
    }

    /** Whether the given server data key relates to map line state. */
    public bool IsMapLineKey(string valueName)
        { return valueName.StartsWith("mapline"); }

    /** Updates a server shared value. */
    public void SetServerData(string valueName, float value, bool add = false)
    {
        // Parse value into line id and parameter name.
        int id; string parameter;
        if (!TryParseKey(valueName, out id, out parameter))
            return;

        // Apply the appropriate change.
        switch (parameter.ToLower())
        {
            case "percent":
                SetLinePercent(id, value);
                break;
        }
    }

    /** Returns a server shared value. */
    public float GetServerData(string valueName, float defaultValue)
    {
        // Parse value into line id and parameter name.
        int id; string parameter;
        if (!TryParseKey(valueName, out id, out parameter))
            return defaultValue;

        // Apply the appropriate change.
        switch (parameter.ToLower())
        {
            case "percent":
                return GetLinePercent(id);

            default:
                return defaultValue;
        }
    }

    /** Return progress of a line by id. */
    public float GetLinePercent(int id)
    {
        if (id >= 1 && id <= Lines.Count)
            return LinePercentages[id - 1];

        return 100f;
    }

    /** Set the progress of a line by id. */
    [Server]
    public void SetLinePercent(int id, float value)
    {
        if (id >= 1 && id <= LinePercentages.Count)
            LinePercentages[id - 1] = value;
    }


    // Load / Save
    // ------------------------------------------------------------

    /** Save map state to JSON. */
    public JSONObject Save()
    {
        var json = new JSONObject();

        var linesJson = new JSONObject(JSONObject.Type.ARRAY);
        foreach (var v in Lines)
            linesJson.Add(SaveLine(v));

        json.AddField("Lines", linesJson);

        // Load in line progress data from file.
        var percentagesJson = new JSONObject(JSONObject.Type.ARRAY);
        foreach (var percentage in LinePercentages)
            percentagesJson.Add(percentage);
        json.AddField("LinePercentages", percentagesJson);

        return json;
    }

    /** Load map state from JSON. */
    [Server]
    public void Load(JSONObject json)
    {
        // Reinitialize lines to default state.
        ClearLines();

        // Check if valid JSON was supplied.
        if (!json || json.IsNull)
            return;

        // Load in line data from file.
        var linesJson = json.GetField("Lines");
        if (linesJson == null || linesJson.IsNull)
            return;
        for (var i = 0; i < linesJson.Count; i++)
            AddLine(LoadLine(linesJson[i]));

        // Load in line progress data from file.
        var percentagesJson = json.GetField("LinePercentages");
        if (percentagesJson == null || percentagesJson.IsNull)
            return;
        for (var i = 0; i < percentagesJson.Count; i++)
            SetLinePercent(i + 1, percentagesJson[i].f);
    }

    /** Save a line state to JSON. */
    private JSONObject SaveLine(Line line)
    {
        var json = new JSONObject();
        json.AddField("Id", line.Id);
        json.AddField("Name", line.Name);
        json.AddField("Color", line.Color);
        json.AddField("Style", line.Style.ToString());
        json.AddField("PointStyle", line.PointStyle.ToString());
        json.AddField("Width", line.Width);

        var pointsJson = new JSONObject(JSONObject.Type.ARRAY);
        if (line.Points != null)
            foreach (var p in line.Points)
                pointsJson.Add(p);

        json.AddField("Points", pointsJson);

        return json;
    }

    /** Load line state from JSON. */
    private Line LoadLine(JSONObject json)
    {
        var line = new Line(0);
        json.GetField(ref line.Id, "Id");
        json.GetField(ref line.Name, "Name");
        json.GetField(ref line.Color, "Color");
        json.GetField(ref line.Width, "Width");

        var styleName = "Normal";
        json.GetField(ref styleName, "Style");
        line.Style = (LineStyle) Enum.Parse(typeof(LineStyle), styleName, true);

        var pointStyleName = "None";
        json.GetField(ref pointStyleName, "PointStyle");
        line.PointStyle = (PointStyle) Enum.Parse(typeof(PointStyle), pointStyleName, true);

        // Load in line points.
        var linesJson = json.GetField("Points");
        if (linesJson != null && linesJson.IsArray)
        {
            var n = linesJson.Count;
            line.Points = new Vector3[n];
            for (var i = 0; i < n; i++)
                line.Points[i] = linesJson[i].toVector3();
        }
        else
            line.Points = new Vector3[0];

        return line;
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Parse a server data value key into line id and parameter key. */
    private bool TryParseKey(string valueName, out int id, out string parameter)
    {
        // Check if a value name was supplied.
        id = 0; parameter = "";
        if (string.IsNullOrEmpty(valueName))
            return false;

        // Pattern match on incoming key to determine what to change.
        var key = valueName.ToLower();
        var match = LineDataKeyPattern.Match(key);
        if (!match.Success)
            return false;

        // Parse the match into line id and state key.
        parameter = match.Groups[2].Value;
        id = int.Parse(match.Groups[3].Value);

        // Successfully parsed key into components.
        return true;
    }


}