using UnityEngine;
using System.Collections;
using Meg.Networking;

public class vesselHoldingPattern : vesselMovement
{

    // Constants
    // ------------------------------------------------------------

    /** Minimum holding pattern period, in seconds. */
    public const float MinPeriod = 1;


    // Properties
    // ------------------------------------------------------------

    /** Period of the holding pattern, in seconds (how quickly vessel repeats the pattern). */
    public float Period = 60;

    /** Magnitude of depth change as a fraction. */
    public float DepthFraction = 0.1f;

    /** Speed that the vehicle should travel at. */
    public float Speed = 0;


    // Members
    // ------------------------------------------------------------

    /** Time at which holding pattern started. */
    private float _startTime;


    // Public Methods
    // ------------------------------------------------------------

    /** Configure the vessel movement. */
    public override void Configure(mapData data, int vessel, bool active)
    {
        base.Configure(data, vessel, active);
        _startTime = Time.time;
    }


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


    // Protected Methods
    // ------------------------------------------------------------

    /** Return the movement save type. */
    protected override string GetSaveKey()
    { return "Holding"; }

    /** Update the vessel's current state. */
    protected override void UpdateMovement()
    {
        // Get the vessel's current state.
        Vector3 position;
        float velocity;
        GetVesselState(out position, out velocity);

        // Set vessel's direction based on time.
        var t = Time.time - _startTime;
        var f = t / Mathf.Max(Period, MinPeriod);
        var dx = Mathf.Cos(f * (Mathf.PI * 2));
        var dy = Mathf.Sin(f * (Mathf.PI * 2));
        var dz = Mathf.Sin(f * (Mathf.PI * 2)) * DepthFraction;
        var direction = new Vector3(dx, dy, dz).normalized;

        // Determine change in position based on direction and velocity.
        Vector3 delta = direction * Speed * Time.deltaTime;

        // Convert the change in position into map-space.
        delta.x *= 0.001f;
        delta.y *= 0.001f;

        // Update the vessel's current state.
        SetVesselState(position + delta, direction * Speed, Speed);
    }

}
