using UnityEngine;
using UnityEngine.Networking;
using Meg.Networking;

public class OSRov : NetworkBehaviour
{
    [SyncVar]
    public float RovLightSBoard;
    [SyncVar]
    public float RovLightPort;
    [SyncVar]
    public float RovLightBow;

    [SyncVar]
    public int RovState = 0;

    private int RovLastState = 0;
    public bool DebugMode = true;
    //0 = not launched
    //1 = launched
    //2 = eaten by a shark **SPOILER ALERT**

    public LightControl Lights;

    Vector3 StartPos;


    //public ROVSonarControl Sonar;
    //public ROVCameraControl Cameras;

    // Use this for initialization
    void Start()
    {
        StartPos = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (DebugMode)
        {
            DebugStuff();
        }

        //each frame updates
        switch (RovState)
        {
            case 0:
                {
                    //not launched. Starting state
                }
                break;
            case 1:
                {
                    //not launched. Starting state

                }
                break;
            case 2:
                {
                    //not launched. Starting state

                }
                break;
        }

        //if the state has changed, set up the new state
        if (RovLastState != RovState)
        {
            switch (RovState)
            {
                case 0:
                    {
                        //not launched. Starting state
                        ResetRov();
                    }
                    break;
                case 1:
                    {
                        //not launched. Starting state
                        LaunchRov();
                    }
                    break;
                case 2:
                    {
                        //not launched. Starting state
                        WarningRov();
                    }
                    break;
            }
        }

        RovLastState = RovState;
    }

    void ResetRov()
    {
        //Lights
        Lights.SetState(RovState);
        this.transform.localPosition = StartPos;
        serverUtils.SubControl.LaunchROV = false;
        this.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
        this.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);

    }

    void LaunchRov()
    {
        //Lights
        Lights.SetState(RovState);
        serverUtils.SubControl.LaunchROV = true;
    }

    void WarningRov()
    {
        //Lights
        Lights.SetState(RovState);
    }

    void DebugStuff()
    {
        if (Input.GetKeyDown("space"))
        {
            RovState++;

            if (RovState > 2)
            {
                RovState = 2;
            }
        }

        if (Input.GetKeyDown("escape"))
        {
            RovState = 0;
        }
    }
}