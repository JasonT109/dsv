using UnityEngine;
using System.Collections;

public class TransformSmoothedRotation : MonoBehaviour
{

    [Header("Components")]
    public Transform Source;
    public Transform Target;

    [Header("Axes")]
    public bool X = true;
    public bool Y = true;
    public bool Z = true;

    [Header("Smoothing Times")]
    public float SmoothTime = 0.25f;

    private Vector3 _smoothVelocity;

    private void Start()
    {
        if (!Source)
            Source = transform.parent;

        if (!Target)
            Target = transform;
    }

    private void LateUpdate()
    {
        var euler = Target.eulerAngles;
        var smoothed = Vector3.SmoothDamp(euler,
            Source.eulerAngles, ref _smoothVelocity, SmoothTime);

        if (!X)
            smoothed.x = euler.x;
        if (!Y)
            smoothed.y = euler.y;
        if (!Z)
            smoothed.z = euler.z;

        Target.eulerAngles = smoothed;
    }

}
