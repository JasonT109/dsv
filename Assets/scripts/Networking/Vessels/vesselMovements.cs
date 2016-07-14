using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/** 
 * Manages the movements of non-player vessels.
 * Each vessel can have a single associated vesselMovement module.
 */

public class vesselMovements : MonoBehaviour
{

    // Components
    // ------------------------------------------------------------

    /** The map data component. */
    public mapData MapData
    { get; private set; }


    // Properties
    // ------------------------------------------------------------

    /** Prefab that is used when putting vessels into a holding pattern. */
    public vesselHoldingPattern HoldingPatternPrefab;

    /** Prefab that is used when putting vessels on a set vector course. */
    public vesselSetVector SetVectorPrefab;

    /** Prefab that is used when putting vessels on an interception course. */
    public vesselIntercept InterceptPrefab;

    /** Whether movement simulation is active. */
    public bool Active
    { get; private set; }


    // Members
    // ------------------------------------------------------------

    /** The set of current vessel movement modes. */
    private Dictionary<int, vesselMovement> current = new Dictionary<int, vesselMovement>();


    // Unity Methods
    // ------------------------------------------------------------

    void OnEnable()
    {
        MapData = GetComponent<mapData>();
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Place the given vessel into a holding pattern. */
    public void SetHoldingPattern(int vessel)
    {
        var holdingPattern = Instantiate(HoldingPatternPrefab);
        holdingPattern.Configure(MapData, vessel, Active);
        SetVesselMovement(vessel, holdingPattern);
    }

    /** Place the given vessel on a set vector course. */
    public void SetVector(int vessel)
    {
        var setVector = Instantiate(SetVectorPrefab);
        setVector.Configure(MapData, vessel, Active);
        SetVesselMovement(vessel, setVector);
    }

    /** Place the given vessel on an interception course. */
    public void SetIntercept(int vessel)
    {
        var intercept = Instantiate(InterceptPrefab);
        intercept.Configure(MapData, vessel, Active);
        SetVesselMovement(vessel, intercept);
    }

    /** Removes any active movement commands from the given vessel. */
    public void SetNone(int vessel)
    {
        RemoveVesselMovement(vessel);
    }

    /** Return the vessel's current movement mode (if any). */
    public vesselMovement GetVesselMovement(int vessel)
    {
        vesselMovement movement = null;
        current.TryGetValue(vessel, out movement);
        return movement;
    }

    /** Set all movements to active or inactive. */
    public void SetActive(bool value)
    {
        Active = value;
        foreach (var movement in current.Values)
            movement.Active = value;
    }

    /** Whether movement simulation is active. */
    public bool IsActive()
    {
        return Active;
    }

    /** Save movement state to JSON. */
    public JSONObject Save()
    {
        var json = new JSONObject();
        json.AddField("Active", Active);

        var movementsJson = new JSONObject(JSONObject.Type.ARRAY);
        var sorted = current.Values.OrderBy(x => x.Vessel);
        foreach (var movement in sorted)
            movementsJson.Add(movement.Save());

        json.AddField("Movements", movementsJson);

        return json;
    }

    /** Load movement state from JSON. */
    public void Load(JSONObject json)
    {
        // Clear out any existing movements.
        Clear();

        // Load in movements from data.
        JSONObject movementsJson = json.GetField("Movements");
        for (var i = 0; i < movementsJson.Count; i++)
        {
            var movementJson = movementsJson[i];
            string type = null;
            movementJson.GetField(ref type, "Type");
            var movement = CreateVesselMovement(type);
            movement.Load(movementJson, MapData);
            SetVesselMovement(movement.Vessel, movement);
        }

        // Load simulation active state.
        bool active = true;
        json.GetField(ref active, "Active");
        SetActive(active);
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Place a vessel into the given movement mode. */
    private void SetVesselMovement(int vessel, vesselMovement movement)
    {
        RemoveVesselMovement(vessel);
        current[vessel] = movement;
    }

    /** Remove any current movement from a vessel. */
    private void RemoveVesselMovement(int vessel)
    {
        if (!current.ContainsKey(vessel))
            return;

        Destroy(current[vessel].gameObject);
        current.Remove(vessel);
    }

    /** Remove all vessel movements. */
    private void Clear()
    {
        foreach (var movement in current.Values)
            Destroy(movement.gameObject);

        current.Clear();
    }

    /** Create a movement given a type code. */
    private vesselMovement CreateVesselMovement(string type)
    {
        switch (type)
        {
            case "Holding":
                return Instantiate(HoldingPatternPrefab);
            case "Intercept":
                return Instantiate(InterceptPrefab);
            case "SetVector":
                return Instantiate(SetVectorPrefab);
            default:
                return null;
        }
    }

}
