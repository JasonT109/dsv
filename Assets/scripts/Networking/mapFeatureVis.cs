using UnityEngine;
using Meg.Networking;

public class mapFeatureVis : MonoBehaviour
{
    public GameObject acidLayer;

	void Update ()
    {
        if (serverUtils.GetServerData("acidLayer") > 0)
            acidLayer.SetActive(true);
        else
            acidLayer.SetActive(false);
	}
}
