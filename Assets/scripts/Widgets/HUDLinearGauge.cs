using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Meg.Maths;
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

    /** Possible gauge layouts. */
    public enum LinearGaugeLayout
    {
        Linear = 0,
        Radial = 1
    }


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

    /** Layout of gauge values. */
    public LinearGaugeLayout Layout = LinearGaugeLayout.Linear;

    /** Amount of smoothing for gauge movement (seconds to reach target). */
    public float SmoothTime = 0;

    /** Whether to fade ticks at edge of visible range. */
    public bool FadeTicks = true;


    [Header("Ticks")]

    /** Increment between successive main ticks. */
    public float MainTickInterval = 5;

    /** Increment between successive ticks. */
    public float SubTickInterval = 1;

    /** Number of extra ticks to add (to prevent visible jumps). */
    public int ExtraMainTicks = 2;

    /** Format string for tick labels. */
    public string TickFormat = "{0:N0}";

    /** Prefab used for a main tick. */
    public GameObject MainTickPrefab;

    /** Prefab used for a sub tick. */
    public GameObject SubTickPrefab;

    /** Prefab used for the zero-point tick (optional). */
    public GameObject ZeroTickPrefab;


    [Header("Tick Orientation")]

    /** Tick local position offset. */
    public Vector3 TickLocalOffset = Vector3.zero;

    /** Tick local rotation (euler angles). */
    public Vector3 TickLocalRotation = Vector3.zero;

    /** Scale adjustment for main ticks. */
    public Vector3 MainTickScaling = Vector3.one;

    /** Scale adjustment for sub ticks. */
    public Vector3 SubTickScaling = Vector3.one;

    /** Scale adjustment for zeroticks. */
    public Vector3 ZeroTickScaling = Vector3.one;



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
        var nMainTicks = Mathf.CeilToInt(VisibleRange / MainTickInterval) + ExtraMainTicks;
        var mainTickPeriod = Mathf.RoundToInt(MainTickInterval / SubTickInterval);
        var nTicks = nMainTicks * mainTickPeriod;

        // Create the tick collection.
        for (var i = 0; i < nTicks; i++)
        {
            var prefab = SubTickPrefab;
            var scaling = SubTickScaling;
            if (i % mainTickPeriod == 0 || !prefab)
            {
                prefab = MainTickPrefab;
                scaling = MainTickScaling;
            }

            var tick = Instantiate(prefab);
            tick.transform.SetParent(gameObject.transform, false);
            tick.transform.localRotation = Quaternion.Euler(TickLocalRotation);
            tick.transform.localScale = Vector3.Scale(tick.transform.localScale, scaling);

            _ticks.Add(tick);
            _renderers.Add(tick.GetComponentInChildren<Renderer>());
            _labels.Add(tick.GetComponentInChildren<widgetText>());
        }

        // Create the zero tick (if specified).
        if (ZeroTickPrefab)
        {
            _zeroTick = Instantiate(ZeroTickPrefab);
            _zeroTick.transform.SetParent(gameObject.transform, false);
            _zeroTick.transform.localRotation = Quaternion.Euler(TickLocalRotation);
            _zeroTick.transform.localScale = Vector3.Scale(_zeroTick.transform.localScale, ZeroTickScaling);
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
            var isZero = Mathf.Approximately(value, 0) && _zeroTick;
            var isNonZero = !isZero;
            var visible = (value >= LowValue && value <= HighValue)
                || Layout == LinearGaugeLayout.Radial;

            // Set tick visibility.
            _ticks[i].SetActive(visible && isNonZero);

            // Select a regular (or zero) tick to position.
            var tick = isNonZero ? _ticks[i] : _zeroTick;
            if (isZero)
                _zeroTick.SetActive(visible);

            // Position the tick.
            tick.transform.localPosition = ValueToLocal(value);
            tick.transform.localRotation = ValueToLocalRotation(value);

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
        var v = (value - _smoothed) * (int) Direction;
        switch (Layout)
        {
            case LinearGaugeLayout.Radial:
                var theta = graphicsMaths.remapValue(v, MinValue, MaxValue, 0, 360);
                var radians = theta * Mathf.Deg2Rad;
                return new Vector3(Mathf.Cos(radians) * _maxSize.x * 0.5f, Mathf.Sin(radians) * _maxSize.y * 0.5f, 0);

            default:
                var x = v * _valueToLocalScale * (int) Direction;
                return new Vector3(x, 0, 0) + TickLocalOffset;
        }
    }

    /** Comput the correct orientation in local space for a given value. */
    private Quaternion ValueToLocalRotation(float value)
    {
        var v = (value - _smoothed) * (int) Direction * -1;
        switch (Layout)
        {
            case LinearGaugeLayout.Radial:
                var theta = graphicsMaths.remapValue(v, MinValue, MaxValue, 0, 360);
                return Quaternion.Euler(0, 0, theta) * Quaternion.Euler(TickLocalRotation);

            default:
                return Quaternion.Euler(TickLocalRotation);
        }
    }

    /** Compute the correct alpha for a given tick value. */
    private float ValueToAlpha(float value)
    {
        if (!FadeTicks)
            return 1;

        var f = Mathf.Abs((value - _smoothed) / (VisibleRange * 0.5f));
        var a = Mathf.Clamp01((1 - f)*5);
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
