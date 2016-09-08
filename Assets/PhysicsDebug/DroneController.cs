using UnityEngine;
using UnityEngine.Networking;

public class DroneController : MonoBehaviour
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
    public bool disableInput = false;

    /** Whether input to sub is overridden by the server. */
    public bool JoystickOverride;

    /** Whether input to sub can come from the pilot. */
    public bool JoystickPilot = true;

    public float inputXaxis;
    public float inputYaxis;
    public float inputZaxis;
    public float inputXaxis2;
    public float inputYaxis2;
    public bool isAutoStabilised;
    public bool IsPitchAlsoStabilised;
    public float Acceleration = 1.0f;
    public float yawSpeed = 400f;
    public float pitchSpeed = 100f;
    public float rollSpeed = 1500f;
    public float MaxSpeed = 200f;
    public float MinSpeed = -100f;
    public bool isAutoPilot = false;
    public bool isControlDecentMode = false;
    public bool isControlModeOverride = false;
    public bool isControlOverrideStandard = false;

    public float speed;
    public float stability;


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
    }

    void FixedUpdate()
    {
        //ApplySubForces();
        inputXaxis = Input.GetAxis("Horizontal");
        inputYaxis = -Input.GetAxis("Vertical");
        inputZaxis = Input.GetAxis("Throttle");

        ApplySubForces();
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Apply per-frame control forces to the sub's rigidbody. */
    public void ApplyForces()
    {
        
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

    /** Apply control forces to pilot a glider sub. */
    private void ApplyGliderForces()
    {
        // TODO: Implement.
        // For now, just pass through to the existing sub control logic.
        ApplySubForces();
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

    /** Apply standard control forces for the big sub. */
    private void ApplyStandardForces()
    {
        // Check if input has been disabled.
        if (disableInput)
            return;

        // Apply the orientation forces.
        _rigidbody.AddRelativeTorque(Vector3.left * (pitchSpeed * inputYaxis));
        _rigidbody.AddRelativeTorque(Vector3.forward * (rollSpeed * inputXaxis));
        //_rigidbody.AddTorque(Vector3.up * (yawSpeed * inputXaxis));


        // Adjust mix for pitch when doing a banking turn.
        //_rigidbody.AddRelativeTorque(Vector3.right * (yawSpeed * 0.5f * Mathf.Abs(-inputXaxis)));
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
        //const float stability = 0.3f * 0.010f;
        //const float speed = 2.0f * 0.010f;
        var predictedUp = Quaternion.AngleAxis(_rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / speed,
            _rigidbody.angularVelocity) * transform.up;

        var torqueVector = Vector3.Cross(predictedUp, Vector3.up);
        if (!IsPitchAlsoStabilised && !isControlDecentMode)
            torqueVector = Vector3.Project(torqueVector, transform.forward);

        _rigidbody.AddTorque(torqueVector * speed * speed);
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

}
