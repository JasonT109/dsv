using System;
using Meg.DCC;
using UnityEngine;
using UnityEngine.Networking;
using Meg.EventSystem;
using Meg.Networking;
using Meg.SonarEvent;

public class serverPlayer : NetworkBehaviour
{

    // Synchronization
    // ------------------------------------------------------------

    [SyncVar]
    public screenData.State ScreenState;


    // Public Properties
    // ------------------------------------------------------------

    /** Return a unique id for this player. */
    public string Id
        { get { return Configuration.Instance.CurrentId + "-" + netId.Value; } }

    /** Return the game input for this player. */
    public gameInputs GameInputs
        { get { return GetComponent<gameInputs>(); } }

    /** Return the input source for this player (if local). */
    public Rewired.Player Input
        { get { return GameInputs.Input; } }
    

    // Private Properties
    // ------------------------------------------------------------

    /** The sonar event manager. */
    private megSonarEventManager Sonar
        { get { return megEventManager.Instance.GetSonarEventManager(); } }

    /** The map camera event manager. */
    private megMapCameraEventManager MapCamera
        { get { return megEventManager.Instance.GetMapCameraEventManager(); } }


    // Public Methods
    // ------------------------------------------------------------

    /** Set a numeric data value on the server. */
    public void PostServerData(string key, float value, bool add)
    {
        if (isServer)
            serverUtils.SetServerData(key, value, add);
        else if (isClient)
            CmdPostServerFloat(key, value, add);
    }

    /** Set a string data value on the server. */
    public void PostServerData(string key, string value)
    {
        if (isServer)
            serverUtils.SetServerData(key, value);
        else if (isClient)
            CmdPostServerString(key, value);
    }

    /** Post a physic impact event. */
    public void PostImpact(Vector3 impactVector)
    {
        if (isServer)
            serverUtils.ServerData.RpcImpact(impactVector);
        else if (isClient)
            CmdPostImpact(impactVector);
    }

    /** Post a sonar event by name. */
    public void PostSonarEvent(megEventSonar sonar)
    {
        if (isServer)
            ServerSonarEvent(sonar);
        else if (isClient)
            CmdPostSonarEvent(sonar.eventName, sonar.waypoints, sonar.destroyOnEnd);
    }

    /** Post a sonar cleanup command. */
    public void PostSonarClear()
    {
        if (isServer)
            ServerSonarClear();
        else if (isClient)
            CmdPostSonarClear();
    }

    /** Add a popup (works on both clients and host). */
    public void PostAddPopup(popupData.Popup popup)
    {
        if (isServer)
            ServerAddPopup(popup);
        else if (isClient)
            CmdAddPopup(popup);
    }

    /** Toggle a popup on or off. */
    public void PostTogglePopup(popupData.Popup popup)
    {
        if (isServer)
            ServerTogglePopup(popup);
        else if (isClient)
            CmdTogglePopup(popup);
    }

    /** Remove a popup (works on both clients and host). */
    public void PostRemovePopup(popupData.Popup popup)
    {
        if (isServer)
            ServerRemovePopup(popup);
        else if (isClient)
            CmdRemovePopup(popup);
    }

    /** Clear all popups (works on both clients and host). */
    public void PostClearPopups()
    {
        if (isServer)
            ServerClearPopups();
        else if (isClient)
            CmdClearPopups();
    }

    /** Post a custom camera event by name. */
    public void PostMapCameraEvent(string eventName)
    {
        if (isServer)
            ServerTriggerMapCameraEvent(eventName);
        else
            CmdTriggerMapCameraEvent(eventName);
    }

    /** Post a custom camera event by supplying the target state. */
    public void PostMapCameraState(megMapCameraEventManager.State state)
    {
        if (isServer)
            ServerTriggerMapCameraState(state);
        else
            CmdTriggerMapCameraState(state);
    }

    /** Post vessel position to the server. */
    public void PostVesselPosition(int id, Vector3 position)
    {
        if (isServer)
            ServerSetVesselPosition(id, position);
        else
            CmdSetVesselPosition(id, position);
    }

    /** Post vessel movement type to the server. */
    public void PostVesselMovementType(int id, string type)
    {
        if (isServer)
            ServerSetVesselMovementType(id, type);
        else
            CmdSetVesselMovementType(id, type);
    }

    /** Post vessel's name to the server. */
    public void PostVesselName(int id, string value)
    {
        if (isServer)
            ServerSetVesselName(id, value);
        else
            CmdSetVesselName(id, value);
    }

