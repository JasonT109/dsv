using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
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


    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Name label. */
    public Text Name;

    /** Canvas group. */
    public CanvasGroup CanvasGroup;

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


    /** The event being represented. */
    public megEvent Event
    {
        get { return _event; }
        set { SetEvent(value); }
    }

    /** Event interpreted as a server value event. */
    public megEventValue ValueEvent
        { get { return _event as megEventValue; } }


    // Members
    // ------------------------------------------------------------

    /** The event. */
    private megEvent _event;

    /** Whether UI is being initialized. */
    private bool _initializing;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        ClearUi();
    }

    /** Updating. */
    private void Update()
    {
        if (_event != null)
            UpdateUi();
    }


    // Private Methods
    // ------------------------------------------------------------

    private void SetEvent(megEvent value)
    {
        _event = value;

        if (_event != null)
        {
            InitUi();
            UpdateUi();
        }
        else
            ClearUi();
    }

    private void InitUi()
    {
        _initializing = true;

        UpdateBaseProperties();

        if (_event is megEventValue)
            UpdateValueProperties();

        _initializing = false;
    }

    private void UpdateUi()
    {
        if (_event == null)
            return;

        Name.text = _event.ToString();
        BaseProperties.gameObject.SetActive(true);
        ValueProperties.gameObject.SetActive(_event is megEventValue);
        CanvasGroup.interactable = !_event.file.playing;

        /*
        TriggerTimeSlider.interactable = editable;
        TriggerTimeInput.interactable = editable;
        CompleteTimeSlider.interactable = editable;
        CompleteTimeInput.interactable = editable;
        ServerParamInput.interactable = editable;
        */
    }

    private void ClearUi()
    {
        Name.text = "";
        BaseProperties.gameObject.SetActive(false);
        ValueProperties.gameObject.SetActive(false);
    }


    // Base Event Properties
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



    // Value Event Properties
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

    /** Update the event's trigger time from input field. */
    public void ServerParamInputChanged(string value)
    {
        if (_initializing)
            return;

        ValueEvent.serverParam = value;
    }

    /** Update the event's trigger time from slider. */
    public void ServerValueSliderChanged(float value)
    {
        if (_initializing)
            return;

        ValueEvent.serverValue = value;
        UpdateServerValueInput();
    }

    /** Update the event's trigger time from input field. */
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


}
