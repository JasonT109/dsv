using UnityEngine;
using System.Collections;
using Meg.Networking;

public class clientCalcValues : Singleton<clientCalcValues>
{
    public float pressureResult;
    public float psiResult;
    public float waterTempResult;
    public float[] depthValues;
    public float[] pressureValues;
    public float[] waterTempValues;

    private float calcFromDepth(float d, string valueType)
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

    void Awake()
    {
        // Manually set up singleton (since this game object is networked and will be initially disabled).
        SetInstance(this);
    }

	// Use this for initialization
	void Start ()
    {
        UpdateCalculatedValues();
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateCalculatedValues();
    }

    private void UpdateCalculatedValues()
    {
        if (depthValues == null || depthValues.Length <= 0 || !serverUtils.IsReady())
            return;

        var depth = serverUtils.GetServerData("depth");
        var pressure = calcFromDepth(depth, "pressure");
        var waterTemp = calcFromDepth(depth, "water");

        // Interpolate between actual and override pressure.
        var pressureOverride = serverUtils.GetServerData("pressureOverride", pressure);
        var pressureOverrideAmount = serverUtils.GetServerData("pressureOverrideAmount", 0);
        pressureResult = Mathf.Lerp(pressure, pressureOverride, Mathf.Clamp01(pressureOverrideAmount));

        // PSI is always related to displayed pressure.
        psiResult = pressureResult * Conversions.BarToPsi;

        // Interpolate between actual and override water temperature.
        var waterTempOverride = serverUtils.GetServerData("waterTempOverride", waterTemp);
        var waterTempOverrideAmount = serverUtils.GetServerData("waterTempOverrideAmount", 0);
        waterTempResult = Mathf.Lerp(waterTemp, waterTempOverride, Mathf.Clamp01(waterTempOverrideAmount));
    }
}
