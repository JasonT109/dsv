using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Meg.Parameters;
using Button = UnityEngine.UI.Button;

/** 
 * The interface logic for editing a megParameterFile. 
 * 
 * See megParameterFile for more information on the parameter setup system.
 * 
 */

public class debugParameterFileUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Header for the current parameters file. */
    public Text Header;

    /** Container for parameter groups. */
    public Transform Groups;

    /** Load button. */
    public Button LoadButton;

    /** Add group button. */
    public Button AddButton;

    /** Remove group button. */
    public Button RemoveButton;

    /** Clear button. */
    public Button ClearButton;

    /** Add value parameter button. */
    public Button AddValueParameterButton;

    /** Add string parameter button. */
    public Button AddStringParameterButton;

    /** Remove group button. */
    public Button RemoveParameterButton;

    /** Clear button. */
    public Button ClearParametersButton;

    /** Move group up button. */
    public Button MoveGroupUpButton;

    /** Move group down button. */
    public Button MoveGroupDownButton;

    /** Move parameter up button. */
    public Button MoveParameterUpButton;

    /** Move parameter down button. */
    public Button MoveParameterDownButton;

    /** Save button. */
    public Button SaveButton;

    /** Parameter folder. */
    public debugSceneFolderUi Folder;

    /** Info toggle. */
    public Toggle InfoToggle;

    /** Parameter info view. */
    public GameObject InfoView;

    /** Parameter list UI. */
    public debugParameterListUi InfoList;


    [Header("Prefabs")]

    /** Prefab to use for a parameter group UI node. */
    public debugParameterGroupUi ParameterGroupUiPrefab;


    /** The current file. */
    public megParameterFile File { get { return _file; } }


    // Members
    // ------------------------------------------------------------

    /** The current parameter file. */
    private readonly megParameterFile _file = new megParameterFile();

    /** Parameter group UI nodes. */
    private readonly List<debugParameterGroupUi> _groups = new List<debugParameterGroupUi>();


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        _file.Loaded += OnFileLoaded;
        _file.Cleared += OnFileCleared;

        UpdateUi();
    }

    /** Updating. */
    private void Update()
    {
        UpdateUi();
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Load the selected event file. */
    public void Load()
    {
        var entry = Folder.FileList.SelectedEntry;
        if (!entry || !entry.FileInfo.Exists)
            return;

        Load(entry.FileInfo);
        Folder.FileList.SelectedEntry = null;
    }

    /** Load the selected parameter file. */
    public void Load(FileInfo f)
    {
        try
        {
            _file.LoadFromFile(f.FullName);
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to load parameter file: " + f.FullName + ", " + ex);
        }
    }

    /** Add a new group to the file. */
    public void AddGroup()
    {
        if (!_file.canAdd)
            return;

        var group = _file.InsertGroup(_file.selectedGroup);
        var ui = AddGroupUi(group);

        HandleGroupSelected(ui);
    }

    /** Remove the selected group from the file. */
    public void RemoveGroup()
    {
        if (!_file.canRemove || _file.selectedGroup == null)
            return;

        var group = _file.selectedGroup;
        DialogManager.Instance.ShowYesNo("REMOVE GROUP?",
            string.Format("Do you wish to remove the group '{0}'?", group.id),
            result =>
            {
                if (result != DialogYesNo.Result.Yes)
                    return;

                _file.RemoveGroup(group);
                RemoveGroupUi(group);
            });
    }

    public void AddValueParameter()
    {
        if (_file.selectedGroup == null)
            return;

        // Add a new parameter entry to the current group.
        var value = _file.selectedGroup.AddParameter(megParameterType.Value) as megParameterValue;

        // Apply selected server parameter (if there is one).
        if (value != null && InfoList.Selected)
            value.serverParam = InfoList.Selected.Text.text;
    }

    public void AddStringParameter()
    {
        if (_file.selectedGroup == null)
            return;

        // Add a new parameter entry to the current group.
        var value = _file.selectedGroup.AddParameter(megParameterType.String) as megParameterString;

        // Apply selected server parameter (if there is one).
        if (value != null && InfoList.Selected)
            value.serverParam = InfoList.Selected.Text.text;
    }

    public void RemoveParameter()
    {
        if (_file.selectedParameter == null)
            return;

        var toRemove = _file.selectedParameter;
        DialogManager.Instance.ShowYesNo("REMOVE PARAMETER?",
            string.Format("Are you sure you wish to remove the parameter '{0}'?", toRemove.name),
            result =>
            {
                if (result == DialogYesNo.Result.Yes)
                    toRemove.group.RemoveParameter(toRemove);
            });
    }

    public void ClearParameters()
    {
        if (_file.selectedGroup == null)
            return;

        var toClear = _file.selectedGroup;
        DialogManager.Instance.ShowYesNo("REMOVE PARAMETERS FROM GROUP?",
            string.Format("Are you sure you wish to remove all parameters from the group '{0}'?", toClear.id),
            result =>
            {
                if (result == DialogYesNo.Result.Yes)
                    toClear.Clear();
            });
    }

    /** Clear the file contents. */
    public void Clear()
    {
        if (!_file.canClear)
            return;

        DialogManager.Instance.ShowYesNo("REMOVE ALL GROUPS?",
            "Are you sure you wish to remove all groups from view?",
            result =>
            {
                if (result == DialogYesNo.Result.Yes)
                    _file.Clear();
            });
    }

    /** Save the file. */
    public void Save()
    {
        if (!_file.canSave)
            return;

        var path = "";

        try
        {
            var saveDialog = new System.Windows.Forms.SaveFileDialog();
            saveDialog.InitialDirectory = Folder.SceneFolder;
            saveDialog.Title = "Save Parameter File";
            saveDialog.Filter = "Parameter Files (*.json)|*.json";
            saveDialog.AddExtension = true;
            saveDialog.DefaultExt = "json";
            saveDialog.ShowDialog();
            path = saveDialog.FileName;

            if (!string.IsNullOrEmpty(path))
                _file.SaveToFile(path);

            Folder.FileList.Refresh();
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to save parameter file: " + path + ", " + ex);
        }
    }

    /** Toggle info. */
    public void ToggleInfo()
    {
        InfoView.SetActive(!InfoView.activeSelf);
    }

    /** Set the selected entry's parameter name. */
    public void SetSelectedParameter(string valueName)
    {
        var p = File.selectedParameter;
        if (p == null)
            return;

        var groupUi = _groups.FirstOrDefault(g => g.Group == p.group);
        if (!groupUi)
            return;

        var valueUi = groupUi.GetParameterValueUi(p);
        if (valueUi)
            valueUi.SetName(valueName);

        var stringUi = groupUi.GetParameterStringUi(p);
        if (stringUi)
            stringUi.SetName(valueName);
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Handle the parameter file being loaded. */
    private void OnFileLoaded(megParameterFile file)
    { InitUi(); }

    /** Handle the parameter file being cleared. */
    private void OnFileCleared(megParameterFile file)
    { InitUi(); }

    /** Initialize the file UI. */
    private void InitUi()
    {
        AddGroupUis(_file);

        // Force a UI reflow to ensure group layouts are correct.
        Canvas.ForceUpdateCanvases();
    }

    /** Update the file UI. */
    private void UpdateUi()
    {
        AddButton.interactable = _file.canAdd;
        RemoveButton.interactable = _file.canRemove && _file.selectedGroup != null;
        ClearButton.interactable = _file.canClear;
        AddValueParameterButton.interactable = _file.canAdd && _file.selectedGroup != null;
        AddStringParameterButton.interactable = _file.canAdd && _file.selectedGroup != null;
        RemoveParameterButton.interactable = _file.canRemove && _file.selectedParameter != null;
        ClearParametersButton.interactable = _file.canClear && _file.selectedGroup != null && !_file.selectedGroup.empty;
        SaveButton.interactable = _file.canSave;
        MoveGroupUpButton.interactable = File.CanMoveUp(File.selectedGroup);
        MoveGroupDownButton.interactable = File.CanMoveDown(File.selectedGroup);

        MoveParameterUpButton.interactable = _file.selectedParameter != null && _file.selectedParameter.CanMoveUp;
        MoveParameterDownButton.interactable = _file.selectedParameter != null && _file.selectedParameter.CanMoveDown;
    }

    /** Remove all event groups. */
    private void RemoveGroupUis()
    {
        foreach (var group in _groups)
            group.gameObject.Cleanup();

        _groups.Clear();
    }

    /** Remove a single group ui. */
    private void RemoveGroupUi(megParameterGroup group)
    {
        var ui = _groups.FirstOrDefault(g => g.Group == group);
        if (!ui)
            return;

        _groups.Remove(ui);
        ui.gameObject.Cleanup();
    }

    /** Add event groups from a file. */
    private void AddGroupUis(megParameterFile file)
    {
        RemoveGroupUis();

        foreach (var group in _file.groups)
            AddGroupUi(group);
    }

    /** Add UI for an event group. */
    private debugParameterGroupUi AddGroupUi(megParameterGroup group)
    {
        var ui = Instantiate(ParameterGroupUiPrefab);
        ui.transform.SetParent(Groups, false);
        ui.Group = group;
        ui.OnSelected += HandleGroupSelected;

        _groups.Add(ui);
        return ui;
    }

    private void HandleGroupSelected(debugParameterGroupUi groupUi, debugParameterUi eventUi = null)
    {
        var g = groupUi.Group;
        var e = eventUi ? eventUi.Parameter : null;
        if (e == _file.selectedParameter)
            groupUi.ToggleParameter(e);
        else
            groupUi.ExpandParameter(e);

        _file.selectedGroup = g;
    }

    /** Move selected group upwards. */
    public void MoveSelectedGroupUp()
    {
        if (File.selectedGroup == null)
            return;

        var group = File.selectedGroup;
        if (!File.MoveUp(group))
            return;

        var ui = _groups.FirstOrDefault(g => g.Group == group);
        if (ui == null)
            return;

        var index = _groups.IndexOf(ui);
        if (index <= 0)
            return;

        _groups[index] = _groups[index - 1];
        _groups[index - 1] = ui;

        ui.transform.SetSiblingIndex(index - 1);
    }

    /** Move a group downwards. */
    public void MoveSelectedGroupDown()
    {
        if (File.selectedGroup == null)
            return;

        var group = File.selectedGroup;
        if (!File.MoveDown(group))
            return;

        var ui = _groups.FirstOrDefault(g => g.Group == group);
        if (ui == null)
            return;

        var index = _groups.IndexOf(ui);
        var last = _groups.Count - 1;
        if (index < 0 || index >= last)
            return;

        _groups[index] = _groups[index + 1];
        _groups[index + 1] = ui;

        ui.transform.SetSiblingIndex(index + 1);
    }

    /** Move selected parameter upwards. */
    public void MoveSelectedParameterUp()
    {
        var ui = _groups.FirstOrDefault(g => g.Group == File.selectedGroup);
        if (ui)
            ui.MoveSelectedParameterUp();
    }

    /** Move selected parameter downwards. */
    public void MoveSelectedParameterDown()
    {
        var ui = _groups.FirstOrDefault(g => g.Group == File.selectedGroup);
        if (ui)
            ui.MoveSelectedParameterDown();
    }

}
