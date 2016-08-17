using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Meg.Scene;

public class debugSceneFilesUi : MonoBehaviour
{


    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Folder list for scenes. */
    public debugFolderListUi Scenes;

    /** File list for the current scene. */
    public debugFileListUi Files;

    /** Button for loading a scene file. */
    public Button LoadButton;


    [Header("Configuration")]

    /** The set of possible scene folder types. */
    public SceneFolderType[] FolderTypes;

    /** Initial folder type (name). */
    public string InitialFolderType;


    // Structures
    // ------------------------------------------------------------

    /** A folder containing files relating to a scene. */
    [System.Serializable]
    public struct SceneFolderType
    {
        public string Name;
        public string Pattern;
    }

    /** The current scene folder type. */
    public SceneFolderType FolderType
    {
        get { return _folderType; }
        set { SetFolderType(value); }
    }

    /** The current scene folder name. */
    public string SceneName
    {
        get { return _sceneName; }
        set { SetSceneName(value); }
    }

    /** The current scene number (integer). */
    public int Scene { get; private set; }
    

    // Members
    // ------------------------------------------------------------

    /** The current folder type. */
    private SceneFolderType _folderType;

    /** The current scene folder name. */
    private string _sceneName;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Awake()
    {
        if (!string.IsNullOrEmpty(InitialFolderType))
            SetFolderTypeByName(InitialFolderType);
    }

    /** Updating. */
    private void Update()
    {
        UpdateUi();
    }

    
    // Public Methods
    // ------------------------------------------------------------

    /** Set the current folder type by name. */
    public void SetFolderTypeByName(string value)
        { SetFolderType(FolderTypes.FirstOrDefault(f => f.Name == value)); }

    /** Set the current folder type. */
    public void SetFolderType(SceneFolderType value)
    {
        _folderType = value;
        UpdateFiles();
    }
    
    /** Set the current scene name. */
    public void SetSceneName(string value)
    {
        _sceneName = value;

        var match = Regex.Match(_sceneName, @"(\d+)");
        if (match.Success)
            Scene = int.Parse(match.Groups[1].Value);

        UpdateFiles();
    }

    /** Load a scene file. */
    public void LoadSelected()
    {
        if (Files.SelectedEntry == null)
            return;

        var info = Files.SelectedEntry.FileInfo;
        if (!File.Exists(info.FullName))
            return;

        DialogManager.Instance.ShowYesNo("LOAD SCENE FILE?",
            string.Format("Are you sure you wish to load the scene file '{0}'?", info.Name),
            result =>
            {
                if (result != DialogYesNo.DialogResult.Yes)
                    return;

                megSceneFile.LoadFromFile(info.FullName);
            });
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Update the file list to match current scene and folder type. */
    private void UpdateFiles()
    {
        // Replace config folders in the folder pattern expression.
        var pattern = Configuration.ExpandPaths(FolderType.Pattern);

        // Update the file listing with new folder.
        Files.Folder = string.Format(pattern, Scene);
    }

    /** Update the UI elements. */
    private void UpdateUi()
    {
        LoadButton.interactable = Files.SelectedEntry != null;
    }

}
