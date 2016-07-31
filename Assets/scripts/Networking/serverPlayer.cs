using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Meg.EventSystem;
using Meg.Networking;

public class serverPlayer : NetworkBehaviour
{

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


    // Private Methods
    // ------------------------------------------------------------

    /** Issue a sonar event from the server. */
    [Server]
    private void ServerSonarEvent(megEventSonar sonar)
    {
        if (!megEventManager.HasInstance)
            return;

        if (!string.IsNullOrEmpty(sonar.eventName))
            megEventManager.Instance.Sonar.megPlayMegSonarEventByName(sonar.eventName);
        else
            megEventManager.Instance.Sonar.megPlayMegSonarEvent(sonar);
    }



}
