using UnityEngine;
using System.Collections;
using Meg.Networking;

public class DCCScreenManager : MonoBehaviour
{
    public DCCQuadBox[] quadBoxes = new DCCQuadBox[5];

    public DCCWindow[] quadWindows;

    public int[] testPattern1 = { 0, 1, 2, 3, 0 };
    public int[] testPattern2 = { 3, 0, 1, 2, 0 };
    public int[] testPattern3 = { 2, 3, 0, 1, 0 };
    public int[] testPattern4 = { 1, 2, 3, 0, 0 };
    private int currentTestPattern = 1;

    void TestPattern()
    {
        int[] TestPattern = new int[5];

        switch (currentTestPattern)
        {
            case 1:
                TestPattern = testPattern1;
                break;
            case 2:
                TestPattern = testPattern2;
                break;
            case 3:
                TestPattern = testPattern3;
                break;
            case 4:
                TestPattern = testPattern4;
                break;
        }

        currentTestPattern++;
        if (currentTestPattern > 4)
            currentTestPattern = 1;

        serverUtils.PostServerData("DCCquadScreen0", (float)TestPattern[0]);
        serverUtils.PostServerData("DCCquadScreen1", (float)TestPattern[1]);
        serverUtils.PostServerData("DCCquadScreen2", (float)TestPattern[2]);
        serverUtils.PostServerData("DCCquadScreen3", (float)TestPattern[3]);
        serverUtils.PostServerData("DCCfullscreen", (float)TestPattern[4]);

    }

    void GetBoxContentID()
    {
        for (int i = 0; i < quadBoxes.Length; i++)
        {
            //box content is driven by server sync vars DCCquadScreen0 etc.
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
	
    void SetBoxContent(DCCQuadBox box)
    {
        for (int i = 0; i < quadWindows.Length; i++)
        {
            //check the box position and see if we need to remove the content
            if (quadWindows[i].quadWindow)
            {
                bool visible = false;

                for (int q = 0; q < quadBoxes.Length; q++)
                {
                    if (quadWindows[i].windowContent == quadBoxes[q].boxContent)
                        visible = true;
                }

                quadWindows[i].gameObject.SetActive(visible);
            }

            //if our window contains the content that should be in this box AND it is NOT currently in that position
            if (quadWindows[i].windowContent == box.boxContent && quadWindows[i].quadPosition != box.boxPosition)
            {
                //lerp from a visible position
                if (quadWindows[i].quadPosition != Meg.DCC.DCCScreenContentPositions.positionID.hidden)
                {
                    //if middle screen check that we are fullscreen, otherwise we don't draw
                    if (box.boxPosition == Meg.DCC.DCCScreenContentPositions.positionID.middle)
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
                //if this box contains the right content but is currently hidden
                else
                {
                    //if middle screen check that we are fullscreen, otherwise we don't draw
                    if (box.boxPosition == Meg.DCC.DCCScreenContentPositions.positionID.middle)
                    {
                        if (serverUtils.GetServerData("DCCfullscreen") == 1)
                        {
                            quadWindows[i].gameObject.SetActive(true);
                            quadWindows[i].SetWindowPosition(box.boxPosition);
                        }
                        else
                        {
                            quadWindows[i].gameObject.SetActive(false);
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
    }

    // Update is called once per frame
    void Update ()
    {
        GetBoxContentID();

        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            TestPattern();
        }
    }
}
