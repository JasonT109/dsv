using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimateTextAlpha : MonoBehaviour
{
    public AnimationCurve Alpha;
    public float StartTime = 0;
    public float StopTime = 1;
    public bool Loop = true;
    public float LoopTime = 1;

    private Text _text;
    private float _startTime;

    private void Start()
    {
        _text = GetComponent<Text>();
        _startTime = Time.time;
        UpdateAlpha();
    }

    private void Update()
    {
        UpdateAlpha();
    }

    private void UpdateAlpha()
    {
        if (!_text)
            return;

        var time = Time.time - _startTime;
        if (Loop)
            time = Mathf.Repeat(time, LoopTime);

        var t = (time - StartTime) / (StopTime - StartTime);

        _text.color = new Color (_text.color.r, _text.color.g, _text.color.b, Alpha.Evaluate(t));
    }
}
