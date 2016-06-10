using UnityEngine;
using System.Collections;
using Meg.Networking;

public class widgetDataLog : MonoBehaviour {

    public string[] dataFeedWarning;
    public string[] dataFeedNormal;
    private string[] dataFeed;
    public TextMesh dataText;
    private float dataSpeed = 0.4f;
    public float normalDataSpeed = 0.4f;
    public float warningDataSpeed = 0.15f;
    private float feedTime;
    public int visibleLines = 10;
    public bool switchOnWarning = false;
    public string warningParam;
    public float warningValue;
    private GameObject sData;
    private bool switched = false;

	// Use this for initialization
	void Start () {
        dataSpeed = normalDataSpeed;
        feedTime = Time.time + dataSpeed;
        dataFeed = dataFeedNormal;
	}
	
	// Update is called once per frame
	void Update () {

        if (switchOnWarning)
        {
            if (sData == null)
            {
                sData = GameObject.FindWithTag("ServerData");
            }
            if (serverUtils.GetServerData(warningParam) <= warningValue && !switched)
            {
                dataSpeed = warningDataSpeed;
                dataFeed = dataFeedWarning;
                switched = true;
            }
            else if (serverUtils.GetServerData(warningParam) > warningValue && switched)
            {
                dataSpeed = normalDataSpeed;
                dataFeed = dataFeedNormal;
                switched = false;
            }
        }

        if (visibleLines > dataFeed.Length)
        {
            visibleLines = dataFeed.Length;
        }

	    if (dataFeed.Length > 0)
        {
            //we can do something with this
            if (dataText)
            {
                if (Time.time > feedTime)
                {
                    feedTime = Time.time + dataSpeed;

                    string firstLine = dataFeed[0];
                    System.Array.Copy(dataFeed, 1, dataFeed, 0, dataFeed.Length - 1);
                    dataFeed[dataFeed.Length - 1] = firstLine;
                    
                    string newTextData = "";
                    for (int i = 0; i < visibleLines; i++)
                    {
                        newTextData += dataFeed[i] + "\n";
                    }
                    dataText.text = newTextData;
                }
            }
        }
	}
}
