using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Meg.Networking;

public class debugMapLineUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Name label for the line. */
    public Text NameLabel;

    /** The line's backdrop graphic. */
    public Graphic Backdrop;

    /** The line being displayed. */
    public mapData.Line Line { get; set; }

    /** Parent line list. */
    public debugMapLinesUi Lines { get; set; }


    // Private Properties
    // ------------------------------------------------------------

    /** The server's map data. */
    private mapData MapData
        { get { return serverUtils.MapData; } }


    // Members
    // ------------------------------------------------------------

    /** Whether ui is being updated. */
    private bool _updating;


    // Public Methods
    // ------------------------------------------------------------

    /** Select this line. */
    public void Select()
        { Lines.Selected = Line; }



    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        if (MapData)
            UpdateLineUi();
    }

    /** Updating. */
    private void Update()
    {
        if (MapData)
            UpdateLineUi();
    }


    // Private Methods
    // ------------------------------------------------------------

    private void UpdateLineUi()
    {
        _updating = true;

        var isSelected = Lines.Selected.Id == Line.Id;
        Backdrop.color = HSBColor.FromColor(Line.Color)
            .Brighten(isSelected ? 0.75f : 0.15f)
            .ToColor();

        NameLabel.text = Line.Name;

        _updating = false;
    }

}
