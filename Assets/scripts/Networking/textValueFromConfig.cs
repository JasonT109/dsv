using UnityEngine;
using UnityEngine.Networking;
using Meg.Networking;

public class textValueFromConfig : widgetText
{
    [Header("Configuration")]
    public string ConfigKey = "";
    public string ConfigDataOverrideParam = "";

    [Header("Updating")]
    public float updateTick = 0.25f;
    private float nextUpdate;

    private string _initialText;

    private void Start()
    {
        _initialText = Text;
        Text = Configuration.Get(ConfigKey, Text);
    }

    private void Update()
    {
        // Check if it's time for the next update.
        if (Time.time < nextUpdate)
            return;
        nextUpdate = Time.time + updateTick;

        // Determine current value.
        var text = Configuration.Get(ConfigKey, Text);

        // Optionally check if we should apply the config data.
        if (!string.IsNullOrEmpty(ConfigDataOverrideParam))
        {
            var applyConfig = serverUtils.GetServerBool(ConfigDataOverrideParam, true);
            text = applyConfig ? text : _initialText;
        }

        // Update the current text value.
        Text = text;
    }

}
