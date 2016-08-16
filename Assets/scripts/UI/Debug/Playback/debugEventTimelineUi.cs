using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Meg.EventSystem;


public class debugEventTimelineUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** The timeline layout. */
    public LayoutElement Layout;

    /** Playback head needle. */
    public Graphic Needle;

    /** Tick marker container. */
    public Transform TickContainer;

    /** Event container. */
    public Transform EventContainer;

    /** Minimize toggle. */
    public Toggle MinimizeToggle;


    [Header("Prefabs")]

    /** Component to use for a second tick mark on the timeline. */
    public debugTickUi TickPrefab;

    /** Component to use for an event on the timeline. */
    public debugEventUi EventUiPrefab;


    [Header("Sizing")]

    /** Scale factor for timeline width (pixels per second). */
    public float Scale = 100;

    /** Minimum timeline width. */
    public float MinWidth = 100;

    /** Timeline height threshold for minimization. */
    public float MinimizeThreshold = 80;

    /** Timeline margin in minimized state. */
    public float MinimizedMargin = 15;

    /** Timeline margin in expanded state. */
    public float ExpandedMargin = 15;

    /** Vertical spacing between events in minimized state. */
    public float MinimizedEventSpacing = 5;

    /** Vertical spacing between events. */
    public float ExpandedEventSpacing = 10;

    /** Height of each event in minimized state. */
    public float MinimizedEventHeight = 6;

    /** Height of each event. */
    public float ExpandedEventHeight = 10;


    /** The event group. */
    public megEventGroup Group
    {
        get { return _group; }
        set { SetGroup(value); }
    }

    /** Whether timeline is minimized. */
    public bool Minimized
    {
        get { return _group.minimized; }
        set { SetMinimized(value); }
    }

    /** Selection event. */
    public event debugEventUi.EventSelectedHandler OnEventSelected;


    // Members
    // ------------------------------------------------------------

    /** The event group. */
    private megEventGroup _group;

    /** Second tick markers. */
    private readonly List<debugTickUi> _ticks = new List<debugTickUi>();

    /** Events. */
    private readonly List<debugEventUi> _events = new List<debugEventUi>();

    /** Whether UI is being updated. */
    private bool _updating;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        Update();
    }

    /** Updating. */
    private void Update()
    {
        if (_group == null)
            return;

        UpdateTimeline();
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Toggle timeline's minimized state. */
    public void ToggleMinimized()
    {
        if (_updating)
            return;

        Minimized = !Minimized;
    }

    /** Set timeline's minimized state. */
    public void SetMinimized(bool value)
    {
        _group.minimized = value;
    }


    // Private Methods
    // ------------------------------------------------------------

    private void SetGroup(megEventGroup value)
    {
        _group = value;
        Update();
    }

    private void UpdateTimeline()
    {
        _updating = true;

        var p = transform.parent as RectTransform;
        var maxWidth = p.sizeDelta.x;

        // Update the current timeline scale.
        var fileEndTime = _group.file.endTime;
        var fileWidth = Mathf.Clamp(fileEndTime * Scale, MinWidth, maxWidth);
        var fileScale = fileEndTime > 0 ? fileWidth / fileEndTime : fileWidth;
        var scale = Mathf.Min(Scale, fileScale);
        var width = Mathf.Clamp(scale * _group.endTime, MinWidth, maxWidth);
        var spacing = Minimized ? MinimizedEventSpacing : ExpandedEventSpacing;
        var margin = Minimized ? MinimizedMargin : ExpandedMargin;
        var height = Minimized ? MinimizedEventHeight : ExpandedEventHeight;


        Layout.preferredWidth = width;
        Layout.minHeight = _group.events.Count * height + margin;

        // Force a UI reflow to ensure timeline layout is correct.
        Canvas.ForceUpdateCanvases();

        UpdateTicks(scale);
        UpdateEvents(scale, spacing);
        UpdateNeedle(scale);

        var canMinimize = (_group.minimized || Layout.minHeight > MinimizeThreshold) && !_group.hideTimeline;
        MinimizeToggle.gameObject.SetActive(canMinimize);
        MinimizeToggle.isOn = !_group.minimized;

        _updating = false;
    }

    private void UpdateNeedle(float scale)
    {
        var x = _group.time * scale;
        var y = Needle.transform.localPosition.y;
        Needle.transform.localPosition = new Vector2(x, y);
    }

    private void UpdateTicks(float scale)
    {
        var groupEndTime = _group.endTime;
        var fileEndTime = _group.file.endTime;
        var interval = GetTickInterval(fileEndTime);
        var fileActive = Mathf.CeilToInt(fileEndTime / interval);
        var active = Mathf.CeilToInt(groupEndTime / interval);
        var n = Mathf.Max(_ticks.Count, active);

        var spacing = GetTickLabelSpacing(fileActive);
        var format = GetTickFormat(fileEndTime);
        
        for (var i = 0; i < n; i++)
        {
            var t = i * interval;
            var tick = GetTick(i, t);
            var y = tick.transform.localPosition.y;
            tick.transform.localPosition = new Vector2(t * scale, y);
            tick.Label.text = GetTickLabel(format, t);
            tick.Label.enabled = (i % spacing) == 0;
            tick.gameObject.SetActive(i < active);
        }
    }

    private float GetTickInterval(float length)
    {
        if (length <= 30)
            return 1.0f;
        if (length <= 120)
            return 5.0f;
        if (length <= 300)
            return 10.0f;
        if (length <= 900)
            return 30.0f;
        if (length <= 3600)
            return 60.0f;

        return 3600.0f;
    }

    private int GetTickLabelSpacing(int active)
    {
        if (active < 15)
            return 1;
        if (active < 150)
            return 5;

        return Mathf.CeilToInt(active / 10.0f);
    }

    private debugTickUi GetTick(int i, float t)
    {
        if (i >= _ticks.Count)
        {
            var tick = Instantiate(TickPrefab);
            tick.transform.SetParent(TickContainer, false);
            _ticks.Add(tick);
        }

        return _ticks[i];
    }

    private string GetTickFormat(float t)
    {
        if (t < 60)
            return "{0:0}";
        if (t < 3600)
            return "{1:0}:{0:00}";

        return "{2:0}:{1:00}:{0:00}";
    }

    private string GetTickLabel(string format, float t)
    {
        var span = TimeSpan.FromSeconds(t);
        return string.Format(format, span.Seconds, span.Minutes, span.Hours);
    }

    private void UpdateEvents(float scale, float spacing)
    {
        var nActive = _group.events.Count;
        var n = Mathf.Max(_events.Count, nActive);
        for (var i = 0; i < n; i++)
        {
            var ui = GetEventUi(i);
            ui.gameObject.SetActive(i < nActive);
            if (i >= nActive)
                continue;

            var e = _group.events[i];
            ui.Event = e;
            ui.UpdatePosition(scale, -i * spacing);
        }
    }

    private debugEventUi GetEventUi(int i)
    {
        if (i >= _events.Count)
        {
            var e = Instantiate(EventUiPrefab);
            e.transform.SetParent(EventContainer, false);
            e.OnSelected += HandleEventSelected;
            _events.Add(e);
        }

        return _events[i];
    }

    private void HandleEventSelected(debugEventUi ui)
    {
        if (OnEventSelected != null)
            OnEventSelected(ui);
    }

}
