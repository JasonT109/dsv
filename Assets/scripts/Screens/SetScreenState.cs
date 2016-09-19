using UnityEngine;
using Meg.Networking;
using UnityEngine.Networking;

public class SetScreenState : MonoBehaviour
{
    public screenData.State State;

    private void OnEnable()
    {
        if (serverUtils.IsReady())
            UpdateScreenState();
    }

    private void Update()
    {
        if (ClientScene.localPlayers.Count == 0)
            UpdateScreenState();
    }

    private void UpdateScreenState()
    {
        var player = serverUtils.LocalPlayer;
        if (player && !Equals(player.ScreenState, State))
            serverUtils.PostScreenState(player.netId, State);
        else if (!player)
            screenData.InitialState = State;
    }
	
}
