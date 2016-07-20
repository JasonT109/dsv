using UnityEngine;
using System.Collections;

public class ThrusterConduits : MonoBehaviour
{
    public widgetThrusterControl Control;

    public widgetPowerConduit L1Conduit;
    public widgetPowerConduit L2Conduit;
    public widgetPowerConduit L3Conduit;

    public widgetPowerConduit R1Conduit;
    public widgetPowerConduit R2Conduit;
    public widgetPowerConduit R3Conduit;

    public widgetPowerConduit MainSharedConduit;
    public widgetPowerConduit[] MainLConduits;
    public widgetPowerConduit[] MainRConduits;

    public widgetPowerConduit DivertL;
    public widgetPowerConduit DivertR;

	private void Update()
	{
        L1Conduit.Value = Mathf.Abs(Control.thrusterSideL1);
        L2Conduit.Value = Mathf.Abs(Control.thrusterSideL2);
        L3Conduit.Value = Mathf.Abs(Control.thrusterSideL3);

        R1Conduit.Value = Mathf.Abs(Control.thrusterSideR1);
        R2Conduit.Value = Mathf.Abs(Control.thrusterSideR2);
        R3Conduit.Value = Mathf.Abs(Control.thrusterSideR3);

        var mainL = Mathf.Abs(Control.thrusterMainL);
        var mainR = Mathf.Abs(Control.thrusterMainL);

        MainSharedConduit.Value = Mathf.Max(mainL, mainR);

	    foreach (var conduit in MainLConduits)
	        conduit.Value = mainL;

        foreach (var conduit in MainRConduits)
            conduit.Value = mainR;

        // TODO: Divert conduit logic.
    }
}
