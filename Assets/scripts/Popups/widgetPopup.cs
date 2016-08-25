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

    /** Backdrop graphic. */
    // public Graphic Backdrop;

    /** Title text. */
    public Text Title;
    
    /** Popup configuration. */
    public popupData.Popup Popup { get; private set; }

    /** Popup's target. */
    public PopupTarget Target { get { return _target; } }

    /** Popup shared data. */
    public popupData PopupData { get { return serverUtils.PopupData; } }

    /** Popup icon graphics. */
    public Image[] Icons;

    /** Popup icon sprites. */
    public Sprite[] IconSprites;


    // Members
    // ------------------------------------------------------------

    /** Whether dialog has been closed. */
    private bool _closed;

    /** Popup's target. */
    private PopupTarget _target;


    // Unity Methods
    // ------------------------------------------------------------

    /** Updating. */
    private void Update()
    {
        // Update the popup's visibility.
        if (_target)
            Root.gameObject.SetActive(_target.gameObject.activeInHierarchy);
        else
            Root.gameObject.SetActive(!serverUtils.IsInDebugScreen());
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

        // Position popup over the target, or fall back to center of screen.
        transform.SetParent(Camera.main.transform, false);
        if (PopupData.TryGetTarget(popup.Target, out _target))
            PositionOverTarget(_target);
        else
            Root.transform.localPosition = Popup.Position;

        // Transition the popup into view..
        Root.transform.DOScale(Vector3.zero, 0.25f).From();
        Root.DOFade(0, 0.25f).From();
        // Backdrop.DOFade(0, 0.25f).From();
    }

    /** Hide this popup. */
    public void Hide()
        { StartCoroutine(CloseRoutine()); }


    // Private Methods
    // ------------------------------------------------------------
    
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
        // Backdrop.DOFade(0, 0.25f);

        // Kill dialog after a short delay.
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }


}
