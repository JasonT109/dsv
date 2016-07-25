using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Meg.Networking;

public class HUDLinearGauge : MonoBehaviour
{

    // Enumerations
    // ------------------------------------------------------------

    /** Possible directions for gauge values. */
    public enum LinearGaugeDirection
    {
        LowToHigh = -1,
        HighToLow = 1
    };


    // Properties
    // ------------------------------------------------------------

    [Header("Quantity")]

    /** Server data ID of the quantity to be displayed. */
    public string ServerQuantity;

    /** Minimum value. */
    public float MinValue = 0;

    /** Maximum value. */
    public float MaxValue = 100;

    /** Whether to wrap value around if it exceeds the max value. */
    public bool Wrap;

    /** Current value. */
    public float Value;


    [Header("Appearance")]

    /** Range of values visible in the gauge. */
    public float VisibleRange = 20;

    /** Direction of the gauge values. */
    public LinearGaugeDirection Direction = LinearGaugeDirection.LowToHigh;

    /** Amount of smoothing for gauge movement (seconds to reach target). */
    public float SmoothTime = 0;


    [Header("Ticks")]

    /** Increment between successive main ticks. */
    public float MainTickInterval = 5;

    /** Increment between successive ticks. */
    public float SubTickInterval = 1;

    /** Format string for tick labels. */
    public string TickFormat = "{0:N0}";

    /** Tick local position offset. */
    public Vector3 TickLocalOffset = Vector3.zero;

    /** Tick local rotation (euler angles). */
    public Vector3 TickLocalRotation = Vector3.zero;

    /** Prefab used for a main tick. */
    public GameObject MainTickPrefab;

    /** Prefab used for a sub tick. */
    public GameObject SubTickPrefab;

    /** Prefab used for the zero-point tick (optional). */
    public GameObject ZeroTickPrefab;


    // Private properties
    // ------------------------------------------------------------

    /** Return value displayed at the low end of the gauge. */
    private float LowValue
        { get { return _smoothed - VisibleRange * 0.5f; } }

    /** Return value displayed at the high end of the gauge. */
    private float HighValue
        { get { return _smoothed + VisibleRange * 0.5f; } }


    // Members
    // ------------------------------------------------------------

    /** Gauge's currently displayed value (smoothly follows actual value). */
    private float _smoothed;

    /** Velocity for smoothing. */
    private float _smoothedVelocity;

    /** Collider, used for sizing. */
    private Collider _collider;

    /** Max width and height for the gauge (local space). */
    private Vector2 _maxSize;

    /** Scale factor used to convert from values to local space. */
    private float _valueToLocalScale;

    /** List of ticks used in the gauge. */
    private readonly List<GameObject> _ticks = new List<GameObject>();

    /** List of tick renderers used in the gauge. */
    private readonly List<Renderer> _renderers = new List<Renderer>();

    /** List of tick labels used in the gauge. */
    private readonly List<widgetText> _labels = new List<widgetText>();

    /** The zero tick (optional). */
    private GameObject _zeroTick;

    /** The zero tick renderer. */
    private Renderer _zeroRenderer;

    /** The zero tick label. */
    private widgetText _zeroLabel;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        // Initialize format string if needed.
        if (string.IsNullOrEmpty(TickFormat))
            TickFormat = "{0:N0}";

        // Initialize sizing behaviour.
        InitializeSizing();

        // Create tick objects.
        InitializeTicks();

