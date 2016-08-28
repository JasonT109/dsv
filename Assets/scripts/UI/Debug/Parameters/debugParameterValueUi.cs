using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Meg.Parameters;
using Meg.Networking;

/**
 * Editing interface for a single server parameter value.
 * Allows the user to edit the value by direct mouse/touch manipulation (slider)
 * or by text input.  The user can also change which parameter is being edited
 * via the ServerParamInput field.
 *  
 */

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

    /** Display name for this parameter. */
    public override string DisplayName
    {
        get
        {
            if (ValueParameter == null)
                return base.DisplayName;

            var key = ValueParameter.serverParam;
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(key);
        }
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Set the name of this parameter. */
    public void SetName(string valueName)
    {
        _updating = true;
        ValueParameter.serverParam = valueName;
        UpdateServerParamInput();
        UpdateServerValueInput();
        UpdateServerValueSlider();
        _updating = false;
    }

    public override void SetParameter(megParameter value, bool initUi)
    {
        // Get previous parameter name (if any).
        var oldParam = ValueParameter != null ? ValueParameter.serverParam : null;

        // Superclass implementation.
        base.SetParameter(value, initUi);

        // If UI initialization was forced, no need to proceed further.
        if (initUi)
            return;

        // Check if parameter name has changed.
        // If so, we need to update UI elements to match.
        if (ValueParameter != null && oldParam != ValueParameter.serverParam)
        {
            _updating = true;
            UpdateServerParamInput();
            _updating = false;
        }
    }



    // Protected Methods
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
        if (ServerValueInput.isFocused)
            return;

        UpdateServerValueSlider();
        UpdateServerValueInput();
    }

    private void UpdateServerParamInput()
    {
        ServerParamInput.text = ValueParameter.serverParam;
    }

    private void UpdateServerValueSlider()
    {
        var key = ValueParameter.serverParam;
        var value = ValueParameter.serverValue;
        var info = serverUtils.GetServerDataInfo(key);
        var minValue = Mathf.Min(value, info.minValue);
        var maxValue = Mathf.Max(value, info.maxValue);

        ServerValueSlider.minValue = minValue;
        ServerValueSlider.maxValue = maxValue;
        ServerValueSlider.value = ValueParameter.serverValue;
        ServerValueSlider.wholeNumbers = (info.type == serverUtils.ParameterType.Bool) 
            || (info.type == serverUtils.ParameterType.Int);
    }

    private void UpdateServerValueInput()
    {
        var value = ValueParameter.serverValue;
        var format = "{0:N" + Precision(value) + "}";
        ServerValueInput.text = string.Format(format, value);
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

    /** Return the number of decimal places to use for a given value. */
    private static int Precision(double value)
    {
        var isInt = Math.Abs(value - Math.Round(value)) < 0.00001;
        if (isInt)
            return 0;

        var abs = Math.Abs(value);
        if (abs >= 100)
            return 0;
        if (abs >= 10)
            return 1;
        if (abs >= 1)
            return 2;
        if (abs >= 0.1)
            return 3;
        if (abs >= 0.01)
            return 4;

        return 5;
    }

}
