using UnityEngine;
using Meg.Networking;

public class mapFeatureVis : MonoBehaviour
{
    public GameObject acidLayer;
    public GameObject waterLayer;

    void Update ()
    {
        /*
        
        // Now handled in widgetMap3d.
        // Should be safe to remove this script from map prefabs.
        
        if (acidLayer)
            acidLayer.SetActive(serverUtils.GetServerBool("acidLayer"));

        if (waterLayer)
            waterLayer.SetActive(serverUtils.GetServerBool("waterLayer"));
        */
    }
}
