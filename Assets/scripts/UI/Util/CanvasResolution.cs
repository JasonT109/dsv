using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class CanvasResolution : MonoBehaviour
{

    public CanvasScaler CanvasScaler;

    [Serializable]
    public struct Resolution
    {
        public Vector2 Size;
        public float PixelsPerUnit;
    }

    public Resolution[] Resolutions;


    private void Awake()
    {
        if (!CanvasScaler)
            CanvasScaler = GetComponent<CanvasScaler>();
        if (!CanvasScaler)
            return;

        var r = GetClosestResolution();
        if (r.PixelsPerUnit > 0)
            CanvasScaler.dynamicPixelsPerUnit = r.PixelsPerUnit;
    }

    private Resolution GetClosestResolution()
    {
        var screen = new Vector2(Screen.width, Screen.height);
        return Resolutions
            .OrderBy(r => Vector2.Distance(r.Size, screen))
            .FirstOrDefault();
    }
}
