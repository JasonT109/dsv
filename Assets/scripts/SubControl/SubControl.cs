using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using Meg.Networking;

public class SubControl : NetworkBehaviour 
{

    // Constants
    // ------------------------------------------------------------

    /** Scaling factor for converting joystick input into forward thrust. */
    private const float ForwardThrust = 50f * 15.0f;

    /** Angle thresholds used to determine when auto-stabilization is appropriate. */
    private const float MinRoll = 0.09f;
    private const float MaxRoll = 364.91f;
    private const float MinPitch = 0.09f;

    /** Scaling factor for converting joystick input into thrust forces. */
    private const float InputToThrust = 0.0337f;

    /** Minimum input level required to apply dynamic drag. */
    private const float ThrustInputThreshold = 0.01f;


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
	public float MotionBasePitch;
	[SyncVar]
	public float MotionBaseYaw;
	[SyncVar]
	public float MotionBaseRoll;
	[SyncVar]
	public float MotionDampen;
	[SyncVar] 
	public bool MotionSafety = true;
	[SyncVar] 
	public bool MotionHazard = false;
	[SyncVar] 
	public float MotionSlerpSpeed = 2f;
	[SyncVar] 
	public float MotionHazardSensitivity = 15f;
	[SyncVar] 
	public bool MotionHazardEnabled = true;
	[SyncVar] 
	public float MotionScaleImpacts = 1.0f;
	[SyncVar] 
	public float MotionMinImpactInterval = 0.75f;


    // Members
    // ------------------------------------------------------------

    /** The rigidbody that's being controlled. */
    private Rigidbody _rigidbody;

    /** Timestamp of last physics impact event. */
    private float _lastImpactTime;

    /** Timestamp for next possible physics impact event. */
    private float _nextImpactTime;

    /** Current forward thrust state. */
    private float _thrust;


    // Unity Methods
    // ------------------------------------------------------------

    /** Preinitialization. */
    private void Awake() 
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = Vector3.zero;

        MinSpeed = -(0.5f * MaxSpeed);
        JoystickOverride = false;
	}


    // Public Methods
    // ------------------------------------------------------------

    /** Apply per-frame control forces to the sub's rigidbody. */
    public void ApplyControlForces()
    {
        // Don't apply forces if vessel is being simulated.
        var movement = serverUtils.GetPlayerVesselMovement();
        if (movement && movement.Active)
            return;

        // Apply the appropriate control logic.
        if (isControlDecentMode)
            ApplyDescentControlForces();
        else
            ApplyStandardControlForces();
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


    // Private Methods
    // ------------------------------------------------------------

    /** Apply standard control forces for the big sub. */
    private void ApplyStandardControlForces()
	{
        if (!disableInput)
		{
            // Apply the orientation forces.
            _rigidbody.AddRelativeTorque(Vector3.left * (pitchSpeed * inputYaxis));
            _rigidbody.AddRelativeTorque(Vector3.forward * (rollSpeed * inputXaxis));
            _rigidbody.AddRelativeTorque(Vector3.up * (yawSpeed * inputXaxis));

            // Adjust mix for pitch when doing a banking turn.
            _rigidbody.AddRelativeTorque(Vector3.right * (yawSpeed * 0.5f * Mathf.Abs(inputXaxis)));
		}

        ApplyStabilization();
        ApplyThrustControl();
	}

    /** Apply control forces while in descent mode for the big sub. */
    private void ApplyDescentControlForces()
	{
        if (!disableInput)
		{
            // Apply the orientation forces.
            _rigidbody.AddTorque(Vector3.up * (yawSpeed * inputXaxis));
            _rigidbody.AddRelativeTorque(Vector3.left * (pitchSpeed * inputYaxis));
		}

        ApplyStabilization();
		ApplyThrustControl();
	}

    /** Update auto-stabilization logic. */
    private void ApplyStabilization()
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
        const float stability = 0.3f * 10f;
        const float speed = 2.0f * 10f;
        var predictedUp = Quaternion.AngleAxis(_rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / speed,
            _rigidbody.angularVelocity) * transform.up;

        var torqueVector = Vector3.Cross(predictedUp, Vector3.up);
        if (!IsPitchAlsoStabilised && !isControlDecentMode)
            torqueVector = Vector3.Project(torqueVector, transform.forward);

        _rigidbody.AddTorque(torqueVector * speed * speed);
    }

    /** Apply thrust forces to the sub's rigidbody. */
    public void ApplyThrustControl()
	{
		var currentThrust = _thrust;
        var scale = (Acceleration / 100f);
        var thrust = inputZaxis * InputToThrust * MaxSpeed;

        if (Mathf.Abs(inputZaxis) > ThrustInputThreshold)
            _rigidbody.drag = scale / 2;
		else
			_rigidbody.drag = 0.5f;

		if (isControlDecentMode)
		{
			_thrust = thrust / 2f;
			currentThrust *= ForwardThrust;
            _rigidbody.AddRelativeForce(0f, -currentThrust * Time.deltaTime * scale, 0f);
		}
		else
		{
			if (inputZaxis > 0)
				_thrust = thrust;
			else
				_thrust = thrust / 2f;

			currentThrust *= ForwardThrust;
            _rigidbody.AddRelativeForce(0f, 0f, currentThrust * Time.deltaTime * scale);
		}
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
