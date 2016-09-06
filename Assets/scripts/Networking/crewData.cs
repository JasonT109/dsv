using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Meg.Networking;

public class crewData : NetworkBehaviour
{
    [SyncVar]
    public float crewHeartRate1 = 86f;
    [SyncVar]
    public float crewHeartRate2 = 86f;
    [SyncVar]
    public float crewHeartRate3 = 86f;
    [SyncVar]
    public float crewHeartRate4 = 86f;
    [SyncVar]
    public float crewHeartRate5 = 86f;
    [SyncVar]
    public float crewHeartRate6 = 86f;

    [SyncVar]
    public float crewBodyTemp1 = 36.5f;
    [SyncVar]
    public float crewBodyTemp2 = 36.5f;
    [SyncVar]
    public float crewBodyTemp3 = 36.5f;
    [SyncVar]
    public float crewBodyTemp4 = 36.5f;
    [SyncVar]
    public float crewBodyTemp5 = 36.5f;
    [SyncVar]
    public float crewBodyTemp6 = 36.5f;

    /** Extra information for Medical bay (MMB). */
    [SyncVar]
    public float crewHeartStrengthMin1 = 0f;
    [SyncVar]
    public float crewHeartStrengthMax1 = 100f;
    [SyncVar]
    public float crewRespirationRate1 = 20f;
    [SyncVar]
    public float crewETCO2Min1 = 0f;
    [SyncVar]
    public float crewETCO2Max1 = 38f;
    [SyncVar]
    public float crewSPO2Min1 = 0f;
    [SyncVar]
    public float crewSPO2Max1 = 38f;
    [SyncVar]
    public float crewABPMin1 = 82f;
    [SyncVar]
    public float crewABPMax1 = 125f;
    [SyncVar]
    public float crewPAPMin1 = 10f;
    [SyncVar]
    public float crewPAPMax1 = 26f;

}
