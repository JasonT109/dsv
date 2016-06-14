using UnityEngine;
using System.Collections;
using Meg.Networking;

public class crewVitals : MonoBehaviour {

    public int crewMemberNumber = 1;
    public float bpm = 86;
    public float respitory = 24;
    public float bodyTemp = 36.5f;
    public float bloodP1 = 90;
    public float bloodP2 = 60;
    public float endTidal = 5.4f;
    public float valueFluctMul = 1.0f;
    public TextMesh bpmText;
    public TextMesh respText;
    public TextMesh bodyTempText;
    public TextMesh bloodPText;
    public TextMesh endTidalText;
    public GameObject heartRateMesh;
    public GameObject warningButton;

    private float timeStep = 0.5f;
    private float t;

    void GetLatestStats()
    {
        switch (crewMemberNumber)
        {
            case 1:
                bpm = serverUtils.GetServerData("crewHeartRate1");
                bodyTemp = serverUtils.GetServerData("crewBodyTemp1");
                break;
            case 2:
                bpm = serverUtils.GetServerData("crewHeartRate2");
                bodyTemp = serverUtils.GetServerData("crewBodyTemp2");
                break;
            case 3:
                bpm = serverUtils.GetServerData("crewHeartRate3");
                bodyTemp = serverUtils.GetServerData("crewBodyTemp3");
                break;
            case 4:
                bpm = serverUtils.GetServerData("crewHeartRate4");
                bodyTemp = serverUtils.GetServerData("crewBodyTemp4");
                break;
            case 5:
                bpm = serverUtils.GetServerData("crewHeartRate5");
                bodyTemp = serverUtils.GetServerData("crewBodyTemp5");
                break;
            case 6:
                bpm = serverUtils.GetServerData("crewHeartRate6");
                bodyTemp = serverUtils.GetServerData("crewBodyTemp6");
                break;
        }
    }

    // Use this for initialization
    void Start ()
    {
        GetLatestStats();
        heartRateMesh.GetComponent<heartrate>().bpm = bpm;
        bpmText.text = bpm.ToString();
        respText.text = respitory.ToString();
        bodyTempText.text = bodyTemp.ToString();
        bloodPText.text = (bloodP1.ToString() + "/" + bloodP2.ToString());
        endTidalText.text = endTidal.ToString();
        t = Time.time + Random.Range(0, 0.25f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.time > t)
        {
            GetLatestStats();

            float newBpm = bpm * Random.Range(1, valueFluctMul);
            heartRateMesh.GetComponent<heartrate>().bpm = bpm;
            bpmText.text = Mathf.RoundToInt(newBpm).ToString("D3");

            float newResp = respitory * Random.Range(1, valueFluctMul);
            respText.text = Mathf.RoundToInt(newResp).ToString("D3");

            float newBodyTemp = bodyTemp * Random.Range(1, valueFluctMul);
            bodyTempText.text = newBodyTemp.ToString("n1");

            bloodPText.text = (bloodP1.ToString() + "/" + bloodP2.ToString());

            float newEndTidal = endTidal * Random.Range(1, valueFluctMul);
            endTidalText.text = endTidal.ToString("n1");

            t = Time.time + timeStep;
        }
        if (bpm > 125f || bpm < 45)
        {
            warningButton.GetComponent<buttonControl>().warning = true;
        }
        else
        {
            warningButton.GetComponent<buttonControl>().warning = false;
        }
    }
}
