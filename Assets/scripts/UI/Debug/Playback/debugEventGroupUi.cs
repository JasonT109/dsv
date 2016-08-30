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

    /** The group's backdrop graphic. */
    public Graphic Backdrop;

    /** The event group's toggle button. */
    public Toggle PausedToggle;

    /** The event group's loop toggle button. */
    public Toggle LoopingToggle;

    /** The timeline for this event group. */
    public debugEventTimelineUi Timeline;

    /** Container for trigger buttons. */
    public Transform TriggerContainer;

    /** Selection graphic. */
    public Graphic On;


    [Header("Configuration")]

    /** Color to use for paused toggle when file is playing. */
    public Color ActiveColor;

    /** Color to use for paused toggle when file is not playing. */
    public Color InactiveColor;

    /** Color to use for backdrop when group is active. */
    public Color BackdropActiveColor;

    /** Color to use for backdrop when group is inactive. */
    public Color BackdropInactiveColor;

    /** Color to use for backdrop when group is selected. */
    public Color BackdropSelectedColor;


    [Header("Prefabs")]

    /** Prefab to use for trigger buttons. */
    public debugEventTriggerUi TriggerPrefab;


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

    /** Event signature for a group selection event. */
    public delegate void GroupSelectedHandler(debugEventGroupUi groupUi, debugEventUi eventUi);

    /** Selection event. */
    public event GroupSelectedHandler OnSelected;


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

    /** Select this group. */
    public void Select()
    {
        if (OnSelected != null)
            OnSelected(this, null);
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

        var c = BackdropInactiveColor;
        if (_group.running)
            c = BackdropActiveColor;
        if (_group.paused)
            c.a *= 0.5f;
        if (_group.file.selectedGroup == _group)
            c = BackdropSelectedColor;
        Backdrop.color = c;

        On.gameObject.SetActive(_group.file.selectedGroup == _group);

        PausedToggle.isOn = !_group.paused;
        PausedToggle.graphic.color = File.playing ? ActiveColor : InactiveColor;
        LoopingToggle.isOn = _group.looping;
        LoopingToggle.graphic.color = (File.playing && !Group.paused) ? ActiveColor : InactiveColor;
        LoopingToggle.gameObject.SetActive(_group.canLoop);

        _nameLabel.text = Group.id.ToUpper();

        // Set group on timeline.
        Timeline.Group = Group;
        Timeline.gameObject.SetActive(!Group.hideTimeline);

        // Update trigger buttons.
        UpdateTriggers();
        TriggerContainer.gameObject.SetActive(!Group.hideTriggers);

        _updating = false;
    }

    private void HandleEventSelected(debugEventUi ui)
    {
        if (OnSelected != null)
            OnSelected(this, ui);
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
