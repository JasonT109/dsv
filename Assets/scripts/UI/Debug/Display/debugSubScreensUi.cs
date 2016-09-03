using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meg.Networking;

public class debugSubScreensUi : MonoBehaviour
{
    public Transform ScreenContainer;
    public debugSubScreenUi ScreenUiPrefab;

    private List<debugSubScreenUi> _screens = new List<debugSubScreenUi>();
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

    private debugSubScreenUi GetScreen(int i)
    {
        if (i >= _screens.Count)
        {
            var ui = Instantiate(ScreenUiPrefab);
            ui.transform.SetParent(ScreenContainer, false);
            _screens.Add(ui);
        }

        return _screens[i];
    }

}
