using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Meg.Networking;

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

    public widgetPowerConduit Systems;
    public widgetPowerConduit DivertL;
    public widgetPowerConduit DivertR;

    [Header("Appearance")]

    public Gradient PowerGradient;

    public float SmoothTime = 0.25f;

    /** Smoothing velocities for the various power conduits. */
    private readonly Dictionary<widgetPowerConduit, float> _smoothingVelocities 
        = new Dictionary<widgetPowerConduit, float>();


    private void Start()
    {
        UpdateConduits(false);
    }

    private void Update()
    {
        UpdateConduits();
    }

    private void UpdateConduits(bool smoothed = true)
	{
        SetConduitValue(L1Conduit, Mathf.Abs(Control.thrusterSideL1), smoothed);
        SetConduitValue(L2Conduit, Mathf.Abs(Control.thrusterSideL2), smoothed);
        SetConduitValue(L3Conduit, Mathf.Abs(Control.thrusterSideL3), smoothed);

        SetConduitValue(R1Conduit, Mathf.Abs(Control.thrusterSideR1), smoothed);
        SetConduitValue(R2Conduit, Mathf.Abs(Control.thrusterSideR2), smoothed);
        SetConduitValue(R3Conduit, Mathf.Abs(Control.thrusterSideR3), smoothed);

        var mainL = Mathf.Abs(Control.thrusterMainL);
        var mainR = Mathf.Abs(Control.thrusterMainL);

        SetConduitValue(MainSharedConduit, Mathf.Max(mainL, mainR), smoothed);

	    foreach (var conduit in MainLConduits)
            SetConduitValue(conduit, mainL, smoothed);

        foreach (var conduit in MainRConduits)
            SetConduitValue(conduit, mainR, smoothed);

        var divert = serverUtils.GetServerData("divertPowerToThrusters");
        SetConduitValue(DivertL, (divert * 0.01f) * mainL, smoothed);
        SetConduitValue(DivertR, (divert * 0.01f) * mainR, smoothed);

        DivertL.gameObject.SetActive(divert > 0);
        DivertR.gameObject.SetActive(divert > 0);
    }

    private void SetConduitValue(widgetPowerConduit conduit, float target, bool smoothed)
    {
        // Apply smoothing to the conduit values.
        if (!_smoothingVelocities.ContainsKey(conduit))
            _smoothingVelocities[conduit] = 0;
        var velocity = _smoothingVelocities[conduit];
        var value = smoothed ? Mathf.SmoothDamp(conduit.Value, target, ref velocity, SmoothTime) : target;
        _smoothingVelocities[conduit] = velocity;

        conduit.Value = value;
        conduit.InactiveColor = PowerGradient.Evaluate(value * 0.01f);
        conduit.ActiveColor = PowerGradient.Evaluate(value * 0.01f);
    }
}
