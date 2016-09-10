using UnityEngine;
using System.Collections;
using Meg.Maths;

public class AlphaForScale : MonoBehaviour
{

    public Transform Root;

    public AnimationCurve AlphaCurve;
    public Vector2 ScaleRange;

    public CanvasGroup Group;

    private void Start()
    {
        if (!Group)
            Group = GetComponent<CanvasGroup>();

        if (!Root)
            Root = transform;

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
        Group.alpha = AlphaCurve.Evaluate(t);
    }

}
