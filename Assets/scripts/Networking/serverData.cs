using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Meg.Networking;

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
    public float waterTemp;
    [SyncVar]
    public float floorDistance;
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
    public string pilot;
    [SyncVar]
    public float verticalVelocity;
    [SyncVar]
    public float horizontalVelocity;
    [SyncVar]
    public float diveTime;
    [SyncVar]
    public float dueTime;
    
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
    // private float bankAmount = 1.0f;
    // private Vector3 bankAxis = new Vector3(0F, 0F, -1F);
    // private float currentThrust = 0.0f;
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
                _batteryData = GetComponent<batteryData>();
            return _batteryData;
        }
    }

    private oxygenData _oxygenData;
    private oxygenData OxygenData
    {
        get
        {
            if (!_oxygenData)
                _oxygenData = GetComponent<oxygenData>();
            return _oxygenData;
        }
    }

    private errorData _errorData;
    private errorData ErrorData
    {
        get
        {
            if (!_errorData)
                _errorData = GetComponent<errorData>();
            return _errorData;
        }
    }

    private crewData _crewData;
    private crewData CrewData
    {
        get
        {
            if (!_crewData)
                _crewData = GetComponent<crewData>();
            return _crewData;
        }
    }

    private mapData _mapData;
    private mapData MapData
    {
        get
        {
            if (!_mapData)
                _mapData = GetComponent<mapData>();
            return _mapData;
        }
    }

    private operatingData _operatingData;
    public operatingData OperatingData
    {
        get
        {
            if (!_operatingData)
                _operatingData = GetComponent<operatingData>();
            return _operatingData;
        }
    }

    private SonarData _sonarData;
    public SonarData SonarData
    {
        get
        {
            if (!_sonarData)
                _sonarData = GetComponent<SonarData>();
            return _sonarData;
        }
    }

    public vesselMovements VesselMovements
    {
        get { return serverUtils.VesselMovements; }
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

        switch (valueName.ToLower())
        {
            case "depth":
                transform.position = new Vector3(transform.position.x, -newValue, transform.position.z);
                depth = newValue;
                break;
            case "duetime":
                dueTime = newValue;
                break;
            case "divetime":
                diveTime = newValue;
                break;
            case "watertemp":
                waterTemp = newValue;
                break;
            case "disableinput":
                disableInput = newValue > 0;
                break;
            case "battery":
                BatteryData.battery = newValue;
                break;
            case "batterytemp":
                BatteryData.batteryTemp = newValue;
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
            case "co2":
                OxygenData.Co2 = newValue;
                break;
            case "cabinpressure":
                OxygenData.cabinPressure = newValue;
                break;
            case "cabintemp":
                OxygenData.cabinTemp = newValue;
                break;
            case "cabinhumidity":
                OxygenData.cabinHumidity = newValue;
                break;
            case "error_bilgeleak":
                ErrorData.error_bilgeLeak = newValue;
                break;
            case "error_batteryleak":
                ErrorData.error_batteryLeak = newValue;
                break;
            case "error_electricleak":
                ErrorData.error_electricLeak = newValue;
                break;
            case "error_oxygenext":
                ErrorData.error_oxygenExt = newValue;
                break;
            case "error_vhf":
                ErrorData.error_vhf = newValue;
                break;
            case "error_forwardsonar":
                ErrorData.error_forwardSonar = newValue;
                break;
            case "error_depthsonar":
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
            case "error_sternlights":
                ErrorData.error_sternLights = newValue;
                break;
            case "error_bowlights":
                ErrorData.error_bowLights = newValue;
                break;
            case "error_portlights":
                ErrorData.error_portLights = newValue;
                break;
            case "error_bowthruster":
                ErrorData.error_bowThruster = newValue;
                break;
            case "error_hyrdaulicres":
                ErrorData.error_hyrdaulicRes = newValue;
                break;
            case "error_starboardlights":
                ErrorData.error_starboardLights = newValue;
                break;
            case "error_runninglights":
                ErrorData.error_runningLights = newValue;
                break;
            case "error_ballasttank":
                ErrorData.error_ballastTank = newValue;
                break;
            case "error_hydraulicpump":
                ErrorData.error_hydraulicPump = newValue;
                break;
            case "error_oxygenpump":
                ErrorData.error_oxygenPump = newValue;
                break;
            case "inputxaxis":
                inputXaxis = newValue;
                break;
            case "inputyaxis":
                inputYaxis = newValue;
                break;
            case "inputzaxis":
                inputZaxis = newValue;
                break;
            case "inputxaxis2":
                inputXaxis2 = newValue;
                break;
            case "inputyaxis2":
                inputYaxis2 = newValue;
                break;
            case "pitchangle":
                Quaternion qPitch = Quaternion.Euler(newValue, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                transform.rotation = qPitch;
                break;
            case "yawangle":
                Quaternion qYaw = Quaternion.Euler(transform.rotation.eulerAngles.x, newValue, transform.rotation.eulerAngles.z);
                transform.rotation = qYaw;
                break;
            case "rollangle":
                Quaternion qRoll = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, newValue);
                transform.rotation = qRoll;
                break;
            case "velocity":
                velocity = newValue;
                rb.velocity = transform.forward * newValue;
                break;
            case "crewheartrate1":
                CrewData.crewHeartRate1 = newValue;
                break;
            case "crewheartrate2":
                CrewData.crewHeartRate2 = newValue;
                break;
            case "crewheartrate3":
                CrewData.crewHeartRate3 = newValue;
                break;
            case "crewheartrate4":
                CrewData.crewHeartRate4 = newValue;
                break;
            case "crewheartrate5":
                CrewData.crewHeartRate5 = newValue;
                break;
            case "crewheartrate6":
                CrewData.crewHeartRate6 = newValue;
                break;
            case "crewbodytemp1":
                CrewData.crewBodyTemp1 = newValue;
                break;
            case "crewbodytemp2":
                CrewData.crewBodyTemp2 = newValue;
                break;
            case "crewbodytemp3":
                CrewData.crewBodyTemp3 = newValue;
                break;
            case "crewbodytemp4":
                CrewData.crewBodyTemp4 = newValue;
                break;
            case "crewbodytemp5":
                CrewData.crewBodyTemp5 = newValue;
                break;
            case "crewbodytemp6":
                CrewData.crewBodyTemp6 = newValue;
                break;
            case "posx":
                transform.position = new Vector3(newValue, transform.position.y, transform.position.z);
                break;
            case "posy":
                transform.position = new Vector3(transform.position.x, newValue, transform.position.z);
                break;
            case "posz":
                transform.position = new Vector3(transform.position.x, transform.position.y, newValue);
                break;
            case "latitude":
                MapData.latitude = newValue;
                break;
            case "longitude":
                MapData.longitude = newValue;
                break;
            case "vessel1vis":
                MapData.vessel1Vis = newValue > 0;
                break;
            case "vessel2vis":
                MapData.vessel2Vis = newValue > 0;
                break;
            case "vessel3vis":
                MapData.vessel3Vis = newValue > 0;
                break;
            case "vessel4vis":
                MapData.vessel4Vis = newValue > 0;
                break;
            case "meg1vis":
                MapData.meg1Vis = newValue > 0;
                break;
            case "intercept1vis":
                MapData.intercept1Vis = newValue > 0;
                break;
            case "vessel1warning":
                MapData.vessel1Warning = newValue > 0;
                break;
            case "vessel2warning":
                MapData.vessel2Warning = newValue > 0;
                break;
            case "vessel3warning":
                MapData.vessel3Warning = newValue > 0;
                break;
            case "vessel4warning":
                MapData.vessel4Warning = newValue > 0;
                break;
            case "meg1warning":
                MapData.meg1Warning = newValue > 0;
                break;
            case "intercept1warning":
                MapData.intercept1Warning = newValue > 0;
                break;
            case "initiatemapevent":
                MapData.initiateMapEvent = newValue;
                break;
            case "towwinchload":
                OperatingData.towWinchLoad = newValue;
                break;
            case "hydraulictemp":
                OperatingData.hydraulicTemp = newValue;
                break;
            case "hydraulicpressure":
                OperatingData.hydraulicPressure = newValue;
                break;
            case "ballastpressure":
                OperatingData.ballastPressure = newValue;
                break;
            case "variableballasttemp":
                OperatingData.variableBallastTemp = newValue;
                break;
            case "variableballastpressure":
                OperatingData.variableBallastPressure = newValue;
                break;
            case "commssignalstrength":
                OperatingData.commsSignalStrength = newValue;
                break;
            case "divertpowertothrusters":
                OperatingData.divertPowerToThrusters = newValue;
                break;
            case "vesselmovementsactive":
                VesselMovements.Active = newValue > 0;
                break;
            case "timetointercept":
                VesselMovements.TimeToIntercept = newValue;
                break;
            case "megspeed":
                SonarData.MegSpeed = newValue;
                break;
            case "megturnspeed":
                SonarData.MegTurnSpeed = newValue;
                break;
        }

        if (ValueChangedEvent != null)
            ValueChangedEvent(valueName, newValue);
    }

    public void OnValueChanged(string valueName, string newValue)
    {
        switch (valueName.ToLower())
        {
            case "mapeventname":
                MapData.mapEventName = newValue;
                break;

            default:
                float value;
                if (float.TryParse(newValue, out value))
                    OnValueChanged(valueName, value);
                break;
        }
    }

    public void OnVesselDataChanged(int vessel, Vector3 pos, float vesselVelocity)
    {
        if (vessel == MapData.playerVessel)
        {
            //convert from map space to world space
            gameObject.transform.position = new Vector3(pos.x * 1000, -pos.z, pos.y * 1000);
        }
        else
        {
            MapData.SetVesselState(vessel, pos, vesselVelocity);
        }
    }

    public void SetPlayerWorldVelocity(Vector3 velocity)
    {
        GetComponent<SubControl>().SetWorldVelocity(velocity);
    }

    public void SetPlayerVessel(int vessel)
    {
        MapData.playerVessel = vessel;
    }

    public void SetVesselVis(int vessel, bool state)
    {
        switch (vessel)
        {
            case 1:
                MapData.vessel1Vis = state;
                break;
            case 2:
                MapData.vessel2Vis = state;
                break;
            case 3:
                MapData.vessel3Vis = state;
                break;
            case 4:
                MapData.vessel4Vis = state;
                break;
            case 5:
                MapData.meg1Vis = state;
                break;
            case 6:
                MapData.intercept1Vis = state;
                break;
        }
    }

    public void OnChangeBool(string boolName, bool newValue)
    {
        switch (boolName.ToLower())
        {
            case "disableinput":
                disableInput = newValue;
                break;
            case "vessel1vis":
                MapData.vessel1Vis = newValue;
                break;
            case "vessel2vis":
                MapData.vessel2Vis = newValue;
                break;
            case "vessel3vis":
                MapData.vessel3Vis = newValue;
                break;
            case "vessel4vis":
                MapData.vessel4Vis = newValue;
                break;
            case "meg1vis":
                MapData.meg1Vis = newValue;
                break;
            case "intercept1vis":
                MapData.intercept1Vis = newValue;
                break;
            case "vessel1warning":
                MapData.vessel1Warning = newValue;
                break;
            case "vessel2warning":
                MapData.vessel2Warning = newValue;
                break;
            case "vessel3warning":
                MapData.vessel3Warning = newValue;
                break;
            case "vessel4warning":
                MapData.vessel4Warning = newValue;
                break;
            case "meg1warning":
                MapData.meg1Warning = newValue;
                break;
            case "intercept1warning":
                MapData.intercept1Warning = newValue;
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

    public float GetFloorDistance()
    {
        /*
        // Delegate to navigation map for sub depths.
        if (NavSubPins.Instance)
        {
            var playerVessel = serverUtils.GetPlayerVessel();
            return NavSubPins.Instance.GetVesselFloorDistance(playerVessel);
        }
        */
        
        return Mathf.Max(0, mapData.DefaultFloorDepth - depth);
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

        // Get oxygen data from server
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
        floorDistance = GetFloorDistance();
        BatteryData.battery = GetBatteryTotal();
        OxygenData.oxygen = GetOxygenTotal();

        // Update ETA timer.
        dueTime = Mathf.Max(0, dueTime - Time.deltaTime);

        // Update dive timer.
        diveTime += Time.deltaTime;

        verticalVelocity = rb.velocity.y;
        horizontalVelocity = rb.velocity.x;
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
