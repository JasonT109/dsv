using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class graphicsSonarObject : NetworkBehaviour
{
    [SyncVar]
    public float turnRightBlend = 0;
    [SyncVar]
    public float turnLeftBlend = 0;
    [SyncVar]
    public float swimBlend = 0;

    //list of way points to travel to
    public Vector3[] wayPoints;

    //speed
    public float speed = 1.0f;

    //turn speed
    public float turnSpeed = 1.0f;

    //distance that is close enough for next waypoint
    public float closeDistance = 8.0f;

    //destroy after last way point?
    public bool destroyOnLastPoint = false;

    public Vector3 angle;
    public Vector3 nextWayPoint;
    private bool goNext = true;
    public int wayPointNumber = 0;
    private Vector3 originalPos;
    private Rigidbody rb;
    private Animator animator;
    public Vector3 localVelocity;
    
    void Start ()
    {
        animator = GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        originalPos = transform.position;
        if (wayPoints.Length > 0)
        {
            transform.position = wayPoints[0];
            wayPointNumber = 1;
        }
	}

	void Update ()
    {
        animator.SetFloat("swim", swimBlend);
        animator.SetFloat("turnright", turnRightBlend);
        animator.SetFloat("turnleft", turnLeftBlend);

        if (!isServer)
            return;
        if (wayPoints.Length > 0)
        {
            //Debug.Log("Moving");
            //if close enough to way point move to next one
            if (Vector3.Distance(transform.position, wayPoints[wayPointNumber]) < closeDistance)
            {
                if (wayPointNumber + 1 < wayPoints.Length)
                {
                    wayPointNumber++;
                    nextWayPoint = wayPoints[wayPointNumber];
                }
                else
                {
                    if (destroyOnLastPoint)
                    {
                        Destroy(gameObject, 1.0f);
                    }
                    else
                    {
                        wayPointNumber = 0;
                    }
                }
            }

            //determine where the next point is in relation to object
            Vector3 directionVector = (wayPoints[wayPointNumber] - transform.position).normalized;
            angle = new Vector3(0, 0, Vector3.Angle(transform.forward, directionVector));
            angle.z = angle.z / 180;
            //boost this a bit so we get more bend
            angle.z *= 2.5f;

            localVelocity = transform.InverseTransformDirection(rb.velocity);

            if (localVelocity.x < 0)
            {
                turnLeftBlend = 0;
                turnRightBlend = Mathf.Lerp(turnRightBlend, angle.z, Time.deltaTime);
                swimBlend = Mathf.Lerp(swimBlend, 1 - angle.z, Time.deltaTime);

            }
            else if (localVelocity.x > 0)
            {
                turnRightBlend = 0;
                turnLeftBlend = Mathf.Lerp(turnLeftBlend, angle.z, Time.deltaTime);
                swimBlend = Mathf.Lerp(swimBlend, 1 - angle.z, Time.deltaTime);
            }
            else
            {
                swimBlend = Mathf.Lerp(swimBlend, 0, Time.deltaTime);
                turnLeftBlend = Mathf.Lerp(turnLeftBlend, 0, Time.deltaTime);
                turnRightBlend = Mathf.Lerp(turnRightBlend, 0, Time.deltaTime);
            }

            //move to waypoint in sequence
            //transform.position = Vector3.Lerp(transform.position, wayPoints[wayPointNumber], Time.deltaTime * speed);
            rb.AddForce(transform.forward * speed, ForceMode.Impulse);

            //xz plane only movement
            transform.position = new Vector3(transform.position.x, originalPos.y, transform.position.z);

            //rotate to waypoint
            Vector3 p = Vector3.RotateTowards(transform.forward, (wayPoints[wayPointNumber] - transform.position).normalized, Time.deltaTime * turnSpeed, 1.0f);
            transform.rotation = Quaternion.LookRotation(p);

            //rotate on y only
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

            //show intended position
            Debug.DrawRay(transform.position, wayPoints[wayPointNumber] - transform.position);
        }
    }
}
