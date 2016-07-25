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
    public float cabinPressure;
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
    public float cabinTemp;
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
    [SyncVar]
    public float commsSignalStrength;
    [SyncVar]
    public float divertPowerToThrusters;

    #endregion
    #region PublicVars

    //public float descent;
    public bool isGlider = false;
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
    
    #region PrivateProperties
    private batteryData _batteryData;
    private batteryData BatteryData
    {
        get
        {
            if (!_batteryData)
                _batteryData = gameObject.GetComponent<batteryData>();
            return _batteryData;
        }
    }

    private oxygenData _oxygenData;
    private oxygenData OxygenData
    {
        get
        {
            if (!_oxygenData)
                _oxygenData = gameObject.GetComponent<oxygenData>();
            return _oxygenData;
        }
    }

    private errorData _errorData;
    private errorData ErrorData
    {
        get
        {
            if (!_errorData)
                _errorData = gameObject.GetComponent<errorData>();
            return _errorData;
        }
    }

    private crewData _crewData;
    private crewData CrewData
    {
        get
        {
            if (!_crewData)
                _crewData = CrewData;
            return _crewData;
        }
    }

    #endregion

    #region PublicMethods
    public void OnBatterySliderChanged(int bank, float newValues)
    {
        switch (bank)
        {
            case 0:
                BatteryData.bank1 = newValues;
                break;
            case 1:
                BatteryData.bank2 = newValues;
                break;
            case 2:
                BatteryData.bank3 = newValues;
                break;
            case 3:
                BatteryData.bank4 = newValues;
                break;
            case 4:
                BatteryData.bank5 = newValues;
                break;
            case 5:
                BatteryData.bank6 = newValues;
                break;
            case 6:
                BatteryData.bank7 = newValues;
                break;
        }
    }

    public void OnOxygenSliderChanged(int tank, float newValues)
    {
        switch (tank)
        {
            case 0:
                OxygenData.oxygenTank1 = newValues;
                break;
            case 1:
                OxygenData.oxygenTank2 = newValues;
                break;
            case 2:
                OxygenData.oxygenTank3 = newValues;
                break;
            case 3:
                OxygenData.oxygenTank4 = newValues;
                break;
            case 4:
                OxygenData.oxygenTank5 = newValues;
                break;
            case 5:
                OxygenData.oxygenTank6 = newValues;
                break;
            case 6:
                OxygenData.oxygenTank7 = newValues;
                break;
        }
    }

    public delegate void ValueChangeHandler(string valueName, float newValue);

    public ValueChangeHandler ValueChangedEvent;

    public void OnValueChanged(string valueName, float newValue)
    {
        //Debug.Log("Setting server data: " + valueName + " to: " + newValue);

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
            case "cabinTemp":
                cabinTemp = newValue;
                break;
            case "b1":
                BatteryData.bank1 = newValue;
                break;
            case "b2":
                BatteryData.bank2 = newValue;
                break;
            case "b3":
                BatteryData.bank3 = newValue;
                break;
            case "b4":
                BatteryData.bank4 = newValue;
                break;
            case "b5":
                BatteryData.bank5 = newValue;
                break;
            case "b6":
                BatteryData.bank6 = newValue;
                break;
            case "b7":
                BatteryData.bank7 = newValue;
                break;
            case "o1":
                OxygenData.oxygenTank1 = newValue;
                break;
            case "o2":
                OxygenData.oxygenTank2 = newValue;
                break;
            case "o3":
                OxygenData.oxygenTank3 = newValue;
                break;
            case "o4":
                OxygenData.oxygenTank4 = newValue;
                break;
            case "o5":
                OxygenData.oxygenTank5 = newValue;
                break;
            case "o6":
                OxygenData.oxygenTank6 = newValue;
                break;
            case "o7":
                OxygenData.oxygenTank7 = newValue;
                break;
            case "error_bilgeLeak":
                ErrorData.error_bilgeLeak = newValue;
                break;
            case "error_batteryLeak":
                ErrorData.error_batteryLeak = newValue;
                break;
            case "error_electricLeak":
                ErrorData.error_electricLeak = newValue;
                break;
            case "error_oxygenExt":
                ErrorData.error_oxygenExt = newValue;
                break;
            case "error_vhf":
                ErrorData.error_vhf = newValue;
                break;
            case "error_forwardSonar":
                ErrorData.error_forwardSonar = newValue;
                break;
            case "error_depthSonar":
                ErrorData.error_depthSonar = newValue;
                break;
            case "error_doppler":
                ErrorData.error_doppler = newValue;
                break;
            case "error_gps":
                ErrorData.error_gps = newValue;
                break;
            case "error_cpu":
                ErrorData.error_cpu = newValue;
                break;
            case "error_vidhd":
                ErrorData.error_vidhd = newValue;
                break;
            case "error_datahd":
                ErrorData.error_datahd = newValue;
                break;
            case "error_tow":
                ErrorData.error_tow = newValue;
                break;
            case "error_radar":
                ErrorData.error_radar = newValue;
                break;
            case "error_sternLights":
                ErrorData.error_sternLights = newValue;
                break;
            case "error_bowLights":
                ErrorData.error_bowLights = newValue;
                break;
            case "error_portLights":
                ErrorData.error_portLights = newValue;
                break;
            case "error_bowThruster":
                ErrorData.error_bowThruster = newValue;
                break;
            case "error_hyrdaulicRes":
                ErrorData.error_hyrdaulicRes = newValue;
                break;
            case "error_starboardLights":
                ErrorData.error_starboardLights = newValue;
                break;
            case "error_runningLights":
                ErrorData.error_runningLights = newValue;
                break;
            case "error_ballastTank":
                ErrorData.error_ballastTank = newValue;
                break;
            case "error_hydraulicPump":
                ErrorData.error_hydraulicPump = newValue;
                break;
            case "error_oxygenPump":
                ErrorData.error_oxygenPump = newValue;
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
                CrewData.crewHeartRate1 = newValue;
                break;
            case "crewHeartRate2":
                CrewData.crewHeartRate2 = newValue;
                break;
            case "crewHeartRate3":
                CrewData.crewHeartRate3 = newValue;
                break;
            case "crewHeartRate4":
                CrewData.crewHeartRate4 = newValue;
                break;
            case "crewHeartRate5":
                CrewData.crewHeartRate5 = newValue;
                break;
            case "crewHeartRate6":
                CrewData.crewHeartRate6 = newValue;
                break;
            case "crewBodyTemp1":
                CrewData.crewBodyTemp1 = newValue;
                break;
            case "crewBodyTemp2":
                CrewData.crewBodyTemp2 = newValue;
                break;
            case "crewBodyTemp3":
                CrewData.crewBodyTemp3 = newValue;
                break;
            case "crewBodyTemp4":
                CrewData.crewBodyTemp4 = newValue;
                break;
            case "crewBodyTemp5":
                CrewData.crewBodyTemp5 = newValue;
                break;
            case "crewBodyTemp6":
                CrewData.crewBodyTemp6 = newValue;
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
            case "commsSignalStrength":
                commsSignalStrength = newValue;
                break;
            case "divertPowerToThrusters":
                divertPowerToThrusters = newValue;
                break;
            case "latitude":
                gameObject.GetComponent<mapData>().latitude = newValue;
                break;
            case "longitude":
                gameObject.GetComponent<mapData>().longitude = newValue;
                break;
        }

        if (ValueChangedEvent != null)
            ValueChangedEvent(valueName, newValue);
    }

    public void OnVesselDataChanged(int vessel, Vector3 pos, float vesselVelocity)
    {
        if (vessel == gameObject.GetComponent<mapData>().playerVessel)
        {
            //convert from map space to world space
            gameObject.transform.position = new Vector3(pos.x * 1000, -pos.z, pos.y * 1000);
        }
        else
        {
            gameObject.GetComponent<mapData>().SetVesselState(vessel, pos, vesselVelocity);
        }
    }

    public void SetPlayerWorldVelocity(Vector3 velocity)
    {
        GetComponent<SubControl>().SetWorldVelocity(velocity);
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
            case 6:
                gameObject.GetComponent<mapData>().intercept1Vis = state;
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
        float batPower;

        //gliders only have 4 battery banks
        if (isGlider)
        {
            batPower = (batteries[0] + batteries[1] + batteries[2] + batteries[3]) / 4.0f;
        }
        else
        {
            batPower = (batteries[0] + batteries[1] + batteries[2] + batteries[3] + batteries[4] + batteries[5] + batteries[6]) / 7.0f;
        }
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
        BatteryData.bank1 = batteries[0];
        BatteryData.bank2 = batteries[1];
        BatteryData.bank3 = batteries[2];
        BatteryData.bank4 = batteries[3];
        BatteryData.bank5 = batteries[4];
        BatteryData.bank6 = batteries[5];
        BatteryData.bank7 = batteries[6];

        OxygenData.oxygenTank1 = oxygenTanks[0];
        OxygenData.oxygenTank2 = oxygenTanks[1];
        OxygenData.oxygenTank3 = oxygenTanks[2];
        OxygenData.oxygenTank4 = oxygenTanks[3];
        OxygenData.oxygenTank5 = oxygenTanks[4];
        OxygenData.oxygenTank6 = oxygenTanks[5];
        OxygenData.oxygenTank7 = oxygenTanks[6];
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
        oxygenTanks[0] = OxygenData.oxygenTank1;
        oxygenTanks[1] = OxygenData.oxygenTank2;
        oxygenTanks[2] = OxygenData.oxygenTank3;
        oxygenTanks[3] = OxygenData.oxygenTank4;
        oxygenTanks[4] = OxygenData.oxygenTank5;
        oxygenTanks[5] = OxygenData.oxygenTank6;
        oxygenTanks[6] = OxygenData.oxygenTank7;
        //get battery data from server
        batteries[0] = BatteryData.bank1;
        batteries[1] = BatteryData.bank2;
        batteries[2] = BatteryData.bank3;
        batteries[3] = BatteryData.bank4;
        batteries[4] = BatteryData.bank5;
        batteries[5] = BatteryData.bank6;
        batteries[6] = BatteryData.bank7;

        //server only
        if (!isServer)
            return;

        //Debug.Log("Executing on this client host...");
        heading = yawResult;
        pitchAngle = -pitchResult;
        yawAngle = yawResult;
        rollAngle = rollResult;
        velocity = rb.velocity.magnitude;
        depth = Mathf.Max(0, -transform.position.y);
        floorDistance = Mathf.Max(0, 10994 - depth);
        battery = GetBatteryTotal();
        oxygen = GetOxygenTotal();
        dueTimeSecs -= Time.deltaTime;
        diveTimeSecs += Time.deltaTime;
        verticalVelocity = rb.velocity.y;
        horizontalVelocity = rb.velocity.x;

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