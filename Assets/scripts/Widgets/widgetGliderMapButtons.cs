using UnityEngine;
using System.Collections;
using Meg.Networking;

public class widgetGliderMapButtons : MonoBehaviour
{
    public buttonControl mapLabelButton;
    private bool contours = false;
    private bool canTrigger = true;


    IEnumerator wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canTrigger = true;
    }

	// Update is called once per frame
	void Update ()
    {
        //switch to contour map
        if (!contours && serverUtils.GetGliderButtonState(0) && canTrigger)
        {
            canTrigger = false;
            StartCoroutine(wait(0.1f));
            contours = true;
            serverUtils.SetServerData("initiateMapEvent", 1f);
            serverUtils.SetMapEventName("MapContours");
            Debug.Log("Switching to contour map.");
        }

        //switch to 3d map
        if (contours && serverUtils.GetGliderButtonState(0) && canTrigger)
        {
            canTrigger = false;
            StartCoroutine(wait(0.1f));
            contours = false;
            serverUtils.SetServerData("initiateMapEvent", 1f);
            serverUtils.SetMapEventName("Map3d");
            Debug.Log("Switching to 3d map.");
        }

        //turn on labels
        if (serverUtils.GetGliderButtonState(2) && canTrigger)
        {
            canTrigger = false;
            StartCoroutine(wait(0.1f));
            mapLabelButton.RemotePress();
        }

        //recenter vessel
        if (serverUtils.GetGliderButtonState(1) && canTrigger)
        {
            canTrigger = false;
            StartCoroutine(wait(0.1f));
            serverUtils.SetServerData("initiateMapEvent", 1f);
            serverUtils.SetMapEventName("RecenterVessel");
            Debug.Log("Re-centering vessel.");
        }
    }
}
