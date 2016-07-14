using UnityEngine;
using System.Collections;
using Meg.Networking;

/** Moves the vessel in a set direction at its current velocity. */

public class vesselSetVector : vesselMovement
{

    // Properties
    // ------------------------------------------------------------

    /** The vessel's current heading, in degrees (0 = North, 90 = East, etc.) */
    public float Heading;

    /** The vessel's current dive angle, in degrees (0 = Level, -90 = up, 90 = down). */
    public float DiveAngle;

    /** Speed that the vehicle should travel at. */
    public float Speed = 0;

    /** The vessel's current direction - XY for heading, Z for depth (normalized). */
    public Vector3 Direction
    { get { return Quaternion.Euler(0, 0, -Heading) * Quaternion.Euler(DiveAngle, 0, 0) * Vector3.up; } }


    // Public Methods
    // ------------------------------------------------------------

    /** Configure the vessel movement. */
    public void Configure(mapData data, int vessel, bool active)
    {
        base.Configure(data, vessel, active);
	}


    // Protected Methods
    // ------------------------------------------------------------

    /** Update the vessel's current state. */
    protected override void UpdateMovement()
    {
        // Get the vessel's current state.
        Vector3 position;
        float velocity;
        GetVesselState(out position, out velocity);

        // Determine change in position based on direction and velocity.
        Vector3 delta = Direction * Speed * Time.deltaTime;

        // Convert the change in position into map-space.
        delta.x *= 0.001f;
        delta.y *= 0.001f;

        // Update the vessel's current state.
        SetVesselState(position + delta, Direction * Speed, Speed);
    }



}
