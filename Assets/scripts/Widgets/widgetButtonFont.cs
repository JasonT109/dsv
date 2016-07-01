using UnityEngine;
using System.Collections;

public class widgetButtonFont : MonoBehaviour
{
    public Color inactiveColor;
    public Color activeColor;
    public Font inactiveFont;
    public Font activeFont;
    public TextMesh buttonText;
    public buttonControl bControl;

	void Update ()
    {
	    if (bControl)
        {
            if (bControl.active)
            {
                buttonText.font = activeFont;
                buttonText.GetComponent<Renderer>().sharedMaterial = activeFont.material;
                buttonText.font.material.color = activeColor;
                buttonText.color = activeColor;
            }
            else
            {
                buttonText.font = inactiveFont;
                buttonText.GetComponent<Renderer>().sharedMaterial = inactiveFont.material;
                buttonText.color = inactiveColor;
            }
        }
	}
}
