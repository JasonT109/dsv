using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using Meg.EventSystem;

public class debugEventPropertiesUi : MonoBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** Default trigger time slider length. */
    public const float DefaultTriggerTimeSliderLength = 10.0f;

    /** Default complete time time slider length. */
    public const float DefaultCompleteTimeSliderLength = 10.0f;

    /** Default server value slider length. */
    public const float DefaultServerValueSliderLength = 100.0f;

    /** Default phyiscs magnitude slider length. */
    public const float DefaultPhysicsMagnitudeSliderLength = 1.0f;


    // Configuration
    // ------------------------------------------------------------

    [Header("Components")]

    /** Name label. */
    public Text Name;

    /** Canvas group. */
    public CanvasGroup CanvasGroup;

    /** Header toggle. */
    public Toggle HeaderToggle;

    /** Minimize toggle. */
    public Toggle MinimizeToggle;


    [Header("Base Event Components")]

    /** Basic properties panel. */
    public Transform BaseProperties;

    /** Trigger time slider. */
    public Slider TriggerTimeSlider;

    /** Trigger time input text. */
    public InputField TriggerTimeInput;

    /** Completion time slider. */
    public Slider CompleteTimeSlider;

    /** Completion time input text. */
    public InputField CompleteTimeInput;


    [Header("Value Event Components")]

    /** Properties panel for value events. */
    public Transform ValueProperties;

    /** Server parameter input text. */
    public InputField ServerParamInput;

    /** Server value slider. */
    public Slider ServerValueSlider;

    /** Server value input text. */
    public InputField ServerValueInput;


    [Header("Physics Event Components")]

    /** Properties panel for physics events. */
    public Transform PhysicsProperties;

    /** Input text for physics direction components. */
    public InputField PhysicsDirectionXInput;
    public InputField PhysicsDirectionYInput;
    public InputField PhysicsDirectionZInput;

    /** Physics magnitude slider. */
    public Slider PhysicsMagnitudeSlider;

    /** Server value input text. */
    public InputField PhysicsMagnitudeInput;


    [Header("Map Camera Event Components")]

    /** Properties panel for map camera events. */
    public Transform MapCameraProperties;

    /** Input text for map camera event name. */
    public InputField MapCameraEventNameInput;


    [Header("Sonar Event Components")]

    /** Properties panel for sonar events. */
    public Transform SonarProperties;

    /** Input text for sonar event name. */
    public InputField SonarEventNameInput;


    // Properties
    // ------------------------------------------------------------

    /** The event being represented. */
    public megEvent Event
    {
        get { return _event; }
        set { SetEvent(value); }
    }

    /** Whether an event is set. */
    public bool HasEvent
        { get { return _event != null; } }

    /** Event interpreted as a value event. */
    public megEventValue ValueEvent
        { get { return _event as megEventValue; } }

    /** Event interpreted as a physics event. */
    public megEventPhysics PhysicsEvent
        { get { return _event as megEventPhysics; } }

    /** Event interpreted as a map camera event. */
    public megEventMapCamera MapCameraEvent
        { get { return _event as megEventMapCamera; } }

    /** Event interpreted as a sonar event. */
    public megEventSonar SonarEvent
        { get { return _event as megEventSonar; } }

    /** Whether event is minimized. */
    public bool Minimized
    {
        get { return _event.minimized; }
        set { SetMinimized(value); }
    }


    // Events
    // ------------------------------------------------------------

    /** Event signature for a event selection. */
    public delegate void EventSelectedHandler(debugEventPropertiesUi eventUi);

    /** Selection event. */
    public event EventSelectedHandler OnSelected;


    // Members
    // ------------------------------------------------------------

    /** The event. */
    private megEvent _event;

    /** Whether UI is being initialized. */
    private bool _initializing;

    /** Whether UI is being updated. */
    private bool _updating;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        ConfigureUi();
        ClearUi();
    }

    /** Updating. */
    private void Update()
    {
        if (_event != null)
            UpdateUi();
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Select this event. */
    public void Select()
    {
        if (_initializing || _updating)
            return;

        if (OnSelected != null)
            OnSelected(this);
    }

    /** Toggle event's minimized state. */
    public void ToggleMinimized()
    {
        if (_initializing || _updating)
            return;

        Minimized = !Minimized;
    }

    /** Set timeline's minimized state. */
    public void SetMinimized(bool value)
    {
        if (_initializing || _updating)
            return;

        _event.minimized = value;
    }


    // Private Methods
    // ------------------------------------------------------------

    private void SetEvent(megEvent value)
    {
        if (_event == value)
            return;

        _event = value;

        if (_event != null)
        {
            InitUi();
            UpdateUi();
        }
        else
            ClearUi();
    }

    private void ConfigureUi()
    {
        ServerParamInput.onValidateInput += ValidateIdentifierInput;
        MapCameraEventNameInput.onValidateInput += ValidateIdentifierInput;
    }

    private char ValidateIdentifierInput(string text, int index, char addedChar)
    {
        if (!Regex.IsMatch(addedChar.ToString(), "[A-Za-z0-9_]"))
            return '\0';

        return addedChar;
    }

    private void InitUi()
    {
        _initializing = true;

        UpdateBaseProperties();

        if (_event is megEventValue)
            UpdateValueProperties();
        else if (_event is megEventPhysics)
            UpdatePhysicsProperties();
        else if (_event is megEventMapCamera)
            UpdateMapCameraProperties();
        else if (_event is megEventSonar)
            UpdateSonarProperties();

        _initializing = false;
    }

    private void UpdateUi()
    {
        if (_event == null)
            return;

        _updating = true;

        var minimized = _event.minimized;
        MinimizeToggle.isOn = !minimized;
        HeaderToggle.isOn = !minimized;

        Name.text = _event.name;
        BaseProperties.gameObject.SetActive(!minimized);
        ValueProperties.gameObject.SetActive(_event is megEventValue && !minimized);
        PhysicsProperties.gameObject.SetActive(_event is megEventPhysics && !minimized);
        MapCameraProperties.gameObject.SetActive(_event is megEventMapCamera && !minimized);
        SonarProperties.gameObject.SetActive(_event is megEventSonar && !minimized);

        CanvasGroup.interactable = !_event.file.playing || _event.group.paused;

        _updating = false;
    }

    private void ClearUi()
    {
        Name.text = "";
        BaseProperties.gameObject.SetActive(false);
        ValueProperties.gameObject.SetActive(false);
        PhysicsProperties.gameObject.SetActive(false);
        SonarProperties.gameObject.SetActive(false);
    }


    // Base Event Interface
    // ------------------------------------------------------------

    private void UpdateBaseProperties()
    {
        UpdateTriggerTimeSlider();
        UpdateTriggerTimeInput();
        UpdateCompleteTimeSlider();
        UpdateCompleteTimeInput();
    }

    private void UpdateTriggerTimeSlider()
    {
        var maxValue = Mathf.Max(_event.group.endTime, DefaultTriggerTimeSliderLength);
        TriggerTimeSlider.maxValue = maxValue;
        TriggerTimeSlider.value = _event.triggerTime;
    }

    private void UpdateTriggerTimeInput()
    {
        TriggerTimeInput.text = string.Format("{0:N1}", _event.triggerTime);
    }

    private void UpdateCompleteTimeSlider()
    {
        var longest = _event.group.events.Max(e => e.completeTime);
        var maxValue = Mathf.Max(longest, DefaultCompleteTimeSliderLength);
        CompleteTimeSlider.maxValue = maxValue;
        CompleteTimeSlider.value = _event.completeTime;
    }

    private void UpdateCompleteTimeInput()
    {
        CompleteTimeInput.text = string.Format("{0:N1}", _event.completeTime);
    }

    /** Update the event's trigger time from slider. */
    public void TriggerTimeSliderChanged(float value)
    {
        if (_initializing)
            return;

        _event.triggerTime = value;
        UpdateTriggerTimeInput();
    }

    /** Update the event's trigger time from input field. */
    public void TriggerTimeInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        _event.triggerTime = result;
        UpdateTriggerTimeSlider();
    }

    /** Update the event's trigger time from slider. */
    public void CompleteTimeSliderChanged(float value)
    {
        if (_initializing)
            return;

        _event.completeTime = value;
        UpdateCompleteTimeInput();
    }

    /** Update the event's trigger time from input field. */
    public void CompleteTimeInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        _event.completeTime = result;
        UpdateCompleteTimeSlider();
    }


    // Value Event Interface
    // ------------------------------------------------------------

    private void UpdateValueProperties()
    {
        UpdateServerParamInput();
        UpdateServerValueSlider();
        UpdateServerValueInput();
    }

    private void UpdateServerParamInput()
    {
        ServerParamInput.text = ValueEvent.serverParam;
    }

    private void UpdateServerValueSlider()
    {
        var maxValue = Mathf.Max(ValueEvent.serverValue, DefaultServerValueSliderLength);
        ServerValueSlider.maxValue = maxValue;
        ServerValueSlider.value = ValueEvent.serverValue;
    }

    private void UpdateServerValueInput()
    {
        ServerValueInput.text = string.Format("{0:N1}", ValueEvent.serverValue);
    }

    public void ServerParamInputChanged(string value)
    {
        if (_initializing)
            return;

        ValueEvent.serverParam = value;
    }

    public void ServerValueSliderChanged(float value)
    {
        if (_initializing)
            return;

        ValueEvent.serverValue = value;
        UpdateServerValueInput();
    }

    public void ServerValueInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        ValueEvent.serverValue = result;
        UpdateServerValueSlider();
    }


    // Physics Event Interface
    // ------------------------------------------------------------

    private void UpdatePhysicsProperties()
    {
        UpdatePhysicsDirectionInputs();
        UpdatePhysicsMagnitudeSlider();
        UpdatePhysicsMagnitudeInput();
    }

    private void UpdatePhysicsDirectionInputs()
    {
        PhysicsDirectionXInput.text = string.Format("{0:N1}", PhysicsEvent.physicsDirection.x);
        PhysicsDirectionYInput.text = string.Format("{0:N1}", PhysicsEvent.physicsDirection.y);
        PhysicsDirectionZInput.text = string.Format("{0:N1}", PhysicsEvent.physicsDirection.z);
    }

    private void UpdatePhysicsMagnitudeSlider()
    {
        var maxValue = Mathf.Max(PhysicsEvent.physicsMagnitude, DefaultPhysicsMagnitudeSliderLength);
        PhysicsMagnitudeSlider.maxValue = maxValue;
        PhysicsMagnitudeSlider.value = PhysicsEvent.physicsMagnitude;
    }

    private void UpdatePhysicsMagnitudeInput()
    {
        PhysicsMagnitudeInput.text = string.Format("{0:N1}", PhysicsEvent.physicsMagnitude);
    }

    public void PhysicsDirectionXInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        PhysicsEvent.physicsDirection.x = result;
    }

    public void PhysicsDirectionYInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        PhysicsEvent.physicsDirection.y = result;
    }

    public void PhysicsDirectionZInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        PhysicsEvent.physicsDirection.z = result;
    }

    public void PhysicsMagnitudeSliderChanged(float value)
    {
        if (_initializing)
            return;

        PhysicsEvent.physicsMagnitude = value;
        UpdatePhysicsMagnitudeInput();
    }

    public void PhysicsMagnitudeInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        PhysicsEvent.physicsMagnitude = result;
        UpdatePhysicsMagnitudeSlider();
    }


    // Map Camera Event Interface
    // ------------------------------------------------------------

    private void UpdateMapCameraProperties()
    {
        UpdateMapCameraEventNameInput();
    }

    private void UpdateMapCameraEventNameInput()
    {
        MapCameraEventNameInput.text = MapCameraEvent.eventName;
    }

    public void MapCameraEventNameInputChanged(string value)
    {
        if (_initializing)
            return;

        MapCameraEvent.eventName = value;
    }


    // Sonar Event Interface
    // ------------------------------------------------------------

    private void UpdateSonarProperties()
    {
        UpdateSonarEventNameInput();
    }

    private void UpdateSonarEventNameInput()
    {
        SonarEventNameInput.text = SonarEvent.eventName;
    }

    public void SonarEventNameInputChanged(string value)
    {
        if (_initializing)
            return;

        SonarEvent.eventName = value;
    }

}
