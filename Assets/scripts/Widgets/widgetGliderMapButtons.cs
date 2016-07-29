using UnityEngine;
using System.Collections;
using Meg.Networking;

public class widgetGliderMapButtons : MonoBehaviour
{
    public buttonControl mapCentreVessel;
    public buttonControl mapContourButton;
    public buttonControl map3dButton;
    public buttonControl mapLabelButton;
	
	// Update is called once per frame
	void Update ()
    {
        //switch to contour map
        if (map3dButton.active && serverUtils.GetGliderButtonState(0))
        {
            mapContourButton.RemoteToggle();
        }

        //switch to 3d map
        if (mapContourButton.active && serverUtils.GetGliderButtonState(0))
        {
            map3dButton.RemoteToggle();
        }

        //turn on labels
        if (serverUtils.GetGliderButtonState(2))
        {
            mapLabelButton.RemoteToggle();
        }

        //recenter vessel
            if (serverUtils.GetGliderButtonState(1))
        {
                mapCentreVessel.RemotePress();
        }
    }
}
