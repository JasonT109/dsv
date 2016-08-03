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
    public static T Get<T>(string key, T defaultValue = default(T))
        { return Instance.GetField<T>(key, defaultValue); }

    /** Return a configuration value for the given key. */
    public static JSONObject GetJson(string key)
        { return Instance.GetField(key); }

    /** Return whether a configuration value exists for the given key. */
    public static bool Has(string key)
        { return Instance.HasField(key); }


    // Public Properties
    // ------------------------------------------------------------

    /** Return the configuration ID for this program instance. */
    public string Id
        { get { return GetConfigId(); } }

    /** Configuration data. */
    public JSONObject Data
    {
        get
        {
            if (!_data)
                LoadData();

            return _data;
        }
    }

    /** Configuration data for the current id. */
    public JSONObject DataForId
    {
        get
        {
            if (!_dataForId)
                _dataForId = Data.GetField(Id);

            if (!_dataForId)
                _dataForId = DataDefault;

            return _dataForId;
        }
    }

    /** Configuration data for the current id. */
    public JSONObject DataDefault
    {
        get
        {
            if (!_dataDefault)
                _dataDefault = Data.GetField(DefaultId);

            if (!_dataDefault)
                _dataDefault = new JSONObject();

            return _dataDefault;
        }
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Retrieve a configuration value by key. */
    public T GetField<T>(string key, T defaultValue)
    {
        if (CommandLine.HasParameter(key))
            return CommandLine.GetParameter(key, defaultValue);

        if (!DataForId.HasField(key))
            return GetDefault(key, defaultValue);

        var json = DataForId.GetField(key);
        var str = json.IsString ? json.str : json.ToString();

        return Parsing.Parse(str, defaultValue);
    }

    /** Retrieve a configuration JSON value by key. */
    public JSONObject GetField(string key)
    {
        if (CommandLine.HasParameter(key))
            return new JSONObject(CommandLine.GetParameter(key));

        if (!DataForId.HasField(key))
            return GetDefault(key);

        return DataForId.GetField(key);
    }

    /** Retrieve a default configuration value by key. */
    public T GetDefault<T>(string key, T defaultValue)
    {
        if (!DataDefault.HasField(key))
            return defaultValue;

        var json = DataDefault.GetField(key);
        var str = json.IsString ? json.str : json.ToString();

        return Parsing.Parse(str, defaultValue);
    }

    /** Retrieve a default configuration JSON value by key. */
    public JSONObject GetDefault(string key)
    {
        if (!DataDefault.HasField(key))
            return new JSONObject();

        return DataDefault.GetField(key);
    }

    /** Return whether a configuration value exists for the given key. */
    public bool HasField(string key)
    {
        return DataForId.HasField(key) || CommandLine.HasParameter(key);
    }


    // Members
    // ------------------------------------------------------------

    /** The configuration ID for this program instance. */
    private string _id;

    /** Configuration data. */
    private JSONObject _data;

    /** Configuration data for the current id. */
    private JSONObject _dataForId;

    /** Configuration data for the default id. */
    private JSONObject _dataDefault;


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
        // Determine the config file location (next to executable).
        var folder = Application.dataPath + "/../";
        var config = folder + CommandLine.GetParameter("config", "config.json");
        var path = Path.GetFullPath(config);

        // Check if config exists.
        if (!File.Exists(path))
            _data = new JSONObject();

        // Load configuration JSON data.
        try
        {
            _data = Load(path);
            Debug.Log("Loaded configuration file: " + path + ", using ID '" + Id + "'.");
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

}
