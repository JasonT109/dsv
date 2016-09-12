using UnityEngine;
using Meg.Maths;
using Meg.Networking;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

[RequireComponent(typeof(VectorLine))]
public class MapGrid : MonoBehaviour
{

    // Constants
    // ------------------------------------------------------------


    // Properties
    // ------------------------------------------------------------

    [Header("Dimensions")]

    /** Spacing between grid lines. */
    public float Spacing = 340;


    // Members
    // ------------------------------------------------------------

    /** The line component. */
    private VectorObject2D _vectorObject;

    /** Canvas renderer. */
    private CanvasRenderer _renderer;


    // Unity Methods
    // ------------------------------------------------------------

    /** Startup. */
    private void Awake()
    {
        // Disable clipping.
        _renderer = GetComponent<CanvasRenderer>();
    }

    /** Enabling. */
    private void OnEnable()
    {
    }

    /** Update the bar. */
    private void LateUpdate()
    {
        UpdateLine();
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Initialize the line component. */
    private void InitLine()
    {
        if (_vectorObject)
            return;

        // Initialize line points.
        _vectorObject = GetComponent<VectorObject2D>();
        var line = _vectorObject.vectorLine;
        var rt = GetComponent<RectTransform>();
        var height = rt.sizeDelta.y;
        var width = rt.sizeDelta.x;
        var rows = Mathf.CeilToInt(height / Spacing);
        var cols = Mathf.CeilToInt(width / Spacing);
        var count = rows * 2 + cols * 2;
        if (rows <= 0 || height <= 0)
            return;

        var index = 0;
        line.Resize(count);

        var cx = width * 0.5f;
        var cy = height * 0.5f;

        for (var i = 0; i < rows; i++)
        {
            var y = i * Spacing - cy;
            line.points2[index++] = new Vector2(-cx, y);
            line.points2[index++] = new Vector2(cx, y);
        }

        for (var i = 0; i < cols; i++)
        {
            var x = i * Spacing - cx;
            line.points2[index++] = new Vector2(x, -cy);
            line.points2[index++] = new Vector2(x, cy);
        }
    }

    /** Update the bar from current value. */
    private void UpdateLine()
    {
        // Initialize line on demand.
        if (!_vectorObject)
            InitLine();

        // Check if line is OK to draw.
        if (!_vectorObject)
            return;

        // Redraw the line.
        var line = _vectorObject.vectorLine;
        line.Draw();
    }

}
