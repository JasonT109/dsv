using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class serverData : NetworkBehaviour
{
    #region SyncVars
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
    public bool IsJoystickSwapped;
    [SyncVar]
    public float depth;
    [SyncVar]
    public float pressure;
    [SyncVar]
    public float heading;
    [SyncVar]
    public float pitchAngle;
    [SyncVar]
    public float yawAngle;
    [SyncVar]
    public float rollAngle;
    [SyncVar]
    public float velocity;
    [SyncVar]
    public float dueTimeHours;
    [SyncVar]
    public float dueTimeMins;
    [SyncVar]
    public float dueTimeSecs;
    [SyncVar]
    public float battery;
    [SyncVar]
    public float Co2;
    [SyncVar]
    public float waterTemp;
    [SyncVar]
    public float oxygen;
    [SyncVar]
    public string pilot;
    [SyncVar]
    public float verticalVelocity;
    [SyncVar]
    public float horizontalVelocity;
    [SyncVar]
    public float floorDistance;
    [SyncVar]
    public float diveTimeHours;
    [SyncVar]
    public float diveTimeMins;
    [SyncVar]
    public float diveTimeSecs;
    #endregion
    #region PublicVars

    //public float descent;
    public float[] batteries = new float[7];
    public float[] oxygenTanks = new float[7];
    public float thrust = 1.0f;
    public float yawSpeed = 0.1f;
    public float pitchSpeed = 0.1f;
    public float rollSpeed = 0.1f;
    public GameObject[] players;
    public bool disableInput = false;

    private Vector3 rotation;
    #endregion
    #region PrivateVars
    private float bankAmount = 1.0f;
    private Vector3 bankAxis = new Vector3(0F, 0F, -1F);
    private float currentThrust = 0.0f;
    private Rigidbody rb;
    private float rollResult;
    private float pitchResult;
    private float yawResult;
    private float bank;
    #endregion
    #region PublicMethods
    public void OnBatterySliderChanged(int bank, float newValues)
    {
        switch (bank)
        {
            case 0:
                gameObject.GetComponent<batteryData>().bank1 = newValues;
                break;
            case 1:
                gameObject.GetComponent<batteryData>().bank2 = newValues;
                break;
            case 2:
                gameObject.GetComponent<batteryData>().bank3 = newValues;
                break;
            case 3:
                gameObject.GetComponent<batteryData>().bank4 = newValues;
                break;
            case 4:
                gameObject.GetComponent<batteryData>().bank5 = newValues;
                break;
            case 5:
                gameObject.GetComponent<batteryData>().bank6 = newValues;
                break;
            case 6:
                gameObject.GetComponent<batteryData>().bank7 = newValues;
                break;
        }
    }

    public void OnOxygenSliderChanged(int tank, float newValues)
    {
        switch (tank)
        {
            case 0:
                gameObject.GetComponent<oxygenData>().oxygenTank1 = newValues;
                break;
            case 1:
                gameObject.GetComponent<oxygenData>().oxygenTank2 = newValues;
                break;
            case 2:
                gameObject.GetComponent<oxygenData>().oxygenTank3 = newValues;
                break;
            case 3:
                gameObject.GetComponent<oxygenData>().oxygenTank4 = newValues;
                break;
            case 4:
                gameObject.GetComponent<oxygenData>().oxygenTank5 = newValues;
                break;
            case 5:
                gameObject.GetComponent<oxygenData>().oxygenTank6 = newValues;
                break;
            case 6:
                gameObject.GetComponent<oxygenData>().oxygenTank7 = newValues;
                break;
        }
    }

    public void OnValueChanged(string valueName, float newValue)
    {
        switch (valueName)
        {
            case "depth":
                transform.position = new Vector3(transform.position.x, -newValue, transform.position.z);
                depth = newValue;
                break;
            case "Co2":
                Co2 = newValue;
                break;
            case "battery":
                battery = newValue;
                break;
            case "dueTimeHours":
                dueTimeHours = newValue;
                diveTimeHours = newValue;
                break;
            case "dueTimeMins":
                dueTimeMins = newValue;
                diveTimeMins = newValue;
                break;
            case "dueTimeSecs":
                dueTimeSecs = newValue;
                diveTimeSecs = newValue;
                break;
            case "diveTimeHours":
                diveTimeHours = newValue;
                break;
            case "diveTimeMins":
                diveTimeMins = newValue;
                break;
            case "diveTimeSecs":
                diveTimeSecs = newValue;
                break;
            case "waterTemp":
                waterTemp = newValue;
                break;
            case "b1":
                gameObject.GetComponent<batteryData>().bank1 = newValue;
                break;
            case "b2":
                gameObject.GetComponent<batteryData>().bank2 = newValue;
                break;
            case "b3":
                gameObject.GetComponent<batteryData>().bank3 = newValue;
                break;
            case "b4":
                gameObject.GetComponent<batteryData>().bank4 = newValue;
                break;
            case "b5":
                gameObject.GetComponent<batteryData>().bank5 = newValue;
                break;
            case "b6":
                gameObject.GetComponent<batteryData>().bank6 = newValue;
                break;
            case "b7":
                gameObject.GetComponent<batteryData>().bank7 = newValue;
                break;
            case "o1":
                gameObject.GetComponent<oxygenData>().oxygenTank1 = newValue;
                break;
            case "o2":
                gameObject.GetComponent<oxygenData>().oxygenTank2 = newValue;
                break;
            case "o3":
                gameObject.GetComponent<oxygenData>().oxygenTank3 = newValue;
                break;
            case "o4":
                gameObject.GetComponent<oxygenData>().oxygenTank4 = newValue;
                break;
            case "o5":
                gameObject.GetComponent<oxygenData>().oxygenTank5 = newValue;
                break;
            case "o6":
                gameObject.GetComponent<oxygenData>().oxygenTank6 = newValue;
                break;
            case "o7":
                gameObject.GetComponent<oxygenData>().oxygenTank7 = newValue;
                break;
            case "error_bilgeLeak":
                gameObject.GetComponent<errorData>().error_bilgeLeak = newValue;
                break;
            case "error_batteryLeak":
                gameObject.GetComponent<errorData>().error_batteryLeak = newValue;
                break;
            case "error_electricLeak":
                gameObject.GetComponent<errorData>().error_electricLeak = newValue;
                break;
            case "error_oxygenExt":
                gameObject.GetComponent<errorData>().error_oxygenExt = newValue;
                break;
            case "error_vhf":
                gameObject.GetComponent<errorData>().error_vhf = newValue;
                break;
            case "error_forwardSonar":
                gameObject.GetComponent<errorData>().error_forwardSonar = newValue;
                break;
            case "error_depthSonar":
                gameObject.GetComponent<errorData>().error_depthSonar = newValue;
                break;
            case "error_doppler":
                gameObject.GetComponent<errorData>().error_doppler = newValue;
                break;
            case "error_gps":
                gameObject.GetComponent<errorData>().error_gps = newValue;
                break;
            case "error_cpu":
                gameObject.GetComponent<errorData>().error_cpu = newValue;
                break;
            case "error_vidhd":
                gameObject.GetComponent<errorData>().error_vidhd = newValue;
                break;
            case "error_datahd":
                gameObject.GetComponent<errorData>().error_datahd = newValue;
                break;
            case "error_tow":
                gameObject.GetComponent<errorData>().error_tow = newValue;
                break;
            case "error_radar":
                gameObject.GetComponent<errorData>().error_radar = newValue;
                break;
            case "error_sternLights":
                gameObject.GetComponent<errorData>().error_sternLights = newValue;
                break;
            case "error_bowLights":
                gameObject.GetComponent<errorData>().error_bowLights = newValue;
                break;
            case "error_portLights":
                gameObject.GetComponent<errorData>().error_portLights = newValue;
                break;
            case "error_bowThruster":
                gameObject.GetComponent<errorData>().error_bowThruster = newValue;
                break;
            case "error_hyrdaulicRes":
                gameObject.GetComponent<errorData>().error_hyrdaulicRes = newValue;
                break;
            case "error_starboardLights":
                gameObject.GetComponent<errorData>().error_starboardLights = newValue;
                break;
            case "error_runningLights":
                gameObject.GetComponent<errorData>().error_runningLights = newValue;
                break;
            case "error_ballastTank":
                gameObject.GetComponent<errorData>().error_ballastTank = newValue;
                break;
            case "error_hydraulicPump":
                gameObject.GetComponent<errorData>().error_hydraulicPump = newValue;
                break;
            case "error_oxygenPump":
                gameObject.GetComponent<errorData>().error_oxygenPump = newValue;
                break;
            case "inputXaxis":
                inputXaxis = newValue;
                break;
            case "inputYaxis":
                inputYaxis = newValue;
                break;
            case "inputZaxis":
                inputZaxis = newValue;
                break;
            case "inputXaxis2":
                inputXaxis2 = newValue;
                break;
            case "inputYaxis2":
                inputYaxis2 = newValue;
                break;
            case "pitchAngle":
                Quaternion qPitch = Quaternion.Euler(newValue, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                transform.rotation = qPitch;
                break;
            case "yawAngle":
                Quaternion qYaw = Quaternion.Euler(transform.rotation.eulerAngles.x, newValue, transform.rotation.eulerAngles.z);
                transform.rotation = qYaw;
                break;
            case "rollAngle":
                Quaternion qRoll = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, newValue);
                transform.rotation = qRoll;
                break;
            case "velocity":
                velocity = newValue;
                rb.velocity = transform.forward * newValue;
                break;
            case "crewHeartRate1":
                gameObject.GetComponent<crewData>().crewHeartRate1 = newValue;
                break;
            case "crewHeartRate2":
                gameObject.GetComponent<crewData>().crewHeartRate2 = newValue;
                break;
            case "crewHeartRate3":
                gameObject.GetComponent<crewData>().crewHeartRate3 = newValue;
                break;
            case "crewHeartRate4":
                gameObject.GetComponent<crewData>().crewHeartRate4 = newValue;
                break;
            case "crewHeartRate5":
                gameObject.GetComponent<crewData>().crewHeartRate5 = newValue;
                break;
            case "crewHeartRate6":
                gameObject.GetComponent<crewData>().crewHeartRate6 = newValue;
                break;
            case "crewBodyTemp1":
                gameObject.GetComponent<crewData>().crewBodyTemp1 = newValue;
                break;
            case "crewBodyTemp2":
                gameObject.GetComponent<crewData>().crewBodyTemp2 = newValue;
                break;
            case "crewBodyTemp3":
                gameObject.GetComponent<crewData>().crewBodyTemp3 = newValue;
                break;
            case "crewBodyTemp4":
                gameObject.GetComponent<crewData>().crewBodyTemp4 = newValue;
                break;
            case "crewBodyTemp5":
                gameObject.GetComponent<crewData>().crewBodyTemp5 = newValue;
                break;
            case "crewBodyTemp6":
                gameObject.GetComponent<crewData>().crewBodyTemp6 = newValue;
                break;
            case "posX":
                transform.position = new Vector3(newValue, transform.position.y, transform.position.z);
                break;
            case "posY":
                transform.position = new Vector3(transform.position.x, newValue, transform.position.z);
                break;
            case "posZ":
                transform.position = new Vector3(transform.position.x, transform.position.y, newValue);
                break;
        }
    }

    public void OnVesselDataChanged(int vessel, Vector3 pos, float vesselVelocity)
    {
        switch (vessel)
        {
            case 0:
                gameObject.GetComponent<mapData>().vessel1Pos = pos;
                gameObject.GetComponent<mapData>().vessel1Velocity = vesselVelocity;
                break;
            case 1:
                gameObject.GetComponent<mapData>().vessel2Pos = pos;
                gameObject.GetComponent<mapData>().vessel2Velocity = vesselVelocity;
                break;
            case 2:
                gameObject.GetComponent<mapData>().vessel3Pos = pos;
                gameObject.GetComponent<mapData>().vessel3Velocity = vesselVelocity;
                break;
            case 3:
                gameObject.GetComponent<mapData>().vessel4Pos = pos;
                gameObject.GetComponent<mapData>().vessel4Velocity = vesselVelocity;
                break;
            case 4:
                gameObject.GetComponent<mapData>().meg1Pos = pos;
                gameObject.GetComponent<mapData>().meg1Velocity = vesselVelocity;
                break;
        }
    }

    public void SetPlayerVessel(int vessel)
    {
        gameObject.GetComponent<mapData>().playerVessel = vessel;
    }

    public void SetPlayerVisibility(int vessel, bool state)
    {
        switch (vessel)
        {
            case 1:
                gameObject.GetComponent<mapData>().vessel1Vis = state;
                break;
            case 2:
                gameObject.GetComponent<mapData>().vessel2Vis = state;
                break;
            case 3:
                gameObject.GetComponent<mapData>().vessel3Vis = state;
                break;
            case 4:
                gameObject.GetComponent<mapData>().vessel4Vis = state;
                break;
            case 5:
                gameObject.GetComponent<mapData>().meg1Vis = state;
                break;
        }
    }


    public void OnChangeBool(string boolName, bool newValue)
    {
        switch (boolName)
        {
            case "disableInput":
                disableInput = newValue;
                break;
        }
    }

    public void GetRollPitchYaw(Quaternion q)
    {
        float x = q.x;
        float y = q.y;
        float z = q.z;
        float w = q.w;
        rollResult = Mathf.Asin(2 * x * y + 2 * z * w);
        pitchResult = Mathf.Atan2(2 * x * w - 2 * y * z, 1 - 2 * x * x - 2 * z * z);
        yawResult = Mathf.Atan2(2 * y * w - 2 * x * z, 1 - 2 * y * y - 2 * z * z);

        rollResult *= 180 / Mathf.PI;
        pitchResult *= 180 / Mathf.PI;
        pitchResult = -pitchResult;
        yawResult *= 180 / Mathf.PI;

        if (yawResult < 0)
        {
            yawResult = 360 + yawResult;
        }
    }

    public float GetBatteryTotal()
    {
        float batPower = (batteries[0] + batteries[1] + batteries[2] + batteries[3] + batteries[4] + batteries[5] + batteries[6]) / 7.0f;
        return batPower;
    }

    public float GetOxygenTotal()
    {
        float oxyTotal = (oxygenTanks[0] + oxygenTanks[1] + oxygenTanks[2] + oxygenTanks[3] + oxygenTanks[4] + oxygenTanks[5] + oxygenTanks[6]) / 7.0f;
        return oxyTotal;
    }

    [ClientRpc]
    public void RpcImpact(Vector3 impactVector)
    {
        gameObject.GetComponent<Rigidbody>().AddTorque(impactVector, ForceMode.VelocityChange);
    }


    public string GetPilot()
    {
        string playersName = "No pilot";

        //get a list of all the players
        players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            //if this player is the pilot
            if (players[i].GetComponent<gameInputs>().pilot)
            {
                playersName = players[i].name;
            }
        }

        return playersName;
    }

    #endregion
    #region UnityMethods
    public override void OnStartClient()
    {
        //vDescent = descent;
        gameObject.GetComponent<batteryData>().bank1 = batteries[0];
        gameObject.GetComponent<batteryData>().bank2 = batteries[1];
        gameObject.GetComponent<batteryData>().bank3 = batteries[2];
        gameObject.GetComponent<batteryData>().bank4 = batteries[3];
        gameObject.GetComponent<batteryData>().bank5 = batteries[4];
        gameObject.GetComponent<batteryData>().bank6 = batteries[5];
        gameObject.GetComponent<batteryData>().bank7 = batteries[6];

        gameObject.GetComponent<oxygenData>().oxygenTank1 = oxygenTanks[0];
        gameObject.GetComponent<oxygenData>().oxygenTank2 = oxygenTanks[1];
        gameObject.GetComponent<oxygenData>().oxygenTank3 = oxygenTanks[2];
        gameObject.GetComponent<oxygenData>().oxygenTank4 = oxygenTanks[3];
        gameObject.GetComponent<oxygenData>().oxygenTank5 = oxygenTanks[4];
        gameObject.GetComponent<oxygenData>().oxygenTank6 = oxygenTanks[5];
        gameObject.GetComponent<oxygenData>().oxygenTank7 = oxygenTanks[6];
    }


    // Use this for initialization
    void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
	
    // Server only update loop
    void Update ()
    {
        //get a list of all the players
        pilot = GetPilot();

        //get oxygen data from server
        oxygenTanks[0] = gameObject.GetComponent<oxygenData>().oxygenTank1;
        oxygenTanks[1] = gameObject.GetComponent<oxygenData>().oxygenTank2;
        oxygenTanks[2] = gameObject.GetComponent<oxygenData>().oxygenTank3;
        oxygenTanks[3] = gameObject.GetComponent<oxygenData>().oxygenTank4;
        oxygenTanks[4] = gameObject.GetComponent<oxygenData>().oxygenTank5;
        oxygenTanks[5] = gameObject.GetComponent<oxygenData>().oxygenTank6;
        oxygenTanks[6] = gameObject.GetComponent<oxygenData>().oxygenTank7;
        //get battery data from server
        batteries[0] = gameObject.GetComponent<batteryData>().bank1;
        batteries[1] = gameObject.GetComponent<batteryData>().bank2;
        batteries[2] = gameObject.GetComponent<batteryData>().bank3;
        batteries[3] = gameObject.GetComponent<batteryData>().bank4;
        batteries[4] = gameObject.GetComponent<batteryData>().bank5;
        batteries[5] = gameObject.GetComponent<batteryData>().bank6;
        batteries[6] = gameObject.GetComponent<batteryData>().bank7;

        //server only
        if (!isServer)
            return;

        //Debug.Log("Executing on this client host...");
        heading = yawResult;
        pitchAngle = -pitchResult;
        yawAngle = yawResult;
        rollAngle = rollResult;
        velocity = rb.velocity.magnitude;
        depth = -transform.position.y;
        battery = GetBatteryTotal();
        oxygen = GetOxygenTotal();
        dueTimeSecs -= Time.deltaTime;
        diveTimeSecs += Time.deltaTime;
        verticalVelocity = rb.velocity.y;
        horizontalVelocity = rb.velocity.x;
        floorDistance = 10994 - depth;
        if (dueTimeSecs < 0)
        {
            dueTimeSecs = 59.0f + dueTimeSecs;
            dueTimeMins -= 1.0f;
        }
        if (dueTimeMins < 0)
        {
            dueTimeMins = 59.0f;
            dueTimeHours -= 1.0f;
        }

        if (diveTimeSecs >= 60)
        {
            diveTimeSecs = 0f;
            diveTimeMins += 1.0f;
        }
        if (diveTimeMins >= 60)
        {
            diveTimeMins = 0f;
            diveTimeHours += 1.0f;
        }
    }

    void FixedUpdate()
    {
        this.GetComponent<SubControl>().SubController();

        //if (!disableInput)
        //{
        //    //apply the input forces
        //    currentThrust = Mathf.Lerp(currentThrust, (thrust * inputZaxis), Time.deltaTime * 0.1f);
        //    rb.AddForce(transform.forward * currentThrust);
        //    rb.AddRelativeTorque(-Vector3.right * (pitchSpeed * inputYaxis));
        //    rb.AddTorque(Vector3.up * (yawSpeed * inputXaxis));
        //}
        //
        ////calculate bank
        //float amountToBank = rb.angularVelocity.y * bankAmount;
        //bank = Mathf.Lerp(bank, amountToBank, rollSpeed);
        //rotation = transform.rotation.eulerAngles;
        //rotation *= Mathf.Deg2Rad;
        //rotation.z = 0f;
        //rotation += bankAxis * bank;
        //rotation *= Mathf.Rad2Deg;
        //
        ////apply the bank
        //transform.rotation = Quaternion.Euler(rotation);

        //server only
        if (!isServer)
            return;
        GetRollPitchYaw(transform.rotation);

    }

    IEnumerator wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //updateTimer = false;
    }
    #endregion
}