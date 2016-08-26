using UnityEngine;
using System.Collections;
using Meg.Networking;
using UnityEngine.Networking;

/** Base class that moves a vessel over time. */

public abstract class vesselMovement : NetworkBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Synchronization")]

    /** Index of the vessel within map data. */
    [SyncVar]
    public int Vessel;

    /** Vessel's current velocity (map space). */
    [SyncVar]
    public Vector3 Velocity;

    /** Vessel's current velocity (world space). */
    public Vector3 WorldVelocity
        { get { return new Vector3(Velocity.x, -Velocity.z, Velocity.y); } }

    /** Whether interception is active. */
    public bool Active
        { get { return Movements.Active; } }

    /** The type key for this movement. */
    public string Type
        { get { return GetSaveKey(); } }


    // Computed Properties
    // ------------------------------------------------------------

    /** The vessel movement manager. */
    public vesselMovements Movements
    {
        get
        {
            if (!_movements)
                _movements = serverUtils.VesselMovements;

            return _movements;
        }
    }

    /** The map that this vessel moves within. */
    public mapData MapData
        { get { return Movements.MapData; } }


    // Members
    // ------------------------------------------------------------

    /** The vessel movements manager. */
    private vesselMovements _movements;

    /** Whether movement has been spawned on clients. */
    private bool _spawned;


    // Unity Methods
    // ------------------------------------------------------------

    /** Called when movement starts up on a client. */
    public override void OnStartClient()
    {
        base.OnStartClient();
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
    protected virtual void FixedUpdate()
    {
        // Only run movement logic on the server.
        if (!isServer)
            return;

        // Reset velocity.
        Velocity = Vector3.zero;

        // Update the vessel's movement.
        UpdateMovement();
    }

    /** Returns time delta increment to use when updating movments. */
    protected float GetDeltaTime()
        { return Time.fixedDeltaTime; }


    // Load / Save
    // ------------------------------------------------------------

    /** Save movement state to JSON. */
    public virtual JSONObject Save()
    {
        var json = new JSONObject();
        json.AddField("Vessel", Vessel);
        json.AddField("Type", GetSaveKey());

        return json;
    }

    /** Load movement state from JSON. */
    public virtual void Load(JSONObject json, mapData mapData)
    {
        json.GetField(ref Vessel, "Vessel");
        Configure(Vessel, Active);
    }

    /** Return the movement save type. */
    protected abstract string GetSaveKey();


    // Networking
    // ------------------------------------------------------------

    /** Post this movement's local state up to the server. */
    public void PostState()
        { serverUtils.PostVesselMovementState(Save()); }


    // Protected Methods
    // ------------------------------------------------------------

    /** Update the vessel's movement. */
    [Server]
    protected abstract void UpdateMovement();

    /** Configure the vessel's movement. */
    [Server]
    public virtual void Configure(int vessel, bool active)
    {
        Vessel = vessel;
        EnsureSpawned();
    }

    /** Ensure that this movement is spawned on remote clients. */
    protected void EnsureSpawned()
    {
        if (_spawned)
            return;

        NetworkServer.Spawn(gameObject);
        _spawned = true;
    }

    /** Sets the vessel's state (position + velocity). */
    [Server]
    protected void SetVesselState(Vector3 position, Vector3 velocity, float reportedSpeed)
    {
        Velocity = velocity;

        if (!Active)
            return;

        serverUtils.SetVesselData(Vessel, position, reportedSpeed);

        // If we're controlling the player vessel, update its world velocity.
        // Supplied velocity uses map axes, so translate it into world space.
        if (Vessel == serverUtils.GetPlayerVessel())
            serverUtils.SetPlayerWorldVelocity(WorldVelocity);
    }

    /** Returns the vessel's state (position + velocity). */
    protected void GetVesselState(out Vector3 position, out float velocity)
        { serverUtils.GetVesselData(Vessel, out position, out velocity); }

    /** Return the movement's speed. */
    public abstract float GetSpeed();

    /** Set the movement's speed. */
    public abstract void SetSpeed(float value);

    /** Return the movement's maximum speed. */
    public abstract float GetMaxSpeed();

    /** Set the movement's maximum speed. */
    public abstract void SetMaxSpeed(float value);

}