    /** Post vessel's depth to the server. */
    public void PostVesselDepth(int id, float depth)
    {
        if (isServer)
            ServerSetVesselDepth(id, depth);
        else
            CmdSetVesselDepth(id, depth);
    }

    /** Post vessel's icon to the server. */
    public void PostVesselIcon(int id, vesselData.Icon icon)
    {
        if (isServer)
            ServerSetVesselIcon(id, icon);
        else
            CmdSetVesselIcon(id, icon);
    }

    /** Post vessel map visibility to the server. */
    public void PostVesselOnMap(int id, bool value)
    {
        if (isServer)
            ServerSetVesselOnMap(id, value);
        else
            CmdSetVesselOnMap(id, value);
    }

    /** Post vessel sonar visibility to the server. */
    public void PostVesselOnSonar(int id, bool value)
    {
        if (isServer)
            ServerSetVesselOnSonar(id, value);
        else
            CmdSetVesselOnSonar(id, value);
    }

    /** Post vessel movements state to the server. */
    public void PostVesselMovementState(JSONObject json)
    {
        if (isServer)
            ServerSetVesselMovementState(json);
        else
            CmdSetVesselMovementState(json.Print(true));
    }

    /** Add a vessel to the simulation. */
    public void PostAddVessel(vesselData.Vessel vessel)
    {
        if (isServer)
            serverUtils.VesselData.AddVessel(vessel);
        else if (isClient)
            CmdAddVessel(vessel);
    }

    /** Remove the last vessel from the simulation. */
    public void PostRemoveLastVessel()
    {
        if (isServer)
            serverUtils.VesselData.RemoveLastVessel();
        else if (isClient)
            CmdRemoveLastVessel();
    }

    /** Clear extra vessels from the simulation. */
    public void PostClearExtraVessels()
    {
        if (isServer)
            serverUtils.VesselData.ClearExtraVessels();
        else if (isClient)
            CmdClearExtraVessels();
    }

    /** Post glider screen id for the given player. */
    public void PostGliderScreenId(NetworkInstanceId playerId, int screenId)
    {
        if (isServer)
            ServerSetGliderScreenId(playerId, screenId);
        else
            CmdSetGliderScreenId(playerId, screenId);
    }

    /** Post glider screen content id for the given player. */
    public void PostGliderScreenContentId(NetworkInstanceId playerId, int contentId)
    {
        if (isServer)
            ServerSetGliderScreenContentId(playerId, contentId);
        else
            CmdSetGliderScreenContentId(playerId, contentId);
    }

    /** Post screen state for this player. */
    public void PostScreenState(NetworkInstanceId playerId, screenData.State state)
    {
        if (isServer)
            ServerSetScreenState(playerId, state);
        else
        {
            var player = serverUtils.GetPlayer(playerId);
            if (player)
                player.ScreenState = state;

            CmdSetScreenState(playerId, state);
        }
    }

    /** Post screen state for this player. */
    public void PostScreenStateContent(NetworkInstanceId playerId, screenData.Content content)
    {
        if (isServer)
            ServerSetScreenStateContent(playerId, content);
        else
        {
            var player = serverUtils.GetPlayer(playerId);
            if (player)
                player.ScreenState.Content = content;

            CmdSetScreenStateContent(playerId, content);
        }
    }

    /** Post content for the specified DCC screen. */
    public void PostScreenContent(DCCScreenID._screenID id, DCCWindow.contentID value, int stationId)
    {
        if (isServer)
            serverUtils.DCCScreenData.SetScreenContent(id, value, stationId);
        else if (isClient)
            CmdSetScreenContent(id, value, stationId);
    }

    /** Post content for the specified quad screen. */
    public void PostQuadContent(DCCScreenContentPositions.positionID id, DCCWindow.contentID value, int stationId)
    {
        if (isServer)
            serverUtils.DCCScreenData.SetQuadContent(id, value, stationId);
        else if (isClient)
            CmdSetQuadContent(id, value, stationId);
    }

    /** Post cycle state for the specified quad screen. */
    public void PostQuadCycle(int value, int stationId)
    {
        if (isServer)
            serverUtils.DCCScreenData.SetQuadCycle(value, stationId);
        else if (isClient)
            CmdSetQuadCycle(value, stationId);
    }

