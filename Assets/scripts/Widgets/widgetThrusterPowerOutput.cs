using UnityEngine;
using System.Collections;

public class widgetThrusterPowerOutput : MonoBehaviour
{

    public widgetThrusterControl thrusterControl;
    public DynamicText t;
    public widgetThrusterControl.ThrusterId Thruster;

    private float updateTick = 0.2f;
    private float updateTime = 0;

    void Start ()
    {
        LocateThrusterControl();

        if (!t)
            t = GetComponent<DynamicText>();
	}

    private void LocateThrusterControl()
    {
        // Locate the thruster control script (should be in a parent node).
        if (!thrusterControl)
            thrusterControl = ObjectFinder.FindInParents<widgetThrusterControl>(transform);
        if (!thrusterControl)
            thrusterControl = ObjectFinder.Find<widgetThrusterControl>();
    }

    void Update ()
    {
        string valueText = thrusterControl.GetThrusterPowerOutput(Thruster).ToString("N0");
        
        if (Time.time > updateTime)
        {
            updateTime = Time.time + updateTick;
            t.SetText(valueText);
        }
        
	}
}
