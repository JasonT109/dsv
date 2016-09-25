using System;
using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Hit;
using UnityEngine.UI;

public class RadialDial : MonoBehaviour
{
    public float SmoothTime = 0.25f;

    public float value
        { get { return _image ? _image.fillAmount : 0; } }

    private Image _image;
    private PinnedTransformGesture _gesture;

    private float _fill;
    private float _fillVelocity;

    private void Awake()
    {
        _image = GetComponentInChildren<Image>();
        _gesture = GetComponentInChildren<PinnedTransformGesture>(true);
        _gesture.TransformStarted += OnTransformStarted;
        _gesture.Transformed += OnTransformed;
    }

    private void Update()
    {
        // Apply some smoothing to the fill to give it life.
        _image.fillAmount = Mathf.SmoothDamp(_image.fillAmount, 
            _fill, ref _fillVelocity, SmoothTime);
    }

    private void OnTransformStarted(object sender, EventArgs e)
    {
        var gesture = sender as PinnedTransformGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        var o = transform.InverseTransformPoint(hit.Point);
        var angle = Mathf.Atan2(-o.x, -o.y) * Mathf.Rad2Deg;
        angle = Mathf.Repeat(90 + angle, 360f);
        _fill = angle / 360f;
    }

    private void OnTransformed(object sender, EventArgs e)
    {
        var gesture = sender as PinnedTransformGesture;
        _fill -= (gesture.DeltaRotation / 360f);
        _fill = Mathf.Clamp01(_fill);
    }


}
