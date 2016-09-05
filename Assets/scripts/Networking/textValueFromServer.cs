using UnityEngine;
using UnityEngine.Networking;
using Meg.Networking;

public class textValueFromServer : widgetText
{
    [Header("Server Data")]
    public string linkDataString = "depth";
    public string configDataKey = "";

    [Header("Output Value")]
    public bool useOutputCurve;
    public AnimationCurve outputCurve;
    public bool addNoise = false;
    public SmoothNoise noise;
    public float noiseThreshold = 0;
    public float scale = 1;

    [Header("Formatting")]
    public string format = "";
    public string configFormatKey = "";
    public string prefix = "";
    public bool upperCase;
    public ValueRange[] Ranges;

    [Header("Updating")]
    public float updateTick = 0.2f;
    private float nextUpdate = 0;

    [System.Serializable]
    public struct ValueRange
    {
        public string Id;
        public Vector2 Range;
        public Color Color;
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(configDataKey))
            linkDataString = Configuration.Get(configDataKey, linkDataString);

        if (!string.IsNullOrEmpty(configFormatKey))
            format = Configuration.Get(configFormatKey, format);

        if (addNoise)
            noise.Start();

        Update();
        nextUpdate = Time.time + updateTick;
    }

    private void Update()
    {
        // Always update noise function.
        if (addNoise)
            noise.Update();

        // Check if it's time for the next update.
        if (Time.time < nextUpdate)
            return;
        nextUpdate = Time.time + updateTick;

        // Determine current value.
        var value = serverUtils.GetServerData(linkDataString);

        // Apply remapping curve, if specified.
        if (useOutputCurve)
            value = outputCurve.Evaluate(value);

        // Add noise if desired.
        if (addNoise && value > noiseThreshold)
            value += noise.Value;

        // Apply scaling (if specified).
        value = value * scale;

        // Apply baseline value formatting.
        if (string.IsNullOrEmpty(format))
            Text = serverUtils.GetServerDataAsText(linkDataString);
        else
            Text = string.Format(format, value);

        // Convert to uppercase if desired.
        if (upperCase)
            Text = Text.ToUpper();

        // Apply custom formatting for value ranges (if any).
        for (var i = 0; i < Ranges.Length; i++)
        {
            var range = Ranges[i];
            if (value < range.Range.x || value > range.Range.y)
                continue;

            Color = range.Color;

            if (!string.IsNullOrEmpty(range.Id))
                Text = string.Format(range.Id, value);
        }

        // Apply prefix (if any).
        if (!string.IsNullOrEmpty(prefix))
            Text = prefix + Text;
    }

}
