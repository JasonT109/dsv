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
        screen5,
        surface
    }

    //our screen id
    public _screenID screenID = new _screenID(); 

    //list of our child windows, must be populated by hand
    public DCCWindow[] childWindows;

    //the screen manager
    private DCCScreenManager screenManager;

    // The window that's currently visible in this screen.
    private DCCWindow.contentID visibleContent;


    void Awake()
    {
        if (!screenManager)
            screenManager = ObjectFinder.Find<DCCScreenManager>();
    }

    void Update()
    {
        var content = serverUtils.GetScreenContent(screenID, DCCScreenData.StationId);
        if (content != visibleContent)
            UpdateVisibleWindow(content);

        visibleContent = content;
    }

    private void UpdateVisibleWindow(DCCWindow.contentID content)
    {
        foreach (var window in childWindows)
        {
            if (!window)
                continue;

            var visible = window.windowContent == content;
            window.gameObject.SetActive(visible);

            if (visible && !window.isLerping)
            {
                window.SetWindowPosition(DCCScreenContentPositions.positionID.hidden);
                window.MoveWindow(DCCScreenContentPositions.positionID.center);
                window.quadPosition = DCCScreenContentPositions.positionID.center;
            }
            else if (!visible)
            {
                window.SetWindowPosition(DCCScreenContentPositions.positionID.hidden);
                window.quadPosition = DCCScreenContentPositions.positionID.hidden;
            }
        }
    }
}
