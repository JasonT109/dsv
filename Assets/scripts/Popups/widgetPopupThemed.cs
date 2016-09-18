using UnityEngine;
using DG.Tweening;


public class widgetPopupThemed : widgetPopupInfo
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

}
