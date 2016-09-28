using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DG.Tweening;
using Meg.UI;
using UnityEngine.Events;

public class DialogColor : Dialog
{

    [Header("Components")]

    public Button OkButton;
    public Button CancelButton;
    public Button HSVButton;
    public Button RGBButton;
    public Graphic HSVButtonOn;
    public Graphic RGBButtonOn;

    public Text Component1Text;
    public Slider Component1Slider;
    public InputField Component1Input;

    public Text Component2Text;
    public Slider Component2Slider;
    public InputField Component2Input;

    public Text Component3Text;
    public Slider Component3Slider;
    public InputField Component3Input;

    public Text Component4Text;
    public Slider Component4Slider;
    public InputField Component4Input;

    public Graphic ColorGraphic;
    public InputField HexInput;


    /** Current color in RGB format. */
    public Color RgbColor;

    /** Current color in HSV format. */
    public HSBColor HsvColor;

    /** Current color. */
    public Color Color
        { get { return IsRgb ? RgbColor : HsvColor.ToColor(); } }


    /** Current mode. */
    public ColorMode Mode = ColorMode.Hsv;

    public enum ColorMode
    {
        Hsv,
        Rgb
    };

    public class ColorChosenEvent : UnityEvent<Color> { }
    public ColorChosenEvent OnChosen = new ColorChosenEvent();

    private bool IsHsv
        { get { return Mode == ColorMode.Hsv; } }

    private bool IsRgb
        { get { return Mode == ColorMode.Rgb; } }

    private bool _updating;

    /** Configure the dialog. */
    public virtual void Configure(string title, string message, Color color)
    {
        Title.text = title;
        Message.text = message;

        RgbColor = color;
        HsvColor = HSBColor.FromColor(color);

        Component1Slider.onValueChanged.AddListener(OnComponentSliderChanged);
        Component2Slider.onValueChanged.AddListener(OnComponentSliderChanged);
        Component3Slider.onValueChanged.AddListener(OnComponentSliderChanged);
        Component4Slider.onValueChanged.AddListener(OnComponentSliderChanged);

        Component1Input.onEndEdit.AddListener(value => OnComponentInputEndEdit(value, Component1Slider));
        Component2Input.onEndEdit.AddListener(value => OnComponentInputEndEdit(value, Component2Slider));
        Component3Input.onEndEdit.AddListener(value => OnComponentInputEndEdit(value, Component3Slider));
        Component4Input.onEndEdit.AddListener(value => OnComponentInputEndEdit(value, Component4Slider));

        HexInput.onEndEdit.AddListener(OnHexInputEndEdit);

        UpdateUi();
    }

    /** Updating. */
    protected virtual void Update()
    {
        // Cancel the dialog if needed.
        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel();
    }

    public void SelectHSV()
    {
        Mode = ColorMode.Hsv;
        UpdateUi();
    }

    public void SelectRGB()
    {
        Mode = ColorMode.Rgb;
        UpdateUi();
    }

    /** Handle the 'OK' button being pressed. */
    public void OK()
    {
        if (OnChosen != null)
            OnChosen.Invoke(RgbColor);

        Close(Result.OK);
    }

    /** Handle the 'cancel' button being pressed. */
    public void Cancel()
    {
        Close();
    }

    private void UpdateUi()
    {
        _updating = true;

        HSVButtonOn.gameObject.SetActive(IsHsv);
        RGBButtonOn.gameObject.SetActive(IsRgb);

        if (IsHsv)
            UpdateUiHsv();
        else if (IsRgb)
            UpdateUiRgb();

        ColorGraphic.color = Color;
        Color32 c = ColorGraphic.color;

        HexInput.text = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", c.r, c.g, c.b, c.a);

        _updating = false;
    }

    
    private void UpdateUiHsv()
    {
        Component1Text.text = "Hue: ";
        Component2Text.text = "Saturation: ";
        Component3Text.text = "Value: ";
        Component4Text.text = "Alpha: ";

        Component1Slider.maxValue = 1;
        Component2Slider.maxValue = 1;
        Component3Slider.maxValue = 1;
        Component4Slider.maxValue = 1;

        Component1Slider.wholeNumbers = false;
        Component2Slider.wholeNumbers = false;
        Component3Slider.wholeNumbers = false;
        Component4Slider.wholeNumbers = false;

        Component1Slider.value = HsvColor.h;
        Component2Slider.value = HsvColor.s;
        Component3Slider.value = HsvColor.b;
        Component4Slider.value = HsvColor.a;

        Component1Input.text = HsvColor.h.ToString();
        Component2Input.text = HsvColor.s.ToString();
        Component3Input.text = HsvColor.b.ToString();
        Component4Input.text = HsvColor.a.ToString();
    }

    private void UpdateUiRgb()
    {
        Component1Text.text = "Red: ";
        Component2Text.text = "Green: ";
        Component3Text.text = "Blue: ";
        Component4Text.text = "Alpha: ";

        Component1Slider.maxValue = 255;
        Component2Slider.maxValue = 255;
        Component3Slider.maxValue = 255;
        Component4Slider.maxValue = 255;

        Component1Slider.wholeNumbers = true;
        Component2Slider.wholeNumbers = true;
        Component3Slider.wholeNumbers = true;
        Component4Slider.wholeNumbers = true;

        Color32 c = RgbColor;
        Component1Slider.value = c.r;
        Component2Slider.value = c.g;
        Component3Slider.value = c.b;
        Component4Slider.value = c.a;

        Component1Input.text = c.r.ToString();
        Component2Input.text = c.g.ToString();
        Component3Input.text = c.b.ToString();
        Component4Input.text = c.a.ToString();
    }

    private void OnHexInputEndEdit(string value)
    {
        if (_updating)
            return;

        UpdateColorHex();
        UpdateUi();
    }

    private void OnComponentInputEndEdit(string value, Slider slider)
    {
        if (_updating)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        slider.value = result;
        UpdateUi();
    }

    private void OnComponentSliderChanged(float value)
    {
        if (_updating)
            return;

        UpdateColor();
        UpdateUi();
    }

    private void UpdateColor()
    {
        if (IsHsv)
            UpdateColorHsv();
        else if (IsRgb)
            UpdateColorRgb();
    }

    private void UpdateColorHsv()
    {
        HsvColor.h = Component1Slider.value;
        HsvColor.s = Component2Slider.value;
        HsvColor.b = Component3Slider.value;
        HsvColor.a = Component4Slider.value;
        RgbColor = HsvColor.ToColor();
    }

    private void UpdateColorRgb()
    {
        var r = (byte) Component1Slider.value;
        var g = (byte) Component2Slider.value;
        var b = (byte) Component3Slider.value;
        var a = (byte) Component4Slider.value;

        RgbColor = new Color32(r, g, b, a);
        HsvColor = HSBColor.FromColor(RgbColor);
    }

    private void UpdateColorHex()
    {
        var match = Regex.Match(HexInput.text, @"^(#|0x)?([0-9A-F]{6,8})$", RegexOptions.IgnoreCase);
        if (match.Success)
            ColorUtility.TryParseHtmlString("#" + match.Groups[2].Value, out RgbColor);

        HsvColor = HSBColor.FromColor(RgbColor);
    }

}
