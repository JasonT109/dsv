using UnityEngine;
using System.Collections;
using Meg.Networking;
using UnityEngine.Networking;

public class vesselPursue : vesselMovement
{

    // Constants
    // ------------------------------------------------------------

    /** The nominal top speed of this vessel (m/s).  Will never set reported velocity higher than this. */
    public const float RatedTopSpeed = 36;

    /** Distance at which pursuit is considered complete. */
    public const float InterceptAchievedDistance = 10;


    // Properties
    // ------------------------------------------------------------

    /** The vessel we're pursuing. */
    [SyncVar]
    public int TargetVessel = 1;

    /** Speed of pursuit. */
    [SyncVar]
    public float Speed;


    // Load / Save
    // ------------------------------------------------------------

    /** Save movement state to JSON. */
    public override JSONObject Save()
    {
        var json = base.Save();
        json.AddField("Speed", Speed);
        json.AddField("TargetVessel", TargetVessel);
        return json;
    }

    /** Load movement state from JSON. */
    public override void Load(JSONObject json, mapData mapData)
    {
        base.Load(json, mapData);
        json.GetField(ref Speed, "Speed");
        json.GetField(ref TargetVessel, "TargetVessel");
    }

    /** Return the movement save type. */
    protected override string GetSaveKey()
        { return vesselMovements.PursueType; }


    // Protected Methods
    // ------------------------------------------------------------

    /** Update the vessel's current state. */
    // [Server]
    protected override void UpdateMovement()
    {
        // Check if target vessel is valid.
        if (TargetVessel == 0)
            return;

        // Get the vessel's current state.
        Vector3 position;
        float velocity;
        GetVesselState(out position, out velocity);

        // Get the interception point.
        Vector3 interceptPosition;
        float interceptVelocity;
        serverUtils.GetVesselData(TargetVessel, out interceptPosition, out interceptVelocity);

        // Determine distance to target, taking map-space into account.
        var delta = (interceptPosition - position);
        delta.x *= 1000f;
        delta.y *= 1000f;
        var distance = delta.magnitude;

        // Convert delta into a direction.
        var direction = delta.normalized;

        // Add in some random noise when intercepting to make the speed look nice.
        if (distance <= InterceptAchievedDistance)
            Speed = 0;

        // Determine change in position based on direction and velocity.
        // Also convert back into map space.
        var dp = direction * Speed * GetDeltaTime();
        dp.x *= 0.001f;
        dp.y *= 0.001f;

        // Update the vessel's current state.
        SetVesselState(position + dp, direction * Speed, Mathf.Clamp(Speed, 0, RatedTopSpeed));
    }

    /** Return the movement's speed. */
    public override float GetSpeed()
        { return Speed; }

    /** Set the movement's speed. */
    public override void SetSpeed(float value)
        { Speed = value; }


}
