using UnityEngine;
using System.Collections;

public class widgetGliderDescentMetrics : MonoBehaviour {

    public Transform cObj;
    public Transform cCam;
    public Transform marker2d;
    private float distance1;
    private float distance2;
    public float distanceP;
    public Vector3 cObjectPosition;
    public Vector3 cCameraPosition;
    public Vector3 newPosition;
    public Vector3 initPosition;

    // Use this for initialization
    void Start()
    {
        initPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        cCameraPosition = cCam.position;
        cObjectPosition = cObj.position;


        //get distance from camera to object
        distance1 = Vector3.Distance(cCam.position, cObj.position);

        //get distance from camera to transform
        distance2 = Vector3.Distance(cCam.position, initPosition);

        //work out percentage
        distanceP = distance2 / distance1;

        //position is objects - (camera * percentage)
        newPosition = cCam.position + ((cObj.position - cCameraPosition) * distanceP);

        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);

        marker2d.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, marker2d.localPosition.z);
    }
}
