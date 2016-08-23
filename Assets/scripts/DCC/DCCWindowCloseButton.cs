using System;
using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Hit;

public class DCCWindowCloseButton : MonoBehaviour
{
    public bool pressed = false;
    public buttonControl linkedButton;

    private void OnEnable()
    {
        GetComponent<PressGesture>().Pressed += pressedHandler;
        GetComponent<ReleaseGesture>().Released += releaseHandler;
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

        closeWindow();
    }

    public void closeWindow()
    {
        if (linkedButton)
            linkedButton.toggleButton(linkedButton.gameObject);
        else
            gameObject.transform.parent.gameObject.SetActive(false);
    }
}
