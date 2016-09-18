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
using UnityEngine.EventSystems;

public class debugEventPropertiesUi : MonoBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** Default trigger time slider length. */
    public const float DefaultTriggerTimeSliderLength = 10.0f;

    /** Default complete time time slider length. */
    public const float DefaultCompleteTimeSliderLength = 10.0f;

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

    public Transform TriggerProperties;
    public InputField TriggerLabelInput;
    public InputField TriggerHotKeyInput;

    [Header("Value Event Components")]

    public Transform ValueProperties;
    public InputField ServerParamInput;
    public Button ServerParamClearButton;
    public Transform ServerParamEntriesContainer;
    public megScrollRect ServerParamEntriesScrollView;
    public Button ServerParamUpdateButton;
    public Slider ServerValueSlider;
    public InputField ServerValueInput;
    public Toggle ApplyInitialValueToggle;
    public InputField InitialValueInput;


    [Header("Physics Event Components")]

    public Transform PhysicsProperties;
    public Slider PhysicsDirectionXSlider;
    public Slider PhysicsDirectionYSlider;
    public Slider PhysicsDirectionZSlider;
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

    public Transform VesselProperties;
    public Slider VesselIdSlider;
    public InputField VesselIdInput;
    public Text VesselNameLabel;
    public Toggle[] VesselTypeToggles;
    public Transform VesselStateProperties;
    public Toggle VesselAutoSpeedToggle;
    public CanvasGroup VesselSpeedGroup;
    public Slider VesselSpeedSlider;
    public InputField VesselSpeedInput;
    public Slider VesselHeadingSlider;
    public InputField VesselHeadingInput;
    public Slider VesselDiveAngleSlider;
    public InputField VesselDiveAngleInput;
    public Slider VesselPeriodSlider;
    public InputField VesselPeriodInput;
    public Text VesselTargetLabel;
    public Transform VesselTargets;
    public Slider VesselTargetSlider;
    public InputField VesselTargetInput;
    public Button VesselCaptureButton;


    [Header("Popup Event Components")]

    public Transform PopupProperties;
    public Toggle[] PopupTypeToggles;
    public Text PopupTypeLabel;
    public Button PopupTypeSelect;
    public InputField PopupTitleInput;
    public InputField PopupMessageInput;
    public InputField PopupTargetInput;
    public InputField PopupXInput;
    public InputField PopupYInput;
    public InputField PopupZInput;
    public InputField PopupWidthInput;
    public InputField PopupHeightInput;
    public Toggle[] PopupIconToggles;
    public Toggle[] PopupColorToggles;


    [Header("Prefabs")]

    /** Server param entry UI. */
    public debugServerParamEntryUi ServerParamEntryPrefab;

    /** The group UI that owns this event. */
    public debugEventGroupPropertiesUi GroupUi { get; set; }


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
    public megEventVesselMovement VesselEvent
        { get { return _event as megEventVesselMovement; } }

    /** Event interpreted as a popup event. */
    public megEventPopup PopupEvent
        { get { return _event as megEventPopup; } }

    /** Whether event is minimized. */
    public bool Minimized
        { get { return _event.minimized; } }

    /** Vessel data. */
    private vesselData VesselData
        { get { return serverUtils.VesselData; } }


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

    /** Types of vessel movement. */
    private readonly string[] _vesselMovementTypes = {
        vesselMovements.InterceptType,
        vesselMovements.PursueType,
        vesselMovements.SetVectorType,
        vesselMovements.HoldingType };


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
        TriggerLabelInput.onEndEdit.AddListener(TriggerLabelInputChanged);
        TriggerHotKeyInput.onEndEdit.AddListener(TriggerHotKeyInputChanged);
        ServerParamInput.onValidateInput += ValidateIdentifierInput;
        MapCameraEventNameInput.onValidateInput += ValidateIdentifierInput;
        ConfigureVesselsProperties();
        ConfigurePopupProperties();
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
        else if (_event is megEventVesselMovement)
            InitVesselsProperties();
        else if (_event is megEventPopup)
            InitPopupProperties();

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
        else if (_event is megEventVesselMovement)
            UpdateVesselsProperties();
        else if (_event is megEventPopup)
            UpdatePopupProperties();

        _updating = false;
    }

    private void ClearUi()
    {
        Name.text = "";
        TriggerProperties.gameObject.SetActive(false);
        BaseProperties.gameObject.SetActive(false);
        ValueProperties.gameObject.SetActive(false);
        PhysicsProperties.gameObject.SetActive(false);
        SonarProperties.gameObject.SetActive(false);
        VesselProperties.gameObject.SetActive(false);
        PopupProperties.gameObject.SetActive(false);
    }


    // Base Event Interface
    // ------------------------------------------------------------

    private void InitBaseProperties()
    {
        UpdateTriggerTimeSlider();
        UpdateTriggerTimeInput();
        UpdateCompleteTimeSlider();
        UpdateCompleteTimeInput();
        UpdateTriggerLabelInput();
        UpdateTriggerHotKeyInput();
    }

    private void UpdateBaseProperties()
    {
        var minimized = _event.minimized;
        MinimizeToggle.isOn = !minimized;
        HeaderToggle.isOn = !minimized;
        Name.text = _event.name;

        CanvasGroup.interactable = !_event.file.playing || _event.group.paused;
        BaseProperties.gameObject.SetActive(!minimized);
        TriggerProperties.gameObject.SetActive(!minimized);
        ValueProperties.gameObject.SetActive(!minimized && ValueEvent != null);
        PhysicsProperties.gameObject.SetActive(!minimized && PhysicsEvent != null);
        MapCameraProperties.gameObject.SetActive(!minimized && MapCameraEvent != null);
        SonarProperties.gameObject.SetActive(!minimized && SonarEvent != null);
        VesselProperties.gameObject.SetActive(!minimized && VesselEvent != null);
        PopupProperties.gameObject.SetActive(!minimized && PopupEvent != null);
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

    /** Update the event's trigger label input. */
    private void UpdateTriggerLabelInput()
    {
        TriggerLabelInput.text = _event.hasTrigger ? _event.triggerLabel : "";
        TriggerHotKeyInput.interactable = _event.hasTrigger;
    }

    /** Update the event's trigger label from input field. */
    public void TriggerLabelInputChanged(string value)
    {
        if (_initializing)
            return;

        _event.triggerLabel = value;
        TriggerHotKeyInput.interactable = _event.hasTrigger;
    }

    /** Update the event's trigger label input. */
    private void UpdateTriggerHotKeyInput()
    {
        TriggerHotKeyInput.interactable = _event.hasTrigger;
        if (string.IsNullOrEmpty(_event.triggerHotKey))
            TriggerHotKeyInput.text = "";
        else
            TriggerHotKeyInput.text = _event.triggerHotKey;
    }

    /** Update the event's trigger label from input field. */
    public void TriggerHotKeyInputChanged(string value)
    {
        if (_initializing)
            return;

        _event.triggerHotKey = value;
    }


    // Value Event Interface
    // ------------------------------------------------------------

    private void InitValueProperties()
    {
        UpdateServerParamInput();
        UpdateServerValueSlider();
        UpdateServerValueInput();
        UpdateInitialValueInput();
        UpdateApplyInitialValueToggle();
    }

    private void UpdateValueProperties()
    {
        // ServerParamClearButton.interactable = !string.IsNullOrEmpty(ValueEvent.serverParam);
        if (GroupUi)
            ServerParamUpdateButton.interactable = GroupUi.InfoView.activeSelf;

        // Update the state of the server parameter list UI.
        if (_event.minimized)
        {
            HideServerParamList();
            DeselectServerParamInput();
        }
    }

    private void DeselectServerParamInput()
    {
        if (!ServerParamInput.isFocused)
            return;

        EventSystem.SetSelectedGameObject(null);
    }

    private EventSystem EventSystem
        { get { return GameObject.Find("EventSystem").GetComponent<EventSystem>(); } }

    private void UpdateServerParamInput()
    {
        ServerParamInput.text = ValueEvent.serverParam;
    }

    private void UpdateServerValueSlider()
    {
        var key = ValueEvent.serverParam;
        var value = ValueEvent.serverValue;
        var info = serverUtils.GetServerDataInfo(key);
        var minValue = Mathf.Min(value, info.minValue);
        var maxValue = Mathf.Max(value, info.maxValue);

        ServerValueSlider.minValue = minValue;
        ServerValueSlider.maxValue = maxValue;
        ServerValueSlider.value = value;
        ServerValueSlider.wholeNumbers = (info.type == serverUtils.ParameterType.Bool)
            || (info.type == serverUtils.ParameterType.Int);
    }

    private void UpdateServerValueInput()
    {
        var format = "{0:N" + Precision(ValueEvent.serverParam, ValueEvent.serverValue) + "}";
        ServerValueInput.text = string.Format(format, ValueEvent.serverValue);
    }

    private void UpdateInitialValueInput()
    {
        var format = "{0:N" + Precision(ValueEvent.serverParam, ValueEvent.initialValue) + "}";
        InitialValueInput.text = string.Format(format, ValueEvent.initialValue);
        InitialValueInput.gameObject.SetActive(ValueEvent.applyInitialValue);
    }

    private void UpdateApplyInitialValueToggle()
    {
        ApplyInitialValueToggle.isOn = ValueEvent.applyInitialValue;
    }

    public void ToggleParameterList()
    {
        if (ServerParamEntriesScrollView.gameObject.activeSelf)
            HideServerParamList();
        else
        {
            ShowServerParamList();
            EventSystem.SetSelectedGameObject(ServerParamEntriesScrollView.gameObject);
        }
    }

    public void UpdateServerParameterToSelectedInfo()
    {
        var selected = GroupUi.InfoList.Selected;
        if (_initializing || !selected)
            return;

        ValueEvent.serverParam = selected.Text.text;

        _initializing = true;
        UpdateServerParamInput();
        UpdateServerParamList(null, false);
        _initializing = false;
    }

    public void ServerParamInputChanged(string value)
    {
        if (_initializing)
            return;

        UpdateServerParamList(value);
    }

    public void ServerParamInputClicked()
    {
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

        ValueEvent.serverParam = "";
        UpdateServerParamInput();
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

    public void InitialValueInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        ValueEvent.initialValue = result;
    }

    public void ApplyInitialValueToggleChanged(bool value)
    {
        if (_initializing)
            return;

        ValueEvent.applyInitialValue = value;
        UpdateInitialValueInput();
    }

    private void UpdateServerParamList(string value = null, bool recenter = true)
    {
        if (!ServerParamEntriesScrollView.gameObject.activeSelf)
            return;

        var current = ValueEvent.serverParam;
        var prefix = (value ?? current);
        var index = 0;
        var focus = -1;
        var ordered = serverUtils.WriteableParameters
            .Where(p => !serverUtils.GetServerDataInfo(p).hideInUi)
            .OrderBy(key => key);

        foreach (var param in ordered)
        {
            var entry = GetServerParamEntry(index);
            var on = string.Equals(param, current, StringComparison.OrdinalIgnoreCase);
            entry.Text.text = param;
            entry.On.gameObject.SetActive(on);
            if (string.CompareOrdinal(prefix, param) <= 0 && focus < 0)
                focus = index;

            index++;
        }

        if (focus < 0)
            focus = 0;

        for (var i = 0; i < _serverParams.Count; i++)
            _serverParams[i].gameObject.SetActive(i < index);

        if (recenter)
            ServerParamEntriesScrollView.CenterOnItem(
                GetServerParamEntry(focus).GetComponent<RectTransform>(), 0.25f);
    }

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

        _initializing = true;
        UpdateServerParamInput();
        UpdateServerParamList(null, false);
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
        UpdatePhysicsDirectionSliders();
        UpdatePhysicsDirectionInputs();
        UpdatePhysicsMagnitudeSlider();
        UpdatePhysicsMagnitudeInput();
    }

    private void UpdatePhysicsProperties()
    {
    }

    private void UpdatePhysicsDirectionSliders()
    {
        PhysicsDirectionXSlider.value = PhysicsEvent.physicsDirection.x;
        PhysicsDirectionYSlider.value = PhysicsEvent.physicsDirection.y;
        PhysicsDirectionZSlider.value = PhysicsEvent.physicsDirection.z;
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

    public void PhysicsDirectionXSliderChanged(float value)
    {
        var d = PhysicsEvent.physicsDirection;
        PhysicsDirectionChanged(new Vector3(value, d.y, d.z));
    }

    public void PhysicsDirectionYSliderChanged(float value)
    {
        var d = PhysicsEvent.physicsDirection;
        PhysicsDirectionChanged(new Vector3(d.x, value, d.z));
    }

    public void PhysicsDirectionZSliderChanged(float value)
    {
        var d = PhysicsEvent.physicsDirection;
        PhysicsDirectionChanged(new Vector3(d.x, d.y, value));
    }

    private void PhysicsDirectionChanged(Vector3 v)
    {
        if (_initializing)
            return;
        
        PhysicsEvent.physicsDirection = v;
        UpdatePhysicsDirectionInputs();
    }

    public void PhysicsDirectionXInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        PhysicsEvent.physicsDirection.x = result;
        UpdatePhysicsDirectionSliders();
    }

    public void PhysicsDirectionYInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        PhysicsEvent.physicsDirection.y = result;
        UpdatePhysicsDirectionSliders();
    }

    public void PhysicsDirectionZInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        PhysicsEvent.physicsDirection.z = result;
        UpdatePhysicsDirectionSliders();
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
        _initializing = true;
        UpdateMapCameraEventNameInput();
        UpdateMapCameraPositionInputs();
        UpdateMapCameraPitchSlider();
        UpdateMapCameraPitchInput();
        UpdateMapCameraYawSlider();
        UpdateMapCameraYawInput();
        UpdateMapCameraDistanceSlider();
        UpdateMapCameraDistanceInput();
        _initializing = false;
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

    private void ConfigureVesselsProperties()
    {
        VesselIdSlider.onValueChanged.AddListener(OnVesselIdSliderChanged);
        VesselIdInput.onEndEdit.AddListener(OnVesselIdInputChanged);

        for (var i = 0; i < VesselTypeToggles.Length; i++)
        {
            var type = _vesselMovementTypes[i];
            VesselTypeToggles[i].onValueChanged.AddListener(
                on => OnVesselTypeToggled(type, on));
        }

        VesselAutoSpeedToggle.onValueChanged.AddListener(OnVesselAutoSpeedToggleChanged);
        VesselSpeedSlider.onValueChanged.AddListener(OnVesselSpeedSliderChanged);
        VesselSpeedInput.onEndEdit.AddListener(OnVesselSpeedInputChanged);
        VesselHeadingSlider.onValueChanged.AddListener(OnVesselHeadingSliderChanged);
        VesselHeadingInput.onEndEdit.AddListener(OnVesselHeadingInputChanged);
        VesselDiveAngleSlider.onValueChanged.AddListener(OnVesselDiveAngleSliderChanged);
        VesselDiveAngleInput.onEndEdit.AddListener(OnVesselDiveAngleInputChanged);
        VesselPeriodSlider.onValueChanged.AddListener(OnVesselPeriodSliderChanged);
        VesselPeriodInput.onEndEdit.AddListener(OnVesselPeriodInputChanged);
        VesselTargetSlider.onValueChanged.AddListener(OnVesselTargetSliderChanged);
        VesselTargetInput.onEndEdit.AddListener(OnVesselTargetInputChanged);
    }

    private void InitVesselsProperties()
    {
        _initializing = true;

        var v = VesselEvent;
        VesselIdSlider.value = v.Vessel;
        VesselIdInput.text = string.Format("{0:N0}", v.Vessel);

        UpdateVesselTypeToggles();

        VesselAutoSpeedToggle.isOn = v.AutoSpeed;
        VesselSpeedSlider.value = v.Speed;
        VesselSpeedSlider.maxValue = Mathf.Max(VesselSpeedSlider.maxValue, v.Speed);
        VesselSpeedInput.text = string.Format("{0:N1}", v.Speed);
        VesselHeadingSlider.value = v.Heading;
        VesselHeadingInput.text = string.Format("{0:N1}", v.Heading);
        VesselDiveAngleSlider.value = v.DiveAngle;
        VesselDiveAngleInput.text = string.Format("{0:N1}", v.DiveAngle);
        VesselPeriodSlider.value = v.Period;
        VesselPeriodInput.text = string.Format("{0:N1}", v.Period);
        VesselTargetSlider.value = v.TargetVessel;
        VesselTargetInput.text = string.Format("{0:N0}", v.TargetVessel);

        _initializing = false;
    }

    private void UpdateVesselsProperties()
    {
        var v = VesselEvent;
        VesselNameLabel.text = "VESSEL: " + VesselData.GetDebugName(v.Vessel);
        VesselStateProperties.gameObject.SetActive(!v.IsNone);
        VesselAutoSpeedToggle.gameObject.SetActive(v.IsIntercept);
        VesselSpeedGroup.interactable = !VesselAutoSpeedToggle.isOn;
        VesselSpeedGroup.alpha = VesselAutoSpeedToggle.isOn ? 0.5f : 1;
        VesselHeadingSlider.transform.parent.gameObject.SetActive(v.IsSetVector);
        VesselDiveAngleSlider.transform.parent.gameObject.SetActive(v.IsSetVector);
        VesselPeriodSlider.transform.parent.gameObject.SetActive(v.IsHolding);
        VesselTargetLabel.gameObject.SetActive(v.IsPursue);
        VesselTargets.transform.gameObject.SetActive(v.IsPursue);
        VesselTargetLabel.gameObject.SetActive(v.IsPursue);
        VesselTargetLabel.text = v.IsPursue ? "PURSUE " + VesselData.GetDebugName(v.TargetVessel) : "";
    }

    private void OnVesselIdSliderChanged(float value)
    {
        if (_initializing)
            return;

        VesselEvent.Vessel = Mathf.RoundToInt(value);
        VesselIdInput.text = string.Format("{0:N0}", value);
    }

    private void OnVesselIdInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        VesselEvent.Vessel = Mathf.RoundToInt(result);
        VesselIdSlider.value = result;
    }

    private void UpdateVesselTypeToggles()
    {
        var type = VesselEvent.Type;
        for (var i = 0; i < VesselTypeToggles.Length; i++)
        {
            var toggle = VesselTypeToggles[i];
            toggle.isOn = _vesselMovementTypes[i] == type;
        }
    }

    private void OnVesselTypeToggled(string type, bool value)
    {
        if (_initializing)
            return;

        VesselEvent.Type = value ? type : "None";

        _initializing = true;
        UpdateVesselTypeToggles();
        _initializing = false;
    }

    private void OnVesselAutoSpeedToggleChanged(bool on)
    {
        if (_initializing)
            return;

        VesselEvent.AutoSpeed = on;
    }

    private void OnVesselSpeedSliderChanged(float value)
    {
        if (_initializing)
            return;

        VesselEvent.Speed = value;
        VesselSpeedInput.text = string.Format("{0:N1}", value);
    }

    private void OnVesselSpeedInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        VesselEvent.Speed = result;
        VesselSpeedSlider.maxValue = Mathf.Max(VesselSpeedSlider.maxValue, result);
        VesselSpeedSlider.value = result;
    }

    private void OnVesselHeadingSliderChanged(float value)
    {
        if (_initializing)
            return;

        VesselEvent.Heading = value;
        VesselHeadingInput.text = string.Format("{0:N1}", value);
    }

    private void OnVesselHeadingInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        VesselEvent.Heading = result;
        VesselHeadingSlider.value = result;
    }

    private void OnVesselDiveAngleSliderChanged(float value)
    {
        if (_initializing)
            return;

        VesselEvent.DiveAngle = value;
        VesselDiveAngleInput.text = string.Format("{0:N1}", value);
    }

    private void OnVesselDiveAngleInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        VesselEvent.DiveAngle = result;
        VesselDiveAngleSlider.value = result;
    }

    private void OnVesselPeriodSliderChanged(float value)
    {
        if (_initializing)
            return;

        VesselEvent.Period = value;
        VesselPeriodInput.text = string.Format("{0:N1}", value);
    }

    private void OnVesselPeriodInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        VesselEvent.Period = result;
        VesselPeriodSlider.value = result;
    }

    private void OnVesselTargetSliderChanged(float value)
    {
        if (_initializing)
            return;

        VesselEvent.TargetVessel = Mathf.RoundToInt(value);
        VesselTargetInput.text = string.Format("{0:N0}", value);
    }

    private void OnVesselTargetInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        VesselEvent.TargetVessel = Mathf.RoundToInt(result);
        VesselTargetSlider.value = result;
    }

    public void VesselsCapture()
    {
        if (_initializing)
            return;

        VesselEvent.Capture();
        InitVesselsProperties();
    }


    // Popup Event Interface
    // ------------------------------------------------------------

    private void ConfigurePopupProperties()
    {
        PopupTitleInput.onEndEdit.AddListener(PopupTitleInputChanged);
        PopupTypeSelect.onClick.AddListener(PopupTypeSelectClicked);
        PopupMessageInput.onEndEdit.AddListener(PopupMessageInputChanged);
        PopupTargetInput.onEndEdit.AddListener(PopupTargetInputChanged);
        PopupXInput.onEndEdit.AddListener(PopupXInputChanged);
        PopupYInput.onEndEdit.AddListener(PopupYInputChanged);
        PopupZInput.onEndEdit.AddListener(PopupZInputChanged);
        PopupWidthInput.onEndEdit.AddListener(PopupWidthInputChanged);
        PopupHeightInput.onEndEdit.AddListener(PopupHeightInputChanged);

        for (var i = 0; i < PopupIconToggles.Length; i++)
        {
            var icon = (popupData.Icon) i;
            PopupIconToggles[i].onValueChanged.AddListener(
                on => OnPopupIconToggled(icon, on));
        }

        for (var i = 0; i < PopupTypeToggles.Length; i++)
        {
            var type = (popupData.Type) i;
            PopupTypeToggles[i].onValueChanged.AddListener(
                on => OnPopupTypeToggled(type, on));
        }

        for (var i = 0; i < PopupColorToggles.Length; i++)
        {
            var color = PopupColorToggles[i].graphic.color;
            PopupColorToggles[i].onValueChanged.AddListener(
                on => OnPopupColorToggled(color, on));
        }
    }

    private void InitPopupProperties()
    {
        _initializing = true;

        UpdatePopupTypeToggles();
        UpdatePopupTitleInput();
        UpdatePopupMessageInput();
        UpdatePopupTargetInput();
        UpdatePopupPositionInputs();
        UpdatePopupSizeInputs();
        UpdatePopupIconToggles();
        UpdatePopupColorToggles();

        _initializing = false;
    }

    private void UpdatePopupProperties()
    {
        PopupTypeLabel.text = serverUtils.PopupData.NameForType(PopupEvent.Type);
    }

    private void PopupTypeSelectClicked()
    {
        var items = serverUtils.PopupData.Types.Select(
            t => new DialogList.Item {Name = t.Name, Id = t.Type.ToString()} );

        DialogManager.Instance.ShowList("SELECT POPUP TYPE",
            "Please select the type of popup you want to display:",
            items, 
            PopupEvent.Type.ToString(),
            (item) => { PopupEvent.Type = serverUtils.PopupData.TypeForId(item.Id); });
    }

    private void UpdatePopupTitleInput()
    {
        PopupTitleInput.text = PopupEvent.Popup.CanSetTitle ? PopupEvent.Title : "";
        PopupTitleInput.transform.parent.gameObject.SetActive(PopupEvent.Popup.CanSetTitle);
    }

    public void PopupTitleInputChanged(string value)
    {
        if (_initializing)
            return;

        PopupEvent.Title = value;
    }

    private void UpdatePopupMessageInput()
    {
        PopupMessageInput.text = PopupEvent.Popup.CanSetMessage ? PopupEvent.Message : "";
        PopupMessageInput.transform.parent.gameObject.SetActive(PopupEvent.Popup.CanSetMessage);
    }

    public void PopupMessageInputChanged(string value)
    {
        if (_initializing)
            return;

        PopupEvent.Message = value;
    }

    private void UpdatePopupTargetInput()
    {
        PopupTargetInput.text = PopupEvent.Target;
    }

    public void PopupTargetInputChanged(string value)
    {
        if (_initializing)
            return;

        PopupEvent.Target = value;
    }

    private void UpdatePopupPositionInputs()
    {
        PopupXInput.text = string.Format("{0:N0}", PopupEvent.Position.x);
        PopupYInput.text = string.Format("{0:N0}", PopupEvent.Position.y);
        PopupZInput.text = string.Format("{0:N0}", PopupEvent.Position.z);

        PopupZInput.transform.parent.gameObject.SetActive(
            PopupEvent.Popup.CanSetPosition);
    }

    private void UpdatePopupSizeInputs()
    {
        PopupWidthInput.text = string.Format("{0:N0}", PopupEvent.Size.x);
        PopupHeightInput.text = string.Format("{0:N0}", PopupEvent.Size.y);
    }

    public void PopupXInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        PopupEvent.Position.x = result;
    }

    public void PopupYInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        PopupEvent.Position.y = result;
    }

    public void PopupZInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        PopupEvent.Position.z = result;
    }

    public void PopupWidthInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        PopupEvent.Size.x = result;
    }

    public void PopupHeightInputChanged(string value)
    {
        if (_initializing)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        PopupEvent.Size.y = result;
    }

    private void UpdatePopupIconToggles()
    {
        var popup = PopupEvent.Popup;
        if (!popup.IsIconValid(popup.Icon))
            PopupEvent.Icon = popupData.Icon.None;

        for (var i = 0; i < PopupIconToggles.Length; i++)
        {
            var icon = (popupData.Icon) i;
            var toggle = PopupIconToggles[i];
            toggle.isOn = (icon == PopupEvent.Icon);
            toggle.gameObject.SetActive(popup.IsIconValid(icon));
        }

        PopupIconToggles[0].transform.parent.gameObject.SetActive(
            PopupIconToggles.Any(p => p.gameObject.activeSelf));
    }

    private void OnPopupIconToggled(popupData.Icon icon, bool value)
    {
        if (_initializing)
            return;

        PopupEvent.Icon = icon;

        _initializing = true;
        UpdatePopupIconToggles();
        _initializing = false;
    }

    private void UpdatePopupTypeToggles()
    {
        var index = (int) PopupEvent.Type;
        for (var i = 0; i < PopupTypeToggles.Length; i++)
        {
            var toggle = PopupTypeToggles[i];
            toggle.isOn = (i == index);
        }
    }

    private void OnPopupTypeToggled(popupData.Type type, bool value)
    {
        if (_initializing)
            return;

        PopupEvent.Type = type;
        InitPopupProperties();
    }

    private void UpdatePopupColorToggles()
    {
        for (var i = 0; i < PopupColorToggles.Length; i++)
        {
            var toggle = PopupColorToggles[i];
            var color = toggle.graphic.color;
            toggle.isOn = (color == PopupEvent.Color);
        }

        PopupColorToggles[0].transform.parent.gameObject.SetActive(
            PopupEvent.Popup.CanSetColor);
    }

    private void OnPopupColorToggled(Color color, bool value)
    {
        if (_initializing)
            return;

        PopupEvent.Color = color;

        _initializing = true;
        UpdatePopupColorToggles();
        _initializing = false;
    }

    /** Return the number of decimal places to use for a given value. */
    private static int Precision(string param, double value)
    {
        var info = serverUtils.GetServerDataInfo(param);
        if (info.precision > 0)
            return info.precision;

        var isInt = Math.Abs(value - Math.Round(value)) < 0.00001;
        if (isInt)
            return 0;

        var abs = Math.Abs(value);
        if (abs >= 100)
            return 0;
        if (abs >= 10)
            return 1;
        if (abs >= 1)
            return 2;
        if (abs >= 0.1)
            return 3;
        if (abs >= 0.01)
            return 4;

        return 5;
    }

}
