using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class oxygenData : NetworkBehaviour
{
    /** Oxygen reserve %. */
    [SyncVar]
    public float oxygen;

    /** CO2% in cabin atmosphere. */
    [SyncVar]
    public float Co2 = 1.05f;

    /** Oxygen tanks. */
    [SyncVar]
    public float oxygenTank1;
    [SyncVar]
    public float oxygenTank2;
    [SyncVar]
    public float oxygenTank3;
    [SyncVar]
    public float oxygenTank4;
    [SyncVar]
    public float oxygenTank5;
    [SyncVar]
    public float oxygenTank6;
    [SyncVar]
    public float oxygenTank7;

    /** Cabin pressure (in bar). */
    [SyncVar]
    public float cabinPressure = 1.0f;

    /** Cabin temperature (degrees c). */
    [SyncVar]
    public float cabinTemp = 14.3f;


    /** Oxygen flow (liters / minute, tops out to nominal 22 lpm at 5% reserves). */
    public float oxygenFlow
    { get { return Mathf.Clamp01(oxygen * 0.2f) * 22; } }

    /** Cabin oxygen percentage (tops out to nominal 20.9% at 5% reserves). */
    public float cabinOxygen
    { get { return Mathf.Clamp01(oxygen * 0.2f) * 20.9f; } }

    /** CO2 (parts per million). */
    public float Co2Ppm
    { get { return Co2 * Conversions.PercentToPartsPerMillion; } }

    /** Cabin pressure, pounds / square inch. */
    public float cabinPressurePsi
    { get { return cabinPressure * Conversions.BarToPsi; } }

}
