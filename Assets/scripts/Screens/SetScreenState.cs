using UnityEngine;
using Meg.Networking;

public class SetScreenState : MonoBehaviour
{
    public screenData.State State;

    private const float UpdateInterval = 1.0f;
    private float _nextUpdateTime;

    private void OnEnable()
        { UpdateScreenState(); }

    private void UpdateScreenState()
    {
        if (!serverUtils.IsReady())
            return;

        var player = serverUtils.LocalPlayer;
        if (player && !Equals(player.ScreenState, State))
            serverUtils.LocalPlayer.PostScreenState(State);
    }
	
}
