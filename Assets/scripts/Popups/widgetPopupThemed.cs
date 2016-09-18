using UnityEngine;
using DG.Tweening;


public class widgetPopupThemed : widgetPopup
{

    /** Set whether the popup is active. */
    protected override void SetActive(bool value)
    {
        Root.gameObject.SetActive(value);
        Area.gameObject.SetActive(value && Popup.Size.sqrMagnitude > 0);
    }

    /** Set whether the popup is active. */
    protected override void UpdatePopup()
    {
        base.UpdatePopup();

        if (Title)
            Title.gameObject.SetActive(Popup.HasTitle);
        if (Message)
            Message.gameObject.SetActive(Popup.HasMessage);
    }


    /** Animate the popup into place. */
    protected override void AnimateIn()
    {
        base.AnimateIn();

        // Resize and display the popup area box.
        if (Area)
            Area.GetComponent<RectTransform>().sizeDelta = Popup.Size;
    }

}
