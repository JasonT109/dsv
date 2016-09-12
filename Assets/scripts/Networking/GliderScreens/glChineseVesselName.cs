using UnityEngine;
using System.Collections;
using Meg.Networking;

public class glChineseVesselName : MonoBehaviour
{
    public widgetText text;
    public string serverString;
    public string serverValue;

	void Update ()
    {
        if (!text)
            text = GetComponent<widgetText>();

        serverValue = serverUtils.GetServerDataAsText(serverString);
        
        switch(serverValue)
        {
            case "Sub 1":
                text.Text = "潜艇一号";
                break;
            case "Sub 2":
                text.Text = "潜艇二号";
                break;
            case "Glider 1":
                text.Text = "滑翔机一号";
                break;
            case "Glider 2":
                text.Text = "滑翔机二号";
                break;
            default:
                text.Text = "不能辨认";
                break;
        }
	}
}
