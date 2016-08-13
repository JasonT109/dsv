using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


    [Header("Prefabs")]

    /** Prefabs to use for sonar pings. */
    public PingIcon[] PingIcons;


    // Structures
    // ------------------------------------------------------------

    /** Ping icon-prefab association. */
    [System.Serializable]
    public struct PingIcon
    {
        public vesselData.Icon Icon;
        public SonarPing Prefab;
    }


    // Private Properties
    // ------------------------------------------------------------

    /** Vessel data. */
    private vesselData VesselData
        { get { return serverUtils.VesselData; } }


    // Members
    // ------------------------------------------------------------

    /** The list of ping instances. */
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
            var vessel = VesselData.Vessels[i];
            var ping = GetPing(index++, vessel);
            ping.Pings = this;
            ping.Vessel = vessel;
            ping.Refresh();
        }

        for (var i = index; i < _pings.Count; i++)
            _pings[i].gameObject.SetActive(false);
    }

    /** Return a ping object for the given index. */
    private SonarPing GetPing(int i, vesselData.Vessel vessel)
    {
        // Add a new ping instance if needed.
        var prefab = GetPingPrefab(vessel.Icon);
        if (i >= _pings.Count)
        {
            var ping = Instantiate(prefab);
            ping.transform.SetParent(Root, false);
            _pings.Add(ping);
        }

        // Switch ping instances if icon changes.
        if (_pings[i].Vessel.Icon != vessel.Icon)
        {
            Destroy(_pings[i].gameObject);
            _pings[i] = Instantiate(prefab);
            _pings[i].transform.SetParent(Root, false);
        }

        // Return the ping instance.
        return _pings[i];
    }

    /** Return a ping prefab to use for a given vessel icon. */
    private SonarPing GetPingPrefab(vesselData.Icon icon)
        { return PingIcons.FirstOrDefault(p => p.Icon == icon).Prefab; }

}
