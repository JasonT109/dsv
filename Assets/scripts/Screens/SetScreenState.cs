using UnityEngine;
using Meg.Networking;

public class SetScreenState : MonoBehaviour
{
    public screenData.State State;

    private void OnEnable()
        { UpdateScreenState(); }

    private void UpdateScreenState()
    {
        if (!serverUtils.IsReady())
            return;

        var player = serverUtils.LocalPlayer;
        if (player && !Equals(player.ScreenState, State))
            serverUtils.PostScreenState(player.netId, State);
    }
	
}
