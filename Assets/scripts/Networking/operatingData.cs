using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class operatingData : NetworkBehaviour
{

    /** Tow winch load (kg). */
    [SyncVar]
    public float towWinchLoad;

    /** Hydraulic system temperature (°c). */
    [SyncVar]
    public float hydraulicTemp = 50;

    /** Hydraulic system pressure (psi). */
    [SyncVar]
    public float hydraulicPressure = 95;

    /** Ballast air pressure (psi). */
    [SyncVar]
    public float ballastPressure = 1150;

    /** Variable ballast temp (°c). */
    [SyncVar]
    public float variableBallastTemp = 10;

    /** Variable ballast pressure (psi). */
    [SyncVar]
    public float variableBallastPressure = 225;

    /** Communications signal strength (0..100%). */
    [SyncVar]
    public float commsSignalStrength = 100;

    /** Amount of power diverted from systems to thrusters (0..100%). */
    [SyncVar]
    public float divertPowerToThrusters = 0;

}