        // Perform an initial update.
        UpdateValue(0);
        UpdateTicks();
    }

    /** Enabling. */
    private void OnEnable()
    {
        UpdateValue(0);
        UpdateTicks();
    }

    /** Update. */
    private void Update()
    {
        UpdateValue(SmoothTime);
        UpdateTicks();
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Initialize sizing information. */
    private void InitializeSizing()
    {
        if (!_collider)
            _collider = GetComponent<Collider>();
        if (!_collider)
            return;

        var min = transform.InverseTransformPoint(_collider.bounds.min);
        var max = transform.InverseTransformPoint(_collider.bounds.max);
        _maxSize = max - min;

        // Compute spacing between ticks.
        _valueToLocalScale = _maxSize.x / VisibleRange;
    }

    /** Initialize the tick objects. */
    private void InitializeTicks()
    {
        // Determine the number of ticks that are visible.
        var nMainTicks = Mathf.CeilToInt(VisibleRange / MainTickInterval) + 2;
        var mainTickPeriod = Mathf.RoundToInt(MainTickInterval / SubTickInterval);
        var nTicks = nMainTicks * mainTickPeriod;

        // Create the tick collection.
        for (var i = 0; i < nTicks; i++)
        {
            var prefab = SubTickPrefab;
            if (i % mainTickPeriod == 0)
                prefab = MainTickPrefab;

            var tick = Instantiate(prefab);
            tick.transform.parent = gameObject.transform;
            tick.transform.localRotation = Quaternion.Euler(TickLocalRotation);

            _ticks.Add(tick);
            _renderers.Add(tick.GetComponentInChildren<Renderer>());
            _labels.Add(tick.GetComponentInChildren<widgetText>());
        }

        // Create the zero tick (if specified).
        if (ZeroTickPrefab)
        {
            _zeroTick = Instantiate(ZeroTickPrefab);
            _zeroTick.transform.parent = gameObject.transform;
            _zeroTick.transform.localRotation = Quaternion.Euler(TickLocalRotation);
            _zeroTick.SetActive(false);
            _zeroRenderer = _zeroTick.GetComponentInChildren<Renderer>();
            _zeroLabel = _zeroTick.GetComponentInChildren<widgetText>();
        }
    }

    /** Update the value of this gauge. */
    private void UpdateValue(float smoothTime)
    {
        if (!string.IsNullOrEmpty(ServerQuantity))
            Value = serverUtils.GetServerData(ServerQuantity);

        if (smoothTime > 0)
            _smoothed = Mathf.SmoothDamp(_smoothed, Value, ref _smoothedVelocity, smoothTime);
        else
            _smoothed = Value;
    }

    /** Position ticks according to current value. */
    private void UpdateTicks()
    {
        // Disable the zero tick by default.
        if (_zeroTick)
            _zeroTick.SetActive(false);

        // Position ticks.
        var value = GetInitialTickValue();
        for (var i = 0; i < _ticks.Count; i++)
        {
            var visible = value >= LowValue && value <= HighValue;
            var isZero = Mathf.Approximately(value, 0) && _zeroTick;
            var isNonZero = !isZero;

            // Set tick visibility.
            _ticks[i].SetActive(visible && isNonZero);

            // Select a regular (or zero) tick to position.
            var tick = isNonZero ? _ticks[i] : _zeroTick;
            if (isZero)
                _zeroTick.SetActive(visible);

            // Position the tick.
            tick.transform.localPosition = ValueToLocal(value);

            // Update tick label.
            var label = isNonZero ? _labels[i] : _zeroLabel;
            if (label)
            {
                var c = label.Color;
                label.Text = string.Format(TickFormat, value);
                label.Color = new Color(c.r, c.g, c.b, ValueToAlpha(value));
            }

            // Update tick color.
            var r = isNonZero ? _renderers[i] : _zeroRenderer;
            if (r && r.material.HasProperty("_Color"))
            {
                var c = r.material.color;
                r.material.color = new Color(c.r, c.g, c.b, ValueToAlpha(value));
            }
            else if (r && r.material.HasProperty("_TintColor"))
            {
                var c = r.material.GetColor("_TintColor");
                r.material.SetColor("_TintColor", new Color(c.r, c.g, c.b, ValueToAlpha(value)));
            }

            // Update current value.
            value = ConstrainValue(value + SubTickInterval);
        }
    }

    /** Compute the correct position in local space for a given value. */
    private Vector3 ValueToLocal(float value)
    {
        var x = (value - _smoothed) * _valueToLocalScale * (int) Direction;
        return new Vector3(x, 0, 0) + TickLocalOffset;
    }

    /** Compute the correct alpha for a given tick value. */
    private float ValueToAlpha(float value)
    {
        var f = Mathf.Abs((value - _smoothed) / (VisibleRange * 0.5f));
        var a = Mathf.Clamp01((1 - f) * 5);
        return a;
    }

    /** Determine the gauge's starting tick value, given current gauge value. */
    private float GetInitialTickValue()
    {
        var raw = (Mathf.FloorToInt(LowValue / MainTickInterval) - 1) * MainTickInterval;
        return ConstrainValue(raw);
    }

    /** Constrain value to legal range. */
    private float ConstrainValue(float value)
    {
        if (Wrap)
            return MinValue + Mathf.Repeat(value - MinValue, MaxValue - MinValue);

        return value;
    }


}
