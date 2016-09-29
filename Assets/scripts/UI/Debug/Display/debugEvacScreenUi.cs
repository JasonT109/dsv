using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    /** Select windows button. */
    public Button SelectWindowsButton;

    /** Local player indicator graphic. */
    public Graphic LocalIndicator;

    /** The player (game instance) whose screen this is. */
    public serverPlayer Player { get; set; }

    /** The screen content text. */
    private Text _screenLabel;


    private readonly screenData.Content[] _bottomWindows =
    {
        screenData.Content.Controls,
        screenData.Content.Thrusters,
        screenData.Content.SonarLong,
        screenData.Content.Systems,
        screenData.Content.Comms,
        screenData.Content.DiveMode,
        screenData.Content.LifeSupport,
        screenData.Content.Power,
    };

    private readonly screenData.Content[] _topWindows =
    {
        screenData.Content.Thrusters,
        screenData.Content.SonarLong,
        screenData.Content.Comms,
        screenData.Content.DiveMode,
    };

    private readonly screenData.Content[] _noWindows =
    {
    };


    private void Start()
    {
        _screenLabel = ScreenButton.GetComponentInChildren<Text>();
        NextButton.onClick.AddListener(OnNextClicked);
        PreviousButton.onClick.AddListener(OnPreviousClicked);
        NextContentButton.onClick.AddListener(OnNextContentClicked);
        PreviousContentButton.onClick.AddListener(OnPreviousContentClicked);
        SelectWindowsButton.onClick.AddListener(OnSelectWindowsClicked);

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

        NextContentButton.gameObject.SetActive(!Player.isLocalPlayer);
        PreviousContentButton.gameObject.SetActive(!Player.isLocalPlayer);
        SelectWindowsButton.interactable = CanSelectWindows();
    }

    private bool CanSelectWindows()
    {
        switch (Player.ScreenState.Type)
        {
            case screenData.Type.EvacLeft:
            case screenData.Type.EvacMid:
            case screenData.Type.EvacRight:
            case screenData.Type.EvacTop:
                return true;
            default:
                return false;
        }
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

    private void OnNextContentClicked()
    {
        var current = Player.ScreenState.Content;
        var type = Player.ScreenState.Type;
        var next = GetNextContent(type, current);
        serverUtils.PostScreenStateContent(Player.netId, next);
    }

    private void OnPreviousContentClicked()
    {
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
                return screenData.Type.EvacMap;
            case screenData.Type.EvacMap:
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
                return screenData.Type.EvacMap;
            case screenData.Type.EvacMid:
                return screenData.Type.EvacLeft;
            case screenData.Type.EvacRight:
                return screenData.Type.EvacMid;
            case screenData.Type.EvacTop:
                return screenData.Type.EvacRight;
            case screenData.Type.EvacMap:
                return screenData.Type.EvacTop;
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
            case screenData.Content.Controls:
                return screenData.Content.LifeSupport;
            case screenData.Content.LifeSupport:
                return screenData.Content.Power;
            case screenData.Content.Power:
                return screenData.Content.Systems;
            case screenData.Content.Systems:
                return screenData.Content.Controls;
            default:
                return current;
        }
    }

    /** Given a content value, return the previous valid value, cycling round to None. */
    private static screenData.Content GetPreviousTopContent(screenData.Content current)
    {
        switch (current)
        {
            case screenData.Content.Controls:
                return screenData.Content.Systems;
            case screenData.Content.LifeSupport:
                return screenData.Content.Controls;
            case screenData.Content.Power:
                return screenData.Content.LifeSupport;
            case screenData.Content.Systems:
                return screenData.Content.Power;
            default:
                return current;
        }
    }

    private void OnSelectWindowsClicked()
    {
        var type = Player.ScreenState.Type;
        var windows = GetWindowsForType(type);
        SelectWindowsForType(Player.ScreenState.Type, windows);
    }

    private IEnumerable<screenData.Content> GetWindowsForType(screenData.Type type)
    {
        switch (type)
        {
            case screenData.Type.EvacLeft:
            case screenData.Type.EvacMid:
            case screenData.Type.EvacRight:
                return _bottomWindows;

            case screenData.Type.EvacTop:
                return _topWindows;

            default:
                return _noWindows;
        }
    }

    private void SelectWindowsForType(screenData.Type type, IEnumerable<screenData.Content> contents)
    {
        // Locate the first player who has a screen of this type open.
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

        var message = string.Format("Please select windows for {0}", type);

        DialogManager.Instance.ShowListMultiple("SELECT WINDOWS",
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

    private serverPlayer GetPlayerWithScreen(screenData.Type type)
    {
        return serverUtils.GetPlayers()
            .FirstOrDefault(p => p.ScreenState.Type == type);
    }



}
