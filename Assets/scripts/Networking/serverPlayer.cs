using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
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
            CmdImpact(impactVector);
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

    /** Command to fire a physics impact event. */
    [Command]
    public void CmdImpact(Vector3 impactVector)
        { serverUtils.ServerData.RpcImpact(impactVector); }


}
