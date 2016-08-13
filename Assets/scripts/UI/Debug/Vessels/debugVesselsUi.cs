using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Meg.Networking;
using UnityEngine.UI;

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


    [Header("Prefabs")]

    /** The vessel item renderer prefab. */
    public debugVesselUi VesselUiPrefab;



    /** The selected vessel. */
    public vesselData.Vessel Selected { get; set; }



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


    // Unity Methods
    // ------------------------------------------------------------

    private void Awake()
    {
        if (!VesselContainer)
            VesselContainer = transform;
    }

    private void Update()
    {
        if (VesselData)
            UpdateVessels();
    }



    // Public Methods
    // ------------------------------------------------------------

    /** Set the selected vessel. */
    public void Select(vesselData.Vessel vessel)
    {
        Selected = vessel;
    }

    /** Add a vessel to the vessel list. */
    public void AddVessel()
    {
        serverUtils.PostAddVessel(new vesselData.Vessel
            { Name = "Unknown", OnSonar = true });
    }

    /** Remove the last vessel in list. */
    public void RemoveVessel()
    {
        if (CanRemoveVessels)
            serverUtils.PostRemoveLastVessel();
    }

    /** Clear extra vessels. */
    public void ClearExtraVessels()
    {
        if (CanRemoveVessels)
            serverUtils.PostClearExtraVessels();
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
