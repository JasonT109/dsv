using UnityEngine;
using System.Collections;
using Meg.Networking;

public class graphicsSonarBehaviour : MonoBehaviour {

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
    //turn speed
    public float turnSpeed = 2.0f;
    //additional speed added during turn to make it more skittish and fish like
    public float drift = 0.01f;
    //distance under which we stop turning
    public float closeDistance = 1f;

    private Vector3 angle;
    private float timeCheck;
    private Vector3 newPos;
    private Vector3 originalPos;
    private Vector3 directionVector;

    void Awake()
    {
        var scale = serverUtils.SonarData.GetScale();

        transform.localScale = new Vector3(scale, scale, scale);
    }

    void Start ()
    { 
        timeCheck = Time.time;
        newPos = transform.position;
        originalPos = transform.position;
    }

    void Update ()
    {
        var scale = serverUtils.SonarData.GetScale();
        transform.localScale = new Vector3(scale, scale, scale);

        if (Time.time > timeCheck)
        {
            //create a new position relative to where we are
            newPos = new Vector3(originalPos.x + Random.Range(-amountX, amountX), originalPos.y + Random.Range(-amountY, amountY), originalPos.z + Random.Range(-amountZ, amountZ));
            newPos = new Vector3(Mathf.Clamp(newPos.x, originalPos.x - maxX, originalPos.x + maxX), Mathf.Clamp(newPos.y, originalPos.y - maxY, originalPos.y + maxY), Mathf.Clamp(newPos.z, originalPos.z - maxZ, originalPos.z + maxZ));

            //determine where the next point is in relation to object
            directionVector = (newPos - transform.position).normalized;
            angle = new Vector3(0, 0, Vector3.Angle(transform.forward, directionVector));
            angle.z = angle.z / 180;

            //add time
            timeCheck = Time.time + (timeTick + Random.Range(0.1f, 0.8f));
        }

        //reduce angle to 0
        angle.z = Mathf.Lerp(angle.z, 0, Time.deltaTime);

        //lerp to new position
        transform.position = Vector3.Lerp(transform.position + (transform.forward * angle.z * drift), newPos, Time.deltaTime * moveSpeed);
        //xz plane only movement
        transform.position = new Vector3(transform.position.x, originalPos.y, transform.position.z);

        //check we are far enough away to still turn to our point, this will prevent swivelling on the spot
        if (Vector3.Distance(newPos, transform.position) > closeDistance)
        {
            //rotate to angle
            Vector3 p = Vector3.RotateTowards(transform.forward, (newPos - transform.position).normalized, Time.deltaTime * turnSpeed, 1.0f);
            transform.rotation = Quaternion.LookRotation(p);

            //rotate on y only
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }

        //show intended position
        Debug.DrawRay(transform.position, newPos - transform.position);
    }
}
