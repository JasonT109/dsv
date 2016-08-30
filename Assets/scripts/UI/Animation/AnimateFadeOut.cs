using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimateFadeOut : MonoBehaviour
{

    public CanvasGroup Group;
    public Graphic Graphic;

    public float MinAlpha = 0;
    public float Delay = 2;
    public float Duration = 1;
    public int Loops = 0;
    public LoopType LoopType = LoopType.Yoyo;
    public bool ActiveInEditor = true;
    public bool PlayOnStart = true;

    private void Start()
    {
        if (!Group)
            Group = GetComponent<CanvasGroup>();
        if (!Graphic)
            Graphic = GetComponent<Graphic>();

        #if UNITY_EDITOR
        if (!ActiveInEditor)
            return;
        #endif

        if (PlayOnStart)
            Fade();
    }

    public void Fade(float duration = -1, float delay = -1)
    {
        if (duration < 0)
            duration = Duration;
        if (delay < -1)
            delay = Delay;

        if (Group)
        {
            Group.alpha = 1;
            var tween = Group.DOFade(MinAlpha, duration).SetDelay(delay);
            if (Loops != 0)
                tween.SetLoops(Loops, LoopType);
        }
        else if (Graphic)
        {
            var tween = Graphic.DOFade(MinAlpha, duration).SetDelay(delay);
            if (Loops != 0)
                tween.SetLoops(Loops, LoopType);
        }
    }

    public void Crossfade(float duration = -1, float delay = -1)
    {
        if (duration < 0)
            duration = Duration;
        if (delay < 0)
            delay = Delay;

        var sequence = DOTween.Sequence();

        if (Group)
        {
            sequence.Append(Group.DOFade(1, duration*0.5f));
            sequence.Append(Group.DOFade(0, duration));
        }
        else if (Graphic)
        {
            sequence.Append(Graphic.DOFade(1, duration * 0.5f));
            sequence.Append(Graphic.DOFade(0, duration));
        }

        sequence.Play().SetDelay(delay);

        if (sequence != null && Loops != 0)
            sequence.SetLoops(Loops, LoopType);

    }

}
