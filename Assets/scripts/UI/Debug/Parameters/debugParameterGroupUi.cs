using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Meg.Parameters;

public class debugParameterGroupUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** The parameter group's name input. */
    public InputField NameInput;

    /** Header area. */
    public CanvasGroup Header;

    /** Selected indicator. */
    public Graphic On;

    /** Container for parameter properties. */
    public Transform ParameterContainer;

    /** Minimize toggle. */
    public Toggle MinimizeToggle;


    [Header("Prefabs")]

    /** Prefab to use for parameter properties. */
    public debugParameterValueUi ValueParameterPrefab;


    /** The parameter group. */
    public megParameterGroup Group
    {
        get { return _group; }
        set { SetGroup(value); }
    }

    /** Whether an group is set. */
    public bool HasGroup
    { get { return _group != null; } }

    /** The parameter file. */
    public megParameterFile File
    { get { return _group != null ? _group.file : null; } }

    /** Whether timeline is minimized. */
    public bool Minimized
    { get { return _group.minimized; } }


    // Parameters
    // ------------------------------------------------------------

    /** Parameter signature for a parameter selection. */
    public delegate void ParameterSelectedHandler(debugParameterGroupUi groupUi, debugParameterUi parameterUi);

    /** Selection parameter. */
    public event ParameterSelectedHandler OnSelected;


    // Members
    // ------------------------------------------------------------

    /** The parameter group. */
    private megParameterGroup _group;

    /** Whether ui is being updated. */
    private bool _updating;

    /** Trigger buttons for parameters. */
    private readonly List<debugParameterUi> _parameters = new List<debugParameterUi>();



    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        ConfigureUi();
        ClearUi();
    }

    /** Updating. */
    private void Update()
    {
        if (_group != null)
            UpdateUi();
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Sets the group's name. */
    public void NameInputChanged(string value)
    {
        if (_updating || _group == null)
            return;

        _group.id = value;
    }

    /** Toggle an parameter's minimized state. */
    public void ToggleParameter(megParameter toToggle)
    {
        if (File == null)
            return;

        if (toToggle == File.selectedParameter)
            File.selectedParameter = null;
        else
            File.selectedParameter = toToggle;
    }

    /** Expand an parameter. */
    public void ExpandParameter(megParameter toExpand)
    {
        if (File != null)
            File.selectedParameter = toExpand;
    }

    /** Minimize an parameter. */
    public void MinimizeParameter(megParameter toExpand)
    {
        if (File != null)
            File.selectedParameter = null;
    }

    /** Select an parameter. */
    public void SelectParameter(megParameter toSelect)
    {
        var parameter = _parameters.FirstOrDefault(e => e.Parameter == toSelect);
        if (OnSelected != null)
            OnSelected(this, parameter);
    }

    /** Add a value parameter to the group. */
    public void AddValueParameter()
    { AddParameter(megParameterType.Value); }

    /** Add an parameter of a given type to the group. */
    public megParameter AddParameter(megParameterType type)
    {
        var e = _group.InsertParameter(type, File.selectedParameter);
        File.selectedParameter = e;
        ExpandParameter(e);
        return e;
    }

    /** Remove an parameter from the group. */
    public void RemoveParameter()
    {
        var toRemove = File.selectedParameter;
        if (toRemove == null)
            return;

        _group.RemoveParameter(toRemove);
    }

    public void Clear()
    {
        _group.Clear();
    }

    /** Toggle timeline's minimized state. */
    public void ToggleMinimized()
    {
        if (_updating)
            return;

        if (File.selectedGroup != Group)
            File.selectedGroup = Group;
        else
            File.selectedGroup = null;
    }


    // Private Methods
    // ------------------------------------------------------------

    private void SetGroup(megParameterGroup value)
    {
        if (_group == value)
            return;

        _group = value;

        if (_group != null)
        {
            NameInput.text = _group.id;
            UpdateUi();
        }
        else
            ClearUi();
    }

    private void ConfigureUi()
    {
        NameInput.onValidateInput += ValidateGroupNameInput;
    }

    private void UpdateUi()
    {
        if (_group == null)
            return;

        _updating = true;

        On.gameObject.SetActive(!_group.minimized);
        MinimizeToggle.isOn = !_group.minimized;
        ParameterContainer.gameObject.SetActive(!_group.minimized);

        UpdateParameters();

        _updating = false;
    }

    private void ClearUi()
    {
        UpdateParameters();
    }

    private void UpdateParameters()
    {
        var index = 0;
        if (_group != null)
            foreach (var e in _group.parameters)
                GetParameter(index++).Parameter = e;

        for (var i = 0; i < _parameters.Count; i++)
            _parameters[i].gameObject.SetActive(i < index);
    }

    private debugParameterUi GetParameter(int i)
    {
        if (i >= _parameters.Count)
        {
            var parameterUi = Instantiate(ValueParameterPrefab);
            parameterUi.transform.SetParent(ParameterContainer, false);
            parameterUi.OnSelected += HandleParameterSelected;
            _parameters.Add(parameterUi);
        }

        return _parameters[i];
    }

    private void HandleParameterSelected(debugParameterUi ui)
    {
        if (OnSelected != null)
            OnSelected(this, ui);
    }

    private char ValidateGroupNameInput(string text, int index, char addedChar)
    {
        if (!Regex.IsMatch(addedChar.ToString(), "[A-Za-z0-9_ ]"))
            return '\0';

        return addedChar;
    }


}
