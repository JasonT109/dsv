using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class vesselHoldingPattern : vesselMovement
{

    // Constants
    // ------------------------------------------------------------

    /** Minimum holding pattern period, in seconds. */
    public const float MinPeriod = 1;


    // Properties
    // ------------------------------------------------------------

    /** Period of the holding pattern, in seconds (how quickly vessel repeats the pattern). */
    [SyncVar]
    public float Period = 60;

    /** Magnitude of depth change as a fraction. */
    [SyncVar]
    public float DepthFraction = 0.1f;

    /** Speed that the vehicle should travel at. */
    [SyncVar]
    public float Speed = 0;


    // Members
    // ------------------------------------------------------------

    /** Simulation time. */
    private float _time;


    // Load / Save
    // ------------------------------------------------------------

    /** Save movement state to JSON. */
    public override JSONObject Save()
    {
        var json = base.Save();
        json.AddField("Period", Period);
        json.AddField("DepthFraction", DepthFraction);
        json.AddField("Speed", Speed);

        return json;
    }

    /** Load movement state from JSON. */
    public override void Load(JSONObject json, mapData mapData)
    {
        base.Load(json, mapData);

        json.GetField(ref Period, "Period");
        json.GetField(ref DepthFraction, "DepthFraction");
        json.GetField(ref Speed, "Speed");
    }

    /** Return the movement save type. */
    protected override string GetSaveKey()
        { return vesselMovements.HoldingType; }


    // Protected Methods
    // ------------------------------------------------------------

    /** Update the vessel's current state. */
    [Server]
    protected override void UpdateMovement()
    {
        // Get the vessel's current state.
        Vector3 position;
        float velocity;
        GetVesselState(out position, out velocity);

        // Set vessel's direction based on time.
        var f = _time / Mathf.Max(Period, MinPeriod);
        var dx = Mathf.Cos(f * (Mathf.PI * 2));
        var dy = Mathf.Sin(f * (Mathf.PI * 2));
        var dz = Mathf.Sin(f * (Mathf.PI * 2)) * DepthFraction;
        var direction = new Vector3(dx, dy, dz).normalized;

        // Determine change in position based on direction and velocity.
        var delta = direction * Speed * GetDeltaTime();

        // Convert the change in position into map-space.
        delta.x *= 0.001f;
        delta.y *= 0.001f;

        // Update the vessel's current state.
        SetVesselState(position + delta, direction * Speed, Speed);

        // Update time.
        if (Active)
            _time += GetDeltaTime();
    }

    /** Return the movement's speed. */
    public override float GetSpeed()
        { return Speed;}

    /** Set the movement's speed. */
    public override  void SetSpeed(float value)
        { Speed = value; }

}
