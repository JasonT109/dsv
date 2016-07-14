using UnityEngine;
using System.Collections.Generic;

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
    public void SetHoldingPattern(int vessel, bool active)
    {
        var holdingPattern = Instantiate(HoldingPatternPrefab);
        holdingPattern.Configure(MapData, vessel, active);
        SetVesselMovement(vessel, holdingPattern);
    }

    /** Place the given vessel on a set vector course. */
    public void SetVector(int vessel, bool active)
    {
        var setVector = Instantiate(SetVectorPrefab);
        setVector.Configure(MapData, vessel, active);
        SetVesselMovement(vessel, setVector);
    }

    /** Place the given vessel on an interception course. */
    public void SetIntercept(int vessel, bool active, float timeToIntercept)
    {
        var intercept = Instantiate(InterceptPrefab);
        intercept.Configure(MapData, vessel, active, timeToIntercept);
        SetVesselMovement(vessel, intercept);
    }

    /** Removes any active movement commands from the given vessel. */
    public void SetNone(int vessel)
    {
        RemoveVesselMovement(vessel);
    }

    /** Determines whether the vessel is in a holding pattern. */
    public bool IsHoldingPattern(int vessel)
    {
        return GetVesselMovement(vessel) is vesselHoldingPattern;
    }

    /** Determines whether the vessel is on a set vector course. */
    public bool IsSetVector(int vessel)
    {
        return GetVesselMovement(vessel) is vesselSetVector;
    }

    /** Determines whether the vessel is on an interception course. */
    public bool IsIntercept(int vessel)
    {
        return GetVesselMovement(vessel) is vesselIntercept;
    }

    /** Determines whether the vessel has any current movement plan. */
    public bool IsMoving(int vessel)
    {
        return GetVesselMovement(vessel) != null;
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
        foreach (var movement in current.Values)
            movement.Active = value;
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

}
