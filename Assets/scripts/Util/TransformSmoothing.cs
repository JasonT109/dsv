using UnityEngine;
using System.Collections;

public class TransformSmoothing : MonoBehaviour
{

    [Header("Components")]
    public Transform Source;
    public Transform Target;

    [Header("Application")]
    public bool ApplyPosition = true;
    public bool ApplyRotation = true;
    public bool ApplyScale;

    [Header("Coordinate Spaces")]
    public bool UseLocalPosition;
    public bool UseLocalRotation;
    public bool UseEulerAngles;

    [Header("Smoothing Times")]
    public float PositionSmoothTime = 0.25f;
    public float ScaleSmoothTime = 0.25f;
    public float RotationSmoothTime = 0.25f;
    

    private Vector3 _position;
    private Vector3 _scale;
    private Quaternion _rotation;

    private Vector3 _positionSmoothVelocity;
    private Vector3 _scaleSmoothVelocity;
    private Vector3 _rotationSmoothVelocity;

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
        // Update world-space position.
        if (ApplyPosition && !UseLocalPosition)
        {
            _position = Vector3.SmoothDamp(_position, Source.position, ref _positionSmoothVelocity, PositionSmoothTime);
            Target.position = _position;
        }

        // Update local-space position.
        if (ApplyPosition && UseLocalPosition)
        {
            _position = Vector3.SmoothDamp(_position, Source.localPosition, ref _positionSmoothVelocity, PositionSmoothTime);
            Target.localPosition = _position;
        }

        // Update local scale.
        if (ApplyScale)
        {
            _scale = Vector3.SmoothDamp(_scale, Source.localScale, ref _scaleSmoothVelocity, ScaleSmoothTime);
            Target.localScale = _scale;
        }

        // Update world-space rotation.
        if (ApplyRotation && !UseLocalRotation)
        {
            if (UseEulerAngles)
            {
                Target.eulerAngles = Vector3.SmoothDamp(Target.eulerAngles,
                    Source.eulerAngles,
                    ref _rotationSmoothVelocity,
                    RotationSmoothTime);
            }
            else
            {
                _rotation = Quaternion.Slerp(_rotation, Source.rotation, RotationSmoothTime*Time.deltaTime);
                Target.rotation = _rotation;
            }
        }

        // Update local-space rotation.
        if (ApplyRotation && UseLocalRotation)
        {
            if (UseEulerAngles)
            {
                Target.localEulerAngles = Vector3.SmoothDamp(Target.localEulerAngles,
                    Source.localEulerAngles,
                    ref _rotationSmoothVelocity,
                    RotationSmoothTime);
            }
            else
            {
                _rotation = Quaternion.Slerp(_rotation, Source.localRotation, RotationSmoothTime*Time.deltaTime);
                Target.localRotation = _rotation;
            }
        }

    }

}
