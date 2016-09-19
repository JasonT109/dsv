using UnityEngine;
using System.Collections;
using System.IO;
using Meg.Networking;
using Meg.Parameters;
using Meg.Scene;
using UnityEngine.UI;

public class debugSceneSaveUi : MonoBehaviour
{

    [Header("Components")]

    /** The container for scene/shot/take sliders. */
    public Transform ParameterContainer;

    /** Scene files. */
    public debugSceneFilesUi SceneFiles;

    /** Labels for scene, shot, take. */
    public Text SceneLabel;
    public Text ShotLabel;
    public Text TakeLabel;

    /** Text input for entering an optional file suffix. */
    public InputField SuffixInput;


    [Header("Configuration")]

    /** Prefab used to create scene/shot/take editing UI elements. */
    public debugParameterValueUi ParameterUiPrefab;


    /** Initialization. */
	private void Start() 
    {
        AddParameter("scene");
        AddParameter("shot");
        AddParameter("take");
    }

    /** Updating. */
    private void Update()
    {
        SceneLabel.text = string.Format("SCENE {0:N0}", serverUtils.GetServerData("scene"));
        ShotLabel.text = string.Format("SHOT {0:N0}", serverUtils.GetServerData("shot"));
        TakeLabel.text = string.Format("TAKE {0:N0}", serverUtils.GetServerData("take"));
    }

    /** Move to next scene. */
    public void NextScene()
        { IncrementParameter("scene"); }

    /** Move to next shot. */
    public void NextShot()
        { IncrementParameter("shot"); }

    /** Move to next take. */
    public void NextTake()
        { IncrementParameter("take"); }

    /** Request to save the current scene state to file. */
    public void Save()
    {
        // Ask for a filename to save to.
        var path = megSceneFile.GetSceneSaveFilename(SuffixInput.text);
        var filename = Path.GetFileName(path);

        // Get curren scene, shot, take.
        var scene = serverUtils.GetServerData("scene");
        var shot = serverUtils.GetServerData("shot");
        var take = serverUtils.GetServerData("take");

        // Prompt user to save.
        DialogManager.Instance.ShowYesNo(
            string.Format("SAVE [SCENE {0:N0} : SHOT : {1:N0} : TAKE {2:N0}]?", scene, shot, take),
            string.Format("Are you sure you wish to save the scene to file '{0}'?", filename),
            result =>
            {
                if (result != DialogYesNo.Result.Yes)
                    return;

                megSceneFile.SaveToFile(path);
                SceneFiles.Refresh();
            });
    }

    /** Add a UI element to edit the given server parameter. */
    private void AddParameter(string serverParam)
    {
        var param = new megParameterValue {serverParam = serverParam};
        var ui = Instantiate(ParameterUiPrefab);
        ui.transform.SetParent(ParameterContainer, false);
        ui.Parameter = param;
    }

    /** Increment the given parameter. */
    private void IncrementParameter(string serverParam, int delta = 1)
    {
        var value = serverUtils.GetServerData(serverParam) + delta;
        serverUtils.PostServerData(serverParam, value);
    }

    
}
