using UnityEngine;
using System.Collections;

public class DCCStrategyMap : MonoBehaviour
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


    // [Header("UI")]

    /** The 3D map widget. */
    // public widget3DMap Map3d;


    // Public Methods
    // ------------------------------------------------------------

    /** Returns whether map is in contour viewing mode. */
    public bool IsContourMode()
        { return MapContoursButton.active; }

    /** Returns whether map is in 3d viewing mode. */
    public bool Is3DMode()
        { return Map3DButton.active; }

    /** Set contour mode on the 3d map. */
    public void ActivateContourMode(bool value)
        { MapContoursButton.Group.toggleButtonOn(MapContoursButton.gameObject); }

    /** Set 3d mode on the map. */
    public void Activate3DMode(bool value)
        { Map3DButton.Group.toggleButtonOn(Map3DButton.gameObject); }

}
