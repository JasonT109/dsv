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
    //-1 = not live
    //0 = not launched
    //1 = launched
    //2 = eaten by a shark **SPOILER ALERT**

    [SyncVar]
    public int ROVCameraState = 0;
    // 0 = offline screen
    // 1 = live feed
    // 2 = green
 
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

    public GameObject CameraPreset;
    public GameObject SonarPreset;

    public GameObject CameraReset;
    public GameObject SonarReset;

    public GameObject CamerasGreen;
    public GameObject CamerasOffline;
    public GameObject CamerasLive;

    public LightControl Lights;

    public GameObject CamerasGuages;
    public GameObject SonarGuages;

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

        switch(ROVCameraState)
        {
            case 0:
                {
                    if(CamerasOffline.activeInHierarchy == false)
                    {
                        CamerasGreen.SetActive(false);
                        CamerasOffline.SetActive(true);
                        CamerasLive.SetActive(false);
                    }
                }
                break;
            case 1:
                {
                    if (CamerasLive.activeInHierarchy == false)
                    {
                        CamerasGreen.SetActive(false);
                        CamerasOffline.SetActive(false);
                        CamerasLive.SetActive(true);
                    }
                }
                break;
            case 2:
                {
                    if (CamerasGreen.activeInHierarchy == false)
                    {
                        CamerasGreen.SetActive(true);
                        CamerasOffline.SetActive(false);
                        CamerasLive.SetActive(false);
                    }
                }
                break;
        }

        Hotkeys();

        //each frame updates
        switch (RovState)
        {
            case -1:
                {
                    //not launched. Starting state
                    if(serverUtils.SubControl.isControlDecentMode)
                    {
                        serverUtils.SubControl.isControlDecentMode = false;
                        RovState = 0;
                    }
                }
                break;
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
                case -1:
                    {
                        //not launched. Starting state
                        PresetRov();
                    }
                    break;
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

    void PresetRov()
    {
        //Lights
        Lights.SetState(0);
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

        if (CameraPreset)
            CameraPreset.SetActive(true);
        if (SonarPreset)
            SonarPreset.SetActive(true);

        if (CameraReset)
            CameraReset.SetActive(false);
        if (SonarReset)
            SonarReset.SetActive(false);

        serverUtils.ServerData.yawAngle = 15f;

        Physics.gravity = new Vector3(0, -0.5F, 0);

        CamerasGuages.SetActive(false);
        SonarGuages.SetActive(false);

    }

    void ResetRov()
    {
        //Lights
        Lights.SetState(1);
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

        if (CameraPreset)
            CameraPreset.SetActive(false);
        if (SonarPreset)
            SonarPreset.SetActive(true);

        if (CameraReset)
            CameraReset.SetActive(true);
        if (SonarReset)
            SonarReset.SetActive(true);

        serverUtils.ServerData.yawAngle = 15f;

        Physics.gravity = new Vector3(0, -0.5F, 0);

        CamerasGuages.SetActive(false);
        SonarGuages.SetActive(false);

    }

    void LaunchRov()
    {
        //Lights
        Lights.SetState(5);
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

        Physics.gravity = new Vector3(0, -0.5F, 0);

        CamerasGuages.SetActive(true);
        SonarGuages.SetActive(true);
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
            SonarStart.SetActive(true);

        if (CameraLaunched)
            CameraLaunched.SetActive(false);
        if (SonarLaunched)
            SonarLaunched.SetActive(false);

        if (SonarPreset)
            SonarPreset.SetActive(true);
        if (SonarReset)
            SonarReset.SetActive(false);

        serverUtils.ServerData.velocity = 0f;
        serverUtils.ServerData.verticalVelocity = 0f;
        Physics.gravity = new Vector3(0f, 0f, 0f);

        CamerasGuages.SetActive(true);
        SonarGuages.SetActive(true);
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
            RovState = -1;
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