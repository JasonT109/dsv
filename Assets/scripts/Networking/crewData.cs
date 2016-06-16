using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Meg.Networking;

public class crewData : NetworkBehaviour
{
    [SyncVar]
    public int playerVessel = 1;
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
    [SyncVar]
    public Vector3 vessel1Pos = new Vector3(-1f, -0.5f, 2000f);
    [SyncVar]
    public Vector3 vessel2Pos = new Vector3(-2f, 2f, 8900f);
    [SyncVar]
    public Vector3 vessel3Pos = new Vector3(2f, -2f, 7300f);
    [SyncVar]
    public Vector3 vessel4Pos = new Vector3(0f, 0f, 7700f);
    [SyncVar]
    public Vector3 meg1Pos;
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


    void Update()
    {
        if (playerVessel != 0)
        {
            //set the relevant vessels data to match the server object -  SWAP Y and Z for the map
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
