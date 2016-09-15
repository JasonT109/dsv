using UnityEngine;
using System.Collections;
using DG.Tweening;
using Meg.Networking;

public class DCCStrategyMap : MonoBehaviour
{

    public GameObject Map3DRoot;
    public GameObject Map2DRoot;
    public CanvasGroup Fader;

    private mapData.Mode _mode = mapData.Mode.Mode3D ;

    public bool IsMap2D
        { get { return serverUtils.MapData.IsMap2D; } }

    public bool IsMap3D
        { get { return serverUtils.MapData.IsMap3D; } }

    private void Update()
    {
        if (serverUtils.MapData)
            UpdateMapMode(serverUtils.MapData.mapMode);
    }

    public void ToggleMapMode()
    {
        if (DOTween.IsTweening(Fader))
            return;

        var oldMode = (mapData.Mode) serverUtils.GetServerData("mapMode");
        switch (oldMode)
        {
            case mapData.Mode.Mode2D:
                serverUtils.PostServerData("mapMode", (int) mapData.Mode.Mode3D);
                break;
            case mapData.Mode.Mode3D:
                serverUtils.PostServerData("mapMode", (int) mapData.Mode.Mode2D);
                break;
        }
    }

    private void UpdateMapMode(mapData.Mode mode)
    {
        switch (mode)
        {
            case mapData.Mode.Mode2D:
                ActivateMap2D();
                break;
            case mapData.Mode.Mode3D:
                ActivateMap3D();
                break;
        }
    }

    private void ActivateMap2D()
    {
        if (_mode == mapData.Mode.Mode2D)
            return;

        _mode = mapData.Mode.Mode2D;
        StartCoroutine(ActivateMap2DRoutine());
    }

    private void ActivateMap3D()
    {
        if (_mode == mapData.Mode.Mode3D)
            return;

        _mode = mapData.Mode.Mode3D;
        StartCoroutine(ActivateMap3DRoutine());
    }

    private IEnumerator ActivateMap2DRoutine()
    {
        Fader.DOFade(1, 0.25f);
        while (DOTween.IsTweening(Fader))
            yield return 0;

        Map2DRoot.SetActive(true);
        Map3DRoot.SetActive(false);

        Fader.DOFade(0, 0.25f).SetDelay(0.25f);
    }

    private IEnumerator ActivateMap3DRoutine()
    {
        if (DOTween.IsTweening(Fader))
            yield break;

        Fader.DOFade(1, 0.25f);
        while (DOTween.IsTweening(Fader))
            yield return 0;

        Map3DRoot.SetActive(true);
        Map2DRoot.SetActive(false);

        Fader.DOFade(0, 0.25f).SetDelay(0.25f);
    }

}
