using UnityEngine;
using System.Collections;
using Meg.Networking;

public class EvacScreenManager : MonoBehaviour
{

	void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha1))
            PostScreenType(screenData.Type.EvacLeft);
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha2))
            PostScreenType(screenData.Type.EvacMid);
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha3))
            PostScreenType(screenData.Type.EvacRight);
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha4))
            PostScreenType(screenData.Type.EvacMap);
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
