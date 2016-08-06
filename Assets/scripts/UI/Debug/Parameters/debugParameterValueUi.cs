using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Meg.Parameters;

public class debugParameterValueUi : debugParameterUi
{

    // Constants
    // ------------------------------------------------------------

    /** Default server value slider length. */
    public const float DefaultServerValueSliderLength = 100.0f;


    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    public InputField ServerParamInput;
    public Button ServerParamClearButton;
    public Slider ServerValueSlider;
    public InputField ServerValueInput;

    /** The value parameter. */
    public megParameterValue ValueParameter
        { get { return Parameter as megParameterValue; } }



    // Private Methods
    // ------------------------------------------------------------

    protected override void ConfigureParameterUi()
    {
        ServerParamInput.onValidateInput += ValidateIdentifierInput;
    }

    private char ValidateIdentifierInput(string text, int index, char addedChar)
    {
        if (!Regex.IsMatch(addedChar.ToString(), "[A-Za-z0-9_]"))
            return '\0';

        return addedChar;
    }

    protected override void InitParameterUi()
    {
        UpdateServerParamInput();
        UpdateServerValueSlider();
        UpdateServerValueInput();
    }

    protected override void UpdateParameterUi()
    {
    }

    private void UpdateServerParamInput()
    {
        ServerParamInput.text = ValueParameter.serverParam;
    }

    private void UpdateServerValueSlider()
    {
        var maxValue = Mathf.Max(ValueParameter.serverValue, DefaultServerValueSliderLength);
        ServerValueSlider.maxValue = maxValue;
        ServerValueSlider.value = ValueParameter.serverValue;
    }

    private void UpdateServerValueInput()
    {
        ServerValueInput.text = string.Format("{0:N2}", ValueParameter.serverValue);
    }

    public void ServerParamInputChanged(string value)
    {
        if (_updating)
            return;

        _updating = true;
        ValueParameter.serverParam = value;
        UpdateServerValueInput();
        UpdateServerValueSlider();
        _updating = false;
    }

    public void ServerParamInputClicked()
    {
    }

    public void ServerParamInputEndEdit(string value)
    {
        if (_updating)
            return;

        _updating = true;
        ValueParameter.serverParam = value;
        UpdateServerValueInput();
        UpdateServerValueSlider();
        _updating = false;
    }

    public void ServerParamClear()
    {
        if (_updating)
            return;

        ValueParameter.serverParam = "";
        UpdateServerParamInput();
    }

    public void ServerValueSliderChanged(float value)
    {
        if (_updating)
            return;

        ValueParameter.serverValue = value;
        UpdateServerValueInput();
    }

    public void ServerValueInputChanged(string value)
    {
        if (_updating)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        ValueParameter.serverValue = result;
        UpdateServerValueSlider();
    }


}
