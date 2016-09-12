using UnityEngine;
using System.Collections;
using Meg.Maths;

public class AlphaForScale : MonoBehaviour
{

    public Transform Root;

    public AnimationCurve AlphaCurve;
    public Vector2 ScaleRange;

    public float AlphaScale = 1;

    private CanvasGroup _group;
    private Renderer _renderer;

    private float _initialAlpha = 1;
    private string _colorProperty = "_Color";

    private void Start()
    {
        _group = GetComponent<CanvasGroup>();
        _renderer = GetComponent<Renderer>();

        if (!Root)
            Root = transform;

        if (_group)
            _initialAlpha = _group.alpha;
        else if (_renderer && _renderer.material.HasProperty("_TintColor"))
            _colorProperty = "_TintColor";

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
        var alpha = _initialAlpha * AlphaCurve.Evaluate(t) * AlphaScale;

        if (_group)
            _group.alpha = alpha;
        else if (_renderer)
        {
            var c = _renderer.material.GetColor(_colorProperty);
            _renderer.material.SetColor(_colorProperty, new Color(c.r, c.g, c.b, alpha));
            _renderer.enabled = alpha > 0;
        }
    }

}
