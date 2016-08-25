using UnityEngine;
using System.Collections;

public class valueFromButtonGroup : MonoBehaviour {

    public GameObject buttonGroup;
    public int[] values;
    public GameObject graph;
    public float changeSpeed = 1.0f;
    private GameObject[] buttons;

	// Use this for initialization
	void Start () {
	    if (buttonGroup && values.Length > 1)
        {
            //we can drive graph from something
            buttons = buttonGroup.GetComponent<buttonGroup>().buttons;
        }
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetComponent<buttonControl>().active)
            {
                float newValue = (float)graph.GetComponent<digital_gauge>().value;
                newValue = Mathf.Lerp(newValue, values[i], Time.deltaTime * changeSpeed);

                graph.GetComponent<digital_gauge>().value = (int)Mathf.RoundToInt( newValue);
            } 
        }
    }
}
