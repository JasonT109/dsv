using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meg.DCC;
using Meg.Networking;
using UnityEngine.UI;

public class debugDCCStationUi : MonoBehaviour
{

    public int StationId;

    public Text Title;
    public Button TopLeft;
    public Button TopMid;
    public Button TopRight;
    public Button QuadTopLeft;
    public Button QuadTopRight;
    public Button QuadBottomLeft;
    public Button QuadBottomRight;
    public Button QuadMiddle;
    public Button Control;
    public Button Surface;

    public Graphic TopLeftOn;
    public Graphic TopMidOn;
    public Graphic TopRightOn;
    public Graphic QuadTopLeftOn;
    public Graphic QuadTopRightOn;
    public Graphic QuadBottomLeftOn;
    public Graphic QuadBottomRightOn;
    public Graphic QuadMiddleOn;

    public const float UpdateInterval = 0.1f;

    private float _nextUpdateTime;

    private readonly screenData.Content[] _controlWindowContents =
    {
        screenData.Content.Diagnostics,
        screenData.Content.Batteries,
        screenData.Content.Vitals,
        screenData.Content.Oxygen,
        screenData.Content.Comms,
        screenData.Content.SonarLong,
        screenData.Content.Power,
        screenData.Content.Systems,
    };

    private readonly screenData.Content[] _surfaceWindowContents =
    {
        screenData.Content.Diagnostics,
        screenData.Content.Batteries,
        screenData.Content.Vitals,
        screenData.Content.Oxygen,
        screenData.Content.Comms,
        screenData.Content.SonarLong,
        screenData.Content.Power,
        screenData.Content.Systems,
    };

    private readonly DCCWindow.contentID[] _topScreenContentIDs =
    {
        DCCWindow.contentID.none,
        DCCWindow.contentID.comms,
        DCCWindow.contentID.sonar,
        DCCWindow.contentID.batteries,
        DCCWindow.contentID.oxygen,
        DCCWindow.contentID.thrusters,
        DCCWindow.contentID.power,
        DCCWindow.contentID.diagnostics,
        DCCWindow.contentID.lifesupport,
    };

    private readonly DCCWindow.contentID[] _quadScreenContentIDs =
    {
        DCCWindow.contentID.none,
        DCCWindow.contentID.gliderdiagnostics,
        DCCWindow.contentID.gliderpower,
        DCCWindow.contentID.gliderthrusters,
        DCCWindow.contentID.nav,
        DCCWindow.contentID.piloting,
        DCCWindow.contentID.lifesupport,
        DCCWindow.contentID.diagnostics,
        DCCWindow.contentID.instruments,
        DCCWindow.contentID.thrusters,
        DCCWindow.contentID.batteries,
        DCCWindow.contentID.oxygen,
        DCCWindow.contentID.power,
        DCCWindow.contentID.comms,
        DCCWindow.contentID.sonar,
        DCCWindow.contentID.crew1,
        DCCWindow.contentID.crew2,
        DCCWindow.contentID.crew3,
        DCCWindow.contentID.crew4,
        DCCWindow.contentID.crew5,
    };


    void Start()
    {
        TopLeft.onClick.AddListener(() => SelectTopScreenContent(DCCScreenID._screenID.screen3));
        TopMid.onClick.AddListener(() => SelectTopScreenContent(DCCScreenID._screenID.screen4));
        TopRight.onClick.AddListener(() => SelectTopScreenContent(DCCScreenID._screenID.screen5));
        QuadTopLeft.onClick.AddListener(() => SelectQuadContent(DCCScreenContentPositions.positionID.topLeft));
        QuadTopRight.onClick.AddListener(() => SelectQuadContent(DCCScreenContentPositions.positionID.topRight));
        QuadBottomLeft.onClick.AddListener(() => SelectQuadContent(DCCScreenContentPositions.positionID.bottomLeft));
        QuadBottomRight.onClick.AddListener(() => SelectQuadContent(DCCScreenContentPositions.positionID.bottomRight));
        QuadMiddle.onClick.AddListener(() => SelectQuadContent(DCCScreenContentPositions.positionID.middle));
        Control.onClick.AddListener(SelectControlWindows);
        Surface.onClick.AddListener(SelectSurfaceWindows);
    }

