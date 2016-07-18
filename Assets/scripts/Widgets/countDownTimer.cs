using UnityEngine;
using Meg.Networking;

public class countDownTimer : widgetText
{

    public TextMesh cText;
    public float hours = 1;
    public float mins = 1;
    public float seconds = 1;
    public bool diveTime = true;

    void Start ()
    {
        if (cText)
            TextMesh = cText;

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

        string fmt = "00";
        Text = (Mathf.Floor(hours).ToString(fmt) + ":" + Mathf.Floor(mins).ToString(fmt) + ":" + Mathf.Floor(seconds).ToString(fmt));
    }
}
