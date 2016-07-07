using UnityEngine;

public class widgetSliderText : MonoBehaviour
{
    public GameObject sliderWidget;
    public string suffix = "%";

	void Start ()
    {
        gameObject.GetComponent<TextMesh>().text = sliderWidget.GetComponent<sliderWidget>().returnValue.ToString() + suffix;
    }

	void Update ()
    {
	    if (sliderWidget.GetComponent<sliderWidget>().valueChanged)
        {
            gameObject.GetComponent<TextMesh>().text = sliderWidget.GetComponent<sliderWidget>().returnValue.ToString("n0") + suffix;
        }
	}
}
