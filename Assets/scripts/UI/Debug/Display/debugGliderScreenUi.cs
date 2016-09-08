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
        ScreenButton.onClick.AddListener(OnNextClicked);
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

        var screenId = Player.GameInputs.glScreenID;
        var canSetContent = (screenId == glScreenManager.RightScreenId) && !Player.isLocalPlayer;
        NextContentButton.gameObject.SetActive(canSetContent);
        PreviousContentButton.gameObject.SetActive(canSetContent);
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

    private static string GetScreenContentName(serverPlayer player)
    {
        if (player.isLocalPlayer)
            return "";

        var screenId = player.GameInputs.activeScreen;
        return glScreenManager.GetScreenName(screenId);
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
        var current = Player.ScreenState.Content;
        if (current == screenData.Content.Debug)
            return;

        var next = GetNextContent(current);
        serverUtils.PostScreenState(Player.netId, GetRightScreenState(next));
    }

    private void OnPreviousContentClicked()
    {
        var current = Player.ScreenState.Content;
        if (current == screenData.Content.Debug)
            return;

        var next = GetPreviousContent(current);
        serverUtils.PostScreenState(Player.netId, GetRightScreenState(next));
    }

    /** Given a content value, return the next valid value, cycling round to None. */
    private static screenData.State GetRightScreenState(screenData.Content content)
        { return new screenData.State { Type = screenData.Type.GliderRight, Content = content }; }

    /** Given a content value, return the next valid value, cycling round to None. */
    private static screenData.Content GetNextContent(screenData.Content current)
    {
        var id = glScreenManager.GetMatrixIdForContent(current) + 1;
        if (id > 7)
            id = 0;

        return glScreenManager.GetContentForId(id);
    }

    /** Given a content value, return the previous valid value, cycling round to None. */
    private static screenData.Content GetPreviousContent(screenData.Content current)
    {
        var id = glScreenManager.GetMatrixIdForContent(current) - 1;
        if (id < 0)
            id = 7;

        return glScreenManager.GetContentForId(id);

    }

}
