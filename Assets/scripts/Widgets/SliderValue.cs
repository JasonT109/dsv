using UnityEngine;
using System.Collections;

public class SliderValue : MonoBehaviour 
{
    public bool isDynamicText;
    public bool isInt;
    public bool isFloat1;
    public bool isFloat2;
    public bool isFloat3;

    public sliderWidget Slider;
    private float updateTimer = 0f;
    private float updateTick = 0.2f;

    void OnEnable()
    {
        updateTimer = Time.time;
    }

    void Update () 
    {
        if (Time.time < updateTimer)
            return;

        updateTimer = Time.time + updateTick;

        if (isDynamicText)
        {
            if (isInt)
            {
                this.GetComponent<DynamicText>().SetText(Slider.returnValue.ToString("N0"));
            }
            if (isFloat1)
            {
                this.GetComponent<DynamicText>().SetText(Slider.returnValue.ToString("F1"));
            }

            if (isFloat2)
            {
                this.GetComponent<DynamicText>().SetText(Slider.returnValue.ToString("F2"));
            }

            if (isFloat3)
            {
                this.GetComponent<DynamicText>().SetText(Slider.returnValue.ToString("F3"));
            }
        }
        else
        {
            if (isInt)
            {
                this.GetComponent<TextMesh>().text = Slider.returnValue.ToString("N0");
            }
            if (isFloat1)
            {
                this.GetComponent<TextMesh>().text = Slider.returnValue.ToString("F1");
            }

            if (isFloat2)
            {
                this.GetComponent<TextMesh>().text = Slider.returnValue.ToString("F2");
            }

            if (isFloat3)
            {
                this.GetComponent<TextMesh>().text = Slider.returnValue.ToString("F3");
            }
        }
	}
}
