using UnityEngine;
using System.Collections;

public class textValueFromDepth : widgetText
{

    public bool pressure;
    public bool psi;
    public bool waterTemp;
    public float updateTick = 0.2f;
    private float nextUpdate = 0;

    public string format = "";

    private clientCalcValues _depthCalculator;

    // Use this for initialization
    void Start ()
    {
        _depthCalculator = clientCalcValues.Instance;
        if (!_depthCalculator)
            return;

        UpdateValue();
        nextUpdate = Time.time;
    }
	
	// Update is called once per frame
	void Update ()
	{
        _depthCalculator = clientCalcValues.Instance;
        if (!_depthCalculator)
	        return;

        if (Time.time > nextUpdate)
        {
            UpdateValue();
            nextUpdate = Time.time + updateTick;
        }
    }

    void UpdateValue()
    {
        if (pressure)
        {
            float dValue = _depthCalculator.pressureResult;
            int dInt = (int)dValue;

            if (string.IsNullOrEmpty(format))
                Text = dInt.ToString() + "bar";
            else
                Text = string.Format(format, dValue);
        }
        if (waterTemp)
        {
            if (string.IsNullOrEmpty(format))
                Text = (_depthCalculator.waterTempResult.ToString("n2") + "°c");
            else
                Text = string.Format(format, _depthCalculator.waterTempResult);
        }
        if (psi)
        {
            float dValue = _depthCalculator.psiResult;
            int dInt = (int) dValue;

            if (string.IsNullOrEmpty(format))
                Text = dInt.ToString() + "psi";
            else
                Text = string.Format(format, dValue);
        }
    }

}
