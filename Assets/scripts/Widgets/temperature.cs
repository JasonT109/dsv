using UnityEngine;
using System.Collections;

public class temperature : MonoBehaviour
{

    public GameObject tacho;
    public float smooth = 2.0f;
    public float startAngle = 0.0f;
    public float endAngle = 0.0f;
    public float currentAngle = 0.0f;
    public float minTemp = 30.0f;
    public float maxTemp = 120.0f;
    public float normalTemp = 75.0f;
    public float currentTemperature = 0.0f;
    public float heatTime = 30.0f;

    // Use this for initialization
    void Start()
    {
        currentTemperature = minTemp;
        currentAngle = startAngle;
        Quaternion qAngle = Quaternion.Euler(0, 0, currentAngle);
        transform.rotation = qAngle;
    }

    // Update is called once per frame
    void Update()
    {
        //= (X-A)/(B-A) * (D-C) + C
        var tachoScript = tacho.GetComponent<tachometer>();

        //if rpm is > 0
        //increase lerp up to normal operating temp
        if (tachoScript.currentRPM > 0.0f && tachoScript.currentRPM <= 3000.0f)
        {
            currentTemperature = Mathf.Lerp(currentTemperature, normalTemp, Time.deltaTime * (heatTime * (tachoScript.currentRPM * 0.001f)));
        }

        //if rpm > 3000
        //lerp up to max temp
        if (tachoScript.currentRPM > 3000.0f)
        {
            currentTemperature = Mathf.Lerp(currentTemperature, maxTemp, Time.deltaTime * (heatTime * tachoScript.currentRPM * 0.001f));
        }

        currentAngle = (currentTemperature - minTemp) / (maxTemp - minTemp) * (endAngle - startAngle) + startAngle;
        Quaternion qAngle = Quaternion.Euler(0, 0, currentAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, qAngle, Time.deltaTime * smooth);
    }
}