    /** Post cycle state for the specified quad screen. */
    public void PostQuadFullScreen(int value, int stationId)
    {
        if (isServer)
            serverUtils.DCCScreenData.SetQuadFullScreen(value, stationId);
        else if (isClient)
            CmdSetQuadFullScreen(value, stationId);
    }

    /** Add noise to a server parameter. */
    public void PostAddNoise(string parameter, noiseData.Profile profile)
    {
        if (isServer)
            serverUtils.NoiseData.AddNoise(parameter, profile);
        else if (isClient)
            CmdAddNoise(parameter, profile);
    }

    /** Remove noise from a server parameter. */
    public void PostRemoveNoise(string parameter)
    {
        if (isServer)
            serverUtils.NoiseData.RemoveNoise(parameter);
        else if (isClient)
            CmdRemoveNoise(parameter);
    }


    // Commands
    // ------------------------------------------------------------

    /** Command to set a numeric data value on the server. */
    [Command]
    public void CmdPostServerFloat(string key, float value, bool add)
        { serverUtils.SetServerData(key, value, add); }

    /** Command to set a string data value on the server. */
    [Command]
    public void CmdPostServerString(string key, string value)
        { serverUtils.SetServerData(key, value); }

    /** Command to fire a physics impact event on the server. */
    [Command]
    public void CmdPostImpact(Vector3 impactVector)
        { serverUtils.ServerData.RpcImpact(impactVector); }

    /** Command to issue a sonar event on the server. */
    [Command]
    public void CmdPostSonarEvent(string eventName, Vector3[] waypoints, bool destroyOnEnd)
    {
        ServerSonarEvent(new megEventSonar
        {
            eventName = eventName,
            waypoints = waypoints,
            destroyOnEnd = destroyOnEnd
        });
    }

    /** Command to issue a sonar clear command on the server. */
    [Command]
    public void CmdPostSonarClear()
        { ServerSonarClear(); }

    /** Command to trigger a named camera event on the server. */
    [Command]
    public void CmdTriggerMapCameraEvent(string eventName)
        { ServerTriggerMapCameraEvent(eventName); }

    /** Command to trigger a custom camera event on the server. */
    [Command]
    public void CmdTriggerMapCameraState(megMapCameraEventManager.State state)
        { ServerTriggerMapCameraState(state); }

    /** Set vessel position to the server. */
    [Command]
    public void CmdSetVesselPosition(int id, Vector3 position)
        { ServerSetVesselPosition(id, position); }

    /** Set vessel movement type on the server. */
    [Command]
    public void CmdSetVesselMovementType(int id, string type)
        { ServerSetVesselMovementType(id, type); }

    /** Set vessel's name on the server. */
    [Command]
    public void CmdSetVesselName(int id, string name)
        { ServerSetVesselName(id, name); }

    /** Set vessel map visibility to the server. */
    [Command]
    public void CmdSetVesselOnMap(int id, bool value)
        { ServerSetVesselOnMap(id, value); }

    /** Set vessel sonar visibility to the server. */
    [Command]
    public void CmdSetVesselOnSonar(int id, bool value)
        { ServerSetVesselOnSonar(id, value); }

    /** Set vessel's depth on the server. */
    [Command]
    public void CmdSetVesselDepth(int id, float depth)
        { ServerSetVesselDepth(id, depth); }

    /** Set vessel's icon on the server. */
    [Command]
    public void CmdSetVesselIcon(int id, vesselData.Icon icon)
        { ServerSetVesselIcon(id, icon); }

    /** Command to set vessel movements state on the server. */
    [Command]
    public void CmdSetVesselMovementState(string state)
    {
        try
        {
            var json = new JSONObject(state);
            ServerSetVesselMovementState(json);
        }
        catch (Exception ex)
        {
            Debug.LogError("CmdSetVesselMovementsState(): Failed to apply vessels state from client: " + ex);
        }
    }

    /** Add a vessel to the simulation. */
    [Command]
    public void CmdAddVessel(vesselData.Vessel vessel)
        { serverUtils.VesselData.AddVessel(vessel); }

    /** Remove the last vessel from the simulation. */
    [Command]
    public void CmdRemoveLastVessel()
        { serverUtils.VesselData.RemoveLastVessel(); }

    /** Clear extra vessels from the simulation. */
    [Command]
    public void CmdClearExtraVessels()
        { serverUtils.VesselData.ClearExtraVessels(); }

    /** Add a popup on the server. */
    [Command]
    public void CmdAddPopup(popupData.Popup popup)
        { ServerAddPopup(popup); }

