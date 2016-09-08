using System.Linq;
using Meg.Networking;
using UnityEngine;
using UnityEngine.Networking;

/** Sets the screen matrix for the glider screens. The right hand screen determines the content of the other 2 screens. */
public class glScreenData : NetworkBehaviour
{
    /**
    left screen         middle screen           right screen
    -----------         -------------           ------------
    power (12)          thrusters (8)           controls (0)
    power (12)          map (9)                 nav map (1)
    power (12)          tcas (10)               nav tcas (2)
    power (12)          sonar (14)              nav sonar (3)
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
                else if (screenMatrixID == 1 || screenMatrixID == 4)
                    screenOutID = 9; //map
                else if (screenMatrixID == 3)
                    screenOutID = 14; //sonar
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
        // Prefer right screens that are on client instances.  
        // This will help the glider screens behave sensibly even if the host 
        // is also configured to display a right screen.
        var right = serverUtils.GetPlayers()
            .Where(p => p.GameInputs.IsRightGliderScreen)
            .OrderBy(p => p.isLocalPlayer)
            .ThenBy(p => p.netId.Value)
            .FirstOrDefault();

        if (!right)
            return;

        var inputs = right.GameInputs;
        mapElevation = inputs.map3dState;
        recentreMap = inputs.mapCentreState;
        objectLabelsOn = inputs.mapLabelState;

        if (inputs.activeScreen <= 7)
            screenMatrixID = inputs.activeScreen;
    }

    void Update()
    {
        if (!isServer)
            return;

        setMatrixID();
    }
}
