using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    /** Clear button. */
    public Button ClearButton;

    /** Save button. */
    public Button SaveButton;

    /** Pause icon. */
    public Graphic PauseIcon;

    /** Event properties. */
    public debugEventPropertiesUi Properties;

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




    // Members
    // ------------------------------------------------------------

    /** The current event file. */
    private megEventFile _file = new megEventFile();

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
        _playLabel = PlayButton.GetComponentInChildren<Text>();
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
            // _file.Stop();
            _file.LoadFromFile(f.FullName);

            InitUi();
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
        
        if (value && !_file.running || _file.completed)
            _file.Start();
        else if (value && _file.paused)
            _file.Resume();
        else if (!value)
            _file.Pause();
    }

    /** Rewind to the start of the event file. */
    public void Rewind()
    {
        _file.Rewind();
    }

    /** Clear the file contents. */
    public void Clear()
    {
        if (!_file.canClear)
            return;

        Properties.Event = null;

        _file = new megEventFile();
        InitUi();
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

    /** Initialize the file UI. */
    private void InitUi()
    {
        AddGroups(_file);

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

        LoadButton.interactable = _file.canLoad;
        RewindButton.interactable = _file.canRewind;
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

        Properties.gameObject.SetActive(Properties.HasEvent);

        _updatingUi = false;
    }

    /** Remove all event groups. */
    private void RemoveGroups()
    {
        foreach (var group in _groups)
            Destroy(group.gameObject);

        _groups.Clear();
    }

    /** Add event groups from a file. */
    private void AddGroups(megEventFile file)
    {
        RemoveGroups();

        foreach (var group in _file.groups)
            AddGroup(group);
    }

    /** Add UI for an event group. */
    private void AddGroup(megEventGroup group)
    {
        var ui = Instantiate(EventGroupUiPrefab);
        ui.transform.SetParent(Groups, false);
        ui.Group = group;
        ui.OnEventSelected += HandleEventSelected;

        _groups.Add(ui);
    }

    private void HandleEventSelected(debugEventUi ui)
    {
        var selected = ui.Event;
        if (selected == Properties.Event)
            selected = null;

        Properties.Event = selected;
        _file.selectedEvent = selected;
    }


}
