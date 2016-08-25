using UnityEngine;
using System.Collections;
using Meg.Networking;

public class crewVitals : MonoBehaviour {

    public int crewMemberNumber = 1;
    private float bpm = 86;
    private float respitory = 24;
    private float bodyTemp = 36.5f;
    private float bloodP1 = 90;
    private float bloodP2 = 60;
    public float endTidal = 5.4f;
    public float valueFluctMul = 1.0f;
    public float bp1multiplier = 1.33f;
    public float bp2multiplier = 0.8f;
    public TextMesh bpmText;
    public TextMesh respText;
    public TextMesh bodyTempText;
    public TextMesh bloodPText;
    public TextMesh endTidalText;
    public GameObject heartRateMesh;
    public GameObject warningButton;
    public GameObject noSignalText;

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
        noSignalText.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.time > t)
        {
            GetLatestStats();

            if (bpm == 0)
            {
                respitory = 0f;
                bloodP1 = 0f;
                bloodP2 = 0f;
                endTidal = 0f;
                bpmText.text = "--";
                heartRateMesh.GetComponent<heartrate>().bpm = bpm;
                respText.text = "--";
                bloodPText.text = "--/--";
                endTidalText.text = "--";
                if (bodyTemp > 0)
                {
                    bodyTempText.text = bodyTemp.ToString("n1");
                }
                else
                {
                    bodyTempText.text = "--";
                    noSignalText.SetActive(true);
                }
            }
            else
            {
                //fluctuate BMP
                float newBpm = bpm * Random.Range(1, valueFluctMul);
                heartRateMesh.GetComponent<heartrate>().bpm = bpm;
                bpmText.text = Mathf.RoundToInt(newBpm).ToString("D3");

                //calculate respiratory rate
                respitory = bpm * 0.2f;

                //fluctuate respiratory rate
                float newResp = respitory * Random.Range(1, valueFluctMul);
                respText.text = Mathf.RoundToInt(newResp).ToString("D3");

                //fluctuate body temp
                float newBodyTemp = bodyTemp * Random.Range(1, valueFluctMul);
                bodyTempText.text = newBodyTemp.ToString("n1");

                //end tidal is affected by CO2 in the cabin
                float newEndTidal = endTidal + (serverUtils.GetServerData("Co2") * 0.25f);
                endTidalText.text = newEndTidal.ToString("n1");

                //calculate bp and fluctuate
                bloodP1 = Mathf.Round( (bpm * bp1multiplier) * Random.Range(1, valueFluctMul));
                bloodP2 = Mathf.Round( (bpm * bp2multiplier) * Random.Range(1, valueFluctMul));
                bloodPText.text = (bloodP1.ToString() + "/" + bloodP2.ToString());
            }
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
