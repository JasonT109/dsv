using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Meg.Networking;

public class serverData : NetworkBehaviour
{
    #region SyncVars
    [SyncVar]
    public int scene = 1;
    [SyncVar]
    public int shot = 1;
    [SyncVar]
    public int take = 1;
    [SyncVar]
    public float depth;
    [SyncVar]
    public float pressure;
    [SyncVar]
    public float waterTemp;
    [SyncVar]
    public float floorDistance;
    [SyncVar]
    public float floorDepth = mapData.DefaultFloorDepth;
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
    public bool diveTimeActive = true;
    [SyncVar]
    public float dueTime;
    [SyncVar]
    public bool dueTimeActive = true;

    #endregion

    #region PublicVars
    public bool isGlider = false;
    public GameObject[] players;
    #endregion

    #region PrivateVars
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

    private airData _airData;
    private airData AirData
    {
        get
        {
            if (!_airData)
                _airData = GetComponent<airData>();
            return _airData;
        }
    }

    private cabinData _cabinData;
    private cabinData CabinData
    {
        get
        {
            if (!_cabinData)
                _cabinData = GetComponent<cabinData>();
            return _cabinData;
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

    private gliderErrorData _gliderErrorData;
    public gliderErrorData GliderErrorData
    {
        get
        {
            if (!_gliderErrorData)
                _gliderErrorData = GetComponent<gliderErrorData>();
            return _gliderErrorData;
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

    private SubControl _subControl;
    public SubControl SubControl
    {
        get
        {
            if (!_subControl)
                _subControl = GetComponent<SubControl>();
            return _subControl;
        }
    }

    private DCCScreenData _DCCScreenControl;
    public DCCScreenData DCCScreenData
    {
        get
        {
            if (!_DCCScreenControl)
                _DCCScreenControl = GetComponent<DCCScreenData>();
            return _DCCScreenControl;
        }
    }

    public vesselMovements VesselMovements
    {
        get { return serverUtils.VesselMovements; }
    }

    #endregion

    #region PublicMethods

    public delegate void ValueChangeHandler(string valueName, float newValue);

    public ValueChangeHandler ValueChangedEvent;

    public void OnValueChanged(string valueName, float newValue)
    {
        //Debug.Log("Setting server data: " + valueName + " to: " + newValue);

        switch (valueName.ToLower())
        {
            case "scene":
                scene = Mathf.RoundToInt(newValue);
                break;
            case "shot":
                shot = Mathf.RoundToInt(newValue);
                break;
            case "take":
                take = Mathf.RoundToInt(newValue);
                break;
            case "depth":
                transform.position = new Vector3(transform.position.x, -newValue, transform.position.z);
                depth = newValue;
                break;
            case "floordepth":
                floorDepth = newValue;
                break;
            case "duetime":
                dueTime = newValue;
                break;
            case "duetimeactive":
                dueTimeActive = newValue > 0;
                break;
            case "divetime":
                diveTime = newValue;
                break;
            case "divetimeactive":
                diveTimeActive = newValue > 0;
                break;
            case "watertemp":
                waterTemp = newValue;
                break;
            case "disableinput":
                SubControl.disableInput = newValue > 0;
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
            case "oxygentank1":
                OxygenData.oxygenTank1 = newValue;
                break;
            case "oxygentank2":
                OxygenData.oxygenTank2 = newValue;
                break;
            case "oxygentank3":
                OxygenData.oxygenTank3 = newValue;
                break;
            case "reserveoxygentank1":
                OxygenData.reserveOxygenTank1 = newValue;
                break;
            case "reserveoxygentank2":
                OxygenData.reserveOxygenTank2 = newValue;
                break;
            case "reserveoxygentank3":
                OxygenData.reserveOxygenTank3 = newValue;
                break;
            case "reserveoxygentank4":
                OxygenData.reserveOxygenTank4 = newValue;
                break;
            case "reserveoxygentank5":
                OxygenData.reserveOxygenTank5 = newValue;
                break;
            case "reserveoxygentank6":
                OxygenData.reserveOxygenTank6 = newValue;
                break;
            case "airtank1":
                AirData.airTank1 = newValue;
                break;
            case "airtank2":
                AirData.airTank2 = newValue;
                break;
            case "airtank3":
                AirData.airTank3 = newValue;
                break;
            case "airtank4":
                AirData.airTank4 = newValue;
                break;
            case "reserveairtank1":
                AirData.reserveAirTank1 = newValue;
                break;
            case "reserveairtank2":
                AirData.reserveAirTank2 = newValue;
                break;
            case "reserveairtank3":
                AirData.reserveAirTank3 = newValue;
                break;
            case "reserveairtank4":
                AirData.reserveAirTank4 = newValue;
                break;
            case "reserveairtank5":
                AirData.reserveAirTank5 = newValue;
                break;
            case "reserveairtank6":
                AirData.reserveAirTank6 = newValue;
                break;
            case "reserveairtank7":
                AirData.reserveAirTank7 = newValue;
                break;
            case "reserveairtank8":
                AirData.reserveAirTank8 = newValue;
                break;
            case "reserveairtank9":
                AirData.reserveAirTank9 = newValue;
                break;
            case "co2":
                CabinData.Co2 = newValue;
                break;
            case "cabinpressure":
                CabinData.cabinPressure = newValue;
                break;
            case "cabintemp":
                CabinData.cabinTemp = newValue;
                break;
            case "cabinhumidity":
                CabinData.cabinHumidity = newValue;
                break;
            case "cabinoxygen":
                CabinData.cabinOxygen = newValue;
                break;
            case "scrubbedco2":
                CabinData.scrubbedCo2 = newValue;
                break;
            case "scrubbedhumidity":
                CabinData.scrubbedHumidity = newValue;
                break;
            case "scrubbedoxygen":
                CabinData.scrubbedOxygen = newValue;
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
            case "genericerror":
                ErrorData.genericerror = newValue;
                break;
            case "inputxaxis":
                SubControl.inputXaxis = newValue;
                break;
            case "inputyaxis":
                SubControl.inputYaxis = newValue;
                break;
            case "inputzaxis":
                SubControl.inputZaxis = newValue;
                break;
            case "inputxaxis2":
                SubControl.inputXaxis2 = newValue;
                break;
            case "inputyaxis2":
                SubControl.inputYaxis2 = newValue;
                break;
            case "isautostabilised":
                SubControl.isAutoStabilised = newValue > 0;
                break;
            case "ispitchalsostabilised":
                SubControl.IsPitchAlsoStabilised = newValue > 0;
                break;
            case "joystickoverride":
                SubControl.JoystickOverride = newValue > 0;
                break;
            case "joystickpilot":
                SubControl.JoystickPilot = newValue > 0;
                break;
            case "acceleration":
                SubControl.Acceleration = newValue;
                break;
            case "yawspeed":
                SubControl.yawSpeed = newValue;
                break;
            case "pitchspeed":
                SubControl.pitchSpeed = newValue;
                break;
            case "rollspeed":
                SubControl.rollSpeed = newValue;
                break;
            case "maxspeed":
                SubControl.MaxSpeed = newValue;
                break;
            case "minspeed":
                SubControl.MinSpeed = newValue;
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
                if (rb)
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
            case "playervessel":
                SetPlayerVessel(Mathf.RoundToInt(newValue));
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
            case "vesselmovementenabled":
                VesselMovements.Enabled = newValue > 0;
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
            case "vertran_heat_l":
                GliderErrorData.vertran_heat_l = newValue;
                break;
            case "vertran_heat_r":
                GliderErrorData.vertran_heat_r = newValue;
                break;
            case "thruster_heat_l":
                GliderErrorData.thruster_heat_l = newValue;
                break;
            case "thruster_heat_r":
                GliderErrorData.thruster_heat_r = newValue;
                break;
            case "jet_heat_l":
                GliderErrorData.jet_heat_l = newValue;
                break;
            case "jet_heat_r":
                GliderErrorData.jet_heat_r = newValue;
                break;
            case "dccquadscreen0":
                DCCScreenData.DCCquadScreen0 = (int)newValue;
                break;
            case "dccquadscreen1":
                DCCScreenData.DCCquadScreen1 = (int)newValue;
                break;
            case "dccquadscreen2":
                DCCScreenData.DCCquadScreen2 = (int)newValue;
                break;
            case "dccquadscreen3":
                DCCScreenData.DCCquadScreen3 = (int)newValue;
                break;
            case "dccquadscreen4":
                DCCScreenData.DCCquadScreen4 = (int)newValue;
                break;
            case "dccfullscreen":
                DCCScreenData.DCCfullscreen = (int)newValue;
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

            case "playervesselname":
                MapData.playerVesselName = newValue;
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
            gameObject.transform.position = new Vector3(pos.x * 1000, -pos.z, pos.y * 1000);

        MapData.SetVesselState(vessel, pos, vesselVelocity);
    }

    public void SetPlayerWorldVelocity(Vector3 velocity)
    {
        GetComponent<SubControl>().SetWorldVelocity(velocity);
    }

    public void SetPlayerVessel(int vessel)
    {
        MapData.playerVessel = vessel;
        MapData.playerVesselName = GetVesselName(vessel);
    }

    public string GetVesselName(int vessel)
    {
        // Retrieve vessel name from configuration.
        var vesselNames = Configuration.GetJson("vessel-names");
        if (vesselNames && vesselNames.IsArray && vessel < vesselNames.Count)
            return vesselNames[vessel - 1].str;

        return "Unknown";
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
            case "divetimeactive":
                diveTimeActive = newValue;
                break;
            case "duetimeactive":
                dueTimeActive = newValue;
                break;
            case "disableinput":
                SubControl.disableInput = newValue;
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
            case "vesselmovementenabled":
                VesselMovements.Enabled = newValue;
                break;
            case "isautostabilised":
                SubControl.isAutoStabilised = newValue;
                break;
            case "ispitchalsostabilised":
                SubControl.IsPitchAlsoStabilised = newValue;
                break;
            case "joystickoverride":
                SubControl.JoystickOverride = newValue;
                break;
            case "joystickpilot":
                SubControl.JoystickPilot = newValue;
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

    /** Initialize clientside values. */
    public override void OnStartClient()
    {
    }

    /** Initialization. */
    void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        Debug.Log("serverData.Start(): Client ID: " + serverUtils.Id);
    }
	
    /** Updating. */
    void Update ()
    {
        // Update clientside values.
        UpdateOnClient();

        // Update serverside values.
        if (isServer)
            UpdateOnServer();
    }

    /** Update data values on the local client. */
    private void UpdateOnClient()
    {
        // Determine the current pilot.
        pilot = GetPilot();
    }

    /** Update data values only on the server. */
    [Server]
    private void UpdateOnServer()
    {
        heading = yawResult;
        pitchAngle = -pitchResult;
        yawAngle = yawResult;
        rollAngle = rollResult;
        velocity = rb.velocity.magnitude;
        depth = Mathf.Max(0, -transform.position.y);
        floorDistance = Mathf.Max(0, floorDepth - depth);

        // Update ETA timer.
        if (dueTimeActive)
            dueTime = Mathf.Max(0, dueTime - Time.deltaTime);

        // Update dive timer.
        if (diveTimeActive)
            diveTime += Time.deltaTime;

        verticalVelocity = rb.velocity.y;
        horizontalVelocity = rb.velocity.x;
    }

    /** Physics update. */
    void FixedUpdate()
    {
        GetComponent<SubControl>().SubController();

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
        if (isServer)
            GetRollPitchYaw(transform.rotation);
    }

    IEnumerator wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //updateTimer = false;
    }
    #endregion
}
