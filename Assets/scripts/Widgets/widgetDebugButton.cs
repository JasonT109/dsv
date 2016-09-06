using UnityEngine;
using System.Collections;
using Meg.Networking;

/** 
 * Secret button that opens the debug screen.
 * Requires multiple presses on client machines (so it isn't easy to accidentally open).
 */

public class widgetDebugButton : Singleton<widgetDebugButton>
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

    /** Whether button closes debug screen when it's open. */
    public bool canToggleOff;

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

    /** Whether debug screen is active. */
    public bool IsDebug
        { get { return debugVisGroup.activeInHierarchy; } }


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
        if (navButtonGroup)
            foreach (var button in navButtonGroup.buttons)
                button.GetComponent<buttonControl>().onPressed += OnNavButtonPressed;
    }

    /** Updating. */
    private void Update()
    {
        // Check that server is ready.
        if (!serverUtils.IsReady())
            return;
            
        // Reset the press counter after a period of inactivity.
        if (_presses > 0 && Time.time >= _pressResetTime)
            _presses = 0;

        // Keyboard shortcut for the debug menu.
        if (Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
            Toggle();

        // Deactivate when screen has some assigned content.
        var player = serverUtils.LocalPlayer;
        if (_presses == 0 && player && player.ScreenState.Content != screenData.Content.Debug)
            Deactivate();
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Activate the debug screen. */
    public void Activate()
    {
        if (navButtonGroup)
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

            if (navButtonGroup)
                navButtonGroup.toggleButtons(navButtonGroup.lastButton);
        }
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Handle the debug button being pressed. */
    private void OnDebugButtonPressed()
    {
        _presses++;
        _pressResetTime = Time.time + pressesTimeout;

        if (debugVisGroup.activeSelf)
        {
            if (canToggleOff && _presses >= pressesToActivate)
                Toggle();
        }
        else if (_presses >= pressesToActivate)
            Activate();
    }

    /** Handle a screen navigation button being pressed. */
    private void OnNavButtonPressed()
    {
        debugVisGroup.SetActive(false);
        _presses = 0;
    }

}
