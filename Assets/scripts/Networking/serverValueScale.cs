using UnityEngine;
using System.Collections;
using Meg.Maths;
using Meg.Networking;

public class serverValueScale : MonoBehaviour
{
    public string LinkDataString;
    public float DefaultValue;

    public Vector2 FromRange = new Vector2(0, 100);
    public Vector2 ToRange = new Vector2(1, 2);
    public bool Clamped = true;
    public bool Reciprocal;

    public float SmoothTime = 0;

    private float _smoothed;
    private float _smoothedVelocity;

    void OnEnable()
    {
        _smoothed = serverUtils.GetServerData(LinkDataString, DefaultValue);
        UpdateTransform();
    }

    void LateUpdate()
        { UpdateTransform(); }

    void UpdateTransform()
    {
        var value = serverUtils.GetServerData(LinkDataString, DefaultValue);
        _smoothed = Mathf.SmoothDamp(_smoothed, value, ref _smoothedVelocity, SmoothTime*Time.deltaTime);

        var scale = graphicsMaths.remapValue(_smoothed, FromRange.x, FromRange.y, ToRange.x, ToRange.y);
        if (Clamped)
            scale = Mathf.Clamp(scale, ToRange.x, ToRange.y);

        if (Reciprocal && !Mathf.Approximately(scale, 0))
            scale = 1 / scale;

        transform.localScale = Vector3.one * scale;
    }

}
