using UnityEngine;
using System.Collections;
using Meg.Networking;

public class glScreenManager : Singleton<glScreenManager>
{

    public const int LeftScreenId = 2;
    public const int MidScreenId = 1;
    public const int RightScreenId = 0;

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

        // Update the shared screen state to match.
        var player = serverUtils.LocalPlayer;
        var inputs = player ? player.GameInputs : null;
        if (inputs && inputs.activeScreen != screenToShow)
        {
            inputs.activeScreen = screenToShow;
            player.PostGliderScreenContentId(player.netId, screenToShow);
        }
    }

    /** Return a readable name for the given glider screen id. */
    public static string GetScreenName(int screenId)
    {
        switch (screenId)
        {
            case 0:
                return "Controls";
            case 1:
                return "NavMap";
            case 2:
            case 10:
                return "TCAS";
            case 3:
            case 14:
                return "Sonar";
            case 4:
            case 15:
                return "Radar";
            case 5:
            case 11:
                return "Towing";
            case 6:
                return "Comms";
            case 7:
                return "Systems";
            case 8:
                return "Thrusters";
            case 9:
                return "Map";
            case 12:
                return "Power";
            case 13:
                return "Diagnostics";
            default:
                return "";
        }
    }

    /** Return an equivalent screen content value for the given glider id. */
    public static screenData.Content GetScreenContent(int screenId)
    {
        switch (screenId)
        {
            case 0:
                return screenData.Content.Controls;
            case 1:
                return screenData.Content.Nav;
            case 2:
            case 10:
                return screenData.Content.TCAS;
            case 3:
            case 14:
                return screenData.Content.SonarLong;
            case 4:
            case 15:
                return screenData.Content.Radar;
            case 5:
            case 11:
                return screenData.Content.Towing;
            case 6:
                return screenData.Content.Comms;
            case 7:
                return screenData.Content.Systems;
            case 8:
                return screenData.Content.Thrusters;
            case 9:
                return screenData.Content.Map;
            case 12:
                return screenData.Content.Power;
            case 13:
                return screenData.Content.Diagnostics;
            default:
                return screenData.Content.None;
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
