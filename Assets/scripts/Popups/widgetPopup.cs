using UnityEngine;
using System.Collections;
using DG.Tweening;
using Meg.Networking;
using UnityEngine.UI;

public class widgetPopup : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Dialog's root panel. */
    public CanvasGroup Root;

    /** Title text. */
    public Text Title;
    
    /** Popup configuration. */
    public popupData.Popup Popup { get; private set; }

    /** Popup icon graphics. */
    public Image[] Icons;

    /** Popup icon sprites. */
    public Sprite[] IconSprites;


    // Private Properties
    // ------------------------------------------------------------

    /** Popup shared data. */
    private popupData PopupData { get { return serverUtils.PopupData; } }


    // Members
    // ------------------------------------------------------------

    /** Whether dialog has been closed. */
    private bool _closed;

    /** Popup's target. */
    private PopupTarget _target;


    // Unity Methods
    // ------------------------------------------------------------

    /** Updating. */
    private void LateUpdate()
    {
        // Update popup's visibility and positioning.
        UpdatePopup();
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Show this popup. */
    public void Show(popupData.Popup popup)
    {
        Popup = popup;

        // Configure popup.
        Title.text = popup.Title;
        var titleSequence = DOTween.Sequence();
        titleSequence.Append(Title.transform.DOPunchScale(Vector3.one * 0.05f, 0.25f).SetDelay(3));
        titleSequence.SetLoops(-1, LoopType.Restart);

        // Configure icons.
        var iconSprite = IconSprites[(int) popup.Icon];
        foreach (var icon in Icons)
        {
            icon.sprite = iconSprite;
            icon.DOFade(0, 0.25f).From().SetLoops(-1, LoopType.Yoyo);
        }

        // Place popup in the UI heirarchy and give it an initial update.
        transform.SetParent(Camera.main.transform, false);
        UpdatePopup();

        // Transition the popup into view..
        Root.transform.DOScale(Vector3.zero, 0.25f).From();
        Root.DOFade(0, 0.25f).From();
    }

    /** Hide this popup. */
    public void Hide()
        { StartCoroutine(CloseRoutine()); }


    // Private Methods
    // ------------------------------------------------------------

    /** Update popup's visibility and position. */
    private void UpdatePopup()
    {
        // Try to locate popup target (if any)
        if (!_target && !string.IsNullOrEmpty(Popup.Target))
            PopupData.TryGetTarget(Popup.Target, out _target);

        // Update the popup's position.
        if (_target)
            PositionOverTarget(_target);
        else
            Root.transform.localPosition = Popup.Position;

        // Update the popup's visibility.
        if (!string.IsNullOrEmpty(Popup.Target))
            Root.gameObject.SetActive(_target && _target.gameObject.activeInHierarchy);
        else
            Root.gameObject.SetActive(!serverUtils.IsInDebugScreen());
    }

    /** Position this popup over the target. */
    private void PositionOverTarget(PopupTarget target)
    {
        // Get the target's worldspace bounds, and locate center in popup's local space.
        var bounds = target.Bounds;
        var p = transform.InverseTransformPoint(bounds.center) + Popup.Position;

        // Place popup at the center of the target, and add on position offset.
        Root.transform.localPosition = new Vector3(p.x, p.y, 0);
    }

    /** Routine to close the dialog. */
    private IEnumerator CloseRoutine()
    {
        // Guard against multiple close attempts.
        if (_closed)
            yield break;

        _closed = true;

        // Zoom out the dialog.
        Root.interactable = false;
        Root.transform.DOScale(Vector3.zero, 0.25f);
        Root.DOFade(0, 0.25f);

        // Kill dialog after a short delay.
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }


}
