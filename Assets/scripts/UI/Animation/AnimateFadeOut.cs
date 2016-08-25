using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimateFadeOut : MonoBehaviour
{

    public CanvasGroup Group;
    public Graphic Graphic;

    public float Delay = 2;
    public float Duration = 1;
    public bool ActiveInEditor = true;

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
            Group.DOFade(0, duration).SetDelay(delay);
        }
        else if (Graphic)
        {
            Graphic.DOFade(0, duration).SetDelay(delay);
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
    }

}
