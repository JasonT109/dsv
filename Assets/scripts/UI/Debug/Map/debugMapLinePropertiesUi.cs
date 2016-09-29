using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using Meg.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class debugMapLinePropertiesUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    public CanvasGroup PropertiesGroup;
    public Transform PropertiesContainer;
    public InputField NameInput;
    public Button SelectColorButton;
    public Graphic Color;

    /** The line being edited. */
    public mapData.Line Line
    {
        get { return _line; }
        set { SetLine(value); }
    }

    // Private Properties
    // ------------------------------------------------------------

    /** Line data. */
    private mapData MapData
        { get { return serverUtils.MapData; } }

    
    // Members
    // ------------------------------------------------------------

    /** The line being edited. */
    private mapData.Line _line;

    /** Whether ui is being updated. */
    private bool _updating;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        ConfigureUi();
        InitUi();
    }

    /** Updating. */
    private void Update()
    {
        UpdateUi();
    }



    // Public Methods
    // ------------------------------------------------------------

    /** Sets the group's name. */
    public void SetName(string value)
    {
        if (_updating)
            return;

        _line.Name = value;
        serverUtils.PostSetMapLine(_line);
    }


    // Private Methods
    // ------------------------------------------------------------

    private void SetLine(mapData.Line value)
    {
        _line = value;
        InitUi();
    }

    private void ConfigureUi()
    {
        NameInput.onEndEdit.AddListener(SetName);
        SelectColorButton.onClick.AddListener(OnSelectColorClicked);
    }

    private void InitUi()
    {
        _updating = true;

        if (Line.Id <= 0)
            { ClearUi(); return; }
        
        PropertiesContainer.gameObject.SetActive(true);
        PropertiesGroup.interactable = true;
        NameInput.text = Line.Name;
        Color.color = Line.Color;

        _updating = false;
    }

    private void UpdateUi()
    {
        _updating = true;

        _updating = false;
    }

    private void ClearUi()
    {
        PropertiesGroup.interactable = false;
        NameInput.text = "PROPERTIES";
        PropertiesContainer.gameObject.SetActive(false);
    }

    private void OnSelectColorClicked()
    {
        DialogManager.Instance.ShowColor("SELECT LINE COLOR",
            string.Format("Please select the color for line '{0}':", Line.Name),
            Line.Color,
            color =>
            {
                _line.Color = color;
                serverUtils.PostSetMapLine(_line);
                Color.color = color;
            });
    }
    

}
