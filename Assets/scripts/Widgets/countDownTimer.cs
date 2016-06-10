using UnityEngine;
using System.Collections;
using Meg.Networking;

public class countDownTimer : MonoBehaviour {

    public TextMesh cText;
    public GameObject sData;
    public float dueTimeHours = 1;
    public float dueTimeMins = 1;
    public float dueTimeSecs = 1;
	
    void Start ()
    {
        sData = GameObject.FindWithTag("ServerData");
        if (sData)
        {
            dueTimeHours = serverUtils.GetServerData("dueTimeHours");
            dueTimeMins = serverUtils.GetServerData("dueTimeMins");
            dueTimeSecs = serverUtils.GetServerData("dueTimeSecs");
        }
    }

	void Update ()
    {
        if (sData != null)
        {
            dueTimeHours = serverUtils.GetServerData("dueTimeHours");
            dueTimeMins = serverUtils.GetServerData("dueTimeMins");
            dueTimeSecs = serverUtils.GetServerData("dueTimeSecs");
        }
        else
        {
            sData = GameObject.FindWithTag("ServerData");
        }
        cText.text = (Mathf.Floor(dueTimeHours).ToString() + "h " + Mathf.Floor(dueTimeMins).ToString() + "m " + Mathf.Floor(dueTimeSecs).ToString() + "s");
    }
}
