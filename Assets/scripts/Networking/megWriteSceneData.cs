using UnityEngine;
using System.Collections;
using System.IO;
using Meg.Networking;
using Meg.Scene;

public class megWriteSceneData : MonoBehaviour
{
    public string filePath
        { get { return Configuration.Get("save-folder", @"C:\meg\"); } }

    public string fileName = "arsenal_01_01_03_UTC";
    public GameObject saveButton;
    public GameObject saveText;

    public TextMesh vesselName;
    public TextMesh sceneNumber;
    public TextMesh shotNumber;
    public TextMesh takeNumber;

    public GameObject readFilesObject;

    private bool canPress = true;

    void setFileName(string sText)
    {
        saveText.GetComponent<TextMesh>().text = sText;
    }

    void saveFile(string saveFile)
    {
        var path = string.Format(@"{0}Scene_{1}\{2}", filePath, sceneNumber.text, saveFile);
        megSceneFile.SaveToFile(path);

        readFilesObject.GetComponent<megGetSceneDataFiles>().getFiles();
    }

    IEnumerator wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canPress = true;
    }

    void Update()
    {
        fileName = (vesselName.text + "_" + sceneNumber.text + "_" + shotNumber.text + "_" + takeNumber.text + "_UTC.json");

        saveText.GetComponent<TextMesh>().text = fileName;

        if (saveButton.GetComponent<buttonControl>().pressed && canPress)
        {
            canPress = false;
            StartCoroutine(wait(0.2f));

            serverUtils.SetServerData("scene", sceneNumber.text);
            serverUtils.SetServerData("shot", shotNumber.text);
            serverUtils.SetServerData("take", takeNumber.text);

            saveFile(fileName);
        }
    }

}

