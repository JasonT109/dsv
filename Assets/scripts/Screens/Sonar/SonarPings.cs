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

    /** Server parameter used to drive scale factor (optional). */
    public string ScaleServerParam;

    /** Possible display spaces. */
    public enum DisplaySpace
    {
        Sonar,
        Vessel
    }

    /** Whether to display pings in vessel space. */
    public DisplaySpace Space = DisplaySpace.Sonar;

    /** Whether to display depth relative to the player vessel (or absolute). */
    public bool RelativeDepth = true;

    /** Whether to hide pings if out of range. */
    public bool HideIfOutOfRange = true;

    /** Whether to hide pings if not visible on sonar. */
    public bool HideIfNotOnSonar = true;

    /** Whether to hide pings if not visible on map. */
    public bool HideIfNotOnMap = false;

    /** Whether to hide pings if this is the player vessel. */
    public bool HideIfPlayer = true;

    /** Whether ping keeps its original world position/scale when added to the ping diplay. */
    public bool WorldPositionStays;


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
    private static vesselData VesselData
        { get { return serverUtils.VesselData; } }


    // Members
    // ------------------------------------------------------------

    /** The list of ping instances. */
    private readonly List<SonarPing> _pings = new List<SonarPing>();

    /** Initial root scale. */
    private float _initialRootScale;
    

    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        _initialRootScale = SonarToRootScale;

        if (!Root)
            Root = transform;
    }
	
	/** Update sonar pings. */
	private void LateUpdate()
	{
	    UpdatePings();
	}



    // Public Methods
    // ------------------------------------------------------------

    /** Convert a position in vessel space to ping vspace. */
    public Vector3 VesselToPingSpace(vesselData.Vessel vessel)
    {
        return VesselToPingSpace(VesselData.GetPosition(vessel.Id));
    }

    /** Convert a position in vessel space to ping vspace. */
    public Vector3 VesselToPingSpace(Vector3 v)
    {
        if (Space == DisplaySpace.Sonar)
            return VesselData.VesselToSonarSpace(v, Type) * SonarToRootScale;
        else
            { v = v * SonarToRootScale; v.z = 0; return v; }
    }

    /** Convert a position in ping space to vessel space. */
    public Vector3 PingToVesselSpace(Vector3 ping)
    {
        if (Space == DisplaySpace.Sonar)
            return VesselData.SonarToVesselSpace(ping / SonarToRootScale, Type);
        else
            { ping = ping / SonarToRootScale; ping.z = 0; return ping; }
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Update sonar pings. */
    private void UpdatePings()
    {
        if (!VesselData)
            return;

        if (!string.IsNullOrEmpty(ScaleServerParam))
            SonarToRootScale = serverUtils.GetServerData(ScaleServerParam, 
                SonarToRootScale) * _initialRootScale;

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
            _pings[i].gameObject.Cleanup();
            _pings[i] = Instantiate(prefab);
            _pings[i].transform.SetParent(Root, WorldPositionStays);
        }

        // Return the ping instance.
        return _pings[i];
    }

    /** Return a ping prefab to use for a given vessel icon. */
    private SonarPing GetPingPrefab(vesselData.Icon icon)
    {
        var entry = PingIcons.FirstOrDefault(p => p.Icon == icon);
        if (!entry.Prefab)
            return PingIcons.First().Prefab;

        return entry.Prefab;
    }

}
