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

    public widgetPowerConduit[] AuxillaryL;
    public widgetPowerConduit[] AuxillaryR;

    public widgetPowerConduit[] AuxillarySystems;
    public widgetPowerConduit[] DivertedSystems;

    public float SmoothTime = 0.25f;

    /** Smoothing velocities for the various power conduits. */
    private readonly Dictionary<widgetPowerConduit, float> _smoothingVelocities 
        = new Dictionary<widgetPowerConduit, float>();


    private void Start()
    {
        LocateThrusterControl();
        UpdateConduits(false);
    }

    private void OnEnable()
    {
        LocateThrusterControl();
    }

    private void LocateThrusterControl()
    {
        // Locate the thruster control script (should be in a parent node).
        if (!Control)
            Control = ObjectFinder.FindInParents<widgetThrusterControl>(transform);
        if (!Control)
            Control = ObjectFinder.Find<widgetThrusterControl>();
    }

    private void Update()
    {
        UpdateConduits();
    }

    private void UpdateConduits(bool smoothed = true)
	{
        if (L1Conduit)
            SetConduitValue(L1Conduit, Mathf.Abs(Control.thrusterSideL1), smoothed);
        if (L2Conduit)
            SetConduitValue(L2Conduit, Mathf.Abs(Control.thrusterSideL2), smoothed);
        if (L3Conduit)
            SetConduitValue(L3Conduit, Mathf.Abs(Control.thrusterSideL3), smoothed);
        if (R1Conduit)
            SetConduitValue(R1Conduit, Mathf.Abs(Control.thrusterSideR1), smoothed);
        if (R2Conduit)
            SetConduitValue(R2Conduit, Mathf.Abs(Control.thrusterSideR2), smoothed);
        if (R3Conduit)
            SetConduitValue(R3Conduit, Mathf.Abs(Control.thrusterSideR3), smoothed);

        var mainL = Mathf.Abs(Control.thrusterMainL);
        var mainR = Mathf.Abs(Control.thrusterMainL);
        
        if (MainSharedConduit)
            SetConduitValue(MainSharedConduit, Mathf.Max(mainL, mainR), smoothed);

	    foreach (var conduit in MainLConduits)
            SetConduitValue(conduit, mainL, smoothed);

        foreach (var conduit in MainRConduits)
            SetConduitValue(conduit, mainR, smoothed);

        var divert = serverUtils.GetServerData("divertPowerToThrusters");
        if (DivertL)
        {
            SetConduitValue(DivertL, (divert * 0.01f) * mainL, smoothed);
            DivertL.gameObject.SetActive(divert > 0);

        }
        if (DivertR)
        {
            SetConduitValue(DivertR, (divert * 0.01f) * mainR, smoothed);
            DivertR.gameObject.SetActive(divert > 0);
        }

        if (AuxillaryL.Length > 0)
        {
            foreach (var conduit in AuxillaryL)
            {
                conduit.gameObject.SetActive(divert > 0);
                SetConduitValue(conduit, (divert * 0.01f) * mainL, smoothed);
            }
        }

        if (AuxillaryR.Length > 0)
        {
            foreach (var conduit in AuxillaryR)
            {
                conduit.gameObject.SetActive(divert > 0);
                SetConduitValue(conduit, (divert * 0.01f) * mainR, smoothed);
            }
        }

        if (DivertedSystems.Length > 0)
        {
            foreach (var conduit in DivertedSystems)
            {
                conduit.gameObject.SetActive(divert == 0);
                SetConduitValue(conduit, 100f, smoothed);
            }
        }

        if (AuxillarySystems.Length > 0)
        {
            foreach (var conduit in AuxillarySystems)
            {
                //conduit.gameObject.SetActive(divert == 0);
                SetConduitValue(conduit, 100f, smoothed);
            }
        }
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
        conduit.InactiveColor = Control.PowerGradient.Evaluate(value * 0.01f);
        conduit.ActiveColor = Control.PowerGradient.Evaluate(value * 0.01f);
    }
}
