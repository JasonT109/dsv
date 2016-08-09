using UnityEngine;
using System.Collections;
using Meg.Networking;
using Meg.DCC;

public class DCCScreenManager : MonoBehaviour
{
    public DCCQuadBox[] quadBoxes = new DCCQuadBox[5];

    public DCCWindow[] quadWindows;

    public int[] testPattern1 = { 4, 1, 2, 3, 5 };
    public int[] testPattern2 = { 3, 4, 1, 2, 5 };
    public int[] testPattern3 = { 2, 3, 4, 1, 5 };
    public int[] testPattern4 = { 1, 2, 3, 4, 5 };
    private int currentTestPattern = 1;

    void TestPattern()
    {
        int[] newTestPattern = new int[5];

        switch (currentTestPattern)
        {
            case 1:
                newTestPattern = testPattern1;
                break;
            case 2:
                newTestPattern = testPattern2;
                break;
            case 3:
                newTestPattern = testPattern3;
                break;
            case 4:
                newTestPattern = testPattern4;
                break;
        }

        currentTestPattern++;
        if (currentTestPattern > 4)
            currentTestPattern = 1;

        serverUtils.PostServerData("DCCquadScreen0", (float)newTestPattern[0]);
        serverUtils.PostServerData("DCCquadScreen1", (float)newTestPattern[1]);
        serverUtils.PostServerData("DCCquadScreen2", (float)newTestPattern[2]);
        serverUtils.PostServerData("DCCquadScreen3", (float)newTestPattern[3]);
        serverUtils.PostServerData("DCCquadScreen4", (float)newTestPattern[4]);
    }

    /** Gets the desired content of each box from the server. */
    void GetBoxContentID()
    {
        for (int i = 0; i < quadBoxes.Length; i++)
        {
            switch (quadBoxes[i].boxPosition)
            {
                case Meg.DCC.DCCScreenContentPositions.positionID.topLeft :
                    quadBoxes[i].boxContent = (DCCWindow.contentID)(int)serverUtils.GetServerData("DCCquadScreen0");
                    break;
                case Meg.DCC.DCCScreenContentPositions.positionID.topRight:
                    quadBoxes[i].boxContent = (DCCWindow.contentID)(int)serverUtils.GetServerData("DCCquadScreen1");
                    break;
                case Meg.DCC.DCCScreenContentPositions.positionID.bottomLeft:
                    quadBoxes[i].boxContent = (DCCWindow.contentID)(int)serverUtils.GetServerData("DCCquadScreen2");
                    break;
                case Meg.DCC.DCCScreenContentPositions.positionID.bottomRight:
                    quadBoxes[i].boxContent = (DCCWindow.contentID)(int)serverUtils.GetServerData("DCCquadScreen3");
                    break;
                case Meg.DCC.DCCScreenContentPositions.positionID.middle:
                    quadBoxes[i].boxContent = (DCCWindow.contentID)(int)serverUtils.GetServerData("DCCquadScreen4");
                    break;
            }

            SetBoxContent(quadBoxes[i]);
        }
    }

    /** Sets a window to be hidden and sets its correct position. */
    void SetWindowHiddenPosition(DCCWindow hiddenWindow)
    {
        hiddenWindow.quadPosition = Meg.DCC.DCCScreenContentPositions.positionID.hidden;
        hiddenWindow.SetWindowPosition(Meg.DCC.DCCScreenContentPositions.positionID.hidden);
        hiddenWindow.gameObject.SetActive(false);
    }

    /** Looks for orphaned windows. */
    void CheckForOrphanedWindows()
    {
        for (int i = 0; i < quadWindows.Length; i++)
        {
            //ensure the window is set to be a quad window
            if (quadWindows[i].quadWindow)
            {
                bool visible = false;

                //go through each quad box
                for (int q = 0; q < quadBoxes.Length; q++)
                {
                    //if this window content is in a quad box we don't need to hide it
                    if (quadWindows[i].windowContent == quadBoxes[q].boxContent)
                        visible = true;
                }

                //if this window isn't required for any boxes hide it and set the correct position
                if (!visible)
                    SetWindowHiddenPosition(quadWindows[i]);
            }
        }
    }

    /** Sets the contents of a specific quad box. */
    void SetBoxContent(DCCQuadBox box)
    {
        for (int i = 0; i < quadWindows.Length; i++)
        {
            //if our window contains the content that should be in this box AND it is NOT currently in that position
            if (quadWindows[i].windowContent == box.boxContent)
            {
                if (quadWindows[i].quadPosition != box.boxPosition)
                {
                    //if middle screen check that we are fullscreen, otherwise we don't draw
                    if (box.boxPosition == DCCScreenContentPositions.positionID.middle)
                    {
                        if (serverUtils.GetServerData("DCCfullscreen") == 1)
                        {
                            //lerp this window to its new position
                            quadWindows[i].gameObject.SetActive(true);
                            quadWindows[i].MoveWindow(box.boxPosition);
                        }
                        else
                        {
                            quadWindows[i].gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        //lerp this window to its new position
                        quadWindows[i].gameObject.SetActive(true);
                        quadWindows[i].MoveWindow(box.boxPosition);
                    }
                }
                else
                {
                    quadWindows[i].gameObject.SetActive(true);
                    quadWindows[i].SetWindowPosition(box.boxPosition);
                }
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
        GetBoxContentID();
        CheckForOrphanedWindows();

        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            TestPattern();
        }
    }
}
