using UnityEngine;
using UnityEngine.Networking;
using Meg.Networking;

public class textValueFromServer : widgetText
{
    public string linkDataString = "depth";
    public float updateTick = 0.2f;
    private float nextUpdate = 0;

    public string format = "";

    void Start()
    {
        nextUpdate = Time.time;
    }

    void Update()
    {
        if (Time.time > nextUpdate)
        {
            nextUpdate = Time.time + updateTick;

            var value = serverUtils.GetServerDataAsText(linkDataString);
            if (string.IsNullOrEmpty(format))
                Text = value;
            else
                Text = string.Format(format, value);
        }
    }
}
