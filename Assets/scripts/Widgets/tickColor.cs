using UnityEngine;
using System.Collections;

public class tickColor : MonoBehaviour {

    //public GameObject thisParent;
    private Renderer r;
    private Material m;

    public float colorPosition = 0.0f;
    public int colorIndex = 0;
    public float colorLerpValue = 0.0f;
    public float v = 0.0f;
    public digital_gauge dScript;

    // Use this for initialization
    void OnEnable () {

        dScript = gameObject.GetComponentInParent<digital_gauge>();
        r = gameObject.GetComponent<Renderer>();
        m = r.material;
        m.color = dScript.colorGradient[0];

        float x;
        if (dScript.mapGradientToTicks)
        {
            for (int i = 0; i < dScript.digitalTicks.Length; i++)
            {
                if (dScript.digitalTicks[i] == gameObject)
                {
                    
                    if (i > 0)
                    {
                        x = Mathf.Floor(((float)i / dScript.digitalTicks.Length) * dScript.colorGradient.Length);
                        //Debug.Log("Setting colour on tick: " + gameObject + "to color index: " + (int)x);
                    }
                    else
                    {
                        x = 0.0f;
                    }
                    m.color = dScript.colorGradient[(int)x];
                    m.SetColor("_MainColor", dScript.colorGradient[(int)x]);
                    m.SetColor("_TintColor", dScript.colorGradient[(int)x]);
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!dScript.mapGradientToTicks)
        {
            //get value and determine what percentage it is from min to max value
            float dv = dScript.value;
            float maxV = dScript.maxValue;
            float minV = dScript.minValue;
            v = (dv / (maxV - minV)) * 100;
            //v = dScript.value;
            if (v != 0)
            {
                colorPosition = Mathf.Clamp(v / (100 / (dScript.colorGradient.Length - 1)), 0.0f, dScript.colorGradient.Length - 1);
                colorIndex = Mathf.FloorToInt(colorPosition);
                colorLerpValue = colorPosition - colorIndex;
            }
            else
            {
                colorPosition = 0.0f;
                colorIndex = 0;
                colorLerpValue = 0.0f;
            }

            //set color to be a lerp between color gradient
            if (colorIndex >= dScript.colorGradient.Length - 1)
            {
                m.color = Color.Lerp(dScript.colorGradient[colorIndex], dScript.colorGradient[colorIndex - 1], colorLerpValue);
                m.SetColor("_TintColor", Color.Lerp(dScript.colorGradient[colorIndex], dScript.colorGradient[colorIndex - 1], colorLerpValue));
                m.SetColor("_MainColor", Color.Lerp(dScript.colorGradient[colorIndex], dScript.colorGradient[colorIndex - 1], colorLerpValue));
            }
            else
            {
                m.color = Color.Lerp(dScript.colorGradient[colorIndex], dScript.colorGradient[colorIndex + 1], colorLerpValue);
                m.SetColor("_TintColor", Color.Lerp(dScript.colorGradient[colorIndex], dScript.colorGradient[colorIndex + 1], colorLerpValue));
                m.SetColor("_MainColor", Color.Lerp(dScript.colorGradient[colorIndex], dScript.colorGradient[colorIndex + 1], colorLerpValue));
            }
        }
    }
}
