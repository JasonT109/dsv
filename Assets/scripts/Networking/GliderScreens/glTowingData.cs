using UnityEngine;
using UnityEngine.Networking;

/** Glider towing target position and behaviour data. */
public class glTowingData : NetworkBehaviour
{
    [SyncVar]
    public float towTargetX;

    [SyncVar]
    public float towTargetY;

    [SyncVar]
    public float towTargetSpeed = 0.6f;

    [SyncVar]
    public bool towTargetVisible = true;

    [SyncVar]
    public float towFiringPressure;

    [SyncVar]
    public float towFiringPower;

    //Tow firing status: 0 = ready, 1 = acquiring, 2 = locked, 3 = fired
    [SyncVar]
    public float towFiringStatus;

    [SyncVar]
    public float towLineSpeed;

    [SyncVar]
    public float towLineLength = 1000;

    [SyncVar]
    public float towLineRemaining = 1000;

    [SyncVar]
    public float towTargetDistance = 20;
}