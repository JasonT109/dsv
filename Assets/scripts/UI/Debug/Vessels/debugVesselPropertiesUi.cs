using UnityEngine;
using System.Collections;
using Meg.Networking;
using UnityEngine.UI;

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


    [Header("Movement Components")]
    public Transform MovementProperties;
    public Toggle[] MovementTypeToggles;
    public Transform MovementStateProperties;
    public Toggle MovementAutoSpeedToggle;
    public CanvasGroup MovementSpeedGroup;
    public Slider MovementSpeedSlider;
    public InputField MovementSpeedInput;
    public Slider MovementHeadingSlider;
    public InputField MovementHeadingInput;
    public Slider MovementDiveAngleSlider;
    public InputField MovementDiveAngleInput;
    public Slider MovementPeriodSlider;
    public InputField MovementPeriodInput;
    public Slider MovementTargetSlider;
    public InputField MovementTargetInput;

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


    // Members
    // ------------------------------------------------------------

    /** The vessel being edited. */
    private vesselData.Vessel _vessel;

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

        InitMovementProperties();
    }

    private void UpdateUi()
    {
        _updating = true;
        UpdateMovementProperties();
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


    // Movements Interface
    // ------------------------------------------------------------

    private vesselMovements Movements
        { get { return serverUtils.VesselMovements; } }

    private vesselMovement Movement
        { get { return Movements.GetVesselMovement(Vessel.Id); } }

    private vesselIntercept Intercept
        { get { return Movement as vesselIntercept; } }

    private vesselPursue Pursue
        { get { return Movement as vesselPursue; } }

    private vesselSetVector SetVector
        { get { return Movement as vesselSetVector; } }

    private vesselHoldingPattern Holding
        { get { return Movement as vesselHoldingPattern; } }

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
        MovementHeadingSlider.onValueChanged.AddListener(OnMovementHeadingSliderChanged);
        MovementHeadingInput.onEndEdit.AddListener(OnMovementHeadingInputChanged);
        MovementDiveAngleSlider.onValueChanged.AddListener(OnMovementDiveAngleSliderChanged);
        MovementDiveAngleInput.onEndEdit.AddListener(OnMovementDiveAngleInputChanged);
        MovementPeriodSlider.onValueChanged.AddListener(OnMovementPeriodSliderChanged);
        MovementPeriodInput.onEndEdit.AddListener(OnMovementPeriodInputChanged);
        MovementTargetSlider.onValueChanged.AddListener(OnMovementTargetSliderChanged);
        MovementTargetInput.onEndEdit.AddListener(OnMovementTargetInputChanged);
    }

    private void InitMovementProperties()
    {
        _updating = true;

        UpdateMovementTypeToggles();
        MovementStateProperties.gameObject.SetActive(Movement);
        if (!Movement)
            return;

        MovementAutoSpeedToggle.isOn = Intercept && Intercept.AutoSpeed;
        MovementSpeedSlider.value = Movement.GetSpeed();
        MovementSpeedSlider.maxValue = Mathf.Max(MovementSpeedSlider.maxValue, Movement.GetSpeed());
        MovementSpeedInput.text = string.Format("{0:N1}", Movement.GetSpeed());

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
    }

    private void UpdateMovementProperties()
    {
        MovementStateProperties.gameObject.SetActive(Movement != null);
        MovementAutoSpeedToggle.gameObject.SetActive(Intercept);
        MovementSpeedGroup.interactable = !MovementAutoSpeedToggle.isOn;
        MovementSpeedGroup.alpha = MovementAutoSpeedToggle.isOn ? 0.5f : 1;
        MovementHeadingSlider.transform.parent.gameObject.SetActive(SetVector);
        MovementDiveAngleSlider.transform.parent.gameObject.SetActive(SetVector);
        MovementPeriodSlider.transform.parent.gameObject.SetActive(Holding);
        MovementTargetSlider.transform.parent.gameObject.SetActive(Pursue);
    }

    private void UpdateMovementTypeToggles()
    {
        var type = Movement ? Movement.Type : vesselMovements.NoType;
        for (var i = 0; i < MovementTypeToggles.Length; i++)
        {
            var toggle = MovementTypeToggles[i];
            toggle.isOn = _vesselMovementTypes[i] == type;
        }
    }

    private void OnMovementTypeToggled(string type, bool value)
    {
        if (_updating)
            return;

        Movements.SetMovementType(Vessel.Id, value ? type : vesselMovements.NoType);
        InitMovementProperties();
    }

    private void OnMovementAutoSpeedToggleChanged(bool on)
    {
        if (_updating || !Intercept)
            return;

        Intercept.AutoSpeed = on;
    }

    private void OnMovementSpeedSliderChanged(float value)
    {
        if (_updating || !Movement)
            return;

        Movement.SetSpeed(value);
        MovementSpeedInput.text = string.Format("{0:N1}", value);
    }

    private void OnMovementSpeedInputChanged(string value)
    {
        if (_updating || !Movement)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        Movement.SetSpeed(result);
        MovementSpeedSlider.maxValue = Mathf.Max(MovementSpeedSlider.maxValue, result);
        MovementSpeedSlider.value = result;
    }

    private void OnMovementHeadingSliderChanged(float value)
    {
        if (_updating || !SetVector)
            return;

        SetVector.Heading = value;
        MovementHeadingInput.text = string.Format("{0:N1}", value);
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
    }

    private void OnMovementDiveAngleSliderChanged(float value)
    {
        if (_updating || !SetVector)
            return;

        SetVector.DiveAngle = value;
        MovementDiveAngleInput.text = string.Format("{0:N1}", value);
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
    }

    private void OnMovementPeriodSliderChanged(float value)
    {
        if (_updating || !Holding)
            return;

        Holding.Period = value;
        MovementPeriodInput.text = string.Format("{0:N1}", value);
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
    }

    private void OnMovementTargetSliderChanged(float value)
    {
        if (_updating || !Pursue)
            return;

        Pursue.TargetVessel = Mathf.RoundToInt(value);
        MovementTargetInput.text = string.Format("{0:N0}", value);
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
    }


}
