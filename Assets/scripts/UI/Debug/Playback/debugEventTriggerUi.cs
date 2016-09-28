using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Meg.EventSystem;

public class debugEventTriggerUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    /** The trigger button. */
    public Button Button;

    /** The trigger label. */
    public Text Label;

    /** The event to trigger. */
    public megEvent Event
    {
        get { return _event; } 
        set { SetEvent(value); }
    }

    /** Color for event when it's not running. */
    public Color InactiveColor;

    /** Color for event when it's running. */
    public Color ActiveColor;

    /** Color for event when it's complete. */
    public Color CompletedColor;

    /** Color for event label when it's not running. */
    public Color LabelInactiveColor;

    /** Color for event label when it's running. */
    public Color LabelActiveColor;

    /** Color for event label when it's complete. */
    public Color LabelCompletedColor;

    /** Hotkey label. */
    public Text HotKey;


    // Members
    // ------------------------------------------------------------

    /** Whether trigger component has been started. */
    private bool _started;

    /** Trigger's current event. */
    private megEvent _event;



    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        _started = true;

        if (!Button)
            Button = GetComponent<Button>();

        Button.onClick.AddListener(OnButtonClicked);

        Update();
    }

    /** Updating. */
    private void Update()
    {
        if (Event == null || !_started)
            return;

        var recentlyTriggered = Time.time - Event.lastTriggerTime < 0.25f;

        Button.interactable = Event.file.playing && !Event.group.paused;
        Label.text = Event.triggerLabel;

        var c = InactiveColor;
        if (Event.running && !Event.completed)
            c = Color.Lerp(ActiveColor, CompletedColor, Event.timeFraction);
        if (Event.group.paused)
            c.a *= 0.5f;

        if (recentlyTriggered)
            c = ActiveColor;

        // Make popup triggers appear inactive id needed.
        var popupEvent = Event as megEventPopup;
        var hasPopup = popupEvent != null && popupEvent.HasPopup;
        if (popupEvent != null)
            c = hasPopup ? ActiveColor : InactiveColor;

        Button.targetGraphic.color = c;

        var l = LabelInactiveColor;
        if (Event.running && !Event.completed)
            l = Color.Lerp(LabelActiveColor, LabelCompletedColor, Event.timeFraction);
        if (Event.group.paused)
            l.a *= 0.5f;

        if (recentlyTriggered)
            l = LabelActiveColor;

        if (popupEvent != null)
            l = hasPopup ? LabelActiveColor : LabelInactiveColor;

        Label.color = l;

        HotKey.transform.parent.gameObject.SetActive(Event.hasTriggerHotKey);
        if (Event.hasTriggerHotKey)
            HotKey.text = Event.triggerHotKey;
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Set the trigger's current event. */
    private void SetEvent(megEvent e)
    {
        _event = e;

        if (_started)
            Update();
    }

    /** Button click handler. */
    private void OnButtonClicked()
    {
        if (Event != null)
            Event.Trigger();
    }

}
