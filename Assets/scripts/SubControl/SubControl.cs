using UnityEngine;
using System.Collections;
using Meg.Networking;

public class SubControl : MonoBehaviour 
{


    public float inputXaxis;

    public float inputYaxis;

    public float inputZaxis;

    public float inputXaxis2;

    public float inputYaxis2;

    public float depth;

    private Rigidbody rb;
    public bool disableInput = false;
    private Vector3 rotation;
    public float currentThrust = 0.0f;
    public float currentDiveThrust = 0.0f;

    private float thrust = 25.258f;
    public float Acceleration = 1.0f;
    public float yawSpeed = 400f;
    public float pitchSpeed = 100f;
    public float rollSpeed = 1500f;

    public float MaxSpeed = 200f;
    public float MinSpeed = -100f;

    public bool isAutoStabilised;
    public bool IsPitchAlsoStabalised;
    public bool JoystickOverride;

    public bool IsJoystickSwapped = false;

    // private float bankAmount = 1.0f;
    // private Vector3 bankAxis = new Vector3(0F, 0F, -1F);
    private float rollResult;
    private float pitchResult;
    private float yawResult;
    private float bank;
    private float roll;
    private float pitch;


	// Use this for initialization
	void Start () 
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0,0,0);

        MinSpeed = -(0.5f * MaxSpeed);

        JoystickOverride = false;
	}

    // Snap the sub to a given worldspace velocity vector.
    public void SetWorldVelocity(Vector3 velocity)
    {
        // Apply velocity immediately to the sub.
        rb.velocity = velocity;

        // Force sub to look in the direction of travel.
        // TODO: Should this be smoothed out somehow?
        transform.LookAt(rb.position + velocity);
    }

    // Update is called once per frame
    public void SubController()
    {
        UpdateData();

        // Don't apply sub control if vessel is being moved by the vessel simulation.
        var movement = serverUtils.GetVesselMovements().GetPlayerVesselMovement();
        if (movement)
            return;

        Vector3 RollExtract = new Vector3(0,0,-1);
        Vector3 PitchExtract = new Vector3(-1,0,0);
        Vector3 YawExtract = new Vector3(0,1,0);

        if (!disableInput)
        {
            //this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z);

            //apply the orientation forces
            rb.AddRelativeTorque(PitchExtract * (pitchSpeed * inputYaxis));
            rb.AddRelativeTorque(RollExtract * (rollSpeed * inputXaxis));
            rb.AddRelativeTorque(YawExtract * (yawSpeed * inputXaxis));

            //adjust mix for pitch when doing a banking turn
            rb.AddRelativeTorque(PitchExtract * (pitchSpeed * Mathf.Abs(inputXaxis)));

            //apply the thrust forces    
            if(currentThrust < MaxSpeed && currentThrust > MinSpeed)
            {
                currentThrust = inputZaxis * (Acceleration * thrust);
            }
            
            if(currentThrust > MaxSpeed)
            {
                currentThrust = MaxSpeed - 0.01f;
            }
            
            if(currentThrust < MinSpeed)
            {
                currentThrust = MinSpeed + 0.01f;
            }

            rb.AddRelativeForce(0, 0, currentThrust * thrust * Time.deltaTime);


            //apply the dive forces    
            //if(currentDiveThrust < MaxSpeed && currentDiveThrust > MinSpeed)
            //{
            //    currentDiveThrust = inputYaxis2 * (Acceleration * thrust);
            //}
            //
            //if(currentDiveThrust > MaxSpeed)
            //{
            //    currentDiveThrust = MaxSpeed - 0.01f;
            //}
            //
            //if(currentDiveThrust < MinSpeed)
            //{
            //    currentDiveThrust = MinSpeed + 0.01f;
            //}
            //
            //rb.AddRelativeForce(0, currentDiveThrust * thrust * Time.deltaTime, 0);

        }

        //if roll is almost right, stop adjusting (PID dampening todo here?)
        if(roll > 0.09f && roll < 364.91f && isAutoStabilised)
        {
            AutoStabilise();
        }
        else if(pitch > 0.09f && roll < 364.91f && isAutoStabilised)
        {
            AutoStabilise();
        }


        //ApplyDataChanges();
    }

    //this function 
    public void UpdateData()
    {
        serverData Data;
        Data = this.GetComponent<serverData>();

        if(JoystickOverride)
        {
            // axis are update directly from sliders in ship debug. refer to code
            // coming from debug ship panel object
        }
        else
        {
            inputXaxis = Data.inputXaxis;
            inputYaxis = Data.inputYaxis;
            inputZaxis = Data.inputZaxis;
            inputXaxis2 = Data.inputXaxis2;
            inputYaxis2 = Data.inputYaxis2;

            //if(!IsJoystickSwapped)
            //{
            //    inputXaxis = Data.inputXaxis;
            //    inputYaxis = Data.inputYaxis;
            //    inputZaxis = Data.inputZaxis;
            //    inputXaxis2 = Data.inputXaxis2;
            //    inputYaxis2 = Data.inputYaxis2;
            //}
            //else
            //{
            //    inputXaxis = Data.inputXaxis2;
            //    inputYaxis = Data.inputYaxis2;
            //
            //    inputZaxis = Data.inputYaxis;
            //
            //    inputXaxis2 = Data.inputXaxis;
            //    inputYaxis2 = Data.inputYaxis;
            //}
        }

        disableInput = Data.disableInput;

        roll = Data.transform.eulerAngles.z;
        pitch = Data.transform.eulerAngles.x;

    }

    public void ApplyDataChanges()
    {
        
    }

    void AutoStabilise()
    {
        float stability = 0.3f * 10f;
        float speed = 2.0f * 10f;

        Vector3 predictedUP = Quaternion.AngleAxis(rb.angularVelocity.magnitude * Mathf.Rad2Deg * stability / speed,
            rb.angularVelocity) * transform.up;

        Vector3 torqueVector = Vector3.Cross(predictedUP, Vector3.up);

        if(!IsPitchAlsoStabalised)
        {
            torqueVector = Vector3.Project(torqueVector, transform.forward);
        }

        rb.AddTorque(torqueVector * speed * speed);
    }
        
}
