using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using DG.Tweening;
using Meg.Maths;
using TouchScript.Gestures;
using TouchScript.Hit;

public class MapTapZoom : MonoBehaviour
{
    public float ZoomFactor = 2;
    public Vector2 ZoomLimits = new Vector2(0.3f, 20f);
    public float Duration = 1;

    private TransformGesture _gesture;
    private TapGesture _tap;

    private void Start()
    {
        _gesture = transform.GetComponent<TransformGesture>();
        _gesture.TransformStarted += OnTransformStarted;
        _gesture.TransformCompleted += OnTransformCompleted;

        _tap = GetComponent<TapGesture>();
        if (_tap)
            _tap.Tapped += OnTapped;
    }

    private void OnTapped(object sender, EventArgs eventArgs)
    {
        TouchHit hit;
        _tap.GetTargetHitResult(out hit);
        var local = _gesture.transform.InverseTransformPoint(hit.Point);
        Focus(local);
    }

    private void OnTransformStarted(object sender, EventArgs e)
        { }

    private void OnTransformCompleted(object sender, EventArgs e)
        { }

    public void Focus(Vector3 local)
    {
        var t = _gesture.transform;
        if (DOTween.IsTweening(t))
            return;

        var zoom = Mathf.Clamp(t.localScale.x * ZoomFactor, ZoomLimits.x, ZoomLimits.y);
        var target = -local * zoom;

        _gesture.Cancel();
        t.DOScale(zoom, Duration).SetEase(Ease.OutSine);
        t.DOLocalMove(target, Duration).SetEase(Ease.OutSine);
    }
    
}
