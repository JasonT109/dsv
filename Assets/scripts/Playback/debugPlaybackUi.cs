using System.IO;
using Meg.EventSystem;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class debugPlaybackUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** The event scene folder list. */
    public debugSceneFolderUi SceneFolder;

    /** The event file UI. */
    public debugEventFileUi EventFileUi;



    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Awake()
    {
        // Inform event manager about the current event file.
        megEventManager.Instance.SetCurrentFile(EventFileUi.File);
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Load the selected event file. */
    public void Load()
    {
        var entry = SceneFolder.FileList.SelectedEntry;
        if (!entry || !entry.FileInfo.Exists)
            return;

        EventFileUi.Load(entry.FileInfo);
        SceneFolder.FileList.SelectedEntry = null;
    }

    // Private Methods
    // ------------------------------------------------------------




}

