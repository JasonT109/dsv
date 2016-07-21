using UnityEngine;
using System.Collections;

public class GenerateTextOnEnable : MonoBehaviour
{
    private bool _initial = true;

    void OnDisable()
    {
        _initial = false;
    }

    void OnEnable()
    {
        // if (_initial)
        //    return;

        // Rebuild all dynamic components as object becomes visible.
        var texts = GetComponentsInChildren<DynamicText>();
        foreach (var text in texts)
            text.GenerateMesh();
    }

}
