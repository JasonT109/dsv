using UnityEngine;
using System.Collections;

public class InitialSubScreenSelector : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** The buttons for each screen. */
    public buttonGroup ScreenButtons;

    /** The screen buttons. */
    public buttonControl Instruments;
    public buttonControl Thrusters;
    public buttonControl Navigation;
    public buttonControl LifeSupport;
    public buttonControl Debug;
    public buttonControl Diagnostics;
    public buttonControl Sonar;
    public buttonControl Piloting;
    public buttonControl Dome;


    [Header("Configuration")]

    /** Whether selector is active in the editor. */
    public bool ActiveInEditor = false;

    /** Default startup screen. */
    public string DefaultScreen = "instruments";

    /** Whether to crossfade between screens. */
    public bool CrossFadeEnabled = false;


    // Members
    // ------------------------------------------------------------

    /** Screen fader for doing crossfade transitions. */
    private ScreenFader _screenFader;

    /** Number of screen transitions that have been observed. */
    private int _screenTransitionCount;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Awake()
    {
        #if UNITY_EDITOR
        if (!ActiveInEditor)
            return;
        #endif

        // Determine the initial screen to display.
        var initial = Configuration.Get("screen-initial", DefaultScreen).ToLower();

        // Switch to the desired initial screen.
        switch (initial)
        {
            case "instruments":
                ScreenButtons.toggleButtonOn(Instruments.gameObject);
                break;
            case "thrusters":
                ScreenButtons.toggleButtonOn(Thrusters.gameObject);
                break;
            case "navigation":
                ScreenButtons.toggleButtonOn(Navigation.gameObject);
                break;
            case "vitals":
            case "lifesupport":
                ScreenButtons.toggleButtonOn(LifeSupport.gameObject);
                break;
            case "debug":
                Debug.GetComponent<widgetDebugButton>().Activate();
                break;
            case "diagnostics":
                ScreenButtons.toggleButtonOn(Diagnostics.gameObject);
                break;
            case "sonar":
                ScreenButtons.toggleButtonOn(Sonar.gameObject);
                break;
            case "piloting":
                ScreenButtons.toggleButtonOn(Piloting.gameObject);
                break;
            case "dome":
                ScreenButtons.toggleButtonOn(Dome.gameObject);
                break;
        }

        // Ensure debug screen is deactivated if needed.
        if (initial != "debug")
            Debug.GetComponent<widgetDebugButton>().Deactivate();

        // Set up initial screen scaling state.
        graphicsDisplaySettings.Instance.Initialize();

        // Configure the screen fader.
        _screenFader = ScreenFader.Instance;
        CrossFadeEnabled = Configuration.Get("screen-crossfade", CrossFadeEnabled);
    }
    
    /** Updating. */ 
    private void Update()
    {
        if (ScreenButtons.changed)
        {
            ScreenButtons.changed = false;
            if (_screenFader && CrossFadeEnabled && _screenTransitionCount > 0)
                _screenFader.Fade();

            _screenTransitionCount++;
        }
    }

}
