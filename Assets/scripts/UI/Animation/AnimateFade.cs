using System.Collections;
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
    public bool From;
    public bool ActiveInEditor = true;
    public bool PlayOnStart = true;
    public bool PlayOnEnable;
    public bool PlayOnUpdate;

    public bool PlayRandomly;
    public Vector2 RandomInterval = new Vector2(1, 5);

    private void Start()
    {
        if (!Group && !Graphic && !Renderer)
            Group = GetComponent<CanvasGroup>();
        if (!Group && !Graphic && !Renderer)
            Graphic = GetComponent<Graphic>();
        if (!Group && !Graphic && !Renderer)
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

        if (PlayRandomly)
            StartCoroutine(PlayRandomlyRoutine());
    }

    private IEnumerator PlayRandomlyRoutine()
    {
        while (gameObject.activeSelf)
        {
            var t = Random.Range(RandomInterval.x, RandomInterval.y);
            yield return new WaitForSeconds(t);
            Fade();
        }
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

        Tweener tweener = null;
        if (Group)
        {
            Group.alpha = (MinAlpha <= 0) ? 1 : 0;
            tweener = Group.DOFade(MinAlpha, duration);
        }
        else if (Graphic)
            tweener = Graphic.DOFade(MinAlpha, duration).SetDelay(delay);
        else if (Renderer)
            tweener = Renderer.material.DOFade(MinAlpha, "_TintColor", duration);

        if (tweener == null)
            return;

        if (From)
            tweener = tweener.From();

        if (Loops != 0)
            tweener.SetLoops(Loops, LoopType);
        if (delay > 0)
            tweener.SetDelay(delay);
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
