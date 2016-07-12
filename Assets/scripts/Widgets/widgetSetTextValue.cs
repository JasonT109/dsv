using UnityEngine;
using System.Collections;

public class widgetSetTextValue : MonoBehaviour {

    public GameObject increaseValue;
    public GameObject decreaseValue;
    public bool numbers;
    public int currentNumber = 1;
    public bool wrapNumbers;
    public int maxValue = 9;
    public bool strings;
    public string[] stringValues;
    public string currentString = "Arsenal";
    private int currentStringNum = 0;
    private bool canPress = true;
    public GameObject optionalLoaderObject;

    IEnumerator wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canPress = true;
        if (optionalLoaderObject)
        {
            optionalLoaderObject.GetComponent<megGetSceneDataFiles>().getFiles();
        }
    }

	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (numbers)
        {
            gameObject.GetComponent<TextMesh>().text = currentNumber.ToString("00");

        }

        if (strings)
        {
            currentString = stringValues[currentStringNum];
            gameObject.GetComponent<TextMesh>().text = currentString;
        }

        if (increaseValue.GetComponent<buttonControl>().pressed && canPress)
        {
            canPress = false;
            StartCoroutine(wait(0.25f));
            if (numbers)
            {
                currentNumber += 1;
                if (wrapNumbers)
                {
                    if (currentNumber > maxValue)
                    {
                        currentNumber = 0;
                    }
                }
            }
            else
            {
                currentStringNum += 1;
                if (currentStringNum >= stringValues.Length)
                {
                    currentStringNum = 0;
                }
            }
        }

        if (decreaseValue.GetComponent<buttonControl>().pressed && canPress)
        {
            canPress = false;
            StartCoroutine(wait(0.25f));
            if (numbers)
            {
                currentNumber -= 1;
                if (wrapNumbers)
                {
                    if (currentNumber < 0)
                    {
                        currentNumber = maxValue;
                    }
                }
            }
            else
            {
                currentStringNum -= 1;
                if (currentStringNum < 0)
                {
                    currentStringNum = stringValues.Length - 1;
                }
            }
        }
    }
}
