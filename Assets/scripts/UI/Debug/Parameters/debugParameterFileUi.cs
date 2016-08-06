using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Meg.Parameters;
using Button = UnityEngine.UI.Button;

public class debugParameterFileUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Header for the current parameters file. */
    public Text Header;

    /** Container for parameters parameters. */
    public Transform Parameters;

    /** Load button. */
    public Button LoadButton;

    /** Add group button. */
    public Button AddParameterButton;

    /** Remove group button. */
    public Button RemoveParameterButton;

    /** Clear button. */
    public Button ClearButton;

    /** Save button. */
    public Button SaveButton;

    /** Parameter properties. */
    public debugParameterListUi Properties;

    /** Event folder. */
    public debugSceneFolderUi Folder;


    [Header("Prefabs")]

    /** Prefab to use for an parameter group UI node. */
    public debugParameterUi ParameterUiPrefab;


    /** The current file. */
    public megParameterFile File { get { return _file; } }


    // Members
    // ------------------------------------------------------------

    /** The current parameter file. */
    private readonly megParameterFile _file = new megParameterFile();

    /** Event group UI nodes. */
    private readonly List<debugParameterUi> _parameters = new List<debugParameterUi>();

    /** Whether we're updating the UI right now. */
    private bool _updatingUi;


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
    public void AddParameter()
    {
        if (!_file.canAdd)
            return;

        var group = _file.InsertParameter(_file.selectedParameter, megParameterType.Value);
        var ui = AddParameterUi(group);

        HandleParameterSelected(ui);
    }

    /** Remove the selected group from the file. */
    public void RemoveParameter()
    {
        if (!_file.canRemove)
            return;

        var group = _file.selectedParameter;
        _file.RemoveParameter(group);
        RemoveParameterUi(group);
    }

    /** Clear the file contents. */
    public void Clear()
    {
        if (!_file.canClear)
            return;

        _file.Clear();
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
        AddParameterUis(_file);

        // Force a UI reflow to ensure group layouts are correct.
        Canvas.ForceUpdateCanvases();
    }

    /** Update the file UI. */
    private void UpdateUi()
    {
        _updatingUi = true;

        AddParameterButton.interactable = _file.canAdd;
        RemoveParameterButton.interactable = _file.canRemove && _file.selectedParameter != null;
        ClearButton.interactable = _file.canClear;
        SaveButton.interactable = _file.canSave;

        _updatingUi = false;
    }

    /** Remove all parameter parameters. */
    private void RemoveParameterUis()
    {
        foreach (var group in _parameters)
            Destroy(group.gameObject);

        _parameters.Clear();
    }

    /** Remove a single group ui. */
    private void RemoveParameterUi(megParameter group)
    {
        var ui = _parameters.FirstOrDefault(g => g.Parameter == group);
        if (!ui)
            return;

        _parameters.Remove(ui);
        Destroy(ui.gameObject);
    }

    /** Add parameter from a file. */
    private void AddParameterUis(megParameterFile file)
    {
        RemoveParameterUis();

        foreach (var group in _file.parameters)
            AddParameterUi(group);
    }

    /** Add UI for an parameter group. */
    private debugParameterUi AddParameterUi(megParameter group)
    {
        var ui = Instantiate(ParameterUiPrefab);
        ui.transform.SetParent(Parameters, false);
        ui.Parameter = group;
        ui.OnSelected += HandleParameterSelected;

        _parameters.Add(ui);
        return ui;
    }

    private void HandleParameterSelected(debugParameterUi ui)
    {
        File.selectedParameter = ui.Parameter;
    }

}
