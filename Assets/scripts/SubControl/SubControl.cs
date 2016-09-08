using UnityEngine;
using UnityEngine.Networking;
using Meg.Networking;

public class SubControl : NetworkBehaviour 
{

    // Constants
    // ------------------------------------------------------------

    /** Scaling factor for adjusting acceleration scale. */
    private const float AccelerationScale = 0.25f;

    /** Angle thresholds used to determine when auto-stabilization is appropriate. */
    private const float MinRoll = 0.09f;
    private const float MaxRoll = 364.91f;
    private const float MinPitch = 0.09f;


    // Properties
    // ------------------------------------------------------------

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
	[SyncVar] 
	public float MotionScaleImpacts = 1.0f;
	[SyncVar] 
	public float MotionMinImpactInterval = 0.75f;

    [SyncVar]
    public float StabiliserSpeed = 20f;
    [SyncVar]
    public float StabiliserStability = 30f;
    [SyncVar]
    public float BowtieDeadzone; //syncvar this


    //TODO Make these sync vars

    public AnimationCurve RollLimitCurve;
    public AnimationCurve PitchLimitCurve;

    public bool TripPitch = false;
    public bool TripRoll = false;
    //public float ScaleRoll;


    // Members
    // ------------------------------------------------------------

    /** The rigidbody that's being controlled. */
    private Rigidbody _rigidbody;

    /** Timestamp of last physics impact event. */
    private float _lastImpactTime;

    /** Timestamp for next possible physics impact event. */
    private float _nextImpactTime;


    // Unity Methods
    // ------------------------------------------------------------

    /** Preinitialization. */
    private void Awake() 
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = Vector3.zero;

        MinSpeed = -(0.5f * MaxSpeed);
        JoystickOverride = false;

        if (serverUtils.IsGlider())
            ApplyGliderDefaults();
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Apply per-frame control forces to the sub's rigidbody. */
    public void ApplyForces()
    {
        // Don't apply forces if vessel is being simulated.
        var movement = serverUtils.GetPlayerVesselMovement();
        if (movement && movement.Active)
            return;

        // Apply the appropriate control logic.
        if (serverUtils.IsGlider())
            ApplyGliderForces();
        else
            ApplySubForces();
    }

    /** Apply an impact impulse vector to the sub's rigidbody. */
    public void Impact(Vector3 impactVector)
    {
        // Ensure impact scaling factor remains sensible (between 0 and 1).
        MotionScaleImpacts = Mathf.Clamp01(MotionScaleImpacts);

        // Apply impact vector to the sub's rigidbody.
        ApplyImpact(impactVector);
    }

    /** Snap the sub to a given worldspace velocity vector. */
    public void SetWorldVelocity(Vector3 velocity)
    {
        // Apply velocity immediately to the sub.
        _rigidbody.velocity = velocity;

        // Force sub to look in the direction of travel.
        // TODO: Should this be smoothed out somehow?
        transform.LookAt(_rigidbody.position + velocity);
    }

    /** Return the sub's current world velocity. */
    public Vector3 GetWorldVelocity()
        { return _rigidbody.velocity; }

