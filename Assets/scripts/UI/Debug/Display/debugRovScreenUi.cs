using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Meg.Networking;

public class debugRovScreenUi : MonoBehaviour
{
    /** Screen instance title. */
    public Text Title;

    /** Screen content button. */
    public Button ContentButton;

    /** Next screen button. */
    public Button NextButton;

    /** Previous screen content button. */
    public Button PreviousButton;

    /** Local player indicator graphic. */
    public Graphic LocalIndicator;

    /** The player (game instance) whose screen this is. */
    public serverPlayer Player { get; set; }

    /** The screen content text. */
    private Text _contentLabel;


    private void Start()
    {
        _contentLabel = ContentButton.GetComponentInChildren<Text>();
        ContentButton.onClick.AddListener(OnNextClicked);
        NextButton.onClick.AddListener(OnNextClicked);
        PreviousButton.onClick.AddListener(OnPreviousClicked);
    }

    private void Update()
    {
        if (!Player)
            return;

        Title.text = Player.Id;
        _contentLabel.text = screenData.NameForContent(Player.ScreenState.Content);
        ContentButton.interactable = !Player.isLocalPlayer;
        PreviousButton.interactable = !Player.isLocalPlayer;
        NextButton.interactable = !Player.isLocalPlayer;
        LocalIndicator.gameObject.SetActive(Player.isLocalPlayer);
    }

    private void OnNextClicked()
    {
        var current = Player.ScreenState.Content;
        if (current == screenData.Content.Debug)
            return;

        var next = GetNextContent(current);
        serverUtils.PostScreenState(Player.netId, new screenData.State { Type = screenData.Type.Rov, Content = next });
    }

    private void OnPreviousClicked()
    {
        var current = Player.ScreenState.Content;
        if (current == screenData.Content.Debug)
            return;

        var next = GetPreviousContent(current);
        serverUtils.PostScreenState(Player.netId, new screenData.State { Type = screenData.Type.Rov, Content = next });
    }

    /** Given a content value, return the next valid value, cycling round to None. */
    private screenData.Content GetNextContent(screenData.Content current)
    {
        switch (current)
        {
            case screenData.Content.Cameras:
                return screenData.Content.SonarLong;
            case screenData.Content.SonarLong:
                return screenData.Content.Lights;
            case screenData.Content.Lights:
                return screenData.Content.Cameras;
            default:
                return screenData.Content.Cameras;
        }
    }

    /** Given a content value, return the previous valid value, cycling round to None. */
    private screenData.Content GetPreviousContent(screenData.Content current)
    {
        switch (current)
        {
            case screenData.Content.Cameras:
                return screenData.Content.Lights;
            case screenData.Content.SonarLong:
                return screenData.Content.Cameras;
            case screenData.Content.Lights:
                return screenData.Content.SonarLong;
            default:
                return screenData.Content.Cameras;
        }
    }


}
