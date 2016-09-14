using UnityEngine;
using Meg.Networking;

public class SetScreenType : MonoBehaviour
{
    public screenData.Type Type;

    private void OnEnable()
        { UpdateScreenType(); }

    private void UpdateScreenType()
    {
        if (!serverUtils.IsReady())
            return;

        var player = serverUtils.LocalPlayer;
        if (player && !Equals(player.ScreenState.Type, Type))
            serverUtils.PostScreenStateType(player.netId, Type);
    }
	
}
