using UnityEngine;
using System.Collections;
using Meg.Networking;
using Meg.DCC;

public class DCCScreenID : MonoBehaviour
{
    public enum _screenID
    {
        control,
        qaud,
        screen3,
        screen4,
        screen5
    }

    //our desired slots, content will go to these window positions
    public DCCScreenContentPositions.positionID[] slots;

    //these get set when a new window is made visible or invisible
    public DCCWindow[] slotContent = new DCCWindow[3];

    //our screen id
    public _screenID screenID = new _screenID(); 

    //bit mask int for what windows are visible
    public int visibleWindows;

    //list of our child windows, must be populated by hand
    public DCCWindow[] childWindows;

    //the screen manager
    private DCCScreenManager screenManager;

    private DCCWindow.contentID content;
    private int previousVisibleWindows;
    private int nextSlot = 0;

    private void SetWindowVis()
    {
        if (visibleWindows == 0)
        {
            ClearSlot(0);
            ClearSlot(1);
            ClearSlot(2);
        }

        //check each of our windows to see if we should be visible
        for (int i = 0; i < childWindows.Length; i++)
        {
            //check whether visibleWindows has this windows content marked as visible
            int id = (int)childWindows[i].windowContent;
            int bitmask = 1 << (1 * id);
            bool vis = (visibleWindows & bitmask) > 0;
            childWindows[i].gameObject.SetActive(vis);

            //Debug.Log("id: " + id + " bit mask: " + bitmask + " vis: " + vis);

            if (vis)
            {
                //don't restart the lerp if we are already lerping or in position
                if (!childWindows[i].isLerping && !QuerySlots(childWindows[i]))
                {
                    //first move this to a position offscreen
                    childWindows[i].SetWindowPosition(DCCScreenContentPositions.positionID.hidden);
                
                    //start this lerping to our next slot position
                    childWindows[i].MoveWindow(slots[nextSlot]);

                    //if destination slot is not empty, hide that content
                    if (slotContent[nextSlot] != null)
                    {
                        slotContent[nextSlot].SetWindowPosition(DCCScreenContentPositions.positionID.hidden);
                        slotContent[nextSlot].gameObject.SetActive(false);
                        screenManager.SetWindowActive(childWindows[i].windowContent, screenID, false);
                    }

                    //set the slot content
                    slotContent[nextSlot] = childWindows[i];

                    //set the windows position field so we don't accidentely lerp this again
                    childWindows[i].quadPosition = slots[nextSlot];

                    nextSlot++;
                    if (nextSlot > 2)
                        nextSlot = 0;
                }
            }
            else
            {
                childWindows[i].SetWindowPosition(DCCScreenContentPositions.positionID.hidden);
                childWindows[i].quadPosition = DCCScreenContentPositions.positionID.hidden;
            }

            previousVisibleWindows = visibleWindows;
        }
    }

    public void ClearSlot(int slotNumber)
    {
        if (slotContent[slotNumber])
            slotContent[slotNumber] = null;
    }

    public bool QuerySlots(DCCWindow queryWindow)
    {
        bool exists = false;
        for (int i = 0; i < slotContent.Length; i++)
        {
            if (slotContent[i] == queryWindow)
                exists = true;
        }
        return exists;
    }

    void Awake()
    {
        if (!screenManager)
            screenManager = ObjectFinder.Find<DCCScreenManager>();
    }

    void Update ()
    {
        if (screenID == _screenID.screen3)
        {
            visibleWindows = (int)serverUtils.GetServerData("dccscreen3content");
            if (previousVisibleWindows != visibleWindows)
                SetWindowVis();
        }

        if (screenID == _screenID.screen4)
        {
            visibleWindows = (int)serverUtils.GetServerData("dccscreen4content");
            if (previousVisibleWindows != visibleWindows)
                SetWindowVis();
        }

        if (screenID == _screenID.screen5)
        {
            visibleWindows = (int)serverUtils.GetServerData("dccscreen5content");
            if (previousVisibleWindows != visibleWindows)
                SetWindowVis();
        }
    }
}
