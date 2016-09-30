using UnityEngine;
using System.Collections;
using Meg.Networking;

public class SonarPingFacing : MonoBehaviour
{

    [Header("Components")]

    public SonarPing Ping;
    public Transform Transform;


    // Private Properties
    // ------------------------------------------------------------

    private vesselData.Vessel Vessel
    { get { return Ping.Vessel; } }

    private void Start()
    {
        if (!Transform)
            Transform = transform;
    }

    private void LateUpdate()
    {
        var movement = Vessel.Movement;
        if (!movement)
            return;

        var v = movement.Velocity;
        var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg - 90;
        Transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

}