    /** set the y velocity to the motion base.  */
    public void CalculateYawVelocity()
    {
        serverUtils.MotionBaseData.MotionBaseYaw = _rigidbody.angularVelocity.y;
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Apply defaults that relate to the glider here */
    private void ApplyGliderDefaults()
    {
        _rigidbody.angularDrag = 1.3f;
        _rigidbody.mass = 0.2f;

        pitchSpeed = 350f;
        rollSpeed = 500f;
        yawSpeed = 0;
        
        StabiliserSpeed = 7f;
        StabiliserStability = 1f;

        serverUtils.MotionBaseData.MotionSlerpSpeed = 5.0f;
    }

    /** Apply control forces to pilot a big sub. */
    private void ApplySubForces()
    {
        // Switch between descent mode and regular piloting.
        if (isControlDecentMode)
            ApplyDescentForces();
        else
            ApplyStandardForces();

        // Auto-stabilize the sub if desired.
        ApplyStabilizationForce();

        // Apply thrust to move the sub forward or backwards.
        ApplyThrustForce();
    }

    private void ApplyGliderForces()
    {
        // Apply the orientation forces.
        //_rigidbody.AddRelativeTorque(Vector3.left * (pitchSpeed * inputYaxis));

        _rigidbody.AddRelativeTorque(Vector3.up * (yawSpeed * inputXaxis));

        GliderRollLogic();

        GliderPitchLogic();


        // Adjust mix for pitch when doing a banking turn.
        _rigidbody.AddRelativeTorque(Vector3.right * (yawSpeed * 0.5f * Mathf.Abs(inputXaxis)));

        // Auto-stabilize the sub if desired, and if inputs are weak.
        if(inputYaxis < 0.3f && inputYaxis > -0.3f)
        {
            if (inputXaxis < 0.3f && inputXaxis > -0.3f)
                ApplyStabilizationForce();
        }

        // Apply thrust to move the sub forward or backwards.
        ApplyThrustForce();

        //DEBUG STUFF
        if (transform.localRotation.eulerAngles.z > 175f && transform.localRotation.eulerAngles.z < 185f)
        {
            TripRoll = true;
            serverUtils.MotionBaseData.MotionHazard = true;
            Debug.Log("MotionHazard too much roll detected");
        }
           
        if (transform.localRotation.eulerAngles.x > 85f && transform.localRotation.eulerAngles.x < 265f)
        {
            TripPitch = true;
            serverUtils.MotionBaseData.MotionHazard = true;
            Debug.Log("MotionHazard too much pitch detected");
        }
    }

    /* Roll with constraints */
    private void GliderRollLogic()
    {
        //test for bowtie deadzone
        if (inputXaxis < BowtieDeadzone * inputYaxis && inputXaxis > -0.2f * inputYaxis)
        {
            //fallin within deadzone. Go directly to jail, do not pass go.
            return;
        }

        //roll curve value
        var currentRoll = transform.localRotation.eulerAngles.z;
        if (currentRoll > 180f)
        {
            currentRoll = Mathf.Abs(currentRoll - 360f);
        }
        var ScaleRoll = RollLimitCurve.Evaluate(currentRoll / 180f);
        ScaleRoll = Mathf.Abs(ScaleRoll);

        //roll logiic
        if (inputXaxis > 0 && transform.localRotation.eulerAngles.z > 180f && transform.localRotation.eulerAngles.z < 360f)
        {
            _rigidbody.AddRelativeTorque((Vector3.forward * ((rollSpeed) * -inputXaxis)) * ScaleRoll);
        }
        else if (inputXaxis < 0 && transform.localRotation.eulerAngles.z > 0f && transform.localRotation.eulerAngles.z < 180f)
        {
            _rigidbody.AddRelativeTorque((Vector3.forward * ((rollSpeed) * -inputXaxis)) * ScaleRoll);
        }
        else
        {
            _rigidbody.AddRelativeTorque((Vector3.forward * (rollSpeed * -inputXaxis)));
        }
    }

    /* Pitch with constraints */
    private void GliderPitchLogic()
    {
        //test for bowtie deadzone
        if(inputYaxis < BowtieDeadzone * inputXaxis && inputYaxis > -0.2f * inputXaxis)
        {
            //fallin within deadzone. Go directly to jail, do not pass go.
            return;
        }

        //pitch curve value
        var currentPitch = transform.localRotation.eulerAngles.x;
        if (currentPitch > 180f)
        {
            currentPitch = Mathf.Abs(currentPitch - 360f);
        }
        var ScalePitch = PitchLimitCurve.Evaluate(currentPitch / 180f);
        ScalePitch = Mathf.Abs(ScalePitch);

        //pitch logic
        if (inputYaxis > 0 && transform.localRotation.eulerAngles.x > 180f && transform.localRotation.eulerAngles.x < 360f)
        {
            _rigidbody.AddRelativeTorque((Vector3.left * ((pitchSpeed) * inputYaxis)) * ScalePitch);
        }
        else if (inputYaxis < 0 && transform.localRotation.eulerAngles.x > 0f && transform.localRotation.eulerAngles.x < 180f)
        {
            _rigidbody.AddRelativeTorque((Vector3.left * ((pitchSpeed) * inputYaxis)) * ScalePitch);
        }
        else
        {
            _rigidbody.AddRelativeTorque(Vector3.left * (pitchSpeed * inputYaxis));
        }
    }

    /** Apply standard control forces for the big sub. */
    private void ApplyStandardForces()
	{
        // Check if input has been disabled.
        if (disableInput)
            return;

        // Apply the orientation forces.
        _rigidbody.AddRelativeTorque(Vector3.left * (pitchSpeed * inputYaxis));
        _rigidbody.AddRelativeTorque(Vector3.forward * (rollSpeed * -inputXaxis));
        _rigidbody.AddRelativeTorque(Vector3.up * (yawSpeed * inputXaxis));

        // Adjust mix for pitch when doing a banking turn.
        _rigidbody.AddRelativeTorque(Vector3.right * (yawSpeed * 0.5f * Mathf.Abs(inputXaxis)));
	}

    /** Apply control forces while in descent mode for the big sub. */
    private void ApplyDescentForces()
	{
        // Check if input has been disabled.
        if (disableInput)
            return;

        // Apply the orientation forces.
        _rigidbody.AddTorque(Vector3.up * (yawSpeed * inputXaxis));
        _rigidbody.AddRelativeTorque(Vector3.left * (pitchSpeed * inputYaxis));
	}

    /** Update auto-stabilization logic. */
    private void ApplyStabilizationForce()
    {
        if (!isAutoStabilised)
            return;

        // Determine sub's current orientation.
        var roll = transform.eulerAngles.z;
        var pitch = transform.eulerAngles.x;

        // If roll is almost right, stop adjusting (PID dampening todo here?)
        if (roll > MinRoll && roll < MaxRoll)
            AutoStabilize();
        else if (pitch > MinPitch && roll < MaxRoll)
            AutoStabilize();
    }

    /** Apply autostabilization to the sub's rigidbody. */
    private void AutoStabilize()
    {
        // TODO: Factor these numbers into constants.
        //const float stability = 0.3f * 10f;
        //const float speed = 2.0f * 10f;
        var predictedUp = Quaternion.AngleAxis(_rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * StabiliserStability / StabiliserSpeed,
            _rigidbody.angularVelocity) * transform.up;

        var torqueVector = Vector3.Cross(predictedUp, Vector3.up);
        if (!IsPitchAlsoStabilised && !isControlDecentMode)
            torqueVector = Vector3.Project(torqueVector, transform.forward);

        _rigidbody.AddTorque(torqueVector * StabiliserSpeed * StabiliserSpeed);
    }

    /** Apply thrust forces to the sub's rigidbody. */
    public void ApplyThrustForce()
	{
        // Check if input has been disabled.
        if (disableInput)
            return;

        // Check that sub has a non-zero max speed.
        if (Mathf.Approximately(MaxSpeed, 0))
            return;

        // Compute drag based on max. acceleration and desired terminal velocity.
        // See http://forum.unity3d.com/threads/terminal-velocity.34667/ for more info.
        var maxAcceleration = Acceleration * AccelerationScale;
        var maxSpeed = Mathf.Abs(MaxSpeed);
        var maxThrust = maxAcceleration * _rigidbody.mass;
        var idealDrag = maxAcceleration / maxSpeed;
        _rigidbody.drag = idealDrag / (idealDrag * Time.fixedDeltaTime + 1);

        // Determine the amount of thrust force to apply based on input.
        var thrust = inputZaxis * maxThrust;
        var reverseThrustRatio = Mathf.Abs(MinSpeed) / maxSpeed;
        if (inputZaxis < 0)
            thrust *= reverseThrustRatio;

        // Accelerate the sub according to thrust force.
        _rigidbody.AddRelativeForce(0f, 0f, thrust);
    }

    /** Apply an impact impulse vector to the sub's rigidbody. */
    private void ApplyImpact(Vector3 impactVector)
    {
        // Check if we're allowed to apply another impact yet.
        if (Time.time < _nextImpactTime)
            return;

        // Schedule the next possible impact.
        var t = Time.time;
        var dt = t - _lastImpactTime;
        _lastImpactTime = t;
        _nextImpactTime = t + MotionMinImpactInterval;

        // Apply an impact torque to the sub.
        Debug.Log(string.Format("IMPACT TRIGGERED: t = {0:N2}, delta = {1:N2}, next = {2:N2}", t, dt, _nextImpactTime));
        _rigidbody.AddTorque(impactVector * MotionScaleImpacts, ForceMode.VelocityChange);
    }

}
