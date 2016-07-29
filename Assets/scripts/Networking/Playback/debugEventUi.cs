using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Meg.EventSystem;

public class debugEventUi : MonoBehaviour
{

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

    /** Event signature for a selection event. */
    public delegate void EventSelectedHandler(debugEventUi ui);

    /** Selection event. */
    public event EventSelectedHandler OnSelected;


    // Members
    // ------------------------------------------------------------

    /** The event. */
    private megEvent _event;


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

        var s = transform.localScale;
        transform.DOKill();
        transform.DOPunchScale(new Vector3(0, s.y * 0.25f, 0), 0.25f);
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
    }


}
