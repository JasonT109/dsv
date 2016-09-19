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
        surface,
        strategy
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

        UpdateScreenType();
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

    private void UpdateScreenType()
    {
        if (!serverUtils.IsReady())
            return;

        var type = TypeForScreenId(screenID);
        var player = serverUtils.LocalPlayer;
        if (player && !Equals(player.ScreenState.Type, type))
            serverUtils.PostScreenStateType(player.netId, type);
    }

    public static screenData.Type TypeForScreenId(_screenID id)
    {
        switch (id)
        {
            case _screenID.control:
                return screenData.Type.DccControl;
            case _screenID.qaud:
                return screenData.Type.DccQuad;
            case _screenID.screen3:
                return screenData.Type.DccScreen3;
            case _screenID.screen4:
                return screenData.Type.DccScreen4;
            case _screenID.screen5:
                return screenData.Type.DccScreen5;
            case _screenID.surface:
                return screenData.Type.DccSurface;
            case _screenID.strategy:
                return screenData.Type.DccStrategy;
            default:
                return screenData.Type.DccControl;
        }
    }

    public static _screenID ScreenIdForType(screenData.Type type)
    {
        switch (type)
        {
            case screenData.Type.DccControl:
                return _screenID.control;
            case screenData.Type.DccQuad:
                return _screenID.qaud;
            case screenData.Type.DccScreen3:
                return _screenID.screen3;
            case screenData.Type.DccScreen4:
                return _screenID.screen4;
            case screenData.Type.DccScreen5:
                return _screenID.screen5;
            case screenData.Type.DccSurface:
                return _screenID.surface;
            case screenData.Type.DccStrategy:
                return _screenID.strategy;
            default:
                return _screenID.control;
        }
    }

}
