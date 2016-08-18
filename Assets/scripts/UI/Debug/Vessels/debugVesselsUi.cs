using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Meg.Networking;
using UnityEngine.UI;


/**
 * Interface logic for the Vessel setup screen.
 * 
 * Allows users to modify the positions, visibility and movements of Vessels (subs)
 * in the simulation.  Users can also add and remove vessels from the simulation,
 * set their names, icons, and so on.
 * 
 * The logic for editing properties of the selected vessel lives in debugVesselPropertiesUi.
 * 
 * Each vessel is represented by a 'ping' on the map viewport.  The ping is a customized
 * version of the SonarPing as used in the long range Sonar screen, with the custom behaviour
 * being defined in debugVesselPingUi.
 * 
 */

public class debugVesselsUi : Singleton<debugVesselsUi>
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Container that will hold vessel renderers. */
    public Transform VesselContainer;

    /** Button for adding a vessel. */
    public Button AddButton;

    /** Button for removing a vessel. */
    public Button RemoveButton;

    /** Button for clearing extra vessels. */
    public Button ClearButton;

    /** Properties interface. */
    public debugVesselPropertiesUi Properties;


    [Header("Prefabs")]

    /** The vessel item renderer prefab. */
    public debugVesselUi VesselUiPrefab;

    /** The selected vessel. */
    public vesselData.Vessel Selected
    {
        get { return _selected;  }
        set { Select(value);}
    }



    // Private Properties
    // ------------------------------------------------------------

    /** The server's vessel data. */
    private vesselData VesselData
        { get { return serverUtils.VesselData; } }

    /** Whether any vessels can be removed. */
    private bool CanRemoveVessels
        { get { return VesselData.VesselCount > vesselData.BaseVesselCount; } }


    // Members
    // ------------------------------------------------------------

    /** Vessel renderers. */
    private readonly List<debugVesselUi> _vessels = new List<debugVesselUi>();

    /** Selected vessel. */
    private vesselData.Vessel _selected;


    // Unity Methods
    // ------------------------------------------------------------

    private void Awake()
    {
        if (!VesselContainer)
            VesselContainer = transform;
    }

    private void Update()
    {
        if (!VesselData)
            return;

        if (Selected.Id <= 0)
            Selected = VesselData.GetVessel(VesselData.PlayerVessel);

        UpdateVessels();
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Set the selected vessel. */
    public void Select(vesselData.Vessel vessel)
    {
        _selected = vessel;
        Properties.Vessel = vessel;
    }

    /** Add a vessel to the vessel list. */
    public void AddVessel()
    {
        var playerDepth = VesselData.GetDepth(VesselData.PlayerVessel);
        serverUtils.PostAddVessel(new vesselData.Vessel
        {
            Name = "Unknown",
            Position = new Vector3(0, 0, playerDepth),
            ColorOnMap = vesselData.DefaultColorOnMap,
            ColorOnSonar = vesselData.DefaultColorOnSonar
        });
    }

    /** Remove the last vessel in list. */
    public void RemoveVessel()
    {
        if (!CanRemoveVessels)
            return;

        var vesselName = VesselData.GetName(VesselData.LastVessel);
        DialogManager.Instance.ShowYesNo("REMOVE LAST VESSEL?",
            string.Format("Are you sure you wish to remove the last vessel '{0}'?", vesselName),
            result =>
            {
                if (result != DialogYesNo.DialogResult.Yes)
                    return;

                serverUtils.PostRemoveLastVessel();
                Select(VesselData.GetVessel(VesselData.LastVessel));
            });
    }

    /** Clear extra vessels. */
    public void ClearExtraVessels()
    {
        if (!CanRemoveVessels)
            return;

        DialogManager.Instance.ShowYesNo("REMOVE ALL EXTRA VESSELS?",
            "Are you sure you wish to remove all extra vessels from view?",
            result =>
            {
                if (result != DialogYesNo.DialogResult.Yes)
                    return;

                serverUtils.PostClearExtraVessels();
                Select(VesselData.GetVessel(VesselData.LastVessel));
            });

    }


    // Private Methods
    // ------------------------------------------------------------

    private void UpdateVessels()
    {
        AddButton.interactable = true;
        RemoveButton.interactable = CanRemoveVessels;
        ClearButton.interactable = CanRemoveVessels;

        var vessels = VesselData.Vessels;
        var index = 0;
        foreach (var v in vessels)
        {
            var ui = GetVesselUi(index++);
            ui.Vessels = this;
            ui.Vessel = v;
        }

        for (var i = 0; i < _vessels.Count; i++)
            _vessels[i].gameObject.SetActive(i < index);
    }

    private debugVesselUi GetVesselUi(int i)
    {
        if (i >= _vessels.Count)
        {
            var vessel = Instantiate(VesselUiPrefab);
            vessel.transform.SetParent(VesselContainer, false);
            _vessels.Add(vessel);
        }

        return _vessels[i];
    }
}
