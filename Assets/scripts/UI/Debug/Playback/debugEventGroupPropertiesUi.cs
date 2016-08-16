using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Meg.EventSystem;

public class debugEventGroupPropertiesUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")] 
    
    /** The event group's name input. */
    public InputField NameInput;

    /** Header area. */
    public CanvasGroup Header;

    /** The event group's loopable button. */
    public Toggle CanLoopToggle;

    /** The event group's timeline toggle button. */
    public Toggle ShowTimelineToggle;

    /** The event group's triggers toggle button. */
    public Toggle ShowTriggersToggle;

    /** Container for event properties. */
    public Transform EventPropertiesContainer;

    /** Footer area. */
    public CanvasGroup Footer;

    /** Event execute button. */
    public Button ExecuteEventButton;

    /** Event removal button. */
    public Button RemoveEventButton;

    /** Parameter info toggle. */
    public Toggle InfoToggle;

    /** Parameter info view. */
    public GameObject InfoView;

    /** Parameter list UI. */
    public debugParameterListUi InfoList;



    [Header("Prefabs")]

    /** Prefab to use for event properties. */
    public debugEventPropertiesUi EventPropertiesPrefab;


    /** The event group. */
    public megEventGroup Group
    {
        get { return _group; }
        set { SetGroup(value); }
    }

    /** Whether an group is set. */
    public bool HasGroup
        { get { return _group != null; } }

    /** The event file. */
    public megEventFile File
        { get { return _group != null ? _group.file : null; } }

    /** Whether group can be made to loop. */
    public bool CanLoop
    {
        get { return _group.canLoop; }
        set { SetCanLoop(value); }
    }

    /** Whether group's timeline is shown. */
    public bool ShowTimeline
    {
        get { return !_group.hideTimeline; }
        set { SetShowTimeline(value); }
    }

    /** Whether group's triggers are shown. */
    public bool ShowTriggers
    {
        get { return !_group.hideTriggers; }
        set { SetShowTriggers(value); }
    }


    // Events
    // ------------------------------------------------------------

    /** Event signature for a event selection. */
    public delegate void EventSelectedHandler(debugEventGroupPropertiesUi groupUi, debugEventPropertiesUi eventUi);

    /** Selection event. */
    public event EventSelectedHandler OnSelected;


    // Members
    // ------------------------------------------------------------

    /** The event group. */
    private megEventGroup _group;

    /** Whether ui is being updated. */
    private bool _updating;

    /** Trigger buttons for events. */
    private readonly List<debugEventPropertiesUi> _eventProperties = new List<debugEventPropertiesUi>();



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
        if (_group != null)
            UpdateUi();
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Sets the group's name. */
    public void NameInputChanged(string value)
    {
        if (_updating || _group == null)
            return;

        _group.id = value;
    }

    /** Toggle whether group can be looped. */
    public void ToggleCanLoop()
    {
        if (_updating)
            return;

        CanLoop = !CanLoop;
    }

    /** Set whether group can be looped. */
    public void SetCanLoop(bool value)
    {
        if (_updating)
            return;

        _group.canLoop = value;
        CanLoopToggle.isOn = value;
    }

    /** Toggle timeline visibility. */
    public void ToggleShowTimeline()
    {
        if (_updating)
            return;

        ShowTimeline = !ShowTimeline;
    }

    /** Set timeline visibility. */
    public void SetShowTimeline(bool value)
    {
        if (_updating)
            return;

        _group.hideTimeline = !value;
        ShowTimelineToggle.isOn = value;
    }

    /** Toggle triggers visibility. */
    public void ToggleShowTriggers()
    {
        if (_updating)
            return;

        ShowTriggers = !ShowTriggers;
    }

    /** Set paused state. */
    public void SetShowTriggers(bool value)
    {
        if (_updating)
            return;

        _group.hideTriggers = !value;
        ShowTriggersToggle.isOn = value;
    }

    /** Toggle an event's minimized state. */
    public void ToggleEvent(megEvent toToggle)
    {
        if (File == null)
            return;

        if (toToggle == File.selectedEvent)
            File.selectedEvent = null;
        else
            File.selectedEvent = toToggle;
    }

    /** Expand an event. */
    public void ExpandEvent(megEvent toExpand)
    {
        if (File != null)
            File.selectedEvent = toExpand;
    }

    /** Minimize an event. */
    public void MinimizeEvent(megEvent toExpand)
    {
        if (File != null)
            File.selectedEvent = null;
    }

    /** Select an event. */
    public void SelectEvent(megEvent toSelect)
    {
        var eventProperties = _eventProperties.FirstOrDefault(e => e.Event == toSelect);
        if (OnSelected != null)
            OnSelected(this, eventProperties);
    }

    /** Add a value event to the group. */
    public void AddValueEvent()
    {
        var valueEvent = AddEvent(megEventType.Value) as megEventValue;
    
        // Apply selected server parameter (if there is one).
        if (valueEvent != null && InfoList.Selected)
            valueEvent.serverParam = InfoList.Selected.Text.text;
    }

    /** Add a physics event to the group. */
    public void AddPhysicsEvent()
        { AddEvent(megEventType.Physics); }

    /** Add a sonar event to the group. */
    public void AddSonarEvent()
        { AddEvent(megEventType.Sonar); }

    /** Add a map camera event to the group. */
    public void AddMapCameraEvent()
    {
        var cameraEvent = AddEvent(megEventType.MapCamera) as megEventMapCamera;
        if (cameraEvent != null)
            cameraEvent.Capture();
    }

    /** Add a vessel simulation event to the group. */
    public void AddVesselsEvent()
    {
        var vesselEvent = AddEvent(megEventType.VesselMovement) as megEventVesselMovement;
        if (vesselEvent != null)
        {
            vesselEvent.Vessel = debugVessels.Instance.activeVessel;
            vesselEvent.Capture();
        }
    }

    /** Add an event of a given type to the group. */
    public megEvent AddEvent(megEventType type)
    {
        var e = _group.InsertEvent(type, File.selectedEvent);
        File.selectedEvent = e;
        ExpandEvent(e);
        return e;
    }

    /** Execute the selected event. */
    public void ExecuteSelectedEvent()
    {
        if (File.selectedEvent != null)
            File.selectedEvent.Execute();
    }

    /** Remove an event from the group. */
    public void RemoveEvent()
    {
        var toRemove = File.selectedEvent;
        if (toRemove == null)
            return;

        _group.RemoveEvent(toRemove);
    }

    /** Toggle parameter info list. */
    public void ToggleInfo()
    {
        InfoView.SetActive(!InfoView.activeSelf);
    }



    // Private Methods
    // ------------------------------------------------------------

    private void SetGroup(megEventGroup value)
    {
        if (_group == value)
            return;

        _group = value;

        if (_group != null)
        {
            NameInput.text = _group.id;
            UpdateUi();
        }
        else
            ClearUi();
    }

    private void ConfigureUi()
    {
        NameInput.onValidateInput += ValidateGroupNameInput;
    }

    private void UpdateUi()
    {
        if (_group == null)
            return;

        _updating = true;

        NameInput.interactable = !File.playing;

        Header.gameObject.SetActive(true);
        CanLoopToggle.isOn = _group.canLoop;
        ShowTimelineToggle.isOn = !_group.hideTimeline;
        ShowTriggersToggle.isOn = !_group.hideTriggers;

        Footer.interactable = !File.playing;
        ExecuteEventButton.interactable = File.selectedEvent != null && !File.playing;
        RemoveEventButton.interactable = File.selectedEvent != null && !File.playing;

        // Update event properties.
        UpdateEventProperties();

        _updating = false;
    }

    private void ClearUi()
    {
        NameInput.text = "";
        Header.gameObject.SetActive(false);
        Footer.interactable = false;

        UpdateEventProperties();
    }
    
    private void UpdateEventProperties()
    {
        var index = 0;
        if (_group != null)
            foreach (var e in _group.events)
                GetEventProperties(index++).Event = e;

        for (var i = 0; i < _eventProperties.Count; i++)
            _eventProperties[i].gameObject.SetActive(i < index);
    }
    
    private debugEventPropertiesUi GetEventProperties(int i)
    {
        if (i >= _eventProperties.Count)
        {
            var eventProperties = Instantiate(EventPropertiesPrefab);
            eventProperties.transform.SetParent(EventPropertiesContainer, false);
            eventProperties.GroupUi = this;
            eventProperties.OnSelected += HandleEventSelected;
            _eventProperties.Add(eventProperties);
        }

        return _eventProperties[i];
    }

    private void HandleEventSelected(debugEventPropertiesUi ui)
    {
        if (OnSelected != null)
            OnSelected(this, ui);
    }

    private char ValidateGroupNameInput(string text, int index, char addedChar)
    {
        if (!Regex.IsMatch(addedChar.ToString(), "[A-Za-z0-9_ ]"))
            return '\0';

        return addedChar;
    }


}
