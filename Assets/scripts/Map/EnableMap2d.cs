using UnityEngine;
using System.Collections;

public class EnableMap2d : MonoBehaviour
{

    private bool _quitting;

    void OnEnable()
        { SetEnabled(true); }

    void Update()
        { SetEnabled(true); }

    void OnApplicationQuit()
        { _quitting = true; }

    void OnDisable()
    {
        if (!_quitting)
            SetEnabled(false);
    }

    void SetEnabled(bool value)
    {
        if (!Map2d.HasInstance)
            return;

        var map = Map2d.Instance.gameObject;
        if (map.activeSelf != value)
            map.SetActive(value);
    }

}
