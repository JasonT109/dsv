using System;
using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Hit;

public class DCCCameraDragDrop : MonoBehaviour
{
    public bool hovering;
    public DCCCameraFeed parent;

    private Color fadecolor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
    private Color hilightColor = new Color(0.8f, 0.8f, 0.8f, 0.5f);
    private Color lerpColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    private Color defaultColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    private float lerpValue = 0;
    private Renderer r;
    private Transform child;

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

        hovering = true;
    }

    private void releaseHandler(object sender, EventArgs e)
    {
        var gesture = sender as ReleaseGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        var droptarget = GetDropTarget();
        if (droptarget && (droptarget.position == DCCCameraFeed.positions.midLeft || droptarget.position == DCCCameraFeed.positions.midRight) && droptarget != gameObject.GetComponent<DCCCameraFeed>())
        {
            //Debug.Log("Dropped camera on to: " + droptarget);
            droptarget.materialID = parent.materialID;
        }
        hovering = false;
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
