using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Events;

public class debugFileList : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    /** Prefab to spawn for each file entry. */
    public debugFileEntry FileEntryPrefab;

    /** The current folder. */
    public string Folder
    {
        get { return _folder; }
        set { SetFolder(value); }
    }

    /** The currently selected file. */

    public debugFileEntry SelectedEntry
    {
        get { return _selectedEntry; }
        set { SetSelectedEntry(value); }
    }


    // Members
    // ------------------------------------------------------------

    /** The current folder. */
    private string _folder;

    /** The current set of file entries. */
    private readonly List<debugFileEntry> _entries = new List<debugFileEntry>();

    /** The selected entry. */
    private debugFileEntry _selectedEntry;


    // Unity Methods
    // ------------------------------------------------------------

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

    private void SetSelectedEntry(debugFileEntry entry)
    {
        foreach (var e in _entries)
            entry.Selected = (e == entry);

        _selectedEntry = entry;
    }

    private void SetFolder(string value)
    {
        _folder = value;
        Refresh();
    }

    private void AddEntries(FileInfo[] info)
    {
        RemoveEntries();

        foreach (var f in info)
        {
            var entry = Instantiate(FileEntryPrefab);
            entry.transform.SetParent(transform, false);
            entry.FileInfo = f;
            entry.Button.onClick.AddListener(() => OnButtonClicked(entry));
            _entries.Add(entry);
        }
    }

    private void OnButtonClicked(debugFileEntry entry)
    {
        SelectedEntry = entry;
    }

    private void RemoveEntries()
    {
        foreach (var entry in _entries)
            Destroy(entry.gameObject);

        _entries.Clear();
        SetSelectedEntry(null);
    }

}
