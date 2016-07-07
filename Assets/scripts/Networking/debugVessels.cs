using UnityEngine;
using System;
using System.Collections;
using Meg.Networking;
using TouchScript.Hit;
using TouchScript.Gestures;

public class debugVessels : MonoBehaviour
{
    public GameObject vesselButtonGroup;
    public GameObject marker;
    public GameObject mapObject;
    public GameObject depthSlider;
    public GameObject velocitySlider;
    public GameObject depthText;
    public GameObject velocityText;
    public GameObject activeButton;
    public GameObject holdingButton;
    public GameObject vectorButton;
    public string[] markerNames;
    public float updateTick = 0.4f;
    private float nextUpdateTime = 0.0f;
    private buttonControl[] buttonControls;
    private bool[] buttonStates;
    private bool[] prevStates;
    private bool stateChanged;
    private GameObject[] markers;
    public int activeVessel;
    private float vx;
    private float vy;
    private float vz;
    private float vv;
    private bool canUpdate = true;



    private void OnEnable()
    {
        GetComponent<PressGesture>().Pressed += pressedHandler;
        GetComponent<ReleaseGesture>().Released += releaseHandler;
    }

    private void OnDisable()
    {
        GetComponent<PressGesture>().Pressed -= pressedHandler;
        GetComponent<ReleaseGesture>().Released -= releaseHandler;
    }

