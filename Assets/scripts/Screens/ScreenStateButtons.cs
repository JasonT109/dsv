using UnityEngine;
using Meg.Networking;
using System;
using System.Collections;

public class ScreenStateButtons : MonoBehaviour
{

    [Serializable]
    public struct StateButton
    {
        public screenData.State State;
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

        var state = serverUtils.LocalScreenState;
        foreach (var b in Buttons)
            if (Equals(b.State, state) && !b.Button.active)
                ToggleOn(b);
	}

    /** Activate a given screen state button. */
    private void ToggleOn(StateButton b)
    {
        ButtonGroup.toggleButtonOn(b.Button.gameObject);
    }

}
