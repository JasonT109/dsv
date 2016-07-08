using UnityEngine;
using System.Collections;

public class enableDepthCamera : MonoBehaviour {

    public GameObject dCam;
    public GameObject dButton;
    public GameObject[] disableObjects;

	// Update is called once per frame
	void Update () {
	
        if (dButton.GetComponent<buttonControl>().active)
        {
            dCam.SetActive(true);
            for (int i = 0; i < disableObjects.Length; i++)
            {
                disableObjects[i].SetActive(true);
            }
        }
        else
        {
            dCam.SetActive(false);
            for (int i = 0; i < disableObjects.Length; i++)
            {
                disableObjects[i].SetActive(false);
            }
        }
	}
}
