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
    public float InitialTransformRootScale = 0.35f;


    // Members
    // ------------------------------------------------------------

    /** The vector object component. */
    private VectorObject2D _vectorObject;

    /** The line. */
    private VectorLine _line;

    /** The transform root. */
    private Transform _transformRoot;

    /** Line's initial width. */
    private float _initialLineWidth;



    // Unity Methods
    // ------------------------------------------------------------

    /** Enabling. */
    private void OnEnable()
        { InitLine(); }

    /** Updating. */
    private void LateUpdate()
        { UpdateLine(); }

    /** Initialize the line component. */
    private void InitLine()
    {
        if (_vectorObject)
            return;

        // Initialize line points.
        _vectorObject = GetComponent<VectorObject2D>();
        _line = _vectorObject.vectorLine;
        _initialLineWidth = _line.lineWidth;
    }

    /** Update the line component. */
    private void UpdateLine()
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

        var points = Line.Points;
        if (points == null)
            return;

        var n = points.Length;
        if (_line.points2.Count != n)
        {
            // HACK: Force Vectrocity to reset the line by changing its type.
            _line.lineType = LineType.Points;
            _line.lineType = LineType.Continuous;
            _line.Resize(n);
        }

        for (var i = 0; i < n; i++)
            _line.points2[i] = Line.Points[i];

        // Update line progress fraction.
        var progress = (Progress / 100f) * (n - 1);
        var end = Mathf.CeilToInt(progress);
        var t = progress - Mathf.FloorToInt(progress);
        _line.drawEnd = end;

        if (end > 0)
        {
            var p0 = _line.points2[end - 1];
            var p1 = _line.points2[end];
            _line.points2[end] = Vector2.Lerp(p0, p1, t);
        }

        // Update line widths to remain a constant size on screen.
        var s = InitialTransformRootScale / _transformRoot.localScale.x;
        _line.lineWidth = _initialLineWidth * s;

        _line.Draw();
    }

}
