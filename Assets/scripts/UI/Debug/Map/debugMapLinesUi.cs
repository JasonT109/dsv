using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Meg.Networking;
using UnityEngine.UI;


public class debugMapLinesUi : Singleton<debugMapLinesUi>
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Container that will hold line renderers. */
    public Transform LineContainer;

    /** Button for adding a line. */
    public Button AddButton;

    /** Button for removing a line. */
    public Button RemoveButton;

    /** Button for clearing extra lines. */
    public Button ClearButton;

    /** Properties interface. */
    public debugMapLinePropertiesUi Properties;


    [Header("Prefabs")]

    /** The line item renderer prefab. */
    public debugMapLineUi LineUiPrefab;

    /** The selected line. */
    public mapData.Line Selected
    {
        get { return _selected; }
        set { Select(value); }
    }



    // Private Properties
    // ------------------------------------------------------------

    /** The server's line data. */
    private mapData MapData
        { get { return serverUtils.MapData; } }

    /** Whether any lines can be removed. */
    private bool CanRemoveMapLines
        { get { return MapData.Lines.Count > 0; } }


    // Members
    // ------------------------------------------------------------

    /** MapLine renderers. */
    private readonly List<debugMapLineUi> _lines = new List<debugMapLineUi>();

    /** Selected line. */
    private mapData.Line _selected;


    // Unity Methods
    // ------------------------------------------------------------

    private void Awake()
    {
        if (!LineContainer)
            LineContainer = transform;
    }

    private void Update()
    {
        if (!MapData)
            return;

        UpdateMapLines();
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Set the selected line. */
    public void Select(mapData.Line line)
    {
        _selected = line;
        Properties.Line = line;
    }

    /** Add a line to the line list. */
    public void AddMapLine()
    {
        serverUtils.PostAddMapLine(new mapData.Line
        {
            Name = "Line",
            Color = Color.white,
            Style = mapData.LineStyle.Normal,
            Width = 0.1f
        });
    }

    /** Remove the last line in list. */
    public void RemoveMapLine()
    {
        if (!CanRemoveMapLines)
            return;

        var lineName = MapData.GetLine(MapData.LastLine).Name;
        DialogManager.Instance.ShowYesNo("REMOVE LAST MAP LINE?",
            string.Format("Are you sure you wish to remove the last line '{0}'?", lineName),
            result =>
            {
                if (result != DialogYesNo.Result.Yes)
                    return;

                serverUtils.PostRemoveLastMapLine();
                Select(MapData.GetLine(MapData.LastLine));
            });
    }

    /** Clear map lines. */
    public void ClearMapLines()
    {
        if (!CanRemoveMapLines)
            return;

        DialogManager.Instance.ShowYesNo("REMOVE ALL MAP LINES?",
            "Are you sure you wish to remove all map lines from view?",
            result =>
            {
                if (result != DialogYesNo.Result.Yes)
                    return;

                serverUtils.PostClearMapLines();
                Select(MapData.GetLine(MapData.LastLine));
            });
    }


    // Private Methods
    // ------------------------------------------------------------

    private void UpdateMapLines()
    {
        AddButton.interactable = true;
        RemoveButton.interactable = CanRemoveMapLines;
        ClearButton.interactable = CanRemoveMapLines;

        var lines = MapData.Lines;
        var index = 0;
        foreach (var line in lines)
        {
            var ui = GetLineUi(index++);
            ui.Lines = this;
            ui.Line = line;
        }

        for (var i = 0; i < _lines.Count; i++)
            _lines[i].gameObject.SetActive(i < index);
    }

    private debugMapLineUi GetLineUi(int i)
    {
        if (i >= _lines.Count)
        {
            var line = Instantiate(LineUiPrefab);
            line.transform.SetParent(LineContainer, false);
            _lines.Add(line);
        }

        _lines[i].gameObject.SetActive(true);

        return _lines[i];
    }
}
