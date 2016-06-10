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
    public bool battery = true;
    public bool oxygen = false;
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
                    if (battery)
                    {
                        sliderGroup.GetComponent<sliderGroup>().batterySliders[0].GetComponentInChildren<sliderWidget>().SetValue(slider1);
                        sliderGroup.GetComponent<sliderGroup>().batterySliders[1].GetComponentInChildren<sliderWidget>().SetValue(slider2);
                        sliderGroup.GetComponent<sliderGroup>().batterySliders[2].GetComponentInChildren<sliderWidget>().SetValue(slider3);
                        sliderGroup.GetComponent<sliderGroup>().batterySliders[3].GetComponentInChildren<sliderWidget>().SetValue(slider4);
                        sliderGroup.GetComponent<sliderGroup>().batterySliders[4].GetComponentInChildren<sliderWidget>().SetValue(slider5);
                        sliderGroup.GetComponent<sliderGroup>().batterySliders[5].GetComponentInChildren<sliderWidget>().SetValue(slider6);
                        sliderGroup.GetComponent<sliderGroup>().batterySliders[6].GetComponentInChildren<sliderWidget>().SetValue(slider7);
                    }
                    if (oxygen)
                    {
                        sliderGroup.GetComponent<sliderGroup>().oxygenSliders[0].GetComponentInChildren<sliderWidget>().SetValue(slider1);
                        sliderGroup.GetComponent<sliderGroup>().oxygenSliders[1].GetComponentInChildren<sliderWidget>().SetValue(slider2);
                        sliderGroup.GetComponent<sliderGroup>().oxygenSliders[2].GetComponentInChildren<sliderWidget>().SetValue(slider3);
                        sliderGroup.GetComponent<sliderGroup>().oxygenSliders[3].GetComponentInChildren<sliderWidget>().SetValue(slider4);
                        sliderGroup.GetComponent<sliderGroup>().oxygenSliders[4].GetComponentInChildren<sliderWidget>().SetValue(slider5);
                        sliderGroup.GetComponent<sliderGroup>().oxygenSliders[5].GetComponentInChildren<sliderWidget>().SetValue(slider6);
                        sliderGroup.GetComponent<sliderGroup>().oxygenSliders[6].GetComponentInChildren<sliderWidget>().SetValue(slider7);
                    }
                }
            }
        }
    }
}
