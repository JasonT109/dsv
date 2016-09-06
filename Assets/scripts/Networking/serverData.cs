using UnityEngine;
using UnityEngine.Networking;
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
    public float depthOverride;
    [SyncVar]
    public float depthOverrideAmount;
    [SyncVar]
    public bool depthDisplayed = true;
    [SyncVar]
    public float floorDistance;
    [SyncVar]
    public bool floorDistanceDisplayed = true;
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

    /** Whether joystick can be used to toggle descent mode. */
    [SyncVar]
    public bool isControlDecentModeOnJoystick = false;

    /** Where input is coming from (0 = none, 1 = client, 2 = server). */
    [SyncVar]
    public float inputSource; 

    #endregion

    #region DynamicValues

    /** Class definition for a synchronized list of dynamic server values. */
    public class SyncListValues : SyncListStruct<serverUtils.ServerValue> { };

    /** Synchronized list for server values that are defined at runtime. */
    public SyncListValues dynamicValues = new SyncListValues();
    
    #endregion

    #region PublicVars
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

    private vesselData _vesselData;
    private vesselData VesselData
    {
        get
        {
            if (!_vesselData)
                _vesselData = GetComponent<vesselData>();
            return _vesselData;
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

    private domeData _domeData;
    public domeData DomeData
    {
        get
        {
            if (!_domeData)
                _domeData = GetComponent<domeData>();
            return _domeData;
        }
    }

    private screenData _screenData;
    public screenData ScreenData
    {
        get
        {
            if (!_screenData)
                _screenData = GetComponent<screenData>();
            return _screenData;
        }
    }

    private lightData _lightData;
    public lightData LightData
    {
        get
        {
            if (!_lightData)
                _lightData = GetComponent<lightData>();
            return _lightData;
        }
    }

    private dockingData _dockingData;
    public dockingData DockingData
    {
        get
        {
            if (!_dockingData)
                _dockingData = GetComponent<dockingData>();
            return _dockingData;
        }
    }

    private glTowingData _glTowingData;
    public glTowingData GLTowingData
    {
        get
        {
            if (!_glTowingData)
                _glTowingData = GetComponent<glTowingData>();
            return _glTowingData;
        }
    }

    private popupData _popupData;
    public popupData PopupData
    {
        get
        {
            if (!_popupData)
                _popupData = GetComponent<popupData>();
            return _popupData;
        }
    }

    public vesselMovements VesselMovements
    {
        get { return serverUtils.VesselMovements; }
    }

    #endregion


    #region Events

    /** Handler type for shared server value change events. */
    public delegate void ValueChangeHandler(string valueName, float newValue);

    /** An event that fires when a shared server value is changed on this instance. */
    public ValueChangeHandler ValueChangedEvent;

    #endregion


    #region PublicMethods

    /** Updates a server shared value. */
    public void OnValueChanged(string valueName, float newValue, bool add = false)
    {
        // Check if a value name was supplied.
        if (string.IsNullOrEmpty(valueName))
            return;

        // Check if we're setting a vessel state value.
        if (VesselData.IsVesselKey(valueName))
        {
            VesselData.SetServerData(valueName, newValue, add);
            if (ValueChangedEvent != null)
                ValueChangedEvent(valueName, newValue);

            return;
        }

        // Match the server data key against known data values.
        var key = valueName.ToLower();
        switch (key)
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
            case "depthoverride":
                depthOverride = newValue;
                break;
            case "depthoverrideamount":
                depthOverrideAmount = newValue;
                break;
            case "depthdisplayed":
                depthDisplayed = newValue > 0;
                break;
            case "floordepth":
                floorDepth = newValue;
                break;
            case "floordistancedisplayed":
                floorDistanceDisplayed = newValue > 0;
                break;
            case "domecenter":
                DomeData.domeCenter = (domeData.OverlayId) newValue;
                break;
            case "domecornerbottomleft":
                DomeData.domeCornerBottomLeft = (domeData.OverlayId) newValue;
                break;
            case "domecornerbottomright":
                DomeData.domeCornerBottomRight = (domeData.OverlayId) newValue;
                break;
            case "domecornertopleft":
                DomeData.domeCornerTopLeft = (domeData.OverlayId) newValue;
                break;
            case "domecornertopright":
                DomeData.domeCornerTopRight = (domeData.OverlayId) newValue;
                break;
            case "domeleft":
                DomeData.domeLeft = (domeData.OverlayId) newValue;
                break;
            case "domehexbottomleft":
                DomeData.domeHexBottomLeft = (domeData.OverlayId) newValue;
                break;
            case "domehexbottomright":
                DomeData.domeHexBottomRight = (domeData.OverlayId) newValue;
                break;
            case "domehextopleft":
                DomeData.domeHexTopLeft = (domeData.OverlayId) newValue;
                break;
            case "domehextopright":
                DomeData.domeHexTopRight = (domeData.OverlayId) newValue;
                break;
            case "domeright":
                DomeData.domeRight = (domeData.OverlayId) newValue;
                break;
            case "domesquarebottom":
                DomeData.domeSquareBottom = (domeData.OverlayId) newValue;
                break;
            case "domesquareleft":
                DomeData.domeSquareLeft = (domeData.OverlayId) newValue;
                break;
            case "domesquareright":
                DomeData.domeSquareRight = (domeData.OverlayId) newValue;
                break;
            case "domesquaretop":
                DomeData.domeSquareTop = (domeData.OverlayId) newValue;
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
            case "disableinput":
                SubControl.disableInput = newValue > 0;
                break;
            case "battery":
                BatteryData.battery = newValue;
                break;
            case "batterytemp":
                BatteryData.batteryTemp = newValue;
                break;
            case "batterycurrent":
                BatteryData.batteryCurrent = newValue;
                break;
            case "batterydrain":
                BatteryData.batteryDrain = newValue;
                break;
            case "batterylife":
                BatteryData.batteryLife = newValue;
                break;
            case "batterylifeenabled":
                BatteryData.batteryLifeEnabled = newValue > 0;
                break;
            case "batterylifemax":
                BatteryData.batteryLifeMax = newValue;
                break;
            case "batterytimeremaining":
                BatteryData.batteryTimeRemaining = newValue;
                break;
            case "batterytimeenabled":
                BatteryData.batteryTimeEnabled = newValue > 0;
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
            case "batteryerrorthreshold":
                BatteryData.batteryErrorThreshold = newValue;
                break;
            case "b1error":
                BatteryData.bank1Error = newValue;
                break;
            case "b2error":
                BatteryData.bank2Error = newValue;
                break;
            case "b3error":
                BatteryData.bank3Error = newValue;
                break;
            case "b4error":
                BatteryData.bank4Error = newValue;
                break;
            case "b5error":
                BatteryData.bank5Error = newValue;
                break;
            case "b6error":
                BatteryData.bank6Error = newValue;
                break;
            case "b7error":
                BatteryData.bank7Error = newValue;
                break;
            case "o1":
            case "oxygentank1":
                OxygenData.oxygenTank1 = newValue;
                break;
            case "o2":
            case "oxygentank2":
                OxygenData.oxygenTank2 = newValue;
                break;
            case "o3":
            case "oxygentank3":
                OxygenData.oxygenTank3 = newValue;
                break;
            case "o4":
            case "reserveoxygentank1":
                OxygenData.reserveOxygenTank1 = newValue;
                break;
            case "o5":
            case "reserveoxygentank2":
                OxygenData.reserveOxygenTank2 = newValue;
                break;
            case "o6":
            case "reserveoxygentank3":
                OxygenData.reserveOxygenTank3 = newValue;
                break;
            case "o7":
            case "reserveoxygentank4":
                OxygenData.reserveOxygenTank4 = newValue;
                break;
            case "o8":
            case "reserveoxygentank5":
                OxygenData.reserveOxygenTank5 = newValue;
                break;
            case "o9":
            case "reserveoxygentank6":
                OxygenData.reserveOxygenTank6 = newValue;
                break;
            case "oxygenflow":
                OxygenData.oxygenFlow = newValue;
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
            case "pressureoverride":
                CabinData.pressureOverride = newValue;
                break;
            case "pressureoverrideamount":
                CabinData.pressureOverrideAmount = newValue;
                break;
            case "watertempoverride":
                CabinData.waterTempOverride = newValue;
                break;
            case "watertempoverrideamount":
                CabinData.waterTempOverrideAmount = newValue;
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
            case "error_hatch":
                ErrorData.error_hatch = newValue;
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
            case "error_diagnostics":
                ErrorData.error_diagnostics = newValue;
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
            case "inputsource":
                inputSource = newValue;
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
            case "heading":
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
            case "crewheartpattern1":
                CrewData.crewHeartPattern1 = newValue;
                break;
            case "crewheartstrengthmin1":
                CrewData.crewHeartStrengthMin1 = newValue;
                break;
            case "crewheartstrengthmax1":
                CrewData.crewHeartStrengthMax1 = newValue;
                break;
            case "crewrespirationrate1":
                CrewData.crewRespirationRate1 = newValue;
                break;
            case "crewetco2min1":
                CrewData.crewETCO2Min1 = newValue;
                break;
            case "crewetco2max1":
                CrewData.crewETCO2Max1 = newValue;
                break;
            case "crewetco2pattern1":
                CrewData.crewETCO2Pattern1 = newValue;
                break;
            case "crewspo2min1":
                CrewData.crewSPO2Min1 = newValue;
                break;
            case "crewspo2max1":
                CrewData.crewSPO2Max1 = newValue;
                break;
            case "crewspo2pattern1":
                CrewData.crewSPO2Pattern1 = newValue;
                break;
            case "crewabpmin1":
                CrewData.crewABPMin1 = newValue;
                break;
            case "crewabpmax1":
                CrewData.crewABPMax1 = newValue;
                break;
            case "crewpapmin1":
                CrewData.crewPAPMin1 = newValue;
                break;
            case "crewpapmax1":
                CrewData.crewPAPMax1 = newValue;
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
                VesselData.SetPlayerVessel(Mathf.RoundToInt(newValue));
                break;
            case "latitude":
                MapData.latitude = newValue;
                break;
            case "longitude":
                MapData.longitude = newValue;
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
            case "pilotbuttonenabled":
                OperatingData.pilotButtonEnabled = newValue > 0;
                break;
            case "dockingbuttonenabled":
                OperatingData.dockingButtonEnabled = newValue > 0;
                break;
            case "screenglitchamount":
                ScreenData.screenGlitch = newValue;
                break;
            case "screenglitchautodecay":
                ScreenData.screenGlitchAutoDecay = newValue > 0;
                break;
            case "screenglitchautodecaytime":
                ScreenData.screenGlitchAutoDecayTime = newValue;
                break;
            case "screenglitchmaxdelay":
                ScreenData.screenGlitchMaxDelay = newValue;
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
			//case "motionbasepitch":
			//	SubControl.MotionBasePitch = newValue;
            //    break;
			//case "motionbaseyaw":
			//	SubControl.MotionBaseYaw = newValue;
			//	break;
			//case "motionbaseroll":
			//	SubControl.MotionBaseRoll = newValue;
			//	break;
			case "motionslerpspeed":
				SubControl.MotionSlerpSpeed = newValue;
				break;
			case "motionhazardsensitivity":
				SubControl.MotionHazardSensitivity = newValue;
				break;
			case "motionsafety":
				SubControl.MotionSafety = newValue > 0;
				break;
			case "motionhazard":
				SubControl.MotionHazard = newValue > 0;
				break;
			case "motionhazardenabled":
				SubControl.MotionHazardEnabled = newValue > 0;
				break;
			case "motionscaleimpacts":
				SubControl.MotionScaleImpacts = newValue;
				break;
			case "motionminimpactinterval":
				SubControl.MotionMinImpactInterval = newValue;
				break;
            case "sonarheadingup":
                SonarData.HeadingUp = newValue > 0;
                break;
            case "sonarlongfrequency":
                SonarData.LongFrequency = newValue;
                break;
            case "sonarlonggain":
                SonarData.LongGain = newValue;
                break;
            case "sonarlongrange":
                SonarData.LongRange = newValue;
                break;
            case "sonarlongsensitivity":
                SonarData.LongSensitivity = newValue;
                break;
            case "sonarproximity":
                SonarData.Proximity = newValue;
                break;
            case "sonarshortfrequency":
                SonarData.ShortFrequency = newValue;
                break;
            case "sonarshortgain":
                SonarData.ShortGain = newValue;
                break;
            case "sonarshortrange":
                SonarData.ShortRange = newValue;
                break;
            case "sonarshortsensitivity":
                SonarData.ShortSensitivity = newValue;
                break;
            case "error_thruster_l":
                GliderErrorData.error_thruster_l = newValue;
                break;
            case "error_thruster_r":
                GliderErrorData.error_thruster_r = newValue;
                break;
            case "error_vertran_l":
                GliderErrorData.error_vertran_l = newValue;
                break;
            case "error_vertran_r":
                GliderErrorData.error_vertran_r = newValue;
                break;
            case "error_jet_l":
                GliderErrorData.error_jet_l = newValue;
                break;
            case "error_jet_r":
                GliderErrorData.error_jet_r = newValue;
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
            case "error_panel_l":
                GliderErrorData.error_panel_l = newValue;
                break;
            case "error_panel_r":
                GliderErrorData.error_panel_r = newValue;
                break;
            case "error_pressure":
                GliderErrorData.error_pressure = newValue;
                break;
            case "error_structural":
                GliderErrorData.error_structural = newValue;
                break;
            case "error_grapple":
                GliderErrorData.error_grapple = newValue;
                break;
            case "error_system":
                GliderErrorData.error_system = newValue;
                break;
            case "dcccommscontent":
                DCCScreenData.DCCcommsContent = (int)newValue;
                break;
            case "dccvesselnameintitle":
                DCCScreenData.DCCvesselNameInTitle = newValue > 0;
                break;
            case "dcccommsusesliders":
                DCCScreenData.DCCcommsUseSliders = newValue > 0;
                break;
            case "isautopilot":
				SubControl.isAutoPilot = newValue > 0;
				break;
            case "iscontroldecentmode":
                SubControl.isControlDecentMode = newValue > 0;
				break;
            case "iscontroldecentmodeonjoystick":
                isControlDecentModeOnJoystick = newValue > 0;
                break;
            case "iscontrolmodeoverride":
                SubControl.isControlModeOverride = newValue > 0;
                break;
            case "iscontroloverridestandard":
                SubControl.isControlOverrideStandard = newValue > 0;
                break;
            case "camerabrightness":
                ScreenData.cameraBrightness = newValue;
                break;
            case "startimagesequence":
                ScreenData.startImageSequence = (int)newValue;
                break;
            case "greenscreenbrightness":
                ScreenData.greenScreenBrightness = newValue;
                break;
            case "acidlayer":
                MapData.acidLayer = (int)newValue;
                break;
            case "maxwildlife":
                SonarData.MaxWildlife = (int)newValue;
                break;
            case "lightarray1":
                LightData.lightArray1 = (int)newValue;
                break;
            case "lightarray2":
                LightData.lightArray2 = (int)newValue;
                break;
            case "lightarray3":
                LightData.lightArray3 = (int)newValue;
                break;
            case "lightarray4":
                LightData.lightArray4 = (int)newValue;
                break;
            case "lightarray5":
                LightData.lightArray5 = (int)newValue;
                break;
            case "lightarray6":
                LightData.lightArray6 = (int)newValue;
                break;
            case "lightarray7":
                LightData.lightArray7 = (int)newValue;
                break;
            case "lightarray8":
                LightData.lightArray8 = (int)newValue;
                break;
            case "lightarray9":
                LightData.lightArray9 = (int)newValue;
                break;
            case "lightarray10":
                LightData.lightArray10 = (int)newValue;
                break;
            case "docking1":
                DockingData.docking1 = (int)newValue;
                break;
            case "docking2":
                DockingData.docking2 = (int)newValue;
                break;
            case "docking3":
                DockingData.docking3 = (int)newValue;
                break;
            case "docking4":
                DockingData.docking4 = (int)newValue;
                break;
            case "bootcodeduration":
                PopupData.bootCodeDuration = newValue;
                break;
            case "bootprogress":
                PopupData.bootProgress = newValue;
                break;
            case "towtargetx":
                GLTowingData.towTargetX = newValue;
                break;
            case "towtargety":
                GLTowingData.towTargetY = newValue;
                break;
            case "towtargetspeed":
                GLTowingData.towTargetSpeed = newValue;
                break;
            case "towtargetvisible":
                GLTowingData.towTargetVisible = newValue > 0;
                break;
            case "towfiringpressure":
                GLTowingData.towFiringPressure = newValue;
                break;
            case "towfiringpower":
                GLTowingData.towFiringPower = newValue;
                break;
            case "towfiringstatus":
                GLTowingData.towFiringStatus = newValue;
                break;
            case "towlinespeed":
                GLTowingData.towLineSpeed = newValue;
                break;
            case "towlinelength":
                GLTowingData.towLineLength = newValue;
                break;
            case "towlineremaining":
                GLTowingData.towLineRemaining = newValue;
                break;
            case "towtargetdistance":
                GLTowingData.towTargetDistance = newValue;
                break;
            default:
                SetDynamicValue(new serverUtils.ServerValue(key, newValue), add);
                break;
        }

        if (ValueChangedEvent != null)
            ValueChangedEvent(valueName, newValue);
    }

    /** Set a shared server value at runtime. */
    public void SetDynamicValue(serverUtils.ServerValue value, bool add)
    {
        // Check that value name is valid.
        if (string.IsNullOrEmpty(value.key))
            return;

        // Try to replace an existing entry if possible.
        for (var i = 0; i < dynamicValues.Count; i++)
            if (string.Equals(dynamicValues[i].key, value.key, System.StringComparison.OrdinalIgnoreCase))
            {
                dynamicValues[i] = value;
                return;
            }
                
        // Otherwise, insert a new entry.
        if (add)
        {
            dynamicValues.Add(value);
            serverUtils.RegisterServerValue(value);
        }
    }

    /** Return a shared server value that has been defined at runtime. */
    public float GetDynamicValue(string valueName, float defaultValue = serverUtils.Unknown)
    {
        // Check that value name is valid.
        if (string.IsNullOrEmpty(valueName))
            return defaultValue;

        // Search for a matching entry in shared dynamic value list.
        for (var i = 0; i < dynamicValues.Count; i++)
            if (string.Equals(dynamicValues[i].key, valueName, System.StringComparison.OrdinalIgnoreCase))
            {
                serverUtils.RegisterServerValue(dynamicValues[i]);
                return dynamicValues[i].value;
            }

        return defaultValue;
    }

    /** Return a shared server value that has been defined at runtime. */
    public bool HasDynamicValue(string valueName)
    {
        // Check that value name is valid.
        if (string.IsNullOrEmpty(valueName))
            return false;

        // Search for a matching entry in shared dynamic value list.
        for (var i = 0; i < dynamicValues.Count; i++)
            if (string.Equals(dynamicValues[i].key, valueName, System.StringComparison.OrdinalIgnoreCase))
            {
                serverUtils.RegisterServerValue(dynamicValues[i]);
                return true;
            }

        return false;
    }

    /** Updates a server string value. */
    public void OnValueChanged(string valueName, string newValue)
    {
        switch (valueName.ToLower())
        {
            case "mapeventname":
                MapData.mapEventName = newValue;
                break;

            case "playervesselname":
                VesselData.PlayerVesselName = newValue;
                break;

            default:
                float value;
                if (float.TryParse(newValue, out value))
                    OnValueChanged(valueName, value);
                break;
        }
    }

    public void OnChangeBool(string boolName, bool newValue)
    {
        // Check if we're setting a vessel state value.
        if (VesselData.IsVesselKey(boolName))
        {
            VesselData.SetServerData(boolName, newValue ? 1 : 0);
            return;
        }

        // Match incoming key against known data values.
        switch (boolName.ToLower())
        {
            case "depthdisplayed":
                depthDisplayed = newValue;
                break;
            case "divetimeactive":
                diveTimeActive = newValue;
                break;
            case "duetimeactive":
                dueTimeActive = newValue;
                break;
            case "disableinput":
                SubControl.disableInput = newValue;
                break;
            case "floordistancedisplayed":
                floorDistanceDisplayed = newValue;
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
            case "batterylifeenabled":
                BatteryData.batteryLifeEnabled = newValue;
                break;
            case "batterytimeenabled":
                BatteryData.batteryTimeEnabled = newValue;
                break;
			case "isautopilot":
				SubControl.isAutoPilot = newValue;
				break;
            case "iscontroldecentmode":
                SubControl.isControlDecentMode = newValue;
                break;
            case "iscontroldecentmodeonjoystick":
                isControlDecentModeOnJoystick = newValue;
                break;
            case "iscontrolmodeoverride":
                SubControl.isControlModeOverride = newValue;
                break;
            case "iscontroloverridestandard":
                SubControl.isControlOverrideStandard = newValue;
                break;
			case "motionhazard":
				SubControl.MotionHazard = newValue;
				break;
			case "motionhazardenabled":
				SubControl.MotionHazardEnabled = newValue;
				break;
			case "motionsafety":
				SubControl.MotionSafety = newValue;
				break;
            case "pilotbuttonenabled":
                OperatingData.pilotButtonEnabled = newValue;
                break;
            case "dockingbuttonenabled":
                OperatingData.dockingButtonEnabled = newValue;
                break;
            case "dccvesselnameintitle":
                DCCScreenData.DCCvesselNameInTitle = newValue;
                break;
            case "dcccommsusesliders":
                DCCScreenData.DCCcommsUseSliders = newValue;
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

    /** Return the current displayed depth (defaults to player vessel depth, can be overridden. */
    public float GetDepth()
    {
        // Determine the proportion of overriding to apply.
        // 0 = No overriding (use player vessel depth only).
        // 1 = Full overriding (use override depth only).
        // 0.5 = Interpolate halfway between the two.
        var t = Mathf.Clamp01(depthOverrideAmount);

        // Interpolate between actual and override depth to reach displayed depth.
        return Mathf.Lerp(depth, depthOverride, t);
    }

    [ClientRpc]
    public void RpcImpact(Vector3 impactVector)
    {
        // Delegate impact handling to the sub controller.
        SubControl.Impact(impactVector);
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

    /** Network pre-startup notification for a client. */
    public override void PreStartClient()
    {
        base.PreStartClient();
        Debug.Log("serverData.PreStartClient(): ID: " + serverUtils.Id);
    }

    /** Network startup notification for a client. */
    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("serverData.OnStartClient(): ID: " + serverUtils.Id);
    }

    /** Network startup notification for a server. */
    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("serverData.OnStartServer(): ID: " + serverUtils.Id);
    }

    /** Network destruction notification. */
    public override void OnNetworkDestroy()
    {
        base.OnNetworkDestroy();
        Debug.Log("serverData.OnNetworkDestroy(): ID: " + serverUtils.Id);
    }

    /** Initialization. */
    void Start ()
    {
        Debug.Log("serverData.Start(): Client ID: " + serverUtils.Id);
        rb = gameObject.GetComponent<Rigidbody>();

        // Register for dynamic value change notifications.
        dynamicValues.Callback += OnDynamicValueChanged;
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
        floorDistance = Mathf.Max(0, floorDepth - GetDepth());

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
        // Update the sub control logic.
        GetComponent<SubControl>().SubController();

        // Update roll, pitch and yaw.
        if (isServer)
            GetRollPitchYaw(transform.rotation);
    }

    /** Handle changes to dynamic server values. */
    private void OnDynamicValueChanged(SyncListValues.Operation op, int index)
    {
        // TODO: Update a dictionary to get O(1) lookups for dynamic values.
        // UpdateDynamicLookup();
    }


    #endregion
}
