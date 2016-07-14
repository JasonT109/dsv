using UnityEngine;
using System.Collections;
using Meg.Networking;

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

    /** Distance at which interception is considered complete. */
    public const float InterceptAchievedDistance = 10;

    /** Smoothing time for reported velocity. */
    public const float SpeedSmoothTime = 2;

    /** Magnitude of random noise to introduce to reported velocity. */
    public const float SpeedRandomness = 0.5f;

    
    // Properties
    // ------------------------------------------------------------

    /** Initial time to intercept (seconds). */
    public float TimeToIntercept = 60;


    // Members
    // ------------------------------------------------------------

    /** Current time to intercept (seconds). */
    private float _timeLeft;

    /** Currently reported velocity. */
    private float _speed;

    /** Smoothing for reported velocity. */
    private float _speedSmoothingVelocity;


    // Public Methods
    // ------------------------------------------------------------

    /** Configure the vessel movement. */
    public void Configure(mapData data, int vessel, bool active, float timeTointercept)
    {
        base.Configure(data, vessel, active);
        SetTimeToIntercept(timeTointercept);
    }

    /** Set the vessel's time to intercept. */
    public void SetTimeToIntercept(float value)
    {
        TimeToIntercept = value;
        _timeLeft = value;
        _speed = 0;
        _speedSmoothingVelocity = 0;
    }


    // Protected Methods
    // ------------------------------------------------------------

    /** Update the vessel's current state. */
    protected override void UpdateMovement()
    {
        // Update interception time.
        _timeLeft = Mathf.Max(0, _timeLeft - Time.deltaTime);

        // Get the vessel's current state.
        Vector3 position;
        float velocity;
        GetVesselState(out position, out velocity);

        // Get the interception point.
        Vector3 interceptPosition;
        float interceptVelocity;
        serverUtils.GetVesselData(InterceptVessel, out interceptPosition, out interceptVelocity);

        // Determine distance to target, taking map-space into account.
        var delta = (interceptPosition - position);
        delta.x *= 1000f;
        delta.y *= 1000f;
        var distance = delta.magnitude;

        // Convert delta into a direction.
        var direction = delta.normalized;

        // Determine if we've intercepted the target.
        var intercepted = _timeLeft <= 0 || distance < InterceptAchievedDistance;

        // Compute speed required to reach target at correct time.
        float requiredSpeed = 0;
        if (intercepted)
            requiredSpeed = 0;
        else
            requiredSpeed = Mathf.Clamp(distance / _timeLeft, 0, ActualTopSpeed);

        // Determine change in position based on direction and velocity.
        // Also convert back into map space.
        Vector3 dp = direction * requiredSpeed * Time.deltaTime;
        dp.x *= 0.001f;
        dp.y *= 0.001f;

        // Clamp reported velocity to the vessel's rated top-speed.
        // Add in some random noise when intercepting to make the speed look nice.
        var targetSpeed = Mathf.Clamp(requiredSpeed, 0, RatedTopSpeed);
        if (_timeLeft <= SpeedSmoothTime)
            targetSpeed = 0;
        else
            targetSpeed = Mathf.Max(0, targetSpeed + Random.Range(-SpeedRandomness, SpeedRandomness));

        // Smooth reported velocity over time.
        _speed = Mathf.SmoothDamp(_speed, targetSpeed, ref _speedSmoothingVelocity, SpeedSmoothTime);

        // Update the vessel's current state.
        SetVesselState(position + dp, direction * requiredSpeed, _speed);

        // Update due time to match interception time.
        var ts = System.TimeSpan.FromSeconds(_timeLeft);
        serverUtils.SetServerData("dueTimeHours", ts.Hours);
        serverUtils.SetServerData("dueTimeMins", ts.Minutes);
        serverUtils.SetServerData("dueTimeSecs", ts.Seconds);
    }

}
