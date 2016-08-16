using System;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Meg.Parameters;

public abstract class debugParameterUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** The parameter's backdrop graphic. */
    public Graphic Backdrop;

    /** The parameter name label. */
    public Text Name;


    [Header("Configuration")]

    /** Color to use for backdrop when parameter is active. */
    public Color BackdropColor;

    /** Color to use for backdrop when parameter is selected. */
    public Color BackdropSelectedColor;


    /** The parameter. */
    public megParameter Parameter
    {
        get { return _parameter; }
        set { SetParameter(value, true); }
    }

    /** Whether an parameter is set. */
    public bool HasParameter
        { get { return _parameter != null; } }

    /** The event file. */
    public megParameterFile File
        { get { return _parameter.file; } }

    /** Event signature for a parameter selection event. */
    public delegate void ParameterSelectedHandler(debugParameterUi ui);

    /** Selection event. */
    public event ParameterSelectedHandler OnSelected;


    // Members
    // ------------------------------------------------------------

    /** The parameter. */
    private megParameter _parameter;

    /** Whether ui is being updated. */
    protected bool _updating;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    protected virtual void Start()
    {
        ConfigureUi();
        InitUi();
        UpdateUi();
    }

    /** Updating. */
    protected virtual void Update()
    {
        if (Parameter != null)
            UpdateUi();
    }



    // Public Methods
    // ------------------------------------------------------------

    /** Select this parameter. */
    public void Select()
    {
        if (OnSelected != null)
            OnSelected(this);
    }


    // Private Methods
    // ------------------------------------------------------------

    public void SetParameter(megParameter value, bool initUi)
    {
        _parameter = value;

        if (initUi)
            InitParameterUi();
    }

    private void ConfigureUi()
    {
        ConfigureParameterUi();
    }

    private void InitUi()
    {
        _updating = true;

        InitParameterUi();

        _updating = false;
    }

    private void UpdateUi()
    {
        if (_parameter == null)
            return;

        _updating = true;

        var c = BackdropColor;
        var isSelected = _parameter.file.selectedParameter == _parameter;
        if (isSelected)
            c = BackdropSelectedColor;

        Backdrop.color = c;

        if (Name)
            Name.text = Parameter.ToString();

        UpdateParameterUi();

        _updating = false;
    }

    protected abstract void ConfigureParameterUi();
    protected abstract void InitParameterUi();
    protected abstract void UpdateParameterUi();

}
