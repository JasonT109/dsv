using UnityEngine;
using System.Collections;

public class DCCBlinkerWarningColor : MonoBehaviour
{
    public graphicsAnimateWarningColor[] Blinkers;
    public Color WarningColor;

    public buttonControl[] IgnoreList;

    private bool _WarningDetected = false;
    private Color[] _InitialColors;

    void SetWarningColor(graphicsAnimateWarningColor Blinker, Color NewColor)
    {
        Blinker.warningColor = NewColor;
    }

    bool Ignored(buttonControl Button)
    {
        bool _Ignored = false;

        for (int i = 0; i < IgnoreList.Length; i++)
        {
            if (Button == IgnoreList[i])
                _Ignored = true;
        }

        return _Ignored;
    }

	void Start ()
    {
        _InitialColors = new Color[Blinkers.Length];

        for (int i = 0; i < Blinkers.Length; i++)
            _InitialColors[i] = Blinkers[i].warningColor;
	}

	void Update ()
    {
        _WarningDetected = false;

        foreach (buttonControl c in GetComponentsInChildren<buttonControl>())
        {
            if (c.warning && !Ignored(c))
                _WarningDetected = true;
        }

        if (_WarningDetected)
        {
            for (int i = 0; i < Blinkers.Length; i++)
                SetWarningColor(Blinkers[i], WarningColor);
        }
        else
        {
            for (int i = 0; i < Blinkers.Length; i++)
                SetWarningColor(Blinkers[i], _InitialColors[i]);
        }
    }
}
