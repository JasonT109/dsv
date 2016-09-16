using UnityEngine;
using Meg.Networking;

public class mapFeatureVis : MonoBehaviour
{
    public GameObject acidLayer;
    public GameObject waterLayer;

    void Update ()
    {
        if (acidLayer)
            acidLayer.SetActive(serverUtils.GetServerBool("acidLayer"));

        if (waterLayer)
            waterLayer.SetActive(serverUtils.GetServerBool("waterLayer"));
        
    }
}
