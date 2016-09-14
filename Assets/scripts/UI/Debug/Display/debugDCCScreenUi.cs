using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Meg.Networking;

public class debugDCCScreenUi : MonoBehaviour
{
    /** Screen instance title. */
    public Text Title;

    /** Screen type button. */
    public Button TypeButton;

    /** Next screen button. */
    public Button NextButton;

    /** Previous screen type button. */
    public Button PreviousButton;

    /** Local player indicator graphic. */
    public Graphic LocalIndicator;

    /** The player (game instance) whose screen this is. */
    public serverPlayer Player { get; set; }

    /** The screen type text. */
    private Text _typeLabel;


    private void Start()
    {
        _typeLabel = TypeButton.GetComponentInChildren<Text>();
        TypeButton.onClick.AddListener(OnNextClicked);
        NextButton.onClick.AddListener(OnNextClicked);
        PreviousButton.onClick.AddListener(OnPreviousClicked);
    }

    private void Update()
    {
        if (!Player)
            return;

        Title.text = Player.Id;
        _typeLabel.text = Enum.GetName(typeof(screenData.Type), Player.ScreenState.Type).ToUpper();
        TypeButton.interactable = !Player.isLocalPlayer;
        PreviousButton.interactable = !Player.isLocalPlayer;
        NextButton.interactable = !Player.isLocalPlayer;
        LocalIndicator.gameObject.SetActive(Player.isLocalPlayer);
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
                return screenData.Type.DccSurface;
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
            default:
                return screenData.Type.DccControl;
        }
    }


}
