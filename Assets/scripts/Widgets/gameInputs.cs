using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;

public class gameInputs : NetworkBehaviour
{
    //public WindZone wind;
    private GameObject sData;
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
    public string[] joysticks;
    [SyncVar]
    public bool pilot = false;
    public bool focused = false;
    public GameObject status;
    private bool canSendData = true;
    public float x1;
    public float y1;
    public float z1;
    public float x2;
    public float y2;


    // Use this for initialization
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

    void OnApplicationFocus(bool focusStatus)
    {
        if (!isLocalPlayer)
            return;

        focused = focusStatus;
    }

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

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (!localPlayerAuthority)
            return;

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
                if(status)
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

    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canSendData = true;
    }
}
