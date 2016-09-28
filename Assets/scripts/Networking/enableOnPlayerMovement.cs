using UnityEngine;
using System.Collections;
using Meg.Networking;

/** Enable or disable game objects based on whether player vessel has active movements. */

public class enableOnPlayerMovement : MonoBehaviour
{
    /** Whether to disable instead of enable. */
    public bool Invert = false;

    /** Objects that should be enabled/disabled. */
    public GameObject[] Targets;

    private void Start()
    {
        Update();
    }

    private void Update()
    {
        var movement = serverUtils.GetPlayerVesselMovement();
        var active = movement != null;
        if (Invert)
            active = !active;

        for (var i = 0; i < Targets.Length; i++)
            if (Targets[i].activeSelf != active)
                Targets[i].SetActive(active);
    }

}
