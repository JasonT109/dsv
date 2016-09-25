using System;
using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Hit;
using Meg.Networking;

public class DCCCameraDragDrop : MonoBehaviour
{
    public bool hovering;
    public DCCCameraFeed parent;
    public bool canDropBucket = false;

    private Color fadecolor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
    private Color hilightColor = new Color(0.8f, 0.8f, 0.8f, 0.5f);
    private Color lerpColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    private Color defaultColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    private float lerpValue = 0;
    private Renderer r;
    private Transform child;
    private DCCDropBucketManager lastBucketManager;
    private DCCScreenManager screenManager;

    private void OnEnable()
    {
        GetComponent<PressGesture>().Pressed += pressedHandler;
        GetComponent<ReleaseGesture>().Released += releaseHandler;
        GetComponent<TransformGesture>().Transformed += transformHandler;
    }

    private void OnDisable()
    {
        GetComponent<PressGesture>().Pressed -= pressedHandler;
        GetComponent<ReleaseGesture>().Released -= releaseHandler;
        GetComponent<TransformGesture>().Transformed -= transformHandler;
    }

    private void pressedHandler(object sender, EventArgs e)
    {
        var gesture = sender as PressGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        hovering = true;
    }

    private void releaseHandler(object sender, EventArgs e)
    {
        var gesture = sender as ReleaseGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        bool droppedInBucket = false;

        if (canDropBucket)
        {
            GameObject DropTargetObject = GetDropBucket(hit.Point);
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
                    screenManager.ActivateWindow(DCCWindow.contentID.comms, targetScreen);
                    serverUtils.PostServerData("dcccommscontent", parent.materialID);
                    if (lastBucketManager)
                        lastBucketManager.highlightedBucket = null;

                    droppedInBucket = true;
                }
            }
        }

        if (!droppedInBucket)
        {
            var droptarget = GetDropTarget();
            if (droptarget && (droptarget.position == DCCCameraFeed.positions.midLeft || droptarget.position == DCCCameraFeed.positions.midLeftLow || droptarget.position == DCCCameraFeed.positions.midRight) && droptarget != gameObject.GetComponent<DCCCameraFeed>())
            {
                //Debug.Log("Dropped camera on to: " + droptarget);
                droptarget.materialID = parent.materialID;
            }
        }

        hovering = false;
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

        if (canDropBucket)
        {
            GameObject DropTargetObject = GetDropBucket(hit.Point);
            if (DropTargetObject)
            {
                //windowGlow.SetActive(true);
                DropTargetObject.GetComponent<DCCDropBucket>().manager.highlightedBucket = DropTargetObject;
                lastBucketManager = DropTargetObject.GetComponent<DCCDropBucket>().manager;
            }
            else
            {
                //windowGlow.SetActive(false);
                if (lastBucketManager)
                    lastBucketManager.highlightedBucket = null;
            }
        }
    }


    private DCCCameraFeed GetDropTarget()
    {
        var s = Camera.main.WorldToScreenPoint(transform.position);
        var ray = Camera.main.ScreenPointToRay(s);
        var hits = Physics.RaycastAll(ray);
        foreach (var hit in hits)
        {
            var dropzone = hit.transform.GetComponent<DCCCameraFeed>();
            if (dropzone)
                return dropzone;
        }

        return null;
    }

    /** Returns a drop bucket target gameobject for sending content to other screens. */
    private GameObject GetDropBucket(Vector3 point)
    {
        var screen = Camera.main.WorldToScreenPoint(point);
        var ray = Camera.main.ScreenPointToRay(screen);
        var hits = Physics.RaycastAll(ray);
        foreach (var hit in hits)
        {
            var dropzone = hit.transform.GetComponent<DCCDropBucket>();
            if (dropzone)
                return hit.transform.gameObject;
        }
        return null;
    }


    private void SetMaterialColor()
    {
        r = gameObject.GetComponentInChildren<Renderer>();
        r.material.SetColor("_TintColor", lerpColor);
        r.material.SetColor("_MainColor", lerpColor);
    }

    void Start ()
    {
        child = gameObject.transform.GetChild(0);
    }

    void Awake()
    {
        if (!screenManager)
            screenManager = ObjectFinder.Find<DCCScreenManager>();
    }

    void Update ()
    {
        if (hovering)
        {
            child.transform.localPosition = new Vector3(0, 0, -5);
            if (lerpValue < 1)
            {
                lerpValue = Mathf.Clamp01(lerpValue += Time.deltaTime * 3f);
                lerpColor = Color.Lerp(defaultColor, hilightColor, lerpValue);
                SetMaterialColor();
            }
        }
        else
        {
            transform.localPosition = Vector3.zero;
            child.transform.localPosition = new Vector3(0, 0, 0);
            if (lerpValue > 0)
            {
                lerpValue = Mathf.Clamp01(lerpValue -= Time.deltaTime);
                lerpColor = Color.Lerp(defaultColor, fadecolor, lerpValue);
                SetMaterialColor();
            }
        }
	}
}
