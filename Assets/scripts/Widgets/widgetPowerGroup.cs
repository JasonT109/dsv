using UnityEngine;
using System.Collections;
using Vectrosity;

public class widgetPowerGroup : MonoBehaviour {

    public digital_gauge[] gauges;
    public VectorObject2D line;
    public float lineAlpha = 1f;
    private float lineToAlpha = 0;
    private float lineFromAlpha = 0;
    private Color lineColor;

    public bool power = false;
    public bool changed = false;

    private bool lerping = false;
    private float lerpDuration = 3.0f;
    private float lerpTime = 0;
    private float lerpToValue = 0f;
    private float lerpFromValue = 0f;

    public bool powerOn
    {
        get
        {
            return power;
        }
        set
        {
            if (power)
            {
                power = false;
                changed = true;
                Debug.Log("Powering off: " + gameObject.name);
            }
            else
            {
                power = true;
                changed = true;
                Debug.Log("Powering on: " + gameObject.name);
            }
        }
    }

    IEnumerator wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        for (int i = 0; i < gauges.Length; i++)
        {
            gauges[i].useStaticValue = true;
            gauges[i].doWobble = true;
        }
    }

    void SetChildColor(Transform rootObject, bool on)
    {
        for (int c = 0; c < rootObject.childCount; c++)
        {
            Transform child = rootObject.GetChild(c);
            Renderer r = child.GetComponent<Renderer>();
            if (r)
            {
                if (on)
                    r.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0.5f));
                else
                    r.material.SetColor("_TintColor", new Color(0.1f, 0.1f, 0.1f, 0.5f));
            }

            if (child.childCount > 0)
                SetChildColor(child, on);
        }
    }

    void Start ()
    {
        if (line)
            lineColor = line.vectorLine.GetColor(0);
    }

	void Update ()
    {
        if (lerping)
        {
            lerpTime += Time.deltaTime;
            float percentComplete = lerpTime / lerpDuration;
            for (int i = 0; i < gauges.Length; i++)
            {
                gauges[i].value = (int)Mathf.Lerp(lerpFromValue, lerpToValue, percentComplete);
            }
            if (lerpTime > lerpDuration)
                lerping = false;
            if (line)
            {
                lineAlpha = Mathf.Lerp(lineFromAlpha, lineToAlpha, percentComplete);
                line.vectorLine.SetColor(new Color(lineColor.r, lineColor.g, lineColor.b, lineAlpha));
            }
        }

	    if (!power && changed)
        {
            changed = false;

            for (int i = 0; i < gauges.Length; i++)
            {
                SetChildColor(gauges[i].transform, false);
                lineToAlpha = 0.2f;
                lineFromAlpha = lineAlpha;
                lerpTime = 0;
                lerping = true;
                lerpFromValue = gauges[i].value;
                lerpToValue = 0;
                gauges[i].useStaticValue = false;
                gauges[i].doWobble = false;
            }
        }
        else if (power && changed)
        {
            changed = false;

            for (int i = 0; i < gauges.Length; i++)
            {
                SetChildColor(gauges[i].transform, true);
                lineToAlpha = 1;
                lineFromAlpha = lineAlpha;
                lerpTime = 0;
                lerping = true;
                lerpFromValue = gauges[i].value;
                lerpToValue = gauges[i].staticValue;
                StartCoroutine(wait(lerpDuration));
            }
        }
	}
}
