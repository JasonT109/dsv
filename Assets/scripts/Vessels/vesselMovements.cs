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


    // Members
    // ------------------------------------------------------------

    /** The set of current vessel movement modes. */
    private readonly Dictionary<int, List<vesselMovement>> _movements = new Dictionary<int, List<vesselMovement>>();


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Awake()
    {
        // Get the map data component.
        MapData = GetComponent<mapData>();
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
    public void SetHoldingPattern(int id)
    {
        if (IsHoldingPattern(id))
            return;

        var holdingPattern = CreateVesselMovement(HoldingPatternPrefab);
        holdingPattern.Configure(id, Active);
        SetVesselMovement(id, holdingPattern);
    }

    /** Place the given vessel on a set vector course. */
    [Server]
    public void SetVector(int id)
    {
        if (IsSetVector(id))
            return;

        var setVector = CreateVesselMovement(SetVectorPrefab);
        setVector.Configure(id, Active);
        SetVesselMovement(id, setVector);
    }

    /** Place the given vessel on an interception course. */
    [Server]
    public void SetIntercept(int id)
    {
        if (IsIntercepting(id))
            return;

        // Can't allow intercept pin to intercept itself!
        if (id == vesselData.InterceptId)
            return;

        var intercept = CreateVesselMovement(InterceptPrefab);
        intercept.Configure(id, Active);
        SetVesselMovement(id, intercept);
    }

    /** Place the given vessel in pursuit mode. */
    [Server]
    public void SetPursue(int id)
    {
        if (IsPursuing(id))
            return;

        var pursue = CreateVesselMovement(PursuePrefab);
        pursue.Configure(id, Active);
        SetVesselMovement(id, pursue);
    }

    /** Set the vessel's time to intercept. */
    [Server]
    public void SetTimeToIntercept(float value)
    {
        TimeToIntercept = value;
    }

    /** Removes any active movement commands from the given vessel. */
    [Server]
    public void SetNone(int id)
    {
        RemoveVesselMovement(id);
    }

    /** Sets up movement on a vessel by type. */
    [Server]
    public void SetMovementType(int id, string type)
    {
        var current = GetMovementType(id);
        if (current == type)
            return;

        // Can't allow intercept pin to intercept itself!
        if (type == InterceptType && id == vesselData.InterceptId)
            return;

        var movement = CreateVesselMovement(type);
        if (movement)
            movement.Configure(id, Active);

        SetVesselMovement(id, movement);
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Register a new movement module. */
    public void Register(vesselMovement movement)
    {
        if (!isServer)
            GetMovements(movement.Vessel).Add(movement);
    }

    /** Unregister a movement module. */
    public void Unregister(vesselMovement movement)
    {
        if (!isServer)
            GetMovements(movement.Vessel).Remove(movement);
    }

    /** Returns whether a vessel is in a holding pattern. */
    public bool IsHoldingPattern(int id)
        { return GetVesselMovement(id) is vesselHoldingPattern; }

    /** Returns whether a vessel is intercepting. */
    public bool IsIntercepting(int id)
        { return GetVesselMovement(id) is vesselIntercept; }

    /** Returns whether any vessel is intercepting. */
    public bool IsAnyIntercepting()
        { return _movements.Values.Any(ms => ms.Any(m => m is vesselIntercept)); }

    /** Returns whether a vessel is on a set vector course. */
    public bool IsSetVector(int id)
        { return GetVesselMovement(id) is vesselSetVector; }

    /** Returns whether a vessel is pursuing. */
    public bool IsPursuing(int id)
        { return GetVesselMovement(id) is vesselPursue; }

    /** Return the type of movement for a given vessel. */
    public string GetMovementType(int id)
    {
        var movement = GetVesselMovement(id);
        return movement ? movement.Type : NoType;
    }

    /** Return the player vessel's current movement mode (if any). */
    public vesselMovement GetPlayerVesselMovement()
        { return GetVesselMovement(serverUtils.GetPlayerVessel());}

    /** Return the vessel's current movement mode (if any). */
    public vesselMovement GetVesselMovement(int id)
    {
        if (!_movements.ContainsKey(id))
            return null;

        return _movements[id].Count > 0 
            ? _movements[id][0] : null;
    }

    /** Whether movement simulation is active. */
    public bool IsActive()
        { return Active; }


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
        foreach (var m in _movements.Values)
            if (m.Count > 0)
                movementsJson.Add(m[0].Save());

        json.AddField("Movements", movementsJson);

        return json;
    }

    /** Save a single vessel's movement state to JSON. */
    public JSONObject SaveVessel(int id)
    {
        var m = GetMovements(id).FirstOrDefault();
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

    /** Capture the initial state of vessel movements. */
    public void CaptureInitialState()
    {
        _initialTimeToIntercept = TimeToIntercept;
    }

    /** Reset vessel movement state to initial values. */
    public void ResetToInitialState()
    {
        // Update due time to match interception time.
        SetTimeToIntercept(_initialTimeToIntercept);
        serverUtils.SetServerData("dueTime", TimeToIntercept);
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Return the collection of movements registered against a vessel. */
    private List<vesselMovement> GetMovements(int id)
    {
        if (!_movements.ContainsKey(id))
            _movements[id] = new List<vesselMovement>();

        return _movements[id];
    }

        /** Place a vessel into the given movement mode. */
    [Server]
    private void SetVesselMovement(int id, vesselMovement movement)
    {
        RemoveVesselMovement(id);
        if (!movement)
            return;

        GetMovements(id).Add(movement);
    }

    /** Remove any current movement from a vessel. */
    [Server]
    private void RemoveVesselMovement(int id)
    {
        var movements = GetMovements(id);
        foreach (var movement in movements)
            Destroy(movement.gameObject);
        
        movements.Clear();
    }

    /** Remove all vessel movements. */
    [Server]
    private void Clear()
    {
        foreach (var id in _movements.Keys)
            RemoveVesselMovement(id);
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
    private static vesselMovement CreateVesselMovement(vesselMovement prefab)
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
