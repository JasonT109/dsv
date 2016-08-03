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
        if (!thrusterControl)
            thrusterControl = GameObject.FindWithTag("Inputs").GetComponent<widgetThrusterControl>();

        if (!t)
            t = GetComponent<DynamicText>();
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
