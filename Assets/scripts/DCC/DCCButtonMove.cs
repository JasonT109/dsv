using UnityEngine;
using System.Collections;

public class DCCButtonMove : MonoBehaviour
{
    public GameObject PlayerButton;
    public GameObject TargetButton;
	

    public void Start()
        {
        PlayerButton.transform.position = TargetButton.transform.position;
        }


	// Update is called once per frame
	void Update ()
    {
        PlayerButton.transform.position = Vector3.Lerp(PlayerButton.transform.position, TargetButton.transform.position, Time.time);
	}
}
