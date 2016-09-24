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

    private int RovLastState = 5;
    public bool DebugMode = true;

    public GameObject LightsScreen;
    public GameObject CameraScreen;
    public GameObject SonarScreen;

    public GameObject CameraAlerts;
    public GameObject SonarAlerts;

    public GameObject CameraaStart;
    public GameObject SonarStart;

    public GameObject CameraLaunched;
    public GameObject SonarLaunched;
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
        if (!serverUtils.IsReady())
            return;

        if (!serverUtils.IsRov())
            return;

        StartPos = this.transform.localPosition;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        if (!serverUtils.IsReady())
            return;

        if (!serverUtils.IsRov())
            return;

        StartPos = this.transform.localPosition;
        ResetRov();

    }

    // Update is called once per frame
    void Update()
    {
        if (!serverUtils.IsReady())
            return;

        if (!serverUtils.IsRov())
            return;

        if (DebugMode)
        {
            DebugStuff();
        }

        Hotkeys();

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

        CameraAlerts.SetActive(false);
        SonarAlerts.SetActive(false);

        if (CameraaStart)
            CameraaStart.SetActive(true);
        if (SonarStart)
            SonarStart.SetActive(true);

        if (CameraLaunched)
            CameraLaunched.SetActive(false);
        if (SonarLaunched)
            SonarLaunched.SetActive(false);

        serverUtils.ServerData.yawAngle = 15f;

    }

    void LaunchRov()
    {
        //Lights
        Lights.SetState(RovState);
        serverUtils.SubControl.LaunchROV = true;

        CameraAlerts.SetActive(false);
        SonarAlerts.SetActive(false);

        if(CameraaStart)
            CameraaStart.SetActive(false);
        if(SonarStart)
            SonarStart.SetActive(false);

        if(CameraLaunched)
            CameraLaunched.SetActive(true);
        if(SonarLaunched)
            SonarLaunched.SetActive(true);
    }

    void WarningRov()
    {
        //Lights
        Lights.SetState(RovState);

        CameraAlerts.SetActive(true);
        SonarAlerts.SetActive(true);

        if (CameraaStart)
            CameraaStart.SetActive(false);
        if (SonarStart)
            SonarStart.SetActive(false);

        if (CameraLaunched)
            CameraLaunched.SetActive(false);
        if (SonarLaunched)
            SonarLaunched.SetActive(false);
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

    void Hotkeys()
    {
        //if (Input.GetKeyDown("1") || Input.GetKeyDown("1"))
        //    if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        //        SetLightScreen();
        //
        //if (Input.GetKeyDown("2") || Input.GetKeyDown("2"))
        //    if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        //        SetLightScreen();
        //
        //if (Input.GetKeyDown("3") || Input.GetKeyDown("3"))
        //    if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        //        SetLightScreen();

        if (Input.GetKeyDown("1"))
        {
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                SetLightScreen();
        }

        if (Input.GetKeyDown("2"))
        {
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                SetCameraScreen();
        }

        if (Input.GetKeyDown("3"))
        {
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                SetSonarScreen();
        }

    }

    public void SetLightScreen()
    {
        LightsScreen.SetActive(true);
        CameraScreen.SetActive(false);
        SonarScreen.SetActive(false);
    }

    public void SetCameraScreen()
    {
        LightsScreen.SetActive(false);
        CameraScreen.SetActive(true);
        SonarScreen.SetActive(false);
    }

    public void SetSonarScreen()
    {
        LightsScreen.SetActive(false);
        CameraScreen.SetActive(false);
        SonarScreen.SetActive(true);
    }
}