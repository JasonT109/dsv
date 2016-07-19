using UnityEngine;
using System.Collections;
using Vectrosity;

/** A line that animates radially - useful for gauges and dials. */

[ExecuteInEditMode]
[RequireComponent(typeof(VectorLine))]
public class widgetRadialBar : MonoBehaviour, ValueSettable
{

    // Constants
    // ------------------------------------------------------------

    /** Number of line segments to use per degree of arc. */
    private const float SegmentsPerDegree = 0.5f;


    // Properties
    // ------------------------------------------------------------

    /** Radius of the bar. */
    public float Radius = 0.5f;

    /** Angle range, in degrees. */
    public Vector2 AngleRange = new Vector2(0, 360);

    /** Minimum value. */
    public float MinValue = 0;

    /** Maximum value. */
    public float MaxValue = 100;

    /** The bar's current value. */
    public float Value;


    // Members
    // ------------------------------------------------------------

    /** The bar's current value. */
    private float _value;


    // Public Methods
    // ------------------------------------------------------------

    /** Set the value for this bar. */
    public void SetValue(float value)
    {
        Value = value;
        UpdateLine();
    }


    // Unity Methods
    // ------------------------------------------------------------

    /** Enabling. */
    private void OnEnable()
    {
        UpdateLine();
    }

    /** Update the bar. */
    private void Update()
    {
        if (Value != _value)
            UpdateLine();
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Update the bar from current value. */
    private void UpdateLine()
    {
        var vo = GetComponent<VectorObject2D>();
        if (!vo)
            return;

        _value = Value;

        float f = 0;
        if (MaxValue > MinValue)
            f = (_value - MinValue) / (MaxValue - MinValue);

        var from = AngleRange.x;
        var to = Mathf.Lerp(AngleRange.x, AngleRange.y, f);
        var angle = Mathf.Abs(to - from);
        var segments = Mathf.Max(Mathf.RoundToInt(angle * SegmentsPerDegree), 4);

        var line = vo.vectorLine;
        line.active = _value > MinValue;
        if (!line.active)
            return;

        line.MakeArc(Vector2.zero, Radius, Radius, from, to);
        line.Draw();
    }

}
