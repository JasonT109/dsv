using UnityEngine;
using System;
using System.Collections;
using Meg.Networking;
using TouchScript.Hit;
using TouchScript.Gestures;

public class debugVessels : MonoBehaviour
{
    public buttonGroup vesselButtonGroup;
    public buttonGroup movementButtonGroup;
    public GameObject marker;
    public GameObject mapObject;
    public GameObject depthSlider;
    public GameObject velocitySlider;
    public GameObject headingSlider;
    public GameObject diveAngleSlider;
    public GameObject depthText;
    public GameObject velocityText;
    public GameObject headingText;
    public GameObject diveAngleText;
    public GameObject visibleButton;
    public GameObject holdingButton;
    public GameObject vectorButton;
    public GameObject interceptButton;
    public buttonControl activeButton;

    public GameObject timeToIntercept;
    public widgetSetTextValue timeToInterceptHours;
    public widgetSetTextValue timeToInterceptMins;
    public widgetSetTextValue timeToInterceptSeconds;
    public buttonControl timeToInterceptSetButton;

    public string[] markerNames;
    public float updateTick = 0.4f;

    public Color defaultMarkerColor;
    public Color activeMarkerColor;

    private float nextUpdateTime = 0.0f;
    private buttonControl[] buttonControls;
    private bool[] buttonStates;
    private bool[] prevStates;
    private bool stateChanged;
    private GameObject[] markers;
    public int activeVessel = 1;
    private float vx;
    private float vy;
    private float vz;
    private float vv;
    private bool canUpdate = true;
    private bool initialized;

