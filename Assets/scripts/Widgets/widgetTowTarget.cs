using UnityEngine;
using System.Collections;

public class widgetTowTarget : MonoBehaviour {

    //amount we can move on x
    public float amountX = 0.0f;
    //max deviation from start point
    public float maxX = 10f;
    //amount we can move on y
    public float amountY = 0.0f;
    //max deviation from start point
    public float maxY = 10f;
    //amount we can move on z
    public float amountZ = 0.0f;
    //max deviation from start point
    public float maxZ = 10f;
    //time inbetween picking new positions
    public float timeTick = 0.1f;
    //speed we move
    public float moveSpeed = 1.0f;
    //additional speed added during turn to make it more skittish and fish like
    public float drift = 0.01f;
    //distance under which we stop turning
    public float closeDistance = 1f;
    //turn speed
    public float turnSpeed = 2.0f;

    public float newPosDriftSpeed = 1f;

    public float updateTimeMin = 0.2f;
    public float updateTimeMax = 1f;

    private Vector3 angle;
    private float timeCheck;
    private Vector3 newPos;
    private Vector3 originalPos;
    private Vector3 directionVector;

    void Start ()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update ()
    {
        Vector3 oldPos = newPos;
        if (Time.time > timeCheck)
        {
            //create a new position relative to where we are
            newPos = new Vector3(originalPos.x + Random.Range(-amountX, amountX), originalPos.y + Random.Range(-amountY, amountY), originalPos.z);
            newPos = new Vector3(Mathf.Clamp(newPos.x, originalPos.x - maxX, originalPos.x + maxX), Mathf.Clamp(newPos.y, originalPos.y - maxY, originalPos.y + maxY), originalPos.z);

            //determine where the next point is in relation to object
            directionVector = (newPos - transform.position).normalized;
            angle = new Vector3(0, 0, Vector3.Angle(transform.forward, directionVector));
            angle.z = angle.z / 180;

            //add time
            timeCheck = Time.time + (timeTick + Random.Range(updateTimeMin, updateTimeMax));
        }

        //reduce angle to 0
        angle.z = Mathf.Lerp(angle.z, 0, Time.deltaTime);

        newPos = Vector3.Lerp(newPos, oldPos, Time.deltaTime * newPosDriftSpeed);

        //lerp to new position
        transform.position = Vector3.Lerp(transform.position + (transform.forward * angle.z * drift), newPos, Time.deltaTime * moveSpeed);
        //xy plane only movement
        transform.position = new Vector3(transform.position.x, transform.position.y, originalPos.z);


        //check we are far enough away to still turn to our point, this will prevent swivelling on the spot
        if (Vector3.Distance(newPos, transform.position) > closeDistance)
        {
            //rotate to angle
            Vector3 p = Vector3.RotateTowards(transform.forward, (newPos - transform.position).normalized, Time.deltaTime * turnSpeed, 1.0f);
            transform.rotation = Quaternion.LookRotation(p);

            //rotate on z only
            //transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
        }
    }
}
