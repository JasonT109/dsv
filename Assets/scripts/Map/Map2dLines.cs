using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Meg.Networking;

public class Map2dLines : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** The root transform in which to place lines. */
    public Transform Root;


    [Header("Configuration")]

    /** Scaling factor from normalized space into root space. */
    public float LineToRootScale = 1;

    /** Server parameter used to drive scale factor (optional). */
    public string ScaleServerParam;

    /** Line width scaling factor. */
    public float LineWidthScale = 1;

    /** Initial transform root scale. */
    public float InitialTransformRootScale = 1;


    [Header("Prefabs")]

    /** Line prefab. */
    public Map2dLine LinePrefab;


    // Private Properties
    // ------------------------------------------------------------

    /** Map data. */
    private static mapData MapData
        { get { return serverUtils.MapData; } }



    // Members
    // ------------------------------------------------------------

    /** Initial root scale. */
    private float _initialRootScale;

    /** The list of line instances. */
    private readonly List<Map2dLine> _lines = new List<Map2dLine>();



    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        _initialRootScale = LineToRootScale;

        if (!Root)
            Root = transform;
    }

    /** Update lines. */
    private void LateUpdate()
    {
        UpdateLines();
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Update map lines. */
    private void UpdateLines()
    {
        if (!MapData)
            return;

        if (!string.IsNullOrEmpty(ScaleServerParam))
            LineToRootScale = serverUtils.GetServerData(ScaleServerParam, LineToRootScale) * _initialRootScale;

        transform.localScale = Vector3.one * LineToRootScale;

        var index = 0;
        var n = MapData.Lines.Count;
        var nPercentages = MapData.LinePercentages.Count;
        for (var i = 0; i < n; i++)
        {
            var line = GetLine(index++);
            line.LineWidthScale = LineWidthScale;
            line.InitialTransformRootScale = InitialTransformRootScale;
            line.Line = MapData.Lines[i];
            line.Progress = i < nPercentages ? MapData.LinePercentages[i] : 0f;
            line.UpdateLine();
        }

        for (var i = index; i < _lines.Count; i++)
            _lines[i].gameObject.SetActive(false);
    }

    /** Return a line object for the given index. */
    private Map2dLine GetLine(int i)
    {
        // Add a new line instance if needed.
        if (i >= _lines.Count)
        {
            var line = Instantiate(LinePrefab);
            line.transform.SetParent(Root, false);
            line.transform.localPosition = Vector3.zero;
            line.transform.localScale = Vector3.one;
            _lines.Add(line);
        }

        _lines[i].gameObject.SetActive(true);

        // Return the line instance.
        return _lines[i];
    }

}
