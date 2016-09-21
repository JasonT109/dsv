using UnityEngine;
using System.Collections;
using Meg.Networking;

public class GliderScreenManager : MonoBehaviour
{

    private void Start()
    {
        // Set up initial screen scaling state.
        if (graphicsDisplaySettings.Instance)
            graphicsDisplaySettings.Instance.Initialize();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha1))
            PostScreenType(screenData.Type.GliderLeft);
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha2))
            PostScreenType(screenData.Type.GliderRight);
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha3))
            PostScreenType(screenData.Type.GliderRight);
    }

    private void PostScreenType(screenData.Type type)
    {
        if (!serverUtils.IsReady())
            return;

        var player = serverUtils.LocalPlayer;
        if (player && !Equals(player.ScreenState.Type, type))
            serverUtils.PostScreenStateType(player.netId, type);
    }

}
