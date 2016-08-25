using UnityEngine;

public class widgetSliderText : MonoBehaviour
{
    public bool isDynamicText;
    public GameObject sliderWidget;
    public string suffix = "%";

    private float updateTimer = 0f;
    private float updateTick = 0.2f;

    void OnEnable ()
    {
        updateTimer = Time.time;
    }

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
        if (Time.time < updateTimer)
            return;

        updateTimer = Time.time + updateTick;

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
