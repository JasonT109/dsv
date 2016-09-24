using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meg.Networking;
using UnityEngine.UI;

public class debugDCCScreensUi : MonoBehaviour
{
    public Transform ScreenContainer;
    public debugDCCScreenUi ScreenUiPrefab;

    public Graphic SortOrderIdOn;
    public Graphic SortOrderStationOn;

    public enum ScreenSortOrder
    {
        Id,
        Station
    }

    public ScreenSortOrder SortOrder = ScreenSortOrder.Id;

    private readonly List<debugDCCScreenUi> _screens = new List<debugDCCScreenUi>();
    private const float UpdateInterval = 0.5f;
    private float _nextUpdateTime;

    private void Update()
    {
        SortOrderIdOn.gameObject.SetActive(SortOrder == ScreenSortOrder.Id);
        SortOrderStationOn.gameObject.SetActive(SortOrder == ScreenSortOrder.Station);

        if (Time.time < _nextUpdateTime)
            return;

        _nextUpdateTime = Time.time + UpdateInterval;

        if (serverUtils.IsReady())
            UpdateScreens();
    }

    public void SetSortOrderToId()
        { SetSortOrder(ScreenSortOrder.Id); }

    public void SetSortOrderToStation()
        { SetSortOrder(ScreenSortOrder.Station); }

    public void SetSortOrder(ScreenSortOrder order)
        { SortOrder = order; }

    private void UpdateScreens()
    {
        var index = 0;
        var players = GetScreens();

        foreach (var player in players)
            GetScreen(index++).Player = player;

        for (var i = 0; i < _screens.Count; i++)
            _screens[i].gameObject.SetActive(i < index);
    }

    private List<serverPlayer> GetScreens()
    {
        switch (SortOrder)
        {
            case ScreenSortOrder.Id:
                return serverUtils.GetPlayers()
                    .OrderBy(p => p.netId.Value).ToList();

            case ScreenSortOrder.Station:
                return serverUtils.GetPlayers()
                    .OrderBy(p => p.StationId)
                    .ThenBy(p => p.ScreenState.Type)
                    .ToList();

            default:
                return serverUtils.GetPlayers()
                    .OrderBy(p => p.netId.Value).ToList();
        }
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

}
