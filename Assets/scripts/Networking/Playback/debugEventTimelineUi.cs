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


    [Header("Prefabs")]

    /** Component to use for a second tick mark on the timeline. */
    public Graphic SecondTickPrefab;

    /** Component to use for an event on the timeline. */
    public debugEventUi EventUiPrefab;


    [Header("Configuration")]

    /** Scale factor for timeline width (pixels per second). */
    public float Scale = 100;

    /** Minimum timeline width. */
    public float MinWidth = 0;

    /** Maximum timeline width. */
    public float MaxWidth = 780;

    /** Vertical spacing between events. */
    public float EventSpacing = 10;

    /** The event group. */
    public megEventGroup Group
    {
        get { return _group; }
        set { SetGroup(value); }
    }

    /** Selection event. */
    public event debugEventUi.EventSelectedHandler OnEventSelected;


    // Members
    // ------------------------------------------------------------

    /** The event group. */
    private megEventGroup _group;

    /** Second tick markers. */
    private readonly List<Graphic> _ticks = new List<Graphic>();

    /** Events. */
    private readonly List<debugEventUi> _events = new List<debugEventUi>();


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
        UpdateNeedle();
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
        // Update the current timeline scale.
        var fileEndTime = _group.file.endTime;
        var fileWidth = Mathf.Clamp(fileEndTime * Scale, MinWidth, MaxWidth);
        var fileScale = fileWidth / fileEndTime;
        var scale = Mathf.Min(Scale, fileScale);

        var width = scale * _group.endTime;
        Layout.preferredWidth = width;
        Layout.minHeight = _group.events.Count * EventSpacing + 30;

        UpdateTicks(scale);
        UpdateEvents(scale);
    }

    private void UpdateNeedle()
    {
        var t = _group.time;
        var e = _group.endTime;
        var x = e > 0 ? (t / e) * Layout.preferredWidth : 0;
        var y = Needle.transform.localPosition.y;
        Needle.transform.localPosition = new Vector2(x, y);
    }

    private void UpdateTicks(float scale)
    {
        var nActive = Mathf.FloorToInt(_group.endTime);
        var n = Mathf.Max(_ticks.Count, nActive);
        for (var i = 0; i < n; i++)
        {
            var tick = GetSecondTick(i);
            var y = tick.transform.localPosition.y;
            tick.transform.localPosition = new Vector2(i * scale, y);
            tick.gameObject.SetActive(i < nActive);
        }
    }

    private Graphic GetSecondTick(int i)
    {
        if (i >= _ticks.Count)
        {
            var tick = Instantiate(SecondTickPrefab);
            tick.transform.SetParent(TickContainer, false);
            tick.GetComponentInChildren<Text>().text = string.Format("{0}", i);
            _ticks.Add(tick);
        }

        return _ticks[i];
    }

    private void UpdateEvents(float scale)
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

            var y = -i * EventSpacing;
            var t = ui.GetComponent<RectTransform>();
            t.localPosition = new Vector2(e.triggerTime * scale, y);
            t.sizeDelta = new Vector2(e.completeTime * scale, t.sizeDelta.y);
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
