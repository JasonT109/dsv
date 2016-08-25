using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Meg.Networking;

public class serverValueNoise : NetworkBehaviour
{
    /** The noise source. */
    public SmoothNoise NoiseSource;

    /** The server data value to inject noise into. */
    public string LinkDataString;

    /** Optional range to constrain values to. */
    public Vector2 OutputRange;
    
    /** Current base value for the server data. */
    private float _baseValue;

    /** Whether we're in the process of updating the server data value. */
    private bool _updating;

    /** Initialization. */
    private void Start()
    {
        NoiseSource.Start();
    }

    /** Handle startup on the server. */
    public override void OnStartServer()
    {
        if (serverUtils.ServerData)
            serverUtils.ServerData.ValueChangedEvent += OnValueChanged;

        _baseValue = serverUtils.GetServerData(LinkDataString);
    }

    /** Handle object being disabled. */
    [ServerCallback]
    private void OnDisable()
    {
        if (serverUtils.ServerData)
            serverUtils.ServerData.ValueChangedEvent -= OnValueChanged;
    }

    /** Handle a change in the server data base value. */
    [ServerCallback]
    private void OnValueChanged(string valueName, float newValue)
    {
        if (_updating || valueName != LinkDataString)
            return;

        _baseValue = serverUtils.GetServerData(LinkDataString);
    }

    /** Updating. */
    [ServerCallback]
    private void Update()
	{
        // Update noise.
        NoiseSource.Update();

        // Get the current noisy server data value.
        var value = _baseValue + NoiseSource.Value;

        // Optionally clamp the output value.
	    if (OutputRange.x != OutputRange.y)
	        value = Mathf.Clamp(value, OutputRange.x, OutputRange.y);

        // Write value back to the server data.
        // Indicate that we're updating the value so we don't set a new base value.
        _updating = true;
        serverUtils.SetServerData(LinkDataString, value);
        _updating = false;

	}

}
