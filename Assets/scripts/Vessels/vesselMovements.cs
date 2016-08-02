using UnityEngine;
using System.Collections.Generic;
using System.Linq;
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

    /** Whether movement simulation is active. */
    [SyncVar]
    public bool Active;

    /** Initial time to intercept (seconds). */
    [SyncVar]
    public float TimeToIntercept = 120;


    // Members
    // ------------------------------------------------------------

    /** The set of current vessel movement modes. */
    private readonly List<List<vesselMovement>> _movements = new List<List<vesselMovement>>();


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
    protected virtual void LateUpdate()
    {
        // Update active state on all vessels.
        SetActive(Active);

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

    /** Set all movements to active or inactive. */
    [Server]
    public void SetActive(bool value)
    {
        Active = value;
        foreach (var vessel in _movements)
            foreach (var movement in vessel)
                movement.Active = value;
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

    /** Save both movement and vessel state to JSON. */
    public JSONObject SaveFullState()
        { return Save(true); }

    /** Save movement state to JSON. */
    public JSONObject Save(bool saveVesselState = false)
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

        // Save full vessel states (position, visibility etc.) if needed.
        if (saveVesselState)
        {
            var vesselsJson = new JSONObject(JSONObject.Type.ARRAY);
            for (var i = 0; i < serverUtils.GetVesselCount(); i++)
                vesselsJson.Add(SaveVesselState(i + 1));
            json.AddField("Vessels", vesselsJson);
        }

        return json;
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
        {
            var movementJson = movementsJson[i];
            string type = null;
            movementJson.GetField(ref type, "Type");
            var movement = CreateVesselMovement(type);
            movement.Load(movementJson, MapData);
            SetVesselMovement(movement.Vessel, movement);
        }

        // Load in vessel states from data.
        var vesselsJson = json.GetField("Vessels");
        if (vesselsJson != null)
        {
            for (var i = 0; i < vesselsJson.Count; i++)
                LoadVesselState(i + 1, vesselsJson[i]);
        }

        // Load simulation active state.
        var active = true;
        json.GetField(ref active, "Active");
        serverUtils.SetServerData("simulating", active ? 1 : 0);
        
        float tti = 0;
        json.GetField(ref tti, "TimeToIntercept");
        SetTimeToIntercept(tti);
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Place a vessel into the given movement mode. */
    [Server]
    private void SetVesselMovement(int vessel, vesselMovement movement)
    {
        RemoveVesselMovement(vessel);
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
            case "Holding":
                return CreateVesselMovement(HoldingPatternPrefab);
            case "Intercept":
                return CreateVesselMovement(InterceptPrefab);
            case "SetVector":
                return CreateVesselMovement(SetVectorPrefab);
            case "Pursue":
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

    /** Save vessel state to JSON. */
    private JSONObject SaveVesselState(int vessel)
    {
        var json = new JSONObject();
        json.AddField("Position", serverUtils.GetVesselPosition(vessel));
        json.AddField("Velocity", serverUtils.GetVesselVelocity(vessel));
        json.AddField("Visible", serverUtils.GetVesselVis(vessel));
        return json;
    }

    /** Load vessel state from JSON. */
    private void LoadVesselState(int vessel, JSONObject json)
    {
        if (json.HasField("Position"))
        {
            var position = Vector3.zero;
            json.GetField(ref position, "Position");
            serverUtils.SetVesselPosition(vessel, position);

            // If this is the player vessel, reset its world velocity.
            if (vessel == MapData.playerVessel)
                serverUtils.SetPlayerWorldVelocity(Vector3.zero);
        }
        if (json.HasField("Velocity"))
        {
            var velocity = 0.0f;
            json.GetField(ref velocity, "Velocity");
            serverUtils.SetVesselVelocity(vessel, velocity);
        }
        if (json.HasField("Visible"))
        {
            var visible = false;
            json.GetField(ref visible, "Visible");
            serverUtils.SetVesselVis(vessel, visible);
        }
    }


}
