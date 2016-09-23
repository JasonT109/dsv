using System;
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using DG.Tweening;
using Meg.Networking;
using TouchScript.Gestures;
using UnityEngine.UI;

public class widgetPopup : MonoBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** Popup in animation duration. */
    protected const float AnimateInDuration = 0.25f;

    /** Popup out animation duration. */
    protected const float AnimateOutDuration = 0.25f;

    /** Z-Sorting bias to use when in the DCC. */
    protected const float DccZSortBias = -18f;


    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Dialog's root panel. */
    public CanvasGroup Root;

    /** Popup area box. */
    public CanvasGroup Area;

    /** Title text. */
    public Text Title;

    /** Message text. */
    public Text Message;

    /** Popup icon graphics. */
    public Image[] Icons;

    /** Popup icon sprites. */
    public Sprite[] IconSprites;



    // Protected Properties
    // ------------------------------------------------------------

    /** Popup configuration. */
    protected popupData.Popup Popup { get; private set; }

    /** Popup shared data. */
    protected popupData PopupData { get { return serverUtils.PopupData; } }

    /** Determines if the popup is hosted in the DCC scene. */
    protected bool IsInDcc { get { return NetworkManagerCustom.IsInDcc; } }

    /** Whether popup should be on top of other scene elements. */
    protected bool Topmost { get { return _topmost; } set { SetTopmost(value); } }


    // Members
    // ------------------------------------------------------------

    /** Popup target id. */
    private string _targetId;

    /** Optional station id restriction. */
    private int _stationId = -1;

    /** Popup's target. */
    private PopupTarget _target;

    /** Whether popup is closed. */
    private bool _closed;

    /** Whether popup appears on top of everything. */
    private bool _topmost = true;

    /** Z-sorting bias (used in DCC). */
    private float _zBias = -1.9f;

    /** Transform gesture, if any. */
    private TransformGesture _gesture;

    /** Whether user has moved this popup. */
    private bool _moved;

    /** Popup's root canvas. */
    private Canvas _canvas;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    protected virtual void Start()
    {
        _canvas = GetComponent<Canvas>();

        if (Root)
            _gesture = Root.GetComponent<TransformGesture>();

        if (_gesture)
            RegisterWithGesture();
    }

    /** Updating. */
    protected virtual void LateUpdate()
    {
        // Update popup's visibility and positioning.
        UpdatePopup();
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Show this popup. */
    public virtual void Show(popupData.Popup popup)
    {
        Popup = popup;

        // Parse popup target info.
        _stationId = ParseStationIdFromTarget(popup.Target);
        _targetId = ParseTargetId(popup.Target);

        // Configure the popup.
        Configure();

        // Apply color theming.
        var themed = GetComponentsInChildren<PopupColorThemed>();
        foreach (var t in themed)
            t.UpdateColor(popup);

        // Configure popup title.
        if (Title)
            Title.text = serverUtils.Expanded(popup.Title);

        // Configure popup message.
        if (Message)
            Message.text = serverUtils.Expanded(popup.Message);

        // Configure icons.
        var iconIndex = (int) popup.Icon;
        var iconSprite = (IconSprites.Length > iconIndex) ? IconSprites[iconIndex] : null;
        foreach (var icon in Icons)
        {
            icon.sprite = iconSprite;
            icon.gameObject.SetActive(iconSprite != null);
        }

        // Place popup in the UI heirarchy and give it an initial update.
        transform.SetParent(Camera.main.transform, false);
        UpdatePopup();

        // Animate the popup into view.
        AnimateIn();
    }

    /** Hide this popup. */
    public void Hide()
    {
        if (gameObject.activeSelf)
            StartCoroutine(CloseRoutine());
        else
            Destroy(gameObject);
    }

    /** Request that this popup be removed from the scene. */
    public void Remove()
        { serverUtils.PostRemovePopup(Popup); }


    // Protected Methods
    // ------------------------------------------------------------

    /** Configure the popup. */
    protected virtual void Configure()
    {
        // If we're in the DCC screens, need to set default sorting order.
        if (IsInDcc)
            ConfigureForDcc();
    }

    /** Animate the popup into place. */
    protected virtual void AnimateIn()
    {
        var duration = AnimateInDuration;

        Root.transform.DOScale(Vector3.zero, duration).From();
        Root.DOFade(0, duration).From();
        Area.DOFade(0, duration).From();
    }

    /** Animate the popup away. */
    protected virtual void AnimateOut()
    {
        var duration = AnimateOutDuration;

        Root.DOKill();
        Root.transform.DOScale(Vector3.zero, duration);
        Root.DOFade(0, duration);

        Area.DOKill();
        Area.DOFade(0, duration);
    }

    /** Update popup's visibility and position. */
    protected virtual void UpdatePopup()
    {
        // Try to locate popup target (if any)
        if (!_target && !string.IsNullOrEmpty(_targetId))
            PopupData.TryGetTarget(_targetId, out _target);

        // Update the popup's position.
        UpdatePosition();

        // Update the popup's visibility.
        SetActive(ShouldBeActive());

        // Configure popup title.
        if (Title)
            Title.text = serverUtils.Expanded(Popup.Title);

        // Configure popup message.
        if (Message)
            Message.text = serverUtils.Expanded(Popup.Message);
    }

    /** Convert the raw popup target string into a target id. */
    protected virtual string ParseTargetId(string target)
    {
        if (string.IsNullOrEmpty(target))
            return target;

        // Strip out station id from target.
        return Regex.Replace(target, @"\s*Station\s*[A-Z0-9]\s*", "");
    }

    /** Determine if popup should be active. */
    protected virtual bool ShouldBeActive()
    {
        // Optionally check that popup is on the right station.
        if (_stationId >= 0 && DCCScreenData.StationId != _stationId)
            return false;

        // Check if we're in the debug screen.
        // Don't show global popups there.
        if (string.IsNullOrEmpty(_targetId))
            return !serverUtils.IsInDebugScreen();

        // Otherwise, check if target is active.
        return _target && _target.gameObject.activeInHierarchy;
    }

    /** Set whether the popup is active. */
    protected virtual void SetActive(bool value)
    {
        Root.gameObject.SetActive(value);
        Area.gameObject.SetActive(value && Popup.Size.sqrMagnitude > 0);
    }

    /** Configure popup for DCC mode. */
    protected virtual void ConfigureForDcc()
    {
        // Set a z-sorting bias that works with the DCC window setup.
        _zBias = DccZSortBias;

        // In DCC, sort certain popups in with other elements.
        switch (Popup.Type)
        {
            case popupData.Type.Info:
            case popupData.Type.Warning:
            case popupData.Type.GreenScreen:
                Topmost = false;
                break;
        }
    }

    /** Set up popup for Z-sorting. */
    protected virtual void SetTopmost(bool value)
    {
        _topmost = value;

        var canvas = GetComponent<Canvas>();
        canvas.sortingOrder = value ? 100 : 0;
        canvas.sortingLayerName = value ? "Top" : "Default";
    }

    /** Update the popup's position. */
    protected virtual void UpdatePosition()
    {
        // Update sorting order for topmost popups.
        if (_canvas && _topmost)
            _canvas.sortingOrder = 100 + transform.GetSiblingIndex();

        // Only update if user hasn't moved the popup.
        if (_moved)
            return;

        // Rescale the popup.
        if (Popup.Scale.sqrMagnitude > 0)
            Root.transform.localScale = Popup.Scale;

        // Position the popup.
        if (_target)
            PositionOverTarget(_target);
        else
            SetPosition(Popup.Position);
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Look for a station ID in the popup's target. */
    private int ParseStationIdFromTarget(string target)
    {
        if (string.IsNullOrEmpty(target))
            return -1;

        var match = Regex.Match(target, @"Station\s*([0-9])");
        if (match.Success)
            return int.Parse(match.Groups[1].Value);

        match = Regex.Match(target, @"Station\s*([A-Z])");
        if (match.Success)
        {
            var id = match.Groups[1].Value[0];
            return (id - 'A');
        }

        return -1;
    }

    /** Position this popup over the target. */
    private void PositionOverTarget(PopupTarget target)
    {
        var bounds = target.Bounds;
        var center = bounds.center;
        var offset = Popup.Position;
        center.z += (_zBias + Popup.Position.z);
        var p = transform.InverseTransformPoint(center) + new Vector3(offset.x, offset.y);
        SetPosition(p);
    }

    /** Set the popup's current position. */
    private void SetPosition(Vector3 p)
    {
        p = new Vector3(p.x, p.y, Topmost ? 0 : p.z);
        Root.transform.localPosition = p;
        Area.transform.localPosition = p;
    }

    /** Routine to close the dialog. */
    private IEnumerator CloseRoutine()
    {
        // Guard against multiple close attempts.
        if (_closed)
            yield break;

        _closed = true;

        // Disable popup interactions.
        Root.interactable = false;
        Area.interactable = false;

        // Animate the popup away.
        AnimateOut();

        // Kill dialog after a short delay.
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }

    /** Register for transform gesture events. */
    private void RegisterWithGesture()
        { _gesture.TransformStarted += OnTransformStarted; }

    /** Handle user moving the popup. */
    private void OnTransformStarted(object sender, EventArgs eventArgs)
    {
        _moved = true;

        // Bump popup to the top of the render order.
        transform.SetAsLastSibling();
    }

}
