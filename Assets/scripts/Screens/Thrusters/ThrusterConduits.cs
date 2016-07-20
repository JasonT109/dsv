using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThrusterConduits : MonoBehaviour
{
    [Header("Controller")]

    public widgetThrusterControl Control;

    [Header("Conduits")]

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

    [Header("Appearance")]

    public Gradient PowerGradient;

    public float SmoothTime = 0.5f;

    private readonly Dictionary<widgetPowerConduit, float> _smoothingVelocities 
        = new Dictionary<widgetPowerConduit, float>();


    private void Update()
	{
        SetConduitValue(L1Conduit, Mathf.Abs(Control.thrusterSideL1));
        SetConduitValue(L2Conduit, Mathf.Abs(Control.thrusterSideL2));
        SetConduitValue(L3Conduit, Mathf.Abs(Control.thrusterSideL3));

        SetConduitValue(R1Conduit, Mathf.Abs(Control.thrusterSideR1));
        SetConduitValue(R2Conduit, Mathf.Abs(Control.thrusterSideR2));
        SetConduitValue(R3Conduit, Mathf.Abs(Control.thrusterSideR3));

        var mainL = Mathf.Abs(Control.thrusterMainL);
        var mainR = Mathf.Abs(Control.thrusterMainL);

        SetConduitValue(MainSharedConduit, Mathf.Max(mainL, mainR));

	    foreach (var conduit in MainLConduits)
            SetConduitValue(conduit, mainL);

        foreach (var conduit in MainRConduits)
            SetConduitValue(conduit, mainR);

        // TODO: Divert conduit logic.
    }

    private void SetConduitValue(widgetPowerConduit conduit, float target)
    {
        // Apply smoothing to the conduit values.
        if (!_smoothingVelocities.ContainsKey(conduit))
            _smoothingVelocities[conduit] = 0;
        var velocity = _smoothingVelocities[conduit];
        var value = Mathf.SmoothDamp(conduit.Value, target, ref velocity, SmoothTime);
        _smoothingVelocities[conduit] = velocity;

        conduit.Value = value;
        conduit.InactiveColor = PowerGradient.Evaluate(value * 0.01f);
        conduit.ActiveColor = PowerGradient.Evaluate(value * 0.01f);
    }
}
