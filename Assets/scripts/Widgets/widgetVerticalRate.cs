using UnityEngine;
using System.Collections;
using Meg.Networking;

public class widgetVerticalRate : MonoBehaviour
{

    private Quaternion q;
	
	void Update ()
    {
        // Vertical rate of ascent/descent (expected units: m/s).
        var verticalRate = serverUtils.GetServerData("verticalVelocity");

        // Position the needle to correspond to the vertical rate.
        // Vertical rate of +/- 45 m/s corresponds to +/- 90 degrees needle rotation.
        var angle = verticalRate * 2;
        q = Quaternion.Euler(new Vector3(0, 180, angle));
        transform.rotation = q;
    }
}
