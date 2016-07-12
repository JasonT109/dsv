using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using Meg.JSON;

public class megGetSceneDataFiles : MonoBehaviour {

    public string filePath = @"C:\meg\";
    public string[] dataFiles;
    public int textVerticalOffset = 54;
    public GameObject textObject;
    public string fileToLoad = "";
    public TextMesh fileStatus;
    public GameObject loadButton;
    private bool canPress = true;

    public void getFiles()
    {
        DirectoryInfo info = new DirectoryInfo(filePath);
        FileInfo[] fileInfo = info.GetFiles();
        dataFiles = new string[fileInfo.Length];
        for (int i = 0; i < fileInfo.Length; i++)
        {
            //get the files as a string
            dataFiles[i] = fileInfo[i].ToString();
        }
        spawnContents();
    }

    public void setFileToLoad(GameObject button)
    {
        fileToLoad = button.GetComponentInChildren<Text>().text;
        fileStatus.text = fileToLoad;
    }

    void spawnContents()
    {
        int textVerticalPosition = 0;

        for (int i = 0; i < dataFiles.Length; i++)
        {
            //instantiate a new mesh object
            if (i == 0)
            {
                textObject.GetComponentInChildren<Text>().text = dataFiles[i];
                textVerticalPosition -= textVerticalOffset;
            }
            else
            {
                GameObject newText = Instantiate(textObject);
                newText.transform.SetParent(textObject.transform.parent);
                newText.transform.localPosition = new Vector3(3, textVerticalPosition, -5);
                newText.GetComponentInChildren<Text>().text = dataFiles[i];
                newText.transform.localScale = textObject.transform.localScale;
                textVerticalPosition -= textVerticalOffset;
            }
        }
    }

    IEnumerator wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canPress = true;
    }

	void Start ()
    {
        getFiles();
    }

    void Update()
    {
        if (loadButton.GetComponent<buttonControl>().pressed && canPress)
        {
            canPress = false;
            StartCoroutine(wait(0.2f));

            if (fileToLoad != "")
            {
                jsonData.megLoadJSONData(fileToLoad);
            }
        }
    }
}
