using UnityEngine;
using System.Collections;

public class UpdateDynamicText : MonoBehaviour
{
    public const float RegenerateExtraInterval = 0.25f;

    public bool scaleTextMinSize = false;
    public int minTextSize = 19;

    void OnEnable()
        { StartCoroutine(UpdateTextRoutine()); }

    private IEnumerator UpdateTextRoutine()
    {
        // Enable dynamic text pixel snapping as object becomes visible.
        yield return 0;
        var texts = GetComponentsInChildren<DynamicText>();
        foreach (var text in texts)
        {
            var widget = text.GetComponent<widgetText>();

            if (widget && !widget.PixelSnapping)
                continue;
            
            text.pixelSnapTransformPos = true;
        }

        // Regenerate text meshes.
        yield return 0;
        foreach (var text in texts)
            text.GenerateMesh();

        // Wait a bit, then regenerate text meshes again.
        // This is just in case the first attempt fails to do the job.
        // (we've been having issues with dynamic text getting corrupted.)
        yield return new WaitForSeconds(RegenerateExtraInterval);
        foreach (var text in texts)
            text.GenerateMesh();
    }

    void UpdateTextSize()
    {
        var texts = GetComponentsInChildren<DynamicText>();
        foreach (var text in texts)
        {
            float parentScale = transform.parent.transform.localScale.x;
            float t = minTextSize;

            t *= parentScale;
            text.minFontPxSize = Mathf.RoundToInt(t);

            text.GenerateMesh();
        }
    }

    void Update()
    {
        if (scaleTextMinSize)
            UpdateTextSize();
    }
}
