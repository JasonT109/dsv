using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Meg.Networking;

public class debugChangeValue : NetworkBehaviour {

    public GameObject debugObject;
    public GameObject subSystemDebug;

    private GameObject valueUp10k;
    private GameObject valueUp1k;
    private GameObject valueUp100;
    private GameObject valueUp10;
    private GameObject valueUp1;

    private GameObject valueDown10k;
    private GameObject valueDown1k;
    private GameObject valueDown100;
    private GameObject valueDown10;
    private GameObject valueDown1;

    public GameObject[] buttons;
    public GameObject[] subSystemButtons;
    public GameObject batteryObject;
    public GameObject[] batterySliders;
    public GameObject oxygenObject;
    public GameObject[] oxygenSliders;
    public float[] sliderValues = new float[7];

    private TextMesh text10k;
    private TextMesh text1k;
    private TextMesh text100;
    private TextMesh text10;
    private TextMesh text1;

    private bool canChangeValue = true;
    private string valueString;
    private float valueFloat;

    private float clickWaitTime = 0.2f;

    bool[] previousStates;
    bool stateChanged = false;

    bool[] subButtonStates;
    bool subButtonStateChanged = false;

    [Command]
    void CmdChangeValue()
    {
        foreach (GameObject b in buttons)
        {
            if (b.GetComponent<buttonControl>().active)
            {
                if (b.GetComponent<buttonControl>().serverType == "time")
                {
                    string valueString1 = (text10k.text.ToString());
                    string valueString2 = (text1k.text.ToString() + text100.text.ToString());
                    string valueString3 = (text10.text.ToString() + text1.text.ToString());
                    float valueFloat1 = float.Parse(valueString1);
                    float valueFloat2 = float.Parse(valueString2);
                    float valueFloat3 = float.Parse(valueString3);
                    if (b.GetComponent<buttonControl>().serverName == "ETA")
                    {
                        serverUtils.SetServerData("dueTimeHours", valueFloat1);
                        serverUtils.SetServerData("dueTimeMins", valueFloat2);
                        serverUtils.SetServerData("dueTimeSecs", valueFloat3);
                    }
                    if (b.GetComponent<buttonControl>().serverName == "diveTime")
                    {
                        serverUtils.SetServerData("diveTimeHours", valueFloat1);
                        serverUtils.SetServerData("diveTimeMins", valueFloat2);
                        serverUtils.SetServerData("diveTimeSecs", valueFloat3);
                    }

                }
                else if (b.GetComponent<buttonControl>().serverType == "float")
                {
                    string buttonName = b.GetComponent<buttonControl>().serverName;
                    valueString = (text10k.text.ToString() + text1k.text.ToString() + text100.text.ToString() + "." + text10.text.ToString() + text1.text.ToString());
                    valueFloat = float.Parse(valueString);
                    serverUtils.SetServerData(buttonName, valueFloat);
                }
                else
                {
                    string buttonName = b.GetComponent<buttonControl>().serverName;
                    valueString = (text10k.text.ToString() + text1k.text.ToString() + text100.text.ToString() + text10.text.ToString() + text1.text.ToString());
                    valueFloat = float.Parse(valueString);
                    serverUtils.SetServerData(buttonName, valueFloat);
                }
            }
        }
    }

    [Command]
    void CmdBatteryValueChanged(int bank, float value)
    {
        serverUtils.SetBatteryData(bank, value);
    }

    [Command]
    void CmdOxygenValueChanged(int bank, float value)
    {
        serverUtils.SetOxygenData(bank, value);
    }

