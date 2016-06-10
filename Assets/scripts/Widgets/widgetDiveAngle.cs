using UnityEngine;
using System.Collections;
using Meg.Networking;

public class widgetDiveAngle : MonoBehaviour
{

    private Quaternion q;
	
	// Update is called once per frame
	void Update ()
    {
        q = Quaternion.Euler(new Vector3(0, 180, serverUtils.GetServerData("pitchAngle")));
        transform.rotation = q;
    }
}
