using UnityEngine;
using System.Collections;

public class SliderValue : MonoBehaviour 
{
    public bool isFloat1;
    public bool isFloat2;
    public bool isFloat3;

    public sliderWidget Slider;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(isFloat1)
        {
            this.GetComponent<TextMesh>().text = Slider.returnValue.ToString("F1");
        }

        if(isFloat2)
        {
            this.GetComponent<TextMesh>().text = Slider.returnValue.ToString("F2");
        }

        if(isFloat3)
        {
            this.GetComponent<TextMesh>().text = Slider.returnValue.ToString("F3");
        }
	}
}
