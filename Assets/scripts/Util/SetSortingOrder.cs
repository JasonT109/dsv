using UnityEngine;

[ExecuteInEditMode]
public class SetSortingOrder : MonoBehaviour
{
    public string SortingLayer;
    public int SortingOrder = 0;

    void Start()
    { UpdateSortingOrder(); }

    private void UpdateSortingOrder()
    {
        var r = GetComponent<Renderer>();
        if (r == null)
        {
            var system = GetComponent<ParticleSystem>();
            if (system != null)
                r = system.GetComponent<Renderer>();
        }

        if (r == null)
            return;

        r.sortingOrder = SortingOrder;

        if (!string.IsNullOrEmpty(SortingLayer))
            r.sortingLayerName = SortingLayer;
    }

    #if UNITY_EDITOR
    void Update()
    { UpdateSortingOrder(); }
    #endif

}