    private void pressedHandler(object sender, EventArgs e)
    {
        var gesture = sender as PressGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);
    }

    private void releaseHandler(object sender, EventArgs e)
    {
        var gesture = sender as ReleaseGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        //convert to local space
        Vector3 localPos = gameObject.transform.InverseTransformPoint(hit.Point);

        Debug.Log("Player vessel: " + serverUtils.GetPlayerVessel());
        Debug.Log("Active vessel: " + (activeVessel + 1));

        //set position of the active vessel
        if (activeVessel + 1 == serverUtils.GetPlayerVessel())
        {
            serverUtils.SetServerData("posX", localPos.x * 1000f);
            serverUtils.SetServerData("posZ", localPos.y * 1000f);
        }
        else
        {
            serverUtils.SetVesselData(activeVessel, new Vector3(localPos.x, localPos.y, vz), vv);
        }
    }

    void SpawnShipMarkers(string markerId, Vector2 markerPos, int markerNumber)
    {
        //spawn a visual waypoint
        GameObject wp = (GameObject)Instantiate(marker, new Vector3(0, 0, 0), Quaternion.identity);
        TextMesh t = wp.GetComponentInChildren<TextMesh>();

        //set the ID of the marker
        t.text = markerId;

        //parent to this object
        wp.transform.parent = mapObject.transform;

        //set the wp local position
        wp.transform.localPosition = new Vector3(markerPos.x, markerPos.y, -2f);

        //populate markers array
        markers[markerNumber] = wp;
    }

    void UpdateShipMarkers()
    {
        for (int i = 0; i < markers.Length; i++)
        {
            ActiveVesselData(i);
            markers[i].transform.localPosition = new Vector3(vx, vy, -2f);
        }
    }

    void ChangeValue(int vessel, Vector3 pos, float velocity)
    {
        serverUtils.SetVesselData(vessel, pos, velocity);
    }

    void ActiveVesselData(int vessel)
    {
        switch (vessel)
        {
            case 0:
                vx = serverUtils.GetServerData("v1posX");
                vy = serverUtils.GetServerData("v1posY");
                vz = serverUtils.GetServerData("v1posZ");
                vv = serverUtils.GetServerData("v1velocity");
                break;
            case 1:
                vx = serverUtils.GetServerData("v2posX");
                vy = serverUtils.GetServerData("v2posY");
                vz = serverUtils.GetServerData("v2posZ");
                vv = serverUtils.GetServerData("v2velocity");
                break;
            case 2:
                vx = serverUtils.GetServerData("v3posX");
                vy = serverUtils.GetServerData("v3posY");
                vz = serverUtils.GetServerData("v3posZ");
                vv = serverUtils.GetServerData("v3velocity");
                break;
            case 3:
                vx = serverUtils.GetServerData("v4posX");
                vy = serverUtils.GetServerData("v4posY");
                vz = serverUtils.GetServerData("v4posZ");
                vv = serverUtils.GetServerData("v4velocity");
                break;
        }
    }

    IEnumerator wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canUpdate = true;
    }

    void Start ()
    {
        nextUpdateTime = Time.time + updateTick;
        buttonControls = new buttonControl[vesselButtonGroup.GetComponent<buttonGroup>().buttons.Length];
        buttonStates = new bool[buttonControls.Length];
        prevStates = new bool[buttonControls.Length];
        markers = new GameObject[buttonStates.Length];
        for (int i = 0; i < vesselButtonGroup.GetComponent<buttonGroup>().buttons.Length; i++)
        {
            buttonControls[i] = vesselButtonGroup.GetComponent<buttonGroup>().buttons[i].GetComponent<buttonControl>();
            buttonStates[i] = buttonControls[i].active;
            prevStates[i] = buttonStates[i];
            ActiveVesselData(i);
            SpawnShipMarkers(markerNames[i], new Vector2(vx, vy), i);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.time > nextUpdateTime)
        {
            nextUpdateTime = Time.time + updateTick;
            UpdateShipMarkers();
        }

	    for (int i = 0; i < buttonStates.Length; i++)
        {
            buttonStates[i] = buttonControls[i].active;
            if (buttonStates[i] != prevStates[i])
            {
                stateChanged = true;
            }
        }

        if (stateChanged)
        {
            stateChanged = false;
            canUpdate = false;
            for (int i = 0; i < buttonStates.Length; i++)
            {
                prevStates[i] = buttonStates[i];

                if (buttonControls[i].active)
                {
                    activeVessel = i;
                    ActiveVesselData(activeVessel);
                    //update the sliders and marker positions
                    switch (i)
                    {
                        case 0:
                            depthSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("v1posZ"));
                            velocitySlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("v1velocity"));
                            depthText.GetComponent<textValueFromServer>().linkDataString = "v1depth";
                            velocityText.GetComponent<textValueFromServer>().linkDataString = "v1velocity";
                            break;
                        case 1:
                            depthSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("v2posZ"));
                            velocitySlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("v2velocity"));
                            depthText.GetComponent<textValueFromServer>().linkDataString = "v2depth";
                            velocityText.GetComponent<textValueFromServer>().linkDataString = "v2velocity";
                            break;
                        case 2:
                            depthSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("v3posZ"));
                            velocitySlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("v3velocity"));
                            depthText.GetComponent<textValueFromServer>().linkDataString = "v3depth";
                            velocityText.GetComponent<textValueFromServer>().linkDataString = "v3velocity";
                            break;
                        case 3:
                            depthSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("v4posZ"));
                            velocitySlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("v4velocity"));
                            depthText.GetComponent<textValueFromServer>().linkDataString = "v4depth";
                            velocityText.GetComponent<textValueFromServer>().linkDataString = "v4velocity";
                            break;
                    }
                }
            }
            StartCoroutine(wait(0.1f));
        }

        if (depthSlider.GetComponentInChildren<sliderWidget>().valueChanged && canUpdate)
        {
            ActiveVesselData(activeVessel);
            ChangeValue(activeVessel, new Vector3(vx, vy, depthSlider.GetComponentInChildren<sliderWidget>().returnValue), vv);
        }
        if (velocitySlider.GetComponentInChildren<sliderWidget>().valueChanged && canUpdate)
        {
            ActiveVesselData(activeVessel);
            ChangeValue(activeVessel, new Vector3(vx, vy, vz), velocitySlider.GetComponentInChildren<sliderWidget>().returnValue);
        }
    }
}
