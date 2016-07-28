using System.IO;
using Meg.EventSystem;
using UnityEngine;
using UnityEngine.UI;

public class debugPlayback : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** The event scene folder list. */
    public debugSceneFolder SceneFolder;

    /** Header for the current event file. */
    public Text CurrentFileHeader;


    // Members
    // ------------------------------------------------------------

    /** The current event file. */
    private megEventFile Events = new megEventFile();


    // Unity Methods
    // ------------------------------------------------------------



    // Public Methods
    // ------------------------------------------------------------

    /** Load the selected event file. */
    public void Load()
    {
        var entry = SceneFolder.FileList.SelectedEntry;
        if (!entry || !entry.FileInfo.Exists)
            return;

        Events = new megEventFile();
        Events.LoadFromFile(entry.FileInfo.FullName);
    }

}

