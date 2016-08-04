using UnityEngine;
using System.Collections;

public class InitialSubScreenSelector : MonoBehaviour
{

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
    public buttonControl Rov;

    /** Default startup screen. */
    public string DefaultScreen = "instruments";

    /** Initialization. */
    private void Awake()
    {
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
            case "rov":
                ScreenButtons.toggleButtonOn(Rov.gameObject);
                break;
        }

        // Ensure debug screen is deactivated if needed.
        if (initial != "debug")
            Debug.GetComponent<widgetDebugButton>().Deactivate();

        // Set up initial screen scaling state.
        graphicsDisplaySettings.Instance.Initialize();
    }

}
