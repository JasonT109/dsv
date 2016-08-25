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

    public bool Continuous;

    private widgetText _widgetText;
    private DynamicText _dynamicText;
    private TextMesh _textMesh;

    private void OnEnable()
    {
        UpdateColor();
    }

    private void Update()
    {
        if (Continuous)
            UpdateColor();
    }

    private void UpdateColor()
    { 
        if (!bc)
            bc = button.GetComponent<buttonControl>();

        // Determine adjusted text color.
        var c = bc.GetThemeColor(3);
        var hsb = HSBColor.FromColor(c);
        hsb.s = Mathf.Max(hsb.s * SaturationScale, MinSaturation);
        hsb.b = Mathf.Max(hsb.b * BrightnessScale, MinBrightness);
        c = hsb.ToColor();

        if (!_widgetText)
            _widgetText = GetComponent<widgetText>();
        if (_widgetText)
            { _widgetText.Color = c; return; }

        if (!_dynamicText)
            _dynamicText = GetComponent<DynamicText>();
        if (_dynamicText)
            { _dynamicText.color = c; return; }

        if (!_textMesh)
            _textMesh = GetComponent<TextMesh>();
        if (_textMesh)
            { _textMesh.color = c; }
    }
}
