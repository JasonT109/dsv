using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DCCStrategyMenu : MonoBehaviour
{

    // Components
    // ------------------------------------------------------------

    [Header("Components")]

    /** The 3D map widget. */
    public widget3DMap Map3D;


    [Header("UI")]

    /** Toggle indicator for contour mode. */
    public Graphic MapContoursOn;

    /** Toggle indicator for 3d mode. */
    public Graphic Map3DOn;

    /** Toggle indicator for vessel labels. */
    public Graphic VesselLabelsOn;


    // Unity Methods
    // ------------------------------------------------------------

    /** Updating. */
    private void Update()
    {
        MapContoursOn.gameObject.SetActive(Map3D.IsContourMode);
        Map3DOn.gameObject.SetActive(Map3D.Is3DMode);
        VesselLabelsOn.gameObject.SetActive(NavSubPins.Instance.HasLabels);
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Set contour mode on the 3d map. */
    public void ActivateContourMode()
        { megMapCameraEventManager.Instance.triggerByName("MapContours"); }

    /** Set 3d mode on the map. */
    public void Activate3DMode()
        { megMapCameraEventManager.Instance.triggerByName("Map3d"); }

    /** Toggle vessel labels on the map. */
    public void ToggleVesselLabels()
        { NavSubPins.Instance.ToggleLabels(); }

    /** Recenter on selected vessel. */
    public void RecenterVessel()
        { megMapCameraEventManager.Instance.triggerByName("RecenterVessel"); }

}
