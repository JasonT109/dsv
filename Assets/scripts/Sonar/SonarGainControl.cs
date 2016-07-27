using UnityEngine;
using System.Collections;

public class SonarGainControl : MonoBehaviour
{

    public float Gain = 75;
    public float GainIncrement = 5;

    public float MinGain = 50;
    public float MaxGain = 150;

    public GameObject UpButton;
    public GameObject DownButton;

    public widgetText GainText;
    public string GainFormat = "{0:N0}%";

    bool canChangeValue = true;

    public const float SmoothTime = 0.1f;
    public const float PressInterval = 0.25f;

    private float _target;
    private float _velocity;

    private bool _initialized;

    void Update()
    {
        if (!_initialized)
            InitValues();

        if (!_initialized)
            return;

        if (DownButton.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            if (canChangeValue)
            {
                _target = Mathf.Clamp(_target - GainIncrement, MinGain, MaxGain);
                canChangeValue = false;
                StartCoroutine(Wait(PressInterval));
            }
        }
        else if (UpButton.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            if (canChangeValue)
            {
                _target = Mathf.Clamp(_target + GainIncrement, MinGain, MaxGain);
                canChangeValue = false;
                StartCoroutine(Wait(PressInterval));
            }
        }

        UpdateValues();
    }

    void InitValues()
    {
        _target = Gain;
        _initialized = true;
    }

    void UpdateValues()
    {
        Gain = Mathf.SmoothDamp(Gain, _target, ref _velocity, SmoothTime);

        if (GainText)
            GainText.Text = string.Format(GainFormat, Gain);

        GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
        Root.GetComponent<SonarData>().SonarGain = Gain;
    }

    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canChangeValue = true;
    }

}
