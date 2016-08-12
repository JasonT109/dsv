using UnityEngine;
using System.Collections;
using Meg.Networking;

public class serverValueRotate : MonoBehaviour
{
    public string LinkDataString;
    public float DefaultValue;

    public Vector3 Axis = new Vector3(0, 0, 1);
    public float Offset = 0;
    public float Scale = 1;

    void OnEnable()
        { UpdateTransform(); }

    void LateUpdate()
        { UpdateTransform(); }

    void UpdateTransform()
    {
        var value = serverUtils.GetServerData(LinkDataString, DefaultValue);
        transform.localRotation = Quaternion.AngleAxis(Offset + value * Scale, Axis);
    }

}
