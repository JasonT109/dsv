using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class debugSceneFolderUi : MonoBehaviour
{

    // Enumerations
    // ------------------------------------------------------------

    /** Possible base paths for the folder. */
    public enum BaseFolder
    {
        Root,
        DataPath,
        StreamingAssetsPath,
        PersistentDataPath
    }


    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** File list for event files. */
    public debugFileListUi FileList;

    /** Label for the current scene. */
    public Text CurrentSceneText;

    /** Button used to load a file. */
    public Button LoadButton;


    [Header("Configuration")]

    /** Format to convert from a scene into a folder. */
    public string FolderFormat = @"C:\meg\Scene_{0:00}\Events";

    /** Folder's base path (if any). */
    public BaseFolder FolderLocation = BaseFolder.Root;

    /** Whether to create folders that don't exist. */
    public bool CreateFoldersIfNeeded = false;


    // Derived Properties
    // ------------------------------------------------------------

    /** Current scene. */
    public int Scene
    {
        get { return _scene; }
        set { SetScene(value); }
    }

    /** Current scene folder. */
    public string SceneFolder
        { get { return GetBaseFolder() + string.Format(FolderFormat, Scene); } }


    // Members
    // ------------------------------------------------------------

    /** The current scene. */
    private int _scene = 1;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        SetScene(1);
        LoadButton.interactable = false;
    }

    /** Updating. */
    private void Update()
    {
        LoadButton.interactable = FileList.SelectedEntry != null;
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Select the next scene. */
    public void NextScene()
    { Scene = Scene + 1; }

    /** Select the previous scene. */
    public void PreviousScene()
    { Scene = Mathf.Max(Scene - 1, 1); }


    // Private Methods
    // ------------------------------------------------------------

    /** Sets the current scene. */
    private void SetScene(int value)
    {
        _scene = Mathf.Max(value, 1);

        CurrentSceneText.text = string.Format("{0:00}", _scene);
        UpdateSceneFolder();
    }

    /** Set the current scene folder. */
    private void UpdateSceneFolder()
    {
        var folder = SceneFolder;
        if (!Directory.Exists(folder) && CreateFoldersIfNeeded)
            Directory.CreateDirectory(folder);

        FileList.Folder = folder;
    }

    /** Return the base folder location. */
    public string GetBaseFolder()
    {
        switch (FolderLocation)
        {
            case BaseFolder.Root:
                return "";
            case BaseFolder.DataPath:
                return Application.dataPath;
            case BaseFolder.PersistentDataPath:
                return Application.persistentDataPath;
            case BaseFolder.StreamingAssetsPath:
                return Application.streamingAssetsPath;
            default:
                return "";
        }
    }

}
