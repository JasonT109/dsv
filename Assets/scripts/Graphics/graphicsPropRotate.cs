using UnityEngine;
using System.Collections;

public class graphicsPropRotate : MonoBehaviour {

    public GameObject gauge;
    public float multiplier = 30.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        var gScript = gauge.GetComponent<digital_gauge>();
        gameObject.transform.Rotate(0, 0, -(gScript.value * Time.deltaTime)* multiplier);
	}
}
