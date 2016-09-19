using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Meg.Networking;
using Meg.DCC;

public class DCCScreenManager : Singleton<DCCScreenManager>
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

    [Header("Surface Pro Screen")]
    public GameObject surfaceScreen;

    [Header("Strategy Table Screen")]
    public GameObject strategyScreen;

    [Header("Registered non quad windows")]
    public DCCWindow[] nonQuadWindows;

    [Header("Control screen buttons")]
    public buttonControl resetButton;
    public buttonControl cycleButton;

    [Header("Swipe indicator")]
    public GameObject swipeIndicator;

    private int currentTestPattern = 1;
    private float initTimer = 0;
    private float initTime = 2;
    private bool initialised = false;
    private float updateTimer = 0;
    private float updateTick = 0.3f;
    private bool canPress = true;


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

        serverUtils.PostQuadContent(DCCScreenContentPositions.positionID.topLeft, (DCCWindow.contentID) newTestPattern[0], DCCScreenData.StationId);
        serverUtils.PostQuadContent(DCCScreenContentPositions.positionID.topRight, (DCCWindow.contentID) newTestPattern[1], DCCScreenData.StationId);
        serverUtils.PostQuadContent(DCCScreenContentPositions.positionID.bottomLeft, (DCCWindow.contentID) newTestPattern[2], DCCScreenData.StationId);
        serverUtils.PostQuadContent(DCCScreenContentPositions.positionID.bottomRight, (DCCWindow.contentID) newTestPattern[3], DCCScreenData.StationId);
        serverUtils.PostQuadContent(DCCScreenContentPositions.positionID.middle, (DCCWindow.contentID) newTestPattern[4], DCCScreenData.StationId);
    }

    /** Gets the desired content of each box from the server. */
    void GetBoxContentID()
    {
        if (initialised)
        {
            foreach (DCCQuadBox quad in quadBoxes)
            {
                quad.boxContent = serverUtils.GetQuadContent(quad.boxPosition, DCCScreenData.StationId);
                SetBoxContent(quad);
            }
        }
    }

    /** Sets a window to be hidden and sets its correct position. */
    void SetWindowHiddenPosition(DCCWindow hiddenWindow)
    {
        hiddenWindow.quadPosition = DCCScreenContentPositions.positionID.hidden;
        hiddenWindow.SetWindowPosition(DCCScreenContentPositions.positionID.hidden);
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
        if (serverUtils.GetQuadFullScreen(DCCScreenData.StationId) == 1)
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
        var x = System.Array.IndexOf(nonQuadWindows, oldWindow);
        if (x >= 0)
            nonQuadWindows = RemoveAt(nonQuadWindows, x);
    }

    /** Whether a given piece of window content is currently visible on screen. */
    public bool IsContentVisible(DCCWindow.contentID content)
    {
        // Check if content is visible on regular a DCC window.
        for (var i = 0; i < nonQuadWindows.Length; i++)
            if (nonQuadWindows[i].windowContent == content)
                return true;
        
        // Check if content is visible in a quad window.
        for (var i = 0; i < quadWindows.Length; i++)
            if (quadWindows[i].windowContent == content)
                if (quadWindows[i].quadPosition != DCCScreenContentPositions.positionID.hidden)
                    return true;

        // Nav content is always required on the strategy table.
        if (content == DCCWindow.contentID.nav)
        {
            var player = serverUtils.LocalPlayer;
            if (player && player.ScreenState.Type == screenData.Type.DccStrategy)
                return true;
        }

        return false;
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

    /** Activate window with specified content on the specified screen. */
    public void ActivateWindow(DCCWindow.contentID content, DCCScreenID._screenID id)
        { serverUtils.PostScreenContent(id, content, DCCScreenData.StationId); }

    /** Resets the content of the top screens. */
    public void ResetTopWindows()
    {
        serverUtils.PostScreenContent(DCCScreenID._screenID.screen3, 0, DCCScreenData.StationId);
        serverUtils.PostScreenContent(DCCScreenID._screenID.screen4, 0, DCCScreenData.StationId);
        serverUtils.PostScreenContent(DCCScreenID._screenID.screen5, 0, DCCScreenData.StationId);
    }

    public void CycleWindows()
    {
        Debug.Log("Cycling quad windows.");
        serverUtils.PostQuadCycle(1, DCCScreenData.StationId);
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

    /** Set the screen to its position on the DCC set. 1 left screen, 2 middle screen, 3 overhead left, 4 overhead middle, 5 overhead right.*/
    void SetScreen(screenData.Type type)
    {
        if (!screenData.IsDccScreen(type))
            return;

        controlScreen.SetActive(type == screenData.Type.DccControl);
        quadScreen.SetActive(type == screenData.Type.DccQuad);
        surfaceScreen.SetActive(type == screenData.Type.DccSurface);
        strategyScreen.SetActive(type == screenData.Type.DccStrategy);
        screen3.SetActive(type >= screenData.Type.DccScreen3 && type <= screenData.Type.DccScreen5);
        screen3.GetComponent<DCCScreenID>().screenID = screen3.activeSelf
            ? DCCScreenID.ScreenIdForType(type)
            : DCCScreenID._screenID.screen3;

        PostScreenType(type);
    }

    IEnumerator wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canPress = true;
    }

    void Awake ()
    {
        initTime += Time.deltaTime;
    }

    void Start ()
    {
        //Debug.Log("Device = " + SystemInfo.deviceName);
        SetScreen(screenData.Type.DccControl);
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

        if (cycleButton.pressed && canPress)
        {
            CycleWindows();
            canPress = false;
            StartCoroutine(wait(0.6f));
        }

        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            TestPattern();
        }

        // Update DCC screen to match shared state.
        UpdateScreenType();

        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha1))
            SetScreen(screenData.Type.DccControl);
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha2))
            SetScreen(screenData.Type.DccQuad);
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha3))
            SetScreen(screenData.Type.DccScreen3);
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha4))
            SetScreen(screenData.Type.DccScreen4);
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha5))
            SetScreen(screenData.Type.DccScreen5);
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha6))
            SetScreen(screenData.Type.DccSurface);
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha7))
            SetScreen(screenData.Type.DccStrategy);

        if (Time.time < updateTimer)
            return;

        updateTimer += updateTick;

        // Reset quad cycle if it has been set.
        if (serverUtils.GetQuadCycle(DCCScreenData.StationId) == 1)
        {
            TestPattern();
            serverUtils.PostQuadCycle(0, DCCScreenData.StationId);
        }
    }

    private void PostScreenType(screenData.Type type)
    {
        if (!serverUtils.IsReady())
            return;

        var player = serverUtils.LocalPlayer;
        if (player && !Equals(player.ScreenState.Type, type))
            serverUtils.PostScreenStateType(player.netId, type);
    }

    private void UpdateScreenType()
    {
        if (!serverUtils.IsReady())
            return;

        var player = serverUtils.LocalPlayer;
        if (player)
            SetScreen(player.ScreenState.Type);
    }

}
