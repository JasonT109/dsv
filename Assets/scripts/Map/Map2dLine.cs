using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TouchScript.Gestures;
using Vectrosity;

[RequireComponent(typeof(VectorLine))]
public class Map2dLine : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    /** The backing line data. */
    public mapData.Line Line { get; set; }

    /** Line's progress percentage. */
    public float Progress { get; set; }

    /** Initial transform root scale. */
    public float InitialTransformRootScale { get; set; }

    /** Line width scale. */
    public float LineWidthScale { get; set; }


    // Members
    // ------------------------------------------------------------

    /** The vector object component. */
    private VectorObject2D _vectorObject;

    /** The line. */
    private VectorLine _line;

    /** The transform root. */
    private Transform _transformRoot;


    // Unity Methods
    // ------------------------------------------------------------

    /** Enabling. */
    private void OnEnable()
        { InitLine(); }

    /** Initialize the line component. */
    private void InitLine()
    {
        if (_vectorObject)
            return;

        // Initialize line points.
        _vectorObject = GetComponent<VectorObject2D>();
        _line = _vectorObject.vectorLine;
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Update the line component. */
    public void UpdateLine()
    {
        // Initialize line on demand.
        if (!_vectorObject)
            InitLine();

        // Locate the transform root on demand.
        if (!_transformRoot)
        {
            var gesture = transform.GetComponentInParents<TransformGesture>();
            _transformRoot = gesture.transform;
        }

        // Check if line is OK to draw.
        if (_line == null || !_transformRoot)
            return;

        // Update line size.
        var points = Line.Points;
        var n = points != null ? points.Length : 0;
        if (_line.points2.Count != n)
        {
            // HACK: Force Vectrocity to reset the line by changing its type.
            _line.lineType = LineType.Points;
            _line.lineType = LineType.Continuous;
            _line.Resize(n);
        }

        // Check for empty line.
        if (n == 0)
            return;

        // Update line positions.
        for (var i = 0; i < n; i++)
            _line.points2[i] = Line.Points[i];

        // Update line progress fraction.
        var progress = (Progress / 100f) * (n - 1);
        var start = Mathf.FloorToInt(progress);
        var end = Mathf.CeilToInt(progress);
        var t = progress - start;
        _line.drawEnd = end;

        if (end > 0 && start < end)
        {
            var p0 = _line.points2[end - 1];
            var p1 = _line.points2[end];
            _line.points2[end] = Vector2.Lerp(p0, p1, t);
        }

        // Update line width to remain a constant size on screen.
        var s = InitialTransformRootScale / _transformRoot.localScale.x;
        _line.lineWidth = Line.Width * s * LineWidthScale;

        // Redraw the line.
        _line.color = Line.Color;
        _line.Draw();
    }

}
