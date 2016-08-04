using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Meg.EventSystem;
using Meg.Networking;
using Meg.UI;

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

    /** Default phyiscs magnitude slider length. */
    public const float DefaultMapCameraDistanceSliderLength = 1000.0f;


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
    public Slider TriggerTimeSlider;
    public InputField TriggerTimeInput;
    public Slider CompleteTimeSlider;
    public InputField CompleteTimeInput;


    [Header("Value Event Components")]

    public Transform ValueProperties;
    public InputField ServerParamInput;
    public Button ServerParamClearButton;
    public Transform ServerParamEntriesContainer;
    public megScrollRect ServerParamEntriesScrollView;
    public Slider ServerValueSlider;
    public InputField ServerValueInput;


    [Header("Physics Event Components")]

    public Transform PhysicsProperties;
    public InputField PhysicsDirectionXInput;
    public InputField PhysicsDirectionYInput;
    public InputField PhysicsDirectionZInput;
    public Slider PhysicsMagnitudeSlider;
    public InputField PhysicsMagnitudeInput;


    [Header("Map Camera Event Components")]

    public Transform MapCameraProperties;
    public InputField MapCameraEventNameInput;
    public CanvasGroup MapCameraStateGroup;
    public InputField MapCameraXInput;
    public InputField MapCameraYInput;
    public InputField MapCameraZInput;
    public Slider MapCameraPitchSlider;
    public InputField MapCameraPitchInput;
    public Slider MapCameraYawSlider;
    public InputField MapCameraYawInput;
    public Slider MapCameraDistanceSlider;
    public InputField MapCameraDistanceInput;
    public Button MapCameraCaptureButton;


    [Header("Sonar Event Components")]

    public Transform SonarProperties;
    public InputField SonarEventNameInput;
    public debugEventSonarWaypointsUi SonarWaypointsUi;


    [Header("Vessels Event Components")]

    public Transform VesselsProperties;
    public Button VesselsCaptureButton;


    [Header("Prefabs")]

    /** Server param entry UI. */
    public debugServerParamEntryUi ServerParamEntryPrefab;


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

    /** Event interpreted as a vessels event. */
    public megEventVessels VesselsEvent
        { get { return _event as megEventVessels; } }

    /** Whether event is minimized. */
    public bool Minimized
        { get { return _event.minimized; } }


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

    /** List of server param UI entries. */
    private readonly List<debugServerParamEntryUi> _serverParams = new List<debugServerParamEntryUi>();


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

        InitBaseProperties();

        if (_event is megEventValue)
            InitValueProperties();
        else if (_event is megEventPhysics)
            InitPhysicsProperties();
        else if (_event is megEventMapCamera)
            InitMapCameraProperties();
        else if (_event is megEventSonar)
            InitSonarProperties();
        else if (_event is megEventVessels)
            InitVesselsProperties();

        _initializing = false;
    }

    private void UpdateUi()
    {
        if (_event == null)
            return;

        _updating = true;

        UpdateBaseProperties();

        if (_event is megEventValue)
            UpdateValueProperties();
        else if (_event is megEventPhysics)
            UpdatePhysicsProperties();
        else if (_event is megEventMapCamera)
            UpdateMapCameraProperties();
        else if (_event is megEventSonar)
            UpdateSonarProperties();
        else if (_event is megEventVessels)
            UpdateVesselsProperties();

        _updating = false;
    }

    private void ClearUi()
    {
        Name.text = "";
        BaseProperties.gameObject.SetActive(false);
        ValueProperties.gameObject.SetActive(false);
        PhysicsProperties.gameObject.SetActive(false);
        SonarProperties.gameObject.SetActive(false);
        VesselsProperties.gameObject.SetActive(false);
    }


    // Base Event Interface
    // ------------------------------------------------------------

    private void InitBaseProperties()
    {
        UpdateTriggerTimeSlider();
        UpdateTriggerTimeInput();
        UpdateCompleteTimeSlider();
        UpdateCompleteTimeInput();
    }

    private void UpdateBaseProperties()
    {
        var minimized = _event.minimized;
        MinimizeToggle.isOn = !minimized;
        HeaderToggle.isOn = !minimized;
        Name.text = _event.name;

        CanvasGroup.interactable = !_event.file.playing || _event.group.paused;
        BaseProperties.gameObject.SetActive(!minimized);
        ValueProperties.gameObject.SetActive(!minimized && ValueEvent != null);
        PhysicsProperties.gameObject.SetActive(!minimized && PhysicsEvent != null);
        MapCameraProperties.gameObject.SetActive(!minimized && MapCameraEvent != null);
        SonarProperties.gameObject.SetActive(!minimized && SonarEvent != null);
        VesselsProperties.gameObject.SetActive(!minimized && VesselsEvent != null);
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

    private void InitValueProperties()
    {
        UpdateServerParamInput();
        UpdateServerValueSlider();
        UpdateServerValueInput();
    }

    private void UpdateValueProperties()
    {
        // ServerParamClearButton.interactable = !string.IsNullOrEmpty(ValueEvent.serverParam);

        // Update the state of the server parameter list UI.
        if (_event.minimized)
        {
            HideServerParamList();
            DeselectServerParamInput();
        }
        else if (ServerParamInput.isFocused)
            ShowServerParamList();
    }

    private void DeselectServerParamInput()
    {
        if (!ServerParamInput.isFocused)
            return;

        var eventSystem = GameObject.Find("EventSystem");
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
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
        ServerValueInput.text = string.Format("{0:N2}", ValueEvent.serverValue);
    }

    public void ServerParamInputChanged(string value)
    {
        if (_initializing)
            return;

        UpdateServerParamList(value);
    }

    public void ServerParamInputEndEdit(string value)
    {
        if (_initializing)
            return;

        ValueEvent.serverParam = value;
        UpdateServerParamList(value);
    }

    public void ServerParamClear()
    {
        if (_initializing)
            return;

        if (!string.IsNullOrEmpty(ValueEvent.serverParam))
        {
            ValueEvent.serverParam = "";
            UpdateServerParamInput();
        }
        else
            HideServerParamList();
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

    private void UpdateServerParamList(string value = null)
    {
        var current = ValueEvent.serverParam;
        var prefix = (value ?? current);
        var index = 0;
        var focus = 0;
        foreach (var param in serverUtils.Parameters)
        {
            var entry = GetServerParamEntry(index);
            var on = string.Equals(param, current, StringComparison.OrdinalIgnoreCase);
            entry.Text.text = param;
            entry.On.gameObject.SetActive(on);
            if (string.CompareOrdinal(prefix, param) >= 0)
                focus = index;

            index++;
        }

        for (var i = 0; i < _serverParams.Count; i++)
            _serverParams[i].gameObject.SetActive(i < index);

        ServerParamEntriesScrollView.CenterOnItem(
            GetServerParamEntry(focus).GetComponent<RectTransform>());
    }

    private bool IsParameterPrefixed(string param, string prefix)
        { return param.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase); }

    private debugServerParamEntryUi GetServerParamEntry(int i)
    {
        if (i >= _serverParams.Count)
        {
            var ui = Instantiate(ServerParamEntryPrefab);
            ui.transform.SetParent(ServerParamEntriesContainer, false);
            ui.Button.onClick.AddListener(() => HandleServerParamSelected(ui));
            _serverParams.Add(ui);
        }

        return _serverParams[i];
    }

    private void HandleServerParamSelected(debugServerParamEntryUi selected)
    {
        if (_initializing)
            return;

        ValueEvent.serverParam = selected.Text.text;
        HideServerParamList();

        _initializing = true;
        UpdateServerParamInput();
        _initializing = false;
    }

    private void ShowServerParamList()
    {
        if (ServerParamEntriesScrollView.gameObject.activeSelf)
            return;

        ServerParamEntriesScrollView.gameObject.SetActive(true);
        UpdateServerParamList();
    }

    private void HideServerParamList()
        { ServerParamEntriesScrollView.gameObject.SetActive(false); }


    // Physics Event Interface
    // ------------------------------------------------------------

    private void InitPhysicsProperties()
    {
        UpdatePhysicsDirectionInputs();
        UpdatePhysicsMagnitudeSlider();
        UpdatePhysicsMagnitudeInput();
    }

    private void UpdatePhysicsProperties()
    {
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

    private void InitMapCameraProperties()
    {
        UpdateMapCameraEventNameInput();
        UpdateMapCameraPositionInputs();
        UpdateMapCameraPitchSlider();
        UpdateMapCameraPitchInput();
        UpdateMapCameraYawSlider();
        UpdateMapCameraYawInput();
        UpdateMapCameraDistanceSlider();
        UpdateMapCameraDistanceInput();
    }


    private void UpdateMapCameraProperties()
    {
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

    private void UpdateMapCameraPositionInputs()
    {
        MapCameraXInput.text = string.Format("{0:N1}", MapCameraEvent.toPosition.x);
        MapCameraYInput.text = string.Format("{0:N1}", MapCameraEvent.toPosition.y);
        MapCameraZInput.text = string.Format("{0:N1}", MapCameraEvent.toPosition.z);
    }

    public void MapCameraXInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        MapCameraEvent.toPosition.x = result;
    }

    public void MapCameraYInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        MapCameraEvent.toPosition.y = result;
    }

    public void MapCameraZInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        MapCameraEvent.toPosition.z = result;
    }

    private void UpdateMapCameraPitchSlider()
    {
        MapCameraPitchSlider.value = MapCameraEvent.toOrientation.x;
    }

    private void UpdateMapCameraPitchInput()
    {
        MapCameraPitchInput.text = string.Format("{0:N1}", MapCameraEvent.toOrientation.x);
    }

    public void MapCameraPitchInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        MapCameraEvent.toOrientation.x = result;
        UpdateMapCameraPitchSlider();
    }

    public void MapCameraPitchSliderChanged(float value)
    {
        if (_initializing)
            return;

        MapCameraEvent.toOrientation.x = value;
        UpdateMapCameraPitchInput();
    }

    private void UpdateMapCameraYawSlider()
    {
        MapCameraYawSlider.value = MapCameraEvent.toOrientation.y;
    }

    private void UpdateMapCameraYawInput()
    {
        MapCameraYawInput.text = string.Format("{0:N1}", MapCameraEvent.toOrientation.y);
    }

    public void MapCameraYawInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        MapCameraEvent.toOrientation.y = result;
        UpdateMapCameraYawSlider();
    }

    public void MapCameraYawSliderChanged(float value)
    {
        if (_initializing)
            return;

        MapCameraEvent.toOrientation.y = value;
        UpdateMapCameraYawInput();
    }

    private void UpdateMapCameraDistanceSlider()
    {
        var distance = -MapCameraEvent.toZoom;
        var maxValue = Mathf.Max(distance, DefaultMapCameraDistanceSliderLength);
        MapCameraDistanceSlider.maxValue = maxValue;
        MapCameraDistanceSlider.value = distance;
    }

    private void UpdateMapCameraDistanceInput()
    {
        var distance = -MapCameraEvent.toZoom;
        MapCameraDistanceInput.text = string.Format("{0:N1}", distance);
    }

    public void MapCameraDistanceSliderChanged(float value)
    {
        if (_initializing)
            return;

        MapCameraEvent.toZoom = -value;
        UpdateMapCameraDistanceInput();
    }

    public void MapCameraDistanceInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        MapCameraEvent.toZoom = -result;
        UpdateMapCameraDistanceSlider();
    }
    
    public void MapCameraCapture()
    {
        if (_initializing)
            return;

        MapCameraEvent.Capture();
        InitUi();
    }


    // Sonar Event Interface
    // ------------------------------------------------------------

    private void InitSonarProperties()
    {
        SonarWaypointsUi.Event = SonarEvent;
        UpdateSonarEventNameInput();
    }

    private void UpdateSonarProperties()
    {
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


    // Vessels Event Interface
    // ------------------------------------------------------------

    private void InitVesselsProperties()
    {
    }

    private void UpdateVesselsProperties()
    {
    }

    public void VesselsCapture()
    {
        if (_initializing)
            return;

        VesselsEvent.Capture();
    }

}
