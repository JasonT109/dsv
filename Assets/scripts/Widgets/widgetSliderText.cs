using UnityEngine;

public class widgetSliderText : MonoBehaviour
{
    public bool isDynamicText;
    public GameObject sliderWidget;
    public string suffix = "%";

	void Start ()
    {
        if (isDynamicText)
        {
            gameObject.GetComponent<DynamicText>().SetText(sliderWidget.GetComponent<sliderWidget>().returnValue.ToString("n0") + suffix);
        }
        else
        {
            gameObject.GetComponent<TextMesh>().text = sliderWidget.GetComponent<sliderWidget>().returnValue.ToString() + suffix;
        }
    }

	void Update ()
    {
        if (isDynamicText)
        {
            if (sliderWidget.GetComponent<sliderWidget>().valueChanged)
            {
                gameObject.GetComponent<DynamicText>().SetText(sliderWidget.GetComponent<sliderWidget>().returnValue.ToString("n0") + suffix);
            }
        }
        else
        {
            if (sliderWidget.GetComponent<sliderWidget>().valueChanged)
            {
                gameObject.GetComponent<TextMesh>().text = sliderWidget.GetComponent<sliderWidget>().returnValue.ToString("n0") + suffix;
            }
        }

	}
}
