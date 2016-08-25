using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Meg.Networking;
using TouchScript.Hit;
using TouchScript.Gestures;

public class debugVessels : Singleton<debugVessels>
{
    public const float DepthIncrement = 5;

    public debugVesselMarker MarkerPrefab;

    public buttonGroup vesselButtonGroup;
    public buttonGroup movementButtonGroup;

    public GameObject mapObject;

    public GameObject depthSlider;
    public GameObject velocitySlider;
    public GameObject headingSlider;
    public GameObject diveAngleSlider;

    public GameObject depthText;
    public GameObject velocityText;
    public GameObject headingText;
    public GameObject diveAngleText;

    public buttonControl visibleButton;
    public buttonControl holdingButton;
    public buttonControl vectorButton;
    public buttonControl interceptButton;
    public buttonControl pursueButton;
    public buttonControl activeButton;
    public buttonControl autoSpeedButton;
    public buttonControl decreaseDepthButton;
    public buttonControl increaseDepthButton;

    public GameObject timeToIntercept;
    public widgetSetTextValue timeToInterceptHours;
    public widgetSetTextValue timeToInterceptMins;
    public widgetSetTextValue timeToInterceptSeconds;
    public buttonControl timeToInterceptSetButton;

    public buttonGroup targetVesselButtonGroup;

    public string[] markerNames;
    public float updateTick = 0.4f;

    private buttonControl[] buttonControls;
    private bool[] buttonStates;
    private bool[] prevStates;
    private bool stateChanged;
    private debugVesselMarker[] markers;
    public int activeVessel = 1;
    private float vx;
    private float vy;
    private float vz;
    private float vv;
    private bool canUpdate = true;
    private bool initialized;

    private vesselMovements Movements
        { get { return serverUtils.GetVesselMovements(); } }

    private void OnEnable()
    {
        GetComponent<PressGesture>().Pressed += pressedHandler;
        GetComponent<ReleaseGesture>().Released += releaseHandler;
        timeToInterceptSetButton.onPressed += OnSetTimeToInterceptPressed;
        activeButton.onReleased += OnActiveButtonReleased;
        visibleButton.onReleased += OnVisibleButtonReleased;
        autoSpeedButton.onReleased += OnAutoSpeedButtonReleased;
        decreaseDepthButton.onReleased += OnDecreaseDepthButtonReleased;
        increaseDepthButton.onReleased += OnIncreaseDepthButtonReleased;

        if (serverUtils.IsReady())
            UpdateUiState();
    }

    private void OnDisable()
    {
        GetComponent<PressGesture>().Pressed -= pressedHandler;
        GetComponent<ReleaseGesture>().Released -= releaseHandler;
        timeToInterceptSetButton.onPressed -= OnSetTimeToInterceptPressed;
        activeButton.onReleased -= OnActiveButtonReleased;
        visibleButton.onReleased -= OnVisibleButtonReleased;
        autoSpeedButton.onReleased -= OnAutoSpeedButtonReleased;
        decreaseDepthButton.onReleased -= OnDecreaseDepthButtonReleased;
        increaseDepthButton.onReleased -= OnIncreaseDepthButtonReleased;
    }

