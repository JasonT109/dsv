using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Meg.EventSystem;
using Meg.Networking;

public class debugEventFileUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Header for the current event file. */
    public Text Header;

    /** Container for event groups. */
    public Transform Groups;

    /** Playback button. */
    public Toggle PlayButton;

    /** Rewind button. */
    public Button RewindButton;

    /** Pause icon. */
    public Graphic PauseIcon;

    /** Event properties. */
    public debugEventPropertiesUi Properties;


    [Header("Prefabs")]

    /** Prefab to use for an event group UI node. */
    public debugEventGroupUi EventGroupUiPrefab;


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
            _file.Stop();

            var events = new megEventFile();
            events.LoadFromFile(f.FullName);
            _file = events;
            AddGroups(events);

            var label = Path.GetFileNameWithoutExtension(f.Name);
            label = Regex.Replace(label, "[A-Z]", " $0");
            Header.text = label.ToUpper();
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


    // Private Methods
    // ------------------------------------------------------------

    /** Update the file UI. */
    private void UpdateUi()
    {
        _updatingUi = true;

        PlayButton.interactable = _file.canPlay;
        PlayButton.isOn = _file.playing;

        _playLabel.text = _file.playing ? "PAUSE" : "PLAY";
        PauseIcon.gameObject.SetActive(_file.playing);

        RewindButton.interactable = _file.canRewind;

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
