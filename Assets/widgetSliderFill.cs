using UnityEngine;
using System.Collections;

public class widgetSliderFill : MonoBehaviour {

    public GameObject sliderWidget;

    Renderer r;
    Material m;

	// Use this for initialization
	void Start ()
    {
        r = gameObject.GetComponent<Renderer>();
        m = r.material;
        m.SetFloat("_sliderValue", sliderWidget.GetComponent<sliderWidget>().returnValue);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (sliderWidget.GetComponent<sliderWidget>().valueChanged)
        {
            m.SetFloat("_sliderValue", sliderWidget.GetComponent<sliderWidget>().returnValue);
        }
	}
}