    private void pressedHandler(object sender, EventArgs e)
    {
        var gesture = sender as PressGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);
    }

    private void releaseHandler(object sender, EventArgs e)
    {
        // Don't allow changes to map while simulating.
        if (Movements.IsActive())
            return;

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

        // Reset marker trail.
        markers[activeVessel - 1].Reset();
    }

    IEnumerator wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canUpdate = true;
    }

    private void Start()
    {
        activeVessel = 1;
        buttonControls = new buttonControl[vesselButtonGroup.buttons.Length];
        buttonStates = new bool[buttonControls.Length];
        prevStates = new bool[buttonControls.Length];
        markers = new debugVesselMarker[buttonStates.Length];
        for (int i = 0; i < vesselButtonGroup.buttons.Length; i++)
        {
            buttonControls[i] = vesselButtonGroup.buttons[i].GetComponent<buttonControl>();
            buttonStates[i] = buttonControls[i].active;
            prevStates[i] = buttonStates[i];
            ActiveVesselData(i + 1);
            CreateVesselMarker(markerNames[i], new Vector2(vx, vy), i);
        }
	}

    private void CreateVesselMarker(string markerId, Vector2 markerPos, int markerNumber)
    {
        var marker = Instantiate(MarkerPrefab);
        marker.Name = markerId;
        marker.Vessel = markerNumber + 1;
        marker.transform.parent = mapObject.transform;
        marker.transform.localPosition = new Vector3(markerPos.x, markerPos.y, -2f);
        markers[markerNumber] = marker;
    }

    void Update()
    {
        // Check if debug map screen is visible.
        if (!gameObject.activeInHierarchy)
            return;

        // Perform an initial UI update when the server is ready.
        if (!initialized && serverUtils.IsReady())
        {
            UpdateUiState();
            initialized = true;
        }

        UpdateVesselMarkers();

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

        if (movementButtonGroup.changed)
        {
            movementButtonGroup.changed = false;
            if (holdingButton.active)
                Movements.SetHoldingPattern(activeVessel);
            else if (vectorButton.active)
                Movements.SetVector(activeVessel);
            else if (interceptButton.active)
                Movements.SetIntercept(activeVessel);
            else if (pursueButton.active)
                Movements.SetPursue(activeVessel);
            else
                Movements.SetNone(activeVessel);

            UpdateUiState();
        }

        if (targetVesselButtonGroup.changed)
        {
            UpdateTargetVesselFromUi();
            targetVesselButtonGroup.changed = false;
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

    void UpdateVesselMarkers()
    {
        var interceptMarker = markers[5];

        for (int i = 0; i < markers.Length; i++)
        {
            var marker = markers[i];
            var vessel = i + 1;
            ActiveVesselData(vessel);
            marker.transform.localPosition = new Vector3(vx, vy, -2f);
            marker.Selected = (vessel == activeVessel);
            marker.InterceptMarker = Movements.IsIntercepting(vessel) ? interceptMarker : null;
        }
    }

    void UpdateDepthFromUi()
    {
        ActiveVesselData(activeVessel);

        var depth = depthSlider.GetComponentInChildren<sliderWidget>().returnValue;
        depth = Mathf.RoundToInt(depth / 5) * 5;

        ChangeValue(activeVessel, new Vector3(vx, vy, depth), vv);
    }

    void UpdateSpeedFromUi()
    {
        var movement = Movements.GetVesselMovement(activeVessel);
        var setVector = movement as vesselSetVector;
        var holding = movement as vesselHoldingPattern;
        var intercept = movement as vesselIntercept;
        var pursue = movement as vesselPursue;

        var speed = velocitySlider.GetComponentInChildren<sliderWidget>().returnValue;
        speed = Mathf.RoundToInt(speed);

        if (setVector)
            setVector.Speed = speed;
        else if (holding)
            holding.Speed = speed;
        else if (intercept && !intercept.AutoSpeed)
            intercept.Speed = speed;
        else if (pursue)
            pursue.Speed = speed;
    }

    void UpdateHeadingFromUi()
    {
        var setVector = Movements.GetVesselMovement(activeVessel) as vesselSetVector;
        if (setVector)
            setVector.Heading = Mathf.RoundToInt(headingSlider.GetComponentInChildren<sliderWidget>().returnValue);
    }

    void UpdateAngleFromUi()
    {
        var setVector = Movements.GetVesselMovement(activeVessel) as vesselSetVector;
        if (setVector)
            setVector.DiveAngle = Mathf.RoundToInt(diveAngleSlider.GetComponentInChildren<sliderWidget>().returnValue);
    }

    void UpdateTargetVesselFromUi()
    {
        var pursue = Movements.GetVesselMovement(activeVessel) as vesselPursue;
        if (!pursue)
            return;

        var buttons = targetVesselButtonGroup.buttons;
        var n = buttons.Length;
        for (var i = 0; i < n; i++)
        {
            var vessel = i + 1;
            if (buttons[i].GetComponent<buttonControl>().active)
                if (vessel != activeVessel)
                    pursue.TargetVessel = vessel;
        }
    }

    void UpdateTimeToInterceptFromUi()
    {
        var h = timeToInterceptHours.currentNumber;
        var m = timeToInterceptMins.currentNumber;
        var s = timeToInterceptSeconds.currentNumber;
        var ts = new TimeSpan(h, m, s);

        Movements.SetTimeToIntercept((float) ts.TotalSeconds);
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
        visibleButton.active = serverUtils.GetVesselVis(activeVessel);

        // Determine if the vessel has a current movement plan.
        var movement = Movements.GetVesselMovement(activeVessel);
        var holding = movement as vesselHoldingPattern;
        var setVector = movement as vesselSetVector;
        var intercept = movement as vesselIntercept;
        var pursue = movement as vesselPursue;

        // Update movement buttons to reflect current state.
        holdingButton.active = holding != null;
        vectorButton.active = setVector != null;
        interceptButton.active = intercept != null;
        interceptButton.gameObject.SetActive(activeVessel != vesselIntercept.InterceptVessel);
        pursueButton.active = pursue != null;
        
        // Update simulation active button state.
        activeButton.active = serverUtils.GetServerBool("vesselmovementenabled");

        // Update velocity slider.
        velocitySlider.SetActive(holding || setVector || pursue || intercept);
        if (holding)
        {
            SetVelocityText(holding.Speed);
            if (updateValues)
                velocitySlider.GetComponentInChildren<sliderWidget>().SetValue(holding.Speed);
        }
        else if (setVector)
        {
            SetVelocityText(setVector.Speed);
            if (updateValues)
                velocitySlider.GetComponentInChildren<sliderWidget>().SetValue(setVector.Speed);
        }
        else if (intercept)
        {
            SetVelocityText(intercept.Speed);
            if (updateValues || intercept.AutoSpeed)
                velocitySlider.GetComponentInChildren<sliderWidget>().SetValue(intercept.Speed);
        }
        else if (pursue)
        {
            SetVelocityText(pursue.Speed);
            if (updateValues)
                velocitySlider.GetComponentInChildren<sliderWidget>().SetValue(pursue.Speed);
        }

        // Update auto-speed button.
        autoSpeedButton.gameObject.SetActive(intercept);
        if (intercept)
            autoSpeedButton.active = intercept.AutoSpeed;

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
        if (intercept && (updateValues || intercept.Active))
        {
            var ts = TimeSpan.FromSeconds(Movements.TimeToIntercept);
            timeToInterceptHours.currentNumber = ts.Hours;
            timeToInterceptMins.currentNumber = ts.Minutes;
            timeToInterceptSeconds.currentNumber = ts.Seconds;
        }
        else if (intercept && !intercept.Active)
        {
            UpdateTimeToInterceptFromUi();
        }

        // Update target vessel button group.
        targetVesselButtonGroup.gameObject.SetActive(pursue != null);
        if (pursue)
            UpdateTargetVesselButtons();

        // Update the depth slider value.
        if (updateValues || Movements.Active)
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

    void UpdateTargetVesselButtons()
    {
        var pursue = Movements.GetVesselMovement(activeVessel) as vesselPursue;
        if (!pursue)
            return;

        var buttons = targetVesselButtonGroup.buttons;
        var n = buttons.Length;
        for (var i = 0; i < n; i++)
            buttons[i].GetComponent<buttonControl>().active = pursue.TargetVessel == i + 1;
    }

    void SetVelocityText(float speed)
    {
        var kph = speed * Conversions.MetresPerSecondToKph;
        var kts = speed * Conversions.MetresPerSecondToKnots;

        velocityText.GetComponent<TextMesh>().text = speed.ToString("n1") + " m/s"
            + " (" + kph.ToString("n0") + "kph"
            + ", " + kts.ToString("n0") + "kts)";
    }

    void OnSetTimeToInterceptPressed()
    {
        var intercept = Movements.GetVesselMovement(activeVessel) as vesselIntercept;
        if (intercept)
            UpdateTimeToInterceptFromUi();
    }

    void OnVisibleButtonReleased()
    {
        serverUtils.SetVesselVis(activeVessel, visibleButton.active);
    }

    void OnActiveButtonReleased()
    {
        serverUtils.SetServerBool("vesselmovementenabled", activeButton.active);
    }

    void OnAutoSpeedButtonReleased()
    {
        var intercept = Movements.GetVesselMovement(activeVessel) as vesselIntercept;
        if (intercept)
            intercept.AutoSpeed = autoSpeedButton.active;
    }

    void OnDecreaseDepthButtonReleased()
    {
        ChangeVesselDepth(-DepthIncrement);
    }

    void OnIncreaseDepthButtonReleased()
    {
        ChangeVesselDepth(DepthIncrement);
    }

    void ChangeVesselDepth(float delta)
    {
        if (Movements.Active)
            return;

        var depth = serverUtils.GetVesselDepth(activeVessel);
        var slider = depthSlider.GetComponentInChildren<sliderWidget>();
        depth = Mathf.Clamp(depth + delta, slider.minValue, slider.maxValue);
        serverUtils.SetVesselDepth(activeVessel, depth);
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
