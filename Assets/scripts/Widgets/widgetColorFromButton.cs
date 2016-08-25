using UnityEngine;
using System.Collections;

public class widgetColorFromButton : MonoBehaviour
{
    public GameObject button;
    private buttonControl _buttonControl;
    private Renderer _renderer;

    public float BrightnessScale = 1;
    public float MinBrightness = 0;

    public float SaturationScale = 1;
    public float MinSaturation = 0;

    public bool Continuous;


    private void Start()
    {
        _buttonControl = button.GetComponent<buttonControl>();
    }

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
        if (!_buttonControl)
            _buttonControl = button.GetComponent<buttonControl>();
        if (!_buttonControl)
            return;

        // Determine adjusted text color.
        var c = _buttonControl.GetThemeColor(3);
        var hsb = HSBColor.FromColor(c);
        hsb.s = Mathf.Max(hsb.s * SaturationScale, MinSaturation);
        hsb.b = Mathf.Max(hsb.b * BrightnessScale, MinBrightness);
        c = hsb.ToColor();

        if (!_renderer)
            _renderer = gameObject.GetComponent<Renderer>();

        if (_renderer.material.HasProperty("_TintColor"))
            _renderer.material.SetColor("_TintColor", c);
        else if (_renderer.material.HasProperty("_Color"))
            _renderer.material.SetColor("_Color", c);
    }

}
