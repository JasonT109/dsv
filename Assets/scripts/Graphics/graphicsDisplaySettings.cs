using UnityEngine;
using System.Collections;

public class graphicsDisplaySettings : Singleton<graphicsDisplaySettings>
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


    /** Whether scaling is active. */
    public bool Scaled { get; private set; }

    /** The current scale factor in effect. */
    public float Scale { get; private set; }



    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    void Awake()
    {
        Scaled = false;
        Scale = 1;
        Initialize();
    }

    /** Updating. */
    private void Update()
    {
	    if (b16x9.GetComponent<buttonControl>().active)
        {
            mainPanel.transform.localPosition = new Vector3(offset_16x9_X, mainPanel.transform.localPosition.y, mainPanel.transform.localPosition.z);

        }
        if (b16x10.GetComponent<buttonControl>().active)
        {
            mainPanel.transform.localPosition = new Vector3(offset_16x10_X, mainPanel.transform.localPosition.y, mainPanel.transform.localPosition.z);

        }
        if (b21x9l.GetComponent<buttonControl>().active)
        {
            mainPanel.transform.localPosition = new Vector3(offset_21x9l_X, mainPanel.transform.localPosition.y, mainPanel.transform.localPosition.z);

        }
        if (b21x9c.GetComponent<buttonControl>().active)
        {
            mainPanel.transform.localPosition = new Vector3(offset_21x9c_X, mainPanel.transform.localPosition.y, mainPanel.transform.localPosition.z);

        }
        if (b21x9r.GetComponent<buttonControl>().active)
        {
            mainPanel.transform.localPosition = new Vector3(offset_21x9r_X, mainPanel.transform.localPosition.y, mainPanel.transform.localPosition.z);
        }

        // Update camera scaling state.
        if (scaling)
            SetScaled(scaling.GetComponent<buttonControl>().active);
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Set up the initial screen scaling state. */
    public void Initialize()
    {
        if (!mainPanel)
            mainPanel = ObjectFinder.FindUiByName("Scene");
        if (!panelLeftSmall)
            panelLeftSmall = ObjectFinder.FindUiByRegex(".*PanelLeftSmall", "Panels_21x9");
        if (!panelRightSmall)
            panelRightSmall = ObjectFinder.FindUiByRegex(".*PanelRightSmall", "Panels_21x9");
        if (!panelLeftLarge)
            panelLeftLarge = ObjectFinder.FindUiByRegex(".*PanelLeftLarge", "Panels_21x9");
        if (!panelRightLarge)
            panelRightLarge = ObjectFinder.FindUiByRegex(".*PanelRightLarge", "Panels_21x9");

        var screenMode = NetworkManagerCustom.IsInGlider ? "16x10" : "16x9";
        screenMode = Configuration.Get("screen-mode", screenMode).ToLower();
        b16x9.GetComponent<buttonControl>().active = (screenMode == "16x9");
        b16x10.GetComponent<buttonControl>().active = (screenMode == "16x10");
        b21x9l.GetComponent<buttonControl>().active = (screenMode == "21x9l");
        b21x9c.GetComponent<buttonControl>().active = (screenMode == "21x9c");
        b21x9r.GetComponent<buttonControl>().active = (screenMode == "21x9r");

        var screenScaling = NetworkManagerCustom.IsInGlider;
        screenScaling = Configuration.Get("screen-scaling", screenScaling);
        scaling.GetComponent<buttonControl>().active = screenScaling;

        // Perform an initial update to set things up.
        Update();
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Update the current camera scaling state based on debug settings. */
    private void SetScaled(bool value)
    {
        var scaleRoot = Camera.main;
        var scaleFrom = GetAspectToScaleFrom();
        var scaleTo = GetAspectToScaleTo();

        Scaled = value;
        Scale = value ? (scaleTo / scaleFrom) : 1;

        scaleRoot.transform.localScale = new Vector3(Scale, 1, 1);
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
