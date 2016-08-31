using UnityEngine;
using System.Collections;
using Meg.Networking;

public class DCCWindowTitle : MonoBehaviour
{
    public bool prefixVesselName = true;

    private DynamicText t;
    private string originalText;
    private float checkTime = 2f;
    private float timer = 2f;
    private string _defaultPrefix = "DIVE CONTROL";

    void Awake ()
    {
        t = gameObject.GetComponent<DynamicText>();
        originalText = t.GetText();
	    _defaultPrefix = Configuration.Get("dcc-window-title-prefix", "DIVE CONTROL");
    }
	
    void Update()
    {
        if (prefixVesselName && Time.time > timer)
        {
            var prefix = _defaultPrefix;
            if (serverUtils.GetServerBool("dccVesselNameInTitle"))
                prefix = serverUtils.GetPlayerVesselName();

            t.SetText(prefix.ToUpper() + " - " + originalText);
            timer += checkTime;
        }
    }
}
