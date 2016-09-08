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
    public Button ContentButton;

    /** Next screen type button. */
    public Button NextButton;

    /** Previous screen type button. */
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
        _contentLabel.text = GetScreenName(Player);

        ContentButton.interactable = !Player.isLocalPlayer;
        PreviousButton.interactable = !Player.isLocalPlayer;
        NextButton.interactable = !Player.isLocalPlayer;
        LocalIndicator.gameObject.SetActive(Player.isLocalPlayer);
    }

    private static string GetScreenName(serverPlayer player)
    {
        if (player.isLocalPlayer)
            return "Debug";

        var id = player.GameInputs.glScreenID;
        switch (id)
        {
            case 2:
                return "Right";
            case 1:
                return "Mid";
            case 0:
                return "Left";
            default:
                return "Mid";
        }
    }

    private void OnNextClicked()
    {
        var next = Player.GameInputs.glScreenID - 1;
        if (next < 0)
            next = 2;

        // serverUtils.PostGliderScreenId(Player.netId, next);
    }

    private void OnPreviousClicked()
    {
        var prev = Player.GameInputs.glScreenID + 1;
        if (prev > 2)
            prev = 0;

        // serverUtils.PostGliderScreenId(Player.netId, prev);
    }

}
