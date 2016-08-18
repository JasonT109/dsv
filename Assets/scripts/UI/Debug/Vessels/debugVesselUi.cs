using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Meg.Networking;

/**
 * List entry interface for a single vessel (non-spatial).
 * Allows user to select a vessel for editing, and also control
 * that vessel's visibility on map/sonar in a quick way. 
 */

public class debugVesselUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Name label for the vessel. */
    public Text NameLabel;

    /** The vessel's backdrop graphic. */
    public Graphic Backdrop;

    /** The vessel's on map button. */
    public Toggle OnMapToggle;

    /** The vessel's on sonar toggle button. */
    public Toggle OnSonarToggle;


    /** The vessel being displayed. */
    public vesselData.Vessel Vessel { get; set; }

    /** Parent vessel list. */
    public debugVesselsUi Vessels { get; set; }


    [Header("Configuration")]

    /** Color to use for backdrop when vessel is inactive. */
    public Color BackdropInactiveColor;

    /** Color to use for backdrop when vessel is selected. */
    public Color BackdropSelectedColor;

    /** Color to use for backdrop when vessel is the player vessel. */
    public Color BackdropPlayerColor;

    /** Color to use for backdrop when vessel is the player vessel and is selected. */
    public Color BackdropPlayerSelectedColor;


    // Private Properties
    // ------------------------------------------------------------

    /** The server's vessel data. */
    private vesselData VesselData
        { get { return serverUtils.VesselData; } }


    // Members
    // ------------------------------------------------------------

    /** Whether ui is being updated. */
    private bool _updating;


    // Public Methods
    // ------------------------------------------------------------

    /** Toggle the vessel's map visibility. */
    public void ToggleOnMap()
    {
        if (!_updating)
            VesselData.SetOnMap(Vessel.Id, !Vessel.OnMap);
    }

    /** Toggle the vessel's sonar visibility. */
    public void ToggleOnSonar()
    {
        if (!_updating)
            VesselData.SetOnSonar(Vessel.Id, !Vessel.OnSonar);
    }

    /** Select this vessel. */
    public void Select()
        { Vessels.Selected = Vessel; }



    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        if (VesselData)
            UpdateVesselUi();
    }

    /** Updating. */
    private void Update()
    {
        if (VesselData)
            UpdateVesselUi();
    }


    // Private Methods
    // ------------------------------------------------------------

    private void UpdateVesselUi()
    {
        _updating = true;

        var isPlayer = Vessel.Id == VesselData.PlayerVessel;
        var c = isPlayer ? BackdropPlayerColor : BackdropInactiveColor;
        if (Vessels.Selected.Id == Vessel.Id)
            c = isPlayer ? BackdropPlayerSelectedColor : BackdropSelectedColor;

        Backdrop.color = c;

        OnMapToggle.isOn = Vessel.OnMap;
        OnSonarToggle.isOn = Vessel.OnSonar;
        NameLabel.text = VesselData.GetDebugName(Vessel.Id);

        _updating = false;
    }

}
