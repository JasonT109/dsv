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
    public GameObject windowGlow;
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
    private float transformSpeed = 1f;
    private Vector2 transformDirection;
    private DCCScreenID._screenID swipeScreenID;
    private float pressTimer = 0;
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

    public static string NameForContent(contentID content)
    {
        var result = Enum.GetName(typeof(contentID), content);
        return result ?? "";
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

        windowGlow.SetActive(false);

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

        GameObject DropTargetObject = GetDropTarget();
        if (DropTargetObject)
        {
            DCCDropBucket.dropBucket DropTarget = DropTargetObject.GetComponent<DCCDropBucket>().bucket;
            DCCScreenID._screenID targetScreen = DCCScreenID._screenID.control;

            if (DropTarget == DCCDropBucket.dropBucket.left && HasScreen(screenData.Type.DccScreen3))
                targetScreen = DCCScreenID._screenID.screen3;

            if (DropTarget == DCCDropBucket.dropBucket.middle && HasScreen(screenData.Type.DccScreen4))
                targetScreen = DCCScreenID._screenID.screen4;

            if (DropTarget == DCCDropBucket.dropBucket.right && HasScreen(screenData.Type.DccScreen5))
                targetScreen = DCCScreenID._screenID.screen5;

            if (targetScreen != DCCScreenID._screenID.control)
            {
                TransformOffscreen(transformDirection, transformSpeed);
                screenManager.ActivateWindow(windowContent, targetScreen);
            }
        }
        pressTimer = 0;

        windowGlow.SetActive(false);
    }

    private static bool HasScreen(screenData.Type type)
    {
        return DCCScreenData.GetStationHasScreen(DCCScreenData.StationId, type);
    }

    private void transformHandler(object sender, EventArgs e)
    {
        var gesture = sender as TransformGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        GameObject DropTargetObject = GetDropTarget();
        if (DropTargetObject)
            windowGlow.SetActive(true);
        else
            windowGlow.SetActive(false);

    }

    /** Sets up data for sending content offscreen. */
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

    /** Returns a drop bucket target gameobject for sending content to other screens. */
    private GameObject GetDropTarget()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray);
        foreach (var hit in hits)
        {
            var dropzone = hit.transform.GetComponent<DCCDropBucket>();
            if (dropzone)
            {
                transformDirection = hit.transform.position - transform.position;
                return hit.transform.gameObject;
            }
        }
        return null;
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
            transform.localPosition = new Vector3 (transform.localPosition.x + (offscreenDirection.x * (offscreenSpeed * Time.deltaTime)), transform.localPosition.y + (offscreenDirection.y * (offscreenSpeed * Time.deltaTime)), transform.localPosition.z);

            if (offscreenLerpTimer > 1)
            {
                transformOffscreen = false;
                transform.localPosition = offscreenInitPos;
                GetComponentInChildren<DCCWindowCloseButton>().closeWindow();
            }
        }
	}
}
