using UnityEngine;
using System.Collections;

public class EnableMap : MonoBehaviour
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
        if (!Map.Instance)
            return;

        var root = Map.Instance.Root.gameObject;
        if (root.activeSelf != value)
            root.SetActive(value);
    }

}
