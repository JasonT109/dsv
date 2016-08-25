using UnityEngine;
using System.Collections;

public class widgetWarningFlashSync : MonoBehaviour
{

    public float warningFlashSpeed = 3.0f;
    public float warningFlashPause = -1.0f;
    public float timeIndex;

	// Update is called once per frame
	void Update ()
    {
        timeIndex += Time.deltaTime * warningFlashSpeed;
        if (timeIndex > 1.0f)
        {
            timeIndex = warningFlashPause;
        }
    }
}
