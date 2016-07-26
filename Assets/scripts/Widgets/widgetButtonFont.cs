using UnityEngine;
using System.Collections;

public class widgetButtonFont : MonoBehaviour
{
    public bool usingDynamicText;
    public Color inactiveColor;
    public Color activeColor;
    public Font inactiveFont;
    public Font activeFont;
    public DynamicText dText;
    public TextMesh buttonText;
    public buttonControl bControl;

	void Update ()
    {
        if (usingDynamicText)
        {
            if (bControl)
            {
                if (bControl.active)
                {
                    dText.font = activeFont;
                    //dText.GetComponent<Renderer>().sharedMaterial = activeFont.material;
                    //dText.font.material.color = activeColor;
                    dText.color = activeColor;
                }
                else
                {
                    dText.font = inactiveFont;
                    //dText.GetComponent<Renderer>().sharedMaterial = inactiveFont.material;
                    dText.color = inactiveColor;
                }
            }
        }
        else
        {
	        if (bControl)
            {
                if (bControl.active)
                {
                    buttonText.font = activeFont;
                    //buttonText.GetComponent<Renderer>().sharedMaterial = activeFont.material;
                    //buttonText.font.material.color = activeColor;
                    buttonText.color = activeColor;
                }
                else
                {
                    buttonText.font = inactiveFont;
                    //buttonText.GetComponent<Renderer>().sharedMaterial = inactiveFont.material;
                    buttonText.color = inactiveColor;
                }
            }
        }
	}
}
