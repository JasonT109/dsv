using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Meg.Networking;

public class SubControl : NetworkBehaviour 
{
    /** Whether input to sub is completely disabled. */
    [SyncVar]
    public bool disableInput = false;

    /** Whether input to sub is overridden by the server. */
    [SyncVar]
    public bool JoystickOverride;

    /** Whether input to sub can come from the pilot. */
    [SyncVar]
    public bool JoystickPilot = true;

    [SyncVar]
    public float inputXaxis;
    [SyncVar]
    public float inputYaxis;
    [SyncVar]
    public float inputZaxis;
    [SyncVar]
    public float inputXaxis2;
    [SyncVar]
    public float inputYaxis2;
    [SyncVar]
    public bool isAutoStabilised;
    [SyncVar]
    public bool IsPitchAlsoStabilised;
    [SyncVar]
    public float Acceleration = 1.0f;
    [SyncVar]
    public float yawSpeed = 400f;
    [SyncVar]
    public float pitchSpeed = 100f;
    [SyncVar]
    public float rollSpeed = 1500f;
    [SyncVar]
    public float MaxSpeed = 200f;
    [SyncVar]
    public float MinSpeed = -100f;
	[SyncVar]
	public bool isAutoPilot = false;
	[SyncVar]
	public bool isControlDecentMode = false;
	[SyncVar]
	public bool isControlModeOverride = false;
	[SyncVar]
	public bool isControlOverrideStandard = false;


    private float currentThrust = 0.0f;
    private Rigidbody rb;
    private Vector3 rotation;
	private float thrust = 0f;//25.258f;
    private float rollResult;
    private float pitchResult;
    private float yawResult;
    private float bank;
    private float roll;
    private float pitch;

	private float forwardThrust = 50f;
	private float AccelRatio = 0.0f;

	// Use this for initialization
	void Awake() 
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

    /** Return the sub's current world velocity. */
    public Vector3 GetWorldVelocity()
    {
        return rb.velocity;
    }

    // Update is called once per frame
    public void SubController()
    {
        UpdateData();

        // Don't apply sub control if vessel is being moved by the vessel simulation.
        var movement = serverUtils.GetVesselMovements().GetPlayerVesselMovement();
        if (movement && movement.Active)
            return;

		if(!isControlDecentMode)
		{
			ApplyStandardControlForces();
		}
		else
		{
			ApplyDecentControlForces();
		}

    }

    public void UpdateData()
    {
        serverData Data;
        Data = this.GetComponent<serverData>();

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

        if(!IsPitchAlsoStabilised)
        {
			if(!isControlDecentMode)
			{
				torqueVector = Vector3.Project(torqueVector, transform.forward);
			}
        }

        rb.AddTorque(torqueVector * speed * speed);
    }

	void ApplyStandardControlForces()
	{
		Vector3 RollExtract = new Vector3(0,0,1);
		Vector3 PitchExtract = new Vector3(-1,0,0);
		Vector3 YawExtract = new Vector3(0,1,0);

		//if (!disableInput)
		//{
		//	// Apply the orientation forces
		//	rb.AddRelativeTorque(PitchExtract * (pitchSpeed * inputYaxis));
		//	rb.AddRelativeTorque(RollExtract * (rollSpeed * inputXaxis));
		//	rb.AddRelativeTorque(YawExtract * (yawSpeed * inputXaxis));
		//
		//	// Adjust mix for pitch when doing a banking turn.
		//	rb.AddRelativeTorque(-PitchExtract * (pitchSpeed * Mathf.Abs(inputXaxis)));
		//
		//	// Apply the thrust forces.
		//	if (currentThrust < MaxSpeed && currentThrust > MinSpeed)
		//		currentThrust = inputZaxis * (Acceleration * thrust);
		//
		//	if (currentThrust > MaxSpeed)
		//		currentThrust = MaxSpeed - 0.01f;
		//
		//	if (currentThrust < MinSpeed)
		//		currentThrust = MinSpeed + 0.01f;
		//
		//	rb.AddRelativeForce(0, 0, currentThrust * thrust * Time.deltaTime);
		//}

		if (!disableInput)
		{
			// Apply the orientation forces
			rb.AddRelativeTorque(PitchExtract * (pitchSpeed * inputYaxis));
			rb.AddRelativeTorque(RollExtract * (rollSpeed * inputXaxis));
			rb.AddRelativeTorque(YawExtract * (yawSpeed * inputXaxis));
			//this.gameObject.transform.rotation = Quaternion.Euler(this.gameObject.transform.rotation.eulerAngles + new Vector3(0f,inputXaxis*Time.deltaTime*yawSpeed,0f));
			//this.gameObject.transform.Rotate(0f,inputXaxis*yawSpeed*Time.deltaTime,0f);

			// Adjust mix for pitch when doing a banking turn.
			rb.AddRelativeTorque(-PitchExtract * (yawSpeed*0.5f * Mathf.Abs(inputXaxis)));

			// Apply the thrust forces.
			//if (currentThrust < MaxSpeed && currentThrust > MinSpeed)
			//	currentThrust = inputZaxis * ((Acceleration) * thrust);
			//
			//if (currentThrust > MaxSpeed)
			//	currentThrust = MaxSpeed - 0.01f;
			//
			//if (currentThrust < MinSpeed)
			//	currentThrust = MinSpeed + 0.01f;
			//
			//rb.AddRelativeForce(0, 0, currentThrust, ForceMode.Acceleration);
		}

		// If roll is almost right, stop adjusting (PID dampening todo here?)
		if (roll > 0.09f && roll < 364.91f && isAutoStabilised)
			AutoStabilise();
		else if (pitch > 0.09f && roll < 364.91f && isAutoStabilised)
			AutoStabilise();

		ThrustControl();
	}

	void ApplyDecentControlForces()
	{
		Vector3 PitchExtract = new Vector3(-1,0,0);
		Vector3 YawExtract = new Vector3(0,1,0);

		if (!disableInput)
		{
			// Apply the orientation forces
			rb.AddTorque(YawExtract * (yawSpeed * inputXaxis));
			rb.AddRelativeTorque(PitchExtract * (pitchSpeed * inputYaxis));
		}

		// If roll is almost right, stop adjusting (PID dampening todo here?)
		if (roll > 0.09f && roll < 364.91f && isAutoStabilised)
			AutoStabilise();
		else if (pitch > 0.09f && roll < 364.91f && isAutoStabilised)
			AutoStabilise();

		ThrustControl();
	}

	public void ThrustControl()
	{
		currentThrust = thrust;

		if(inputZaxis > 0.01f || inputZaxis < -0.01f)
		{
			rb.drag = ((Acceleration/100f))/2;
		}
		else
		{
			rb.drag = 0.5f;
		}

		if(isControlDecentMode)
		{
			thrust = inputZaxis * 0.0337f * (MaxSpeed/2f);
			currentThrust*= forwardThrust * 15.0f;
			rb.AddRelativeForce(0f, -currentThrust * Time.deltaTime * ((Acceleration/100f)),0f);
		}
		else
		{
			if(inputZaxis > 0)
			{
				thrust = inputZaxis * 0.0337f * MaxSpeed;
			}
			else
			{
				thrust = inputZaxis * 0.0337f * MaxSpeed/2f;
			}

			currentThrust*= forwardThrust * 15.0f;
			rb.AddRelativeForce(0f,0f,currentThrust * Time.deltaTime * ((Acceleration/100f)));
		}
			
	}
        
}
