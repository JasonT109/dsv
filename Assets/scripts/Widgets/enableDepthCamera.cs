using UnityEngine;
using System.Collections;

public class enableDepthCamera : MonoBehaviour {

    public GameObject dCam;
    public GameObject dButton;

	// Update is called once per frame
	void Update () {
	
        if (dButton.GetComponent<buttonControl>().active)
        {
            dCam.SetActive(true);
        }
        else
        {
            dCam.SetActive(false);
        }
	}
}
