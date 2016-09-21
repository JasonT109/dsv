using UnityEngine;
using System.Collections;
using System.Linq;
using Meg.Networking;

public class WindowStateButton : MonoBehaviour
{
    /** ID of the window to show/hide.*/
    public screenData.WindowId WindowId;

    /** Whether to apply only to the local player's screen. */
    public bool Local;

    /** Associated button control. */
    private buttonControl _button;

    private void Start()
    {
        // Set up button association automatically.
        _button = GetComponent<buttonControl>();
        _button.onPress.AddListener(Apply);
    }

    private void Update()
    {
        _button.active = HasWindow();
        _button.toggleVisGroup();
    }

    /** Apply window visibility to matching screens. */
    public void Apply()
    {
        if (HasWindow() && _button.toggleType)
            RemoveWindow();
        else
            AddWindow();
    }

    private bool HasWindow()
    {
        if (Local && serverUtils.LocalPlayer)
            return serverUtils.LocalPlayer.HasWindow(WindowId);

        return serverUtils.GetPlayers().Any(p => p.HasWindow(WindowId));
    }

    private void AddWindow()
    {
        if (Local && serverUtils.LocalPlayer)
        {
            var player = serverUtils.LocalPlayer;
            serverUtils.PostAddWindow(player.netId, WindowId);
        }
        else
        {
            var players = serverUtils.GetPlayers()
                .Where(p => p.ScreenState.Type == WindowId.State.Type);

            foreach (var player in players)
                serverUtils.PostAddWindow(player.netId, WindowId);
        }
    }

    private void RemoveWindow()
    {
        if (Local && serverUtils.LocalPlayer)
        {
            var player = serverUtils.LocalPlayer;
            serverUtils.PostRemoveWindow(player.netId, WindowId);
        }
        else
        {
            var players = serverUtils.GetPlayers()
                .Where(p => p.ScreenState.Type == WindowId.State.Type);

            foreach (var player in players)
                serverUtils.PostRemoveWindow(player.netId, WindowId);
        }
    }
}
