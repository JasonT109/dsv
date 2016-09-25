using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ROVPasscode : MonoBehaviour
{

    public GameObject Key1;
    public GameObject Key2;
    public GameObject Key3;
    public GameObject Key4;

    public GameObject LiveButton;

    int iKeysEntered = 0;

    // Use this for initialization
    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(iKeysEntered == 0)
        {
            Key1.SetActive(false);
            Key2.SetActive(false);
            Key3.SetActive(false);
            Key4.SetActive(false);
        }
	}

    public void Reset()
    {

    }

    public void ButtonPress()
    {
        iKeysEntered++;

        if(iKeysEntered == 4)
        {

        }

        if(iKeysEntered > 4)
        {
            iKeysEntered = 4;
        }

        if(iKeysEntered == 1)
        {
            Key1.GetComponent<Image>().color = new Color(1f, 225f / 255f, 0f);
            Key2.GetComponent<Image>().color = new Color(1f, 225f / 255f, 0f);
            Key3.GetComponent<Image>().color = new Color(1f, 225f / 255f, 0f);
            Key4.GetComponent<Image>().color = new Color(1f, 225f / 255f, 0f);

            Key1.SetActive(true);
            Key2.SetActive(false);
            Key3.SetActive(false);
            Key4.SetActive(false);

        }

        if (iKeysEntered == 2)
        {
            Key1.GetComponent<Image>().color = new Color(1f, 225f / 255f, 0f);
            Key2.GetComponent<Image>().color = new Color(1f, 225f / 255f, 0f);
            Key3.GetComponent<Image>().color = new Color(1f, 225f / 255f, 0f);
            Key4.GetComponent<Image>().color = new Color(1f, 225f / 255f, 0f);

            Key1.SetActive(true);
            Key2.SetActive(true);
            Key3.SetActive(false);
            Key4.SetActive(false);
        }

        if (iKeysEntered == 3)
        {
            Key1.GetComponent<Image>().color = new Color(1f, 225f / 255f, 0f);
            Key2.GetComponent<Image>().color = new Color(1f, 225f / 255f, 0f);
            Key3.GetComponent<Image>().color = new Color(1f, 225f / 255f, 0f);
            Key4.GetComponent<Image>().color = new Color(1f, 225f / 255f, 0f);

            Key1.SetActive(true);
            Key2.SetActive(true);
            Key3.SetActive(true);
            Key4.SetActive(false);
        }

        if (iKeysEntered == 4)
        {
            Key1.GetComponent<Image>().color = new Color(0.9f, 0f, 0f);
            Key2.GetComponent<Image>().color = new Color(0.9f, 0f, 0f);
            Key3.GetComponent<Image>().color = new Color(0.9f, 0f, 0f);
            Key4.GetComponent<Image>().color = new Color(0.9f, 0f, 0f);

            Key1.SetActive(true);
            Key2.SetActive(true);
            Key3.SetActive(true);
            Key4.SetActive(true);
        }
    }


}
