using UnityEngine;
using System.Collections;

public class SonarRangeControl : MonoBehaviour 
{

    public TextMesh OneThirdL;
    public TextMesh TwoThirdsL;
    public TextMesh FullRangeL;
           
    public TextMesh OneThirdR;
    public TextMesh TwoThirdsR;
    public TextMesh FullRangeR;

    public GameObject LeftButton;
    public GameObject RightButton;

    public TextMesh RangeValObj;

    public int Range = 30;

    bool canChangeValue = true;

	// Use this for initialization
	void Start () 
    {
        UpdateValues();
	}
	
	// Update is called once per frame
	void Update () 
    {   
        if(LeftButton.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            if(canChangeValue)
            {
                Range -= 15;
                if(Range < 15)
                {
                    Range = 15;
                }
                UpdateValues();
                canChangeValue = false;
                StartCoroutine(Wait(0.2f));
            }
        }
        else if(RightButton.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            if(canChangeValue)
            {
                Range += 15;
               
                UpdateValues();
                canChangeValue = false;
                StartCoroutine(Wait(0.2f));
            }
        }
	}

    void UpdateValues()
    {
        OneThirdL.text = "" + Range/3;
        TwoThirdsL.text = "" + (Range/3) * 2;
        FullRangeL.text = "" + Range;

        OneThirdR.text = "" + Range/3;
        TwoThirdsR.text = "" + (Range/3) * 2;
        FullRangeR.text = "" + Range;

        RangeValObj.text = "" + Range;
    }

    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canChangeValue = true;
    }
}
