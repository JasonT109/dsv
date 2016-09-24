using UnityEngine;
using UnityEngine.UI;
using System;
using Meg.Networking;

public class debugDCCScreenUi : MonoBehaviour
{
    /** Screen instance title. */
    public Text Title;

    /** Station button. */
    public Button StationButton;

    /** Station name text. */
    public Text StationName;

    /** Next station button. */
    public Button NextStationButton;

    /** Previous station type button. */
    public Button PreviousStationButton;

    /** Screen type button. */
    public Button TypeButton;

    /** Next screen button. */
    public Button NextButton;

    /** Previous screen type button. */
    public Button PreviousButton;

    /** Local player indicator graphic. */
    public Graphic LocalIndicator;

    /** Local player color. */
    public Color LocalColor;

    /** Station colors. */
    public Color[] StationColors;

    /** The player (game instance) whose screen this is. */
    public serverPlayer Player { get; set; }

    /** The screen type text. */
    private Text _typeLabel;


    private void Start()
    {
        _typeLabel = TypeButton.GetComponentInChildren<Text>();
        NextButton.onClick.AddListener(OnNextClicked);
        PreviousButton.onClick.AddListener(OnPreviousClicked);
        NextStationButton.onClick.AddListener(OnNextStationClicked);
        PreviousStationButton.onClick.AddListener(OnPreviousStationClicked);
    }

    private void Update()
    {
        if (!Player)
            return;

        Title.text = Player.Id;
        StationName.text = DCCScreenData.GetStationName(Player.StationId);
        _typeLabel.text = GetScreenLabel();

        if (Player.isLocalPlayer)
            LocalIndicator.color = LocalColor;
        else
            LocalIndicator.color = ColorForStation(Player.StationId);
    }

    private Color ColorForStation(int id)
    {
        if (id >= 0 && id < StationColors.Length)
            return StationColors[id];

        return new Color(0, 0, 0, 0);
    }

    private string GetScreenLabel()
    {
        return screenData.NameForType(Player.ScreenState.Type).ToUpper();
    }

    private void OnNextClicked()
    {
        var current = Player.ScreenState.Type;
        var next = GetNextType(current);
        serverUtils.PostScreenStateType(Player.netId, next);
    }

    private void OnPreviousClicked()
    {
        var current = Player.ScreenState.Type;
        var next = GetPreviousType(current);
        serverUtils.PostScreenStateType(Player.netId, next);
    }

    private void OnNextStationClicked()
    {
        serverUtils.PostStationId(Player.netId, Player.StationId + 1);
    }

    private void OnPreviousStationClicked()
    {
        serverUtils.PostStationId(Player.netId, Player.StationId - 1);
    }

    /** Given a type value, return the next valid value, cycling round to None. */
    private screenData.Type GetNextType(screenData.Type current)
    {
        switch (current)
        {
            case screenData.Type.DccControl:
                return screenData.Type.DccQuad;
            case screenData.Type.DccQuad:
                return screenData.Type.DccScreen3;
            case screenData.Type.DccScreen3:
                return screenData.Type.DccScreen4;
            case screenData.Type.DccScreen4:
                return screenData.Type.DccScreen5;
            case screenData.Type.DccScreen5:
                return screenData.Type.DccSurface;
            case screenData.Type.DccSurface:
                return screenData.Type.DccStrategy;
            case screenData.Type.DccStrategy:
                return screenData.Type.DccControl;
            default:
                return screenData.Type.DccControl;
        }
    }

    /** Given a type value, return the previous valid value, cycling round to None. */
    private screenData.Type GetPreviousType(screenData.Type current)
    {
        switch (current)
        {
            case screenData.Type.DccControl:
                return screenData.Type.DccStrategy;
            case screenData.Type.DccQuad:
                return screenData.Type.DccControl;
            case screenData.Type.DccScreen3:
                return screenData.Type.DccQuad;
            case screenData.Type.DccScreen4:
                return screenData.Type.DccScreen3;
            case screenData.Type.DccScreen5:
                return screenData.Type.DccScreen4;
            case screenData.Type.DccSurface:
                return screenData.Type.DccScreen5;
            case screenData.Type.DccStrategy:
                return screenData.Type.DccSurface;
            default:
                return screenData.Type.DccControl;
        }
    }


}
