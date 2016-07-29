using UnityEngine;
using UnityEngine.Networking;

/** Sets the screen matrix for the glider screens. The right hand screen determins the content of the other 2 screens. */
public class glScreenData : NetworkBehaviour
{
    /**
    left screen         middle screen           right screen
    -----------         -------------           ------------
    power (12)          thrusters (8)           controls (0)
    power (12)          map (9)                 nav map (1)
    power (12)          tcas (10)               nav tcas (2)
    power (12)          map (9)                 nav sonar (3)
    power (12)          map (9)                 nav radar (4)
    power (12)          towing (11)             towing (5)
    power (12)          thrusters (8)           comms (6)
    diagnostics (13)    thrusters (8)           systems (7)
    */

    [SyncVar]
    public int screenMatrixID;

    [SyncVar]
    public bool mapElevation;

    [SyncVar]
    public bool recentreMap;

    [SyncVar]
    public bool objectLabelsOn;

    /**Get which screen we should be on by passing in our screen ID (0 = right, 1 = middle, 2 = left). */
    public int getScreen(int screenInID)
    {
        int screenOutID = 0;
        switch (screenInID)
        {
            case 0:
                screenOutID = screenInID;
                break;
            case 1:
                if (screenMatrixID == 0 || screenMatrixID == 6 || screenMatrixID == 7) 
                    screenOutID = 8; //thrusters
                else if (screenMatrixID == 1 || screenMatrixID == 3 || screenMatrixID == 4)
                    screenOutID = 9; //map
                else if (screenMatrixID == 2)
                    screenOutID = 10; //tcas
                else if (screenMatrixID == 5)
                    screenOutID = 11; //towing
                break;
            case 2:
                if (screenMatrixID == 7)
                    screenOutID = 13; //diagnostics
                else
                    screenOutID = 12; //power
                break;
        }
        return screenOutID;
    }

    /** Set the matrix ID based on which client is displaying the right screen */
    void setMatrixID()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            //if this client is screen 0 (right screen)
            if (players[i].GetComponent<gameInputs>().glScreenID == 0)
            {
                screenMatrixID = players[i].GetComponent<gameInputs>().activeScreen;
                mapElevation = players[i].GetComponent<gameInputs>().map3dState;
                recentreMap = players[i].GetComponent<gameInputs>().mapCentreState;
                objectLabelsOn = players[i].GetComponent<gameInputs>().mapLabelState;
            }
        }
    }

    void Update()
    {
        if (!isServer)
            return;

        setMatrixID();
    }
}
