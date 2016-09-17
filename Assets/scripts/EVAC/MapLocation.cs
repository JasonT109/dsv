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
    private bool _transforming;

    private static bool _focussing;

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
        { _transforming = true; }

    private void OnTransformCompleted(object sender, EventArgs e)
        { _transforming = false; }


    /*
    public void Focus()
    {
        var t = _gesture.transform;
        if (DOTween.IsTweening(t))
            return;

        var f = GetCurrentFocalPoint();

        var target = t.InverseTransformPoint(transform.position);
        var focal = t.InverseTransformPoint(f);
        var ratio = Zoom / t.localScale.x;
        var delta = (focal - target) * ratio;
        delta.z = 0;

        var to = t.position + delta;

        Debug.DrawLine(Camera.main.transform.position, f, Color.cyan, 60);
        Debug.DrawLine(transform.position, f, Color.green, 60);
        Debug.DrawLine(transform.position, to, Color.yellow, 60);

        _gesture.Cancel();
        t.DOScale(Zoom, Duration);
        t.DOMove(to, Duration);
    }
    */

    public void Focus()
    {
        if (!_transforming && !_focussing)
            StartCoroutine(FocusRoutine());
    }

    private IEnumerator FocusRoutine()
    {
        if (_focussing)
            yield break;

        _focussing = true;
        var t = _gesture.transform;
        _gesture.Cancel();

        var stopTime = Time.time + 10;
        var targetZoom = new Vector3(Zoom, Zoom, 1);
        var v = Vector3.zero;

        while (Time.time < stopTime)
        {
            var target = transform.position;
            var focal = GetCurrentFocalPoint();
            var delta = focal - target;

            Debug.DrawLine(Camera.main.transform.position, focal, Color.cyan);
            Debug.DrawLine(target, focal, Color.green);

            t.Translate(delta * 5 * Time.deltaTime, Space.World);
            t.localScale = Vector3.SmoothDamp(t.localScale, targetZoom, ref v, 1);

            var scaleCorrect = Mathf.Abs(t.localScale.x - targetZoom.x) < (Zoom * 0.001f);
            var positionCorrect = delta.magnitude < 0.01f;
            if ((scaleCorrect && positionCorrect) || _transforming)
            {
                _focussing = false;
                yield break;
            }

            yield return 0;
        }

        _focussing = false;
    }

    private Vector3 GetCurrentFocalPoint()
    {
        var t = _gesture.transform;
        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        var hits = Physics.RaycastAll(ray);

        var hit = hits.FirstOrDefault(h => h.transform == t);
        return hit.point;
    }

}
