using UnityEngine;
using System.Collections;

public class MegTurnSpeedSlider : MonoBehaviour 
{

    public sliderWidget Slider;

    void Start()
    {

    }

    void Update()
    {
        if(Slider.valueChanged)
        {
            this.GetComponent<TextMesh>().text = Slider.returnValue.ToString("F2");
            GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
            Root.GetComponent<SonarData>().MegTurnSpeed = Slider.returnValue;
        }
    }
}
