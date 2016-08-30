using UnityEngine;
using System.Collections;
using DG.Tweening;
using Meg.Networking;
using UnityEngine.UI;

public class widgetPopupGreen : widgetPopup
{
    private const float MaxDotSize = 1000f;

    /** Update popup's visibility and position. */
    protected override void UpdatePopup()
    {
        base.UpdatePopup();

        // Update greenscreen brightness level.
        var level = serverUtils.GetServerData("greenScreenBrightness", 1.0f);
        Area.GetComponent<Image>().color = new Color(0, level, 0, 1);
        Area.GetComponent<RectTransform>().sizeDelta = Popup.Size;

        var size = Mathf.Min(Popup.Size.x, Popup.Size.y);
        var scale = Vector3.one * Mathf.Clamp(size / MaxDotSize, 0.1f, 1f);

        foreach (var dot in Icons)
            dot.transform.localScale = scale;

    }

}