    void Update()
    {
        if (Time.time < _nextUpdateTime)
            return;

	    _nextUpdateTime = Time.time + UpdateInterval;

	    try
	    {
	        UpdateLayout();
	    }
        catch (Exception) { }
    }

    private void UpdateLayout()
    {
        var dcc = serverUtils.DCCScreenData;
        if (!dcc)
            return;

        var station = dcc.GetStation(StationId);

        Title.text = "ID " + StationId + ": "+ DCCScreenData.GetStationName(StationId);

        var topLeft = station.Screen3;
        var topMid = station.Screen4;
        var topRight = station.Screen5;

        var quadTopLeft = serverUtils.GetQuadContent(DCCScreenContentPositions.positionID.topLeft, StationId);
        var quadTopRight = serverUtils.GetQuadContent(DCCScreenContentPositions.positionID.topRight, StationId);
        var quadBottomLeft = serverUtils.GetQuadContent(DCCScreenContentPositions.positionID.bottomLeft, StationId);
        var quadBottomRight = serverUtils.GetQuadContent(DCCScreenContentPositions.positionID.bottomRight, StationId);
        var quadMiddle = serverUtils.GetQuadContent(DCCScreenContentPositions.positionID.middle, StationId);

        TopLeft.GetComponentInChildren<Text>().text = DCCWindow.NameForContent(topLeft).ToUpper();
        TopLeft.gameObject.SetActive(station.HasScreen(screenData.Type.DccScreen3));
        TopMid.GetComponentInChildren<Text>().text = DCCWindow.NameForContent(topMid).ToUpper();
        TopMid.gameObject.SetActive(station.HasScreen(screenData.Type.DccScreen4));
        TopRight.GetComponentInChildren<Text>().text = DCCWindow.NameForContent(topRight).ToUpper();
        TopRight.gameObject.SetActive(station.HasScreen(screenData.Type.DccScreen5));

        QuadTopLeft.GetComponentInChildren<Text>().text = DCCWindow.NameForContent(quadTopLeft).ToUpper();
        QuadTopRight.GetComponentInChildren<Text>().text = DCCWindow.NameForContent(quadTopRight).ToUpper();
        QuadBottomLeft.GetComponentInChildren<Text>().text = DCCWindow.NameForContent(quadBottomLeft).ToUpper();
        QuadBottomRight.GetComponentInChildren<Text>().text = DCCWindow.NameForContent(quadBottomRight).ToUpper();
        QuadMiddle.GetComponentInChildren<Text>().text = DCCWindow.NameForContent(quadMiddle).ToUpper();

        TopLeftOn.gameObject.SetActive(topLeft != DCCWindow.contentID.none);
        TopMidOn.gameObject.SetActive(topMid != DCCWindow.contentID.none);
        TopRightOn.gameObject.SetActive(topRight != DCCWindow.contentID.none);
        QuadTopLeftOn.gameObject.SetActive(quadTopLeft != DCCWindow.contentID.none);
        QuadTopRightOn.gameObject.SetActive(quadTopRight != DCCWindow.contentID.none);
        QuadBottomLeftOn.gameObject.SetActive(quadBottomLeft != DCCWindow.contentID.none);
        QuadBottomRightOn.gameObject.SetActive(quadBottomRight != DCCWindow.contentID.none);
        QuadMiddleOn.gameObject.SetActive(quadMiddle != DCCWindow.contentID.none);

        Control.interactable = GetPlayerWithScreen(screenData.Type.DccControl) != null;
        Surface.interactable = GetPlayerWithScreen(screenData.Type.DccSurface) != null;
    }

    public void SelectTopScreenContent(DCCScreenID._screenID id)
        { SelectScreenContent(id, _topScreenContentIDs); }

