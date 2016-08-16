using UnityEngine;
using System.Collections;
using Meg.Networking;
using Meg.DCC;

public class DCCScreenManager : MonoBehaviour
{
    [Header("Screen 1")]
    public GameObject controlScreen;

    [Header("Screen 2")]
    public GameObject quadScreen;
    public DCCQuadBox[] quadBoxes = new DCCQuadBox[5];
    public DCCWindow[] quadWindows;
    public int[] testPattern1 = { 4, 1, 2, 3, 5 };
    public int[] testPattern2 = { 3, 4, 1, 2, 5 };
    public int[] testPattern3 = { 2, 3, 4, 1, 5 };
    public int[] testPattern4 = { 1, 2, 3, 4, 5 };

    [Header("Screen 3")]
    public GameObject screen3;

    [Header("Registered non quad windows")]
    public DCCWindow[] nonQuadWindows;

    [Header("Control screen buttons")]
    public buttonControl resetButton;
    public buttonControl clearButton;

    [Header("Swipe indicator")]
    public GameObject swipeIndicator;

    private int currentTestPattern = 1;
    private float initTimer = 0;
    private float initTime = 2;
    private bool initialised = false;

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
        if (initialised)
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
                if (!visible || quadWindows[i].quadPosition == DCCScreenContentPositions.positionID.hidden) //
                    SetWindowHiddenPosition(quadWindows[i]);
            }
        }
    }

    /** Sets the hidden status of middle windows when switching between fullscreen modes. */
    void SetMiddleScreenState(DCCQuadBox box, DCCWindow quadWindow)
    {
        if ((int)serverUtils.GetServerData("DCCfullscreen") == 1)
        {
            //Debug.Log("Showing this window: " + quadWindow + " in quad box " + box);
            if (quadWindow.quadPosition != DCCScreenContentPositions.positionID.middle)
            {
                //lerp this window to its new position
                quadWindow.gameObject.SetActive(true);
                quadWindow.MoveWindow(box.boxPosition);
            }
        }
        else
        {
            //Debug.Log("Hiding this window: " + quadWindow + " in quad box " + box);
            SetWindowHiddenPosition(quadWindow);
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
                //if middle screen check that we are fullscreen, otherwise we don't draw
                if (box.boxPosition == DCCScreenContentPositions.positionID.middle)
                {
                    SetMiddleScreenState(box, quadWindows[i]);
                }
                else
                {
                    if (quadWindows[i].quadPosition != box.boxPosition)
                    {
                        //lerp this window to its new position
                        quadWindows[i].gameObject.SetActive(true);
                        quadWindows[i].MoveWindow(box.boxPosition);
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

    /** Non quad windows register with this so we can sort them. */
    public void RegisterWindow(DCCWindow newWindow)
    {
        if (nonQuadWindows.Length == 0)
        {
            nonQuadWindows = new DCCWindow[1];
            nonQuadWindows[0] = newWindow;
        }
        else
        {
            DCCWindow[] newWindowArray = new DCCWindow[nonQuadWindows.Length + 1];
            int i = newWindowArray.Length;

            System.Array.Copy(nonQuadWindows, newWindowArray, i - 1);
            nonQuadWindows = new DCCWindow[i];
            System.Array.Copy(newWindowArray, nonQuadWindows, i - 1);
            nonQuadWindows[i - 1] = newWindow;
        }
    }

    /** Removes a window from the nonQuadWindows array. Called by a DCCWindow onDisable and ensures we don't have invisible windows in the sort order. */
    public void UnregisterWindow(DCCWindow oldWindow)
    {
        int x = System.Array.IndexOf(nonQuadWindows, oldWindow);

        nonQuadWindows = RemoveAt(nonQuadWindows, x);
    }

    /** Removes a window from a specified array at index. */
    DCCWindow[] RemoveAt(DCCWindow[] source, int index)
    {
        //Debug.Log("Removing window  " + source[index] + "at index: " + index);

        DCCWindow[] dest = new DCCWindow[source.Length - 1];
        if (index > 0)
            System.Array.Copy(source, 0, dest, 0, index);

        if (index < source.Length - 1)
            System.Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

        return dest;
    }

    /** Pushes a window to the front of the sorting order. */
    public void PushWindowToFront(DCCWindow focusedWindow)
    {
        //find the index of this window
        int x = System.Array.IndexOf(nonQuadWindows, focusedWindow);

        //Debug.Log("Window at index: " + x + " to be removed from shunted forward.");

        //get an array with this removed, but in its original order
        DCCWindow[] tempWindows = RemoveAt(nonQuadWindows, x);

        //copy the temp array back over at 0
        System.Array.Copy(tempWindows, 0, nonQuadWindows, 0, nonQuadWindows.Length - 1);

        //add the focused window to the end
        nonQuadWindows[nonQuadWindows.Length - 1] = focusedWindow;
    }

    /** Sets any window with specified content enabled/disabled on the specified screen. */
    public void SetWindowActive(DCCWindow.contentID content, DCCScreenID._screenID id, bool state)
    {
        string screen = "dccscreen3content";

        if (id == DCCScreenID._screenID.screen4)
            screen = "dccscreen4content";
        if (id == DCCScreenID._screenID.screen5)
            screen = "dccscreen5content";

        //get current content int and bit or against the content
        int currentContent = (int)serverUtils.GetServerData(screen);
        if (state)
            currentContent = currentContent | (1 << 1 * ((int)content));
        else
            currentContent = currentContent ^ (1 << 1 * ((int)content));

        serverUtils.PostServerData(screen, currentContent);

        Debug.Log(GetIntBinaryString(currentContent));
    }

    /** Debug function to check binary values. */
    static string GetIntBinaryString(int n)
    {
        char[] b = new char[32];
        int pos = 31;
        int i = 0;

        while (i < 32)
        {
            if ((n & (1 << i)) != 0)
            {
                b[pos] = '1';
            }
            else
            {
                b[pos] = '0';
            }
            pos--;
            i++;
        }
        return new string(b);
    }

    /** Resets the content of the top screens. */
    public void ResetTopWindows()
    {
        serverUtils.PostServerData("dccscreen3content", 0);
        serverUtils.PostServerData("dccscreen4content", 0);
        serverUtils.PostServerData("dccscreen5content", 0);
    }

    /** Sorts the windows on the active screen. Each window has 2 units of depth, so each window should be carefull authored within this range. */
    void SetWindowsSortDepth()
    {
        float zDepth = 20;

        for (int i = 0; i < nonQuadWindows.Length; i++)
        {
            nonQuadWindows[i].transform.localPosition = new Vector3(nonQuadWindows[i].transform.localPosition.x, nonQuadWindows[i].transform.localPosition.y, zDepth);
            zDepth -= 2;
        }
    }

    void Awake ()
    {
        initTime += Time.deltaTime;
    }

    void Update ()
    {
        if (!initialised)
            initTimer += Time.deltaTime;

        if (initTimer > initTime)
            initialised = true;

        GetBoxContentID();
        CheckForOrphanedWindows();
        SetWindowsSortDepth();

        if (resetButton.pressed)
            ResetTopWindows();

        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            TestPattern();
        }

        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha1))
        {
            controlScreen.SetActive(true);
            quadScreen.SetActive(false);
            screen3.SetActive(false);
            screen3.GetComponent<DCCScreenID>().screenID = DCCScreenID._screenID.screen3;
        }

        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha2))
        {
            controlScreen.SetActive(false);
            quadScreen.SetActive(true);
            screen3.SetActive(false);
            screen3.GetComponent<DCCScreenID>().screenID = DCCScreenID._screenID.screen3;
        }

        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha3))
        {
            controlScreen.SetActive(false);
            quadScreen.SetActive(false);
            screen3.SetActive(true);
            screen3.GetComponent<DCCScreenID>().screenID = DCCScreenID._screenID.screen3;
        }


        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha4))
        {
            controlScreen.SetActive(false);
            quadScreen.SetActive(false);
            screen3.SetActive(true);
            screen3.GetComponent<DCCScreenID>().screenID = DCCScreenID._screenID.screen4;
        }


        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha5))
        {
            controlScreen.SetActive(false);
            quadScreen.SetActive(false);
            screen3.SetActive(true);
            screen3.GetComponent<DCCScreenID>().screenID = DCCScreenID._screenID.screen5;
        }
    }
}
