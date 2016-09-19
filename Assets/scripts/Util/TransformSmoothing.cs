using UnityEngine;
using System.Collections;

public class TransformSmoothing : MonoBehaviour
{

    public Transform Source;
    public Transform Target;

    public bool ApplyPosition = true;
    public bool ApplyRotation = true;
    public bool ApplyScale;

    public float PositionSmoothTime = 0.25f;
    public float ScaleSmoothTime = 0.25f;
    public float RotationSmoothTime = 0.25f;

    private Vector3 _position;
    private Vector3 _scale;
    private Quaternion _rotation;

    private Vector3 _positionSmoothVelocity;
    private Vector3 _scaleSmoothVelocity;

    private void Start()
    {
        if (!Source)
            Source = transform.parent;

        if (!Target)
            Target = transform;

        _position = Source.position;
        _rotation = Source.rotation;
        _scale = Source.localScale;
    }

    private void LateUpdate()
    {
        if (ApplyPosition)
        {
            _position = Vector3.SmoothDamp(_position, Source.position, ref _positionSmoothVelocity, PositionSmoothTime);
            transform.position = _position;
        }

        if (ApplyScale)
        {
            _scale = Vector3.SmoothDamp(_scale, Source.localScale, ref _scaleSmoothVelocity, ScaleSmoothTime);
            transform.localScale = _scale;
        }

        if (ApplyRotation)
        {
            _rotation = Quaternion.Slerp(_rotation, Source.rotation, RotationSmoothTime*Time.deltaTime);
            transform.rotation = _rotation;
        }

    }

}
