using UnityEngine;
using System.Collections;
using Meg.Networking;
using UnityEngine.Networking;

/** Base class that moves a vessel over time. */

public abstract class vesselMovement : MonoBehaviour // NetworkBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Synchronization")]

    /** Index of the vessel within map data. */
    // [SyncVar]
    public int Vessel;

    /** Whether interception is active. */
    // [SyncVar]
    public bool Active;

    /** Vessel's current velocity. */
    // [SyncVar]
    public Vector3 Velocity;


    // Computed Properties
    // ------------------------------------------------------------

    /** The vessel movement manager. */
    public vesselMovements Movements
        { get { return serverUtils.GetVesselMovements(); } }

    /** The map that this vessel moves within. */
    public mapData MapData
        { get { return Movements.MapData; } }


    // Unity Methods
    // ------------------------------------------------------------

    /** Called when movement is enabled. */
    protected virtual void OnEnable()
    {
        if (Movements)
            Movements.Register(this);
    }

    /** Called when movement is disabled. */
    protected virtual void OnDisable()
    {
        if (Movements)
            Movements.Unregister(this);
    }

    /** Update the vessel movement module. */
    // [ServerCallback]
    protected virtual void Update()
    {
        // Only run movement logic on the server.
        if (!isServer)
            return;

        // Reset velocity.
        Velocity = Vector3.zero;

        // Update the vessel's movement.
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
        Configure(Vessel, Active);
    }

    /** Return the movement save type. */
    protected abstract string GetSaveKey();


    // Protected Methods
    // ------------------------------------------------------------

    // TODO: Remove this when reinstating NetworkBehaviour.
    protected bool isServer
    { get { return true; } }

    /** Update the vessel's movement. */
    // [Server]
    protected abstract void UpdateMovement();

    /** Configure the vessel's movement. */
    // [Server]
    public virtual void Configure(int vessel, bool active)
    {
        Vessel = vessel;
        Active = active;

        // var parent = MapData.transform;
        // transform.position = parent.position;
        // transform.rotation = parent.rotation;
        // transform.parent = parent;
    }

    /** Sets the vessel's state (position + velocity). */
    // [Server]
    protected void SetVesselState(Vector3 position, Vector3 velocity, float reportedSpeed)
    {
        Velocity = velocity;

        if (!Active)
            return;

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
