using UnityEngine;
using System.Collections;

public class crewVitals : MonoBehaviour {

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

    private float timeStep = 0.5f;
    private float t;

    // Use this for initialization
    void Start ()
    {
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
    }
}
