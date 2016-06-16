using UnityEngine;
using System.Collections;
using Meg.Networking;

public class graphicsRotateObject : MonoBehaviour {

    public string inputValue = "inputZaxis";
    public float multiplier = 1.0f;
    private float value = 0f;
    public int axis = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        value = serverUtils.GetServerData(inputValue) * (Time.deltaTime * multiplier);

        switch (axis)
        {
            case 0:
                gameObject.transform.Rotate(value, 0, 0);
                break;
            case 1:
                gameObject.transform.Rotate(0, value, 0);
                break;
            case 2:
                gameObject.transform.Rotate(0, 0, value);
                break;
        }

        
    }
}
