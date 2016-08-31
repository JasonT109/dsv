using UnityEngine;
using System.Collections;

public class DCCButtonColor : MonoBehaviour {

    private GameObject colourThemeObj;
    private Color hColor;
    private Color kColor;
    private Renderer r;
    private Material m;

    public bool useBackgroundColor = true;
    public bool useHighlightColor = false;
    public bool useKeyColor = false;
    public bool overrideAlpha = false;

    public float BrightnessScale = 1;
    public float BrightnessMin = 0;
    public float Alpha = 1;

    public void updateColor()
    {
        if (useHighlightColor)
        {
            var color = hColor;
            m.color = color;
            m.SetColor("_StartColor", color);
            color = AdjustColor(hColor);
            m.SetColor("_EndColor", color);
        }
        if (useKeyColor)
        {
            var color = kColor;
            m.color = color;
            m.SetColor("_StartColor", color);
            color = AdjustColor(kColor);
            m.SetColor("_EndColor", color);
        }
    }

    void Start()
    {
        r = gameObject.GetComponent<Renderer>();
        m = r.material;
    }

    void Update()
    {
        if (colourThemeObj == null)
            colourThemeObj = GameObject.FindWithTag("ServerData");

        if (colourThemeObj)
        {
            hColor = colourThemeObj.GetComponent<graphicsColourHolder>().theme.highlightColor;
            kColor = colourThemeObj.GetComponent<graphicsColourHolder>().theme.keyColor;
            updateColor();
        }
    }

    private Color AdjustColor(Color c)
    {
        if (BrightnessScale == 1 && BrightnessMin <= 0 && !overrideAlpha)
            return c;
        else
        {
            var hsb = HSBColor.FromColor(c);
            hsb.b = Mathf.Max(BrightnessMin, hsb.b * BrightnessScale);
            hsb.a = overrideAlpha ? Alpha : c.a;

            return hsb.ToColor();
        }
    }
}
