using UnityEngine;
using System.Collections;

public class graphicsBackColor : MonoBehaviour
{
    private GameObject colourThemeObj;
    private Color bColor;
    private Color hColor;
    private Renderer r;
    private Material m;
    public bool useBackgroundColor = true;
    public bool useHighlightColor = false;

    public float BrightnessScale = 1;
    public float BrightnessMin = 0;

    public void updateColor()
    {
        if (useBackgroundColor)
        {
            m.color = AdjustColor(bColor);
        }
        if (useHighlightColor)
        {
            var color = AdjustColor(hColor);
            m.color = color;
            m.SetColor("_TintColor", color);
        }
    }

    void Start ()
    {
        r = gameObject.GetComponent<Renderer>();
        m = r.material;
    }

    void Update ()
    {
        if (colourThemeObj == null)
            colourThemeObj = GameObject.FindWithTag("ServerData");

        if (colourThemeObj)
        {
            bColor = colourThemeObj.GetComponent<graphicsColourHolder>().theme.backgroundColor;
            hColor = colourThemeObj.GetComponent<graphicsColourHolder>().theme.highlightColor;
            updateColor();
        }
    }

    private Color AdjustColor(Color c)
    {
        if (BrightnessScale == 1 && BrightnessMin <= 0)
            return c;
        else
        {
            var hsb = HSBColor.FromColor(c);
            hsb.b = Mathf.Max(BrightnessMin, hsb.b * BrightnessScale);
            return hsb.ToColor();
        }
    }
}
