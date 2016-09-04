using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using Meg.Networking;

public class noiseData : NetworkBehaviour
{

    // Structures
    // ------------------------------------------------------------

    /** Specification for noise behaviour. */
    [Serializable]
    public struct Profile
    {
        public Vector2 ValueRange;
        public Vector2 IntervalRange;

        public bool Active
            { get { return ValueRange.sqrMagnitude > 0; } }
    }


    // Properties
    // ------------------------------------------------------------

    [Header("Prefabs")]

    /** Prefab for noise sources. */
    public serverValueNoise NoisePrefab;


    [Header("Synchronization")]

    /** Overall noise scaling factor. */
    [SyncVar]
    public float Scale = 1f;


    // Members
    // ------------------------------------------------------------

    /** Collection of registered noise sources. */
    private readonly Dictionary<string, serverValueNoise> _sources = new Dictionary<string, serverValueNoise>();

    /** Noise profiles for various server parameters. */
    private static readonly Dictionary<string, Profile> Profiles = new Dictionary<string, Profile>
    {
        { "b1", new Profile { ValueRange = new Vector2(-1, 1), IntervalRange = new Vector2(0.5f, 3f)} },
        { "b2", new Profile { ValueRange = new Vector2(-1, 1), IntervalRange = new Vector2(0.5f, 3f)} },
        { "b3", new Profile { ValueRange = new Vector2(-1, 1), IntervalRange = new Vector2(0.5f, 3f)} },
        { "b4", new Profile { ValueRange = new Vector2(-1, 1), IntervalRange = new Vector2(0.5f, 3f)} },
        { "b5", new Profile { ValueRange = new Vector2(-1, 1), IntervalRange = new Vector2(0.5f, 3f)} },
        { "b6", new Profile { ValueRange = new Vector2(-1, 1), IntervalRange = new Vector2(0.5f, 3f)} },
        { "b7", new Profile { ValueRange = new Vector2(-1, 1), IntervalRange = new Vector2(0.5f, 3f)} },
        { "ballastpressure", new Profile { ValueRange = new Vector2(-1, 1), IntervalRange = new Vector2(0.5f, 3f)} },
        { "batterycurrent", new Profile { ValueRange = new Vector2(-0.2f, 0.2f), IntervalRange = new Vector2(0.5f, 3f) } },
        { "batterylife", new Profile { ValueRange = new Vector2(-0.2f, 0.2f), IntervalRange = new Vector2(0.5f, 3f)} },
        { "batterytemp", new Profile { ValueRange = new Vector2(-0.2f, 0.2f), IntervalRange = new Vector2(0.5f, 3f)} },
        { "cabinhumidity", new Profile { ValueRange = new Vector2(-0.2f, 0.2f), IntervalRange = new Vector2(0.5f, 3f)} },
        { "cabinoxygen", new Profile { ValueRange = new Vector2(-0.2f, 0.2f), IntervalRange = new Vector2(0.5f, 3f)} },
        { "cabinpressure", new Profile { ValueRange = new Vector2(-0.02f, 0.02f), IntervalRange = new Vector2(0.5f, 3f)} },
        { "cabintemp", new Profile { ValueRange = new Vector2(-0.2f, 0.2f), IntervalRange = new Vector2(0.5f, 3f)} },
        { "oxygenflow", new Profile { ValueRange = new Vector2(-2f, 2f), IntervalRange = new Vector2(0.5f, 3f)} },
        { "co2", new Profile { ValueRange = new Vector2(-0.001f, 0.001f), IntervalRange = new Vector2(0.5f, 3f)} },
        { "commssignalstrength", new Profile { ValueRange = new Vector2(-5f, 5f), IntervalRange = new Vector2(0.5f, 3f)} },
        { "hydraulicpressure", new Profile { ValueRange = new Vector2(-2f, 2f), IntervalRange = new Vector2(0.5f, 3f)} },
        { "hydraulictemp", new Profile { ValueRange = new Vector2(-1f, 1f), IntervalRange = new Vector2(0.5f, 3f)} },
        { "scrubbedco2", new Profile { ValueRange = new Vector2(-0.001f, 0.001f), IntervalRange = new Vector2(0.5f, 3f)} },
        { "scrubbedhumidity", new Profile { ValueRange = new Vector2(-0.2f, 0.2f), IntervalRange = new Vector2(0.5f, 3f)} },
        { "scrubbedoxygen", new Profile { ValueRange = new Vector2(-0.2f, 0.2f), IntervalRange = new Vector2(0.5f, 3f)} },
    };


    // Unity Methods
    // ------------------------------------------------------------

        /** Serverside initialization logic. */
    public override void OnStartServer()
    {
        base.OnStartServer();

        // Add default noise in for each server parameter that wants it.
        foreach (var info in Profiles)
            AddNoise(info.Key, info.Value);
    }


    /** Add noise for the given parameter. */
    [Server]
    public void AddNoise(string parameter, Profile profile)
    {
        if (string.IsNullOrEmpty(parameter))
            return;

        parameter = parameter.ToLower();
        if (_sources.ContainsKey(parameter))
            return;

        var info = serverUtils.GetServerDataInfo(parameter);
        if (info.type == serverUtils.ParameterType.Bool)
            return;
        
        var source = Instantiate(NoisePrefab);
        source.transform.SetParent(transform, false);
        source.LinkDataString = parameter;
        source.NoiseSource.Values.Range = profile.ValueRange;
        source.NoiseSource.Interval.Range = profile.IntervalRange; 

        NetworkServer.Spawn(source.gameObject);
    }

    /** Remove noise for the given parameter. */
    [Server]
    public void RemoveNoise(string parameter)
    {
        if (string.IsNullOrEmpty(parameter))
            return;

        parameter = parameter.ToLower();
        if (!_sources.ContainsKey(parameter))
            return;

        var source = _sources[parameter];
        _sources.Remove(parameter);
        Destroy(source.gameObject);
    }

    /** Return whether a given parameter has associated noise. */
    public bool HasNoise(string parameter)
    {
        if (string.IsNullOrEmpty(parameter))
            return false;

        parameter = parameter.ToLower();
        return _sources.ContainsKey(parameter);
    }

    /** Return current noise value for the given parameter. */
    public float Sample(string parameter)
    {
        if (string.IsNullOrEmpty(parameter))
            return 0;

        serverValueNoise source;
        if (!_sources.TryGetValue(parameter.ToLower(), out source))
            return 0;

        return source.Value;
    }

    /** Register a new noise source. */
    public void Register(serverValueNoise source)
    {
        if (!source || string.IsNullOrEmpty(source.LinkDataString))
            return;

        var parameter = source.LinkDataString.ToLower();
        _sources[parameter] = source;
    }

    /** Unregister a noise source. */
    public void Unregister(serverValueNoise source)
    {
        if (!source || string.IsNullOrEmpty(source.LinkDataString))
            return;

        var parameter = source.LinkDataString.ToLower();
        _sources.Remove(parameter);
    }

}

