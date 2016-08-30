using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Meg.Parameters;
using Meg.Networking;

/**
 * Editing interface for a single server parameter value.
 * Allows the user to edit the value by text input.  
 * The user can also change which parameter is being edited
 * via the ServerParamInput field.
 *  
 */

public class debugParameterStringUi : debugParameterUi
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    public InputField ServerParamInput;
    public Button ServerParamClearButton;
    public InputField ServerStringInput;


    /** The value parameter. */
    public megParameterString StringParameter
        { get { return Parameter as megParameterString; } }

    /** Display name for this parameter. */
    public override string DisplayName
    {
        get
        {
            if (StringParameter == null)
                return base.DisplayName;

            var key = StringParameter.serverParam;
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(key);
        }
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Set the name of this parameter. */
    public void SetName(string valueName)
    {
        _updating = true;
        StringParameter.serverParam = valueName;
        UpdateServerParamInput();
        UpdateServerStringInput();
        _updating = false;
    }

    public override void SetParameter(megParameter value, bool initUi)
    {
        // Get previous parameter name (if any).
        var oldParam = StringParameter != null ? StringParameter.serverParam : null;

        // Superclass implementation.
        base.SetParameter(value, initUi);

        // If UI initialization was forced, no need to proceed further.
        if (initUi)
            return;

        // Check if parameter name has changed.
        // If so, we need to update UI elements to match.
        if (StringParameter != null && oldParam != StringParameter.serverParam)
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
        UpdateServerStringInput();
    }

    protected override void UpdateParameterUi()
    {
        if (ServerStringInput.isFocused)
            return;

        UpdateServerStringInput();
    }

    private void UpdateServerParamInput()
    {
        ServerParamInput.text = StringParameter.serverParam;
    }

    private void UpdateServerStringInput()
    {
        ServerStringInput.text = StringParameter.serverValue;
    }

    public void ServerParamInputChanged(string value)
    {
        if (_updating)
            return;

        _updating = true;
        StringParameter.serverParam = value;
        UpdateServerStringInput();
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
        StringParameter.serverParam = value;
        UpdateServerStringInput();
        _updating = false;
    }

    public void ServerParamClear()
    {
        if (_updating)
            return;

        StringParameter.serverParam = "";
        UpdateServerParamInput();
    }

    public void ServerStringInputChanged(string value)
    {
        if (_updating)
            return;

        StringParameter.serverValue = value;
    }

}
