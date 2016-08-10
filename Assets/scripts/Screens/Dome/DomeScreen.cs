using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DomeScreen : MonoBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** Default tween duration. */
    private const float DefaultTweenDuration = 0.25f;

    /** Label tween duration. */
    private const float LabelTweenDuration = 0.5f;


    // Components
    // ------------------------------------------------------------

    [Header("Components")]

    public buttonControl Button;
    public MeshRenderer Fill;
    public MeshRenderer Outline;
    public MeshRenderer Highlight;
    public widgetText Label;


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

    /** Whether screen is currently on (powered). */
    public bool On { get { return _on; } set { SetOn(value); } }

    /** Whether screen is currently pressed. */
    public bool Pressed { get { return _pressed; } set { SetPressed(value); } }

    /** Whether screen has an overlay assigned. */
    public bool HasOverlay { get { return !string.IsNullOrEmpty(Name); } }

    /** Screen's current name. */
    public string Name { get { return _overlay.Name; } }

    /** Screen's current overlay. */
    public Overlay Current { get { return _overlay; } set { SetOverlay(value); } }

    /** Whether screen is hovered over. */
    public bool Hovering { get { return _hovering; } set { SetHovering(value); } }


    // Structures
    // ------------------------------------------------------------

    [System.Serializable]
    public struct Overlay
    {
        public string Name;
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

    /** Whether UI state is dirty. */
    private bool _dirty;

    /** Screen's current overlay. */
    private Overlay _overlay;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Awake()
    {
        Button.onPressed += OnButtonPressed;
        Button.onReleased += OnButtonReleased;

        Outline.material.SetColor("_TintColor", OutlineOffColor);
        Highlight.material.SetColor("_TintColor", HighlightOffColor);
        Fill.material.SetColor("_TintColor", FillOffColor);
        Label.Color = LabelOffColor;
    }

    /** Updating. */
    private void LateUpdate()
    {
        Pressed = Button.pressed;

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
        { Current = new Overlay(); }

    // Private Methods
    // ------------------------------------------------------------

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

    /** Sets the screen's overlay. */
    private void SetOverlay(Overlay value)
    {
        if (_overlay.Name == value.Name)
            return;

        if (!string.IsNullOrEmpty(value.Name))
            Label.Text = value.Name;

        _overlay = value;
        _dirty = true;
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

        FadeRenderer(Outline, Pressed ? OutlinePressedColor : (hot ? OutlineOnColor : OutlineOffColor), 0);

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
    private void OnButtonPressed()
    {
    }

    /** Handle the screen's button being released. */
    private void OnButtonReleased()
    {
        Toggle();
    }



}
