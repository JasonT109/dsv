using UnityEngine;
using System.Collections;
using Meg.Networking;
using UnityEngine.Networking;

public class SonarData : NetworkBehaviour 
{

    // Enumerations
    // ------------------------------------------------------------

    /** The types of available sonar. */
    public enum Type
    {
        None,
        ShortRange,
        LongRange
    };


    // Configuration
    // ------------------------------------------------------------

    [Header("Configuration")]

    /** Configuration for short range sonar. */
    public Config ShortRangeConfig;

    /** Configuration for short range sonar. */
    public Config LongRangeConfig;


    // Synchronization
    // ------------------------------------------------------------

    [Header("Synchronization")]

    /** Current range setting for front-scanning (short-range) sonar. */
    [SyncVar]
    public float ShortRange = 30;

    /** Current frequency setting for front-scanning (short-range) sonar. */
    [SyncVar]
    public float ShortFrequency = 455;

    /** Current gain setting for front-scanning (short-range) sonar. */
    [SyncVar]
    public float ShortGain = 72;

    /** Current sensitivity setting for front-scanning (short-range) sonar. */
    [SyncVar]
    public float ShortSensitivity = 78;

    /** Current range setting for 360� (long-range) sonar. */
    [SyncVar]
    public float LongRange = 6000;

    /** Current frequency setting for 360� (long-range) sonar. */
    [SyncVar]
    public float LongFrequency = 200;

    /** Current gain setting for 360� (long-range) sonar. */
    [SyncVar]
    public float LongGain = 72;

    /** Current sensitivity setting for 360� (long-range) sonar. */
    [SyncVar]
    public float LongSensitivity = 72;

    [SyncVar]
    public float MegSpeed;

    [SyncVar]
    public float MegTurnSpeed;

    [SyncVar]
    public float DefaultScale;

    [SyncVar]
    public int MaxWildlife = 0;


    // Public Methods
    // ------------------------------------------------------------

    public float GetScaleSpeed()
        { return MegSpeed * ShortRangeRatio; }

    public float GetScaleTurnSpeed()
        { return MegTurnSpeed; }

    public float GetScale()
        { return DefaultScale * ShortRangeRatio; }

    /** Return a sonar configuration based on type. */
    public Config GetConfigForType(Type type)
    {
        switch (type)
        {
            case Type.ShortRange:
                return ShortRangeConfig;
            case Type.LongRange:
                return LongRangeConfig;
            default:
                return ShortRangeConfig;
        }
    }


    // Structures
    // ------------------------------------------------------------

    /** Configuration for a sonar type. */
    [System.Serializable]
    public class Config
    {
        public Type Type = Type.ShortRange;
        public string LinkDataPrefix = "sonarshort";
        public float RangeIncrement = 15;
        public float FrequencyIncrement = 5;
        public float GainIncrement = 5;
        public float SensitivityIncrement = 5;

        public float Range
            { get { return GetServerData("range"); } }

        public float Frequency
            { get { return GetServerData("frequency"); } }

        public float Gain
            { get { return GetServerData("gain"); } }

        public float Sensitivity
            { get { return GetServerData("sensitivity"); } }

        public float MinRange
            { get { return GetMinValue("range"); } }

        public float MaxRange
            { get { return GetMaxValue("range"); } }

        public serverUtils.ParameterInfo GetInfo(string suffix)
            { return serverUtils.GetServerDataInfo(LinkDataPrefix + suffix); }

        public float GetMinValue(string suffix)
            { return GetInfo(suffix).minValue; }

        public float GetMaxValue(string suffix)
            { return GetInfo(suffix).maxValue; }

        public float GetServerData(string suffix)
            { return serverUtils.GetServerData(LinkDataPrefix + suffix); }

        public void PostServerData(string suffix, float value)
            { serverUtils.PostServerData(LinkDataPrefix + suffix, value); }

        public void AddServerData(string suffix, float increment)
        {
            var min = GetMinValue(suffix);
            var max = GetMaxValue(suffix);
            var value = Mathf.Clamp(GetServerData(suffix) + increment, min, max);
            PostServerData(suffix, value);
        }
    }


    // Private Methods
    // ------------------------------------------------------------

    private float ShortRangeRatio
        { get { return ShortRange <= 0 ? 1 : 30.0f / ShortRange; } }

}
