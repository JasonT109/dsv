using UnityEngine;

[System.Serializable]
public class visSequenceObject
{
    public enum visStatus
    {
        show,
        hide
    }
    public GameObject visObject;
    public float visTime;
    public visStatus visibility;
}
