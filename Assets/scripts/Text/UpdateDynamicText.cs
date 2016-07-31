using UnityEngine;
using System.Collections;

public class UpdateDynamicText : MonoBehaviour
{

    void OnEnable()
    {
        // Enable dynamic text pixel snapping as object becomes visible.
        var texts = GetComponentsInChildren<DynamicText>();
        foreach (var text in texts)
            text.pixelSnapTransformPos = true;
    }

}
