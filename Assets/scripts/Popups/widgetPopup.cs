using UnityEngine;
using System.Collections;
using DG.Tweening;
using Meg.Networking;
using UnityEngine.UI;

public class widgetPopup : MonoBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** Popup in animation duration. */
    protected const float AnimateInDuration = 0.25f;

    /** Popup out animation duration. */
    protected const float AnimateOutDuration = 0.25f;


    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Dialog's root panel. */
    public CanvasGroup Root;

    /** Popup area box. */
    public CanvasGroup Area;

    /** Title text. */
    public Text Title;

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



    // Members
    // ------------------------------------------------------------

    /** Popup's target. */
    private PopupTarget _target;

    /** Whether popup is closed. */
    private bool _closed;


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
    public virtual void Show(popupData.Popup popup)
    {
        Popup = popup;

        // Configure popup.
        if (Title)
            Title.text = popup.Title;

        // Configure icons.
        var iconIndex = (int) popup.Icon;
        var iconSprite = (IconSprites.Length > iconIndex) ? IconSprites[iconIndex] : null;
        foreach (var icon in Icons)
        {
            icon.sprite = iconSprite;
            icon.gameObject.SetActive(iconSprite != null);
        }

        // Resize and display the popup area box.
        Area.GetComponent<RectTransform>().sizeDelta = Popup.Size;

        // Place popup in the UI heirarchy and give it an initial update.
        transform.SetParent(Camera.main.transform, false);
        UpdatePopup();

        // Animate the popup into view.
        AnimateIn();
    }

    /** Hide this popup. */
    public void Hide()
        { StartCoroutine(CloseRoutine()); }


    // Protected Methods
    // ------------------------------------------------------------

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
        if (!_target && !string.IsNullOrEmpty(Popup.Target))
            PopupData.TryGetTarget(Popup.Target, out _target);

        // Update the popup's position.
        if (_target)
            PositionOverTarget(_target);
        else
            SetPosition(Popup.Position);

        // Update the popup's visibility.
        if (!string.IsNullOrEmpty(Popup.Target))
            SetActive(_target && _target.gameObject.activeInHierarchy);
        else
            SetActive(!serverUtils.IsInDebugScreen());
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Set whether the popup is active. */
    private void SetActive(bool value)
    {
        Root.gameObject.SetActive(value);
        Area.gameObject.SetActive(value && Popup.Size.sqrMagnitude > 0);
    }

    /** Position this popup over the target. */
    private void PositionOverTarget(PopupTarget target)
    {
        // Get the target's worldspace bounds, and locate center in popup's local space.
        var bounds = target.Bounds;
        var p = transform.InverseTransformPoint(bounds.center) + Popup.Position;

        // Place popup at the center of the target, and add on position offset.
        SetPosition(p);
    }

    /** Set the popup's current position. */
    private void SetPosition(Vector3 p)
    {
        Root.transform.localPosition = new Vector3(p.x, p.y, 0);
        Area.transform.localPosition = new Vector3(p.x, p.y, 0);
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


}
