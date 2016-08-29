using UnityEngine;
using UnityEngine.Networking;
using Meg.Networking;

public class textValueFromConfig : widgetText
{
    [Header("Configuration")]
    public string ConfigKey = "";

    [Header("Updating")]
    public float updateTick = 0.25f;
    private float nextUpdate;

    private void Start()
        { Text = Configuration.Get(ConfigKey, Text); }

    private void Update()
    {
        // Check if it's time for the next update.
        if (Time.time < nextUpdate)
            return;
        nextUpdate = Time.time + updateTick;

        // Determine current value.
        Text = Configuration.Get(ConfigKey, Text);
    }

}
