using System;
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using DG.Tweening;
using Meg.Networking;
using TouchScript.Gestures;

public class DomeScreen : MonoBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** Default tween duration. */
    private const float DefaultTweenDuration = 0.25f;

    /** Outline tween duration. */
    private const float OutlineTweenDuration = 0.5f;

    /** Label tween duration. */
    private const float LabelTweenDuration = 0.75f;


    // Components
    // ------------------------------------------------------------

    [Header("Components")]

    public MeshRenderer Fill;
    public MeshRenderer Outline;
    public MeshRenderer Highlight;
    public widgetText Label;


    [Header("Configuration")]

    /** The name of this screen. */
    public string Name;


    [Header("Server Data")]

    /** Server data value that governs this screen's content. */
    public string linkDataString;


    [Header("Colors")]

    public Color FillOnColor = Color.white;
    public Color FillOffColor = new Color(0, 0, 0, 0);

    public Color OutlineOnColor = Color.white;
    public Color OutlineOffColor = new Color(0, 0, 0, 0);
    public Color OutlinePressedColor = new Color(0, 0, 0, 0);

    public Color HighlightOnColor = Color.white;
    public Color HighlightOffColor = new Color(0, 0, 0, 0);

    public Color LabelOnColor = Color.white;
    public Color LabelOffColor = new Color(0, 0, 0, 0);
    public Color LabelHiddenColor = new Color(0, 0, 0, 0);


    // Properties
    // ------------------------------------------------------------

    /** Set that this screen belongs to. */
    public DomeScreens Screens { get; private set; }

    /** Whether screen is currently on (powered). */
    public bool On { get { return _on; } set { SetOn(value); } }

    /** Whether screen is currently pressed. */
    public bool Pressed { get { return _pressed; } set { SetPressed(value); } }

    /** Whether screen is hovered over. */
    public bool Hovering { get { return _hovering; } set { SetHovering(value); } }

    /** Whether screen has an overlay assigned. */
    public bool HasOverlay { get { return !string.IsNullOrEmpty(OverlayName); } }

    /** Screen's current overlay. */
    public Overlay CurrentOverlay { get { return _overlay; } private set { SetOverlay(value); } }

    /** Screen's current overlay name. */
    public string OverlayName { get { return _overlay.Name; } }

    /** Whether screen's label is currently being dragged. */
    public bool Dragging{ get { return _dragging; } set { SetDragging(value); } }


    // Structures
    // ------------------------------------------------------------

    [System.Serializable]
    public struct Overlay
    {
        public string Name;
        public domeData.OverlayId Id;
    }


    // Members
    // ------------------------------------------------------------

    /** Whether screen is currently on. */
    private bool _on;

    /** Time when screen was turned on. */
    private float _onTime;

    /** Whether screen is currently pressed. */
    private bool _pressed;

    /** Whether screen is hovered over. */
    private bool _hovering;

    /** Whether screen's label is currently being dragged. */
    private bool _dragging;

    /** Whether UI state is dirty. */
    private bool _dirty;

    /** Screen's current overlay. */
    private Overlay _overlay;

    /** Label transform gesture. */
    private TransformGesture _labelTransform;

    /** Label home position. */
    private Vector3 _homePosition;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Awake()
    {
        var press = GetComponent<PressGesture>();
        var longPress = GetComponent<LongPressGesture>();
        var release = GetComponent<ReleaseGesture>();

        if (press)
            press.Pressed += OnPressed;
        if (release)
            release.Released += OnReleased;
        if (longPress)
            longPress.LongPressed += OnLongPressed;

        Outline.material.SetColor("_TintColor", OutlineOffColor);
        Highlight.material.SetColor("_TintColor", HighlightOffColor);
        Fill.material.SetColor("_TintColor", FillOffColor);
        Label.Color = LabelOffColor;

        Screens = GetComponentInParent<DomeScreens>();

        _labelTransform = Label.GetComponent<TransformGesture>();
        if (_labelTransform)
        {
            _labelTransform.TransformStarted += OnTransformStarted;
            _labelTransform.TransformCompleted += OnTransformComplete;
        }
    }

    /** Enabling. */
    private void OnEnable()
    {
        // Force an overlay update when returning to screen.
        SetOverlay(_overlay, true);
    }

    /** Updating. */
    private void LateUpdate()
    {
        UpdateOverlayFromData();

        if (_labelTransform)
            _labelTransform.enabled = HasOverlay;

        if (!_dirty)
            return;

        UpdateUi();
        _dirty = false;
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Toggle the button's on state. */
    public void Toggle()
        { On = !On; }

    /** Clear the screen. */
    public void Clear()
        { RequestOverlay(domeData.OverlayId.None); }

    /** Request an overlay to be applied to this screen. */
    public void RequestOverlay(domeData.OverlayId id)
        { serverUtils.PostServerData(linkDataString, (float) id); }

    /** Request panning dial be displayed for this screen. */
    public void RequestPan()
    {
        On = true;
        Screens.PanDial.Show(this);
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Update the current overlay from server data. */
    private void UpdateOverlayFromData()
    {
        var lastOverlayId = CurrentOverlay.Id;
        var index = Mathf.RoundToInt(serverUtils.GetServerData(linkDataString));
        var overlayId = (domeData.OverlayId) index;
        SetOverlay(Screens.GetOverlay(overlayId));
        if (overlayId != domeData.OverlayId.None && lastOverlayId != overlayId)
            SetOn(true);
    }

    /** Sets the screen's 'on' state. */
    private void SetOn(bool value)
    {
        if (_on == value)
            return;

        _on = value;
        _dirty = true;

        if (value)
            _onTime = Time.time;
    }

    /** Sets the screen's 'pressed' state. */
    private void SetPressed(bool value)
    {
        if (_pressed == value)
            return;

        _pressed = value;
        _dirty = true;
    }

    /** Sets the screen's 'hover' state. */
    private void SetHovering(bool value)
    {
        if (_hovering == value)
            return;

        _hovering = value;
        _dirty = true;
    }

    /** Sets the screen's 'dragging' state. */
    private void SetDragging(bool value)
    {
        if (_dragging == value)
            return;

        _dragging = value;
        _dirty = true;
    }

    /** Sets the screen's overlay. */
    private void SetOverlay(Overlay value, bool forceUpdate = false)
    {
        if ((_overlay.Name == value.Name) && !forceUpdate)
            return;

        // Update the overlay display label.
        if (!string.IsNullOrEmpty(value.Name))
        {
            var label = Regex.Replace(value.Name, @"\s+", "\n").ToUpper();
            StopAllCoroutines();
            StartCoroutine(ChangeLabelRoutine(label));
        }

        _overlay = value;
        _dirty = true;
    }

    /** Routine to change the current overlay name. */
    private IEnumerator ChangeLabelRoutine(string value)
    {
        Label.Text = "";
        yield return new WaitForSeconds(0.1f);
        Label.Text = value;

        var dyn = Label.GetComponent<DynamicText>();
        if (dyn)
            dyn.GenerateMesh();

        var t = Label.transform;
        if (DOTween.IsTweening(t))
            yield break;

        if (dyn)
            dyn.pixelSnapTransformPos = false;

        t.DOPunchScale(t.localScale * 0.05f, 0.25f);
        yield return new WaitForSeconds(0.25f);

        if (dyn)
        { 
            dyn.pixelSnapTransformPos = true;
            dyn.GenerateMesh();
        }
    }

    /** Update the screen's appearance based on current state. */
    private void UpdateUi()
    {
        var hot = On || Pressed;
        var d = DefaultTweenDuration;

        if (HasOverlay)
            FadeLabel(Label, hot ? LabelOnColor : LabelOffColor, LabelTweenDuration);
        else
            FadeLabel(Label, LabelHiddenColor, LabelTweenDuration);

        FadeRenderer(Outline, Pressed ? OutlinePressedColor 
            : (hot ? OutlineOnColor : OutlineOffColor), OutlineTweenDuration);

        if (Time.time < (_onTime + d))
            FadeRenderer(Highlight, HighlightOnColor, d).OnComplete(UpdateUi);
        else
            FadeRenderer(Highlight, (hot && HasOverlay) || Pressed ? HighlightOnColor : HighlightOffColor, d);

        FadeRenderer(Fill, Hovering ? FillOnColor : FillOffColor, d);
    }

    private Tweener FadeRenderer(MeshRenderer r, Color c, float duration)
    {
        DOTween.Kill(r.material);

        if (r.material.HasProperty("_TintColor"))
            return r.material.DOColor(c, "_TintColor", duration);

        return r.material.DOColor(c, duration);
    }

    private Tweener FadeLabel(widgetText label, Color c, float duration)
    {
        DOTween.Kill(Label);
        return DOTween.To(() => label.Color, x => label.Color = x, c, duration);
    }

    /** Handle the screen's button being pressed. */
    private void OnPressed(object sender, EventArgs eventArgs)
    {
        Pressed = true;
    }

    /** Handle the screen's button being released. */
    private void OnReleased(object sender, EventArgs eventArgs)
    {
        Pressed = false;
        if (!Screens.PanDial.Showing)
            Toggle();
    }

    /** Handle a long press on the screen. */
    private void OnLongPressed(object sender, EventArgs eventArgs)
    {
        Pressed = false;
        RequestPan();
    }

    /** Handle the screen's label being transformed. */
    private void OnTransformStarted(object sender, EventArgs eventArgs)
    {
        Dragging = true;
    }

    /** Handle the screen's label being transformed. */
    private void OnTransformComplete(object sender, EventArgs eventArgs)
    {
        Dragging = false;

        // Determine what screen label was dropped on (if any).
        var screen = GetScreenUnderLabel();

        // Reset the label.
        Label.transform.localPosition = Vector3.zero;
        if (screen != this)
            Label.Text = "";

        // Toggle screen if releasing on self (will toggle again in OnReleased).
        if (screen == this)
            Toggle();

        // Check if we need to switch screens.
        if (!HasOverlay || screen == this)
            return;

        // Clear this overlay.
        RequestOverlay(domeData.OverlayId.None);

        // Drop overlay onto screen.
        if (screen)
        {
            screen.On = true;
            screen.RequestOverlay(_overlay.Id);
        }
    }

    /** Locate the screen underneath the label. */
    private DomeScreen GetScreenUnderLabel()
    {
        var s = Camera.main.WorldToScreenPoint(Label.transform.position);
        var ray = Camera.main.ScreenPointToRay(s);
        var hits = Physics.RaycastAll(ray);
        foreach (var hit in hits)
        {
            var screen = hit.transform.GetComponent<DomeScreen>();
            if (screen)
                return screen;
        }

        return null;
    }

}
