using System;
using UnityEngine;
using UnityEngine.Events;
using TouchScript.Gestures;
using TouchScript.Hit;

public class OnTouch : MonoBehaviour
{

    public UnityEvent onPress;
    public UnityEvent onLongPress;
    public UnityEvent onTap;
    public UnityEvent onRelease;
    public UnityEvent onReleaseInside;
    public UnityEvent onReleaseOutside;

    private void Start()
    {
    }

    private void OnEnable()
    {
        var tap = GetComponent<TapGesture>();
        var press = GetComponent<PressGesture>();
        var longPress = GetComponent<LongPressGesture>();
        var release = GetComponent<ReleaseGesture>();

        if (tap)
            tap.Tapped += tappedHandler;
        if (press)
            press.Pressed += pressedHandler;
        if (longPress)
            longPress.LongPressed += longPressedHandler;
        if (release)
            release.Released += releaseHandler;
    }

    private void OnDisable()
    {
        var tap = GetComponent<TapGesture>();
        var press = GetComponent<PressGesture>();
        var longPress = GetComponent<LongPressGesture>();
        var release = GetComponent<ReleaseGesture>();

        if (tap)
            tap.Tapped -= tappedHandler;
        if (press)
            press.Pressed -= pressedHandler;
        if (longPress)
            longPress.LongPressed -= longPressedHandler;
        if (release)
            release.Released -= releaseHandler;
    }

    private void tappedHandler(object sender, EventArgs e)
    {
        if (onTap != null)
            onTap.Invoke();
    }

    private void pressedHandler(object sender, EventArgs e)
    {
        if (onPress != null)
            onPress.Invoke();
    }

    private void longPressedHandler(object sender, EventArgs e)
    {
        if (onLongPress != null)
            onLongPress.Invoke();
    }

    private void releaseHandler(object sender, EventArgs e)
    {
        TouchHit hit;
        if (onRelease != null)
            onRelease.Invoke();

        var gesture = sender as ReleaseGesture;
        if (!gesture)
            return;

        gesture.GetTargetHitResult(out hit);
        if (onReleaseInside != null && hit.Transform == transform)
            onReleaseInside.Invoke();
        else if (onReleaseOutside != null && hit.Transform != transform)
            onReleaseOutside.Invoke();
    }

}
