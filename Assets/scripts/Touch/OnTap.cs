using System;
using UnityEngine;
using UnityEngine.Events;
using TouchScript.Gestures;

public class OnTap : MonoBehaviour
{

    public UnityEvent onTap;

    private void OnEnable()
    {
        var tap = GetComponent<TapGesture>();

        if (tap)
            tap.Tapped += tappedHandler;
    }

    private void OnDisable()
    {
        var tap = GetComponent<TapGesture>();

        if (tap)
            tap.Tapped -= tappedHandler;
    }

    private void tappedHandler(object sender, EventArgs e)
    {
        if (onTap != null)
            onTap.Invoke();
    }

}
