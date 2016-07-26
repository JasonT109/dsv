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

    void Start ()
    {
        GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
        Gain = Root.GetComponent<SonarData>().SonarGain;

        UpdateValues();
    }

    void Update()
    {
        if (DownButton.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            if (canChangeValue)
            {
                Gain = Mathf.Clamp(Gain - GainIncrement, MinGain, MaxGain);
                UpdateValues();
                canChangeValue = false;
                StartCoroutine(Wait(0.2f));
            }
        }
        else if (UpButton.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            if (canChangeValue)
            {
                Gain = Mathf.Clamp(Gain + GainIncrement, MinGain, MaxGain);
                UpdateValues();
                canChangeValue = false;
                StartCoroutine(Wait(0.2f));
            }
        }

        if (GainText)
            GainText.Text = string.Format(GainFormat, Gain);
    }

    void UpdateValues()
    {
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
