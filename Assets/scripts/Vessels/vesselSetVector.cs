using UnityEngine;
using System.Collections;
using Meg.Networking;
using UnityEngine.Networking;

/** Moves the vessel in a set direction at its current velocity. */

public class vesselSetVector : vesselMovement
{

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


    // Computed Properties
    // ------------------------------------------------------------

    /** The vessel's current direction - XY for heading, Z for depth (normalized). */
    public Vector3 Direction
        { get { return Quaternion.Euler(0, 0, -Heading) * Quaternion.Euler(DiveAngle, 0, 0) * Vector3.up; } }


    // Load / Save
    // ------------------------------------------------------------

    /** Save movement state to JSON. */
    public override JSONObject Save()
    {
        var json = base.Save();
        json.AddField("Heading", Heading);
        json.AddField("DiveAngle", DiveAngle);
        json.AddField("Speed", Speed);
        return json;
    }

    /** Load movement state from JSON. */
    public override void Load(JSONObject json, mapData mapData)
    {
        base.Load(json, mapData);
        json.GetField(ref Heading, "Heading");
        json.GetField(ref DiveAngle, "DiveAngle");
        json.GetField(ref Speed, "Speed");
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

        // Determine change in position based on direction and velocity.
        var delta = Direction * Speed * GetDeltaTime();

        // Convert the change in position into map-space.
        delta.x *= 0.001f;
        delta.y *= 0.001f;

        // Update the vessel's current state.
        SetVesselState(position + delta, Direction * Speed, Speed);
    }

}
