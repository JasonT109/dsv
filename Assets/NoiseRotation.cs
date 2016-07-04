using UnityEngine;
using System.Collections;

public class NoiseRotation : MonoBehaviour 
{

    public GameObject serverData;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector3 constrained = new Vector3(transform.localRotation.eulerAngles.x, serverData.transform.rotation.eulerAngles.y, transform.localRotation.eulerAngles.x);
        transform.localRotation = Quaternion.Euler(constrained);
	}
}