    private void OnEnable()
    {
        GetComponent<PressGesture>().Pressed += pressedHandler;
        GetComponent<ReleaseGesture>().Released += releaseHandler;
        timeToInterceptSetButton.onPressed += OnSetTimeToInterceptPressed;
        activeButton.onReleased += OnActiveButtonReleased;
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

        //Debug.Log("Player vessel: " + serverUtils.GetPlayerVessel());
        //Debug.Log("Active vessel: " + (activeVessel + 1));

        //set position of the active vessel
        if (activeVessel == serverUtils.GetPlayerVessel())
        {
            serverUtils.SetServerData("posX", localPos.x * 1000f);
            serverUtils.SetServerData("posZ", localPos.y * 1000f);
        }
        else
        {
            ActiveVesselData(activeVessel);
            serverUtils.SetVesselData(activeVessel, new Vector3(localPos.x, localPos.y, vz), vv);
        }

        // Re-activate the vessel in order to update interface.
        ActivateVessel(activeVessel);
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
            ActiveVesselData(GetVesselForMarker(i));
            markers[i].transform.localPosition = new Vector3(vx, vy, -2f);
            markers[i].GetComponent<MeshRenderer>().material.color = GetColorForMarker(i);
        }
    }

    Color GetColorForMarker(int markerNumber)
    {
        int playerMarker = GetMarkerForVessel(serverUtils.GetPlayerVessel());
        int activeMarker = GetMarkerForVessel(activeVessel);
        var playerColor = serverUtils.GetColorTheme().highlightColor;
        var isPlayer = markerNumber == playerMarker;
        var isActive = markerNumber == activeMarker;
        var color = isPlayer ? playerColor : defaultMarkerColor;

        if (isActive)
            color = Color.Lerp(color, activeMarkerColor, 0.5f);

        if (!serverUtils.GetVesselVis(GetVesselForMarker(markerNumber)))
            color *= 0.25f;

        return color;
    }

    int GetMarkerForVessel(int vessel)
    {
        // Marker 0 is vessel 1, and so on.
        return vessel - 1;
    }

    int GetVesselForMarker(int marker)
    {
        // Marker 0 is vessel 1, and so on.
        return marker + 1;
    }

    IEnumerator wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canUpdate = true;
    }

    void Start ()
    {
        activeVessel = 1;
        nextUpdateTime = Time.time + updateTick;
        buttonControls = new buttonControl[vesselButtonGroup.buttons.Length];
        buttonStates = new bool[buttonControls.Length];
        prevStates = new bool[buttonControls.Length];
        markers = new GameObject[buttonStates.Length];
        for (int i = 0; i < vesselButtonGroup.buttons.Length; i++)
        {
            buttonControls[i] = vesselButtonGroup.buttons[i].GetComponent<buttonControl>();
            buttonStates[i] = buttonControls[i].active;
            prevStates[i] = buttonStates[i];
            ActiveVesselData(i + 1);
            SpawnShipMarkers(markerNames[i], new Vector2(vx, vy), i);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Perform an initial UI update when the server is ready.
        if (!initialized && serverUtils.IsReady())
        {
            UpdateUiState();
            initialized = true;
        }

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
                    ActivateVessel(i + 1);
            }
            StartCoroutine(wait(0.1f));
        }

        if (canUpdate)
            serverUtils.SetVesselVis(activeVessel, visibleButton.GetComponent<buttonControl>().active);

        if (movementButtonGroup.changed)
        {
            movementButtonGroup.changed = false;
            var simulating = activeButton.active;
            if (holdingButton.GetComponent<buttonControl>().active)
                serverUtils.GetVesselMovements().SetHoldingPattern(activeVessel, simulating);
            else if (vectorButton.GetComponent<buttonControl>().active)
                serverUtils.GetVesselMovements().SetVector(activeVessel, simulating);
            else if (interceptButton.GetComponent<buttonControl>().active)
                serverUtils.GetVesselMovements().SetIntercept(activeVessel, simulating, 60);
            else
                serverUtils.GetVesselMovements().SetNone(activeVessel);

            UpdateUiState();
        }

        if (canUpdate)
        {
            if (depthSlider.GetComponentInChildren<sliderWidget>().valueChanged)
                UpdateDepthFromUi();
            if (velocitySlider.GetComponentInChildren<sliderWidget>().valueChanged)
                UpdateSpeedFromUi();
            if (headingSlider.GetComponentInChildren<sliderWidget>().valueChanged)
                UpdateHeadingFromUi();
            if (diveAngleSlider.GetComponentInChildren<sliderWidget>().valueChanged)
                UpdateAngleFromUi();
            if (serverUtils.IsReady())
                UpdateUiState(false);
        }
    }

    void UpdateDepthFromUi()
    {
        ActiveVesselData(activeVessel);
        ChangeValue(activeVessel, new Vector3(vx, vy, depthSlider.GetComponentInChildren<sliderWidget>().returnValue), vv);
    }

    void UpdateSpeedFromUi()
    {
        var movement = serverUtils.GetVesselMovements().GetVesselMovement(activeVessel);
        var setVector = movement as vesselSetVector;
        var holding = movement as vesselHoldingPattern;
        if (setVector)
            setVector.Speed = velocitySlider.GetComponentInChildren<sliderWidget>().returnValue;
        else if (holding)
            holding.Speed = velocitySlider.GetComponentInChildren<sliderWidget>().returnValue;
    }

    void UpdateHeadingFromUi()
    {
        var setVector = serverUtils.GetVesselMovements().GetVesselMovement(activeVessel) as vesselSetVector;
        if (setVector)
            setVector.Heading = Mathf.RoundToInt(headingSlider.GetComponentInChildren<sliderWidget>().returnValue);
    }

    void UpdateAngleFromUi()
    {
        var setVector = serverUtils.GetVesselMovements().GetVesselMovement(activeVessel) as vesselSetVector;
        if (setVector)
            setVector.DiveAngle = Mathf.RoundToInt(diveAngleSlider.GetComponentInChildren<sliderWidget>().returnValue);
    }

    void UpdateTimeToInterceptFromUi()
    {
        var intercept = serverUtils.GetVesselMovements().GetVesselMovement(activeVessel) as vesselIntercept;
        if (!intercept)
            return;

        var h = timeToInterceptHours.currentNumber;
        var m = timeToInterceptMins.currentNumber;
        var s = timeToInterceptSeconds.currentNumber;
        var ts = new TimeSpan(h, m, s);

        intercept.SetTimeToIntercept((float)ts.TotalSeconds);
    }

    void ActivateVessel(int vessel)
    {
        activeVessel = vessel;
        UpdateUiState();
    }

    void UpdateUiState(bool updateValues = true)
    { 
        ActiveVesselData(activeVessel);

        // Show whether vessel is visible.
        visibleButton.GetComponent<buttonControl>().active = serverUtils.GetVesselVis(activeVessel);

        // Determine if the vessel has a current movement plan.
        var movement = serverUtils.GetVesselMovements().GetVesselMovement(activeVessel);
        var holding = movement as vesselHoldingPattern;
        var setVector = movement as vesselSetVector;
        var intercept = movement as vesselIntercept;

        // Update movement buttons to reflect current state.
        holdingButton.GetComponent<buttonControl>().active = holding != null;
        vectorButton.GetComponent<buttonControl>().active = setVector != null;
        interceptButton.GetComponent<buttonControl>().active = intercept != null;
        interceptButton.SetActive(activeVessel != vesselIntercept.InterceptVessel);

        // Update velocity slider.
        velocitySlider.SetActive(holding || setVector);
        if (holding)
        {
            if (updateValues)
                velocitySlider.GetComponentInChildren<sliderWidget>().SetValue(holding.Speed);

            velocityText.GetComponent<TextMesh>().text = holding.Speed.ToString("n1");
        }
        else if (setVector)
        {
            if (updateValues)
                velocitySlider.GetComponentInChildren<sliderWidget>().SetValue(setVector.Speed);

            velocityText.GetComponent<TextMesh>().text = setVector.Speed.ToString("n1");
        }

        // Update heading slider.
        headingSlider.SetActive(setVector != null);
        if (setVector)
        {
            if (updateValues)
                headingSlider.GetComponentInChildren<sliderWidget>().SetValue(setVector.Heading);

            headingText.GetComponent<TextMesh>().text = setVector.Heading.ToString("n") + "°";
        }

        // Update dive angle slider.
        diveAngleSlider.SetActive(setVector != null);
        if (setVector)
        {
            if (updateValues)
                diveAngleSlider.GetComponentInChildren<sliderWidget>().SetValue(setVector.DiveAngle);

            diveAngleText.GetComponent<TextMesh>().text = setVector.DiveAngle.ToString("n1") + "°";
        }

        // Update interception countdown.
        timeToIntercept.SetActive(intercept != null);
        if (intercept && updateValues)
        {
            var ts = TimeSpan.FromSeconds(intercept.TimeToIntercept);
            timeToInterceptHours.currentNumber = ts.Hours;
            timeToInterceptMins.currentNumber = ts.Minutes;
            timeToInterceptSeconds.currentNumber = ts.Seconds;
        }

        // Update the depth slider value.
        if (updateValues)
        {
            switch (activeVessel)
            {
                case 1:
                    depthSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("v1posZ"));
                    depthText.GetComponent<textValueFromServer>().linkDataString = "v1depth";
                    break;
                case 2:
                    depthSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("v2posZ"));
                    depthText.GetComponent<textValueFromServer>().linkDataString = "v2depth";
                    break;
                case 3:
                    depthSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("v3posZ"));
                    depthText.GetComponent<textValueFromServer>().linkDataString = "v3depth";
                    break;
                case 4:
                    depthSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("v4posZ"));
                    depthText.GetComponent<textValueFromServer>().linkDataString = "v4depth";
                    break;
                case 5:
                    depthSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("meg1posZ"));
                    depthText.GetComponent<textValueFromServer>().linkDataString = "meg1depth";
                    break;
                case 6:
                    depthSlider.GetComponentInChildren<sliderWidget>().SetValue(serverUtils.GetServerData("intercept1posZ"));
                    depthText.GetComponent<textValueFromServer>().linkDataString = "intercept1depth";
                    break;
            }
        }
}

    void OnSetTimeToInterceptPressed()
    {
        var intercept = serverUtils.GetVesselMovements().GetVesselMovement(activeVessel) as vesselIntercept;
        if (!intercept)
            return;

        UpdateTimeToInterceptFromUi();
    }

    void OnActiveButtonReleased()
    {
        serverUtils.GetVesselMovements().SetActive(activeButton.active);
    }

    void ChangeValue(int vessel, Vector3 pos, float velocity)
    {
        serverUtils.SetVesselData(vessel, pos, velocity);
    }

    void ActiveVesselData(int vessel)
    {
        switch (vessel)
        {
            case 1:
                vx = serverUtils.GetServerData("v1posX");
                vy = serverUtils.GetServerData("v1posY");
                vz = serverUtils.GetServerData("v1posZ");
                vv = serverUtils.GetServerData("v1velocity");
                break;
            case 2:
                vx = serverUtils.GetServerData("v2posX");
                vy = serverUtils.GetServerData("v2posY");
                vz = serverUtils.GetServerData("v2posZ");
                vv = serverUtils.GetServerData("v2velocity");
                break;
            case 3:
                vx = serverUtils.GetServerData("v3posX");
                vy = serverUtils.GetServerData("v3posY");
                vz = serverUtils.GetServerData("v3posZ");
                vv = serverUtils.GetServerData("v3velocity");
                break;
            case 4:
                vx = serverUtils.GetServerData("v4posX");
                vy = serverUtils.GetServerData("v4posY");
                vz = serverUtils.GetServerData("v4posZ");
                vv = serverUtils.GetServerData("v4velocity");
                break;
            case 5:
                vx = serverUtils.GetServerData("meg1posX");
                vy = serverUtils.GetServerData("meg1posY");
                vz = serverUtils.GetServerData("meg1posZ");
                vv = serverUtils.GetServerData("meg1velocity");
                break;
            case 6:
                vx = serverUtils.GetServerData("intercept1posX");
                vy = serverUtils.GetServerData("intercept1posY");
                vz = serverUtils.GetServerData("intercept1posZ");
                vv = serverUtils.GetServerData("intercept1velocity");
                break;
        }
    }

}
