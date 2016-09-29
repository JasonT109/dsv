using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Meg.Networking;
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

    /** Line point scale. */
    public float LinePointScale { get; set; }

    public Map2dLinePoint PointPrefab;

    /** Line style options. */
    public StyleOption[] StyleOptions;


    // Structures
    // ------------------------------------------------------------

    [Serializable]
    public struct StyleOption
    {
        public mapData.LineStyle Style;
        public Texture Texture;
        public float Scale;
        public bool Continuous;
    }


    // Members
    // ------------------------------------------------------------

    /** The vector object component. */
    private VectorObject2D _vectorObject;

    /** The line. */
    private VectorLine _line;

    /** The transform root. */
    private Transform _transformRoot;

    /** The list of point instances. */
    private readonly List<Map2dLinePoint> _points = new List<Map2dLinePoint>();

    /** Currently applied style. */
    private mapData.LineStyle _style = mapData.LineStyle.None;


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
        // Locate the transform root on demand.
        if (!_transformRoot)
        {
            var gesture = transform.GetComponentInParents<TransformGesture>();
            _transformRoot = gesture.transform;
        }

        if (!_transformRoot)
            return;

        // Update line points.
        UpdatePointUis();

        // Initialize line on demand.
        if (!_vectorObject)
            InitLine();

        // Check if line is OK to draw.
        if (_line == null)
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

        // Set line styling.
        if (Line.Style != _style)
            UpdateStyle();

        // Redraw the line.
        _line.color = Line.Color;
        _line.Draw();
    }

    /** Update line styling. */
    private void UpdateStyle()
    {
        _style = Line.Style;
        for (var i = 0; i < StyleOptions.Length; i++)
            if (StyleOptions[i].Style == _style)
            {
                var option = StyleOptions[i];
                _line.texture = option.Texture;
                _line.textureScale = option.Scale;
                _line.continuousTexture = option.Continuous;
            }
    }

    /** Return the line's ith point. */
    public Vector3 GetPoint(int i)
    {
        if (Line.Points != null && i >= 0 && i < Line.Points.Length)
            return Line.Points[i];

        return Vector3.zero;
    }

    /** Return the line's ith point. */
    public void SetPoint(int i, Vector3 value)
    {
        if (Line.Points != null && i >= 0 && i < Line.Points.Length)
        {
            Line.Points[i] = value;
            serverUtils.PostSetMapLine(Line);
        }
    }

    /** Return a point's alpha based on current progress. */
    public float GetPointAlpha(int i)
    {
        var points = Line.Points;
        var n = points != null ? points.Length : 0;
        if (n <= 0 || Progress <= 0)
            return 0;

        if (Progress >= 100)
            return 1;

        var progress = (Progress / 100f) * (n - 1);
        var start = Mathf.FloorToInt(progress);
        var end = Mathf.CeilToInt(progress);
        var t = progress - start;

        if (i < start)
            return 1;

        return i >= end ? 0 : t;
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Update map points. */
    private void UpdatePointUis()
    {
        if (!PointPrefab)
            return;

        // Update points to remain a constant size on screen.
        var s = InitialTransformRootScale / _transformRoot.localScale.x;
        var scale = s * LinePointScale;

        var index = 0;
        var n = Line.Points != null ? Line.Points.Length : 0;
        for (var i = 0; i < n; i++)
        {
            var point = GetPointUi(index++);
            point.Parent = this;
            point.Index = i;
            point.transform.localScale = Vector3.one * scale;
            point.UpdatePoint();
        }

        for (var i = index; i < _points.Count; i++)
            _points[i].gameObject.SetActive(false);
    }

    /** Return a line object for the given index. */
    private Map2dLinePoint GetPointUi(int i)
    {
        if (i >= _points.Count)
        {
            var point = Instantiate(PointPrefab);
            point.transform.SetParent(transform, false);
            point.transform.localPosition = Vector3.zero;
            point.transform.localScale = Vector3.one;
            _points.Add(point);
        }

        _points[i].gameObject.SetActive(true);
        return _points[i];
    }


}
