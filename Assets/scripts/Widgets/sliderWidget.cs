using UnityEngine;
using System.Collections;

public class sliderWidget : MonoBehaviour, ValueSettable
{
    public float minValue;
    public float maxValue;
    public float returnValue;
    public bool valueChanged = false;
    public float prevValue;
    private float sliderMin = -1f;
    private float sliderMax = 1f;
    private float carotPos = 0f;
	
	// Update is called once per frame
	void Update ()
    {
        if (returnValue != prevValue)
        {
            valueChanged = true;
        }
        else
        {
            valueChanged = false;
        }
        //= (X-A)/(B-A) * (D-C) + C
        prevValue = returnValue;
        returnValue = (transform.localPosition.x - sliderMin) / (sliderMax - sliderMin) * (maxValue - minValue) + minValue;
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, sliderMin, sliderMax), 0, 0);
        
    }

    public void SetValue(float v)
    {
        carotPos = (v - minValue) / (maxValue - minValue) * (sliderMax - sliderMin) + sliderMin;
        transform.localPosition = new Vector3(Mathf.Clamp(carotPos, sliderMin, sliderMax), 0, 0);
    }
}