    void Start ()
    {
        //get game objects
        debugObject = GameObject.FindWithTag("Debug");
        subSystemDebug = GameObject.FindWithTag("SubSystemDebug");
        batteryObject = debugObject.GetComponent<debugObject>().batteryGroup;
        oxygenObject = debugObject.GetComponent<debugObject>().oxygenGroup;

        //get components
        valueUp10k = debugObject.GetComponent<debugObject>().valueUp10k;
        valueUp1k = debugObject.GetComponent<debugObject>().valueUp1k;
        valueUp100 = debugObject.GetComponent<debugObject>().valueUp100;
        valueUp10 = debugObject.GetComponent<debugObject>().valueUp10;
        valueUp1 = debugObject.GetComponent<debugObject>().valueUp1;
        valueDown10k = debugObject.GetComponent<debugObject>().valueDown10k;
        valueDown1k = debugObject.GetComponent<debugObject>().valueDown1k;
        valueDown100 = debugObject.GetComponent<debugObject>().valueDown100;
        valueDown10 = debugObject.GetComponent<debugObject>().valueDown10;
        valueDown1 = debugObject.GetComponent<debugObject>().valueDown1;
        text10k = debugObject.GetComponent<debugObject>().text10k;
        text1k = debugObject.GetComponent<debugObject>().text1k;
        text100 = debugObject.GetComponent<debugObject>().text100;
        text10 = debugObject.GetComponent<debugObject>().text10;
        text1 = debugObject.GetComponent<debugObject>().text1;

        //get values
        valueString = (text10k.text.ToString() + text1k.text.ToString() + text100.text.ToString() + text10.text.ToString() + text1.text.ToString());
        valueFloat = float.Parse(valueString);

        //get button and slider group objects
        buttons = debugObject.GetComponent<buttonGroup>().buttons;
        subSystemButtons = subSystemDebug.GetComponent<buttonGroup>().buttons;
        if (batteryObject != null)
        {
            batterySliders = batteryObject.GetComponent<sliderGroup>().sliders;
        }
        if (oxygenObject != null)
        {
            oxygenSliders = oxygenObject.GetComponent<sliderGroup>().sliders;
        }

        //get initial button states, we can check to see if this changes in update loop
        previousStates = new bool[debugObject.GetComponent<buttonGroup>().buttons.Length];
        for (int i = 0; i < debugObject.GetComponent<buttonGroup>().buttons.Length; i++)
        {
            previousStates[i] = buttons[i].GetComponent<buttonControl>().active;
        }

        //get initial sub button states
        subButtonStates = new bool[subSystemDebug.GetComponent<buttonGroup>().buttons.Length];
        for (int i = 0; i < subSystemDebug.GetComponent<buttonGroup>().buttons.Length; i++)
        {
            subButtonStates[i] = subSystemButtons[i].GetComponent<buttonControl>().active;
        }

        //initialise the states
        changeMainState();
        changeSubState();
    }

    void parseAsInt(int tempInt)
    {
        string tempVal = tempInt.ToString("D5");
        text10k.text = tempVal.Substring(0, 1);
        text1k.text = tempVal.Substring(1, 1);
        text100.text = tempVal.Substring(2, 1);
        text10.text = tempVal.Substring(3, 1);
        text1.text = tempVal.Substring(4, 1);
    }

    void parseAsTime(int tempInt1, int tempInt2, int tempInt3)
    {
        string tempVal1 = tempInt1.ToString("D2");
        string tempVal2 = tempInt2.ToString("D2");
        string tempVal3 = tempInt3.ToString("D2");
        text10k.text = tempVal1.Substring(1, 1);
        text1k.text = tempVal2.Substring(0, 1);
        text100.text = tempVal2.Substring(1, 1);
        text10.text = tempVal3.Substring(0, 1);
        text1.text = tempVal3.Substring(1, 1);
    }

    void parseAsFloat(string tempVal)
    {
        string[] tempVals = tempVal.Split('.');
        if (tempVals.Length > 1)
        {
            int tempInt1 = int.Parse(tempVals[0]);
            //int tempInt2 = int.Parse(tempVals[1]);
            string v1 = tempInt1.ToString("D3");
            string v2 = tempVals[1];
            text10k.text = v1.Substring(0, 1);
            text1k.text = v1.Substring(1, 1);
            text100.text = v1.Substring(2, 1);
            text10.text = v2.Substring(0, 1);
            if (v2.Length > 1)
            {
                text1.text = v2.Substring(1, 1);
            }
            else
            {
                text1.text = "0";
            }
        }
        else
        {
            int tempInt1 = int.Parse(tempVal);
            string v1 = tempInt1.ToString("D5");
            text10k.text = v1.Substring(0, 1);
            text1k.text = v1.Substring(1, 1);
            text100.text = v1.Substring(2, 1);
            text10.text = v1.Substring(3, 1);
            text1.text = v1.Substring(4, 1);
        }
    }

