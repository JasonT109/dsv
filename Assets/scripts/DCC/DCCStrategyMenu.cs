using UnityEngine;
using System.Collections;
using DG.Tweening;
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

    /** Toggle indicator for contour mode. */
    public Graphic MapContoursOn;

    /** Toggle indicator for 3d mode. */
    public Graphic Map3DOn;

    /** Toggle indicator for vessel labels. */
    public Graphic VesselLabelsOn;

    /** Toggle indicator for nautical map. */
    public Graphic NauticalMapOn;


    // Private Properties
    // ------------------------------------------------------------

    /** Whether view menu is visible. */
    private bool _viewMenuVisible = true;
    private bool ViewMenuVisible
    {
        get { return _viewMenuVisible; }
        set
        {
            if (_viewMenuVisible == value)
                return;

            _viewMenuVisible = value;
            StopAllCoroutines();
            StartCoroutine(value ? ShowViewMenuRoutine() : HideViewMenuRoutine());
        }
    }


    // Unity Methods
    // ------------------------------------------------------------

    /** Updating. */
    private void Update()
    {
        MapContoursOn.gameObject.SetActive(Map3D.IsContourMode);
        Map3DOn.gameObject.SetActive(Map3D.Is3DMode);
        VesselLabelsOn.gameObject.SetActive(NavSubPins.Instance.HasLabels);
        NauticalMapOn.gameObject.SetActive(StrategyMap.IsMap2d);
        ViewMenuVisible = StrategyMap.IsMap3d;
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

    /** Toggle the nautical map. */
    public void ToggleNauticalMap()
        { StrategyMap.ToggleMap2d(); }


    // Private Methods
    // ------------------------------------------------------------

    /** Reveal the view menu. */
    private IEnumerator ShowViewMenuRoutine()
    {
        ViewGroup.gameObject.SetActive(true);
        ViewGroup.transform.localScale = Vector3.zero;
        ViewGroup.transform.DOScale(1, 0.25f);
        ViewGroup.alpha = 0;
        ViewGroup.DOFade(1, 0.25f);

        yield return 0;
    }

    /** Hide the view menu. */
    private IEnumerator HideViewMenuRoutine()
    {
        yield return new WaitForSeconds(0.25f);

        ViewGroup.transform.localScale = Vector3.one;
        ViewGroup.transform.DOScale(Vector3.zero, 0.25f);
        ViewGroup.DOFade(0, 0.25f);

        while (DOTween.IsTweening(ViewGroup))
            yield return 0;

        ViewGroup.gameObject.SetActive(false);
    }

}
