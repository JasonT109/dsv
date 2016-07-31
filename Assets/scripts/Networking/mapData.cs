using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Meg.Networking;

public class mapData : NetworkBehaviour
{

    /** Number of vessels that can be displayed on the map. */
    public const int VesselCount = 6;

    /** Default floor depth to assume. */
    public const float DefaultFloorDepth = 10994;


    [Header("Coordinates")]
    [SyncVar]
    public float latitude = 18.553059f;
    [SyncVar]
    public float longitude = 112.244285f;

    [Header("Player")]
    [SyncVar]
    public int playerVessel = 1;

    [Header("Vessel Positions")]
    [SyncVar]
    public Vector3 vessel1Pos = new Vector3(-1f, -0.5f, 2000f);
    [SyncVar]
    public Vector3 vessel2Pos = new Vector3(-2f, 2f, 8900f);
    [SyncVar]
    public Vector3 vessel3Pos = new Vector3(2f, -2f, 7300f);
    [SyncVar]
    public Vector3 vessel4Pos = new Vector3(0f, 0f, 7700f);
    [SyncVar]
    public Vector3 meg1Pos = new Vector3(0f, -2.5f, 8200f);
    [SyncVar]
    public Vector3 intercept1Pos = new Vector3(2f, 2f, 8200f);

    [Header("Vessel Velocities")]
    [SyncVar]
    public float vessel1Velocity;
    [SyncVar]
    public float vessel2Velocity;
    [SyncVar]
    public float vessel3Velocity;
    [SyncVar]
    public float vessel4Velocity;
    [SyncVar]
    public float meg1Velocity;
    [SyncVar]
    public float intercept1Velocity;

    [Header("Vessel Visibility")]
    [SyncVar]
    public bool vessel1Vis = true;
    [SyncVar]
    public bool vessel2Vis = true;
    [SyncVar]
    public bool vessel3Vis = true;
    [SyncVar]
    public bool vessel4Vis = true;
    [SyncVar]
    public bool meg1Vis = true;
    [SyncVar]
    public bool intercept1Vis = true;

    [Header("Vessel Warnings")]
    [SyncVar]
    public bool vessel1Warning;
    [SyncVar]
    public bool vessel2Warning;
    [SyncVar]
    public bool vessel3Warning;
    [SyncVar]
    public bool vessel4Warning;
    [SyncVar]
    public bool meg1Warning;
    [SyncVar]
    public bool intercept1Warning;

    [Header("Map Events")]
    [SyncVar]
    public float initiateMapEvent;
    [SyncVar]
    public string mapEventName;


    /** Physics update. */
    void FixedUpdate()
    {
        if (playerVessel != 0)
        {
            // Set the player vessel's data to match the server objects world position. 
            // Map space is 0.001 x world space. Also SWAP Y and Z for map space.
            var x = serverUtils.GetServerData("xPos") * 0.001f;
            var y = serverUtils.GetServerData("zPos") * 0.001f;
            var z = serverUtils.GetServerData("depth");
            var position = new Vector3(x, y, z);
            var velocity = serverUtils.GetServerData("velocity");
            SetVesselState(playerVessel, position, velocity);
        }
    }

    /** Set a vessel's current position and nominal speed (1-based index). */
    public void SetVesselState(int vessel, Vector3 position, float velocity)
    {
        switch (vessel)
        {
            case 1:
                vessel1Pos = position;
                vessel1Velocity = velocity;
                break;
            case 2:
                vessel2Pos = position;
                vessel2Velocity = velocity;
                break;
            case 3:
                vessel3Pos = position;
                vessel3Velocity = velocity;
                break;
            case 4:
                vessel4Pos = position;
                vessel4Velocity = velocity;
                break;
            case 5:
                meg1Pos = position;
                meg1Velocity = velocity;
                break;
            case 6:
                intercept1Pos = position;
                intercept1Velocity = velocity;
                break;
        }
    }

}