﻿using UnityEngine;
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

    //used to force the bend animation to blend in more
    public float bendBoost = 10f;

    //used to control blend speed of bend animations
    public float bendBlendSpeed = 0.5f;

    //next way point number
    public int wayPointNumber = 0;

    private Vector3 angle;
    private Vector3 nextWayPoint;
    private bool goNext = true;
    private Vector3 originalPos;
    private Rigidbody rb;
    private Animator animator;
    private Vector3 localVelocity;
    private float blendValueC = 0f;
    private float blendValueL = 0f;
    private float blendValueR = 0f;
    
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
            angle.z *= bendBoost;

            //get local velocity from rigid body
            localVelocity = transform.InverseTransformDirection(rb.velocity);

            if (localVelocity.x < 0)
            {
                blendValueL = 0;
                blendValueR = angle.z;
                blendValueC = 1 - angle.z;

            }
            else if (localVelocity.x > 0)
            {
                blendValueL = angle.z;
                blendValueR = 0;
                blendValueC = 1 - angle.z;
            }
            else
            {
                blendValueL = 0;
                blendValueR = 0;
                blendValueC = 0;
            }

            turnRightBlend = Mathf.Lerp(turnRightBlend, blendValueR, Time.deltaTime * bendBlendSpeed);
            turnLeftBlend = Mathf.Lerp(turnLeftBlend, blendValueL, Time.deltaTime * bendBlendSpeed);
            swimBlend = Mathf.Lerp(swimBlend, blendValueC, Time.deltaTime * bendBlendSpeed);

            //move to waypoint in sequence
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
