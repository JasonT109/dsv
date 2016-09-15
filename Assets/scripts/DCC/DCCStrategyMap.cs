using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DCCStrategyMap : MonoBehaviour
{

    public GameObject Map3DRoot;
    public GameObject Map2DRoot;
    public CanvasGroup Fader;

    public bool IsMap2d
        { get { return !IsMap3d; } }

    public bool IsMap3d
        { get { return Map3DRoot.activeSelf; } }

    public void ActivateMap2D()
        { StartCoroutine(ActivateMap2DRoutine()); }

    public void ActivateMap3D()
        { StartCoroutine(ActivateMap3DRoutine()); }

    public void ToggleMap2d()
    {
        if (IsMap3d)
            ActivateMap2D();
        else
            ActivateMap3D();
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
