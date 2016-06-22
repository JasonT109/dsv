using UnityEngine;
using System.Collections;

public class textValueFromDepth : MonoBehaviour {

    public GameObject depthCalculator;
    public bool pressure;
    public bool psi;
    public bool waterTemp;
    public float updateTick = 0.2f;
    private float nextUpdate = 0;

    // Use this for initialization
    void Start ()
    {
	    if (depthCalculator)
        {
            if (pressure)
            {
                gameObject.GetComponent<TextMesh>().text = (depthCalculator.GetComponent<clientCalcValues>().pressureResult.ToString("n2") + "bar");
            }
            if (waterTemp)
            {
                gameObject.GetComponent<TextMesh>().text = (depthCalculator.GetComponent<clientCalcValues>().waterTempResult.ToString("n2") + "°c");
            }
        }
        nextUpdate = Time.time;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (depthCalculator)
        {
            if (Time.time > nextUpdate)
            {
                nextUpdate = Time.time + updateTick;
                if (pressure)
                {
                    float dValue = depthCalculator.GetComponent<clientCalcValues>().pressureResult;
                    int dInt = (int)dValue;
                    gameObject.GetComponent<TextMesh>().text = dInt.ToString() + "bar";
                }
                if (waterTemp)
                {
                    gameObject.GetComponent<TextMesh>().text = (depthCalculator.GetComponent<clientCalcValues>().waterTempResult.ToString("n2") + "°c");
                }
                if (psi)
                {
                    float dValue = depthCalculator.GetComponent<clientCalcValues>().psiResult;
                    int dInt = (int)dValue;
                    gameObject.GetComponent<TextMesh>().text = dInt.ToString() + "psi";
                }
            }
        }
    }
}
