using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Meg.Scene;

public class debugSceneFilesUi : MonoBehaviour
{

    public const string ArchiveFolder = "{archive-folder}";
    public const string LocalFolder = "{save-folder}";


    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Folder list for scenes. */
    public debugFolderListUi Scenes;

    /** File list for the current scene. */
    public debugFileListUi Files;

    /** Button for loading a scene file. */
    public Button LoadButton;

    /** Title for the scene file list. */
    public Text Title;

    /** Title for the scene folder list. */
    public Text FoldersTitle;

    /** Button for selecting the local save folder. */
    public Toggle LocalFolderToggle;

    /** Button for selecting the archived save folder. */
    public Toggle ArchiveFolderToggle;


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

    /** Current scene folder type. */
    public SceneFolderLocation Location = SceneFolderLocation.Local;

    public enum SceneFolderLocation
    {
        Local,
        Archives
    }
    

    // Members
    // ------------------------------------------------------------

    /** The current folder type. */
    private SceneFolderType _folderType;

    /** The current scene folder name. */
    private string _sceneName;

    /** Whether UI is being updated. */
    private bool _updating;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Awake()
    {
        LocalFolderToggle.onValueChanged.AddListener(OnLocalFolderClicked);
        ArchiveFolderToggle.onValueChanged.AddListener(OnArchiveFolderClicked);

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

        DialogManager.Instance.ShowYesNo(
            string.Format("LOAD {0} FILE?", FolderType.Name.ToUpper()),
            string.Format("Are you sure you wish to load the {0} file '{1}'?", FolderType.Name, info.Name),
            result =>
            {
                if (result != DialogYesNo.Result.Yes)
                    return;

                megSceneFile.LoadFromFile(info.FullName);
            });
    }

    /** Refresh the scene file/folder lists. */
    public void Refresh()
    {
        Scenes.Refresh();
        Files.Refresh();
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Update the file list to match current scene and folder type. */
    private void UpdateFiles()
    {
        Title.text = FolderType.Name.ToUpper() + " FILES";

        // Replace config folders in the folder pattern expression.
        var pattern = Configuration.ExpandedPath(FolderType.Pattern);

        // Update the file listing with new folder.
        Files.Folder = Scenes.Folder + "/" + string.Format(pattern, Scene);
    }

    /** Update the UI elements. */
    private void UpdateUi()
    {
        _updating = true;

        LoadButton.interactable = Files.SelectedEntry != null;
        LocalFolderToggle.isOn = Location == SceneFolderLocation.Local;
        ArchiveFolderToggle.isOn = Location == SceneFolderLocation.Archives;

        _updating = false;
    }

    private void OnArchiveFolderClicked(bool value)
    {
        if (!value || _updating)
            return;

        Location = SceneFolderLocation.Archives;
        UpdateLocationFolder();
    }

    private void OnLocalFolderClicked(bool value)
    {
        if (!value || _updating)
            return;

        Location = SceneFolderLocation.Local;
        UpdateLocationFolder();
    }

    private void UpdateLocationFolder()
    {
        Scenes.SelectedEntry = null;

        switch (Location)
        {
            case SceneFolderLocation.Archives:
                Scenes.Folder = ArchiveFolder;
                FoldersTitle.text = "SCENES (ARCHIVED)";
                break;

            case SceneFolderLocation.Local:
                Scenes.Folder = LocalFolder;
                FoldersTitle.text = "SCENES (LOCAL)";
                break;
        }
    }


}
