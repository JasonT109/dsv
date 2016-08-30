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
    public megEvent Event;

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


    // Members
    // ------------------------------------------------------------

    /** Time since last button press. */
    private float _lastClickTime;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        if (!Button)
            Button = GetComponent<Button>();

        Button.onClick.AddListener(OnButtonClicked);
    }

    /** Updating. */
    private void Update()
    {
        if (Event == null)
            return;

        var recentlyPressed = Time.time - _lastClickTime < 0.25f;

        Button.interactable = Event.file.playing && !Event.group.paused;
        Label.text = Event.triggerLabel;

        var c = InactiveColor;
        if (Event.running && !Event.completed)
            c = Color.Lerp(ActiveColor, CompletedColor, Event.timeFraction);
        if (Event.group.paused)
            c.a *= 0.5f;

        if (recentlyPressed)
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

        if (recentlyPressed)
            l = LabelActiveColor;

        if (popupEvent != null)
            l = hasPopup ? LabelActiveColor : LabelInactiveColor;

        Label.color = l;
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Button click handler. */
    private void OnButtonClicked()
    {
        _lastClickTime = Time.time;

        if (Event != null)
            Event.Trigger();
    }
}
