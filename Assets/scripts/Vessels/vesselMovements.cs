using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Meg.EventSystem;
using Meg.Networking;
using UnityEngine.Networking;

/** 
 * Manages the movements of non-player vessels.
 * Each vessel can have a single associated vesselMovement module.
 */

public class vesselMovements : NetworkBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** Type id for intercept movements. */
    public const string InterceptType = "Intercept";

    /** Type id for pursuit movements. */
    public const string PursueType = "Pursue";

    /** Type id for holding pattern movements. */
    public const string HoldingType = "Holding";

    /** Type id for set vector movements. */
    public const string SetVectorType = "SetVector";

    /** Type id for no movement. */
    public const string NoType = "None";

    /** The maximum number of vessels to consider. */
    private const int MaxVessels = 10;


    // Components
    // ------------------------------------------------------------

    /** The map data component. */
    public mapData MapData
    { get; private set; }


    // Properties
    // ------------------------------------------------------------

    [Header("Prefabs")]

    /** Prefab that is used when putting vessels into a holding pattern. */
    public vesselHoldingPattern HoldingPatternPrefab;

    /** Prefab that is used when putting vessels on a set vector course. */
    public vesselSetVector SetVectorPrefab;

    /** Prefab that is used when putting vessels on an interception course. */
    public vesselIntercept InterceptPrefab;

    /** Prefab that is used when putting vessels in pursuit of another vessel. */
    public vesselPursue PursuePrefab;


    // Synchronization
    // ------------------------------------------------------------

    [Header("Synchronization")]

    /** Whether movement simulation is enabled. */
    [SyncVar]
    public bool Enabled = true;

    /** Whether movement simulation is currently active. */
    [SyncVar]
    public bool Active;

    /** Initial time to intercept (seconds). */
    [SyncVar]
    public float TimeToIntercept = 120;


    // Structures
    // ------------------------------------------------------------

    /** Time to intercept initial state. */
    private float _initialTimeToIntercept;

    /** Tracking data for a vessel. */
    private struct VesselState
    {
        public Vector3 position;
        public float velocity;
        public bool visible;
    }


    // Members
    // ------------------------------------------------------------

    /** The set of current vessel movement modes. */
    private readonly List<List<vesselMovement>> _movements = new List<List<vesselMovement>>();

    /** Initial position of each vessel. */
    private readonly List<VesselState> _initialVesselStates = new List<VesselState>();


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Awake()
    {
        // Get the map data component.
        MapData = GetComponent<mapData>();

        // Initialize movement tracking collection.
        for (var i = 0; i < MaxVessels; i++)
            _movements.Add(new List<vesselMovement>());
    }

    /** Update the vessel movement module. */
    [ServerCallback]
    protected virtual void LateUpdate()
    {
        // Update active state.
        if (isServer)
            Active = Enabled && megEventManager.Instance.Playing;

        // Update the vessel's movement.
        if (Active && isServer)
            UpdateMovement();
    }


    // Server Methods
    // ------------------------------------------------------------

    /** Place the given vessel into a holding pattern. */
    [Server]
    public void SetHoldingPattern(int vessel)
    {
        if (IsHoldingPattern(vessel))
            return;

        var holdingPattern = CreateVesselMovement(HoldingPatternPrefab);
        holdingPattern.Configure(vessel, Active);
        SetVesselMovement(vessel, holdingPattern);
    }

    /** Place the given vessel on a set vector course. */
    [Server]
    public void SetVector(int vessel)
    {
        if (IsSetVector(vessel))
            return;

        var setVector = CreateVesselMovement(SetVectorPrefab);
        setVector.Configure(vessel, Active);
        SetVesselMovement(vessel, setVector);
    }

    /** Place the given vessel on an interception course. */
    [Server]
    public void SetIntercept(int vessel)
    {
        if (IsIntercepting(vessel))
            return;

        var intercept = CreateVesselMovement(InterceptPrefab);
        intercept.Configure(vessel, Active);
        SetVesselMovement(vessel, intercept);
    }

    /** Place the given vessel in pursuit mode. */
    [Server]
    public void SetPursue(int vessel)
    {
        if (IsPursuing(vessel))
            return;

        var pursue = CreateVesselMovement(PursuePrefab);
        pursue.Configure(vessel, Active);
        SetVesselMovement(vessel, pursue);
    }

    /** Set the vessel's time to intercept. */
    [Server]
    public void SetTimeToIntercept(float value)
    {
        TimeToIntercept = value;
    }

    /** Removes any active movement commands from the given vessel. */
    [Server]
    public void SetNone(int vessel)
    {
        RemoveVesselMovement(vessel);
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Register a new movement module. */
    public void Register(vesselMovement movement)
    {
        if (!isServer)
            _movements[movement.Vessel].Add(movement);
    }

    /** Unregister a movement module. */
    public void Unregister(vesselMovement movement)
    {
        if (!isServer)
            _movements[movement.Vessel].Remove(movement);
    }

    /** Returns whether a vessel is in a holding pattern. */
    public bool IsHoldingPattern(int vessel)
        { return GetVesselMovement(vessel) is vesselHoldingPattern; }

    /** Returns whether a vessel is intercepting. */
    public bool IsIntercepting(int vessel)
    { return GetVesselMovement(vessel) is vesselIntercept; }

    /** Returns whether any vessel is intercepting. */
    public bool IsAnyIntercepting()
    { return _movements.Any(ms => ms.Any(m => m is vesselIntercept)); }

    /** Returns whether a vessel is on a set vector course. */
    public bool IsSetVector(int vessel)
    { return GetVesselMovement(vessel) is vesselSetVector; }

    /** Returns whether a vessel is pursuing. */
    public bool IsPursuing(int vessel)
    { return GetVesselMovement(vessel) is vesselPursue; }

    /** Return the player vessel's current movement mode (if any). */
    public vesselMovement GetPlayerVesselMovement()
        { return GetVesselMovement(serverUtils.GetPlayerVessel());}

    /** Return the vessel's current movement mode (if any). */
    public vesselMovement GetVesselMovement(int vessel)
    {
        if (vessel <= 0 || vessel >= _movements.Count)
            return null;

        if (_movements[vessel].Count > 0)
            return _movements[vessel][0];

        return null;
    }

    /** Whether movement simulation is active. */
    public bool IsActive()
    {
        return Active;
    }


    // Load / Save
    // ------------------------------------------------------------

    /** Save movement state to JSON. */
    public JSONObject Save()
    {
        var json = new JSONObject();
        json.AddField("Active", Active);
        json.AddField("TimeToIntercept", TimeToIntercept);

        // Save out active movements for each vessel.
        var movementsJson = new JSONObject(JSONObject.Type.ARRAY);
        foreach (var m in _movements)
            if (m.Count > 0)
                movementsJson.Add(m[0].Save());
        json.AddField("Movements", movementsJson);

        return json;
    }

    /** Save a single vessel's movement state to JSON. */
    public JSONObject SaveVessel(int vessel)
    {
        var m = _movements[vessel].FirstOrDefault();
        return m ? m.Save() : new JSONObject();
    }

    /** Load movement state from JSON. */
    [Server]
    public void Load(JSONObject json)
    {
        // Clear out any existing movements.
        Clear();

        // Load in movements from data.
        var movementsJson = json.GetField("Movements");
        for (var i = 0; i < movementsJson.Count; i++)
            LoadVessel(movementsJson[i]);

        float tti = 0;
        json.GetField(ref tti, "TimeToIntercept");
        SetTimeToIntercept(tti);
    }

    /** Load a single vessel's movement state to JSON. */
    [Server]
    public void LoadVessel(JSONObject json)
    {
        var vessel = 0;
        string type = null;
        json.GetField(ref type, "Type");
        json.GetField(ref vessel, "Vessel");
        var movement = CreateVesselMovement(type);
        if (movement)
            movement.Load(json, MapData);

        SetVesselMovement(vessel, movement);
    }

    /** Capture the initial state of all vessels. */
    public void CaptureInitialState()
    {
        _initialTimeToIntercept = TimeToIntercept;

        _initialVesselStates.Clear();
        var n = serverUtils.GetVesselCount();
        for (var i = 0; i < n; i++)
        {
            var vessel = i + 1;
            _initialVesselStates.Add(new VesselState
            {
                position = serverUtils.GetVesselPosition(vessel),
                velocity = serverUtils.GetVesselVelocity(vessel),
                visible = serverUtils.GetVesselVis(vessel)
            });
        }
    }

    /** Reset vessel states to initial values. */
    public void ResetToInitialState()
    {
        // Reset vessels to the recorded state.
        var n = serverUtils.GetVesselCount();
        for (var i = 0; i < n; i++)
        {
            var state = _initialVesselStates[i];
            var vessel = i + 1;

            serverUtils.SetVesselPosition(vessel, state.position);
            serverUtils.SetVesselVelocity(vessel, state.velocity);
            serverUtils.SetVesselVis(vessel, state.visible);
        }

        // Reset player's world velocity.
        serverUtils.SetPlayerWorldVelocity(Vector3.zero);

        // Update due time to match interception time.
        SetTimeToIntercept(_initialTimeToIntercept);
        serverUtils.SetServerData("dueTime", TimeToIntercept);
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Place a vessel into the given movement mode. */
    [Server]
    private void SetVesselMovement(int vessel, vesselMovement movement)
    {
        RemoveVesselMovement(vessel);
        if (!movement)
            return;

        _movements[vessel].Add(movement);
    }

    /** Remove any current movement from a vessel. */
    [Server]
    private void RemoveVesselMovement(int vessel)
    {
        foreach (var movement in _movements[vessel])
            Destroy(movement.gameObject);
        
        _movements[vessel].Clear();
    }

    /** Remove all vessel movements. */
    [Server]
    private void Clear()
    {
        for (var i = 0; i < _movements.Count; i++)
            RemoveVesselMovement(i);
    }

    /** Create a movement given a type code. */
    [Server]
    private vesselMovement CreateVesselMovement(string type)
    {
        switch (type)
        {
            case HoldingType:
                return CreateVesselMovement(HoldingPatternPrefab);
            case InterceptType:
                return CreateVesselMovement(InterceptPrefab);
            case SetVectorType:
                return CreateVesselMovement(SetVectorPrefab);
            case PursueType:
                return CreateVesselMovement(PursuePrefab);
            default:
                return null;
        }
    }

    /** Create a movement module, given the appropriate prefab. */
    [Server]
    private vesselMovement CreateVesselMovement(vesselMovement prefab)
    {
        return Instantiate(prefab);
    }

    /** Update vessel movements. */
    [Server]
    private void UpdateMovement()
    {
        if (IsAnyIntercepting())
            UpdateTimeToIntercept();
    }

    /** Update vessel time to intercept. */
    [Server]
    private void UpdateTimeToIntercept()
    {
        // Update interception time.
        TimeToIntercept = Mathf.Max(0, TimeToIntercept - Time.deltaTime);

        // Update due time to match interception time.
        serverUtils.SetServerData("dueTime", TimeToIntercept);
    }

}
