using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

    public Slider WidthSlider;
    public InputField WidthInput;

    public Button SelectStyleButton;
    public Text Style;

    public Button SelectPointStyleButton;
    public Text PointStyle;

    public InputField PointCountInput;

    public Button AddPointButton;
    public Button RemovePointButton;

    public Transform PointContainer;

    public Slider PercentageSlider;
    public InputField PercentageInput;


    [Header("Prefabs")]

    public debugMapLinePointUi PointUiPrefab;

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
    {
        get { return serverUtils.MapData; }
    }


    // Members
    // ------------------------------------------------------------

    /** The line being edited. */
    private mapData.Line _line;

    /** Whether ui is being updated. */
    private bool _updating;

    /** Point UI renderers. */
    private readonly List<debugMapLinePointUi> _points = new List<debugMapLinePointUi>();


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

    /** Sets the line's name. */
    public void SetName(string value)
    {
        if (_updating)
            return;

        _line.Name = value;
        serverUtils.PostSetMapLine(_line);
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
        WidthSlider.onValueChanged.AddListener(OnWidthSliderChanged);
        WidthInput.onEndEdit.AddListener(OnWidthInputEndEdit);
        SelectStyleButton.onClick.AddListener(OnSelectStyleClicked);
        SelectPointStyleButton.onClick.AddListener(OnSelectPointStyleClicked);
        PointCountInput.onEndEdit.AddListener(OnPointCountInputEndEdit);
        AddPointButton.onClick.AddListener(OnAddPointClicked);
        RemovePointButton.onClick.AddListener(OnRemovePointClicked);
        PercentageSlider.onValueChanged.AddListener(OnPercentageSliderChanged);
        PercentageInput.onEndEdit.AddListener(OnPercentageInputEndEdit);
    }

    private void InitUi()
    {
        _updating = true;

        if (Line.Id <= 0)
        {
            ClearUi();
            return;
        }

        PropertiesContainer.gameObject.SetActive(true);
        PropertiesGroup.interactable = true;
        NameInput.text = Line.Name;
        Color.color = Line.Color;
        WidthSlider.value = Line.Width;
        WidthInput.text = Line.Width.ToString();
        PointCountInput.text = PointCount.ToString();
        Style.text = Line.Style.ToString();
        PointStyle.text = Line.PointStyle.ToString();

        var percent = serverUtils.MapData.GetLinePercent(Line.Id);
        PercentageSlider.maxValue = Mathf.Max(PercentageSlider.maxValue, percent);
        PercentageSlider.value = percent;
        PercentageInput.text = percent.ToString();

        _updating = false;
    }

    private int PointCount
    {
        get { return Line.Points != null ? Line.Points.Length : 0; }
    }

    private void UpdateUi()
    {
        _updating = true;

        RemovePointButton.interactable = PointCount > 0;
        UpdatePointUis();

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

    private void OnSelectStyleClicked()
    {
        var items = Enum.GetNames(typeof(mapData.LineStyle))
            .OrderBy(t => t)
            .Select(t => new DialogList.Item { Name = t, Id = t });

        DialogManager.Instance.ShowList("SELECT LINE STYLE",
            string.Format("Please select the style for line '{0}':", Line.Name),
            items,
            Line.Style.ToString(),
            item =>
            {
                var style = mapData.LineStyleForName(item.Id);
                _line.Style = style;
                serverUtils.PostSetMapLine(_line);
                Style.text = _line.Style.ToString();
            });
    }

    private void OnSelectPointStyleClicked()
    {
        var items = Enum.GetNames(typeof(mapData.PointStyle))
            .OrderBy(t => t)
            .Select(t => new DialogList.Item { Name = t, Id = t });

        DialogManager.Instance.ShowList("SELECT POINT STYLE",
            string.Format("Please select the point style for line '{0}':", Line.Name),
            items,
            Line.PointStyle.ToString(),
            item =>
            {
                var style = mapData.PointStyleForName(item.Id);
                _line.PointStyle = style;
                serverUtils.PostSetMapLine(_line);
                PointStyle.text = _line.PointStyle.ToString();
            });
    }

    private void OnWidthSliderChanged(float value)
    {
        if (_updating)
            return;

        _line.Width = value;
        serverUtils.PostSetMapLine(_line);
        WidthInput.text = value.ToString();
    }

    private void OnWidthInputEndEdit(string value)
    {
        if (_updating)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        _line.Width = result;
        serverUtils.PostSetMapLine(_line);
        WidthSlider.maxValue = Mathf.Max(WidthSlider.maxValue, result);
        WidthSlider.value = result;
    }

    private void OnPointCountInputEndEdit(string value)
    {
        int count;
        if (!int.TryParse(value, out count))
            return;

        SetPointCount(count);
    }

    private void SetPointCount(int count)
    {
        if (_updating)
            return;

        count = Mathf.Clamp(count, 0, mapData.MaxLinePointCount);
        if (_line.Points == null)
            _line.Points = new Vector3[count];
        else
            Array.Resize(ref _line.Points, count);

        // Set up the new point's position.
        var p = _line.Points;
        var n = _line.Points.Length;
        var last = n - 1;
        if (n > 2)
            p[last] = p[last - 1] + (p[last - 1] - p[last - 2]);

        serverUtils.PostSetMapLine(_line);

        _updating = true;
        PointCountInput.text = count.ToString();
        _updating = false;
    }

    private void OnAddPointClicked()
        { SetPointCount(PointCount + 1); }

    private void OnRemovePointClicked()
        { SetPointCount(PointCount - 1); }

    private void OnPercentageSliderChanged(float value)
    {
        if (_updating)
            return;

        serverUtils.PostSetMapLinePercent(_line.Id, value);
        PercentageInput.text = value.ToString();
    }

    private void OnPercentageInputEndEdit(string value)
    {
        if (_updating)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        serverUtils.PostSetMapLinePercent(_line.Id, result);
        PercentageSlider.maxValue = Mathf.Max(PercentageSlider.maxValue, result);
        PercentageSlider.value = result;
    }


    // Private Methods
    // ------------------------------------------------------------

    private void UpdatePointUis()
    {
        var points = _line.Points;
        var n = points != null ? points.Length : 0;
        var index = 0;
        for (var i = 0; i < n; i++)
        {
            var ui = GetPointUi(index++);
            ui.LineUi = this;
            ui.Index = i;
        }

        for (var i = 0; i < _points.Count; i++)
            _points[i].gameObject.SetActive(i < index);
    }

    private debugMapLinePointUi GetPointUi(int i)
    {
        if (i >= _points.Count)
        {
            var ui = Instantiate(PointUiPrefab);
            ui.transform.SetParent(PointContainer, false);
            _points.Add(ui);
        }

        return _points[i];
    }

}