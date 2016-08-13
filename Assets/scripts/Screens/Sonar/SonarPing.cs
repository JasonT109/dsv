using UnityEngine;
using System.Collections;
using Meg.Networking;

public class SonarPing : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Label for this ping. */
    public widgetText NameLabel;

    /** Label for depth information. */
    public widgetText DepthLabel;

    /** Parent sonar ping manager. */
    public SonarPings Pings { get; set; }

    /** Vessel represented by this ping. */
    public vesselData.Vessel Vessel { get; set; }


    // Private Properties
    // ------------------------------------------------------------

    /** Vessel data. */
    private vesselData VesselData
        { get { return serverUtils.VesselData; } }


    // Public Methods
    // ------------------------------------------------------------

    /** Updating. */
    public void Refresh()
    {
        var p = VesselData.GetSonarPosition(Vessel.Id, Pings.Type);
        var player = VesselData.PlayerVessel;
        var visible = Vessel.VisibleOnSonar 
            && (Vessel.Id != player) 
            && (p.magnitude <= 1);

        transform.localPosition = p * Pings.SonarToRootScale;
        transform.rotation = Quaternion.identity;

        gameObject.SetActive(visible);
        NameLabel.Text = Vessel.Name.ToUpper();
        DepthLabel.Text = string.Format("{0:+0;-#}m", Vessel.Depth - VesselData.GetDepth(player));
    }

}
