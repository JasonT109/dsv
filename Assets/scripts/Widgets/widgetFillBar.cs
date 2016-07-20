using UnityEngine;
using Meg.Maths;
using Meg.Networking;
using System.Collections;

public class widgetFillBar : MonoBehaviour
{
    public string serverValue = "battery";
    public float serverMaxValue = 100f;
    public float serverMinValue = 0f;
    public float fillMaxValue = 1f;
    public bool setHeight = false;
    public bool setWidth = false;
    public Color defaultColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    public Color lowColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    public Color highColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    [Range(0.01f, 0.5f)]
    public float colorBlendThreshold = 0.25f;

    void Start ()
    {
        float value;
        value = graphicsMaths.remapValue(serverUtils.GetServerData(serverValue), serverMinValue, serverMaxValue, 0, fillMaxValue);

        if (setWidth)
            gameObject.GetComponent<graphicsSlicedMesh>().Width = value;

        if (setHeight)
            gameObject.GetComponent<graphicsSlicedMesh>().Height = value;
    }
	
	void Update ()
    {
        float value;
        value = graphicsMaths.remapValue(serverUtils.GetServerData(serverValue), serverMinValue, serverMaxValue, 0, fillMaxValue);

        if (setWidth)
            gameObject.GetComponent<graphicsSlicedMesh>().Width = value;

        if (setHeight)
            gameObject.GetComponent<graphicsSlicedMesh>().Height = value;

        //if value is under the threshold lerp to lowColor
        if (value / fillMaxValue < colorBlendThreshold)
            gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", Color.Lerp(defaultColor, lowColor, graphicsMaths.remapValue(value / fillMaxValue, 0, fillMaxValue * colorBlendThreshold, 1, 0)));

        //if value is above the threshold lerp to lowColor
        else if (value / fillMaxValue > 1 - colorBlendThreshold)
            gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", Color.Lerp(defaultColor, highColor, graphicsMaths.remapValue(value / fillMaxValue, fillMaxValue - fillMaxValue * colorBlendThreshold, fillMaxValue, 0, 1)));

        //else set to default color
        else
            gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", defaultColor);
    }
}
