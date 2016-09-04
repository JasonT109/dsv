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

    /** The current noise value. */
    [SyncVar]
    public float Value;


    /** Whether we've registered with the manager. */
    private bool _registered;

    /** Disabling. */
    private void OnDisable()
    {
        if (serverUtils.NoiseData)
            serverUtils.NoiseData.Unregister(this);
    }

    /** Initialization. */
    private void Start()
    {
        NoiseSource.Start();
    }

    /** Updating. */
    [ServerCallback]
    private void Update()
	{
        // Register noise component with the manager.
        if (!_registered && !string.IsNullOrEmpty(LinkDataString))
        {
            serverUtils.NoiseData.Register(this);
            _registered = true;
        }

        // Update noise source.
        NoiseSource.Update();

        // Update synchronized noise value.
        Value = NoiseSource.Value;
    }

}
