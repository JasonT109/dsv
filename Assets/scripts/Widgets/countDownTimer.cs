using UnityEngine;
using Meg.Networking;

public class countDownTimer : MonoBehaviour {

    public TextMesh cText;
    public float hours = 1;
    public float mins = 1;
    public float seconds = 1;
    public bool diveTime = true;
    void Start ()
    {
        if (diveTime)
        {
            hours = serverUtils.GetServerData("diveTimeHours");
            mins = serverUtils.GetServerData("diveTimeMins");
            seconds = serverUtils.GetServerData("diveTimeSecs");
        }
        else
        {
            hours = serverUtils.GetServerData("dueTimeHours");
            mins = serverUtils.GetServerData("dueTimeMins");
            seconds = serverUtils.GetServerData("dueTimeSecs");
        }
    }

	void Update ()
    {
        if (diveTime)
        {
            hours = serverUtils.GetServerData("diveTimeHours");
            mins = serverUtils.GetServerData("diveTimeMins");
            seconds = serverUtils.GetServerData("diveTimeSecs");
        }
        else
        {
            hours = serverUtils.GetServerData("dueTimeHours");
            mins = serverUtils.GetServerData("dueTimeMins");
            seconds = serverUtils.GetServerData("dueTimeSecs");
        }
        cText.text = (Mathf.Floor(hours).ToString() + "h " + Mathf.Floor(mins).ToString() + "m " + Mathf.Floor(seconds).ToString() + "s");
    }
}
