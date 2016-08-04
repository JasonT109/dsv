using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimateFadeOut : MonoBehaviour
{

    public CanvasGroup Group;
    public float Delay = 2;
    public float Duration = 1;
    public bool ActiveInEditor = true;

    private void Start()
    {
        #if UNITY_EDITOR
        if (!ActiveInEditor)
            return;
        #endif

        Group.alpha = 1;
        Group.DOFade(0, Duration).SetDelay(Delay);
    }

}
