using UnityEngine;
using System.Collections;

/** Enable a set of game object based on whether this object is enabled. */

public class EnableGameObjects : MonoBehaviour
{

    public GameObject[] Objects;

    void OnEnable()
    {
        SetEnabled(true);
    }

    void OnDisable()
    {
        SetEnabled(false);
    }

    void SetEnabled(bool value)
    {
        foreach (var go in Objects)
            if (go)
                go.SetActive(value);
    }

}
