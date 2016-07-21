using UnityEngine;
using System.Collections;

public class ThrusterPowerLabel : widgetText
{
    public widgetThrusterControl Control;
    public widgetThrusterControl.ThrusterId Thruster;

    public Renderer Frame;

	void Update()
	{
	    var level = Control.GetThrusterLevel(Thruster);
        var color = Control.PowerGradient.Evaluate(Mathf.Abs(level * 0.01f));

        Text = (Mathf.Abs(level * 1f)).ToString("n0") + "%";
	    Color = color;

	    if (Frame)
	        Frame.material.SetColor("_TintColor", color);
	}

}
