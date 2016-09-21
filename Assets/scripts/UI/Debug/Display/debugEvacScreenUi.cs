using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Meg.Networking;

public class debugEvacScreenUi : MonoBehaviour
{
    /** Screen instance title. */
    public Text Title;

    /** Screen content button. */
    public Button ScreenButton;

    /** Screen content label. */
    public Text Content;

    /** Next screen type button. */
    public Button NextButton;

    /** Previous screen type button. */
    public Button PreviousButton;

    /** Next content button. */
    public Button NextContentButton;

    /** Previous content button. */
    public Button PreviousContentButton;

    /** Local player indicator graphic. */
    public Graphic LocalIndicator;

    /** The player (game instance) whose screen this is. */
    public serverPlayer Player { get; set; }

    /** The screen content text. */
    private Text _screenLabel;


    private void Start()
    {
        _screenLabel = ScreenButton.GetComponentInChildren<Text>();
        NextButton.onClick.AddListener(OnNextClicked);
        PreviousButton.onClick.AddListener(OnPreviousClicked);
        NextContentButton.onClick.AddListener(OnNextContentClicked);
        PreviousContentButton.onClick.AddListener(OnPreviousContentClicked);

        Update();
    }

    private void Update()
    {
        if (!Player)
            return;

        Title.text = Player.Id;
        _screenLabel.text = GetScreenLabel();
        Content.text = screenData.NameForContent(Player.ScreenState.Content);

        ScreenButton.interactable = !Player.isLocalPlayer;
        PreviousButton.interactable = !Player.isLocalPlayer;
        NextButton.interactable = !Player.isLocalPlayer;
        LocalIndicator.gameObject.SetActive(Player.isLocalPlayer);

        var canSetContent = CanSetContent();
        NextContentButton.gameObject.SetActive(canSetContent);
        PreviousContentButton.gameObject.SetActive(canSetContent);
    }

    private bool CanSetContent()
    {
        if (Player.isLocalPlayer)
            return false;

        if (Player.ScreenState.Content == screenData.Content.Debug)
            return false;

        if (Player.ScreenState.Type != screenData.Type.EvacTop)
            return false;

        return true;
    }

    private string GetScreenLabel()
    {
        var content = Player.ScreenState.Content;
        if (content == screenData.Content.Debug)
            return "DEBUG";

        return screenData.NameForType(Player.ScreenState.Type).ToUpper();
    }

    private void OnNextClicked()
    {
        var content = Player.ScreenState.Content;
        if (content == screenData.Content.Debug)
            return;

        var current = Player.ScreenState.Type;
        var next = GetNextType(current);
        serverUtils.PostScreenStateType(Player.netId, next);
    }

    private void OnPreviousClicked()
    {
        var content = Player.ScreenState.Content;
        if (content == screenData.Content.Debug)
            return;

        var current = Player.ScreenState.Type;
        var next = GetPreviousType(current);
        serverUtils.PostScreenStateType(Player.netId, next);
    }

    private void OnNextContentClicked()
    {
        if (!CanSetContent())
            return;

        var current = Player.ScreenState.Content;
        var type = Player.ScreenState.Type;
        var next = GetNextContent(type, current);
        serverUtils.PostScreenStateContent(Player.netId, next);
    }

    private void OnPreviousContentClicked()
    {
        if (!CanSetContent())
            return;

        var current = Player.ScreenState.Content;
        var type = Player.ScreenState.Type;
        var next = GetPreviousContent(type, current);
        serverUtils.PostScreenStateContent(Player.netId, next);
    }

    /** Given a type value, return the next valid value, cycling round to None. */
    private screenData.Type GetNextType(screenData.Type current)
    {
        switch (current)
        {
            case screenData.Type.EvacLeft:
                return screenData.Type.EvacMid;
            case screenData.Type.EvacMid:
                return screenData.Type.EvacRight;
            case screenData.Type.EvacRight:
                return screenData.Type.EvacTop;
            case screenData.Type.EvacTop:
                return screenData.Type.EvacLeft;
            default:
                return screenData.Type.EvacMid;
        }
    }

    /** Given a type value, return the previous valid value, cycling round to None. */
    private screenData.Type GetPreviousType(screenData.Type current)
    {
        switch (current)
        {
            case screenData.Type.EvacLeft:
                return screenData.Type.EvacTop;
            case screenData.Type.EvacMid:
                return screenData.Type.EvacLeft;
            case screenData.Type.EvacRight:
                return screenData.Type.EvacMid;
            case screenData.Type.EvacTop:
                return screenData.Type.EvacRight;
            default:
                return screenData.Type.EvacTop;
        }
    }

    /** Given a content value, return the next valid value, cycling round to None. */
    private static screenData.Content GetNextContent(screenData.Type type, screenData.Content current)
    {
        switch (type)
        {
            case screenData.Type.EvacTop:
                return GetNextTopContent(current);
            default:
                return current;
        }
    }

    /** Given a content value, return the previous valid value, cycling round to None. */
    private static screenData.Content GetPreviousContent(screenData.Type type, screenData.Content current)
    {
        switch (type)
        {
            case screenData.Type.EvacTop:
                return GetPreviousTopContent(current);
            default:
                return current;
        }
    }

    /** Given a content value, return the next valid value, cycling round to None. */
    private static screenData.Content GetNextTopContent(screenData.Content current)
    {
        switch (current)
        {
            case screenData.Content.Thrusters:
                return screenData.Content.LifeSupport;
            case screenData.Content.LifeSupport:
                return screenData.Content.Power;
            case screenData.Content.Power:
                return screenData.Content.Systems;
            case screenData.Content.Systems:
                return screenData.Content.Thrusters;
            default:
                return current;
        }
    }

    /** Given a content value, return the previous valid value, cycling round to None. */
    private static screenData.Content GetPreviousTopContent(screenData.Content current)
    {
        switch (current)
        {
            case screenData.Content.Thrusters:
                return screenData.Content.Systems;
            case screenData.Content.LifeSupport:
                return screenData.Content.Thrusters;
            case screenData.Content.Power:
                return screenData.Content.LifeSupport;
            case screenData.Content.Systems:
                return screenData.Content.Power;
            default:
                return current;
        }
    }


}
