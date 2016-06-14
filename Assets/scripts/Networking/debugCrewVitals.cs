using UnityEngine;
using System.Collections;
using Meg.Networking;

public class debugCrewVitals : MonoBehaviour
{
    public GameObject bpmSlider;
    public GameObject bodyTempSlider;
    public GameObject[] crewMembers;
    public TextMesh bpmText;
    public TextMesh bodyTempText;
    public bool[] buttonStates;
    public bool[] prevStates;
    private bool canCheck = true;
    public bool statesChanged = false;

    void ChangeValue(string slider, float value)
    {
        serverUtils.SetServerData(slider, value);
    }

    void GetButtonStates()
    {
        for (int i = 0; i < crewMembers.Length; i++)
        {
            buttonStates[i] = crewMembers[i].GetComponent<buttonControl>().active;
        }
    }

    IEnumerator wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

    }

	void Start ()
    {
        crewMembers = new GameObject[gameObject.GetComponent<buttonGroup>().buttons.Length];
        buttonStates = new bool[gameObject.GetComponent<buttonGroup>().buttons.Length];
        prevStates = new bool[gameObject.GetComponent<buttonGroup>().buttons.Length];
        crewMembers = gameObject.GetComponent<buttonGroup>().buttons;
    }

	void Update ()
    {
        for (int i = 0; i < buttonStates.Length; i++)
        {
            buttonStates[i] = crewMembers[i].GetComponent<buttonControl>().active;

            if (buttonStates[i] != prevStates[i])
            {
                statesChanged = true;
            }
        }

        if (statesChanged)
        {
            statesChanged = false;

            for (int i = 0; i < crewMembers.Length; i++)
            {
                prevStates[i] = buttonStates[i];
                if (crewMembers[i].GetComponent<buttonControl>().active)
                {
                    switch (i)
                    {
                        case 0:
                            bpmSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("crewHeartRate1"));
                            bodyTempSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("crewBodyTemp1"));
                            bpmText.GetComponent<textValueFromServer>().linkDataString = "crewHeartRate1";
                            bodyTempText.GetComponent<textValueFromServer>().linkDataString = "crewBodyTemp1";
                            break;
                        case 1:
                            bpmSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("crewHeartRate2"));
                            bodyTempSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("crewBodyTemp2"));
                            bpmText.GetComponent<textValueFromServer>().linkDataString = "crewHeartRate2";
                            bodyTempText.GetComponent<textValueFromServer>().linkDataString = "crewBodyTemp2";
                            break;
                        case 2:
                            bpmSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("crewHeartRate3"));
                            bodyTempSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("crewBodyTemp3"));
                            bpmText.GetComponent<textValueFromServer>().linkDataString = "crewHeartRate3";
                            bodyTempText.GetComponent<textValueFromServer>().linkDataString = "crewBodyTemp3";
                            break;
                        case 3:
                            bpmSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("crewHeartRate4"));
                            bodyTempSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("crewBodyTemp4"));
                            bpmText.GetComponent<textValueFromServer>().linkDataString = "crewHeartRate4";
                            bodyTempText.GetComponent<textValueFromServer>().linkDataString = "crewBodyTemp4";
                            break;
                        case 4:
                            bpmSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("crewHeartRate5"));
                            bodyTempSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("crewBodyTemp5"));
                            bpmText.GetComponent<textValueFromServer>().linkDataString = "crewHeartRate5";
                            bodyTempText.GetComponent<textValueFromServer>().linkDataString = "crewBodyTemp5";
                            break;
                        case 5:
                            bpmSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("crewHeartRate6"));
                            bodyTempSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("crewBodyTemp6"));
                            bpmText.GetComponent<textValueFromServer>().linkDataString = "crewHeartRate6";
                            bodyTempText.GetComponent<textValueFromServer>().linkDataString = "crewBodyTemp6";
                            break;
                    }
                }
            }
        }

        if (bpmSlider.GetComponentInChildren<sliderWidget>().valueChanged)
        {
            for (int i = 0; i < buttonStates.Length; i++)
            {
                if (crewMembers[i].GetComponent<buttonControl>().active)
                {
                    switch (i)
                    {
                        case 0:
                            ChangeValue("crewHeartRate1", bpmSlider.GetComponentInChildren<sliderWidget>().returnValue);
                            break;
                        case 1:
                            ChangeValue("crewHeartRate2", bpmSlider.GetComponentInChildren<sliderWidget>().returnValue);
                            break;
                        case 2:
                            ChangeValue("crewHeartRate3", bpmSlider.GetComponentInChildren<sliderWidget>().returnValue);
                            break;
                        case 3:
                            ChangeValue("crewHeartRate4", bpmSlider.GetComponentInChildren<sliderWidget>().returnValue);
                            break;
                        case 4:
                            ChangeValue("crewHeartRate5", bpmSlider.GetComponentInChildren<sliderWidget>().returnValue);
                            break;
                        case 5:
                            ChangeValue("crewHeartRate6", bpmSlider.GetComponentInChildren<sliderWidget>().returnValue);
                            break;
                    }
                }
            }
        }
        if (bodyTempSlider.GetComponentInChildren<sliderWidget>().valueChanged)
        {
            for (int i = 0; i < buttonStates.Length; i++)
            {
                if (crewMembers[i].GetComponent<buttonControl>().active)
                {
                    switch (i)
                    {
                        case 0:
                            ChangeValue("crewBodyTemp1", bodyTempSlider.GetComponentInChildren<sliderWidget>().returnValue);
                            break;
                        case 1:
                            ChangeValue("crewBodyTemp2", bodyTempSlider.GetComponentInChildren<sliderWidget>().returnValue);
                            break;
                        case 2:
                            ChangeValue("crewBodyTemp3", bodyTempSlider.GetComponentInChildren<sliderWidget>().returnValue);
                            break;
                        case 3:
                            ChangeValue("crewBodyTemp4", bodyTempSlider.GetComponentInChildren<sliderWidget>().returnValue);
                            break;
                        case 4:
                            ChangeValue("crewBodyTemp5", bodyTempSlider.GetComponentInChildren<sliderWidget>().returnValue);
                            break;
                        case 5:
                            ChangeValue("crewBodyTemp6", bodyTempSlider.GetComponentInChildren<sliderWidget>().returnValue);
                            break;
                    }
                }
            }
        }
    }
}
