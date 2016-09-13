using UnityEngine;
using DG.Tweening;


public class widgetPopupWarning : widgetPopupInfo
{

    /** Animate the popup into place. */
    protected override void AnimateIn()
    {
        base.AnimateIn();

        var rootSequence = DOTween.Sequence();
        rootSequence.Append(Root.transform.DOPunchScale(Vector3.one * 0.05f, 0.25f).SetDelay(1));
        rootSequence.SetLoops(-1, LoopType.Restart);
    }

}
