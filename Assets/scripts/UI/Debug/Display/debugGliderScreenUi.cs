using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Meg.Networking;

public class debugGliderScreenUi : MonoBehaviour
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

    private string GetScreenLabel()
    {
        var content = Player.ScreenState.Content;
        if (content == screenData.Content.Debug)
            return "DEBUG";

        return screenData.NameForType(Player.ScreenState.Type).ToUpper();
    }

    private bool CanSetContent()
    {
        if (Player.isLocalPlayer)
            return false;
        if (Player.ScreenState.Content == screenData.Content.Debug)
            return false;

        return true;
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

    /** Given a type value, return the next valid value. */
    private screenData.Type GetNextType(screenData.Type current)
    {
        switch (current)
        {
            case screenData.Type.GliderLeft:
                return screenData.Type.GliderRight;
            case screenData.Type.GliderRight:
                return screenData.Type.GliderLeft;
            default:
                return screenData.Type.GliderRight;
        }
    }

    /** Given a type value, return the previous valid value. */
    private screenData.Type GetPreviousType(screenData.Type current)
    {
        switch (current)
        {
            case screenData.Type.GliderLeft:
                return screenData.Type.GliderRight;
            case screenData.Type.GliderRight:
                return screenData.Type.GliderLeft;
            default:
                return screenData.Type.GliderRight;
        }
    }

    /** Given a content value, return the next valid value. */
    private static screenData.Content GetNextContent(screenData.Type type, screenData.Content current)
    {
        switch (type)
        {
            case screenData.Type.GliderRight:
                return GetNextRightScreenContent(current);
            case screenData.Type.GliderLeft:
                return GetNextLeftScreenContent(current);
            default:
                return current;
        }
    }

    /** Given a content value, return the previous valid value. */
    private static screenData.Content GetPreviousContent(screenData.Type type, screenData.Content current)
    {
        switch (type)
        {
            case screenData.Type.GliderRight:
                return GetPreviousRightScreenContent(current);
            case screenData.Type.GliderLeft:
                return GetPreviousLeftScreenContent(current);
            default:
                return current;
        }
    }

    /** Given a content value, return the next valid value. */
    private static screenData.Content GetNextLeftScreenContent(screenData.Content current)
    {
        switch (current)
        {
            case screenData.Content.LifeSupport:
                return screenData.Content.Power;
            case screenData.Content.Power:
                return screenData.Content.Systems;
            case screenData.Content.Systems:
                return screenData.Content.LifeSupport;
            default:
                return screenData.Content.Power;
        }
    }

    /** Given a content value, return the previous valid value. */
    private static screenData.Content GetPreviousLeftScreenContent(screenData.Content current)
    {
        switch (current)
        {
            case screenData.Content.LifeSupport:
                return screenData.Content.Systems;
            case screenData.Content.Power:
                return screenData.Content.LifeSupport;
            case screenData.Content.Systems:
                return screenData.Content.Power;
            default:
                return screenData.Content.Power;
        }
    }

    /** Given a content value, return the next valid value. */
    private static screenData.Content GetNextRightScreenContent(screenData.Content current)
    {
        switch (current)
        {
            case screenData.Content.Controls:
                return screenData.Content.Nav;
            case screenData.Content.Nav:
                return screenData.Content.SonarLong;
            case screenData.Content.SonarLong:
                return screenData.Content.Towing;
            case screenData.Content.Towing:
                return screenData.Content.Comms;
            case screenData.Content.Comms:
                return screenData.Content.Systems;
            case screenData.Content.Systems:
                return screenData.Content.Controls;
            default:
                return screenData.Content.Controls;
        }
    }

    /** Given a content value, return the previous valid value. */
    private static screenData.Content GetPreviousRightScreenContent(screenData.Content current)
    {
        switch (current)
        {
            case screenData.Content.Controls:
                return screenData.Content.Systems;
            case screenData.Content.Nav:
                return screenData.Content.Controls;
            case screenData.Content.SonarLong:
                return screenData.Content.Nav;
            case screenData.Content.Towing:
                return screenData.Content.SonarLong;
            case screenData.Content.Comms:
                return screenData.Content.Towing;
            case screenData.Content.Systems:
                return screenData.Content.Comms;
            default:
                return screenData.Content.Controls;
        }
    }

}
