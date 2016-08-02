using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Meg.EventSystem;
using Meg.Networking;
using Meg.SonarEvent;

public class serverPlayer : NetworkBehaviour
{

    // Private Properties
    // ------------------------------------------------------------

    /** The sonar event manager. */
    private megSonarEventManager Sonar
    {
        get
        {
            if (megEventManager.HasInstance)
                return megEventManager.Instance.GetSonarEventManager();
            else
                return null;
        }
    }

    /** The map camera event manager. */
    private megMapCameraEventManager MapCamera
    {
        get
        {
            if (megEventManager.HasInstance)
                return megEventManager.Instance.GetMapCameraEventManager();
            else
                return null;
        }
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Set a numeric data value on the server. */
    public void PostServerData(string key, float value)
    {
        if (isServer)
            serverUtils.SetServerData(key, value);
        else if (isClient)
            CmdPostServerFloat(key, value);
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
    public void PostSonarClear(megEventSonar sonar)
    {
        if (isServer)
            ServerSonarClear();
        else if (isClient)
            CmdPostSonarClear();
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


    // Commands
    // ------------------------------------------------------------

    /** Command to set a numeric data value on the server. */
    [Command]
    public void CmdPostServerFloat(string key, float value)
        { serverUtils.SetServerData(key, value); }

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


}
