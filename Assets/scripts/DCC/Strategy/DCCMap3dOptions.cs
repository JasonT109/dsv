using UnityEngine;
using System.Collections;
using DG.Tweening;
using Meg.Networking;
using UnityEngine.UI;

public class DCCMap3dOptions : MonoBehaviour
{

    // Components
    // ------------------------------------------------------------

    [Header("Components")]

    /** The 3D map widget. */
    public widget3DMap Map3D;

    /** The strategy map controller. */
    public DCCStrategyMap StrategyMap;


    [Header("UI")]

    /** Toggle indicators for 3d dive map. */
    [Header("Dive Map Options")]
    public Button MapContoursButton;
    public Button Map3DButton;
    public Button VesselLabelsButton;
    public Button AcidLayerButton;
    public Button WaterLayerButton;
    public Button RecenterVesselButton;


    // Members
    // ------------------------------------------------------------

    /** Toggle indicators for 3d dive map. */
    private Graphic _mapContoursOn;
    private Graphic _map3DOn;
    private Graphic _vesselLabelsOn;
    private Graphic _acidLayerOn;
    private Graphic _waterLayerOn;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        _mapContoursOn = MapContoursButton.GetComponentInChildrenNotMe<Image>(true);
        _map3DOn = Map3DButton.GetComponentInChildrenNotMe<Image>(true);
        _vesselLabelsOn = VesselLabelsButton.GetComponentInChildrenNotMe<Image>(true);
        _acidLayerOn = AcidLayerButton.GetComponentInChildrenNotMe<Image>(true);
        _waterLayerOn = WaterLayerButton.GetComponentInChildrenNotMe<Image>(true);

        MapContoursButton.onClick.AddListener(ActivateContourMode);
        Map3DButton.onClick.AddListener(Activate3DMode);
        VesselLabelsButton.onClick.AddListener(ToggleVesselLabels);
        AcidLayerButton.onClick.AddListener(ToggleAcidLayer);
        WaterLayerButton.onClick.AddListener(ToggleWaterLayer);
        RecenterVesselButton.onClick.AddListener(RecenterVessel);

        Update();
    }

    /** Updating. */
    private void Update()
    {
        // Update 3d dive map options.
        var isMap3D = StrategyMap.IsMap3D;
        MapContoursButton.gameObject.SetActive(isMap3D);
        Map3DButton.gameObject.SetActive(isMap3D);
        VesselLabelsButton.gameObject.SetActive(isMap3D);
        AcidLayerButton.gameObject.SetActive(isMap3D);
        WaterLayerButton.gameObject.SetActive(isMap3D);
        RecenterVesselButton.gameObject.SetActive(isMap3D);

        _mapContoursOn.gameObject.SetActive(Map3D.IsContourMode);
        _map3DOn.gameObject.SetActive(Map3D.Is3DMode);
        _vesselLabelsOn.gameObject.SetActive(NavSubPins.Instance.HasLabels);
        _acidLayerOn.gameObject.SetActive(serverUtils.GetServerBool("acidlayer"));
        _waterLayerOn.gameObject.SetActive(serverUtils.GetServerBool("waterlayer"));
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

    /** Toggle the acid layer. */
    public void ToggleAcidLayer()
        { Map3D.ToggleAcidLayer(); }

    /** Toggle the water layer. */
    public void ToggleWaterLayer()
        { Map3D.ToggleWaterLayer(); }

}
