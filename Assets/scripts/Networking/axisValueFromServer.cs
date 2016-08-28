using UnityEngine;
using System.Collections;
using Meg.Networking;

public class axisValueFromServer : MonoBehaviour
{

    public string LinkDataString;
    public float DefaultValue;
    public Vector3 Axis = Vector3.right;
    public float Scale = 1;

    private void Update()
	{
	    var value = serverUtils.GetServerData(LinkDataString, DefaultValue);
        var axis = Axis * value * Scale;
        transform.localPosition = axis;
	}
}