    void changeMainState()
    {
        for (int i = 0; i < debugObject.GetComponent<buttonGroup>().buttons.Length; i++)
        {
            previousStates[i] = buttons[i].GetComponent<buttonControl>().active;
            if (buttons[i].GetComponent<buttonControl>().active)
            {
                switch (buttons[i].GetComponent<buttonControl>().serverName)
                {
                    case "depth":
                        int tempInt = (int)serverUtils.GetServerData("depth");
                        parseAsInt(tempInt);
                        break;
                    case "ETA":
                        int tempInt1 = (int)serverUtils.GetServerData("dueTimeHours");
                        int tempInt2 = (int)serverUtils.GetServerData("dueTimeMins");
                        int tempInt3 = (int)serverUtils.GetServerData("dueTimeSecs");
                        parseAsTime(tempInt1, tempInt2, tempInt3);
                        break;
                    case "diveTime":
                        int tempIntDive1 = (int)serverUtils.GetServerData("diveTimeHours");
                        int tempIntDive2 = (int)serverUtils.GetServerData("diveTimeMins");
                        int tempIntDive3 = (int)serverUtils.GetServerData("diveTimeSecs");
                        parseAsTime(tempIntDive1, tempIntDive2, tempIntDive3);
                        break;
                    case "Co2":
                        string tempVal = serverUtils.GetServerData("Co2").ToString();
                        parseAsFloat(tempVal);
                        break;
                    case "waterTemp":
                        tempVal = serverUtils.GetServerData("waterTemp").ToString();
                        parseAsFloat(tempVal);
                        break;
                    case "cabinTemp":
                        tempVal = serverUtils.GetServerData("cabinTemp").ToString();
                        parseAsFloat(tempVal);
                        break;
                }
            }
        }
    }

    void changeSubState()
    {
        for (int i = 0; i < subSystemButtons.Length; i++)
        {
            subButtonStates[i] = subSystemButtons[i].GetComponent<buttonControl>().active;
            //battery sliders
            if (subSystemButtons[0].GetComponent<buttonControl>().active)
            {
                for (int s = 0; s < batterySliders.Length; s++)
                {
                    switch (s)
                    {
                        case 0:
                            sliderValues[s] = serverUtils.GetServerData("b1");
                            break;
                        case 1:
                            sliderValues[s] = serverUtils.GetServerData("b2");
                            break;
                        case 2:
                            sliderValues[s] = serverUtils.GetServerData("b3");
                            break;
                        case 3:
                            sliderValues[s] = serverUtils.GetServerData("b4");
                            break;
                        case 4:
                            sliderValues[s] = serverUtils.GetServerData("b5");
                            break;
                        case 5:
                            sliderValues[s] = serverUtils.GetServerData("b6");
                            break;
                        case 6:
                            sliderValues[s] = serverUtils.GetServerData("b7");
                            break;
                    }
                    batterySliders[s].GetComponentInChildren<sliderWidget>().SetValue(sliderValues[s]);
                }
            }
            //oxygen sliders
            if (subSystemButtons[1].GetComponent<buttonControl>().active)
            {
                for (int s = 0; s < oxygenSliders.Length; s++)
                {
                    switch (s)
                    {
                        case 0:
                            sliderValues[s] = serverUtils.GetServerData("o1");
                            break;
                        case 1:
                            sliderValues[s] = serverUtils.GetServerData("o2");
                            break;
                        case 2:
                            sliderValues[s] = serverUtils.GetServerData("o3");
                            break;
                        case 3:
                            sliderValues[s] = serverUtils.GetServerData("o4");
                            break;
                        case 4:
                            sliderValues[s] = serverUtils.GetServerData("o5");
                            break;
                        case 5:
                            sliderValues[s] = serverUtils.GetServerData("o6");
                            break;
                        case 6:
                            sliderValues[s] = serverUtils.GetServerData("o7");
                            break;
                    }
                    oxygenSliders[s].GetComponentInChildren<sliderWidget>().SetValue(sliderValues[s]);
                }
            }
        }
    }

