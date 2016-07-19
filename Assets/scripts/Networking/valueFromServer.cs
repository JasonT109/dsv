using UnityEngine;
using System.Collections;
using Meg.Networking;

public class valueFromServer : MonoBehaviour
{
    
    /** The component to set values on (must implement ValueSettable). */
    public Component Target;

    /** Link key used when looking up the data value from server. */
    public string linkDataString = "depth";

    /** How often to update the value. */
    public float updateTick = 0.2f;

    /** Timestamp for next update. */
    private float nextUpdate = 0;

    void Start()
    {
        nextUpdate = Time.time;
    }

    void Update()
    {
        var target = Target as ValueSettable;
        if (target == null)
            return;

        if (Time.time < nextUpdate)
            return;

        nextUpdate = Time.time + updateTick;
        target.SetValue(serverUtils.GetServerData(linkDataString));
    }

}
