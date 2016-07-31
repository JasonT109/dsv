using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Meg.EventSystem;

public class debugEventGroupUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** The event group's toggle button. */
    public Toggle PausedToggle;

    /** The event group's loop toggle button. */
    public Toggle LoopingToggle;

    /** The timeline for this event group. */
    public debugEventTimelineUi Timeline;

    /** Container for trigger buttons. */
    public Transform TriggerContainer;


    [Header("Configuration")]

    /** Color to use for paused toggle when file is playing. */
    public Color ActiveColor;

    /** Color to use for paused toggle when file is not playing. */
    public Color InactiveColor;


    [Header("Prefabs")]

    /** Prefab to use for trigger buttons. */
    public debugEventTriggerUi TriggerPrefab;


    /** The event group. */
    public megEventGroup Group
    {
        get { return _group; }
        set { SetGroup(value); }
    }

    /** The event file. */
    public megEventFile File
        { get { return _group.file; } }

    /** Whether group is paused. */
    public bool Paused
    {
        get { return _group.paused; }
        set { SetPaused(value); }
    }

    /** Whether group is looping. */
    public bool Looping
    {
        get { return _group.looping; }
        set { SetLooping(value); }
    }

    /** Selection event. */
    public event debugEventUi.EventSelectedHandler OnEventSelected;


    // Members
    // ------------------------------------------------------------

    /** The event group. */
    private megEventGroup _group;

    /** The event group name. */
    private Text _nameLabel;

    /** Whether ui is being updated. */
    private bool _updating;

    /** Trigger buttons for events. */
    private readonly List<debugEventTriggerUi> _triggers = new List<debugEventTriggerUi>();



    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        _nameLabel = PausedToggle.GetComponentInChildren<Text>();
        Timeline.OnEventSelected += HandleEventSelected;

        UpdateGroupUi();
    }

    /** Updating. */
    private void Update()
    {
        UpdateGroupUi();
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Toggle pause on/off. */
    public void TogglePaused()
    {
        Paused = !Paused;
    }

    /** Set paused state. */
    public void SetPaused(bool value)
    {
        if (_updating)
            return;

        _group.SetPaused(value);
        PausedToggle.isOn = !value;
    }

    /** Toggle looping on/off. */
    public void ToggleLooping()
    {
        Looping = !Looping;
    }

    /** Set paused state. */
    public void SetLooping(bool value)
    {
        if (_updating)
            return;

        _group.SetLooping(value);
        LoopingToggle.isOn = value;
    }


    // Private Methods
    // ------------------------------------------------------------

    private void SetGroup(megEventGroup value)
    {
        _group = value;
    }

    private void UpdateGroupUi()
    {
        if (_group == null)
            return;

        _updating = true;

        PausedToggle.isOn = !_group.paused;
        PausedToggle.graphic.color = File.playing ? ActiveColor : InactiveColor;
        LoopingToggle.isOn = _group.looping;
        LoopingToggle.graphic.color = (File.playing && !Group.paused) ? ActiveColor : InactiveColor;
        LoopingToggle.gameObject.SetActive(_group.canLoop);

        var label = Regex.Replace(Group.id, "[A-Z]", " $0");
        label = Regex.Replace(label, "([A-Z])([0-9])", "$0 $1").ToUpper();
        _nameLabel.text = label;

        // Set group on timeline.
        Timeline.Group = Group;
        Timeline.gameObject.SetActive(!Group.hideTimeline);

        // Update trigger buttons.
        UpdateTriggers();

        _updating = false;
    }

    private void HandleEventSelected(debugEventUi ui)
    {
        if (OnEventSelected != null)
            OnEventSelected(ui);
    }

    private void UpdateTriggers()
    {
        var triggered = _group.events.Where(e => e.hasTrigger);
        var index = 0;
        foreach (var e in triggered)
            GetTrigger(index++).Event = e;

        for (var i = 0; i < _triggers.Count; i++)
            _triggers[i].gameObject.SetActive(i < index);
    }

    private debugEventTriggerUi GetTrigger(int i)
    {
        if (i >= _triggers.Count)
        {
            var trigger = Instantiate(TriggerPrefab);
            trigger.transform.SetParent(TriggerContainer, false);
            _triggers.Add(trigger);
        }

        return _triggers[i];
    }

}
