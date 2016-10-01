using UnityEngine;
using System.Collections;
using DG.Tweening;
using TouchScript.Gestures;
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

    /** Header. */
    public GameObject Header;

    /** Footer. */
    public GameObject Footer;

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

    /** Toggle for setting nautical map mode. */
    public Toggle NauticalMapToggle;

    /** Possible viewport display modes. */
    public enum ViewportMode
    {
        SonarNorthUp,
        SonarHeadingUp,
        Map,
        NauticalMap
    }

    /** Current viewport mode. */
    public ViewportMode Mode = ViewportMode.SonarNorthUp;

    /** Whether sonar is displayed. */
    public bool IsSonar
        { get { return Mode == ViewportMode.SonarNorthUp || Mode == ViewportMode.SonarHeadingUp; } }

    /** Whether map is displayed. */
    public bool IsMap
        { get { return Mode == ViewportMode.Map; } }

    /** Whether nautical map is displayed. */
    public bool IsNauticalMap
        { get { return Mode == ViewportMode.NauticalMap; } }


    // Members
    // ------------------------------------------------------------

    /** Whether UI is being updated. */
    private bool _updating;

    /** Viewport's backdrop graphic. */
    private Graphic _graphic;



    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Awake()
        { _graphic = GetComponent<Graphic>(); }

    /** Enabling. */
    private void OnEnable()
        { UpdateUi(); }

    /** Disabling. */
    private void OnDisable()
    {
        // Disable 2D map when leaving the map viewport.
        if (Map2d.HasInstance && Mode == ViewportMode.NauticalMap)
            Map2d.Instance.gameObject.SetActive(false);
    }

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

    /** Select Nautical Map mode. */
    public void SelectNauticalMap()
        { if (!_updating) Mode = ViewportMode.NauticalMap; }


    // Private Methods
    // ------------------------------------------------------------

    /** Update the UI's current state. */
    private void UpdateUi()
    {
        _updating = true;

        SonarNorthUpToggle.isOn = Mode == ViewportMode.SonarNorthUp;
        SonarHeadingUpToggle.isOn = Mode == ViewportMode.SonarHeadingUp;
        MapToggle.isOn = Mode == ViewportMode.Map;

        NauticalMapToggle.isOn = Mode == ViewportMode.NauticalMap;
        NauticalMapToggle.interactable = Map2d.HasInstance;

        _graphic.enabled = Mode != ViewportMode.NauticalMap;

        Sonar.gameObject.SetActive(IsSonar);
        Map.gameObject.SetActive(IsMap);
        SonarYaw.enabled = Mode == ViewportMode.SonarHeadingUp;
        SonarShipYaw.enabled = Mode == ViewportMode.SonarNorthUp;

        if (Map2d.HasInstance)
            Map2d.Instance.gameObject.SetActive(Mode == ViewportMode.NauticalMap);

        // Allow zooming with the mouse wheel.
        var dz = Input.GetAxis("MouseAxis3");
        if (IsMap && !Mathf.Approximately(dz, 0))
            MouseWheelZoom(dz);

        _updating = false;
    }


    private void MouseWheelZoom(float dz)
    {
        var gesture = Map.GetComponentInChildren<TransformGesture>();
        var root = gesture.transform;

        var world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var target = root.InverseTransformPoint(world);

        target.z = 0;

        if (DOTween.IsTweening(root))
            return;

        var dZoom = (1 + dz * 0.5f);
        var zoom = root.localScale.x * dZoom;

        gesture.Cancel();
        var duration = 0.25f;
        root.DOScale(zoom, duration).SetEase(Ease.OutSine);
        root.DOLocalMove(-target * zoom, duration).SetEase(Ease.OutSine);
    }


}
