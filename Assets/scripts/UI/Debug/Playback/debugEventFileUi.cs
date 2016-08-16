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

/** 
 * The interface logic for editing an Event File. 
 * 
 * See megEventFile for more information on the event playback system.
 * 
 */

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
    public megEventFile File { get { return megEventManager.Instance.File; } }


    // Members
    // ------------------------------------------------------------

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
        File.Loaded += OnFileLoaded;
        File.Cleared += OnFileCleared;
        File.GroupAdded += OnGroupAdded;
        File.GroupRemoved += OnGroupRemoved;

        _playLabel = PlayButton.GetComponentInChildren<Text>();
        Properties.OnSelected += HandlePropertiesSelected;

        InitUi();
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
            File.LoadFromFile(f.FullName);
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to load event file: " + f.FullName + ", " + ex);
        }
    }

    /** Toggle playback on the event file. */
    public void TogglePlay()
    {
        // Ignore events originating from internal ui updates.
        if (_updatingUi)
            return;
        
        if (!File.running)
            File.Start();
        else if (File.paused)
            File.Resume();
        else
            File.Pause();
    }

    /** Rewind to the start of the event file. */
    public void Rewind()
    {
        if (!File.canRewind)
            return;

        File.Rewind();
    }

    /** Add a new group to the file. */
    public void AddGroup()
    {
        if (!File.canAdd)
            return;

        File.InsertGroup(File.selectedGroup);
    }

    /** Remove the selected group from the file. */
    public void RemoveGroup()
    {
        if (!File.canRemove)
            return;

        var group = File.selectedGroup;
        File.RemoveGroup(group);
    }

    /** Clear the file contents. */
    public void Clear()
    {
        if (!File.canClear)
            return;

        Properties.Group = null;

        File.Clear();
    }

    /** Save the file. */
    public void Save()
    {
        if (!File.canSave)
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
                File.SaveToFile(path);

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

    /** Handle a group being added to the event file. */
    private void OnGroupAdded(megEventFile file, megEventGroup group)
    {
        var ui = AddGroupUi(group);
        HandleGroupSelected(ui);
    }

    /** Handle a group being removed from from the event file. */
    private void OnGroupRemoved(megEventFile file, megEventGroup group)
    {
        RemoveGroupUi(group);
    }

    /** Initialize the file UI. */
    private void InitUi()
    {
        Properties.Group = null;
        AddGroupUis(File);

        // Force a UI reflow to ensure group layouts are correct.
        Canvas.ForceUpdateCanvases();
    }

    /** Update the file UI. */
    private void UpdateUi()
    {
        _updatingUi = true;

        PlayButton.interactable = File.canPlay;
        PlayButton.isOn = File.playing;

        _playLabel.text = File.playing ? "PAUSE" : "PLAY";
        PauseIcon.gameObject.SetActive(File.playing);

        RewindButton.interactable = File.canRewind;
        AddGroupButton.interactable = File.canAdd;
        RemoveGroupButton.interactable = File.canRemove && File.selectedGroup != null;
        ClearButton.interactable = File.canClear;
        SaveButton.interactable = File.canSave;

        var t = TimeSpan.FromSeconds(File.time);
        TimeText.color = File.playing ? ActiveTimeTextColor : InactiveTimeTextColor;
        TimeText.text = string.Format("{0:00}:{1:00}:{2:00}.{3:0}", 
            t.Hours, t.Minutes, t.Seconds, t.Milliseconds / 100);

        var d = TimeSpan.FromSeconds(File.endTime);
        DurationText.color = File.playing ? ActiveDurationTextColor : InactiveDurationTextColor;
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

        foreach (var group in File.groups)
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
        if (g == File.selectedGroup && e == null)
            g = null;

        Properties.Group = g;

        if (e == File.selectedEvent)
            Properties.ToggleEvent(e);
        else
            Properties.ExpandEvent(e);

        File.selectedGroup = g;
    }

    private void HandlePropertiesSelected(debugEventGroupPropertiesUi groupUi, debugEventPropertiesUi eventUi)
    {
        var e = eventUi ? eventUi.Event : null;
        Properties.ToggleEvent(e);
    }

}
