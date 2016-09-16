using UnityEngine;
using System.Collections;
using DG.Tweening;
using Meg.Networking;
using UnityEngine.UI;

public class DCCStrategyMenu : MonoBehaviour
{

    // Components
    // ------------------------------------------------------------

    [Header("Components")]

    /** The 3D map widget. */
    public widget3DMap Map3D;

    /** The strategy map controller. */
    public DCCStrategyMap StrategyMap;


    [Header("UI")] 
    
    /** View menu. */
    public CanvasGroup ViewGroup;

    /** Toggle indicators for 3d dive map. */
    [Header("Dive Map Options")]
    public Button MapContoursButton;
    public Button Map3DButton;
    public Button VesselLabelsButton;
    public Button AcidLayerButton;
    public Button WaterLayerButton;
    public Button RecenterVesselButton;

    /** Toggle indicators for 3d dive map. */
    [Header("Dive Map Indicators")]
    public Graphic MapContoursOn;
    public Graphic Map3DOn;
    public Graphic VesselLabelsOn;
    public Graphic AcidLayerOn;
    public Graphic WaterLayerOn;

    public Graphic DiveMapOn;
    public Graphic NauticalMapOn;
    public Graphic SubSchematicOn;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        StrategyMap.OnMapModeChanged += OnMapModeChanged;
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

        MapContoursOn.gameObject.SetActive(Map3D.IsContourMode);
        Map3DOn.gameObject.SetActive(Map3D.Is3DMode);
        VesselLabelsOn.gameObject.SetActive(NavSubPins.Instance.HasLabels);
        AcidLayerOn.gameObject.SetActive(serverUtils.GetServerBool("acidlayer"));
        WaterLayerOn.gameObject.SetActive(serverUtils.GetServerBool("waterlayer"));

        // Update 2d nautical map options.
        var isMap2D = StrategyMap.IsMap2D;
        

        DiveMapOn.gameObject.SetActive(isMap3D);
        NauticalMapOn.gameObject.SetActive(isMap2D);
        SubSchematicOn.gameObject.SetActive(StrategyMap.IsSubSchematic);
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

    /** Toggle the 3d dive map. */
    public void ToggleDiveMap()
        { StrategyMap.ActivateMapMode(mapData.Mode.Mode3D); }

    /** Toggle the 2d nautical map. */
    public void ToggleNauticalMap()
        { StrategyMap.ActivateMapMode(mapData.Mode.Mode2D); }

    /** Toggle the sub schematic. */
    public void ToggleSubSchematic()
        { StrategyMap.ActivateMapMode(mapData.Mode.ModeSubSchematic); }

    /** Toggle the acid layer. */
    public void ToggleAcidLayer()
        { Map3D.ToggleAcidLayer(); }

    /** Toggle the water layer. */
    public void ToggleWaterLayer()
        { Map3D.ToggleWaterLayer(); }


    // Private Methods
    // ------------------------------------------------------------

    /** Handle the map mode changing. */
    private void OnMapModeChanged(mapData.Mode oldmode, mapData.Mode newmode)
    {
        StartCoroutine(CycleViewOptionsRoutine());
    }

    /** Reveal the view menu. */
    private IEnumerator CycleViewOptionsRoutine()
    {
        ViewGroup.transform.localScale = Vector3.one;
        ViewGroup.transform.DOScale(Vector3.zero, 0.25f);
        ViewGroup.DOFade(0, 0.25f);

        while (DOTween.IsTweening(ViewGroup))
            yield return 0;

        ViewGroup.gameObject.SetActive(true);
        ViewGroup.transform.localScale = Vector3.zero;
        ViewGroup.transform.DOScale(1, 0.25f);
        ViewGroup.alpha = 0;
        ViewGroup.DOFade(1, 0.25f);
    }

}
