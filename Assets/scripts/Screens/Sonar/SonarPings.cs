using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Meg.Networking;

public class SonarPings : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** The root transform in which to place sonar pings. */
    public Transform Root;

    /** The kind of sonar we're using. */
    public SonarData.Type Type;


    [Header("Configuration")]

    /** Scaling factor from normalized sonar space into root space. */
    public float SonarToRootScale = 1;

    /** Angular speed of the sonar sweep. */
    // public float 


    [Header("Prefabs")]

    /** Prefab to instantiate when adding a sonar ping. */
    public SonarPing PingPrefab;


    // Private Properties
    // ------------------------------------------------------------

    /** Vessel data. */
    private vesselData VesselData
        { get { return serverUtils.VesselData; } }


    // Members
    // ------------------------------------------------------------

    /** The list of pings. */
    private List<SonarPing> _pings = new List<SonarPing>();
    

    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        if (!Root)
            Root = transform;
    }
	
	/** Update sonar pings. */
	private void LateUpdate()
	{
	    UpdatePings();
	}


    // Private Methods
    // ------------------------------------------------------------

    /** Update sonar pings. */
    private void UpdatePings()
    {
        if (!VesselData)
            return;

        var index = 0;
        for (var i = 0; i < VesselData.VesselCount; i++)
        {
            var ping = GetPing(index++);
            ping.Pings = this;
            ping.Vessel = VesselData.Vessels[i];
            ping.Refresh();
        }

        for (var i = index; i < _pings.Count; i++)
            _pings[i].gameObject.SetActive(false);
    }

    /** Return a ping object for the given index. */
    private SonarPing GetPing(int i)
    {
        if (i >= _pings.Count)
        {
            var ping = Instantiate(PingPrefab);
            ping.transform.SetParent(Root, false);
            _pings.Add(ping);
        }

        return _pings[i];
    }

}
