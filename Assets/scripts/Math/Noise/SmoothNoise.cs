using UnityEngine;
using System.Collections;

public class SmoothNoise : MonoBehaviour
{

    // Configuration
    // ------------------------------------------------------------

    /** The distribution of noise values. */
    public Distribution Values;

    /** The distribution of the sampling interval. */
    public Distribution Interval;

    /** Amount of smoothing to apply to the value. */
    public float SmoothTime;


    // Properties
    // ------------------------------------------------------------
    
    /** Current (smoothed) noise value. */
    public float Value { get; private set; }


    // Members
    // ------------------------------------------------------------

    /** Current target value. */
    private float _target;

    /** Timestamp for next sample. */
    private float _nextSampleTime;

    /** Smoothing velocity. */
    private float _valueVelocity;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    protected virtual void Start()
    {
        Value = Values.Sample();
    }

    /** Updating. */
    protected virtual void Update()
    {
        if (Time.time >= _nextSampleTime)
            Sample();

        Value = Mathf.SmoothDamp(Value, _target, ref _valueVelocity, SmoothTime);
    }

    /** Pick a new target sampled value. */
    private void Sample()
    {
        _target = Values.Sample();
        _nextSampleTime = Time.time + Interval.Sample();
    }


}
