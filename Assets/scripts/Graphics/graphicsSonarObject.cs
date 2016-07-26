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

    //used to force the bend animation to blend in more
    public float bendBoost = 10f;

    //used to control blend speed of bend animations
    public float bendBlendSpeed = 0.5f;

    //next way point number
    public int wayPointNumber = 0;

    public float minTurnSpeed = 1f;

    private Vector3 angle;
    private Vector3 nextWayPoint;
    private bool goNext = true;
    private Vector3 originalPos;
    private Rigidbody rb;
    private Animator animator;
    public Vector3 localVelocity;
    private float blendValueC = 0f;
    private float blendValueL = 0f;
    private float blendValueR = 0f;

    GameObject DataRoot;
    
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

        DataRoot = GameObject.FindGameObjectWithTag("ServerData");
	}

    void Awake()
    {
        DataRoot = GameObject.FindGameObjectWithTag("ServerData");
        this.transform.localScale = new Vector3 (DataRoot.GetComponent<SonarData>().getScale(), 
                    DataRoot.GetComponent<SonarData>().getScale(), 
                    DataRoot.GetComponent<SonarData>().getScale());
    }

	void Update ()
    {
        this.transform.localScale = new Vector3 (DataRoot.GetComponent<SonarData>().getScale(), 
            DataRoot.GetComponent<SonarData>().getScale(), 
            DataRoot.GetComponent<SonarData>().getScale());

        turnSpeed = DataRoot.GetComponent<SonarData>().getScaleTurnSpeed();
        speed = DataRoot.GetComponent<SonarData>().getScaleSpeed();

        animator.SetFloat("swim", Mathf.Clamp01(swimBlend));
        animator.SetFloat("turnright", Mathf.Clamp01(turnRightBlend));
        animator.SetFloat("turnleft", Mathf.Clamp01(turnLeftBlend));

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

            //get local velocity from rigid body
            localVelocity = transform.InverseTransformDirection(rb.velocity);

            if (localVelocity.x < -minTurnSpeed)
            {
                blendValueL = 0;
                blendValueR = Mathf.Clamp(Mathf.Abs(localVelocity.x) * bendBoost, 0, 1); // angle.z;
                blendValueC = 1 - Mathf.Clamp(Mathf.Abs(localVelocity.x) * bendBoost, 0, 1); // 1 - angle.z;

            }
            else if (localVelocity.x > minTurnSpeed)
            {
                blendValueL = Mathf.Clamp(Mathf.Abs(localVelocity.x) * bendBoost, 0, 1); // angle.z;
                blendValueR = 0;
                blendValueC = 1 - Mathf.Clamp(Mathf.Abs(localVelocity.x) * bendBoost, 0, 1); // 1 - angle.z;
            }
            else
            {
                blendValueL = 0;
                blendValueR = 0;
                blendValueC = 1;
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
