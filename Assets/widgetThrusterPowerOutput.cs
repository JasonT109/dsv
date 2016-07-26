using UnityEngine;
using System.Collections;

public class widgetThrusterPowerOutput : MonoBehaviour
{

    public widgetThrusterControl thrusterControl;
    public DynamicText t;
    public widgetThrusterControl.ThrusterId Thruster;

    void Start ()
    {
        if (!thrusterControl)
            thrusterControl = GameObject.FindWithTag("Inputs").GetComponent<widgetThrusterControl>();

        if (!t)
            t = GetComponent<DynamicText>();
	}
	
	void Update ()
    {
        t.SetText(thrusterControl.GetThrusterPowerOutput(Thruster).ToString("N0"));
	}
}
