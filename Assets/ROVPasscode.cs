using UnityEngine;
using System.Collections;

public class ROVPasscode : MonoBehaviour
{

    public GameObject Key1;
    public GameObject Key2;
    public GameObject Key3;
    public GameObject Key4;

    int iKeysEntered = 0;

    // Use this for initialization
    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
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

        if(iKeysEntered >4)
        {
            iKeysEntered = 4;
        }
    }


}
