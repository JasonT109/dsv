using UnityEngine;
using System.Collections;
using Meg.Networking;
using UnityEngine.Networking;

/** Moves the vessel in a set direction at its current velocity. */

public class vesselSetVector : vesselMovement
{

    // Constants
    // ------------------------------------------------------------

    /** Smoothing time for actual speed. */
    public const float SpeedSmoothTime = 30;

    /** Smoothing time for actual heading . */
    public const float HeadingSmoothTime = 15;

    /** Smoothing time for actual dive angle. */
    public const float DiveAngleSmoothTime = 20;


    // Properties
    // ------------------------------------------------------------

    /** The vessel's current heading, in degrees (0 = North, 90 = East, etc.) */
    [SyncVar]
    public float Heading;

    /** The vessel's current dive angle, in degrees (0 = Level, -90 = up, 90 = down). */
    [SyncVar]
    public float DiveAngle;

    /** Speed that the vehicle should travel at. */
    [SyncVar]
    public float Speed;

    /** Maximum speed that the vehicle should travel at. */
    [SyncVar]
    public float MaxSpeed = 36;


    // Members
    // ------------------------------------------------------------

    /** Current actual heading. */
    private float _heading;

    /** Current actual speed. */
    private float _speed;

    /** Current actual dive angle. */
    private float _diveAngle;

    /** Smoothing velocity for heading. */
    private float _headingVelocity;

    /** Smoothing velocity for speed. */
    private float _speedVelocity;

    /** Smoothing velocity for dive angle. */
    private float _diveAngleVelocity;


    // Load / Save
    // ------------------------------------------------------------

    /** Save movement state to JSON. */
    public override JSONObject Save()
    {
        var json = base.Save();
        json.AddField("Heading", Heading);
        json.AddField("DiveAngle", DiveAngle);
        json.AddField("Speed", Speed);
        json.AddField("MaxSpeed", MaxSpeed);
        return json;
    }

    /** Load movement state from JSON. */
    public override void Load(JSONObject json, mapData mapData)
    {
        base.Load(json, mapData);
        json.GetField(ref Heading, "Heading");
        json.GetField(ref DiveAngle, "DiveAngle");
        json.GetField(ref Speed, "Speed");
        json.GetField(ref MaxSpeed, "MaxSpeed");
    }

    /** Return the movement save type. */
    protected override string GetSaveKey()
        { return vesselMovements.SetVectorType; }


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

        // Apply smoothing to actual speed, etc.
        var dt = GetDeltaTime();
        _speed = Mathf.SmoothDamp(_speed, Speed, ref _speedVelocity, SpeedSmoothTime * dt);
        _heading = Mathf.SmoothDampAngle(_heading, Heading, ref _headingVelocity, HeadingSmoothTime * dt);
        _diveAngle = Mathf.SmoothDamp(_diveAngle, DiveAngle, ref _diveAngleVelocity, DiveAngleSmoothTime * dt);

        // Determine change in position based on direction and velocity.
        var direction = Quaternion.Euler(0, 0, -_heading) * Quaternion.Euler(_diveAngle, 0, 0) * Vector3.up;
        var delta = direction * _speed * dt;

        // Convert the change in position into map-space.
        delta.x *= 0.001f;
        delta.y *= 0.001f;

        // Update the vessel's current state.
        SetVesselState(position + delta, direction * _speed, Speed);
    }

    /** Return the movement's speed. */
    public override float GetSpeed()
        { return Speed; }

    /** Set the movement's speed. */
    public override void SetSpeed(float value)
        { Speed = value; }

    /** Return the movement's maximum speed. */
    public override float GetMaxSpeed()
        { return MaxSpeed; }

    /** Set the movement's maximum speed. */
    public override void SetMaxSpeed(float value)
        { MaxSpeed = value; }


}
