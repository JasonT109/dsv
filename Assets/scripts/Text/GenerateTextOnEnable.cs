using UnityEngine;
using System.Collections;

public class GenerateTextOnEnable : MonoBehaviour
{

    void OnEnable()
    {
        // Rebuild all dynamic components as object becomes visible.
        var texts = GetComponentsInChildren<DynamicText>();
        foreach (var text in texts)
            text.GenerateMesh();
    }

}
