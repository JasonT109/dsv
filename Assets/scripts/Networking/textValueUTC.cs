using System;
using UnityEngine;
using System.Collections;

public class textValueUTC : widgetText
{

    public float updateTick = 0.05f;
    private float nextUpdate = 0;

    public string format = "{0:dd/MM/yy hh:mm:ss.f}";

    private clientCalcValues _depthCalculator;

    void Start()
    {
        UpdateValue();
        nextUpdate = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextUpdate)
        {
            UpdateValue();
            nextUpdate = Time.time + updateTick;
        }
    }

    void UpdateValue()
    {
        var utc = DateTime.UtcNow;
        Text = string.Format(format, utc);
    }

}
