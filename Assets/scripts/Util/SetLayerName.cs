using UnityEngine;

public class SetLayerName : MonoBehaviour 
{
    /** Name of the layer to apply. */
    public string Layer;

    /** Preinitialization. */
    void Awake()
    {
        var layer = LayerMask.NameToLayer(Layer);
        SetLayerRecursively(gameObject, layer);
    }

    private void SetLayerRecursively(GameObject go, int layer)
    {
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
            trans.gameObject.layer = layer;
    }

}
