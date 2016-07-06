using UnityEngine;
using System.Collections;


public class DecklinkModeSetter : MonoBehaviour 
{
    public GameObject LeftButton;
    public GameObject RightButton;
    public TextMesh Number;
    public TextMesh ModeDef;

    ModeTranslator Translator;

    public GameObject cameraManager;

    bool canChangeValue = true;

    int iModeVal = 0;

	// Use this for initialization
	void Start () 
    {
        Translator = GetComponent<ModeTranslator>();
        ModeDef.text = Translator.TranslateFromIndexSmall(iModeVal);
        Number.text = "" + (iModeVal+1);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(LeftButton.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            if(canChangeValue)
            {
                iModeVal--;
                if(iModeVal < 0)
                {
                    iModeVal = 0;
                }
                Number.text = "" + (iModeVal+1);
                canChangeValue = false;
                ModeDef.text = Translator.TranslateFromIndexSmall(iModeVal);
                cameraManager.GetComponent<CustomLiveFeedManager>().SetMode(iModeVal);
                StartCoroutine(Wait(0.2f));
            }
        }
        else if(RightButton.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            if(canChangeValue)
            {
                iModeVal++;
                if(iModeVal > 33)
                {
                    iModeVal = 33;
                }
                Number.text = "" + (iModeVal+1);
                canChangeValue = false;
                ModeDef.text = Translator.TranslateFromIndexSmall(iModeVal);
                cameraManager.GetComponent<CustomLiveFeedManager>().SetMode(iModeVal);
                StartCoroutine(Wait(0.2f));
            }
        }
        //else
        //{
        //    ClickHeld = false;
        //}
	}

    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canChangeValue = true;
    }
}
