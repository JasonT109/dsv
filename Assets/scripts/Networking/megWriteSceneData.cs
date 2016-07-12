using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using Meg.JSON;
using Meg.Networking;

public class megWriteSceneData : MonoBehaviour
{
    public string filePath = @"C:\meg\";
    public string fileName = "arsenal_01_01_03_UTC";
    public GameObject saveButton;
    public GameObject saveText;
    public GameObject ovewriteDialog;
    public GameObject confirmButton;
    public GameObject cancelButton;

    public TextMesh vesselName;
    public TextMesh sceneNumber;
    public TextMesh shotNumber;
    public TextMesh takeNumber;

    private bool canPress = true;

    void setFileName(string sText)
    {
        saveText.GetComponent<TextMesh>().text = sText;
    }

    void saveFile(string saveFile)
    {
        JSONObject j = new JSONObject();

        j.AddField("depth", serverUtils.GetServerData("depth"));
        j.AddField("pitchAngle", serverUtils.GetServerData("pitchAngle"));
        j.AddField("yawAngle", serverUtils.GetServerData("yawAngle"));
        j.AddField("rollAngle", serverUtils.GetServerData("rollAngle"));
        j.AddField("velocity", serverUtils.GetServerData("velocity"));
        j.AddField("dueTimeHours", serverUtils.GetServerData("dueTimeHours"));
        j.AddField("dueTimeMins", serverUtils.GetServerData("dueTimeMins"));
        j.AddField("dueTimeSecs", serverUtils.GetServerData("dueTimeSecs"));
        j.AddField("Co2", serverUtils.GetServerData("Co2"));
        j.AddField("diveTimeHours", serverUtils.GetServerData("diveTimeHours"));
        j.AddField("diveTimeMins", serverUtils.GetServerData("diveTimeMins"));
        j.AddField("diveTimeSecs", serverUtils.GetServerData("diveTimeSecs"));
        j.AddField("b1", serverUtils.GetServerData("b1"));
        j.AddField("b2", serverUtils.GetServerData("b2"));
        j.AddField("b3", serverUtils.GetServerData("b3"));
        j.AddField("b4", serverUtils.GetServerData("b4"));
        j.AddField("b5", serverUtils.GetServerData("b5"));
        j.AddField("b6", serverUtils.GetServerData("b6"));
        j.AddField("b7", serverUtils.GetServerData("b7"));
        j.AddField("o1", serverUtils.GetServerData("o1"));
        j.AddField("o2", serverUtils.GetServerData("o2"));
        j.AddField("o3", serverUtils.GetServerData("o3"));
        j.AddField("o4", serverUtils.GetServerData("o4"));
        j.AddField("o5", serverUtils.GetServerData("o5"));
        j.AddField("o6", serverUtils.GetServerData("o6"));
        j.AddField("o7", serverUtils.GetServerData("o7"));
        j.AddField("crewHeartRate1", serverUtils.GetServerData("crewHeartRate1"));
        j.AddField("crewHeartRate2", serverUtils.GetServerData("crewHeartRate2"));
        j.AddField("crewHeartRate3", serverUtils.GetServerData("crewHeartRate3"));
        j.AddField("crewHeartRate4", serverUtils.GetServerData("crewHeartRate4"));
        j.AddField("crewHeartRate5", serverUtils.GetServerData("crewHeartRate5"));
        j.AddField("crewHeartRate6", serverUtils.GetServerData("crewHeartRate6"));
        j.AddField("crewBodyTemp1", serverUtils.GetServerData("crewBodyTemp1"));
        j.AddField("crewBodyTemp2", serverUtils.GetServerData("crewBodyTemp2"));
        j.AddField("crewBodyTemp3", serverUtils.GetServerData("crewBodyTemp3"));
        j.AddField("crewBodyTemp4", serverUtils.GetServerData("crewBodyTemp4"));
        j.AddField("crewBodyTemp5", serverUtils.GetServerData("crewBodyTemp5"));
        j.AddField("crewBodyTemp6", serverUtils.GetServerData("crewBodyTemp6"));

        jsonData.megSaveJSONData(saveFile, j);
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

            saveFile(filePath + fileName);
        }
    }
}

