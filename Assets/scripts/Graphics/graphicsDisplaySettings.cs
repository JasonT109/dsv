using UnityEngine;
using System.Collections;

public class graphicsDisplaySettings : MonoBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** 16:9 Aspect ratio. */
    public const float AspectRatio16By9 = 16 / 9f;

    /** 16:10 Aspect ratio. */
    public const float AspectRatio16By10 = 16 / 10f;

    /** 21:9 Aspect ratio. */
    public const float AspectRatio21By9 = 21 / 9f;


    // Properties
    // ------------------------------------------------------------

    [Header("Aspect Ratio Buttons")]
    public GameObject b16x9;
    public GameObject b16x10;
    public GameObject b21x9l;
    public GameObject b21x9c;
    public GameObject b21x9r;
    public GameObject scaling;

    [Header("Aspect Ratio Offsets")]
    public float offset_16x9_X = 0f;
    public float offset_16x10_X = 0f;
    public float offset_21x9l_X = -41.1f;
    public float offset_21x9c_X = 0f;
    public float offset_21x9r_X = 41.1f;
    public float leftLargePanelXOffset = -83.6f;

    [Header("Content Panels")]
    public GameObject mainPanel;
    public GameObject panelLeftSmall;
    public GameObject panelRightSmall;
    public GameObject panelLeftLarge;
    public GameObject panelRightLarge;


    // Unity Methods
    // ------------------------------------------------------------

    /** Updating. */
    private void Update()
    {
	    if (b16x9.GetComponent<buttonControl>().active)
        {
            mainPanel.transform.localPosition = new Vector3(offset_16x9_X, mainPanel.transform.localPosition.y, mainPanel.transform.localPosition.z);
            panelLeftSmall.SetActive(false);
            panelRightSmall.SetActive(false);
            panelLeftLarge.SetActive(false);
            panelRightLarge.SetActive(false);
        }
        if (b16x10.GetComponent<buttonControl>().active)
        {
            mainPanel.transform.localPosition = new Vector3(offset_16x10_X, mainPanel.transform.localPosition.y, mainPanel.transform.localPosition.z);
            panelLeftSmall.SetActive(false);
            panelRightSmall.SetActive(false);
            panelLeftLarge.SetActive(false);
            panelRightLarge.SetActive(false);
        }
        if (b21x9l.GetComponent<buttonControl>().active)
        {
            mainPanel.transform.localPosition = new Vector3(offset_21x9l_X, mainPanel.transform.localPosition.y, mainPanel.transform.localPosition.z);
            panelLeftSmall.SetActive(false);
            panelRightSmall.SetActive(false);
            panelLeftLarge.SetActive(false);
            panelRightLarge.SetActive(true);
        }
        if (b21x9c.GetComponent<buttonControl>().active)
        {
            mainPanel.transform.localPosition = new Vector3(offset_21x9c_X, mainPanel.transform.localPosition.y, mainPanel.transform.localPosition.z);
            panelLeftSmall.SetActive(true);
            panelRightSmall.SetActive(true);
            panelLeftLarge.SetActive(false);
            panelRightLarge.SetActive(false);
        }
        if (b21x9r.GetComponent<buttonControl>().active)
        {
            mainPanel.transform.localPosition = new Vector3(offset_21x9r_X, mainPanel.transform.localPosition.y, mainPanel.transform.localPosition.z);
            panelLeftSmall.SetActive(false);
            panelRightSmall.SetActive(false);
            panelLeftLarge.SetActive(true);
            panelRightLarge.SetActive(false);
            panelLeftLarge.transform.localPosition = new Vector3(leftLargePanelXOffset, panelLeftLarge.transform.localPosition.y, panelLeftLarge.transform.localPosition.z);
        }

        // Update camera scaling state.
        if (scaling)
            SetScaled(scaling.GetComponent<buttonControl>().active);
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Update the current camera scaling state based on debug settings. */
    private void SetScaled(bool scaled)
    {
        var scaleRoot = Camera.main;
        var scaleFrom = GetAspectToScaleFrom();
        var scaleTo = GetAspectToScaleTo();
        var scale = scaled ? (scaleTo / scaleFrom) : 1;

        scaleRoot.transform.localScale = new Vector3(scale, 1, 1);
    }

    /** Determine what content aspect ratio we're scaling from. */
    private float GetAspectToScaleFrom()
    {
        if (b16x9.GetComponent<buttonControl>().active)
            return AspectRatio16By9;
        if (b16x10.GetComponent<buttonControl>().active)
            return AspectRatio16By10;
        if (b21x9l.GetComponent<buttonControl>().active)
            return AspectRatio21By9;
        if (b21x9c.GetComponent<buttonControl>().active)
            return AspectRatio21By9;
        if (b21x9r.GetComponent<buttonControl>().active)
            return AspectRatio21By9;

        return AspectRatio16By9;
    }

    /** Determine what output aspect ratio we're scaling to. */
    private float GetAspectToScaleTo()
    {
        return AspectRatio16By9;
    }

}
