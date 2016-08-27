using UnityEngine;
using DG.Tweening;

public class widgetPopupInfo : widgetPopup
{

    // Protected Methods
    // ------------------------------------------------------------

    /** Animate the popup into place. */
    protected override void AnimateIn()
    {
        // Determine if we should have a looping animation.
        var duration = AnimateInDuration;
        var looping = Popup.Icon != popupData.Icon.None;
        if (looping)
        {
            var titleSequence = DOTween.Sequence();
            titleSequence.Append(Title.transform.DOPunchScale(Vector3.one*0.05f, duration).SetDelay(3));
            titleSequence.SetLoops(-1, LoopType.Restart);

            foreach (var icon in Icons)
                icon.DOFade(0, duration).From().SetLoops(-1, LoopType.Yoyo);

            Area.DOFade(0, 1.0f).From().SetLoops(-1, LoopType.Yoyo);
        }

        // Animate the popup root panel.
        Root.transform.DOScale(Vector3.zero, duration).From();
        Root.DOFade(0, duration).From();
    }

}
