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


    // Enumerations
    // ------------------------------------------------------------

    /** Possible screen aspect ratios. */
    public enum ScreenMode
    {
        Mode16X9,
        Mode16X10,
        Mode21X9,
        Mode21X9L,
        Mode21X9C,
        Mode21X9R,
    }


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

    /** Current screen mode. */
    private ScreenMode _mode = ScreenMode.Mode16X9;
    public ScreenMode Mode
    {
        get { return _mode; }
        private set { _mode = value; }
    }

    /** Whether scaling is active. */
    public bool Scaled { get; private set; }

    /** The current scale factor in effect. */
    private float _scale = 1;
    public float Scale
    {
        get { return _scale;}
        private set { _scale = value; }
    }

    /** Whether scaling has been locally overridden via debug menu. */
    public bool Overridden { get; private set; }


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    void Awake()
    {
        Scaled = false;
        Scale = 1;

        // Listen for clicks on the debug menu buttons.
        b16x9.GetComponent<buttonControl>().onPress.AddListener(OnButtonPressed);
        b16x10.GetComponent<buttonControl>().onPress.AddListener(OnButtonPressed);
        b21x9l.GetComponent<buttonControl>().onPress.AddListener(OnButtonPressed);
        b21x9c.GetComponent<buttonControl>().onPress.AddListener(OnButtonPressed);
        b21x9r.GetComponent<buttonControl>().onPress.AddListener(OnButtonPressed);
        scaling.GetComponent<buttonControl>().onPress.AddListener(OnButtonPressed);

        Initialize();
    }

    private void OnButtonPressed()
        { Overridden = true; }

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
        var screenMode = NetworkManagerCustom.IsInGlider ? "16x10" : "16x9";
        screenMode = Configuration.Get("screen-mode", screenMode).ToLower();
        var ratio = ModeForName(screenMode);

        var screenScaling = NetworkManagerCustom.IsInGlider;
        screenScaling = Configuration.Get("screen-scaling", screenScaling);

        Configure(ratio, screenScaling);
    }

    /** Returns an aspect ratio by name. */
    public static ScreenMode ModeForName(string screenMode)
    {
        switch (screenMode.ToLower())
        {
            case "16x9":
                return ScreenMode.Mode16X9;
            case "16x10":
                return ScreenMode.Mode16X10;
            case "21x9":
                return ScreenMode.Mode21X9;
            case "21x9l":
                return ScreenMode.Mode21X9L;
            case "21x9c":
                return ScreenMode.Mode21X9C;
            case "21x9r":
                return ScreenMode.Mode21X9R;
            default:
                return ScreenMode.Mode16X9;
        }
    }

    /** Apply specific screen scaling settings. */
    public void Configure(ScreenMode mode, bool scaling)
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

        b16x9.GetComponent<buttonControl>().active = (mode == ScreenMode.Mode16X9);
        b16x10.GetComponent<buttonControl>().active = (mode == ScreenMode.Mode16X10);
        b21x9l.GetComponent<buttonControl>().active = (mode == ScreenMode.Mode21X9L);
        b21x9c.GetComponent<buttonControl>().active = (mode == ScreenMode.Mode21X9C || mode == ScreenMode.Mode21X9);
        b21x9r.GetComponent<buttonControl>().active = (mode == ScreenMode.Mode21X9R);
        this.scaling.GetComponent<buttonControl>().active = scaling;

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
