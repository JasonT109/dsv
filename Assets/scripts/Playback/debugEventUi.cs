using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Meg.EventSystem;

public class debugEventUi : MonoBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** Threshold for placing event label to the left of its start point. */
    private const float LabelOnLeftThreshold = 0.7f;

    /** offset of label from start point. */
    private const float LabelOffset = 17;



    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Label for event name, etc. */
    public Text Label;

    /** Backdrop graphic. */
    public Image Backdrop;

    /** Start icon graphic. */
    public Image StartIcon;


    [Header("Colors")]

    /** Color for event when it's not running. */
    public Color InactiveColor;

    /** Color for event when it's running. */
    public Color ActiveColor;

    /** Color for event when it's complete. */
    public Color CompletedColor;

    /** Color for event when it's selected. */
    public Color SelectedColor;

    /** Color for event label when it's not running. */
    public Color LabelInactiveColor;

    /** Color for event label when it's running. */
    public Color LabelActiveColor;

    /** Color for event label when it's complete. */
    public Color LabelCompletedColor;

    /** Color for event label when it's selected. */
    public Color LabelSelectedColor;

    /** The event being represented. */
    public megEvent Event
    {
        get { return _event; }
        set { SetEvent(value); }
    }

    /** Whether event is minimized. */
    public bool Minimized
        { get { return _event.group.minimized; } }

    /** Event signature for a selection event. */
    public delegate void EventSelectedHandler(debugEventUi ui);

    /** Selection event. */
    public event EventSelectedHandler OnSelected;


    // Members
    // ------------------------------------------------------------

    /** The event. */
    private megEvent _event;

    /** Event's normal height. */
    private float _height;

    
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
        if (_event == null)
            return;

        UpdateUi();
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Select this event. */
    public void Select()
    {
        if (OnSelected != null)
            OnSelected(this);

        if (!DOTween.IsTweening(transform))
        {
            var s = transform.localScale;
            transform.DOPunchScale(new Vector3(0, s.y*0.25f, 0), 0.25f);
        }
    }

    /** Update event's position and size on the timeline. */
    public void UpdatePosition(float scale, float y)
    {
        var t = GetComponent<RectTransform>();
        if (_height <= 0)
            _height = t.sizeDelta.y;

        var h = Minimized ? _height * 0.1f : _height;
        t.localPosition = new Vector2(_event.triggerTime * scale, y);
        t.sizeDelta = new Vector2(_event.completeTime * scale, h);

        var s = Minimized ? 0.5f : 1;
        StartIcon.rectTransform.localScale = new Vector3(s, s, 1);
    }


    // Private Methods
    // ------------------------------------------------------------

    private void SetEvent(megEvent value)
    {
        _event = value;
        Update();
    }

    private void UpdateUi()
    {
        Label.text = _event.ToString();

        var c = InactiveColor;
        if (_event.running)
            c = Color.Lerp(ActiveColor, CompletedColor, _event.timeFraction);
        if (_event.group.paused)
            c.a *= 0.5f;
        if (_event.file.selectedEvent == _event)
            c = SelectedColor;

        Backdrop.color = c;
        StartIcon.color = c;

        var l = LabelInactiveColor;
        if (_event.running)
            l = Color.Lerp(LabelActiveColor, LabelCompletedColor, _event.timeFraction);
        if (_event.group.paused)
            l.a *= 0.5f;
        if (_event.file.selectedEvent == _event)
            l = LabelSelectedColor;

        Label.color = l;

        // Update label positioning to try and ensure it's visible.
        var fileEndTime = _event.file.endTime;
        var r = fileEndTime <= 5 || (_event.triggerTime / fileEndTime < LabelOnLeftThreshold);
        Label.rectTransform.pivot = new Vector2(r ? 0 : 1, 0.5f);
        Label.transform.localPosition = new Vector2(r ? LabelOffset : -LabelOffset, -2);
        Label.alignment = r ? TextAnchor.LowerLeft : TextAnchor.LowerRight;
        Label.gameObject.SetActive(!Minimized);
    }

}
