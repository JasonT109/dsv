using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.Events;

public class debugFolderListUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    /** Prefab to spawn for each folder entry. */
    public debugFolderEntryUi FolderEntryPrefab;

    /** The current root folder. */
    public string Folder
    {
        get { return _folder; }
        set { SetFolder(value); }
    }

    /** The currently selected file. */
    public debugFolderEntryUi SelectedEntry
    {
        get { return _selectedEntry; }
        set { SetSelectedEntry(value); }
    }

    /** Filter for files. */
    public string Filter = "";

    /** Optional folder path to start in. */
    public string InitialFolder;


    // Events
    // ------------------------------------------------------------

    /** Event type relating to a folder. */
    [System.Serializable]
    public class FolderEvent : UnityEvent<string> { }

    /** Event fired when a folder is selected (passes the full path). */
    public FolderEvent OnSelectedFullName = new FolderEvent();

    /** Event fired when a folder is selected (passes just the folder name). */
    public FolderEvent OnSelected = new FolderEvent();


    // Members
    // ------------------------------------------------------------

    /** The current folder. */
    private string _folder;

    /** The current set of file entries. */
    private readonly List<debugFolderEntryUi> _entries = new List<debugFolderEntryUi>();

    /** The selected entry. */
    private debugFolderEntryUi _selectedEntry;


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
            AddEntries(info.GetDirectories());
    }


    // Private Methods
    // ------------------------------------------------------------

    private void SetSelectedEntry(debugFolderEntryUi entry)
    {
        _selectedEntry = entry;

        foreach (var e in _entries)
            e.Selected = (e == entry);

        if (entry != null && OnSelected != null)
            OnSelected.Invoke(entry.DirectoryInfo.Name);

        if (entry != null && OnSelectedFullName != null)
            OnSelectedFullName.Invoke(entry.DirectoryInfo.FullName);
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

    private void AddEntries(DirectoryInfo[] info)
    {
        RemoveEntries();

        foreach (var f in info)
        {
            if (!Regex.IsMatch(f.FullName, Filter))
                continue;

            var entry = Instantiate(FolderEntryPrefab);
            _entries.Add(entry);

            entry.transform.SetParent(transform, false);
            entry.DirectoryInfo = f;
            entry.Toggle.onValueChanged.AddListener(
                on => OnEntryChanged(entry, on));
        }
    }

    private void OnEntryChanged(debugFolderEntryUi entry, bool value)
    {
        if (value)
            SelectedEntry = entry;
        else if (entry == SelectedEntry)
            SelectedEntry = null;
    }

    private void RemoveEntries()
    {
        foreach (var e in _entries)
            Destroy(e.gameObject);

        _entries.Clear();
        SetSelectedEntry(null);
    }

}
