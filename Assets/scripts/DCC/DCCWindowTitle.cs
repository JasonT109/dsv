using UnityEngine;
using System.Collections;
using Meg.Networking;

public class DCCWindowTitle : MonoBehaviour
{
    public bool prefixVesselName = true;

    private DynamicText t;
    private string originalText;
    private string vesselName;
    private float checkTime = 2f;
    private float timer = 2f;

	void Awake ()
    {
        t = gameObject.GetComponent<DynamicText>();
        originalText = t.GetText();
	}
	
    void Update()
    {
        if (prefixVesselName && Time.time > timer)
        {
            int p = serverUtils.GetPlayerVessel();
            vesselName = serverUtils.GetVesselName(p);

            t.SetText(vesselName.ToUpper() + " - " + originalText);

            timer += checkTime;
        }
    }
}
