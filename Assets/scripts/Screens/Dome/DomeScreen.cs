using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using DG.Tweening;
using Meg.Networking;

public class DomeScreen : MonoBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** Default tween duration. */
    private const float DefaultTweenDuration = 1.0f;

    /** Outline tween duration. */
    private const float OutlineTweenDuration = 0.5f;

    /** Label tween duration. */
    private const float LabelTweenDuration = 0.75f;


    // Components
    // ------------------------------------------------------------

    [Header("Components")]

    public buttonControl Button;
    public MeshRenderer Fill;
    public MeshRenderer Outline;
    public MeshRenderer Highlight;
    public widgetText Label;


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

    /** Screen's current name. */
    public string Name { get { return _overlay.Name; } }

    /** Whether screen is hovered over. */
    public bool Hovering { get { return _hovering; } set { SetHovering(value); } }

    /** Whether screen has an overlay assigned. */
    public bool HasOverlay { get { return !string.IsNullOrEmpty(Name); } }

    /** Screen's current overlay. */
    public Overlay CurrentOverlay { get { return _overlay; } private set { SetOverlay(value); } }


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

        Screens = GetComponentInParent<DomeScreens>();
    }

    /** Updating. */
    private void LateUpdate()
    {
        UpdateOverlayFromData();
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
        { RequestOverlay(domeData.OverlayId.None); }

    /** Request an overlay to be applied to this screen. */
    public void RequestOverlay(domeData.OverlayId id)
        { serverUtils.PostServerData(linkDataString, (float) id); }


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

    /** Sets the screen's overlay. */
    private void SetOverlay(Overlay value)
    {
        if (_overlay.Name == value.Name)
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
        var wait = new WaitForSeconds(0.05f);
        /*
        var oldLength = Label.Text.Length;
        for (var i = 0; i < oldLength; i++)
        {
            Label.Text = Label.Text.Substring(0, Label.Text.Length - 1);
            yield return wait;
        }
        */

        var newLength = value.Length;
        for (var i = 0; i < newLength; i++)
        {
            Label.Text = value.Substring(0, i + 1);
            yield return wait;
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
    private void OnButtonPressed()
    {
    }

    /** Handle the screen's button being released. */
    private void OnButtonReleased()
    {
        Toggle();
    }



}
