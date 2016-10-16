using UnityEngine;
using System.Collections;
using Meg.Networking;

public class HideInDebugScreen : MonoBehaviour
{

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

        var active = !serverUtils.IsInDebugScreen();

        for (var i = 0; i < Targets.Length; i++)
            if (Targets[i].activeSelf != active)
                Targets[i].SetActive(active);

        for (var i = 0; i < Behaviours.Length; i++)
            Behaviours[i].enabled = active;
    }


}