    /** Toggle a popup on the server. */
    [Command]
    public void CmdTogglePopup(popupData.Popup popup)
        { ServerTogglePopup(popup); }

    /** Remove a popup (works on both clients and host). */
    [Command]
    public void CmdRemovePopup(popupData.Popup popup)
        { ServerRemovePopup(popup); }

    /** Clear all popups (works on both clients and host). */
    [Command]
    public void CmdClearPopups()
        { ServerClearPopups(); }

    /** Set glider screen id for the given player. */
    [Command]
    public void CmdSetGliderScreenId(NetworkInstanceId playerId, int screenId)
        { ServerSetGliderScreenId(playerId, screenId); }

    /** Set glider screen content id for the given player. */
    [Command]
    public void CmdSetGliderScreenContentId(NetworkInstanceId playerId, int contentId)
        { ServerSetGliderScreenContentId(playerId, contentId); }

    /** Set screen state for this player. */
    [Command]
    public void CmdSetScreenState(NetworkInstanceId id, screenData.State state)
        { ServerSetScreenState(id, state); }

    /** Set screen content for this player. */
    [Command]
    public void CmdSetScreenStateContent(NetworkInstanceId id, screenData.Content content)
        { ServerSetScreenStateContent(id, content); }

    /** Set content for the specified DCC screen on the server. */
    [Command]
    public void CmdSetScreenContent(DCCScreenID._screenID id, DCCWindow.contentID value, int stationId)
        { serverUtils.DCCScreenData.SetScreenContent(id, value, stationId); }

    /** Post content for the specified quad screen. */
    [Command]
    public void CmdSetQuadContent(DCCScreenContentPositions.positionID id, DCCWindow.contentID value, int stationId)
        { serverUtils.DCCScreenData.SetQuadContent(id, value, stationId); }

    /** Post cycle state for the specified quad screen. */
    [Command]
    public void CmdSetQuadCycle(int value, int stationId)
        { serverUtils.DCCScreenData.SetQuadCycle(value, stationId); }

    /** Post cycle state for the specified quad screen. */
    [Command]
    public void CmdSetQuadFullScreen(int value, int stationId)
        { serverUtils.DCCScreenData.SetQuadFullScreen(value, stationId); }

    /** Add noise to a server parameter. */
    [Command]
    public void CmdAddNoise(string parameter, noiseData.Profile profile)
        { serverUtils.NoiseData.AddNoise(parameter, profile); }

    /** Remove noise from a server parameter. */
    [Command]
    public void CmdRemoveNoise(string parameter)
        { serverUtils.NoiseData.RemoveNoise(parameter); }



    // Private Methods
    // ------------------------------------------------------------

    /** Issue a sonar event from the server. */
    [Server]
    private void ServerSonarEvent(megEventSonar sonar)
    {
        if (!Sonar)
            return;

        if (sonar.hasWaypoints)
            Sonar.megPlayMegSonarEvent(sonar);
        else
            Sonar.megPlayMegSonarEventByName(sonar.eventName);
    }

    /** Clear sonar on the server. */
    [Server]
    private void ServerSonarClear()
    {
        if (Sonar)
            Sonar.megSonarClear();
    }

    /** Trigger a named camera event on the server. */
    [Server]
    public void ServerTriggerMapCameraEvent(string eventName)
    {
        // Forward trigger command to all clients.
        RpcTriggerMapCameraEvent(eventName);
    }

    /** Trigger a custom camera event on the server. */
    [Server]
    public void ServerTriggerMapCameraState(megMapCameraEventManager.State state)
    {
        // Forward trigger command to all clients.
        RpcTriggerMapCameraState(state);
    }

    /** Set vessel position to the server. */
    [Server]
    public void ServerSetVesselPosition(int id, Vector3 position)
        { serverUtils.VesselData.SetPosition(id, position); }

    /** Set vessel movement type on the server. */
    [Server]
    public void ServerSetVesselMovementType(int id, string type)
        { serverUtils.VesselMovements.SetMovementType(id, type); }

    /** Set vessel's name on the server. */
    [Server]
    public void ServerSetVesselName(int id, string value)
        { serverUtils.VesselData.SetName(id, value); }

    /** Set vessel's depth on the server. */
    [Server]
    public void ServerSetVesselDepth(int id, float depth)
        { serverUtils.VesselData.SetDepth(id, depth); }

