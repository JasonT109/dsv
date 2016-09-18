using UnityEngine;

[ExecuteInEditMode]
public class InheritSortingOrderUi : MonoBehaviour
{
    public int SortingOrderBonus = 1;

    private Canvas _canvas;
    private Renderer _renderer;

    void Start()
        { UpdateSortingOrder(); }

    private void UpdateSortingOrder()
    {
        // Locate the UI canvas.
        if (!_canvas)
            _canvas = transform.GetComponentInParents<Canvas>();
        if (!_canvas)
            return;

        // Locate the renderer.
        if (!_renderer)
            _renderer = GetComponent<Renderer>();
        if (!_renderer)
        {
            var system = GetComponent<ParticleSystem>();
            if (system != null)
                _renderer = system.GetComponent<Renderer>();
        }
        if (!_renderer)
            return;

        // Update renderer's sorting order.
        _renderer.sortingOrder = _canvas.sortingOrder + SortingOrderBonus;
        _renderer.sortingLayerName = _canvas.sortingLayerName;
    }

    void LateUpdate()
        { UpdateSortingOrder(); }

}
