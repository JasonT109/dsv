using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupColorThemed : MonoBehaviour
{
    /** Brightness adjustment. */
    public float Brightness = 1;

    /** Alpha adjustment. */
    public float Alpha = 1;

    /** Update the object's color to match popup theme. */
    public void UpdateColor(popupData.Popup popup)
    {
        var graphic = GetComponent<Graphic>();

        graphic.color = HSBColor.FromColor(popup.Color)
            .Brighten(Brightness)
            .Fade(Alpha)
            .ToColor();

    }


}
