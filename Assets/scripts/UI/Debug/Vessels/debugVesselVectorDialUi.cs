using System;
using UnityEngine;
using DG.Tweening;
using Meg.Networking;
using TouchScript.Behaviors;
using TouchScript.Gestures;
using UnityEngine.Events;
using UnityEngine.UI;

public class debugVesselVectorDialUi : MonoBehaviour
{

    // Components
    // ------------------------------------------------------------

    [Header("Components")]

    /** The panning dial backdrop (defines range of motion). */
    public Graphic Dial;

    /** The panning dial thumb (moved by user). */
    public Graphic Thumb;

    /** The thumb transformer. */
    public Transformer Transformer;


    // Properties
    // ------------------------------------------------------------

    [Header("Configuration")]

    /** Thumb's active color. */
    public Color ThumbActiveColor;

    /** Thumb's normal color. */
    public Color ThumbDefaultColor
        { get { return serverUtils.GetColorTheme().highlightColor; } }

    /** The vessel being controlled by this dial. */
    public vesselData.Vessel Vessel
    {
        get { return _vessel; }
        set { SetVessel(value); }
    }


    // Events
    // ------------------------------------------------------------

    /** Event fired when dial value changes. */
    public UnityEvent onValueChanged = new UnityEvent();


    // Private Properties
    // ------------------------------------------------------------

    private vesselMovements Movements
        { get { return serverUtils.VesselMovements; } }

    private vesselMovement Movement
        { get { return Movements ? Movements.GetVesselMovement(Vessel.Id) : null; } }

    private vesselSetVector SetVector
        { get { return Movement as vesselSetVector; } }


    // Members
    // ------------------------------------------------------------

    /** The vessel being edited. */
    private vesselData.Vessel _vessel;

    /** The thumb transform gesture. */
    private TransformGesture _transformGesture;

    /** Whether thumb is being transformed. */
    private bool _transforming;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        if (!Transformer)
            Transformer = GetComponentInChildren<Transformer>();

        _transformGesture = Transformer.GetComponent<TransformGesture>();
        _transformGesture.TransformStarted += OnTransformStarted;
        _transformGesture.TransformCompleted += OnTransformCompleted;
    }

    /** Enabling. */
    private void OnEnable()
    {
    }

    /** Updating. */
    private void Update()
    {
        if (_transforming)
            UpdateWhileTransforming();
        else
            UpdateDialFromVessel();
    }

    /** Disabling. */
    private void OnDisable()
    {
    }


    // Public Methods
    // ------------------------------------------------------------


    // Private Methods
    // ------------------------------------------------------------

    private void SetVessel(vesselData.Vessel value)
    {
        _vessel = value;
        InitUi();
    }

    private void InitUi()
    {
        UpdateDialFromVessel();
    }

    private void OnTransformStarted(object sender, EventArgs e)
    {
        _transforming = true;

        Thumb.DOColor(ThumbActiveColor, 0.25f);
        Thumb.transform.DOPunchScale(Thumb.transform.localScale * 0.05f, 0.15f);
    }

    private void OnTransformCompleted(object sender, EventArgs e)
    {
        _transforming = false;

        Transformer.transform.localPosition = Thumb.transform.localPosition;
        Thumb.DOColor(ThumbDefaultColor, 0.25f);
    }

    private void UpdateWhileTransforming()
    {
        // Clamp thumb to transformer's position, but don't leave the dial.
        var local = Transformer.transform.localPosition;
        var extent = Dial.rectTransform.sizeDelta.x * 0.5f;
        if (local.magnitude > extent)
            local = local.normalized * extent;

        Thumb.transform.localPosition = local;
        UpdateVesselFromDial();
    }

    private void UpdateDialFromVessel()
    {
        var vector = SetVector;
        if (!SetVector)
            return;

        var extent = Dial.rectTransform.sizeDelta.x * 0.5f;
        var max = vector.MaxSpeed;
        var d = max > 0 ? (vector.Speed / max) * extent : 0;
        var p = Quaternion.Euler(0, 0, -vector.Heading) * new Vector3(0, d, 0);

        Thumb.transform.localPosition = p;
        Transformer.transform.localPosition = p;
    }

    private void UpdateVesselFromDial()
    {
        var vector = SetVector;
        if (!SetVector)
            return;

        var extent = Dial.rectTransform.sizeDelta.x * 0.5f;
        var p = Thumb.transform.localPosition;
        vector.Heading = Mathf.Repeat(90 - Mathf.Atan2(p.y, p.x) * Mathf.Rad2Deg, 360);
        vector.Speed = Mathf.Clamp01(p.magnitude / extent) * vector.MaxSpeed;

        if (!vector.isServer)
            vector.PostState();

        if (onValueChanged != null)
            onValueChanged.Invoke();
    }

}

