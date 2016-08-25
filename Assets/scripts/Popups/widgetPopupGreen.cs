using UnityEngine;
using System.Collections;
using DG.Tweening;
using Meg.Networking;
using UnityEngine.UI;

public class widgetPopupGreen : widgetPopup
{

    /** Update popup's visibility and position. */
    protected override void UpdatePopup()
    {
        base.UpdatePopup();

        // Update greenscreen brightness level.
        var level = serverUtils.GetServerData("greenScreenBrightness", 1.0f);
        Area.GetComponent<Image>().color = new Color(0, level, 0, 1);
    }

}
