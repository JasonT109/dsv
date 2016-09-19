using UnityEngine;
using Meg.Networking;
using UnityEngine.Networking;

public class SetScreenType : MonoBehaviour
{
    public screenData.Type Type;

    private void OnEnable()
    {
        if (serverUtils.IsReady())
            UpdateScreenType();
    }

    private void Update()
    {
        if (ClientScene.localPlayers.Count == 0)
            UpdateScreenType();
    }

    private void UpdateScreenType()
    {
        if (!serverUtils.IsReady())
            return;

        var player = serverUtils.LocalPlayer;
        if (player && !Equals(player.ScreenState.Type, Type))
            serverUtils.PostScreenStateType(player.netId, Type);
        else if (!player)
            screenData.InitialState.Type = Type;
    }

}
