using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

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
}
