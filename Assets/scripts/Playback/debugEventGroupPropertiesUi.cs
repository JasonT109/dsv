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
    
    /** The event group's name label. */
    public Text NameLabel;

    /** The event group's loopable button. */
    public Toggle CanLoopToggle;

    /** The event group's timeline toggle button. */
    public Toggle ShowTimelineToggle;

    /** The event group's triggers toggle button. */
    public Toggle ShowTriggersToggle;

    /** Container for event properties. */
    public Transform EventPropertiesContainer;


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
        { get { return _group.file; } }

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
        UpdateGroupUi();
    }

    /** Updating. */
    private void Update()
    {
        UpdateGroupUi();
    }


    // Public Methods
    // ------------------------------------------------------------

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
    public void ToggleEvent(megEvent toToggle, bool minimizeOthers = true)
    {
        foreach (var e in _eventProperties)
            if (e.Event == toToggle)
                e.Minimized = !e.Minimized;
            else if (minimizeOthers)
                e.Minimized = true;
    }

    /** Expand an event. */
    public void ExpandEvent(megEvent toExpand, bool minimizeOthers = true)
    {
        foreach (var e in _eventProperties)
            if (e.Event == toExpand)
                e.Minimized = false;
            else if (minimizeOthers)
                e.Minimized = true;
    }

    /** Minimize an event. */
    public void MinimizeEvent(megEvent toExpand)
    {
        var eventProperties = _eventProperties.FirstOrDefault(e => e.Event == toExpand);
        if (eventProperties)
            eventProperties.Minimized = true;
    }


    // Private Methods
    // ------------------------------------------------------------

    private void SetGroup(megEventGroup value)
    {
        if (_group == value)
            return;

        _group = value;
        UpdateGroupUi();
    }

    private void UpdateGroupUi()
    {
        if (_group == null)
            return;

        _updating = true;

        CanLoopToggle.isOn = _group.canLoop;
        ShowTimelineToggle.isOn = !_group.hideTimeline;
        ShowTriggersToggle.isOn = !_group.hideTriggers;

        var label = Regex.Replace(Group.id, "[A-Z]", " $0");
        label = Regex.Replace(label, "([A-Z])([0-9])", "$0 $1").ToUpper();
        NameLabel.text = label;

        // Update event properties.
        UpdateEventProperties();

        _updating = false;
    }

    private void UpdateEventProperties()
    {
        var index = 0;
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


}
