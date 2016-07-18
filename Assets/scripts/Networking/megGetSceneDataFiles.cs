using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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
    public TextMesh folderNameText;

    public void getFiles()
    {
        DirectoryInfo info = new DirectoryInfo(filePath + "Scene_" + folderNameText.text);
        if (info.Exists)
        {
            FileInfo[] fileInfo = info.GetFiles();
            dataFiles = new string[fileInfo.Length];
            for (int i = 0; i < fileInfo.Length; i++)
            {
                //get the files as a string
                dataFiles[i] = fileInfo[i].ToString();
            }
            spawnContents();
        }
        else
        {
            removeItems();
        }
    }

    public void setFileToLoad(GameObject button)
    {
        fileToLoad = button.GetComponentInChildren<Text>().text;
        fileStatus.text = fileToLoad;
    }

    void removeItems()
    {
        //get contents and delete all except the first one
        int numChildren = gameObject.transform.childCount;
        if (numChildren > 1)
        {
            for (int i = 1; i < numChildren; i++)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }
        GameObject firstChild = gameObject.transform.GetChild(0).gameObject;
        firstChild.GetComponentInChildren<Text>().text = "No items to display";
        fileToLoad = "";
    }

    void spawnContents()
    {
        removeItems();

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

        //set rect height
        RectTransform thisRect = gameObject.GetComponent<RectTransform>();

        thisRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, dataFiles.Length * textVerticalOffset);

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
