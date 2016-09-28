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

    /** Whether objects are enabled when value goes above threshold. */
    public bool thresholdGreaterThan = false;

    /** Whether objects are enabled if value equals threshold. */
    public bool thresholdEqualTo = false;

    /** Objects that should be enabled/disabled. */
    public GameObject[] Targets;

    /** Behaviours that should be enabled/disabled. */
    public Behaviour[] Behaviours;

    private void Start()
    {
        Update();
    }

    private void Update()
    {
        if (!serverUtils.IsReady())
            return;

        var value = serverUtils.GetServerData(linkDataString);
        var active = thresholdGreaterThan ? value > threshold : value <= threshold;
        if (thresholdEqualTo)
            active = Mathf.Approximately(value, threshold);

        for (var i = 0; i < Targets.Length; i++)
            if (Targets[i].activeSelf != active)
                Targets[i].SetActive(active);

        for (var i = 0; i < Behaviours.Length; i++)
            Behaviours[i].enabled = active;
    }

}
