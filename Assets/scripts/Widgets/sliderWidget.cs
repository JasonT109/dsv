using UnityEngine;
using System.Collections;
using TouchScript.Behaviors;

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

        if (returnValue != prevValue)
            valueChanged = true;

        returnValue = v;
        prevValue = returnValue;
    }

    public void SetInputEnabled(bool value)
    {
        var transformer = GetComponent<Transformer>();
        if (transformer)
            transformer.enabled = value;
    }

    public void SetVisible(bool value)
    {
        var root = transform.parent.parent;
        var renderers = root.GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
            r.enabled = value;
    }

}
