using UnityEngine;
using System.Collections;

public class SpawnOnEnable : MonoBehaviour
{

    public GameObject Prefab;
    public bool Reparent = true;
    public bool WorldPositionStays = false;

    private GameObject _instance;

	private void OnEnable()
    {
        if (_instance)
            return;

	    _instance = Instantiate(Prefab);
        if (Reparent)
            _instance.transform.SetParent(transform, WorldPositionStays);
    }
}
