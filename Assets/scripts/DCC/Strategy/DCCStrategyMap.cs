using UnityEngine;
using System.Collections;
using DG.Tweening;
using Meg.Networking;

public class DCCStrategyMap : MonoBehaviour
{

    public GameObject Map3DRoot;
    public GameObject Map2DRoot;
    public GameObject MapSubSchematicRoot;

    public CanvasGroup Fader;

    private mapData.Mode _mode = mapData.Mode.Mode3D ;

    public delegate void MapModeChangeEvent(mapData.Mode oldMode, mapData.Mode newMode);

    public event MapModeChangeEvent OnMapModeChanged;

    public bool IsMap2D
        { get { return serverUtils.MapData.IsMap2D; } }

    public bool IsMap3D
        { get { return serverUtils.MapData.IsMap3D; } }

    public bool IsSubSchematic
        { get { return serverUtils.MapData.IsSubSchematic; } }

    private void Update()
    {
        if (serverUtils.MapData)
            UpdateMapMode(serverUtils.MapData.mapMode);
    }

    public void ToggleMapMode(mapData.Mode mode)
    {
        if (DOTween.IsTweening(Fader))
            return;

        var oldMode = (mapData.Mode) serverUtils.GetServerData("mapMode");
        if (oldMode == mode)
            mode = mapData.Mode.Mode3D;

        serverUtils.PostServerData("mapMode", (int) mode);
    }

    public void ActivateMapMode(mapData.Mode mode)
    {
        if (DOTween.IsTweening(Fader))
            return;

        serverUtils.PostServerData("mapMode", (int) mode);
    }

    private void UpdateMapMode(mapData.Mode mode)
    {
        switch (mode)
        {
            case mapData.Mode.Mode2D:
                Activate(mode, Map2DRoot);
                break;
            case mapData.Mode.Mode3D:
                Activate(mode, Map3DRoot);
                break;
            case mapData.Mode.ModeSubSchematic:
                Activate(mode, MapSubSchematicRoot);
                break;
        }
    }

    private void Activate(mapData.Mode mode, GameObject go)
    {
        if (_mode == mode)
            return;

        var oldMode = _mode;
        _mode = mode;
        StartCoroutine(ActivationRoutine(go));

        if (OnMapModeChanged != null)
            OnMapModeChanged(oldMode, mode);
    }

    private IEnumerator ActivationRoutine(GameObject go)
    {
        if (DOTween.IsTweening(Fader))
            yield break;

        Fader.DOFade(1, 0.25f);
        while (DOTween.IsTweening(Fader))
            yield return 0;

        Map2DRoot.SetActive(go == Map2DRoot);
        Map3DRoot.SetActive(go == Map3DRoot);
        MapSubSchematicRoot.SetActive(go == MapSubSchematicRoot);

        Fader.DOFade(0, 0.25f).SetDelay(0.25f);
    }

}
