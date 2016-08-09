using UnityEngine;
using System.Collections;

public class widgetBatteryCell : MonoBehaviour
{

    public enableOnServerValue WarningBars { get; private set; }
    public buttonControl Button { get; private set; }
    public textValueFromServer StatusText { get; private set; }

    public void Awake()
    {
        WarningBars = GetComponent<enableOnServerValue>();
        Button = GetComponentInChildren<buttonControl>();
        StatusText = transform.Find("Status").GetComponent<textValueFromServer>();

        var threshold = Random.Range(10, 200);
        SetErrorThreshold(threshold);
    }

    private void SetErrorThreshold(float value)
    {
        WarningBars.threshold = value;
        Button.autoWarningValue = value;
        StatusText.Ranges[0].Range.y = value;
        StatusText.Ranges[1].Range.x = value;
    }

}
