using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using Meg.EventSystem;
using Meg.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/**
 * Editing interface for a single vessel in the debug interface.
 * Allows the user to modify the vessel's name, icon etc., as well as
 * the movement properties of the vessel.
 */

public class debugVesselPropertiesUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    public CanvasGroup PropertiesGroup;
    public Transform PropertiesContainer;
    public InputField NameInput;
    public debugVesselsUi Vessels;
    public Slider DepthSlider;
    public InputField DepthInput;
    public Toggle[] IconToggles;
    public Button AddMovementEventButton;

    [Header("Movement Components")]
    public Transform MovementProperties;
    public Toggle[] MovementTypeToggles;
    public Transform MovementStateProperties;
    public Toggle MovementAutoSpeedToggle;
    public CanvasGroup MovementSpeedGroup;
    public Slider MovementSpeedSlider;
    public InputField MovementSpeedInput;
    public CanvasGroup MovementMaxSpeedGroup;
    public Slider MovementMaxSpeedSlider;
    public InputField MovementMaxSpeedInput;
    public Slider MovementHeadingSlider;
    public InputField MovementHeadingInput;
    public Slider MovementDiveAngleSlider;
    public InputField MovementDiveAngleInput;
    public Slider MovementPeriodSlider;
    public InputField MovementPeriodInput;
    public Slider MovementTargetSlider;
    public InputField MovementTargetInput;
    public Text MovementTargetLabel;
    public debugVesselVectorDialUi MovementVectorDialUi;

    [Header("Time To Intercept")]
    public CanvasGroup EtaGroup;
    public Text EtaHeader;
    public Slider EtaHoursSlider;
    public InputField EtaHoursInput;
    public Slider EtaMinutesSlider;
    public InputField EtaMinutesInput;
    public Slider EtaSecondsSlider;
    public InputField EtaSecondsInput;

    /** The vessel being edited. */
    public vesselData.Vessel Vessel
    {
        get { return _vessel; }
        set { SetVessel(value); }
    }


    // Private Properties
    // ------------------------------------------------------------

    /** Vessel data. */
    private vesselData VesselData
        { get { return serverUtils.VesselData; } }

    /** Types of vessel movement. */
    private readonly string[] _vesselMovementTypes = {
        vesselMovements.InterceptType,
        vesselMovements.PursueType,
        vesselMovements.SetVectorType,
        vesselMovements.HoldingType };

    /** Whether it's possible to add events. */
    private bool CanAddEvents
    {
        get
        {
            if (Vessel.Id <= 0 || !Vessel.Movement)
                return false;

            var file = megEventManager.Instance.File;
            return (file != null && file.canAdd);
        }
    }


    // Members
    // ------------------------------------------------------------

    /** The vessel being edited. */
    private vesselData.Vessel _vessel;

    /** Last known movement type. */
    private string _lastMovementType = vesselMovements.NoType;

    /** Whether ui is being updated. */
    private bool _updating;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        ConfigureUi();
        InitUi();
    }

    /** Updating. */
    private void Update()
    {
        UpdateUi();

        // Keyboard shortcut for adding a vessel movement event.
        if (Input.GetKeyDown(KeyCode.KeypadPlus) && CanAddEvents)
            SimulateButtonPress(AddMovementEventButton);
    }



    // Public Methods
    // ------------------------------------------------------------

    /** Sets the group's name. */
    public void SetName(string value)
    {
        if (_updating)
            return;

        VesselData.SetName(Vessel.Id, value);
    }

    /** Add a vessel movement event to the selected event group. */
    public void AddMovementEvent()
    {
        if (!CanAddEvents)
            return;

        var file = megEventManager.Instance.File;
        if (file == null || !file.canAdd)
            return;

        var group = file.selectedGroup;
        if (group == null)
        {
            group = file.AddGroup();
            group.id = "MOVEMENTS";
        }

        var vesselEvent = group.AddEvent(megEventType.VesselMovement) as megEventVesselMovement;
        if (vesselEvent == null)
            return;

        vesselEvent.Vessel = Vessel.Id;
        vesselEvent.Capture();
    }

    // Private Methods
    // ------------------------------------------------------------

    private void SetVessel(vesselData.Vessel value)
    {
        _vessel = value;
        InitUi();
    }

    private void ConfigureUi()
    {
        DepthSlider.onValueChanged.AddListener(OnDepthSliderChanged);
        DepthInput.onEndEdit.AddListener(OnDepthInputChanged);

        for (var i = 0; i < IconToggles.Length; i++)
        {
            var icon = (vesselData.Icon) i;
            IconToggles[i].onValueChanged.AddListener(
                on => OnIconToggled(icon, on));
        }

        ConfigureMovementProperties();
    }

    private void InitUi()
    {
        _updating = true;

        if (Vessel.Id <= 0)
            { ClearUi(); return; }
        
        PropertiesContainer.gameObject.SetActive(true);
        PropertiesGroup.interactable = true;
        NameInput.text = Vessel.Name;
        DepthSlider.value = Vessel.Depth;
        DepthSlider.maxValue = Mathf.Max(DepthSlider.maxValue, Vessel.Depth);
        DepthInput.text = string.Format("{0:N1}", Vessel.Depth);

        _updating = false;

        UpdateIconToggles();
        InitMovementProperties();
    }

    private void UpdateUi()
    {
        _updating = true;
        UpdateMovementProperties();
        AddMovementEventButton.interactable = CanAddEvents;
        EtaGroup.interactable = !megEventManager.Instance.Playing;
        _updating = false;
        
    }

    private void ClearUi()
    {
        PropertiesGroup.interactable = false;
        NameInput.text = "PROPERTIES";
        PropertiesContainer.gameObject.SetActive(false);
    }

    private void OnDepthSliderChanged(float value)
    {
        if (_updating)
            return;

        VesselData.SetDepth(Vessel.Id, value);
        DepthInput.text = string.Format("{0:N1}", value);
    }

    private void OnDepthInputChanged(string value)
    {
        if (_updating)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        VesselData.SetDepth(Vessel.Id, result);
        DepthSlider.maxValue = Mathf.Max(DepthSlider.maxValue, result);
        DepthSlider.value = result;
    }

    private void UpdateIconToggles()
    {
        _updating = true;
        var icon = VesselData.GetIcon(Vessel.Id);
        for (var i = 0; i < IconToggles.Length; i++)
        {
            var toggle = IconToggles[i];
            toggle.isOn = ((vesselData.Icon) i) == icon;
        }
        _updating = false;
    }

    private void OnIconToggled(vesselData.Icon icon, bool value)
    {
        if (_updating)
            return;

        VesselData.SetIcon(Vessel.Id, icon);
        UpdateIconToggles();
    }

    /** Simulate a button press. */
    private void SimulateButtonPress(Button button)
    {
        var pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(AddMovementEventButton.gameObject, pointer, ExecuteEvents.submitHandler);
    }



    // Movements Interface
    // ------------------------------------------------------------

    private vesselMovements Movements
        { get { return serverUtils.VesselMovements; } }

    private vesselMovement Movement
        { get { return Movements ? Movements.GetVesselMovement(Vessel.Id) : null; } }

    private vesselIntercept Intercept
        { get { return Movement as vesselIntercept; } }

    private vesselPursue Pursue
        { get { return Movement as vesselPursue; } }

    private vesselSetVector SetVector
        { get { return Movement as vesselSetVector; } }

    private vesselHoldingPattern Holding
        { get { return Movement as vesselHoldingPattern; } }

    private void UpdateMovement()
    {
        if (Movement && !Movement.isServer)
            Movement.PostState();
    }

    private void ConfigureMovementProperties()
    {
        for (var i = 0; i < MovementTypeToggles.Length; i++)
        {
            var type = _vesselMovementTypes[i];
            MovementTypeToggles[i].onValueChanged.AddListener(
                on => OnMovementTypeToggled(type, on));
        }

        MovementAutoSpeedToggle.onValueChanged.AddListener(OnMovementAutoSpeedToggleChanged);
        MovementSpeedSlider.onValueChanged.AddListener(OnMovementSpeedSliderChanged);
        MovementSpeedInput.onEndEdit.AddListener(OnMovementSpeedInputChanged);
        MovementMaxSpeedSlider.onValueChanged.AddListener(OnMovementMaxSpeedSliderChanged);
        MovementMaxSpeedInput.onEndEdit.AddListener(OnMovementMaxSpeedInputChanged);
        MovementHeadingSlider.onValueChanged.AddListener(OnMovementHeadingSliderChanged);
        MovementHeadingInput.onEndEdit.AddListener(OnMovementHeadingInputChanged);
        MovementDiveAngleSlider.onValueChanged.AddListener(OnMovementDiveAngleSliderChanged);
        MovementDiveAngleInput.onEndEdit.AddListener(OnMovementDiveAngleInputChanged);
        MovementPeriodSlider.onValueChanged.AddListener(OnMovementPeriodSliderChanged);
        MovementPeriodInput.onEndEdit.AddListener(OnMovementPeriodInputChanged);
        MovementTargetSlider.onValueChanged.AddListener(OnMovementTargetSliderChanged);
        MovementTargetInput.onEndEdit.AddListener(OnMovementTargetInputChanged);
        MovementVectorDialUi.onValueChanged.AddListener(OnMovementDialChanged);

        ConfigureEtaProperties();
    }

    private void ConfigureEtaProperties()
    {
        EtaHoursSlider.onValueChanged.AddListener(OnEtaSliderChanged);
        EtaHoursInput.onEndEdit.AddListener(OnEtaInputChanged);
        EtaMinutesSlider.onValueChanged.AddListener(OnEtaSliderChanged);
        EtaMinutesInput.onEndEdit.AddListener(OnEtaInputChanged);
        EtaSecondsSlider.onValueChanged.AddListener(OnEtaSliderChanged);
        EtaSecondsInput.onEndEdit.AddListener(OnEtaInputChanged);
    }

    private void InitMovementProperties()
    {
        _updating = true;
        _lastMovementType = Movements.GetMovementType(Vessel.Id);

        UpdateMovementTypeToggles();
        MovementStateProperties.gameObject.SetActive(Movement);
        if (!Movement)
            return;

        MovementAutoSpeedToggle.isOn = Intercept && Intercept.AutoSpeed;
        MovementSpeedSlider.value = Movement.GetSpeed();
        MovementSpeedSlider.maxValue = Movement.GetMaxSpeed();
        MovementSpeedInput.text = string.Format("{0:N1}", Movement.GetSpeed());
        MovementMaxSpeedSlider.value = Movement.GetMaxSpeed();
        MovementMaxSpeedSlider.maxValue = Mathf.Max(MovementMaxSpeedSlider.maxValue, Movement.GetMaxSpeed());
        MovementMaxSpeedInput.text = string.Format("{0:N1}", Movement.GetMaxSpeed());
        MovementVectorDialUi.Vessel = Vessel;

        if (SetVector)
        {
            MovementHeadingSlider.value = SetVector.Heading;
            MovementHeadingInput.text = string.Format("{0:N1}", SetVector.Heading);
            MovementDiveAngleSlider.value = SetVector.DiveAngle;
            MovementDiveAngleInput.text = string.Format("{0:N1}", SetVector.DiveAngle);
        }

        if (Holding)
        {
            MovementPeriodSlider.value = Holding.Period;
            MovementPeriodInput.text = string.Format("{0:N1}", Holding.Period);
        }

        if (Pursue)
        {
            MovementTargetSlider.value = Pursue.TargetVessel;
            MovementTargetInput.text = string.Format("{0:N0}", Pursue.TargetVessel);
        }

        _updating = false;

        InitEtaUi();
    }

    private void UpdateMovementProperties()
    {
        // Update movement properties if needed.
        if (_lastMovementType != Movements.GetMovementType(Vessel.Id))
            InitMovementProperties();

        _updating = true;

        if (Movement)
            UpdatePropertiesFromMovement();

        MovementStateProperties.gameObject.SetActive(Movement != null);
        MovementAutoSpeedToggle.gameObject.SetActive(Intercept);
        MovementSpeedGroup.interactable = !MovementAutoSpeedToggle.isOn;
        MovementSpeedGroup.alpha = MovementAutoSpeedToggle.isOn ? 0.5f : 1;
        MovementMaxSpeedGroup.interactable = !MovementAutoSpeedToggle.isOn;
        MovementMaxSpeedGroup.alpha = MovementAutoSpeedToggle.isOn ? 0.5f : 1;
        MovementHeadingSlider.transform.parent.gameObject.SetActive(SetVector);
        MovementDiveAngleSlider.transform.parent.gameObject.SetActive(SetVector);
        MovementPeriodSlider.transform.parent.gameObject.SetActive(Holding);
        MovementTargetSlider.transform.parent.gameObject.SetActive(Pursue);
        MovementVectorDialUi.gameObject.SetActive(SetVector);
        MovementTargetLabel.gameObject.SetActive(Pursue);
        MovementTargetLabel.text = Pursue ? "PURSUE " + VesselData.GetDebugName(Pursue.TargetVessel) : "";

        EtaGroup.gameObject.SetActive(Intercept);
        if (Intercept && megEventManager.Instance.Playing)
            InitEtaUi();

        _updating = false;
    }

    private void UpdatePropertiesFromMovement()
    {
        if (SetVector && !Mathf.Approximately(MovementHeadingSlider.value, SetVector.Heading))
        {
            MovementHeadingSlider.value = SetVector.Heading;
            MovementHeadingInput.text = string.Format("{0:N1}", SetVector.Heading);
        }
        if (!Mathf.Approximately(MovementMaxSpeedSlider.value, Movement.GetMaxSpeed()))
        {
            MovementMaxSpeedSlider.value = Movement.GetMaxSpeed();
            MovementMaxSpeedSlider.maxValue = Mathf.Max(MovementMaxSpeedSlider.maxValue, Movement.GetMaxSpeed());
            MovementMaxSpeedInput.text = string.Format("{0:N1}", Movement.GetMaxSpeed());
            MovementSpeedSlider.maxValue = Movement.GetMaxSpeed();
        }
        if (!Mathf.Approximately(MovementSpeedSlider.value, Movement.GetSpeed()))
        {
            MovementSpeedSlider.value = Movement.GetSpeed();
            MovementSpeedInput.text = string.Format("{0:N1}", Movement.GetSpeed());
        }
    }

    private void UpdateMovementTypeToggles()
    {
        var type = Movement ? Movement.Type : vesselMovements.NoType;
        for (var i = 0; i < MovementTypeToggles.Length; i++)
        {
            var toggle = MovementTypeToggles[i];
            toggle.isOn = _vesselMovementTypes[i] == type;
            toggle.interactable = CanSetMovementType(_vesselMovementTypes[i]);
        }
    }

    private string GetSelectedMovementType()
    {
        for (var i = 0; i < MovementTypeToggles.Length; i++)
            if (MovementTypeToggles[i].isOn)
                return _vesselMovementTypes[i];

        return vesselMovements.NoType;
    }

    private bool CanSetMovementType(string type)
    {
        // Don't allow the intercept pin to intercept itself.
        if (Vessel.Id == vesselData.InterceptId && type == vesselMovements.InterceptType)
            return false;

        return true;
    }

    private void OnMovementTypeToggled(string type, bool value)
    {
        if (_updating)
            return;

        // Request an update to the movement type.
        serverUtils.PostVesselMovementType(Vessel.Id, 
            value ? type : vesselMovements.NoType);

        InitMovementProperties();
    }

    private void OnMovementAutoSpeedToggleChanged(bool on)
    {
        if (_updating || !Intercept)
            return;

        Intercept.AutoSpeed = on;
        UpdateMovement();
    }

    private void OnMovementSpeedSliderChanged(float value)
    {
        if (_updating || !Movement)
            return;

        Movement.SetSpeed(value);
        MovementSpeedInput.text = string.Format("{0:N1}", value);
        UpdateMovement();
    }

    private void OnMovementSpeedInputChanged(string value)
    {
        if (_updating || !Movement)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        Movement.SetSpeed(result);
        MovementSpeedSlider.value = result;
        UpdateMovement();
    }

    private void OnMovementMaxSpeedSliderChanged(float value)
    {
        if (_updating || !Movement)
            return;

        Movement.SetMaxSpeed(value);

        // Max speed might have been clamped at some point (e.g.g must be >= 1).
        value = Movement.GetMaxSpeed();
        
        MovementSpeedSlider.maxValue = value;
        MovementMaxSpeedInput.text = string.Format("{0:N1}", value);
        UpdateMovement();
    }

    private void OnMovementMaxSpeedInputChanged(string value)
    {
        if (_updating || !Movement)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        result = Mathf.Max(result, vesselMovement.LowestMaxSpeed);
        
        Movement.SetMaxSpeed(result);
        MovementSpeedSlider.maxValue = result;
        MovementMaxSpeedSlider.maxValue = Mathf.Max(MovementMaxSpeedSlider.maxValue, result);
        MovementMaxSpeedSlider.value = result;
        UpdateMovement();
    }

    private void OnMovementHeadingSliderChanged(float value)
    {
        if (_updating || !SetVector)
            return;

        SetVector.Heading = value;
        MovementHeadingInput.text = string.Format("{0:N1}", value);
        UpdateMovement();
    }

    private void OnMovementHeadingInputChanged(string value)
    {
        if (_updating || !SetVector)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        SetVector.Heading = result;
        MovementHeadingSlider.value = result;
        UpdateMovement();
    }

    private void OnMovementDiveAngleSliderChanged(float value)
    {
        if (_updating || !SetVector)
            return;

        SetVector.DiveAngle = value;
        MovementDiveAngleInput.text = string.Format("{0:N1}", value);
        UpdateMovement();
    }

    private void OnMovementDiveAngleInputChanged(string value)
    {
        if (_updating || !SetVector)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        SetVector.DiveAngle = result;
        MovementDiveAngleSlider.value = result;
        UpdateMovement();
    }

    private void OnMovementDialChanged()
    {
        if (_updating || !SetVector)
            return;

        _updating = true;
        MovementSpeedSlider.value = SetVector.Speed;
        MovementSpeedInput.text = string.Format("{0:N1}", Movement.GetSpeed());
        MovementHeadingSlider.value = SetVector.Heading;
        MovementHeadingInput.text = string.Format("{0:N1}", SetVector.Heading);
        _updating = false;
    }


    private void OnMovementPeriodSliderChanged(float value)
    {
        if (_updating || !Holding)
            return;

        Holding.Period = value;
        MovementPeriodInput.text = string.Format("{0:N1}", value);
        UpdateMovement();
    }

    private void OnMovementPeriodInputChanged(string value)
    {
        if (_updating || !Holding)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        Holding.Period = result;
        MovementPeriodSlider.value = result;
        UpdateMovement();
    }

    private void OnMovementTargetSliderChanged(float value)
    {
        if (_updating || !Pursue)
            return;

        Pursue.TargetVessel = Mathf.RoundToInt(value);
        MovementTargetInput.text = string.Format("{0:N0}", value);
        UpdateMovement();
    }

    private void OnMovementTargetInputChanged(string value)
    {
        if (_updating || !Pursue)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        Pursue.TargetVessel = Mathf.RoundToInt(result);
        MovementTargetSlider.value = result;
        UpdateMovement();
    }

    private void OnEtaSliderChanged(float value)
    {
        if (_updating)
            return;

        var eta = Mathf.RoundToInt(EtaHoursSlider.value) * 3600 
            + Mathf.RoundToInt(EtaMinutesSlider.value) * 60 
            + Mathf.RoundToInt(EtaSecondsSlider.value);

        Movements.SetTimeToIntercept(eta);
        InitEtaUi();
        UpdateMovement();
    }

    private void OnEtaInputChanged(string value)
    {
        if (_updating)
            return;

        float hours, minutes = 0, seconds = 0;
        var parsed = float.TryParse(EtaHoursInput.text, out hours)
            && float.TryParse(EtaMinutesInput.text, out minutes)
            && float.TryParse(EtaSecondsInput.text, out seconds);

        if (!parsed)
            return;
        
        var eta = hours * 3600 + minutes * 60 + seconds;
        Movements.SetTimeToIntercept(eta);
        InitEtaUi();
        UpdateMovement();
    }

    private void InitEtaUi()
    {
        _updating = true;

        var t = TimeSpan.FromSeconds(Movements.TimeToIntercept);

        EtaGroup.interactable = !megEventManager.Instance.Playing;
        EtaHeader.text = string.Format("{0:00}:{1:00}:{2:00}", t.Hours, t.Minutes, t.Seconds);
        EtaHoursInput.text = string.Format("{0:00}", t.Hours);
        EtaMinutesInput.text = string.Format("{0:00}", t.Minutes);
        EtaSecondsInput.text = string.Format("{0:00}", t.Seconds);
        EtaHoursSlider.value = t.Hours;
        EtaMinutesSlider.value = t.Minutes;
        EtaSecondsSlider.value = t.Seconds;

        _updating = false;
    }

}