    public void SelectScreenContent(DCCScreenID._screenID id, IEnumerable<DCCWindow.contentID> options)
    {
        var current = serverUtils.GetScreenContent(id, StationId);
        var items = options
            .Select(o => o.ToString())
            .OrderBy(t => t)
            .Select(t => new DialogList.Item { Name = t.ToUpper(), Id = t });

        var message = string.Format("Please select content for {0} : {1}",
            DCCScreenData.GetStationName(StationId), 
            screenData.NameForType(DCCScreenID.TypeForScreenId(id)));

        DialogManager.Instance.ShowList("SELECT SCREEN CONTENT",
            message,
            items,
            current.ToString(),
            (chosen) =>
            {
                var content = DCCWindow.ContentForName(chosen.Id);
                serverUtils.PostScreenContent(id, content, StationId);
            });   
    }

    public void SelectQuadContent(DCCScreenContentPositions.positionID id)
    {
        var current = serverUtils.GetQuadContent(id, StationId);
        var items = _quadScreenContentIDs
            .Select(o => o.ToString())
            .OrderBy(t => t)
            .Select(t => new DialogList.Item { Name = t.ToUpper(), Id = t });

        var message = string.Format("Please select quad content for {0} : {1}",
            DCCScreenData.GetStationName(StationId), id);

        DialogManager.Instance.ShowList("SELECT QUAD CONTENT",
            message,
            items,
            current.ToString(),
            (chosen) =>
            {
                var content = DCCWindow.ContentForName(chosen.Id);
                SetQuadContent(id, content);
            });
    }

    private void SetQuadContent(DCCScreenContentPositions.positionID position, DCCWindow.contentID content)
    {
        // Check what's in the target position.
        var replaced = serverUtils.GetQuadContent(position, StationId);
        if (replaced == content)
            return;

        // Check to see if content is already on the quad screen.
        // If so, swap content between the old and new positions.
        var oldPosition = serverUtils.GetQuadPosition(content, StationId);
        if (oldPosition != DCCScreenContentPositions.positionID.hidden && content != DCCWindow.contentID.none)
            PostQuadContent(oldPosition, replaced);

        // Place content in the new location.
        PostQuadContent(position, content);
    }

    private void PostQuadContent(DCCScreenContentPositions.positionID position, DCCWindow.contentID content)
    {
        serverUtils.PostQuadContent(position, content, StationId);
        if (position == DCCScreenContentPositions.positionID.middle)
            serverUtils.PostQuadFullScreen(content != DCCWindow.contentID.none ? 1 : 0, StationId);
    }

    public void SelectControlWindows()
        { SelectWindowsForType(screenData.Type.DccControl, _controlWindowContents); }

    public void SelectSurfaceWindows()
        { SelectWindowsForType(screenData.Type.DccSurface, _surfaceWindowContents); }

    private serverPlayer GetPlayerWithScreen(screenData.Type type)
    {
        return serverUtils.GetPlayers()
            .FirstOrDefault(p => p.ScreenState.Type == type
                && p.StationId == StationId);
    }

    private void SelectWindowsForType(screenData.Type type, IEnumerable<screenData.Content> contents)
    {
        // Locate the first player who has a control screen open on this station.
        var player = GetPlayerWithScreen(type);
        if (!player)
            return;

        var items = contents
            .Select(c => c.ToString())
            .OrderBy(c => c)
            .Select(t => new DialogList.Item { Name = t.ToUpper(), Id = t });

        var selected = player.WindowIds
            .Where(id => id.State.Type == type)
            .Select(id => id.State.Content.ToString());

        var message = string.Format("Please select windows for {0} : Control Screen",
            DCCScreenData.GetStationName(StationId));

        DialogManager.Instance.ShowListMultiple("SELECT CONTROL WINDOWS",
            message,
            items,
            selected,
            (chosen) =>
            {
                var windowIds = chosen.Select(item =>
                    screenData.WindowIdForState(type, screenData.ContentForName(item.Id)));

                serverUtils.LocalPlayer.PostSetWindows(
                    player.netId, windowIds);
            });
    }

}
