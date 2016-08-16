using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 *
 * Interface logic for the debug 2D viewport.  Most of the spatial configuration for
 * the viewport is in the Unity scenegraph.  There are two primary display modes -
 * Sonar and Map.  The former can be displayed in either North Up or Heading Up modes.
 * Heading Up means that 'up' on the screen corresponds to the player sub's current direction.
 * 
 */

public class debugVesselViewportUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Root of the sonar display. */
    public Transform Sonar;

    /** Root of the map display. */
    public Transform Map;

    /** Yaw control for the sonar. */
    public serverValueRotate SonarYaw;

    /** Yaw control for the sonar's ship indicator. */
    public serverValueRotate SonarShipYaw;

    /** Toggle for setting sonar north-up mode. */
    public Toggle SonarNorthUpToggle;

    /** Toggle for setting sonar heading-up mode. */
    public Toggle SonarHeadingUpToggle;

    /** Toggle for setting map mode. */
    public Toggle MapToggle;

    /** Possible viewport display modes. */
    public enum ViewportMode
    {
        SonarNorthUp,
        SonarHeadingUp,
        Map
    }

    /** Current viewport mode. */
    public ViewportMode Mode = ViewportMode.SonarNorthUp;

    /** Whether sonar is displayed. */
    public bool IsSonar
        { get { return Mode == ViewportMode.SonarNorthUp || Mode == ViewportMode.SonarHeadingUp; } }

    /** Whether map is displayed. */
    public bool IsMap
        { get { return Mode == ViewportMode.Map; } }


    // Members
    // ------------------------------------------------------------

    /** Whether UI is being updated. */
    private bool _updating;


    // Unity Methods
    // ------------------------------------------------------------

    /** Enabling. */
    private void OnEnable()
        { UpdateUi(); }

    /** Update the UI's current state. */
    private void Update()
        { UpdateUi(); }


    // Public Methods
    // ------------------------------------------------------------

    /** Select Sonar North Up mode. */
    public void SelectSonarNorthUp()
        { if (!_updating) Mode = ViewportMode.SonarNorthUp; }

    /** Select Sonar heading Up mode. */
    public void SelectSonarHeadingUp()
        { if (!_updating) Mode = ViewportMode.SonarHeadingUp; }

    /** Select Map mode. */
    public void SelectMap()
        { if (!_updating) Mode = ViewportMode.Map; }


    // Private Methods
    // ------------------------------------------------------------

    /** Update the UI's current state. */
    private void UpdateUi()
    {
        _updating = true;

        SonarNorthUpToggle.isOn = Mode == ViewportMode.SonarNorthUp;
        SonarHeadingUpToggle.isOn = Mode == ViewportMode.SonarHeadingUp;
        MapToggle.isOn = Mode == ViewportMode.Map;

        Sonar.gameObject.SetActive(IsSonar);
        Map.gameObject.SetActive(IsMap);
        SonarYaw.enabled = Mode == ViewportMode.SonarHeadingUp;
        SonarShipYaw.enabled = Mode == ViewportMode.SonarNorthUp;

        _updating = false;
    }


}
