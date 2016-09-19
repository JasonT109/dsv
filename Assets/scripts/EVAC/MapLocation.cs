using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using DG.Tweening;
using Meg.Maths;
using TouchScript.Gestures;

public class MapLocation : MonoBehaviour
{
    public float Zoom = 1;

    private TransformGesture _gesture;

    private void Start()
    {
        _gesture = transform.GetComponentInParents<TransformGesture>();
        _gesture.TransformStarted += OnTransformStarted;
        _gesture.TransformCompleted += OnTransformCompleted;

        var tap = GetComponent<TapGesture>();
        if (tap)
            tap.Tapped += OnTapped;
    }

    private void OnTapped(object sender, EventArgs eventArgs)
        { Focus(); }

    private void OnTransformStarted(object sender, EventArgs e)
        { }

    private void OnTransformCompleted(object sender, EventArgs e)
        { }

    public void Focus()
    {
        var t = _gesture.transform;
        if (DOTween.IsTweening(t))
            return;

        var local = transform.localPosition;
        var target = -local * Zoom;

        _gesture.Cancel();
        t.DOScale(Zoom, 1);
        t.DOLocalMove(target, 1);
    }
    
}
