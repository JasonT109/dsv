using UnityEngine;
using System.Collections;
using Meg.Networking;

public class glScreenManager : Singleton<glScreenManager> {

    /**
    left screen         middle screen           right screen
    -----------         -------------           ------------
    power (12)          thrusters (8)           controls (0)
    power (12)          map (9)                 nav map (1)
    power (12)          tcas (10)               nav tcas (2)
    power (12)          sonar (14)              nav sonar (3)
    power (12)          radar (15)              nav radar (4)
    power (12)          towing (11)             towing (5)
    power (12)          thrusters (8)           comms (6)
    power (12)          thrusters (8)           systems (7)
    */

    public int screenID;
    public int activeScreen;
    public int rightScreenID = 0;

    public GameObject rightScreens;
    public GameObject middleScreens;
    public GameObject leftScreens;

    public GameObject thrusterScreen;
    public GameObject mapScreen;
    public GameObject tcasScreen;
    public GameObject towingScreen;
    public GameObject powerScreen;
    public GameObject sonarScreen;
    public GameObject radarScreen;


    //these buttons are monitored to set map events
    public buttonControl map3dButton;
    public buttonControl mapCenterButton;
    public buttonControl mapLabelButton;

    public bool hasChanged = true;

    public void SetRightScreenID(int rScreenID)
    {
        rightScreenID = rScreenID;
        hasChanged = true;
    }

    /** Set the correct visibility state for this screen ID from the screen matrix */
    void SetActiveScreen(int screenNumber, int screenToShow)
    {
        switch (screenNumber)
        {
            case 0:
                rightScreens.SetActive(true);
                middleScreens.SetActive(false);
                leftScreens.SetActive(false);
                break;
            case 1:
                rightScreens.SetActive(false);
                middleScreens.SetActive(true);
                leftScreens.SetActive(false);
                break;
            case 2:
                rightScreens.SetActive(false);
                middleScreens.SetActive(false);
                leftScreens.SetActive(true);
                break;
        }

        switch (screenToShow)
        {
            case 8: //thrusters
                thrusterScreen.SetActive(true);     //<---
                mapScreen.SetActive(false);
                tcasScreen.SetActive(false);
                towingScreen.SetActive(false);
                powerScreen.SetActive(false);
                sonarScreen.SetActive(false);
                radarScreen.SetActive(false);
                break;
            case 9: //map
                thrusterScreen.SetActive(false);
                mapScreen.SetActive(true);          //<---
                tcasScreen.SetActive(false);
                towingScreen.SetActive(false);
                powerScreen.SetActive(false);
                sonarScreen.SetActive(false);
                radarScreen.SetActive(false);
                break;
            case 10: //tcas
                thrusterScreen.SetActive(false);
                mapScreen.SetActive(false);
                tcasScreen.SetActive(true);         //<---
                towingScreen.SetActive(false);
                powerScreen.SetActive(false);
                sonarScreen.SetActive(false);
                radarScreen.SetActive(false);
                break;
            case 11: //towing
                thrusterScreen.SetActive(false);
                mapScreen.SetActive(false);
                tcasScreen.SetActive(false);
                towingScreen.SetActive(true);       //<---
                powerScreen.SetActive(false);
                sonarScreen.SetActive(false);
                radarScreen.SetActive(false);
                break;
            case 12: //power
                thrusterScreen.SetActive(false);
                mapScreen.SetActive(false);
                tcasScreen.SetActive(false);
                towingScreen.SetActive(false);
                powerScreen.SetActive(true);        //<---
                sonarScreen.SetActive(false);
                radarScreen.SetActive(false);
                break;
            case 13: //diagnostics
                thrusterScreen.SetActive(false);
                mapScreen.SetActive(false);
                tcasScreen.SetActive(false);
                towingScreen.SetActive(false);
                powerScreen.SetActive(true);        //<---
                sonarScreen.SetActive(false);
                radarScreen.SetActive(false);
                break;
            case 14: //sonar
                thrusterScreen.SetActive(false);
                mapScreen.SetActive(false);
                tcasScreen.SetActive(false);
                towingScreen.SetActive(false);
                powerScreen.SetActive(false);
                sonarScreen.SetActive(true);       //<---
                radarScreen.SetActive(false);
                break;
            case 15: //radar
                thrusterScreen.SetActive(false);
                mapScreen.SetActive(false);
                tcasScreen.SetActive(false);
                towingScreen.SetActive(false);
                powerScreen.SetActive(false);
                sonarScreen.SetActive(false);       
                radarScreen.SetActive(true);        //<---
                break;
        }
    }

    /** Start */
    void Start ()
    {
        activeScreen = serverUtils.getGliderScreen(screenID);
    }
	
	/** Update */
	void Update ()
    {

        if (Input.GetButton("Left Alt") && Input.GetButtonDown("ScreenLeft"))
        {
            screenID = 2;
            hasChanged = true;
        }

        if (Input.GetButton("Left Alt") && Input.GetButtonDown("ScreenMiddle"))
        {
            screenID = 1;
            hasChanged = true;
        }

        if (Input.GetButton("Left Alt") && Input.GetButtonDown("ScreenRight"))
        {
            screenID = 0;
            hasChanged = true;
        }

        activeScreen = serverUtils.getGliderScreen(screenID);
        SetActiveScreen(screenID, activeScreen);
	}
}
