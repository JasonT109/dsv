using UnityEngine;
using System.Collections;

public class AlphaForTime : MonoBehaviour
{

    public AnimationCurve Alpha;
    public float StartTime = 0;
    public float StopTime = 1;
    public bool Loop = true;
    public float LoopTime = 1;

    private CanvasGroup _group;
    private float _startTime;

    private void Start()
    {
        _group = GetComponent<CanvasGroup>();
        _startTime = Time.time;
        UpdateAlpha();
    }

    private void Update()
    {
        UpdateAlpha();
    }

    private void UpdateAlpha()
    {
        if (!_group)
            return;

        var time = Time.time - _startTime;
        if (Loop)
            time = Mathf.Repeat(time, LoopTime);

        var t = (time - StartTime) / (StopTime - StartTime);

        _group.alpha = Alpha.Evaluate(t);
    }
}
