using UnityEngine;
using System.Collections;
using Meg.Networking;

/** 
 * Secret button that opens the debug screen.
 * Requires multiple presses on client machines (so it isn't easy to accidentally open).
 */

public class widgetDebugButton : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** The button group used to navigate between screens. */
    public buttonGroup navButtonGroup;

    /** The debug button. */
    public buttonControl debugButton;

    /** The debug screen. */
    public GameObject debugVisGroup;


    [Header("Configuration")]

    /** Number of presses needed to activate the debug screen on a host machine. */
    public int pressesToActivateOnHost = 1;

    /** Number of presses needed to activate the debug screen on a client machine. */
    public int pressesToActivateOnClient = 10;

    /** Timeout for resetting recent press count back to zero (seconds). */
    public float pressesTimeout = 5;

    /** Number of presses needed to activate the debug screen on the local machine. */
    private int pressesToActivate
    {
        get
        {
            var serverData = serverUtils.ServerData;
            if (serverData && serverData.isServer)
                return pressesToActivateOnHost;

            return pressesToActivateOnClient;
        }
    }


    // Members
    // ------------------------------------------------------------

    /** Number of recent presses on the debug button. */
    private int _presses;

    /** Timestamp at which to reset the press count. */
    private float _pressResetTime;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        if (!debugButton)
            debugButton = GetComponent<buttonControl>();

        debugButton.onPressed += OnDebugButtonPressed;

        // Listen for regular screen navigation button presses.
        foreach (var button in navButtonGroup.buttons)
            button.GetComponent<buttonControl>().onPressed += OnNavButtonPressed;
    }

    /** Updating. */
    private void Update()
    {
        // Reset the press counter after a period of inactivity.
        if (Time.time >= _pressResetTime)
            _presses = 0;

        // Keyboard shortcut for the debug menu.
        if (Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
            Toggle();
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Activate the debug screen. */
    public void Activate()
    {
        navButtonGroup.toggleButtons(debugButton.gameObject);
        debugVisGroup.SetActive(true);
        _presses = 0;
    }

    /** Deactivate the debug screen. */
    public void Deactivate()
    {
        debugVisGroup.SetActive(false);
        _presses = 0;
    }

    /** Toggle the debug screen. */
    public void Toggle()
    {
        if (!debugVisGroup.activeSelf)
            Activate();
        else
        {
            Deactivate();
            navButtonGroup.toggleButtons(navButtonGroup.lastButton);
        }
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Handle the debug button being pressed. */
    private void OnDebugButtonPressed()
    {
        if (debugVisGroup.activeSelf)
            return;

        _presses++;
        _pressResetTime = Time.time + _pressResetTime;
        if (_presses < pressesToActivate)
            return;

        navButtonGroup.toggleButtons(debugButton.gameObject);
        debugVisGroup.SetActive(true);
        _presses = 0;
    }

    /** Handle a screen navigation button being pressed. */
    private void OnNavButtonPressed()
    {
        debugVisGroup.SetActive(false);
        _presses = 0;
    }

}
