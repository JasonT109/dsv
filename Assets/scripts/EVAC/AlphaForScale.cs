using UnityEngine;
using System.Collections;
using Meg.Maths;

public class AlphaForScale : MonoBehaviour
{

    public Transform Root;

    public AnimationCurve AlphaCurve;
    public Vector2 ScaleRange;

    private CanvasGroup _group;

    private float _initialAlpha = 1;

    private void Start()
    {
        _group = GetComponent<CanvasGroup>();

        if (!Root)
            Root = transform;

        _initialAlpha = _group.alpha;

        UpdateAlpha();
    }

    private void Update()
    {
        UpdateAlpha();
    }

    private void UpdateAlpha()
    {
        var scale = Root.localScale.x;
        var t = graphicsMaths.remapValue(scale, ScaleRange.x, ScaleRange.y, 0, 1);
        _group.alpha = _initialAlpha * AlphaCurve.Evaluate(t);
    }

}
