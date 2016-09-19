using UnityEngine;
using System.Collections;

public class EnableMap : MonoBehaviour
{

    void OnEnable()
        { SetEnabled(true); }

    void Update()
        { SetEnabled(true); }

    void OnDisable()
        { SetEnabled(false); }

    void SetEnabled(bool value)
    {
        if (Map.Instance)
            Map.Instance.Root.gameObject.SetActive(value);
    }

}
