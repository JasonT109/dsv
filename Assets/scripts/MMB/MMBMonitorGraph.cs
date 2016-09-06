using UnityEngine;
using Meg.Maths;
using Meg.Networking;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

/** A line that animates radially - useful for gauges and dials. */

[RequireComponent(typeof(VectorLine))]
public class MMBMonitorGraph : MonoBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** Default cycle rate. */
    public const float DefaultRate = 60;

    /** Default minimum value. */
    public const float DefaultMin = 0;

    /** Default maximum value. */
    public const float DefaultMax = 100;

    /** Number units per sample. */
    public const float UnitsPerSample = 2;


    // Properties
    // ------------------------------------------------------------

    [Header("Server Values")]

    /** Server parameter controlling the curve's cycle rate. */
    public string RateParameter = "crewHeartrate1";

    /** Server parameter controlling the curve's minimum amplitude (optional). */
    public string MinParameter = "";

    /** Server parameter controlling the curve's maximum amplitude (optional). */
    public string MaxParameter = "";


    [Header("Dimensions")]

    /** Graph's dimensions. */
    public float Width = 1750;
    public float Height = 280;

    /** Graph's vertical minimum and maximum values. */
    public float MinValue = 0;
    public float MaxValue = 100;

    /** Graph's travelling speed (units/second). */
    public float Speed = 175;


    [Header("Appearance")]

    /** Curve defining the shape of a single cycle. */
    public AnimationCurve Shape;

    /** Curve defining the line's color as it ages. */
    public Gradient ColorForAge;

    /** Curve defining the line's width as it ages. */
    public AnimationCurve WidthForAge;

    /** Noise to add in. */
    public SmoothNoise Noise;


    // Members
    // ------------------------------------------------------------

    /** Sampling position within the current cycle. */
    private float _t;

    /** Target graph point index. */
    private float _target;

    /** Current graph point index. */
    private int _index;

    /** The line component. */
    private VectorObject2D _vectorObject;

    /** The set of line colors to apply. */
    private readonly List<Color32> _colors = new List<Color32>();

    /** The set of line widths to apply. */
    private readonly List<float> _widths = new List<float>();

    /** The set of sample ages. */
    private readonly List<float> _ages = new List<float>();




    // Unity Methods
    // ------------------------------------------------------------

    /** Startup. */
    private void Awake()
    {
    }

    /** Enabling. */
    private void OnEnable()
    {
    }

    /** Update the bar. */
    private void Update()
    {
        UpdateLine();
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Update the signal being monitored. */
    private float Sample(float dt)
    {
        var rate = serverUtils.GetServerData(RateParameter, DefaultRate);
        var min = serverUtils.GetServerData(MinParameter, DefaultMin);
        var max = serverUtils.GetServerData(MaxParameter, DefaultMax);
        if (rate <= 0)
            return min;

        // Convert from cycles/minute to cycles/second.
        var speed = rate / 60;

        _t = Mathf.Repeat(_t + (speed * dt), 1);
        var s = Shape.Evaluate(_t);

        // Add in some noise.
        s += Noise.Update();

        return graphicsMaths.remapValue(s, 0, 1, min, max);
    }

    /** Initialize the line component. */
    private void InitLine()
    {
        if (_vectorObject || Width <= 0)
            return;

        // Initialize line points.
        _vectorObject = GetComponent<VectorObject2D>();
        var line = _vectorObject.vectorLine;
        var count = Mathf.RoundToInt(Width / UnitsPerSample);
        line.Resize(count);
        var dx = Width / count;
        for (var i = 0; i < count; i++)
            line.points2[i] = new Vector2(i * dx, 0);

        // Initialize sample ages.
        _ages.Clear();
        var maxAge = Width / Speed;
        for (var i = 0; i < count; i++)
            _ages.Add(maxAge);

        // Initialize line colors.
        _colors.Clear();
        for (var i = 0; i < count - 1; i++)
            _colors.Add(ColorForAge.Evaluate(_ages[i]));
        line.SetColors(_colors);

        // Initialize line widths.
        _widths.Clear();
        for (var i = 0; i < count - 1; i++)
            _widths.Add(WidthForAge.Evaluate(_ages[i]));
        line.SetWidths(_widths);
    }

    /** Update the bar from current value. */
    private void UpdateLine()
    {
        // Initialize line on demand.
        if (!_vectorObject)
            InitLine();

        // Check if line is OK to draw.
        if (!_vectorObject || Width <= 0 || Speed <= 0)
            return;

        // Update the target sample index.
        var line = _vectorObject.vectorLine;
        var count = line.points2.Count;
        var samplesPerSecond = Speed / UnitsPerSample;
        _target += samplesPerSecond * Time.deltaTime;

        // Update samples to keep up.
        var dx = Width / count;
        var dt = 1 / samplesPerSecond;
        while (_index < _target)
        {
            var i = _index % count; 
            var value = Sample(dt);
            var y = graphicsMaths.remapValue(value, MinValue, MaxValue, 0, Height);
            y = Mathf.Clamp(y, 0, Height);

            line.points2[i] = new Vector2(i * dx, y);
            _ages[i] = 0;
            _index++;
        }

        // Update sample ages.
        for (var i = 0; i < count; i++)
            _ages[i] += Time.deltaTime;

        // Update line colors.
        var scale = Speed / Width;
        for (var i = 0; i < count - 1; i++)
            _colors[i] = ColorForAge.Evaluate(_ages[i] * scale);
        line.SetColors(_colors);

        // Update line widths.
        for (var i = 0; i < count - 1; i++)
            _widths[i] = WidthForAge.Evaluate(_ages[i] * scale);
        line.SetWidths(_widths);

        // Redraw the line.
        line.Draw();
    }

}
