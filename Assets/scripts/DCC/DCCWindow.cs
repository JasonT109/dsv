using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;
using TouchScript.Hit;
using Meg.DCC;

public class DCCWindow : MonoBehaviour
{
    public bool quadWindow;
    public DCCScreenContentPositions.positionID quadPosition;
    public enum contentID
    {
        none,
        instruments,
        thrusters,
        nav,
        sonar,
        diagnostics,
        lifesupport,
        piloting,
        comms,
        power,
        oxygen,
        batteries
    }

    public contentID windowContent;
    public bool hasFocus;
    public bool isLerping = false;
    public float lerpTime = 0.6f;
    private Vector3 toPosition;
    private Vector3 fromPosition;
    private Vector2 toScale;
    private Vector2 fromScale;
    private float lerpTimer = 0f;
    private graphicsDCCWindowSize window;
    private DCCScreenManager screenManager;

    public void MoveWindow(DCCScreenContentPositions.positionID destination)
    {
        if (window)
        {
            quadPosition = destination;
            isLerping = true;
            lerpTimer = 0;
            toPosition = DCCScreenContentPositions.GetScreenPosition(destination);
            fromPosition = transform.localPosition;

            toScale = DCCScreenContentPositions.GetScreenScale(destination);
            fromScale = new Vector2(window.windowWidth, window.windowHeight);
        }
    }

    public void SetWindowPosition(DCCScreenContentPositions.positionID destination)
    {
        quadPosition = destination;

        DCCScreenContentPositions.SetScreenPos(transform, destination);
        DCCScreenContentPositions.SetScreenScale(transform, destination);
    }

    private void OnDisable()
    {
        GetComponent<PressGesture>().Pressed -= pressedHandler;
        GetComponent<ReleaseGesture>().Released -= releaseHandler;
    }

    void OnEnable ()
    {
        GetComponent<PressGesture>().Pressed += pressedHandler;
        GetComponent<ReleaseGesture>().Released += releaseHandler;

        if (!window)
            window = GetComponent<graphicsDCCWindowSize>();

        if (quadWindow)
        {
            DCCScreenContentPositions.SetScreenPos(transform, quadPosition);
            DCCScreenContentPositions.SetScreenScale(transform, quadPosition);
        }
    }

    private void pressedHandler(object sender, EventArgs e)
    {
        var gesture = sender as PressGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        hasFocus = true;
    }

    private void releaseHandler(object sender, EventArgs e)
    {
        var gesture = sender as ReleaseGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        hasFocus = false;
    }


    IEnumerator focusWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        hasFocus = false;
    }

    void Awake ()
    {
        if (!window)
            window = GetComponent<graphicsDCCWindowSize>();
        if (!screenManager)
            screenManager = ObjectFinder.Find<DCCScreenManager>();
    }

    void Start ()
    {
        if (!screenManager)
            screenManager = ObjectFinder.Find<DCCScreenManager>();

        if (!quadWindow)
            screenManager.RegisterWindow(this);
    }

	void Update ()
    {
        if (isLerping)
        {
            lerpTimer += Time.deltaTime;

            float lerpAmount = lerpTimer / lerpTime;
            transform.localPosition = Vector3.Lerp(fromPosition, toPosition, lerpAmount);
            Vector2 windowScale = Vector2.Lerp(fromScale, toScale, lerpAmount);
            window.windowWidth = windowScale.x;
            window.windowHeight = windowScale.y;

            if (lerpTimer > lerpTime)
            {
                isLerping = false;
            }
        }

        if (!quadWindow)
        {
            if (hasFocus)
            {
                //tell manager to push this to the end of the list
                screenManager.PushWindowToFront(this);
            }
        }
	}
}
