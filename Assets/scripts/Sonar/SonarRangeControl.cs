using UnityEngine;
using System.Collections;

public class SonarRangeControl : MonoBehaviour
{

    public int Range = 30;
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

	// Use this for initialization
	void Start () 
    {
        GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
        Range = Root.GetComponent<SonarData>().SonarRange;

        UpdateValues();
	}
	
	// Update is called once per frame
	void Update () 
    {   
        if(LeftButton.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            if(canChangeValue)
            {
                Range = Mathf.Clamp(Range - RangeIncrement, MinRange, MaxRange);
                UpdateValues();
                canChangeValue = false;
                StartCoroutine(Wait(0.2f));
            }
        }
        else if(RightButton.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            if(canChangeValue)
            {
                Range = Mathf.Clamp(Range + RangeIncrement, MinRange, MaxRange);
                UpdateValues();
                canChangeValue = false;
                StartCoroutine(Wait(0.2f));
            }
        }
	}

    void UpdateValues()
    {
        OneThirdL.Text = "" + Range/3;
        TwoThirdsL.Text = "" + (Range/3) * 2;
        FullRangeL.Text = "" + Range;

        OneThirdR.Text = "" + Range/3;
        TwoThirdsR.Text = "" + (Range/3) * 2;
        FullRangeR.Text = "" + Range;

        if (RangeValObj)
            RangeValObj.Text = "" + Range;

        GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
        Root.GetComponent<SonarData>().SonarRange = Range;

    }

    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canChangeValue = true;
    }
}
