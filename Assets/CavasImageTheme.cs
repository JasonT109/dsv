using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CavasImageTheme : MonoBehaviour 
{
	private GameObject colourThemeObj;
	private Color bColor;
	private Color hColor;
	private Color kColor;

	private Image img;

	public bool useBackgroundColor = true;
	public bool useHighlightColor = false;
	public bool useKeyColor = false;
	public bool overrideAlpha = false;

	public float BrightnessScale = 1;
	public float BrightnessMin = 0;
	public float Alpha = 1;

	public void updateColor()
	{
		if (useBackgroundColor)
		{
			img.color = AdjustColor(bColor);
		}
		if (useHighlightColor)
		{
			var color = AdjustColor(hColor);
			img.color = color;
			//img.SetColor("_TintColor", color);
		}
		if (useKeyColor)
		{
			var color = AdjustColor(kColor);
			img.color = color;
			//img.SetColor("_TintColor", color);
		}
	}

	void Start ()
	{
		img = gameObject.GetComponent<Image>();
	}

	void Update ()
	{
		if (colourThemeObj == null)
			colourThemeObj = GameObject.FindWithTag("ServerData");

		if (colourThemeObj)
		{
			bColor = colourThemeObj.GetComponent<graphicsColourHolder>().theme.backgroundColor;
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
