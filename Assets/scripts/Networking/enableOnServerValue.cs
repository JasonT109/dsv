using UnityEngine;
using System.Collections;
using Meg.Networking;

/** Enable or disable game objects based on a server data value. */

public class enableOnServerValue : MonoBehaviour
{
    /** Server data value to monitor. */
    public string linkDataString = "depth";

    /** Threshold for enabling/disabling. */
    public float threshold = 50;

    /** Whether objects are disabled when value goes above threshold. */
    public bool thresholdGreaterThan = false;

    /** Objects that should be enabled/disabled. */
    public GameObject[] Targets;

    private void Start()
    {
        Update();
    }

    private void Update()
    {
        var value = serverUtils.GetServerData(linkDataString);
        var active = thresholdGreaterThan ? value > threshold : value <= threshold;
        for (var i = 0; i < Targets.Length; i++)
            Targets[i].SetActive(active);
    }

}
