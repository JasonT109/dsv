using UnityEngine;
using System.Collections;

public class widgetBatteryPreset : MonoBehaviour {

    public float slider1 = 1f;
    public float slider2 = 1f;
    public float slider3 = 1f;
    public float slider4 = 1f;
    public float slider5 = 1f;
    public float slider6 = 1f;
    public float slider7 = 1f;
    public GameObject sliderGroup;

    // Update is called once per frame
    void Update ()
    {
        var buttonScript = gameObject.GetComponent<buttonControl>();
        if (buttonScript)
        {
            if (buttonScript.pressed)
            {
                //set the sliders
                if (sliderGroup)
                {
                    sliderGroup.GetComponent<sliderGroup>().sliders[0].GetComponentInChildren<sliderWidget>().SetValue(slider1);
                    sliderGroup.GetComponent<sliderGroup>().sliders[1].GetComponentInChildren<sliderWidget>().SetValue(slider2);
                    sliderGroup.GetComponent<sliderGroup>().sliders[2].GetComponentInChildren<sliderWidget>().SetValue(slider3);
                    sliderGroup.GetComponent<sliderGroup>().sliders[3].GetComponentInChildren<sliderWidget>().SetValue(slider4);
                    sliderGroup.GetComponent<sliderGroup>().sliders[4].GetComponentInChildren<sliderWidget>().SetValue(slider5);
                    sliderGroup.GetComponent<sliderGroup>().sliders[5].GetComponentInChildren<sliderWidget>().SetValue(slider6);
                    sliderGroup.GetComponent<sliderGroup>().sliders[6].GetComponentInChildren<sliderWidget>().SetValue(slider7);
                }
            }
        }
    }
}
