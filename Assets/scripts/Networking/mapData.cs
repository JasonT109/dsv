using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Meg.Networking;

public class mapData : NetworkBehaviour
{
    [SyncVar]
    public int playerVessel = 1;
    [SyncVar]
    public float initiateMapEvent;
    [SyncVar]
    public string mapEventName;
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
    public bool vessel1Vis = true;
    [SyncVar]
    public bool vessel2Vis = true;
    [SyncVar]
    public bool vessel3Vis = true;
    [SyncVar]
    public bool vessel4Vis = true;
    [SyncVar]
    public bool meg1Vis = true;

    void Update()
    {
        if (playerVessel != 0)
        {
            //Set the relevant vessels data to match the server objects world position. Map space is 0.001 x world space. Also SWAP Y and Z for map space.
            switch (playerVessel)
            {
                case 1:
                    vessel1Pos = new Vector3(serverUtils.GetServerData("xPos") * 0.001f, serverUtils.GetServerData("zPos") * 0.001f, serverUtils.GetServerData("depth"));
                    vessel1Velocity = serverUtils.GetServerData("velocity");
                    break;
                case 2:
                    vessel2Pos = new Vector3(serverUtils.GetServerData("xPos") * 0.001f, serverUtils.GetServerData("zPos") * 0.001f, serverUtils.GetServerData("depth"));
                    vessel2Velocity = serverUtils.GetServerData("velocity");
                    break;
                case 3:
                    vessel3Pos = new Vector3(serverUtils.GetServerData("xPos") * 0.001f, serverUtils.GetServerData("zPos") * 0.001f, serverUtils.GetServerData("depth"));
                    vessel3Velocity = serverUtils.GetServerData("velocity");
                    break;
                case 4:
                    vessel4Pos = new Vector3(serverUtils.GetServerData("xPos") * 0.001f, serverUtils.GetServerData("zPos") * 0.001f, serverUtils.GetServerData("depth"));
                    vessel4Velocity = serverUtils.GetServerData("velocity");
                    break;
            }
        }
    }
}