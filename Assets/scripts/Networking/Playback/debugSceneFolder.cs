using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class debugSceneFolder : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** File list for event files. */
    public debugFileList FileList;

    /** Label for the current scene. */
    public Text CurrentSceneText;


    [Header("Configuration")]

    /** Format to convert from a scene into a folder. */
    public string FolderFormat = @"C:\meg\Scene_{0:00}\Events";


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
        { get { return string.Format(FolderFormat, Scene); } }


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
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        FileList.Folder = folder;
    }

}
