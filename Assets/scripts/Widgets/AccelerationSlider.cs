using UnityEngine;
using System.Collections;

public class AccelerationSlider : MonoBehaviour {

    public sliderWidget Slider;

    // Use this for initialization
    void Start () 
    {

    }

    // Update is called once per frame
    void Update () 
    {
        if(Slider.valueChanged)
        {
            this.GetComponent<TextMesh>().text = Slider.returnValue.ToString("F1");
            GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
            Root.GetComponent<SubControl>().Acceleration = Slider.returnValue;
        }
    }
}
