using UnityEngine;
using System.Collections;
using DG.Tweening;
using Meg.Networking;
using UnityEngine.UI;

public class widgetPopupWarning : widgetPopup
{

    // Protected Methods
    // ------------------------------------------------------------

    /** Animate the popup into place. */
    protected override void AnimateIn()
    {
        var duration = AnimateInDuration;

        // Animate title text.
        var titleSequence = DOTween.Sequence();
        titleSequence.Append(Title.transform.DOPunchScale(Vector3.one * 0.05f, duration).SetDelay(3));
        titleSequence.SetLoops(-1, LoopType.Restart);

        // Animate icons.
        foreach (var icon in Icons)
            icon.DOFade(0, duration).From().SetLoops(-1, LoopType.Yoyo);

        // Animate the popup area box.
        Area.DOFade(0, 1.0f).From().SetLoops(-1, LoopType.Yoyo);

        // Animate the popup root panel.
        Root.transform.DOScale(Vector3.zero, duration).From();
        Root.DOFade(0, duration).From();
    }

}
