using UnityEngine;
using System.Collections;

public class tachometer : MonoBehaviour {

    public GameObject inputs;
    public float smooth = 2.0f;
    public float minRPM = 0.0f;
    public float maxRPM = 4000.0f;
    public float currentRPM = 0.0f;
    public float currentXThrust = 0.0f;
    public float currentYThrust = 0.0f;
    public float currentX2Thrust = 0.0f;
    public float currentY2Thrust = 0.0f;
    public float needleNoise = 0.0f;
    public float A = -1.0f;
    public float B = 1.0f;
    public float idleRPM = 1000.0f;
    public float currentSpeed = 0.0f;
    public float maxSpeed = 12.0f;
    public float parasiticDrag = 83.0f;
    public float acceleration = 0.0f;
    public float drag = 0.1f;
    public float targetSpeed = 0.0f;

    // Use this for initialization
    void Awake () {
        inputs = GameObject.FindWithTag("Inputs");
        var inputScript = inputs.GetComponent<gameInputs>();
        currentRPM = (inputScript.output - A) / (B - A) * (maxRPM - minRPM) + minRPM;
    }

    void Start()
    {
        
    }
	
	// Update is called once per frame
	void Update () {
        //= (X-A)/(B-A) * (D-C) + C
        var inputScript = inputs.GetComponent<gameInputs>();
        //currentRPM = (inputScript.output - A) / (B - A) * (maxRPM - minRPM) + minRPM;
        //currentRPM = Mathf.Lerp(currentRPM, ((inputScript.output - A) / (B - A) * (maxRPM - minRPM) + minRPM), Time.deltaTime * smooth);
        //currentRPM = Mathf.Clamp(currentRPM, minRPM, maxRPM);
        currentRPM = inputScript.output;
        currentXThrust = inputScript.outputX;
        currentYThrust = inputScript.outputY;
        currentX2Thrust = inputScript.outputX2;
        currentY2Thrust = inputScript.outputY2;

        targetSpeed = (currentSpeed + (currentRPM - (parasiticDrag * currentSpeed)) * Time.deltaTime) * drag;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * acceleration);
    }
}
