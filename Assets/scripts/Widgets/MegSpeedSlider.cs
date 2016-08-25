using UnityEngine;
using System.Collections;

public class MegSpeedSlider : MonoBehaviour {

    //public string linkDataString = "depth";
    //public float updateTick = 0.2f;
    //private float nextUpdate = 0;

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
            Root.GetComponent<SonarData>().MegSpeed = Slider.returnValue;
        }
    }
}