    void Update()
    {

        if (debugObject == null)
        {
            Debug.Log("No debug object found");
        }

        if (!isLocalPlayer)
            return;

        //main debug group (depth, ETA, etc.)
        for (int i = 0; i < debugObject.GetComponent<buttonGroup>().buttons.Length; i++)
        {
            if (buttons[i].GetComponent<buttonControl>().active != previousStates[i])
            {
                stateChanged = true;
            }
        }

        //sub system debug (battery, life systems etc.)
        for (int i = 0; i < subSystemDebug.GetComponent<buttonGroup>().buttons.Length; i++)
        {
            if (subSystemButtons[i].GetComponent<buttonControl>().active != subButtonStates[i])
            {
                subButtonStateChanged = true;
            }
        }

        if (subButtonStateChanged)
        {
            subButtonStateChanged = false;
            changeSubState();
        }

        if (stateChanged)
        {
            stateChanged = false;
            changeMainState();
        }

        //battery sliders changed
        if (subButtonStates[0])
        {
            for (int s = 0; s < batterySliders.Length; s++)
            {
                if (batterySliders[s].GetComponentInChildren<sliderWidget>().valueChanged)
                {
                    //Debug.Log("changing battery states");
                    CmdBatteryValueChanged(s, batterySliders[s].GetComponentInChildren<sliderWidget>().returnValue);
                }
            }
        }

        //oxygen sliders changed
        if (subButtonStates[1])
        {
            
            for (int s = 0; s < oxygenSliders.Length; s++)
            {
                if (oxygenSliders[s].GetComponentInChildren<sliderWidget>().valueChanged)
                {
                    //Debug.Log("changing oxygen states");
                    CmdOxygenValueChanged(s, oxygenSliders[s].GetComponentInChildren<sliderWidget>().returnValue);
                }
            }
        }

        if (valueUp10k.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            canChangeValue = false;
            int newValue = Mathf.Clamp(int.Parse(text10k.text) + 1, -1, 10);
            if (newValue > 9)
            {
                newValue = 0;
            }
            text10k.text = newValue.ToString();
            StartCoroutine(Wait(clickWaitTime));
            CmdChangeValue();

        }

        if (valueDown10k.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            canChangeValue = false;
            int newValue = Mathf.Clamp(int.Parse(text10k.text) - 1, -1, 10);
            if (newValue < 0)
            {
                newValue = 9;
            }
            text10k.text = newValue.ToString();
            StartCoroutine(Wait(clickWaitTime));
            CmdChangeValue();
        }

        if (valueUp1k.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            canChangeValue = false;
            int newValue = Mathf.Clamp(int.Parse(text1k.text) + 1, -1, 10);
            if (newValue > 9)
            {
                newValue = 0;
            }
            text1k.text = newValue.ToString();
            StartCoroutine(Wait(clickWaitTime));
            CmdChangeValue();
        }

        if (valueDown1k.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            canChangeValue = false;
            int newValue = Mathf.Clamp(int.Parse(text1k.text) - 1, -1, 10);
            if (newValue < 0)
            {
                newValue = 9;
            }
            text1k.text = newValue.ToString();
            StartCoroutine(Wait(clickWaitTime));
            CmdChangeValue();
        }

        if (valueUp100.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            canChangeValue = false;
            int newValue = Mathf.Clamp(int.Parse(text100.text) + 1, -1, 10);
            if (newValue > 9)
            {
                newValue = 0;
            }
            text100.text = newValue.ToString();
            StartCoroutine(Wait(clickWaitTime));
            CmdChangeValue();
        }

        if (valueDown100.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            canChangeValue = false;
            int newValue = Mathf.Clamp(int.Parse(text100.text) - 1, -1, 10);
            if (newValue < 0)
            {
                newValue = 9;
            }
            text100.text = newValue.ToString();
            StartCoroutine(Wait(clickWaitTime));
            CmdChangeValue();
        }

        if (valueUp10.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            canChangeValue = false;
            int newValue = Mathf.Clamp(int.Parse(text10.text) + 1, -1, 10);
            if (newValue > 9)
            {
                newValue = 0;
            }
            text10.text = newValue.ToString();
            StartCoroutine(Wait(clickWaitTime));
            CmdChangeValue();
        }

        if (valueDown10.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            canChangeValue = false;
            int newValue = Mathf.Clamp(int.Parse(text10.text) - 1, -1, 10);
            if (newValue < 0)
            {
                newValue = 9;
            }
            text10.text = newValue.ToString();
            StartCoroutine(Wait(clickWaitTime));
            CmdChangeValue();
        }

        if (valueUp1.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            canChangeValue = false;
            int newValue = Mathf.Clamp(int.Parse(text1.text) + 1, -1, 10);
            if (newValue > 9)
            {
                newValue = 0;
            }
            text1.text = newValue.ToString();
            StartCoroutine(Wait(clickWaitTime));
            CmdChangeValue();
        }

        if (valueDown1.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            canChangeValue = false;
            int newValue = Mathf.Clamp(int.Parse(text1.text) - 1, -1, 10);
            if (newValue < 0)
            {
                newValue = 9;
            }
            text1.text = newValue.ToString();
            StartCoroutine(Wait(clickWaitTime));
            CmdChangeValue();
        }
    }
    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canChangeValue = true;
    }
}
