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

    /** The (old) contour mode button. */
    public buttonControl MapContoursButton;

    /** The (old) 3d map mode button. */
    public buttonControl Map3DButton;


    [Header("UI")]

    /** Toggle indicator for contour mode. */
    public Graphic MapContoursOn;

    /** Toggle indicator for 3d mode. */
    public Graphic Map3DOn;


    // Unity Methods
    // ------------------------------------------------------------

    /** Updating. */
    private void Update()
    {
        MapContoursOn.gameObject.SetActive(IsContourMode());
        Map3DOn.gameObject.SetActive(Is3DMode());
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Returns whether map is in contour viewing mode. */
    public bool IsContourMode()
        { return MapContoursButton.active; }

    /** Returns whether map is in 3d viewing mode. */
    public bool Is3DMode()
        { return Map3DButton.active; }

    /** Set contour mode on the 3d map. */
    public void ActivateContourMode()
        { MapContoursButton.Group.toggleButtonOn(MapContoursButton.gameObject); }

    /** Set 3d mode on the map. */
    public void Activate3DMode()
        { Map3DButton.Group.toggleButtonOn(Map3DButton.gameObject); }

}
