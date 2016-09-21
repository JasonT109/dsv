using UnityEngine;
using Meg.Networking;
using System;
using System.Collections;
using System.Linq;

public class RemoteScreenStateButtons : MonoBehaviour
{
    /** The screen type to monitor. */
    public screenData.Type RemoteType;

    [Serializable]
    public struct StateButton
    {
        public screenData.Content Content;
        public buttonControl Button;
    }

    /** The button group that we're updating according to state. */
    public buttonGroup ButtonGroup;

    /** The set of buttons to toggle according to screen state. */
    public StateButton[] Buttons;


    /** Intialization. */
    private void Awake()
    {
        if (!ButtonGroup)
            ButtonGroup = GetComponent<buttonGroup>();
    }

    /** Enabling. */
    private void OnEnable()
    { UpdateButtons(); }

    /** Updating. */
    private void LateUpdate()
    { UpdateButtons(); }

    /** Disabling. */
    private void OnDisable()
    {
        // Deactivate all screen content buttons.
        foreach (var b in Buttons)
            b.Button.active = false;
    }

    /** Update buttons according to screen state. */
    private void UpdateButtons()
    {
        if (!serverUtils.IsReady())
            return;

        // Look for a matching remote screen of the target type.
        var player = serverUtils.GetPlayers()
            .FirstOrDefault(p => p.ScreenState.Type == RemoteType);

        // Check if a suitable screen was found.
        if (player == null)
            { ButtonGroup.toggleButtonOn(null); return; }

        // Toggle on buttons that match the remote state.
        var state = player.ScreenState;
        foreach (var b in Buttons)
            if ((state.Content == b.Content) && !b.Button.active)
                ToggleOn(b);
    }

    /** Activate a given screen state button. */
    private void ToggleOn(StateButton b)
    {
        ButtonGroup.toggleButtonOn(b.Button.gameObject);
    }

}
