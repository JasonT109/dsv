using UnityEngine;
using System.Collections;
using Meg.Networking;

public class SonarShortRangeDisplay : MonoBehaviour
{
    public widgetText OneThirdL;
    public widgetText TwoThirdsL;
    public widgetText FullRangeL;
           
    public widgetText OneThirdR;
    public widgetText TwoThirdsR;
    public widgetText FullRangeR;

    public widgetText RangeValObj;

	private void Update()
	{
	    var range = serverUtils.SonarData.ShortRange;
        var r = Mathf.RoundToInt(range / 5) * 5;

        OneThirdL.Text = "" + r/3;
        TwoThirdsL.Text = "" + (r/3) * 2;
        FullRangeL.Text = "" + r;
        OneThirdR.Text = "" + r/3;
        TwoThirdsR.Text = "" + (r/3) * 2;
        FullRangeR.Text = "" + r;

        if (RangeValObj)
            RangeValObj.Text = "" + r;
    }

}
