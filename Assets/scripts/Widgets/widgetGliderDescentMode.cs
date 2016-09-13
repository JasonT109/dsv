using UnityEngine;
using System.Collections;
using Meg.Networking;

public class widgetGliderDescentMode : MonoBehaviour
{
    public buttonControl descent;
    public buttonControl cruise;
    public buttonControl ascend;

    public float descentValue;
    private float prevValue;
    private bool hasChanged;
    private float timer = 0f;
    private float updateTick = 0.2f;

    public float degreesPerSecond = 10f;

	void Start ()
    {
	
	}

	void Update ()
    {

        if (descent.active && descentValue > -90f)
        {
            descentValue -= Time.deltaTime * degreesPerSecond;
            descentValue = Mathf.Clamp(descentValue, -90, 90);
        }

        if (cruise.active && descentValue != 0)
        {
            if (descentValue >= -90f && descentValue < 0)
                descentValue += Time.deltaTime * degreesPerSecond;
            else if (descentValue <= 90f && descentValue > 0)
                descentValue -= Time.deltaTime * degreesPerSecond;

            if (Mathf.Approximately(descentValue, 0f))
                descentValue = 0f;
        }

        if (ascend.active && descentValue < 90f)
        {
            descentValue += Time.deltaTime * degreesPerSecond;
            descentValue = Mathf.Clamp(descentValue, -90, 90);
        }

        if (descentValue != prevValue)
            hasChanged = true;

        if (Time.time < timer)
            return;
        
        if (hasChanged)
        {
            timer = Time.time + updateTick;
            serverUtils.PostServerData("descentmodevalue", descentValue);
        }

	}
}
