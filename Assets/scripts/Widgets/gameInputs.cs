using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;
using Rewired;

public class gameInputs : NetworkBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** Interval for sending input updates to the server (seconds). */
    private const float ServerSendInterval = 0.05f;


    // Properties
    // ------------------------------------------------------------

    [Header("Configuration")]

    /** Throttle response curve. */
    public AnimationCurve ThrottleResponse;


    // Synchronization
    // ------------------------------------------------------------

    [Header("Synchronization")]

    /** Current input state on this player. */
    [SyncVar]
    public float output = 0.0f;
    [SyncVar]
    public float outputX = 0.0f;
    [SyncVar]
    public float outputY = 0.0f;
    [SyncVar]
    public float outputX2 = 0.0f;
    [SyncVar]
    public float outputY2 = 0.0f;

    /** Whether this player is regarded as a 'pilot' (has an active joystick). */
    [SyncVar]
    public bool pilot = false;

    /** which GLIDER screen this client is controlling (left = 2, middle = 1, right = 0) */
    [SyncVar]
    public int glScreenID = 0;

    /** what GLIDER screen content this client is viewing */
    [SyncVar]
    public int activeScreen = 0;

    [SyncVar]
    public bool map3dState;
    private bool prev3dState;

    [SyncVar]
    public bool mapCentreState;
    private bool prevCentreState;

    [SyncVar]
    public bool mapLabelState;
    private bool prevLabelState;


    // Members
    // ------------------------------------------------------------

    /** The player's input source. */
    private Rewired.Player _input;

    /** Timestamp for sending next input to server. */
    private float _nextSendTime;


    // Unity Methods
    // ------------------------------------------------------------

    /** initialisation */
    void Start()
    {
        glScreenManager.EnsureInstanceExists();
    }

    /** Called on the client who has authority over this player. */
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // Retrieve the player input source from Rewired.
        // Rewired is configured with a single input player per client.
        _input = Rewired.ReInput.players.GetPlayer(0);

        // Print out some input configuration info to the console.
        LogInputConfiguration();
    }

    /** Updating */
    private void Update()
    {
        // Only update inputs for this client's local player.
        if (!isLocalPlayer || !localPlayerAuthority)
            return;

        // If we have changed the current screen ID, update it with the server.
        if (glScreenManager.HasInstance)
            UpdateGliderScreenState();

        // Update input if this is the local player.
        if (_input != null)
            UpdateInput();
    }


    // Server Commands
    // ------------------------------------------------------------

    /** Updates this player's syncvars from the current input state. */
    [Command]
    void CmdChangeInput(bool state, float xAxis1, float yAxis1, float zAxis1, float xAxis2, float yAxis2)
    {
        output = zAxis1;
        outputX = xAxis1;
        outputY = yAxis1;
        outputX2 = xAxis2;
        outputY2 = yAxis2;

        if (state)
        {
            pilot = true;
        }
        else
        {
            pilot = false;
        }
    }

    [Command]
    void CmdChangeScreenContent(int scContent)
    {
        activeScreen = scContent;
    }

    [Command]
    void CmdChangeScreenID(int scID)
    {
        glScreenID = scID;
    }

    [Command]
    void CmdSetMap3dState(bool mapState)
    {
        map3dState = mapState;
        //Debug.Log("Changing map 3d.");
    }

    [Command]
    void CmdSetMapCenterState(bool mapState)
    {
        mapCentreState = mapState;
        //Debug.Log("Centering map.");
    }

    [Command]
    void CmdSetMapLabelState(bool mapState)
    {
        mapLabelState = mapState;
        //Debug.Log("Toggling map labels.");
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Update input from the local player, and send it to the server. */
    private void UpdateInput()
    {
        try
        {
            // Check if there are any joysticks attached to this client.
            if (_input == null || _input.controllers.joystickCount <= 0)
                return;

            // If so, sample the current input state.
            var z1 = _input.GetAxis("Throttle");
            var x1 = _input.GetAxis("Horizontal");
            var y1 = _input.GetAxis("Vertical");
            var x2 = _input.GetAxis("X2");
            var y2 = _input.GetAxis("Y2");

            // Apply throttle response curve to determine final output.
            z1 = ThrottleResponse.Evaluate(z1);

            // Check if it's time to send inputs to the server.
            if (Time.realtimeSinceStartup < _nextSendTime)
                return;

            // Send data to the server.
            CmdChangeInput(true, x1, y1, z1, x2, y2);

            // Schedule next server send.
            _nextSendTime = Time.realtimeSinceStartup + ServerSendInterval;

        }
        catch (Exception ex)
        {
            // Keep on going even if joystick input fails.
            Debug.LogWarning("Failed to get input data: " + ex);
        }
    }

    /** Outputs debugging information about the Rewired input setup. */
    private void LogInputConfiguration()
    {
        // Log assigned Joystick information for all joysticks regardless of whether or not they've been assigned
        Debug.Log("Rewired found " + ReInput.controllers.joystickCount + " joysticks attached.");
        for (int i = 0; i < ReInput.controllers.joystickCount; i++)
        {
            Joystick j = ReInput.controllers.Joysticks[i];
            Debug.Log(
                "[" + i + "] Joystick: " + j.name + "\n" +
                "Hardware Name: " + j.hardwareName + "\n" +
                "Is Recognized: " + (j.hardwareTypeGuid != System.Guid.Empty ? "Yes" : "No") + "\n" +
                "Is Assigned: " + (ReInput.controllers.IsControllerAssigned(j.type, j) ? "Yes" : "No")
            );
        }

        // Log assigned Joystick information for each Player
        foreach (var p in ReInput.players.Players)
        {
            Debug.Log("PlayerId = " + p.id + " is assigned " + p.controllers.joystickCount + " joysticks.");

            // Log information for each Joystick assigned to this Player
            foreach (var j in p.controllers.Joysticks)
            {
                Debug.Log(
                  "Joystick: " + j.name + "\n" +
                  "Is Recognized: " + (j.hardwareTypeGuid != System.Guid.Empty ? "Yes" : "No")
                );

                // Log information for each Controller Map for this Joystick
                var mapsForJoystick = p.controllers.maps.GetMaps(j.type, j.id);
                foreach (var map in mapsForJoystick)
                {
                    Debug.Log("Controller Map:\n" +
                        "Category = " +
                        ReInput.mapping.GetMapCategory(map.categoryId).name + "\n" +
                        "Layout = " +
                        ReInput.mapping.GetJoystickLayout(map.layoutId).name + "\n" +
                        "enabled = " + map.enabled
                    );
                    foreach (var aem in map.GetElementMaps())
                    {
                        var action = ReInput.mapping.GetAction(aem.actionId);
                        if (action == null) continue; // invalid Action
                        Debug.Log("Action \"" + action.name + "\" is bound to \"" +
                            aem.elementIdentifierName
                        );
                    }
                }
            }
        }
    }

    /** Update glider screen state. */
    private void UpdateGliderScreenState()
    {
        if (glScreenManager.Instance.hasChanged)
        {
            CmdChangeScreenID(glScreenManager.Instance.screenID);
            glScreenManager.Instance.hasChanged = false;

            /** If we have changed the right screen content update it with the server */
            if (glScreenManager.Instance.screenID == 0)
            {
                CmdChangeScreenContent(glScreenManager.Instance.rightScreenID);
            }

            glScreenManager.Instance.hasChanged = false;
        }

        //if map 3d state has changed
        if (glScreenManager.Instance.map3dButton.pressed != prev3dState)
        {
            prev3dState = glScreenManager.Instance.map3dButton.pressed;
            CmdSetMap3dState(prev3dState);
        }

        //if centre button pressed
        if (glScreenManager.Instance.mapCenterButton.pressed != prevCentreState)
        {
            prevCentreState = glScreenManager.Instance.mapCenterButton.pressed;
            CmdSetMapCenterState(prevCentreState);
        }

        //labels toggled
        if (glScreenManager.Instance.mapLabelButton.pressed != prevLabelState)
        {
            prevLabelState = glScreenManager.Instance.mapLabelButton.pressed;
            CmdSetMapLabelState(prevLabelState);
        }
    }


}
