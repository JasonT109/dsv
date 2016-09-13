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
        _screenLabel.text = GetScreenName(Player);
        Content.text = GetScreenContentName(Player);

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

        var screenId = Player.GameInputs.glScreenID;
        return screenId == glScreenManager.RightScreenId
            || screenId == glScreenManager.LeftScreenId;
    }

    private static string GetScreenName(serverPlayer player)
    {
        if (player.isLocalPlayer)
            return "Debug";

        var id = player.GameInputs.glScreenID;
        switch (id)
        {
            case 2:
                return "Left";
            case 1:
                return "Mid";
            case 0:
                return "Right";
            default:
                return "";
        }
    }

    private static screenData.Type GetScreenType(serverPlayer player)
    {
        if (player.isLocalPlayer)
            return screenData.Type.Default;

        var id = player.GameInputs.glScreenID;
        switch (id)
        {
            case 2:
                return screenData.Type.GliderLeft;
            case 1:
                return screenData.Type.GliderMid;
            case 0:
                return screenData.Type.GliderRight;
            default:
                return screenData.Type.Default;
        }
    }

    private static string GetScreenContentName(serverPlayer player)
    {
        if (player.isLocalPlayer)
            return "";

        return Enum.GetName(typeof(screenData.Content), player.ScreenState.Content).ToUpper();

        // var screenId = player.GameInputs.activeScreen;
        // return glScreenManager.GetScreenName(screenId);
    }

    private void OnNextClicked()
    {
        var next = Player.GameInputs.glScreenID - 1;
        if (next < 0)
            next = 2;

        serverUtils.PostGliderScreenId(Player.netId, next);
    }

    private void OnPreviousClicked()
    {
        var prev = Player.GameInputs.glScreenID + 1;
        if (prev > 2)
            prev = 0;

        serverUtils.PostGliderScreenId(Player.netId, prev);
    }

    private void OnNextContentClicked()
    {
        if (!CanSetContent())
            return;

        var current = Player.ScreenState.Content;
        var type = GetScreenType(Player);
        var next = GetNextContent(type, current);
        serverUtils.PostScreenState(Player.netId, GetScreenState(next));
    }

    private void OnPreviousContentClicked()
    {
        if (!CanSetContent())
            return;

        var current = Player.ScreenState.Content;
        var type = GetScreenType(Player);
        var next = GetPreviousContent(type, current);
        serverUtils.PostScreenState(Player.netId, GetScreenState(next));
    }

    /** Given a content value, return the next valid value, cycling round to None. */
    private screenData.State GetScreenState(screenData.Content content)
        { return new screenData.State { Type = Player.ScreenState.Type, Content = content }; }

    /** Given a content value, return the next valid value, cycling round to None. */
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

    /** Given a content value, return the previous valid value, cycling round to None. */
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

    /** Given a content value, return the next valid value, cycling round to None. */
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

    /** Given a content value, return the previous valid value, cycling round to None. */
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

    /** Given a content value, return the next valid value, cycling round to None. */
    private static screenData.Content GetNextRightScreenContent(screenData.Content current)
    {
        var id = glScreenManager.GetMatrixIdForContent(current) + 1;
        if (id > 7)
            id = 0;

        return glScreenManager.GetContentForId(id);
    }

    /** Given a content value, return the previous valid value, cycling round to None. */
    private static screenData.Content GetPreviousRightScreenContent(screenData.Content current)
    {
        var id = glScreenManager.GetMatrixIdForContent(current) - 1;
        if (id < 0)
            id = 7;

        return glScreenManager.GetContentForId(id);

    }

}
