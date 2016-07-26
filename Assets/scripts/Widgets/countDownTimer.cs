using System;
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

        var time = serverUtils.GetServerData(diveTime ? "diveTime" : "dueTime");
        var span = TimeSpan.FromSeconds(time);

        hours = span.Hours;
        mins = span.Minutes;
        seconds = span.Seconds;
    }

	void Update ()
    {
        var time = serverUtils.GetServerData(diveTime ? "diveTime" : "dueTime");
        var span = TimeSpan.FromSeconds(time);

        hours = span.Hours;
        mins = span.Minutes;
        seconds = span.Seconds;

        Text = string.Format("{0:00}:{1:00}:{2:00}", hours, mins, seconds);
    }

}
