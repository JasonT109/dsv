using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meg.Networking;

public class debugDCCScreensUi : MonoBehaviour
{
    public Transform ScreenContainer;
    public debugDCCScreenUi ScreenUiPrefab;

    private readonly List<debugDCCScreenUi> _screens = new List<debugDCCScreenUi>();
    private const float UpdateInterval = 0.5f;
    private float _nextUpdateTime;

    private void Update()
    {
        if (Time.time < _nextUpdateTime)
            return;

        _nextUpdateTime = Time.time + UpdateInterval;

        if (serverUtils.IsReady())
            UpdateScreens();
    }

    private void UpdateScreens()
    {
        var index = 0;
        var players = serverUtils.GetPlayers()
            .OrderBy(p => p.Id).ToList();

        foreach (var player in players)
            GetScreen(index++).Player = player;

        for (var i = 0; i < _screens.Count; i++)
            _screens[i].gameObject.SetActive(i < index);
    }

    private debugDCCScreenUi GetScreen(int i)
    {
        if (i >= _screens.Count)
        {
            var ui = Instantiate(ScreenUiPrefab);
            ui.transform.SetParent(ScreenContainer, false);
            _screens.Add(ui);
        }

        return _screens[i];
    }

    public void Identify()
    {
        var popup = new popupData.Popup
        {
            Title = "{player-id}",
            Message = "",
            Icon = popupData.Icon.None,
            Type = popupData.Type.Info,
            Color = Color.grey
        };

        serverUtils.PostTogglePopup(popup);
    }

}
