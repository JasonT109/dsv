using UnityEngine;
using System.Collections;
using Meg.Networking;

public class SonarNoiseRotation : MonoBehaviour 
{

	void Update ()
	{
	    var serverData = serverUtils.ServerData;
        if (!serverData)
            return;

        Vector3 constrained = new Vector3(transform.localRotation.eulerAngles.x, serverData.transform.rotation.eulerAngles.y, transform.localRotation.eulerAngles.x);
        transform.localRotation = Quaternion.Euler(constrained);
	}
}
