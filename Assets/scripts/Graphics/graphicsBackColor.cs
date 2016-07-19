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

    public void updateColor()
    {
        if (useBackgroundColor)
        {
            m.color = bColor;
        }
        if (useHighlightColor)
        {
            m.color = hColor;
            m.SetColor("_TintColor", hColor);
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
}
