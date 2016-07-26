using UnityEngine;
using System.Collections;

public class SonarRangeControl : MonoBehaviour
{

    public float Range = 30;
    public int RangeIncrement = 15;
    public int MinRange = 30;
    public int MaxRange = 1000;

    public widgetText OneThirdL;
    public widgetText TwoThirdsL;
    public widgetText FullRangeL;
           
    public widgetText OneThirdR;
    public widgetText TwoThirdsR;
    public widgetText FullRangeR;

    public GameObject LeftButton;
    public GameObject RightButton;

    public widgetText RangeValObj;

    bool canChangeValue = true;

    public const float SmoothTime = 0.15f;

    private float _target;
    private float _velocity;

    private bool _initialized;


	// Update is called once per frame
	void Update () 
    {
        if (!_initialized)
            InitValues();

        if (!_initialized)
            return;

        if (LeftButton.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            if(canChangeValue)
            {
                _target = Mathf.Clamp(_target - RangeIncrement, MinRange, MaxRange);
                canChangeValue = false;
                StartCoroutine(Wait(0.2f));
            }
        }
        else if(RightButton.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            if(canChangeValue)
            {
                _target = Mathf.Clamp(_target + RangeIncrement, MinRange, MaxRange);
                canChangeValue = false;
                StartCoroutine(Wait(0.2f));
            }
        }

        UpdateValues();
    }

    void InitValues()
    {
        _target = Range;
        _initialized = true;
    }

    void UpdateValues()
    {
        Range = Mathf.SmoothDamp(Range, _target, ref _velocity, SmoothTime);

        var r = Mathf.RoundToInt(Range / 5) * 5;

        OneThirdL.Text = "" + r/3;
        TwoThirdsL.Text = "" + (r/3) * 2;
        FullRangeL.Text = "" + r;

        OneThirdR.Text = "" + r/3;
        TwoThirdsR.Text = "" + (r/3) * 2;
        FullRangeR.Text = "" + r;

        if (RangeValObj)
            RangeValObj.Text = "" + r;

        GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
        Root.GetComponent<SonarData>().SonarRange = Range;

    }

    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canChangeValue = true;
    }
}
