using UnityEngine;
using System.Collections;
using Meg.Networking;
using UnityEngine.Networking;

public class cabinData : NetworkBehaviour
{

    // Synchronization
    // ------------------------------------------------------------

    [Header("Synchronization")]

    /** Cabin humidity (%). */
    [SyncVar]
    public float cabinHumidity = 42.9f;

    /** Cabin oxygen percentage (tops out to nominal 20.9% at 5% reserves). */
    [SyncVar]
    public float cabinOxygen = 20.9f;

    /** Cabin pressure (in bar). */
    [SyncVar]
    public float cabinPressure = 1.024f;

    /** Cabin temperature (degrees c). */
    [SyncVar]
    public float cabinTemp = 14.3f;

    /** CO2% in cabin atmosphere. */
    [SyncVar]
    public float Co2 = 1.05f;

    /** CO2% in atmosphere after leaving the scrubber. */
    [SyncVar]
    public float scrubbedCo2 = 1.05f;

    /** Humidity leaving the scrubber (%). */
    [SyncVar]
    public float scrubbedHumidity = 42.9f;

    /** Scrubbed oxygen percentage (tops out to nominal 20.9% at 5% reserves). */
    [SyncVar]
    public float scrubbedOxygen = 21.8f;

}
