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
    public Button DiveMapButton;
    public Button NauticalMapButton;
    public Button SubSchematicButton;


    // Members
    // ------------------------------------------------------------

    /** Toggle indicators for 3d dive map. */
    private Graphic _diveMapOn;
    private Graphic _nauticalMapOn;
    private Graphic _subSchematicOn;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        StrategyMap.OnMapModeChanged += OnMapModeChanged;

        DiveMapButton.onClick.AddListener(ToggleDiveMap);
        NauticalMapButton.onClick.AddListener(ToggleNauticalMap);
        SubSchematicButton.onClick.AddListener(ToggleSubSchematic);

        _diveMapOn = DiveMapButton.GetComponentInChildrenNotMe<Image>(true);
        _nauticalMapOn = NauticalMapButton.GetComponentInChildrenNotMe<Image>(true);
        _subSchematicOn = SubSchematicButton.GetComponentInChildrenNotMe<Image>(true);
    }

    /** Updating. */
    private void Update()
    {
        _diveMapOn.gameObject.SetActive(StrategyMap.IsMap3D);
        _nauticalMapOn.gameObject.SetActive(StrategyMap.IsMap2D);
        _subSchematicOn.gameObject.SetActive(StrategyMap.IsSubSchematic);
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Toggle the 3d dive map. */
    public void ToggleDiveMap()
        { StrategyMap.ActivateMapMode(mapData.Mode.Mode3D); }

    /** Toggle the 2d nautical map. */
    public void ToggleNauticalMap()
        { StrategyMap.ActivateMapMode(mapData.Mode.Mode2D); }

    /** Toggle the sub schematic. */
    public void ToggleSubSchematic()
        { StrategyMap.ActivateMapMode(mapData.Mode.ModeSubSchematic); }


    // Private Methods
    // ------------------------------------------------------------

    /** Handle the map mode changing. */
    private void OnMapModeChanged(mapData.Mode oldMode, mapData.Mode newMode)
    {
        StartCoroutine(CycleViewOptionsRoutine(oldMode, newMode));
    }

    /** Reveal the view menu. */
    private IEnumerator CycleViewOptionsRoutine(mapData.Mode oldMode, mapData.Mode newMode)
    {
        if (HasViewOptionsInMode(oldMode))
        {
            ViewGroup.transform.localScale = Vector3.one;
            ViewGroup.transform.DOScale(Vector3.zero, 0.25f);
            ViewGroup.DOFade(0, 0.25f);
        }

        while (DOTween.IsTweening(ViewGroup))
            yield return 0;

        yield return new WaitForSeconds(0.25f);

        if (HasViewOptionsInMode(newMode))
        {
            ViewGroup.transform.localScale = Vector3.zero;
            ViewGroup.transform.DOScale(1, 0.25f);
            ViewGroup.alpha = 0;
            ViewGroup.DOFade(1, 0.25f);
        }
    }

    private static bool HasViewOptionsInMode(mapData.Mode mode)
    {
        return mode != mapData.Mode.ModeSubSchematic;
    }

}
