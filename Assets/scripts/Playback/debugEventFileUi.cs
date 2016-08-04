using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Meg.EventSystem;
using Meg.Networking;
using Button = UnityEngine.UI.Button;

public class debugEventFileUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Header for the current event file. */
    public Text Header;

    /** Container for event groups. */
    public Transform Groups;

    /** Load button. */
    public Button LoadButton;

    /** Playback button. */
    public Toggle PlayButton;

    /** Rewind button. */
    public Button RewindButton;

    /** Add group button. */
    public Button AddGroupButton;

    /** Remove group button. */
    public Button RemoveGroupButton;

    /** Clear button. */
    public Button ClearButton;

    /** Save button. */
    public Button SaveButton;

    /** Pause icon. */
    public Graphic PauseIcon;

    /** Event properties. */
    public debugEventGroupPropertiesUi Properties;

    /** Event folder. */
    public debugSceneFolderUi Folder;

    /** Current time. */
    public Text TimeText;

    /** Current duration. */
    public Text DurationText;


    [Header("Prefabs")]

    /** Prefab to use for an event group UI node. */
    public debugEventGroupUi EventGroupUiPrefab;


    [Header("Colors")]

    /** Color for inactive time text. */
    public Color InactiveTimeTextColor;

    /** Color for active time text. */
    public Color ActiveTimeTextColor;

    /** Color for inactive duration text. */
    public Color InactiveDurationTextColor;

    /** Color for active duration text. */
    public Color ActiveDurationTextColor;


    /** The current file. */
    public megEventFile File { get { return _file; } }


    // Members
    // ------------------------------------------------------------

    /** The current event file. */
    private readonly megEventFile _file = new megEventFile();

    /** Event group UI nodes. */
    private readonly List<debugEventGroupUi> _groups = new List<debugEventGroupUi>();

    /** Play button text component. */
    private Text _playLabel;

    /** Whether we're updating the UI right now. */
    private bool _updatingUi;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        _file.Loaded += OnFileLoaded;
        _file.Cleared += OnFileCleared;

        _playLabel = PlayButton.GetComponentInChildren<Text>();
        Properties.OnSelected += HandlePropertiesSelected;

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
    public void Load(FileInfo f)
    {
        try
        {
            _file.LoadFromFile(f.FullName);
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to load event file: " + f.FullName + ", " + ex);
        }
    }

    /** Start playback on the event file. */
    public void TogglePlay(bool value)
    {
        // Ignore events originating from internal ui updates.
        if (_updatingUi)
            return;
        
        if (value && !_file.running)
            _file.Start();
        else if (value && _file.paused)
            _file.Resume();
        else if (!value)
            _file.Pause();
    }

    /** Rewind to the start of the event file. */
    public void Rewind()
    {
        if (!_file.canRewind)
            return;

        _file.Rewind();
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
        if (!_file.canRemove)
            return;

        var group = _file.selectedGroup;
        _file.RemoveGroup(group);
        RemoveGroupUi(group);
    }

    /** Clear the file contents. */
    public void Clear()
    {
        if (!_file.canClear)
            return;

        Properties.Group = null;

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
            saveDialog.Title = "Save Event File";
            saveDialog.Filter = "Event Files (*.json)|*.json";
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
            Debug.LogError("Failed to save event file: " + path + ", " + ex);
        }
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Handle the event file being loaded. */
    private void OnFileLoaded(megEventFile file)
        { InitUi(); }

    /** Handle the event file being cleared. */
    private void OnFileCleared(megEventFile file)
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
        _updatingUi = true;

        PlayButton.interactable = _file.canPlay;
        PlayButton.isOn = _file.playing;

        _playLabel.text = _file.playing ? "PAUSE" : "PLAY";
        PauseIcon.gameObject.SetActive(_file.playing);

        RewindButton.interactable = _file.canRewind;
        AddGroupButton.interactable = _file.canAdd;
        RemoveGroupButton.interactable = _file.canRemove && _file.selectedGroup != null;
        ClearButton.interactable = _file.canClear;
        SaveButton.interactable = _file.canSave;

        var t = TimeSpan.FromSeconds(_file.time);
        TimeText.color = _file.playing ? ActiveTimeTextColor : InactiveTimeTextColor;
        TimeText.text = string.Format("{0:00}:{1:00}:{2:00}.{3:0}", 
            t.Hours, t.Minutes, t.Seconds, t.Milliseconds / 100);

        var d = TimeSpan.FromSeconds(_file.endTime);
        DurationText.color = _file.playing ? ActiveDurationTextColor : InactiveDurationTextColor;
        DurationText.text = string.Format("/ {0:00}:{1:00}:{2:00}",
            d.Hours, d.Minutes, d.Seconds);

        // Properties.gameObject.SetActive(Properties.HasGroup);

        _updatingUi = false;
    }

    /** Remove all event groups. */
    private void RemoveGroupUis()
    {
        foreach (var group in _groups)
            Destroy(group.gameObject);

        _groups.Clear();
    }

    /** Remove a single group ui. */
    private void RemoveGroupUi(megEventGroup group)
    {
        var ui = _groups.FirstOrDefault(g => g.Group == group);
        if (!ui)
            return;

        if (Properties.Group == group)
            Properties.Group = null;

        _groups.Remove(ui);
        Destroy(ui.gameObject);
    }

    /** Add event groups from a file. */
    private void AddGroupUis(megEventFile file)
    {
        RemoveGroupUis();

        foreach (var group in _file.groups)
            AddGroupUi(group);
    }

    /** Add UI for an event group. */
    private debugEventGroupUi AddGroupUi(megEventGroup group)
    {
        var ui = Instantiate(EventGroupUiPrefab);
        ui.transform.SetParent(Groups, false);
        ui.Group = group;
        ui.OnSelected += HandleGroupSelected;

        _groups.Add(ui);
        return ui;
    }

    private void HandleGroupSelected(debugEventGroupUi groupUi, debugEventUi eventUi = null)
    {
        var g = groupUi.Group;
        var e = eventUi ? eventUi.Event : null;
        if (g == _file.selectedGroup && e == null)
            g = null;

        Properties.Group = g;

        if (e == _file.selectedEvent)
            Properties.ToggleEvent(e);
        else
            Properties.ExpandEvent(e);

        _file.selectedGroup = g;
    }

    private void HandlePropertiesSelected(debugEventGroupPropertiesUi groupUi, debugEventPropertiesUi eventUi)
    {
        var e = eventUi ? eventUi.Event : null;
        Properties.ToggleEvent(e);
    }

}
