using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimateFade : MonoBehaviour
{

    public CanvasGroup Group;
    public Graphic Graphic;
    public Renderer Renderer;

    public float MinAlpha = 0;
    public float Delay = 2;
    public float Duration = 1;
    public int Loops = 0;
    public LoopType LoopType = LoopType.Yoyo;
    public bool ActiveInEditor = true;
    public bool PlayOnStart = true;
    public bool PlayOnEnable;
    public bool PlayOnUpdate;
    public bool From;

    private void Start()
    {
        if (!Group)
            Group = GetComponent<CanvasGroup>();
        if (!Graphic)
            Graphic = GetComponent<Graphic>();
        if (!Renderer)
            Renderer = GetComponent<Renderer>();

        #if UNITY_EDITOR
        if (!ActiveInEditor)
            return;
        #endif

        if (PlayOnStart)
            Fade();
    }

    private void OnEnable()
    {
        if (PlayOnEnable)
            Fade();
    }

    private void Update()
    {
        if (PlayOnUpdate)
        {
            if (Group && !DOTween.IsTweening(Group))
                Fade();
            else if (Graphic && !DOTween.IsTweening(Graphic))
                Fade();
            else if (Renderer && !DOTween.IsTweening(Renderer.material))
                Fade();
        }
    }

    public void Fade(float duration = -1, float delay = -1)
    {
        if (duration < 0)
            duration = Duration;
        if (delay < -1)
            delay = Delay;

        Tweener tween = null;
        if (Group)
        {
            Group.alpha = 1;
            tween = Group.DOFade(MinAlpha, duration).SetDelay(delay);
            if (Loops != 0)
                tween.SetLoops(Loops, LoopType);
        }
        else if (Graphic)
        {
            tween = Graphic.DOFade(MinAlpha, duration).SetDelay(delay);
            if (Loops != 0)
                tween.SetLoops(Loops, LoopType);
        }
        else if (Renderer)
        {
            tween = Renderer.material.DOFade(MinAlpha, "_TintColor", duration).SetDelay(delay);
            if (Loops != 0)
                tween.SetLoops(Loops, LoopType);
        }

        if (tween != null && From)
            tween.From();
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
