using UnityEngine;
using System.Collections;
using System.Linq;
using Meg.Networking;

public class EnableOnScreenStates : MonoBehaviour
{

    /** Screen state to match. */
    public screenData.State[] States;

    /** Objects that should be enabled/disabled. */
    public GameObject[] Targets;

    /** Behaviours that should be enabled/disabled. */
    public Behaviour[] Behaviours;

    private void Start()
    {
        LateUpdate();
    }

    private void LateUpdate()
    {
        if (!serverUtils.IsReady() || !serverUtils.LocalPlayer)
            return;

        var state = serverUtils.LocalPlayer.ScreenState;
        var active = States.Any(s => s.Matches(state));

        for (var i = 0; i < Targets.Length; i++)
            Targets[i].SetActive(active);

        for (var i = 0; i < Behaviours.Length; i++)
            Behaviours[i].enabled = active;
    }
}
