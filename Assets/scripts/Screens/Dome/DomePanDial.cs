using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using Meg.Networking;
using TouchScript.Behaviors;
using TouchScript.Gestures;

public class DomePanDial : MonoBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** Interval for automatically hiding the panning dial. */
    public const float AutoHideInterval = 2.0f;



    // Components
    // ------------------------------------------------------------

    [Header("Components")]

    /** Backdrop fill, used to dim out the dome interface. */
    public MeshRenderer Fill;

    /** Root transform for positioning the dial. */
    public Transform Root;

    /** The panning dial backdrop (defines range of motion). */
    public graphicsSlicedMesh Dial;

    /** The panning dial thumb (moved by user). */
    public Transform Thumb;

    /** The thumb transformer. */
    public Transformer Transformer;

    /** Title label. */
    public widgetText Title;

    /** Screen name label. */
    public widgetText ScreenName;


    /** Thumb's normal color. */
    public Color ThumbDefaultColor
        { get { return serverUtils.GetColorTheme().highlightColor; } }

    /** Thumb's active color. */
    public Color ThumbActiveColor;


    // Members
    // ------------------------------------------------------------

    /** The thumb transform gesture. */
    private TransformGesture _transformGesture;

    /** Whether thumb is being transformed. */
    private bool _transforming;

    /** Timestamp for next possible autohide. */
    private float _autoHideTime;

    /** Whether dial is showing. */
    private bool _showing;


    // Properties
    // ------------------------------------------------------------

    /** Whether the panning dial is showing. */
    public bool Showing
        { get { return _showing; } }


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        _transformGesture = Transformer.GetComponent<TransformGesture>();
        _transformGesture.TransformStarted += OnTransformStarted;
        _transformGesture.TransformCompleted += OnTransformCompleted;
    }

    /** Enabling. */
    private void OnEnable()
    {
        Thumb.GetComponent<Renderer>().material.DOColor(ThumbDefaultColor, "_TintColor", 0.25f);
        _autoHideTime = Time.time + AutoHideInterval;
    }

    /** Updating. */
    private void Update()
    {
        if (_transforming)
            UpdateWhileTransforming();
        else if (_showing && Time.time > _autoHideTime)
            Hide();
    }

    /** Disabling. */
    private void OnDisable()
    {
        // Hide dial whenever we leave the screen.
        gameObject.SetActive(false);
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Display the panning dial for a given screen. */
    public void Show(DomeScreen screen)
    {
        _showing = true;
        gameObject.SetActive(true);
        ScreenName.Text = screen.Name.ToUpper() + " VIEWPORT";

        // Position dial over the requested screen.
        var local = transform.InverseTransformPoint(screen.transform.position);
        Root.transform.DOKill();
        Root.transform.localPosition = new Vector3(local.x, 0, 0);
        Root.transform.localScale = Vector3.zero;
        Root.transform.DOScale(Vector3.one, 0.25f);

        // Fade in the fill.
        Fill.material.DOKill();
        Fill.material.SetColor("_TintColor", new Color(0,0,0,0));
        Fill.material.DOFade(0.35f, "_TintColor", 0.5f);

        // Schedule auto-hide.
        _autoHideTime = Time.time + AutoHideInterval;
    }

    /** Hide the panning dial. */
    public void Hide()
    {
        if (_showing)
            StartCoroutine(HideRoutine());

        _showing = false;
    }


    // Private Methods
    // ------------------------------------------------------------

    private IEnumerator HideRoutine()
    {
        // Fade out the dial.
        Root.transform.DOKill();
        Root.transform.DOScale(Vector3.zero, 0.25f);

        // Fade out the fill.
        Fill.material.DOKill();
        Fill.material.DOFade(0, "_TintColor", 0.25f);

        // Disable dial after tweens.
        yield return new WaitForSeconds(0.35f);
        gameObject.SetActive(false);
    }

    private void OnTransformStarted(object sender, EventArgs e)
    {
        _transforming = true;

        Thumb.GetComponent<Renderer>().material.DOColor(ThumbActiveColor, "_TintColor", 0.25f);
        Thumb.transform.DOPunchScale(Thumb.transform.localScale * 0.05f, 0.15f);
        Dial.transform.DOPunchScale(Thumb.transform.localScale * 0.05f, 0.15f);
    }

    private void OnTransformCompleted(object sender, EventArgs e)
    {
        _transforming = false;

        // Reset transformer's position.
        Transformer.transform.localPosition = Vector3.zero;

        // Tween thumb back to center.
        Thumb.transform.DOLocalMove(Vector3.zero, 0.25f);
        Thumb.GetComponent<Renderer>().material.DOColor(ThumbDefaultColor, "_TintColor", 0.25f);
    }

    private void UpdateWhileTransforming()
    {
        // Clamp thumb to transformer's position.
        var local = Transformer.transform.localPosition;
        var extent = Dial.Width * 0.5f;
        if (local.magnitude > extent)
            local = local.normalized * extent;

        Thumb.transform.localPosition = local;

        // Reschedule auto-hide.
        _autoHideTime = Time.time + AutoHideInterval;
    }

}
