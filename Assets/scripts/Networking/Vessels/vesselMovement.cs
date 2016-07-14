using UnityEngine;
using System.Collections;
using Meg.Networking;

/** Base class that moves a vessel over time. */

public abstract class vesselMovement : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    /** Index of the vessel within map data. */
    public int Vessel;

    /** The map that this vessel moves within. */
    public mapData MapData
    { get; private set; }

    /** Whether interception is active. */
    public bool Active;


    // Unity Methods
    // ------------------------------------------------------------

    /** Called when movement is disabled. */
    protected virtual void OnDisable()
    {
        // Re-enable player input.
        // if (Vessel == MapData.playerVessel)
        //    serverUtils.SetServerBool("disableInput", false);
    }

    /** Update the vessel movement module. */
    protected virtual void Update()
    {
        // Update the vessel's movement.
        if (Active)
            UpdateMovement();
    }


    // Protected Methods
    // ------------------------------------------------------------

    /** Update the vessel's movement. */
    protected abstract void UpdateMovement();

    /** Configure the vessel's movement. */
    protected void Configure(mapData data, int vessel, bool active)
    {
        var parent = data.transform;
        transform.position = parent.position;
        transform.rotation = parent.rotation;
        transform.parent = parent;

        MapData = data;
        Vessel = vessel;
        Active = active;

        // Disable player input if needed.
        // if (Vessel == MapData.playerVessel)
        //    serverUtils.SetServerBool("disableInput", true);
    }

    /** Sets the vessel's state (position + velocity). */
    protected void SetVesselState(Vector3 position, Vector3 velocity, float reportedSpeed)
    {
        serverUtils.SetVesselData(Vessel, position, reportedSpeed);

        // If we're controlling the player vessel, update its state directly.
        // TODO: Make sure the player vessel orients correctly in this case.
        // if (Vessel == MapData.playerVessel)
        //    serverUtils.SetPlayerVesselState(position, velocity);
    }

    /** Returns the vessel's state (position + velocity). */
    protected void GetVesselState(out Vector3 position, out float velocity)
    {
        serverUtils.GetVesselData(Vessel, out position, out velocity);
    }

}
