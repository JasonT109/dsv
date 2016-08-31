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
    public float minSwipeSpeed = 1f;
    private Vector3 toPosition;
    private Vector3 fromPosition;
    private Vector2 toScale;
    private Vector2 fromScale;
    private float lerpTimer = 0f;
    private graphicsDCCWindowSize window;
    private DCCScreenManager screenManager;
    private float transformSpeed = 0;
    private float[] transformSpeedHistory = new float[3];
    private Vector3[] deltaHistory = new Vector3[3];
    private Vector2 transformDirection;
    private DCCScreenID._screenID swipeScreenID;
    private float pressTimer = 0;
    private float colorLerpTimer = 0;
    private int _commsContent = 0;
    private bool transformOffscreen;
    private float offscreenLerpTimer = 0f;
    private Vector2 offscreenDirection;
    private float offscreenSpeed;
    private Vector3 offscreenInitPos;

    public int commsContent
    {
        get
        {
            return _commsContent;
        }
        set
        {
            _commsContent = value;
        }
    }

    /** Moves a window to a destination position. */
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

            if (!quadWindow)
                screenManager.PushWindowToFront(this);
        }
    }

    /** Immediately sets a window to a destination position. */
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
        GetComponent<TransformGesture>().Transformed -= transformHandler;

        Unregister();
    }

    void OnEnable ()
    {
        GetComponent<PressGesture>().Pressed += pressedHandler;
        GetComponent<ReleaseGesture>().Released += releaseHandler;
        GetComponent<TransformGesture>().Transformed += transformHandler;

        transformOffscreen = false;

        if (!window)
            window = GetComponent<graphicsDCCWindowSize>();

        if (quadWindow)
        {
            DCCScreenContentPositions.SetScreenPos(transform, quadPosition);
            DCCScreenContentPositions.SetScreenScale(transform, quadPosition);
        }

        Register();
    }

    private void pressedHandler(object sender, EventArgs e)
    {
        var gesture = sender as PressGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        pressTimer += Time.deltaTime;

        hasFocus = true;
    }

    private void releaseHandler(object sender, EventArgs e)
    {
        var gesture = sender as ReleaseGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        hasFocus = false;

        if (transformSpeed > minSwipeSpeed)
        {
            DCCScreenID._screenID screenDirection = DCCScreenID._screenID.control;

            if (transformDirection.x < -0.2f && transformDirection.y > 0)
            {
                screenDirection = DCCScreenID._screenID.screen3;
                TransformOffscreen(transformDirection, Mathf.Clamp01(transformSpeed * 0.1f));
            }

            if (transformDirection.x >= -0.2f &&  transformDirection.x <= 0.2f && transformDirection.y > 0)
            {
                screenDirection = DCCScreenID._screenID.screen4;
                TransformOffscreen(transformDirection, Mathf.Clamp01(transformSpeed * 0.1f));
            }

            if (transformDirection.x > 0.2f && transformDirection.y > 0)
            {
                screenDirection = DCCScreenID._screenID.screen5;
                TransformOffscreen(transformDirection, Mathf.Clamp01(transformSpeed * 0.1f));
            }

            if (screenDirection != DCCScreenID._screenID.control)
                screenManager.ActivateWindow(windowContent, screenDirection);

            //Debug.Log("Swipe detected in direction of screen: " + screenDirection + " " + transformDirection + " at a speed of " + transformSpeed);
        }

        transformSpeedHistory = new float[8];
        screenManager.swipeIndicator.SetActive(false);
        pressTimer = 0;
    }

    private void transformHandler(object sender, EventArgs e)
    {
        var gesture = sender as TransformGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        if (transformSpeed > minSwipeSpeed)
        {
            screenManager.swipeIndicator.SetActive(true);
        }
        else
        {
            screenManager.swipeIndicator.SetActive(false);
        }

        Renderer r = screenManager.swipeIndicator.GetComponentInChildren<Renderer>();

        r.material.SetColor("_TintColor", Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 0.5f), colorLerpTimer));

        screenManager.swipeIndicator.transform.position = hit.Point;
        Quaternion q = new Quaternion();    
        q.SetLookRotation(-Vector3.forward, transformDirection);
        screenManager.swipeIndicator.transform.rotation = q;
    }

    private void TransformOffscreen(Vector2 direction, float speed)
    {
        offscreenLerpTimer = 0;
        offscreenInitPos = transform.localPosition;
        transformOffscreen = true;
        offscreenDirection = direction;
        offscreenSpeed = speed;
    }

    /** Registers a window with the screen manager. */
    private void Register()
    {
        if (!screenManager)
            screenManager = ObjectFinder.Find<DCCScreenManager>();

        if (!quadWindow)
            screenManager.RegisterWindow(this);
    }

    /** Unregisters a window with the screen manager. */
    private void Unregister()
    {
        if (!screenManager)
            screenManager = ObjectFinder.Find<DCCScreenManager>();

        if (!quadWindow)
            screenManager.UnregisterWindow(this);
    }

    IEnumerator focusWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        hasFocus = false;
    }

    void StoreWindowMoveSpeed()
    {
        //calculate transform direction based on last few frames
        Vector3 p = transform.position;

        //copy array over itself moving it one index
        Array.Copy(deltaHistory, 1, deltaHistory, 0, deltaHistory.Length - 1);
        deltaHistory[deltaHistory.Length - 1] = p;

        //get transform direction
        transformDirection = (deltaHistory[1] - deltaHistory[0]).normalized;

        //get distance travelled last frame
        float t = Vector3.Distance(deltaHistory[0], deltaHistory[1]);

        //copy array over itself moving it one index
        Array.Copy(transformSpeedHistory, 1, transformSpeedHistory, 0, transformSpeedHistory.Length - 1);

        //add distance we moved last frame to array
        transformSpeedHistory[transformSpeedHistory.Length - 1] = t;

        //add all the distances together
        transformSpeed = 0;
        for (int i = 0; i < transformSpeedHistory.Length; i++)
            transformSpeed += transformSpeedHistory[i];

        //get average distance travelled
        transformSpeed /= transformSpeedHistory.Length;
    }

    void Awake ()
    {
        if (!window)
            window = GetComponent<graphicsDCCWindowSize>();
        if (!screenManager)
            screenManager = ObjectFinder.Find<DCCScreenManager>();
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

        if (!isLerping)
        {
            StoreWindowMoveSpeed();
            if (transformSpeed > minSwipeSpeed)
                Mathf.Clamp01(colorLerpTimer += Time.deltaTime * 3f);
            else
                colorLerpTimer = 0;
        }

        if (!quadWindow)
        {
            if (hasFocus)
            {
                screenManager.PushWindowToFront(this);
            }
        }

        if (transformOffscreen)
        {
            offscreenLerpTimer += Time.deltaTime;
            transform.localPosition = new Vector3 (transform.localPosition.x + (offscreenDirection.x * offscreenSpeed), transform.localPosition.y + (offscreenDirection.y * offscreenSpeed), transform.localPosition.z);
            //GetComponent<graphicsDCCWindowSize>().windowWidth *= 1 - offscreenLerpTimer;
            //GetComponent<graphicsDCCWindowSize>().windowHeight *= 1 - offscreenLerpTimer;

            if (offscreenLerpTimer > 1)
            {
                transformOffscreen = false;
                transform.localPosition = offscreenInitPos;
                GetComponentInChildren<DCCWindowCloseButton>().closeWindow();
            }
        }
	}
}
