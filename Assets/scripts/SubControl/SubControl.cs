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
    public float inputXaxis3;
    [SyncVar]
    public float inputYaxis3;
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

    [SyncVar]
    public float MaxGliderAngle = 90f;
    [SyncVar]
    public float AbsoluteMaxAngularVel = 1.7f;
    [SyncVar]
    public bool oldPhysics = true;


    //private float SoftMaxGliderAngle = 90f;

    public float MaxAngularVelocity = 1.7f;

    public GameObject MotionBaseSub;
    public Rigidbody _motionRigidBody;


    //TODO Make these sync vars

    public AnimationCurve RollLimitCurve;
    public AnimationCurve PitchLimitCurve;
    public AnimationCurve YawLimitCurve;
    public AnimationCurve AngleLimitCurve;

    public AnimationCurve StabiliseCurve;

    //Vector3 localAngularVelocity;

    public bool TripPitch = false;
    public bool TripRoll = false;

    public float MaxGliderPitch;
    public float MinGliderPitch;
    public float MaxGliderRoll;
    public float MinGliderRoll;

    public float motionYawSpeed;

    public GameObject FakeMotionBase;
    public GameObject YawElement;

    public bool NerfStabiliser = true;
    public float currentStabiliseSpeed;
    public Vector3 AngularVel;

    public bool LaunchROV = false;

    public bool Collision = false;

    public Vector3 AngVel;
    public float sPitch;
    public float sRoll;

    public Vector3 newOrientation;

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

    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        if (serverUtils.IsGlider())
            ApplyGliderDefaults();
        else if (serverUtils.IsRov())
            ApplyRovDefaults();

    }


    // Public Methods
    // ------------------------------------------------------------

    /** Apply per-frame control forces to the sub's rigidbody. */
    public void ApplyForces()
    {
        // Check that the server is up and running.
        if (!serverUtils.IsReady())
            return;

        // Don't apply forces if vessel is being simulated.
        var movement = serverUtils.GetPlayerVesselMovement();
        if (movement && movement.Active)
            return;

        // Apply the appropriate control logic.
        if (serverUtils.IsGlider())
            ApplyGliderForces();
        else if (serverUtils.IsRov())
            ApplyRovForces();
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
        if (serverUtils.IsReady())
            serverUtils.MotionBaseData.MotionBaseYaw = YawElement.GetComponent<Rigidbody>().angularVelocity.y;
    }

    public Quaternion GetMotionBaseQuaternion()
    {
        Quaternion r;

        if(serverUtils.IsGlider())
        {
            r = MotionBaseSub.transform.rotation;
        }
        else
        {
            r = transform.rotation;
        }

        return r;
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Apply defaults that relate to the glider here */
    private void ApplyGliderDefaults()
    {
        _rigidbody.angularDrag = 2.0f;
        _rigidbody.mass = 0.2f;

        _motionRigidBody.angularDrag = 2.0f;
        _motionRigidBody.mass = 0.2f;

        pitchSpeed = 400f;
        rollSpeed = 400f;
        yawSpeed = 200;

        StabiliserSpeed = 20f;
        StabiliserStability = 1f;

        _motionRigidBody.centerOfMass = Vector3.zero;

        //_rigidbody.maxAngularVelocity = 0.0001f;

        serverUtils.MotionBaseData.MotionSlerpSpeed = 5.0f;

        oldPhysics = false;
    }

    /** Apply defaults that relate to the ROV here */
    private void ApplyRovDefaults()
    {
        _rigidbody.angularDrag = 2.0f;
        _rigidbody.mass = 0.2f;

        pitchSpeed = 400f;
        rollSpeed = 400f;
        yawSpeed = 400;

        StabiliserSpeed = 35f;
        StabiliserStability = 1f;

        serverUtils.ServerData.isControlDecentModeOnJoystick = true;

        Physics.gravity = new Vector3(0, -0.5F, 0);

        BowtieDeadzone = 0.6f;
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
        //TODO DELETE THIS
        //if(Input.GetKeyDown("space"))
        //{
        //    oldPhysics = !oldPhysics;
        //}

        //localAngularVelocity = MotionBaseSub.transform.InverseTransformDirection(_motionRigidBody.angularVelocity);

        //Quaternion YawlessMotionBaseQ;
        //YawlessMotionBaseQ.eulerAngles = new Vector3(MotionBaseSub.transform.rotation.x, 0f, MotionBaseSub.transform.rotation.z);
        //
        //MotionBaseSub.transform.rotation = YawlessMotionBaseQ;

        //MotionBaseSub.transform.rotation = 

        // Apply the orientation forces.
        //_rigidbody.AddRelativeTorque(Vector3.left * (pitchSpeed * inputYaxis));

        AbsoluteMaxAngularVel = Mathf.Clamp(AbsoluteMaxAngularVel, 0.1f, 3f);
        pitchSpeed = Mathf.Clamp(pitchSpeed, 0f, 1500f);
        motionYawSpeed = yawSpeed;
        motionYawSpeed = Mathf.Clamp(motionYawSpeed, 0f, 380f);

        MaxGliderAngle = Mathf.Clamp(MaxGliderAngle, Mathf.Min(serverUtils.MotionBaseData.MotionPitchMax, serverUtils.MotionBaseData.MotionRollMax, serverUtils.MotionBaseData.MotionPitchMin, serverUtils.MotionBaseData.MotionRollMin), 50f);
        //SoftMaxGliderAngle = MaxGliderAngle - AbsoluteMaxAngularVel;

        MaxAngularVelocity = (MaxGliderAngle / 90f) * AbsoluteMaxAngularVel;

        AngularVel = _motionRigidBody.angularVelocity;

        CalculateMaxAngles();

        if (oldPhysics == true)
        {
            GliderYawLogic();

            OldGliderRollLogic();

            OldGliderPitchLogic();

            GliderStabiliserSpeedNerf();
        }
        else
        {

            NerfStabiliser = false;
            //var localAngularVelocity = MotionBaseSub.transform.InverseTransformDirection(_motionRigidBody.angularVelocity);
            //
            //Quaternion YawlessMotionBaseQ = Quaternion.identity;
            //YawlessMotionBaseQ.eulerAngles = new Vector3(MotionBaseSub.transform.rotation.x, 0f, MotionBaseSub.transform.rotation.z);
            ////YawlessMotionBaseQ.SetEulerAngles(new Vector3(MotionBaseSub.transform.rotation.x, 0f, MotionBaseSub.transform.rotation.z);

            Vector3 NoYRot = new Vector3(MotionBaseSub.transform.rotation.eulerAngles.x, 0f, MotionBaseSub.transform.rotation.eulerAngles.z);

            MotionBaseSub.transform.eulerAngles = NoYRot;

            GliderYawLogic();

            GliderRollLogic();

            GliderPitchLogic();

            GliderStabiliserSpeedNerf();

            CheckPhysicsSubLimits();

            if(!serverUtils.MotionBaseData.DecoupleMotionBase)
            {
                this.transform.eulerAngles = new Vector3(MotionBaseSub.transform.eulerAngles.x, this.transform.eulerAngles.y, MotionBaseSub.transform.eulerAngles.z);
            }
        } 


        //if(_motionRigidBody.detectCollisions)
        //{
        //    Collision = true;
        //}

        _motionRigidBody.detectCollisions = false;

        if (serverUtils.MotionBaseData.DecoupleMotionBase)
        {
            _rigidbody.AddRelativeTorque((Vector3.forward * (rollSpeed * -inputXaxis)));
            _rigidbody.AddRelativeTorque(Vector3.left * (pitchSpeed * inputYaxis));

        }


        // Adjust mix for pitch when doing a banking turn.
        //_rigidbody.AddRelativeTorque(Vector3.right * (yawSpeed * 0.5f * Mathf.Abs(inputXaxis)));
        //_motionRigidBody.AddRelativeTorque(Vector3.right * (yawSpeed * 0.5f * Mathf.Abs(inputXaxis)));

        // Auto-stabilize the sub if desired, and if inputs are weak.
        if (inputYaxis < serverUtils.MotionBaseData.MotionStabiliserKicker && inputYaxis > -serverUtils.MotionBaseData.MotionStabiliserKicker)
        {
            if (inputXaxis < serverUtils.MotionBaseData.MotionStabiliserKicker && inputXaxis > -serverUtils.MotionBaseData.MotionStabiliserKicker)
                ApplyStabilizationForce();
        }

        // Apply thrust to move the sub forward or backwards.
        //ApplyThrustForce();

        //DEBUG STUFF
        if (MotionBaseSub.transform.localRotation.eulerAngles.z > 179.9f && MotionBaseSub.transform.localRotation.eulerAngles.z < 170.1f)
        {
            TripRoll = true;
            serverUtils.MotionBaseData.MotionHazard = true;
            Debug.Log("MotionHazard too much roll detected");
        }

        if (MotionBaseSub.transform.localRotation.eulerAngles.x > 89.9f && MotionBaseSub.transform.localRotation.eulerAngles.x < 270.1f)
        {
            TripPitch = true;
            serverUtils.MotionBaseData.MotionHazard = true;
            Debug.Log("MotionHazard too much pitch detected");
        }

        _rigidbody.angularVelocity = Vector3.ClampMagnitude(_rigidbody.angularVelocity, MaxAngularVelocity);

        _motionRigidBody.angularVelocity = Vector3.ClampMagnitude(_motionRigidBody.angularVelocity, MaxAngularVelocity);

        YawElement.GetComponent<Rigidbody>().angularVelocity = Vector3.ClampMagnitude(YawElement.GetComponent<Rigidbody>().angularVelocity, serverUtils.MotionBaseData.MotionYawMax / 10f);
    }

    private void ApplyRovForces()
    {
        //test for bowtie deadzone
        if (!(inputXaxis < BowtieDeadzone * Mathf.Abs(inputYaxis) && inputXaxis > -BowtieDeadzone * Mathf.Abs(inputYaxis)))
        {
            //out of deadzone
            _rigidbody.AddRelativeTorque((Vector3.forward * (rollSpeed * -inputXaxis)));
        }

        //test for bowtie deadzone
        if (!(inputYaxis < BowtieDeadzone * Mathf.Abs(inputXaxis) && inputYaxis > -BowtieDeadzone * Mathf.Abs(inputXaxis)))
        {
            //out of deadzone
            _rigidbody.AddRelativeTorque(Vector3.left * (pitchSpeed * inputYaxis));
        }


        //test for bowtie deadzone
        if (!(inputXaxis2 < BowtieDeadzone * Mathf.Abs(inputZaxis) && inputXaxis2 > -BowtieDeadzone * Mathf.Abs(inputZaxis)))
        {
            //out of deadzone
            _rigidbody.AddRelativeTorque(Vector3.up * (yawSpeed * inputXaxis2));
        }

        // Auto-stabilize the sub if desired.
        ApplyStabilizationForce();

        // Apply thrust to move the sub forward or backwards.
        ApplyROVThrustForce();

        if (LaunchROV)
        {
            _rigidbody.useGravity = true;
            StabiliserSpeed = 65f;
        }
    }

    private void CalculateMaxAngles()
    {
        var maxAngle = Mathf.Max(serverUtils.MotionBaseData.MotionPitchMax,
            Mathf.Abs(serverUtils.MotionBaseData.MotionPitchMin),
            serverUtils.MotionBaseData.MotionRollMax,
            Mathf.Abs(serverUtils.MotionBaseData.MotionRollMin));

        var ratio = MaxGliderAngle / maxAngle;

        MaxGliderPitch = Mathf.Abs(serverUtils.MotionBaseData.MotionPitchMax * ratio);
        MinGliderPitch = Mathf.Abs(serverUtils.MotionBaseData.MotionPitchMin * ratio);
        MaxGliderRoll = Mathf.Abs(serverUtils.MotionBaseData.MotionRollMax * ratio);
        MinGliderRoll = Mathf.Abs(serverUtils.MotionBaseData.MotionRollMin * ratio);

    }

    /* Roll with constraints */
    private void OldGliderRollLogic()
    {
        //test for bowtie deadzone
        if (inputXaxis < BowtieDeadzone * Mathf.Abs(inputYaxis) && inputXaxis > -BowtieDeadzone * Mathf.Abs(inputYaxis))
        {
            //fallin within deadzone. Go directly to jail, do not pass go.
            return;
        }

        //roll curve value
        var currentRoll = MotionBaseSub.transform.rotation.eulerAngles.z;
        if (currentRoll > 180f)
        {
            currentRoll = Mathf.Abs(currentRoll - 360f);
        }
        //var ScaleRoll = RollLimitCurve.Evaluate(currentRoll / MaxGliderAngle);
        //ScaleRoll = Mathf.Abs(ScaleRoll);

        var ScaleRoll = 0f;
        if (MotionBaseSub.transform.localRotation.z > 0)
        {
            ScaleRoll = RollLimitCurve.Evaluate(currentRoll / MaxGliderRoll);
        }
        else
        {
            ScaleRoll = RollLimitCurve.Evaluate(currentRoll / MinGliderRoll);
        }
        ScaleRoll = Mathf.Abs(ScaleRoll);

        //roll logiic
        if (inputXaxis > 0 && MotionBaseSub.transform.rotation.eulerAngles.z > 180f && MotionBaseSub.transform.rotation.eulerAngles.z < 360f)
        {
            if (serverUtils.MotionBaseData.DecoupleMotionBase)
            {
                //motion base is decoupled. Different physics is happening
            }
            else
            {
                _rigidbody.AddRelativeTorque((Vector3.forward * ((rollSpeed) * -inputXaxis)) * ScaleRoll);
                _rigidbody.angularVelocity = Vector3.ClampMagnitude(_rigidbody.angularVelocity, (MaxAngularVelocity * Mathf.Clamp(ScaleRoll, 0.4f, 1f)));
            }

            _motionRigidBody.AddRelativeTorque((Vector3.forward * ((rollSpeed) * -inputXaxis)) * ScaleRoll);
            _motionRigidBody.angularVelocity = Vector3.ClampMagnitude(_motionRigidBody.angularVelocity, (MaxAngularVelocity * Mathf.Clamp(ScaleRoll, 0.4f, 1f)));

        }
        else if (inputXaxis < 0 && MotionBaseSub.transform.rotation.eulerAngles.z > 0f && MotionBaseSub.transform.rotation.eulerAngles.z < 180f)
        {
            if (serverUtils.MotionBaseData.DecoupleMotionBase)
            {
                //motion base is decoupled. Different physics is happening
            }
            else
            {
                _rigidbody.AddRelativeTorque((Vector3.forward * ((rollSpeed) * -inputXaxis)) * ScaleRoll);
                _rigidbody.angularVelocity = Vector3.ClampMagnitude(_rigidbody.angularVelocity, (MaxAngularVelocity * Mathf.Clamp(ScaleRoll, 0.4f, 1f)));
            }

            _motionRigidBody.AddRelativeTorque((Vector3.forward * ((rollSpeed) * -inputXaxis)) * ScaleRoll);
            _motionRigidBody.angularVelocity = Vector3.ClampMagnitude(_motionRigidBody.angularVelocity, (MaxAngularVelocity * Mathf.Clamp(ScaleRoll, 0.4f, 1f)));
        }
        else
        {
            if (serverUtils.MotionBaseData.DecoupleMotionBase)
            {
                //motion base is decoupled. Different physics is happening
            }
            else
            {
                _rigidbody.AddRelativeTorque((Vector3.forward * (rollSpeed * -inputXaxis)));
            }

            _motionRigidBody.AddRelativeTorque((Vector3.forward * (rollSpeed * -inputXaxis)));
        }
    }

    /* Roll with constraints */
    private void GliderRollLogic()
    {
        //test for bowtie deadzone
        if (inputXaxis < BowtieDeadzone * Mathf.Abs(inputYaxis) && inputXaxis > -BowtieDeadzone * Mathf.Abs(inputYaxis))
        {
            //fallin within deadzone. Go directly to jail, do not pass go.
            return;
        }

        //roll curve value
        var currentRoll = MotionBaseSub.transform.rotation.eulerAngles.z;
        if (currentRoll > 180f)
        {
            currentRoll = Mathf.Abs(currentRoll - 360f);
        }
        //var ScaleRoll = RollLimitCurve.Evaluate(currentRoll / MaxGliderAngle);
        //ScaleRoll = Mathf.Abs(ScaleRoll);

        var ScaleRoll = 0f;
        if (MotionBaseSub.transform.localRotation.z > 0)
        {
            ScaleRoll = AngleLimitCurve.Evaluate(currentRoll / MaxGliderRoll);
        }
        else
        {
            ScaleRoll = AngleLimitCurve.Evaluate(currentRoll / MinGliderRoll);
        }
        ScaleRoll = Mathf.Abs(ScaleRoll);

        //roll logiic
        if (inputXaxis > 0 && MotionBaseSub.transform.rotation.eulerAngles.z > 180f && MotionBaseSub.transform.rotation.eulerAngles.z < 360f)
        {
            if (serverUtils.MotionBaseData.DecoupleMotionBase)
            {
                //motion base is decoupled. Different physics is happening
            }
            else
            {
                _rigidbody.AddRelativeTorque((Vector3.forward * ((rollSpeed) * -inputXaxis)) * ScaleRoll);
                _rigidbody.angularVelocity = Vector3.ClampMagnitude(_rigidbody.angularVelocity, (MaxAngularVelocity * Mathf.Clamp(ScaleRoll, 0.4f, 1f)));
                clampRollVel(ScaleRoll);
            }

            _motionRigidBody.AddTorque((Vector3.forward * ((rollSpeed) * -inputXaxis)) * ScaleRoll);
            //_motionRigidBody.angularVelocity = Vector3.ClampMagnitude(_motionRigidBody.angularVelocity, (MaxAngularVelocity * Mathf.Clamp(ScaleRoll, 0.4f, 1f)));
            clampRollVel(ScaleRoll);

        }
        else if (inputXaxis < 0 && MotionBaseSub.transform.rotation.eulerAngles.z > 0f && MotionBaseSub.transform.rotation.eulerAngles.z < 180f)
        {
            if (serverUtils.MotionBaseData.DecoupleMotionBase)
            {
                //motion base is decoupled. Different physics is happening
            }
            else
            {
                _rigidbody.AddRelativeTorque((Vector3.forward * ((rollSpeed) * -inputXaxis)) * ScaleRoll);
                _rigidbody.angularVelocity = Vector3.ClampMagnitude(_rigidbody.angularVelocity, (MaxAngularVelocity * Mathf.Clamp(ScaleRoll, 0.4f, 1f)));
                clampRollVel(ScaleRoll);
            }

            _motionRigidBody.AddTorque((Vector3.forward * ((rollSpeed) * -inputXaxis)) * ScaleRoll);
            //_motionRigidBody.angularVelocity = Vector3.ClampMagnitude(_motionRigidBody.angularVelocity, (MaxAngularVelocity * Mathf.Clamp(ScaleRoll, 0.4f, 1f)));
            clampRollVel(ScaleRoll);
        }
        else
        {
            if (serverUtils.MotionBaseData.DecoupleMotionBase)
            {
                //motion base is decoupled. Different physics is happening
            }
            else
            {
                _rigidbody.AddRelativeTorque((Vector3.forward * (rollSpeed * -inputXaxis)));
            }

            _motionRigidBody.AddTorque((Vector3.forward * (rollSpeed * -inputXaxis)));
            //clampRollVel(1f);
        }
    }

    private void OldGliderPitchLogic()
    {
        //test for bowtie deadzone
        if (inputYaxis < BowtieDeadzone * Mathf.Abs(inputXaxis) && inputYaxis > -BowtieDeadzone * Mathf.Abs(inputXaxis))
        {
            //fallin within deadzone. Go directly to jail, do not pass go.
            return;
        }

        //pitch curve value
        var currentPitch = MotionBaseSub.transform.rotation.eulerAngles.x;
        if (currentPitch > 180f)
        {
            currentPitch = Mathf.Abs(currentPitch - 360f);
        }

        var ScalePitch = 0f;
        if (MotionBaseSub.transform.localRotation.x > 0)
        {
            ScalePitch = PitchLimitCurve.Evaluate(currentPitch / Mathf.Clamp(MaxGliderPitch, 0f, (90f - MaxAngularVelocity * 1f)));
        }
        else
        {
            ScalePitch = PitchLimitCurve.Evaluate(currentPitch / Mathf.Clamp(MinGliderPitch, 0f, (90f - MaxAngularVelocity * 1f)));
        }
        //var ScalePitch = PitchLimitCurve.Evaluate(currentPitch / Mathf.Clamp(MaxGliderAngle, 0f, (90f - MaxAngularVelocity * 1f)));
        ScalePitch = Mathf.Abs(ScalePitch);

        //pitch logic
        if (inputYaxis > 0 && MotionBaseSub.transform.rotation.eulerAngles.x > 180f && MotionBaseSub.transform.rotation.eulerAngles.x < 360f)
        {
            if (serverUtils.MotionBaseData.DecoupleMotionBase)
            {
                //motion base is decoupled. Different physics is happening
            }
            else
            {
                _rigidbody.AddRelativeTorque((Vector3.left * ((pitchSpeed) * inputYaxis)) * ScalePitch);
                _rigidbody.angularVelocity = Vector3.ClampMagnitude(_rigidbody.angularVelocity, (MaxAngularVelocity * Mathf.Clamp(ScalePitch, 0.4f, 1f)));
            }


            _motionRigidBody.AddRelativeTorque((Vector3.left * ((pitchSpeed) * inputYaxis)) * ScalePitch);
            _motionRigidBody.angularVelocity = Vector3.ClampMagnitude(_motionRigidBody.angularVelocity, (MaxAngularVelocity * Mathf.Clamp(ScalePitch, 0.4f, 1f)));
        }
        else if (inputYaxis < 0 && MotionBaseSub.transform.rotation.eulerAngles.x > 0f && MotionBaseSub.transform.rotation.eulerAngles.x < 180f)
        {
            if (serverUtils.MotionBaseData.DecoupleMotionBase)
            {
                //motion base is decoupled. Different physics is happening
            }
            else
            {
                _rigidbody.AddRelativeTorque((Vector3.left * ((pitchSpeed) * inputYaxis)) * ScalePitch);
                _rigidbody.angularVelocity = Vector3.ClampMagnitude(_rigidbody.angularVelocity, (MaxAngularVelocity * Mathf.Clamp(ScalePitch, 0.4f, 1f)));
            }

            _motionRigidBody.AddRelativeTorque((Vector3.left * ((pitchSpeed) * inputYaxis)) * ScalePitch);
            _motionRigidBody.angularVelocity = Vector3.ClampMagnitude(_motionRigidBody.angularVelocity, (MaxAngularVelocity * Mathf.Clamp(ScalePitch, 0.4f, 1f)));
        }
        else
        {
            if (serverUtils.MotionBaseData.DecoupleMotionBase)
            {
                //motion base is decoupled. Different physics is happening
            }
            else
            {
                _rigidbody.AddRelativeTorque(Vector3.left * (pitchSpeed * inputYaxis));
            }

            _motionRigidBody.AddRelativeTorque(Vector3.left * (pitchSpeed * inputYaxis));
        }
    }

    /* Pitch with constraints */
    private void GliderPitchLogic()
    {
        //test for bowtie deadzone
        if (inputYaxis < BowtieDeadzone * Mathf.Abs(inputXaxis) && inputYaxis > -BowtieDeadzone * Mathf.Abs(inputXaxis))
        {
            //fallin within deadzone. Go directly to jail, do not pass go.
            return;
        }

        //pitch curve value
        var currentPitch = MotionBaseSub.transform.rotation.eulerAngles.x;
        if (currentPitch > 180f)
        {
            currentPitch = Mathf.Abs(currentPitch - 360f);
        }

        var ScalePitch = 0f;
        if (MotionBaseSub.transform.localRotation.x > 0)
        {
            ScalePitch = AngleLimitCurve.Evaluate((currentPitch / MaxGliderPitch));
        }
        else
        {
            ScalePitch = AngleLimitCurve.Evaluate((currentPitch / MaxGliderPitch));
        }
        //var ScalePitch = PitchLimitCurve.Evaluate(currentPitch / Mathf.Clamp(MaxGliderAngle, 0f, (90f - MaxAngularVelocity * 1f)));
        ScalePitch = Mathf.Abs(ScalePitch);

        //pitch logic
        if (inputYaxis > 0 && MotionBaseSub.transform.localRotation.eulerAngles.x > 180f && MotionBaseSub.transform.localRotation.eulerAngles.x < 360f)
        {
            if (serverUtils.MotionBaseData.DecoupleMotionBase)
            {
                //motion base is decoupled. Different physics is happening
            }
            else
            {
                _rigidbody.AddRelativeTorque((Vector3.left * ((pitchSpeed) * inputYaxis)) * ScalePitch);
                _rigidbody.angularVelocity = Vector3.ClampMagnitude(_rigidbody.angularVelocity, (MaxAngularVelocity * Mathf.Clamp(ScalePitch, 0.4f, 1f)));
                ClampPitchVel(ScalePitch);
            }


            _motionRigidBody.AddTorque((Vector3.left * ((pitchSpeed) * inputYaxis)) * ScalePitch);
            //_motionRigidBody.angularVelocity = Vector3.ClampMagnitude(_motionRigidBody.angularVelocity, (MaxAngularVelocity * Mathf.Clamp(ScalePitch, 0.4f, 1f)));
            ClampPitchVel(ScalePitch);
        }
        else if (inputYaxis < 0 && MotionBaseSub.transform.localRotation.eulerAngles.x > 0f && MotionBaseSub.transform.localRotation.eulerAngles.x < 180f)
        {
            if (serverUtils.MotionBaseData.DecoupleMotionBase)
            {
                //motion base is decoupled. Different physics is happening
            }
            else
            {
                _rigidbody.AddRelativeTorque((Vector3.left * ((pitchSpeed) * inputYaxis)) * ScalePitch);
                _rigidbody.angularVelocity = Vector3.ClampMagnitude(_rigidbody.angularVelocity, (MaxAngularVelocity * Mathf.Clamp(ScalePitch, 0.4f, 1f)));
                ClampPitchVel(ScalePitch);
            }

            _motionRigidBody.AddTorque((Vector3.left * ((pitchSpeed) * inputYaxis)) * ScalePitch);
            //_motionRigidBody.angularVelocity = Vector3.ClampMagnitude(_motionRigidBody.angularVelocity, (MaxAngularVelocity * Mathf.Clamp(ScalePitch, 0.4f, 1f)));
            ClampPitchVel(ScalePitch);
        }
        else
        {
            if (serverUtils.MotionBaseData.DecoupleMotionBase)
            {
                //motion base is decoupled. Different physics is happening
            }
            else
            {
                _rigidbody.AddRelativeTorque(Vector3.left * (pitchSpeed * inputYaxis));
            }

            _motionRigidBody.AddTorque(Vector3.left * (pitchSpeed * inputYaxis));
            //ClampPitchVel(1f);
        }
    }

    private void GliderYawLogic()
    {
        //banking yaw doesn't make a lot of sense on gliders. avoid if possible
        //_rigidbody.AddRelativeTorque(Vector3.forward * (yawSpeed * inputXaxis2));
        //_motionRigidBody.AddRelativeTorque(Vector3.forward * (yawSpeed * inputXaxis2));
        YawElement.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.up * (yawSpeed * inputXaxis2));

        _rigidbody.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.up * (yawSpeed * inputXaxis2));

        if (serverUtils.MotionBaseData.DecoupleMotionBase)
        {
            //motion base is decoupled. Different physics is happening
        }
        else
        {
            //Quaternion SubYawQ;
            //SubYawQ = Quaternion.Euler(new Vector3(transform.rotation.x, YawElement.transform.rotation.y, transform.rotation.z));
            //this.transform.rotation = SubYawQ;
        }
    }

    private void GliderStabiliserSpeedNerf()
    {
        //StabiliseCurve

        //if nerf stabiliser is enabled we wwill nerf the stabiliser based on the input put through an animation curve
        if (NerfStabiliser)
        {
            //calculate the most significant input, this is the one to nerf by
            var SignificantInput = Mathf.Max(Mathf.Abs(inputYaxis), Mathf.Abs(inputXaxis));

            currentStabiliseSpeed = StabiliseCurve.Evaluate(SignificantInput) * StabiliserSpeed;
        }
        else
        {
            currentStabiliseSpeed = StabiliserSpeed;
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

        var motionRoll = 0f;
        var motionPitch = 0f;
        if (serverUtils.IsGlider())
        {
            motionRoll = MotionBaseSub.transform.eulerAngles.z;
            motionPitch = MotionBaseSub.transform.eulerAngles.x;
        }
        else
        {
            currentStabiliseSpeed = StabiliserSpeed;
        }


            // If roll is almost right, stop adjusting (PID dampening todo here?)
            if (roll > MinRoll && roll < MaxRoll)
            AutoStabilize();
        else if (pitch > MinPitch && roll < MaxRoll)
            AutoStabilize();

        if (serverUtils.IsGlider())
        {
            if (motionRoll > MinRoll && motionRoll < MaxRoll)
                MotionAutoStabilize();
            else if (motionPitch > MinPitch && motionRoll < MaxRoll)
                MotionAutoStabilize();
        }
    }

    /** Apply autostabilization to the sub's rigidbody. */
    private void AutoStabilize()
    {
        // TODO: Factor these numbers into constants.
        //const float stability = 0.3f * 10f;
        //const float speed = 2.0f * 10f;
        var predictedUp = Quaternion.AngleAxis(_rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * StabiliserStability / currentStabiliseSpeed,
            _rigidbody.angularVelocity) * transform.up;

        var torqueVector = Vector3.Cross(predictedUp, Vector3.up);
        if (!IsPitchAlsoStabilised && !isControlDecentMode)
            torqueVector = Vector3.Project(torqueVector, transform.forward);

        _rigidbody.AddTorque(torqueVector * currentStabiliseSpeed * currentStabiliseSpeed);
    }

    /** Apply autostabilization to the sub's rigidbody. */
    private void MotionAutoStabilize()
    {
        // TODO: Factor these numbers into constants.
        //const float stability = 0.3f * 10f;
        //const float speed = 2.0f * 10f;
        var predictedUp = Quaternion.AngleAxis(_motionRigidBody.angularVelocity.magnitude * Mathf.Rad2Deg * StabiliserStability / currentStabiliseSpeed,
            _motionRigidBody.angularVelocity) * MotionBaseSub.transform.up;

        var torqueVector = Vector3.Cross(predictedUp, Vector3.up);
        if (!IsPitchAlsoStabilised && !isControlDecentMode)
            torqueVector = Vector3.Project(torqueVector, MotionBaseSub.transform.forward);

        _motionRigidBody.AddTorque(torqueVector * currentStabiliseSpeed * currentStabiliseSpeed);
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

    private void ApplyROVThrustForce()
    {
        // Check if input has been disabled.
        if (disableInput)
            return;

        // Check that sub has a non-zero max speed.
        if (Mathf.Approximately(MaxSpeed, 0))
            return;

        var ThrustInput = inputZaxis;
        ThrustInput = (ThrustInput+1f) /2f;

        // Compute drag based on max. acceleration and desired terminal velocity.
        // See http://forum.unity3d.com/threads/terminal-velocity.34667/ for more info.
        var maxAcceleration = Acceleration * AccelerationScale;
        var maxSpeed = Mathf.Abs(MaxSpeed);
        var maxThrust = maxAcceleration * _rigidbody.mass;
        var idealDrag = maxAcceleration / maxSpeed;
        _rigidbody.drag = idealDrag / (idealDrag * Time.fixedDeltaTime + 1);

        // Determine the amount of thrust force to apply based on input.
        var thrust = ThrustInput * maxThrust;
        var reverseThrustRatio = Mathf.Abs(MinSpeed) / maxSpeed;
        if (ThrustInput < 0)
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
        _motionRigidBody.AddTorque(impactVector * MotionScaleImpacts, ForceMode.VelocityChange);
    }

    void clampRollVel(float ScaleRoll)
    {
        AngVel = _motionRigidBody.angularVelocity;
        sRoll = ScaleRoll;

        if (_motionRigidBody.angularVelocity.z > MaxAngularVelocity * ScaleRoll)
        {
            _motionRigidBody.angularVelocity = new Vector3(_motionRigidBody.angularVelocity.x, 0f, (MaxAngularVelocity * ScaleRoll)); // * Mathf.Clamp(ScaleRoll, 0.4f, 1f)));
        }

        if (_motionRigidBody.angularVelocity.z < -(MaxAngularVelocity * ScaleRoll))
        {
            _motionRigidBody.angularVelocity = new Vector3(_motionRigidBody.angularVelocity.x, 0f, -((MaxAngularVelocity * ScaleRoll))); // * Mathf.Clamp(ScaleRoll, 0.4f, 1f))));
        }
        
        //if (_rigidbody.angularVelocity.z > MaxAngularVelocity * ScaleRoll)
        //{
        //    _rigidbody.angularVelocity = new Vector3(_rigidbody.angularVelocity.x, _rigidbody.angularVelocity.y, (MaxAngularVelocity * ScaleRoll)); // * Mathf.Clamp(ScaleRoll, 0.4f, 1f)));
        //}
        //
        //if (_rigidbody.angularVelocity.z < -(MaxAngularVelocity * ScaleRoll))
        //{
        //    _rigidbody.angularVelocity = new Vector3(_rigidbody.angularVelocity.x, _rigidbody.angularVelocity.y, -((MaxAngularVelocity * ScaleRoll))); // * Mathf.Clamp(ScaleRoll, 0.4f, 1f))));
        //}
    }

    void ClampPitchVel(float ScalePitch)
    {
        AngVel = _motionRigidBody.angularVelocity;
        sPitch = ScalePitch;
        //if (_rigidbody.angularVelocity.x > MaxAngularVelocity * ScalePitch)
        //{
        //    _rigidbody.angularVelocity = new Vector3((MaxAngularVelocity * ScalePitch), _rigidbody.angularVelocity.y, _rigidbody.angularVelocity.z);
        //}
        //
        //if (_rigidbody.angularVelocity.x < -(MaxAngularVelocity * ScalePitch))
        //{
        //    _rigidbody.angularVelocity = new Vector3(-(MaxAngularVelocity * ScalePitch), _rigidbody.angularVelocity.y, _rigidbody.angularVelocity.z);
        //}

        if (_motionRigidBody.angularVelocity.x > MaxAngularVelocity * ScalePitch)
        {
            _motionRigidBody.angularVelocity = new Vector3((MaxAngularVelocity * ScalePitch), 0f, _motionRigidBody.angularVelocity.z);
        }
        
        if (_motionRigidBody.angularVelocity.x < -(MaxAngularVelocity * ScalePitch))
        {
            _motionRigidBody.angularVelocity = new Vector3(-((MaxAngularVelocity * ScalePitch)), 0f, _motionRigidBody.angularVelocity.z);
        }

    }

    void CheckPhysicsSubLimits()
    {
        newOrientation = new Vector3(MotionBaseSub.transform.localRotation.eulerAngles.x, MotionBaseSub.transform.localRotation.eulerAngles.y, MotionBaseSub.transform.localRotation.eulerAngles.z);

        if(newOrientation.x > 180)
        {
            newOrientation.x -= 360;
        }

        if (newOrientation.z > 180)
        {
            newOrientation.z -= 360;
        }

        if (newOrientation.x > MaxGliderPitch)
        {
            newOrientation.x = MaxGliderPitch;
        }

        if (newOrientation.x < -MinGliderPitch)
        {
            newOrientation.x = -MinGliderPitch;
        }

        if(newOrientation.z > MaxGliderRoll)
        {
            newOrientation.z = MaxGliderRoll;
        }

        if (newOrientation.z < -MinGliderRoll)
        {
            newOrientation.z = -MinGliderRoll;
        }

        MotionBaseSub.transform.localEulerAngles = newOrientation;

    }
}
