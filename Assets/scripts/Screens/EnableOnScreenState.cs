using UnityEngine;
using System.Collections;
using Meg.Networking;

public class EnableOnScreenState : MonoBehaviour
{

    /** Screen state to match. */
    public screenData.State State;

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
        if (!serverUtils.IsReady() || !serverUtils.LocalPlayer)
            return;

        var state = serverUtils.LocalPlayer.ScreenState;
        var active = State.Matches(state);

        for (var i = 0; i < Targets.Length; i++)
            Targets[i].SetActive(active);

        for (var i = 0; i < Behaviours.Length; i++)
            Behaviours[i].enabled = active;
    }
}
