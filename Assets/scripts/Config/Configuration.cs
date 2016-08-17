using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class Configuration : AutoSingleton<Configuration>
{

    // Constants
    // ------------------------------------------------------------

    /** Default ID to use if none is specified. */
    public const string DefaultId = "default";


    // Static Methods
    // ------------------------------------------------------------

    /** Return a configuration value for the given key. */
    public static T Get<T>(string key, T defaultValue = default(T), string configId = null)
        { return Instance.GetField(key, defaultValue, configId); }

    /** Return a configuration value for the given key. */
    public static JSONObject GetJson(string key, string configId = null)
        { return Instance.GetField(key, configId); }

    /** Return whether a configuration value exists for the given key. */
    public static bool Has(string key, string configId = null)
        { return Instance.HasField(key, configId); }


    // Public Properties
    // ------------------------------------------------------------

    /** Return the configuration ID for this program instance. */
    public string CurrentId
        { get { return GetConfigId(); } }

    /** Configuration data. */
    public JSONObject DataForAllIds
    {
        get
        {
            if (!_data)
                LoadData();

            return _data;
        }
    }

    /** Return configuration data for a given id. */
    public JSONObject DataForId(string id)
    {
        if (string.IsNullOrEmpty(id))
            return new JSONObject();

        if (_dataForId == null)
        {
            _dataForId = new Dictionary<string, JSONObject>();
            foreach (var key in DataForAllIds.keys)
                _dataForId[key.ToLower()] = DataForAllIds.GetField(key);
        }

        JSONObject result;
        return _dataForId.TryGetValue(id.ToLower(), out result)
            ? result : new JSONObject();
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Retrieve a configuration value by key. */
    public T GetField<T>(string key, T defaultValue, string id = null)
    {
        // Check if command line explictly specifies the value.
        if (CommandLine.HasParameter(key))
            return CommandLine.GetParameter(key, defaultValue);

        // Use the current config id if one isn't explicitly given.
        if (string.IsNullOrEmpty(id))
            id = CurrentId;

        // Look up config value using the given id.
        return GetFieldFromId(key, defaultValue, id);
    }

    /** Retrieve a configuration JSON value by key. */
    public JSONObject GetField(string key, string id = null)
    {
        // Check if command line explictly specifies the value.
        if (CommandLine.HasParameter(key))
            return new JSONObject(CommandLine.GetParameter(key));

        // Use the current config id if one isn't explicitly given.
        if (string.IsNullOrEmpty(id))
            id = CurrentId;

        // Look up config value using the given id.
        return GetJsonFromId(key, id);
    }

    /** Return whether a configuration value exists for the given key. */
    public bool HasField(string key, string id = null)
    {
        // Check if command line explictly specifies the value.
        if (CommandLine.HasParameter(key))
            return true;

        // Use the current config id if one isn't explicitly given.
        if (string.IsNullOrEmpty(id))
            id = CurrentId;

        // Look up config value using the given id.
        return HasFieldFromId(key, id);
    }

    /** Look for and replace configuration path references with their current values. */
    public static string ExpandPaths(string path)
    {
        if (string.IsNullOrEmpty(path))
            return path;

        path = path.Replace("{save-folder}", Get("save-folder", "C:/Meg/"));
        path = path.Replace("{auto-save-folder}", Get("auto-save-folder", "C:/Meg/Autosave/"));

        return path;
    }


    // Members
    // ------------------------------------------------------------

    /** The configuration ID for this program instance. */
    private string _id;

    /** Configuration data. */
    private JSONObject _data;

    /** Lookup table for config data, indexed by config id. */
    private Dictionary<string, JSONObject> _dataForId;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Awake()
    {
        // Keep configuration resident between scenes.
        DontDestroyOnLoad(gameObject);
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Return the configuration ID in use. */
    private string GetConfigId()
    {
        // Check if we already have an id assigned.
        if (!string.IsNullOrEmpty(_id))
            return _id;

        // Parse the commandline to look for a specified ID.
        // For example, "dsv.exe -id pilotLeft" would result in an id of 'pilotLeft'.
        _id = CommandLine.GetParameterByRegex(@"-id[\s|=]+(\w+)", DefaultId);

        return _id;
    }

    /** Load data from configuration file. */
    private void LoadData()
    {
        // Get the name of the config file we'd like to load.
        var config = CommandLine.GetParameter("config", "config.json");
        if (!config.EndsWith(".json"))
            config += ".json";

        // Firstly, look for a config right next to the executable.
        var path = Path.GetFullPath(Application.dataPath + "/../" + config);

        // If that fails, look for one in the streaming assets folder.
        if (!File.Exists(path))
            path = Path.GetFullPath(Application.streamingAssetsPath + "/" + config);

        // Check if config exists.
        if (!File.Exists(path))
        {
            Debug.Log("No configuration file found.");
            _data = new JSONObject();
            return;
        }

        // Load configuration JSON data.
        try
        {
            _data = Load(path);
            Debug.Log("Loaded configuration file: " + path + ", using ID '" + CurrentId + "'.");
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Configuration.LoadData(): Failed to load config from path '" + path + "': " + ex);
            _data = new JSONObject();
        }
    }

    /** Populate the configuration data from file. */
    private JSONObject Load(string path)
    {
        var text = File.ReadAllText(path);
        var json = new JSONObject(text);
        return json;
    }

    /** Retrieve a configuration value by key, using the specified config ID. */
    private T GetFieldFromId<T>(string key, T defaultValue, string configId)
    {
        // Search through the config hierarchy for the given field.
        while (!string.IsNullOrEmpty(configId))
        {
            var data = DataForId(configId);
            if (data.HasField(key))
            {
                var json = data.GetField(key);
                var str = json.IsString ? json.str : json.ToString();
                return Parsing.Parse(str, defaultValue);
            }

            configId = GetParentForId(configId);
        }

        // Value was not found.
        return defaultValue;
    }

    /** Retrieve a configuration JSON object by key, using the specified config ID. */
    private JSONObject GetJsonFromId(string key, string configId)
    {
        // Search through the config hierarchy for the given field.
        while (!string.IsNullOrEmpty(configId))
        {
            var data = DataForId(configId);
            if (data.HasField(key))
                return data.GetField(key);

            configId = GetParentForId(configId);
        }

        // Value was not found.
        return new JSONObject();
    }

    /** Determine if config has a field for the given key. */
    private bool HasFieldFromId(string key, string configId)
    {
        // Search through the config hierarchy for the given field.
        while (!string.IsNullOrEmpty(configId))
        {
            var data = DataForId(configId);
            if (data.HasField(key))
                return true;

            configId = GetParentForId(configId);
        }

        // Value was not found.
        return false;
    }

    /** Return a parent configuration to use if the given id doesn't contain a key. */
    private string GetParentForId(string configId)
    {
        // Check if we are no longer able to parent.
        if (string.IsNullOrEmpty(configId) || configId == DefaultId)
            return null;

        // Get parent ID from the given id's data section.
        DataForId(configId).GetField(out configId, "parent", DefaultId);
        return configId;
    }


}
