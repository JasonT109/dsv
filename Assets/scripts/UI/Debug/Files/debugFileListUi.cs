using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.Events;

public class debugFileListUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    /** Prefab to spawn for each file entry. */
    public debugFileEntryUi FileEntryPrefab;

    /** The current folder. */
    public string Folder
    {
        get { return _folder; }
        set { SetFolder(value); }
    }

    /** The currently selected file. */
    public debugFileEntryUi SelectedEntry
    {
        get { return _selectedEntry; }
        set { SetSelectedEntry(value); }
    }

    /** Filter for files. */
    public string Filter = ".json$";

    /** Optional folder path to start in. */
    public string InitialFolder;


    // Members
    // ------------------------------------------------------------

    /** The current folder. */
    private string _folder;

    /** The current set of file entries. */
    private readonly List<debugFileEntryUi> _entries = new List<debugFileEntryUi>();

    /** The set of instantiated file entries. */
    private readonly List<debugFileEntryUi> _instances = new List<debugFileEntryUi>();

    /** The selected entry. */
    private debugFileEntryUi _selectedEntry;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        if (!string.IsNullOrEmpty(InitialFolder))
            Folder = InitialFolder;
    }

    /** Enabling. */
    private void OnEnable()
    {
        Refresh();
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Refresh the file list from current folder. */
    public void Refresh()
    {
        RemoveEntries();

        if (string.IsNullOrEmpty(Folder))
            return;

        var info = new DirectoryInfo(Folder);
        if (info.Exists)
            AddEntries(info.GetFiles());
    }


    // Private Methods
    // ------------------------------------------------------------

    private void SetSelectedEntry(debugFileEntryUi entry)
    {
        _selectedEntry = entry;

        foreach (var e in _entries)
            e.Selected = (e == entry);
    }

    private void SetFolder(string value)
    {
        if (string.IsNullOrEmpty(value))
            return;

        // Look for and replace common configuration folders.
        value = Configuration.ExpandedPath(value);

        _folder = value;
        Refresh();
    }

    private void AddEntries(FileInfo[] info)
    {
        RemoveEntries();

        foreach (var f in info)
        {
            if (!Regex.IsMatch(f.FullName, Filter))
                continue;

            var entry = GetEntry(_entries.Count);
            entry.FileInfo = f;

            _entries.Add(entry);
        }
    }

    private debugFileEntryUi GetEntry(int i)
    {
        if (i >= _instances.Count)
        {
            var entry = Instantiate(FileEntryPrefab);
            entry.Toggle.onValueChanged.AddListener(
                on => OnEntryChanged(entry, on));

            entry.transform.SetParent(transform, false);
            _instances.Add(entry);
        }

        _instances[i].gameObject.SetActive(true);

        return _instances[i];
    }

    private void OnEntryChanged(debugFileEntryUi entry, bool value)
    {
        if (!entry || !entry.gameObject.activeInHierarchy)
            return;

        if (value)
            SelectedEntry = entry;
        else if (entry == SelectedEntry)
            SelectedEntry = null;
    }

    private void RemoveEntries()
    {
        foreach (var e in _entries)
            e.gameObject.SetActive(false);

        _entries.Clear();
        SetSelectedEntry(null);
    }

}
