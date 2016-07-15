using UnityEngine;
using System.Collections;
using Meg.Networking;

/** Base class that moves a vessel over time. */

public abstract class vesselMovement : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    /** The map that this vessel moves within. */
    public mapData MapData
    { get; private set; }

    /** Index of the vessel within map data. */
    public int Vessel;

    /** Whether interception is active. */
    public bool Active;

    /** Vessel's current velocity. */
    public Vector3 Velocity
    { get; private set; }


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
        // Reset velocity.
        Velocity = Vector3.zero;

        // Update the vessel's movement.
        if (Active)
            UpdateMovement();
    }


    // Load / Save
    // ------------------------------------------------------------

    /** Save movement state to JSON. */
    public virtual JSONObject Save()
    {
        var json = new JSONObject();
        json.AddField("Vessel", Vessel);
        json.AddField("Active", Active);
        json.AddField("Type", GetSaveKey());
        return json;
    }

    /** Load movement state from JSON. */
    public virtual void Load(JSONObject json, mapData mapData)
    {
        json.GetField(ref Vessel, "Vessel");
        json.GetField(ref Active, "Active");
        Configure(mapData, Vessel, Active);
    }


    // Protected Methods
    // ------------------------------------------------------------

    /** Return the movement save type. */
    protected abstract string GetSaveKey();

    /** Update the vessel's movement. */
    protected abstract void UpdateMovement();

    /** Configure the vessel's movement. */
    public virtual void Configure(mapData data, int vessel, bool active)
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
        Velocity = velocity;

        serverUtils.SetVesselData(Vessel, position, reportedSpeed);

        // If we're controlling the player vessel, update its world velocity.
        // Supplied velocity uses map axes, so translate it into world space.
        var worldVelocity = new Vector3(velocity.x, -velocity.z, velocity.y);
        if (Vessel == MapData.playerVessel)
            serverUtils.SetPlayerWorldVelocity(worldVelocity);
    }

    /** Returns the vessel's state (position + velocity). */
    protected void GetVesselState(out Vector3 position, out float velocity)
    {
        serverUtils.GetVesselData(Vessel, out position, out velocity);
    }

}
