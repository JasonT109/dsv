using System;
using UnityEngine;
using System.Collections;
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

    void Start()
    {
        TopLeft.onClick.AddListener(() => NextScreenContent(DCCScreenID._screenID.screen3));
        TopMid.onClick.AddListener(() => NextScreenContent(DCCScreenID._screenID.screen4));
        TopRight.onClick.AddListener(() => NextScreenContent(DCCScreenID._screenID.screen5));
        // QuadTopLeft.onClick.AddListener(() => NextQuadContent(DCCScreenContentPositions.positionID.topLeft));
        // QuadTopRight.onClick.AddListener(() => NextQuadContent(DCCScreenContentPositions.positionID.topRight));
        // QuadBottomLeft.onClick.AddListener(() => NextQuadContent(DCCScreenContentPositions.positionID.bottomLeft));
        // QuadBottomRight.onClick.AddListener(() => NextQuadContent(DCCScreenContentPositions.positionID.bottomRight));
        // QuadMiddle.onClick.AddListener(() => NextQuadContent(DCCScreenContentPositions.positionID.middle));

        QuadTopLeft.interactable = false;
        QuadTopRight.interactable = false;
        QuadBottomLeft.interactable = false;
        QuadBottomRight.interactable = false;
        QuadMiddle.interactable = false;
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
    }

    public void NextScreenContent(DCCScreenID._screenID id)
    {
        try
        {
            var current = serverUtils.GetScreenContent(id, StationId);
            if (current >= DCCWindow.contentID.batteries)
                current = DCCWindow.contentID.none;
            else
                current = (DCCWindow.contentID) ((int) current + 1);

            serverUtils.PostScreenContent(id, current, StationId);
        }
        catch (Exception) { }
    }

    public void NextQuadContent(DCCScreenContentPositions.positionID id)
    {
        try
        { 
            var current = serverUtils.GetQuadContent(id, StationId);
            if (current >= DCCWindow.contentID.batteries)
                current = DCCWindow.contentID.none;
            else
                current = (DCCWindow.contentID) ((int)current + 1);

            serverUtils.PostQuadContent(id, current, StationId);
        }
        catch (Exception) { }
    }

}
