using UnityEngine;
using System.Collections;

public class widgetTextColorFromButton : MonoBehaviour
{
    public GameObject button;
    buttonControl bc;

    public float BrightnessScale = 1;
    public float MinBrightness = 0;

    public float SaturationScale = 1;
    public float MinSaturation = 0;


    void OnEnable()
    {
        if (!bc)
            bc = button.GetComponent<buttonControl>();

        // Determine adjusted text color.
        var c = bc.GetThemeColor(3);
        var hsb = HSBColor.FromColor(c);
        hsb.s = Mathf.Max(hsb.s * SaturationScale, MinSaturation);
        hsb.b = Mathf.Max(hsb.b * BrightnessScale, MinBrightness);
        c = hsb.ToColor();

        var widgetText = GetComponent<widgetText>();
        if (widgetText)
        {
            widgetText.Color = c;
            return;
        }

        var dynamicText = GetComponent<DynamicText>();
        if (dynamicText)
        {
            dynamicText.color = c;
            return;
        }

        var textMesh = GetComponent<TextMesh>();
        if (textMesh)
        {
            textMesh.color = c;
            return;
        }

    }
}
