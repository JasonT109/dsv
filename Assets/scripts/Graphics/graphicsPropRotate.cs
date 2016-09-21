using UnityEngine;
using System.Collections;
using Meg.Networking;

public class graphicsPropRotate : MonoBehaviour {

    public GameObject gauge;
    public float multiplier = 30.0f;

    public widgetThrusterControl Control;
    public widgetThrusterControl.ThrusterId Thruster;

	
	// Update is called once per frame
	void Update()
	{
	    var value = 0f;
        if (gauge)
            value = gauge.GetComponent<digital_gauge>().value;
        else if (Control)
            value = Control.GetThrusterLevel(Thruster);

        gameObject.transform.Rotate(0, 0, -(value * Time.deltaTime)* multiplier);
	}

}
