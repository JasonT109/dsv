using UnityEngine;
using System.Collections;

public class knotmeter : MonoBehaviour {

    public GameObject tacho;
    public float smooth = 2.0f;
    public float startAngle = 0.0f;
    public float endAngle = 0.0f;
    public float currentAngle = 0.0f;
    public float idleRPM = 1000.0f;
    public float currentSpeed = 0.0f;
    public float maxSpeed = 12.0f;
    public float parasiticDrag = 83.0f;
    public float acceleration = 0.0f;
    public float drag = 0.1f;
    public float targetSpeed = 0.0f;
    // Use this for initialization
    void Start () {
        currentAngle = startAngle;
        Quaternion qAngle = Quaternion.Euler(0, 0, currentAngle);
        transform.rotation = qAngle;
    }
	
	// Update is called once per frame
	void Update () {
        var tachoScript = tacho.GetComponent<tachometer>();

        //drag should be greater at lower speeds
        //parasitic drag should be greater at higher speeds

        targetSpeed = (currentSpeed + (tachoScript.currentRPM - (parasiticDrag * currentSpeed)) * Time.deltaTime) * drag; 


        //targetSpeed = Mathf.Clamp(targetSpeed, 0, maxSpeed);
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * acceleration);

        currentAngle = (currentSpeed / maxSpeed) * (startAngle - endAngle) + startAngle;
        Quaternion qAngle = Quaternion.Euler(0, 0, currentAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, qAngle, Time.deltaTime * smooth);
    }
}
