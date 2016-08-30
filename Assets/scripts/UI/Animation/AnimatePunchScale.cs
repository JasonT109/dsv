using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AnimatePunchScale : MonoBehaviour
{

    public Vector3 Amount;
    public float Duration = 0.25f;
    public float Delay;
    public int Loops;
    public LoopType LoopType;

    public bool PlayOnStart = true;
    public bool PlayOnEnable;
    public bool PlayOnUpdate;
    public bool From;

    private void Start()
    {
        if (PlayOnStart)
            Play();
    }

    private void OnEnable()
    {
        if (PlayOnEnable)
            Play();
    }

    private void Update()
    {
        if (PlayOnUpdate && !DOTween.IsTweening(transform))
            Play();
    }

    public void Play()
    {
        var tween = transform.DOPunchScale(Amount, Duration).SetDelay(Delay);
        if (Loops != 0)
            tween.SetLoops(Loops, LoopType);
        if (tween != null && From)
            tween.From();
    }

}
