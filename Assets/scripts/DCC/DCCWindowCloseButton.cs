using System;
using UnityEngine;
using System.Collections;
using Meg.Networking;
using TouchScript.Gestures;
using TouchScript.Hit;

public class DCCWindowCloseButton : MonoBehaviour
{
    public bool pressed = false;
    public buttonControl linkedButton;
    private Vector3 initPos = Vector3.zero;

    private bool _wasClosed;

    private void Awake ()
    {
        initPos = transform.parent.transform.localPosition;
    }

    private void OnEnable()
    {
        GetComponent<PressGesture>().Pressed += pressedHandler;
        GetComponent<ReleaseGesture>().Released += releaseHandler;

        // Set the transform to somewhere sensible
        if (_wasClosed && initPos.sqrMagnitude > 0)
            gameObject.transform.parent.transform.localPosition = initPos;
    }

    private void OnDisable()
    {
        GetComponent<PressGesture>().Pressed -= pressedHandler;
        GetComponent<ReleaseGesture>().Released -= releaseHandler;
    }

    private void pressedHandler(object sender, EventArgs e)
    {
        var gesture = sender as PressGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        pressed = true;
    }

    private void releaseHandler(object sender, EventArgs e)
    {
        var gesture = sender as ReleaseGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);
        pressed = false;

        var state = serverUtils.LocalScreenState;
        if (!CanCloseInScreen(state.Type))
            return;

        closeWindow();
    }

    private bool CanCloseInScreen(screenData.Type type)
    {
        switch (type)
        {
            case screenData.Type.DccQuad:
            case screenData.Type.DccScreen3:
            case screenData.Type.DccScreen4:
            case screenData.Type.DccScreen5:
                return false;
            default:
                return true;
        }
    }

    public void closeWindow()
    {
        _wasClosed = true;

        // Check if window uses shared state, and update that if so.
        var state = linkedButton ? linkedButton.GetComponent<WindowStateButton>() : null;
        if (state)
        {
            gameObject.transform.parent.gameObject.SetActive(false);
            state.RemoveWindow();
            return;
        }

        if (linkedButton)
            linkedButton.toggleButton(linkedButton.gameObject);
        else
            gameObject.transform.parent.gameObject.SetActive(false);

    }
}