    /** Set vessel's icon on the server. */
    [Server]
    public void ServerSetVesselIcon(int id, vesselData.Icon icon)
        { serverUtils.VesselData.SetIcon(id, icon); }

    /** Set vessel map visibility to the server. */
    [Server]
    public void ServerSetVesselOnMap(int id, bool value)
        { serverUtils.VesselData.SetOnMap(id, value); }

    /** Set vessel sonar visibility to the server. */
    [Server]
    public void ServerSetVesselOnSonar(int id, bool value)
        { serverUtils.VesselData.SetOnSonar(id, value); }
    
    /** Set vessel movements state on the server. */
    [Server]
    public void ServerSetVesselMovementState(JSONObject json)
        { serverUtils.VesselMovements.LoadVessel(json); }

    /** Add a popup on the server. */
    [Server]
    public void ServerAddPopup(popupData.Popup popup)
        { serverUtils.PopupData.AddPopup(popup);}

    /** Toggle a popup on or off. */
    [Server]
    public void ServerTogglePopup(popupData.Popup popup)
        { serverUtils.PopupData.TogglePopup(popup); }

    /** Remove a popup on the server. */
    [Server]
    public void ServerRemovePopup(popupData.Popup popup)
        { serverUtils.PopupData.RemovePopup(popup); }

    /** Clear all popups on the server. */
    [Server]
    public void ServerClearPopups()
        { serverUtils.PopupData.Clear(); }

    /** Set glider screen id for the given player. */
    [Server]
    public void ServerSetGliderScreenId(NetworkInstanceId playerId, int screenId)
    {
        var player = serverUtils.GetPlayer(playerId);
        if (player)
            player.RpcSetGliderScreenId(screenId);
    }

    /** Set glider screen content id for the given player. */
    [Server]
    public void ServerSetGliderScreenContentId(NetworkInstanceId playerId, int contentId)
    {
        var player = serverUtils.GetPlayer(playerId);
        if (player)
            player.RpcSetGliderScreenContentId(contentId);
    }

    /** Set screen state for this player. */
    [Server]
    public void ServerSetScreenState(NetworkInstanceId playerId, screenData.State state)
    {
        var player = serverUtils.GetPlayer(playerId);
        if (player)
            player.ScreenState = state;
    }

    /** Set screen content for this player. */
    [Server]
    public void ServerSetScreenStateContent(NetworkInstanceId playerId, screenData.Content content)
    {
        ServerSetScreenState(playerId, new screenData.State { Type = ScreenState.Type, Content = content }); 
    }


    // Client Methods
    // ------------------------------------------------------------

    /** Trigger a named camera event on the client. */
    [ClientRpc]
    public void RpcTriggerMapCameraEvent(string eventName)
    {
        if (MapCamera)
            MapCamera.triggerByName(eventName);
    }

    /** Trigger a custom camera event on the client. */
    [ClientRpc]
    public void RpcTriggerMapCameraState(megMapCameraEventManager.State state)
    {
        if (MapCamera)
            MapCamera.triggerEventFromState(state);
    }

    /** Set glider screen id for the given player. */
    [ClientRpc]
    public void RpcSetGliderScreenId(int screenId)
    {
        if (!hasAuthority)
            return;

        glScreenManager.Instance.screenID = screenId;
        glScreenManager.Instance.hasChanged = true;
    }

    /** Set glider screen id for the given player. */
    [ClientRpc]
    public void RpcSetGliderScreenContentId(int contentId)
    {
        if (!hasAuthority)
            return;

        // TODO: Implement.
    }



    // Networking Methods
    // ------------------------------------------------------------

    /** Network pre-startup notification for a client. */
    public override void PreStartClient()
    {
        base.PreStartClient();
        Debug.Log("serverPlayer.PreStartClient(): ID: " + serverUtils.Id);
    }

    /** Network startup notification for a client. */
    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("serverPlayer.OnStartClient(): ID: " + serverUtils.Id);
    }

    /** Network startup notification for a server. */
    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("serverPlayer.OnStartServer(): ID: " + serverUtils.Id);
    }

    /** Network destruction notification. */
    public override void OnNetworkDestroy()
    {
        base.OnNetworkDestroy();
        Debug.Log("serverPlayer.OnNetworkDestroy(): ID: " + serverUtils.Id);
    }

    /** Initialization. */
    private void Start()
    {
        Debug.Log("serverPlayer.Start(): Client ID: " + serverUtils.Id);
    }

}
