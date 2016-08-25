using UnityEngine;
using System.Collections;
using Meg.Maths;

public class widgetSliderFill : MonoBehaviour {

    public GameObject sliderWidget;

    Renderer r;
    Material m;

	// Use this for initialization
	void Start ()
    {
        r = gameObject.GetComponent<Renderer>();
        m = r.material;
        m.SetFloat("_sliderValue", graphicsMaths.remapValue(sliderWidget.GetComponent<sliderWidget>().returnValue, sliderWidget.GetComponent<sliderWidget>().minValue, sliderWidget.GetComponent<sliderWidget>().maxValue, 0, 1));
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (sliderWidget.GetComponent<sliderWidget>().valueChanged)
        {
            m.SetFloat("_sliderValue", graphicsMaths.remapValue(sliderWidget.GetComponent<sliderWidget>().returnValue, sliderWidget.GetComponent<sliderWidget>().minValue, sliderWidget.GetComponent<sliderWidget>().maxValue, 0, 1));
        }
	}
}
