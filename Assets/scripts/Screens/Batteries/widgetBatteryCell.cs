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

        var status = transform.Find("Status");
        StatusText = status ? status.GetComponent<textValueFromServer>() : null;

        var threshold = Random.Range(10, 200);
        SetErrorThreshold(threshold);
    }

    private void SetErrorThreshold(float value)
    {
        WarningBars.threshold = value;
        Button.autoWarningValue = value;

        if (StatusText)
        {
            StatusText.Ranges[0].Range.y = value;
            StatusText.Ranges[1].Range.x = value;
        }
    }

}
