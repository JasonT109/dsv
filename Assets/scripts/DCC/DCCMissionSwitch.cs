using UnityEngine;
using System.Collections;
using Meg.Networking;


public class DCCMissionSwitch : MonoBehaviour
{
    public GameObject subsScreens;
    public GameObject gliderScreens;

    private int playerVessel = 1;


	// Update is called once per frame
	void Update ()
    {
	    if (serverUtils.GetPlayerVessel() < 3)
        {
            subsScreens.SetActive(true);
            gliderScreens.SetActive(false);
        }
        else
        {
            subsScreens.SetActive(false);
            gliderScreens.SetActive(true);
        }
	}
}
