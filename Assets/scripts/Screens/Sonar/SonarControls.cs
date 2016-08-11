using UnityEngine;
using System.Collections;
using Meg.Networking;

public class SonarControls : MonoBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** Interval between successive range increments. */
    public const float RangeIncrementInterval = 0.1f;

    /** Interval between successive frequency increments. */
    public const float FrequencyIncrementInterval = 0.05f;

    /** Interval between successive gain increments. */
    public const float GainIncrementInterval = 0.05f;

    /** Interval between successive sensitivity increments. */
    public const float SensitivityIncrementInterval = 0.05f;


    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Data overrides for short range. */
    public linkDataOverrides ShortRangeOverrides;

    /** Data overrides for long range. */
    public linkDataOverrides LongRangeOverrides;


    [Header("Configuration")]

    /** The active sonar type . */
    public SonarData.SonarType Type = SonarData.SonarType.ShortRange;

    /** The active sonar configuration. */
    public SonarData.Config Config
        { get { return serverUtils.SonarData.GetConfigForType(Type); } }


    // Public Methods
    // ------------------------------------------------------------

    /** Select short-range sonar. */
    public void SelectShortRange()
    {
        Type = SonarData.SonarType.ShortRange;
        ShortRangeOverrides.Apply();
    }

    /** Select long-range sonar. */
    public void SelectLongRange()
    {
        Type = SonarData.SonarType.LongRange;
        LongRangeOverrides.Apply();
    }

    /** Increase sonar range. */
    public void IncreaseRange(bool value)
        { UpdateValue("range", Config.RangeIncrement, RangeIncrementInterval, value); }

    /** Decrease sonar range. */
    public void DecreaseRange(bool value)
        { UpdateValue("range", -Config.RangeIncrement, RangeIncrementInterval, value); }

    /** Increase sonar frequency. */
    public void IncreaseFrequency(bool value)
        { UpdateValue("frequency", Config.FrequencyIncrement, FrequencyIncrementInterval, value); }

    /** Decrease sonar frequency. */
    public void DecreaseFrequency(bool value)
        { UpdateValue("frequency", -Config.FrequencyIncrement, FrequencyIncrementInterval, value); }

    /** Increase sonar gain. */
    public void IncreaseGain(bool value)
        { UpdateValue("gain", Config.GainIncrement, GainIncrementInterval, value); }

    /** Decrease sonar gain. */
    public void DecreaseGain(bool value)
        { UpdateValue("gain", -Config.GainIncrement, GainIncrementInterval, value); }

    /** Increase sonar sensitivity. */
    public void IncreaseSensitivity(bool value)
        { UpdateValue("sensitivity", Config.SensitivityIncrement, SensitivityIncrementInterval, value); }

    /** Decrease sonar sensitivity. */
    public void DecreaseSensitivity(bool value)
        { UpdateValue("sensitivity", -Config.SensitivityIncrement, SensitivityIncrementInterval, value); }


    // Private Methods
    // ------------------------------------------------------------

    private void UpdateValue(string suffix, float increment, float interval, bool on)
    {
        StopAllCoroutines();
        if (on)
            StartCoroutine(UpdateValueRoutine(suffix, increment, interval));
    }

    private IEnumerator UpdateValueRoutine(string suffix, float increment, float interval)
    {
        var wait = new WaitForSeconds(interval);
        while (gameObject.activeInHierarchy)
        {
            Config.AddServerData(suffix, increment);
            yield return wait;
        }
    }


}
