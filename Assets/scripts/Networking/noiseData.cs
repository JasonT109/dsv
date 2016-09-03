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
        public float Percentage;
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


    // Unity Methods
    // ------------------------------------------------------------

    /** Serverside initialization logic. */
    public override void OnStartServer()
    {
        base.OnStartServer();

        // Add default noise in for each server parameter that wants it.
        foreach (var info in serverUtils.ParameterInfos)
            if (info.Value.noise > 0 && !info.Value.readOnly)
                AddNoise(info.Key, new Profile { Percentage = info.Value.noise });
    }


    /** Add noise for the given parameter. */
    [Server]
    public void AddNoise(string parameter, Profile profile)
    {
        if (_sources.ContainsKey(parameter))
            return;

        var info = serverUtils.GetServerDataInfo(parameter);
        if (info.type == serverUtils.ParameterType.Bool)
            return;
        
        var source = Instantiate(NoisePrefab);
        source.transform.SetParent(transform, false);
        source.LinkDataString = parameter;

        var range = info.maxValue - info.minValue;
        var delta = range * (profile.Percentage * 0.01f);

        source.LinkDataString = parameter;
        source.NoiseSource.Values.Range.x = -delta;
        source.NoiseSource.Values.Range.y = +delta;
        // source.OutputRange.x = info.minValue;
        // source.OutputRange.y = info.maxValue;

        NetworkServer.Spawn(source.gameObject);
    }


    /** Remove noise for the given parameter. */
    [Server]
    public void RemoveNoise(string parameter)
    {
        if (!_sources.ContainsKey(parameter))
            return;

        var source = _sources[parameter];
        _sources.Remove(parameter);
        Destroy(source.gameObject);
    }

    /** Return whether a given parameter has associated noise. */
    public bool HasNoise(string parameter)
    {
        return _sources.ContainsKey(parameter);
    }

    /** Return current noise value for the given parameter. */
    public float Sample(string parameter)
    {
        serverValueNoise source;
        if (!_sources.TryGetValue(parameter, out source))
            return 0;

        return source.Value;
    }

    /** Register a new noise source. */
    public void Register(serverValueNoise source)
    {
        _sources[source.LinkDataString] = source;
    }

    /** Unregister a noise source. */
    public void Unregister(serverValueNoise source)
    {
       _sources.Remove(source.LinkDataString);
    }

}

