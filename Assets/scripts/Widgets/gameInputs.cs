using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;

public class gameInputs : NetworkBehaviour
{
    [SyncVar]
    public float output = 0.0f;
    [SyncVar]
    public float outputX = 0.0f;
    [SyncVar]
    public float outputY = 0.0f;
    [SyncVar]
    public float outputX2 = 0.0f;
    [SyncVar]
    public float outputY2 = 0.0f;
    [SyncVar]
    public bool pilot = false;
    /** which GLIDER screen this client is controlling (left = 2, middle = 1, right = 0) */
    [SyncVar]
    public int glScreenID = 0;
    /** what GLIDER screen content this client is viewing */
    [SyncVar]
    public int activeScreen = 0;

    /** list of all active joysticks plugged in */
    public string[] joysticks;

    /** server data object */
    private GameObject sData;

    /** this client has windows focus, if joystick is plugged in this client will be setting input that the server can then use */
    private bool focused = false;

    /** debug text mesh object so we can see if a screen has focus */
    private GameObject status;

    /** used to restrict updates to a set time tick */
    private bool canSendData = true;

    /** input axis */
    private float x1;
    private float y1;
    private float z1;
    private float x2;
    private float y2;

    /** initialisation */
    void Start()
    {
        if (!isLocalPlayer)
            return;

        joysticks = Input.GetJoystickNames();
        focused = true;
        status = GameObject.FindWithTag("Status");
        if (status)
        {
            status.GetComponent<TextMesh>().text = "Strong";
        }
    }

    /** when app gets windows focus sets the focus state */
    void OnApplicationFocus(bool focusStatus)
    {
        if (!isLocalPlayer)
            return;

        focused = focusStatus;
    }

    /** updates the syncvars with the input */
    [Command]
    void CmdChangeInput(bool state, float xAxis1, float yAxis1, float zAxis1, float xAxis2, float yAxis2)
    {
        output = zAxis1;
        outputX = xAxis1;
        outputY = yAxis1;
        outputX2 = xAxis2;
        outputY2 = yAxis2;

        if (state)
        {
            pilot = true;
        }
        else
        {
            pilot = false;
        }
    }

    [Command]
    void CmdChangeScreenContent(int scContent)
    {
        activeScreen = scContent;
    }

    [Command]
    void CmdChangeScreenID(int scID)
    {
        glScreenID = scID;
    }

    /** update */
    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (!localPlayerAuthority)
            return;

        /** If we have changed the current screen ID update it with the server */
        if (glScreenManager.Instance && glScreenManager.Instance.hasChanged)
        {
            CmdChangeScreenID(glScreenManager.Instance.screenID);
            glScreenManager.Instance.hasChanged = false;

            /** If we have changed the right screen content update it with the server */
            if (glScreenManager.Instance.screenID == 0)
            {
                CmdChangeScreenContent(glScreenManager.Instance.rightScreenID);
            }

            glScreenManager.Instance.hasChanged = false;
        }

        GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
        if(Root)
        {
            if(Root.GetComponent<serverData>().IsJoystickSwapped == false)
            {
                z1 = Input.GetAxis("Throttle");
                x1 = Input.GetAxis("Horizontal");
                y1 = Input.GetAxis("Vertical");
                x2 = Input.GetAxis("X2");
                y2 = Input.GetAxis("Y2");
            }
            else
            {
                z1 = -(Input.GetAxis("Throttle2"));
                x1 = Input.GetAxis("Horizontal2");
                y1 = Input.GetAxis("Vertical2");
                x2 = Input.GetAxis("X2");
                y2 = Input.GetAxis("Y2");
            }
        }
        else
        {
            z1 = Input.GetAxis("Throttle");
            x1 = Input.GetAxis("Horizontal");
            y1 = Input.GetAxis("Vertical");
            x2 = Input.GetAxis("X2");
            y2 = Input.GetAxis("Y2");
        }

        if (sData != null)
        {
            if (focused)
            {
                if (status)
                {
                    status.GetComponent<TextMesh>().text = "Strong";
                }
            }
            else
            {
                if (status)
                {
                    status.GetComponent<TextMesh>().text = "Weak";
                }
            }
        }
        else
        {
            sData = GameObject.FindWithTag("ServerData");
        }

        if (joysticks.Length > 0 && focused)
        {
            if (canSendData)
            {
                CmdChangeInput(true, x1, y1, z1, x2, y2);
                canSendData = false;
                StartCoroutine(Wait(0.1f));
            }
        }
        else
        {
            if (canSendData)
            {
                CmdChangeInput(false, x1, y1, z1, x2, y2);
                canSendData = false;
                StartCoroutine(Wait(0.2f));
            }
        }
    }

    /** wait co-routine so we don't spam the server */
    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canSendData = true;
    }
}
