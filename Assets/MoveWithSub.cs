using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Meg.Networking;

public class MoveWithSub : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, serverUtils.SubControl.transform.eulerAngles.y); ;
    }
}
