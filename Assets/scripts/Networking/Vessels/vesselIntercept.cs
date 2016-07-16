using UnityEngine;
using System.Collections;
using Meg.Networking;
using UnityEngine.Networking;

public class vesselIntercept : vesselMovement
{

    // Constants
    // ------------------------------------------------------------

    /** The vessel we're intercepting. */
    public const int InterceptVessel = 6;

    /** The nominal top speed of this vessel (m/s).  Will never set reported velocity higher than this. */
    public const float RatedTopSpeed = 36;

    /** The actual top speed of this vessel (m/s).  Will never move vessel's actual position faster than this. */
    public const float ActualTopSpeed = 500;

    /** Smoothing time for reported velocity. */
    public const float SpeedSmoothTime = 2;

    /** Magnitude of random noise to introduce to reported velocity. */
    public const float SpeedRandomness = 0.5f;

    /** Distance at which interception is considered complete. */
    public const float InterceptAchievedDistance = 10;

    /** Extent to which target's velocity should be taken into account when predicting interception point. */
    public const float InterceptPredictionFactor = 1.0f;


    // Properties
    // ------------------------------------------------------------

    /** Whether to automatically adjust speed to ensure correct arrival time. */
    [SyncVar]
    public bool AutoSpeed = true;

    /** Speed of interception. */
    [SyncVar]
    public float Speed;

    /** Returns the current time to intercept. */
    private float TimeToIntercept
    { get { return serverUtils.GetVesselMovements().TimeToIntercept; } }



    // Members
    // ------------------------------------------------------------

    /** Smoothing for reported velocity. */
    private float _speedSmoothingVelocity;


    // Load / Save
    // ------------------------------------------------------------

    /** Save movement state to JSON. */
    public override JSONObject Save()
    {
        var json = base.Save();
        json.AddField("AutoSpeed", AutoSpeed);
        json.AddField("Speed", Speed);
        return json;
    }

    /** Load movement state from JSON. */
    public override void Load(JSONObject json, mapData mapData)
    {
        base.Load(json, mapData);
        json.GetField(ref AutoSpeed, "AutoSpeed");
        json.GetField(ref Speed, "Speed");
    }

    /** Return the movement save type. */
    protected override string GetSaveKey()
        { return "Intercept"; }


    // Protected Methods
    // ------------------------------------------------------------

    /** Update the vessel's current state. */
    [Server]
    protected override void UpdateMovement()
    {
        // Determine if we've intercepted the target.
        if (TimeToIntercept <= 0)
            return;

        // Get the vessel's current state.
        Vector3 position;
        float velocity;
        GetVesselState(out position, out velocity);

        // Get the interception point.
        Vector3 interceptPosition;
        float interceptVelocity;
        serverUtils.GetVesselData(InterceptVessel, out interceptPosition, out interceptVelocity);

        // Adjust intercept point according to target's velocity and time to intercept.
        var movement = serverUtils.GetVesselMovements().GetVesselMovement(InterceptVessel);
        if (movement)
        {
            var predicted = movement.Velocity * (TimeToIntercept * InterceptPredictionFactor);
            predicted.x *= 0.001f;
            predicted.y *= 0.001f;
            interceptPosition += predicted;
        }

        // Determine distance to target, taking map-space into account.
        var delta = (interceptPosition - position);
        delta.x *= 1000f;
        delta.y *= 1000f;
        var distance = delta.magnitude;

        // Convert delta into a direction.
        var direction = delta.normalized;

        // Compute speed required to reach target at correct time.
        var requiredSpeed = AutoSpeed 
            ? Mathf.Clamp(distance / TimeToIntercept, 0, ActualTopSpeed)
            : Speed;

        // Determine change in position based on direction and velocity.
        // Also convert back into map space.
        var dp = direction * requiredSpeed * Time.fixedDeltaTime;
        dp.x *= 0.001f;
        dp.y *= 0.001f;

        // Clamp reported velocity to the vessel's rated top-speed.
        // Add in some random noise when intercepting to make the speed look nice.
        var targetSpeed = requiredSpeed;
        if (TimeToIntercept <= SpeedSmoothTime)
            targetSpeed = 0;
        else if (!AutoSpeed && distance <= InterceptAchievedDistance)
            targetSpeed = 0;
        else if (Active)
            targetSpeed = Mathf.Max(0, targetSpeed + Random.Range(-SpeedRandomness, SpeedRandomness));

        // Smooth reported velocity over time.
        if (Active && AutoSpeed)
            Speed = Mathf.SmoothDamp(Speed, targetSpeed, ref _speedSmoothingVelocity, SpeedSmoothTime);
        else
            Speed = targetSpeed;

        // Update the vessel's current state.
        SetVesselState(position + dp, direction * requiredSpeed, Mathf.Clamp(Speed, 0, RatedTopSpeed));
    }

}
