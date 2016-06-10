using UnityEngine;
using System.Collections;
using Meg.Networking;

public class clientCalcValues : MonoBehaviour
{
    public float pressureResult;
    public float psiResult;
    public float waterTempResult;
    public float[] depthValues;
    public float[] pressureValues;
    public float[] waterTempValues;

    public float calcFromDepth(float d, string valueType)
    {
        float p;
        float t;
        int nearestIndex = 0;
        for (int i = 0; i < depthValues.Length; i++) //find the nearest index
        {
            if (d > depthValues[i])
            {
                nearestIndex = i;
            }
        }
        if (d > depthValues[depthValues.Length - 1]) //if d is greater than value in last index
        {
            p = pressureValues[nearestIndex];
            t = waterTempValues[nearestIndex];
        }
        else
        {
            if (d == depthValues[nearestIndex]) //if value matches value at index
            {
                p = pressureValues[nearestIndex];
                t = waterTempValues[nearestIndex];
            }
            else 
            {
                float dp;
                //get position of value between index and next index values
                dp = ((d - depthValues[nearestIndex]) / (depthValues[nearestIndex + 1] - depthValues[nearestIndex]));
                p = (pressureValues[nearestIndex + 1] - pressureValues[nearestIndex]) * dp;
                p += pressureValues[nearestIndex];
                t = (waterTempValues[nearestIndex + 1] - waterTempValues[nearestIndex]) * dp;
                t += waterTempValues[nearestIndex];
            }
        }
        if (valueType == "pressure")
        {
            return p;
        }
        if (valueType == "water")
        {
            return t;
        }
        else
        {
            return -1;
        }
    }

	// Use this for initialization
	void Start ()
    {
	    if (depthValues.Length > 0)
        {
            float value = serverUtils.GetServerData("depth");
            pressureResult = calcFromDepth(value, "pressure");
            waterTempResult = calcFromDepth(value, "water");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (depthValues.Length > 0)
        {
            float value = serverUtils.GetServerData("depth");
            pressureResult = calcFromDepth(value, "pressure");
            waterTempResult = calcFromDepth(value, "water");
            psiResult = pressureResult * 14.5038f;
        }
    }
}
