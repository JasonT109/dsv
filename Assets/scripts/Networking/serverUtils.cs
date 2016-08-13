using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Meg.EventSystem;
using Meg.Graphics;
using UnityEngine.Networking;

namespace Meg.Networking
{
    public class serverUtils : MonoBehaviour
    {

        // Constants
        // ------------------------------------------------------------

        /** Return value representing an unknown server data value. */
        public const float Unknown = -1;


        // Static Properties
        // ------------------------------------------------------------

        /** A unique identifier for this game instance (supplied on the commandline). */
        private static string _id;
        public static string Id
        {
            get
            {
                if (string.IsNullOrEmpty(_id))
                    _id = Configuration.Instance.CurrentId;

                return _id;
            }
        }

        /** Return the local player object (used to push commands to server.) */
        public static serverPlayer LocalPlayer
        {
            get
            {
                var player = ClientScene.localPlayers.First();
                if (player != null)
                    return player.gameObject.GetComponent<serverPlayer>();

                return null;
            }
        }

        /** Return the server object (contains all data objects.) */
        private static GameObject _serverObject;
        public static GameObject ServerObject
        {
            get
            {
                if (!_serverObject)
                    _serverObject = GameObject.FindWithTag("ServerData");
                return _serverObject;
            }
        }

        /** Return the server data object (contains shared core state values.) */
        private static serverData _serverData;
        public static serverData ServerData
        {
            get
            {
                if (!_serverData && ServerObject)
                    _serverData = ServerObject.GetComponent<serverData>();
                return _serverData;
            }
        }

        /** Return the error data object (contains shared error flag values.) */
        private static errorData _errorData;
        public static errorData ErrorData
        {
            get
            {
                if (!_errorData && ServerObject)
                    _errorData = ServerObject.GetComponent<errorData>();
                return _errorData;
            }
        }

        /** Return the glider error data object. */
        private static gliderErrorData _gliderErrorData;
        public static gliderErrorData GliderErrorData
        {
            get
            {
                if (!_gliderErrorData && ServerObject)
                    _gliderErrorData = ServerObject.GetComponent<gliderErrorData>();
                return _gliderErrorData;
            }
        }

        /** Return the map data object (contains shared map state values.) */
        private static mapData _mapData;
        public static mapData MapData
        {
            get
            {
                if (!_mapData && ServerObject)
                    _mapData = ServerObject.GetComponent<mapData>();
                return _mapData;
            }
        }

        /** Return the vessel data object (contains shared vessel state values.) */
        private static vesselData _vesselData;
        public static vesselData VesselData
        {
            get
            {
                if (!_vesselData && ServerObject)
                    _vesselData = ServerObject.GetComponent<vesselData>();
                return _vesselData;
            }
        }

        /** Return the crew data object (contains shared crew state values.) */
        private static crewData _crewData;
        public static crewData CrewData
        {
            get
            {
                if (!_crewData && ServerObject)
                    _crewData = ServerObject.GetComponent<crewData>();
                return _crewData;
            }
        }

        /** Return the oxygen data object (contains shared oxygen state values.) */
        private static oxygenData _oxygenData;
        public static oxygenData OxygenData
        {
            get
            {
                if (!_oxygenData && ServerObject)
                    _oxygenData = ServerObject.GetComponent<oxygenData>();
                return _oxygenData;
            }
        }

        /** Return the air data object. */
        private static airData _airData;
        public static airData AirData
        {
            get
            {
                if (!_airData && ServerObject)
                    _airData = ServerObject.GetComponent<airData>();
                return _airData;
            }
        }

        /** Return the cabin data object. */
        private static cabinData _cabinData;
        public static cabinData CabinData
        {
            get
            {
                if (!_cabinData && ServerObject)
                    _cabinData = ServerObject.GetComponent<cabinData>();
                return _cabinData;
            }
        }

        /** Return the battery data object (contains shared battery state values.) */
        private static batteryData _batteryData;
        public static batteryData BatteryData
        {
            get
            {
                if (!_batteryData && ServerObject)
                    _batteryData = ServerObject.GetComponent<batteryData>();
                return _batteryData;
            }
        }

        /** Return the operating data object (contains shared operating state values.) */
        private static operatingData _operatingData;
        public static operatingData OperatingData
        {
            get
            {
                if (!_operatingData && ServerObject)
                    _operatingData = ServerObject.GetComponent<operatingData>();
                return _operatingData;
            }
        }

        /** Return the sonar data object (contains shared sonar state values.) */
        private static SonarData _sonarData;
        public static SonarData SonarData
        {
            get
            {
                if (!_sonarData && ServerObject)
                    _sonarData = ServerObject.GetComponent<SonarData>();
                return _sonarData;
            }
        }

        /** Return the sub control data object. */
        private static SubControl _subControl;
        public static SubControl SubControl
        {
            get
            {
                if (!_subControl && ServerObject)
                    _subControl = ServerObject.GetComponent<SubControl>();
                return _subControl;
            }
        }

        /** Return the DCC screens data object. */
        private static DCCScreenData _DCCScreenControl;
        public static DCCScreenData DCCScreenData
        {
            get
            {
                if (!_DCCScreenControl)
                    _DCCScreenControl = ServerObject.GetComponent<DCCScreenData>();
                return _DCCScreenControl;
            }
        }

        /** Return the Dome screens data object. */
        private static domeData _domeData;
        public static domeData DomeData
        {
            get
            {
                if (!_domeData && ServerObject)
                    _domeData = ServerObject.GetComponent<domeData>();
                return _domeData;
            }
        }

        /** Return the vessel movements controller. */
        public static vesselMovements VesselMovements
            { get { return GetVesselMovements(); } }

        /** Whether the server object is available for use yet. */
        public static bool IsReady()
            { return ServerObject != null; }

        /** Whether this machine is the server. */
        public static bool IsServer()
            { return ServerData && ServerData.isServer; }

        /** The set of all server data parameters that can be written to. */
        private static HashSet<string> _writeableParameters;
        public static HashSet<string> WriteableParameters
        {
            get
            {
                if (_writeableParameters != null)
                    return _writeableParameters;

                _writeableParameters = new HashSet<string>();
                foreach (var parameter in Parameters)
                    if (!GetServerDataInfo(parameter).readOnly)
                        _writeableParameters.Add(parameter.ToLower());

                return _writeableParameters;
            }
        }

        /** The set of all server data parameters. */
        private static HashSet<string> _parameters;
        public static HashSet<string> Parameters
            { get { return _parameters ?? (_parameters = new HashSet<string>(_parameterSet.Select(x => x.ToLower()))); } }

        /** The set of all server data parameters that can be set or read. */
        private static readonly HashSet<string> _parameterSet = new HashSet<string>
        {
            "acceleration",
            "air",
            "airtank1",
            "airtank2",
            "airtank3",
            "airtank4",
            "b1",
            "b2",
            "b3",
            "b4",
            "b5",
            "b6",
            "b7",
            "b1error",
            "b2error",
            "b3error",
            "b4error",
            "b5error",
            "b6error",
            "b7error",
            "ballastpressure",
            "battery",
            "batterycurrent",
            "batterydrain",
            "batteryerrorthreshold",
            "batterylife",
            "batterylifeenabled",
            "batterylifemax",
            "batterytemp",
            "batterytimeenabled",
            "batterytimeremaining",
            "cabinhumidity",
            "cabinoxygen",
            "cabinpressure",
            "cabinpressurepsi",
            "cabintemp",
            "co2",
            "co2ppm",
            "commssignalstrength",
            "crewbodytemp1",
            "crewbodytemp2",
            "crewbodytemp3",
            "crewbodytemp4",
            "crewbodytemp5",
            "crewbodytemp6",
            "crewheartrate1",
            "crewheartrate2",
            "crewheartrate3",
            "crewheartrate4",
            "crewheartrate5",
            "crewheartrate6",
            "dccquadscreen0",
            "dccquadscreen1",
            "dccquadscreen2",
            "dccquadscreen3",
            "dccquadscreen4",
            "depth",
            "disableinput",
            "divertpowertothrusters",
            "divetime",
            "divetimeactive",
            "domecenter",
            "domecornerbottomleft",
            "domecornerbottomright",
            "domecornertopleft",
            "domecornertopright",
            "domeleft",
            "domehexbottomleft",
            "domehexbottomright",
            "domehextopleft",
            "domehextopright",
            "domeright",
            "domesquarebottom",
            "domesquareleft",
            "domesquareright",
            "domesquaretop",
            "duetime",
            "duetimeactive",
            "error_ballasttank",
            "error_batteryleak",
            "error_bilgeleak",
            "error_bowlights",
            "error_bowthruster",
            "error_cpu",
            "error_datahd",
            "error_depthsonar",
            "error_doppler",
            "error_electricleak",
            "error_forwardsonar",
            "error_gps",
            "error_hydraulicpump",
            "error_hyrdaulicres",
            "error_jet_l",
            "error_jet_r",
            "error_oxygenext",
            "error_oxygenpump",
            "error_portlights",
            "error_radar",
            "error_runninglights",
            "error_starboardlights",
            "error_sternlights",
            "error_thruster_l",
            "error_thruster_r",
            "error_tow",
            "error_vertran_l",
            "error_vertran_r",
            "error_vhf",
            "error_vidhd",
            "floordepth",
            "floordistance",
            "genericerror",
            "heading",
            "horizontalvelocity",
            "hydraulicpressure",
            "hydraulictemp",
            "initiatemapevent",
            "inputxaxis",
            "inputxaxis2",
            "inputyaxis",
            "inputyaxis2",
            "inputzaxis",
            "intercept1posx",
            "intercept1posy",
            "intercept1posz",
            "intercept1velocity",
            "intercept1vis",
            "isautostabilised",
            "ispitchalsostabilised",
            "jet_heat_l",
            "jet_heat_r",
            "joystickoverride",
            "joystickpilot",
            "latitude",
            "longitude",
            "maxspeed",
            "minspeed",
            "meg1posx",
            "meg1posy",
            "meg1posz",
            "meg1velocity",
            "meg1vis",
            "megspeed",
            "megturnspeed",
            "o1",
            "o2",
            "o3",
            "o4",
            "o5",
            "o6",
            "o7",
            "o8",
            "o9",
            "oxygen",
            "oxygenflow",
            "oxygentank1",
            "oxygentank2",
            "oxygentank3",
            "pitchangle",
            "pitchspeed",
            "playervessel",
            "pressure",
            "reserveair",
            "reserveairtank1",
            "reserveairtank2",
            "reserveairtank3",
            "reserveairtank4",
            "reserveairtank5",
            "reserveairtank6",
            "reserveairtank7",
            "reserveairtank8",
            "reserveairtank9",
            "reserveoxygen",
            "reserveoxygentank1",
            "reserveoxygentank2",
            "reserveoxygentank3",
            "reserveoxygentank4",
            "reserveoxygentank5",
            "reserveoxygentank6",
            "rollangle",
            "rollspeed",
            "scene",
            "scrubbedco2",
            "scrubbedhumidity",
            "scrubbedoxygen",
            "shot",
            "sonarlongfrequency",
            "sonarlonggain",
            "sonarlongrange",
            "sonarlongsensitivity",
            "sonarshortfrequency",
            "sonarshortgain",
            "sonarshortrange",
            "sonarshortsensitivity",
            "take",
            "thruster_heat_l",
            "thruster_heat_r",
            "timetointercept",
            "towwinchload",
            "v1posx",
            "v1posy",
            "v1posz",
            "v1depth",
            "v1velocity",
            "v2posx",
            "v2posy",
            "v2posz",
            "v2depth",
            "v2velocity",
            "v3posx",
            "v3posy",
            "v3posz",
            "v3depth",
            "v3velocity",
            "v4posx",
            "v4posy",
            "v4posz",
            "v4depth",
            "v4velocity",
            "v5posx",
            "v5posy",
            "v5posz",
            "v5depth",
            "v5velocity",
            "v6posx",
            "v6posy",
            "v6posz",
            "v6depth",
            "v5velocity",
            "variableballastpressure",
            "variableballasttemp",
            "velocity",
            "verticalvelocity",
            "vertran_heat_l",
            "vertran_heat_r",
            "vessel1depth",
            "vessel1vis",
            "vessel1icon",
            "vessel2depth",
            "vessel2vis",
            "vessel2icon",
            "vessel3depth",
            "vessel3vis",
            "vessel3icon",
            "vessel4depth",
            "vessel4vis",
            "vessel4icon",
            "vessel4depth",
            "vessel5vis",
            "vessel5icon",
            "vessel6depth",
            "vessel6vis",
            "vessel6icon",
            "vesselmovementenabled",
            "watertemp",
            "xpos",
            "yawangle",
            "yawspeed",
            "zpos",
        };

        /** Types of parameter value. */
        public enum ParameterType
        {
            Float,
            Int,
            Bool
        }

        /** Configuration data for a parameter. */
        public struct ParameterInfo
        {
            public ParameterType type;
            public float minValue;
            public float maxValue;
            public bool readOnly;

            public ParameterInfo(ParameterType type)
            {
                this.type = type;
                minValue = 0;
                maxValue = 100;
                readOnly = false;
            }
        }

        /** Structure for tracking a dynamic server value. */
        [System.Serializable]
        public struct ServerValue
        {
            public string key;
            public float value;
            public ParameterInfo info;

            public ServerValue(string key, float value)
            {
                this.key = key;
                this.value = value;
                info = DefaultParameterInfo;
            }
        }

        /** Default parameter configuration. */
        public static readonly ParameterInfo DefaultParameterInfo = new ParameterInfo(ParameterType.Float);

        /** The set of all server data parameter info entries. */
        private static Dictionary<string, ParameterInfo> _parameterInfos;
        public static Dictionary<string, ParameterInfo> ParameterInfos
        {
            get
            {
                if (_parameterInfos != null)
                    return _parameterInfos;

                _parameterInfos = new Dictionary<string, ParameterInfo>();
                foreach (var info in ParameterData)
                    _parameterInfos.Add(info.Key.ToLower(), info.Value);

                return _parameterInfos;
            }
        }

        /** Metadata about various server parameters. */
        private static readonly Dictionary<string, ParameterInfo> ParameterData = new Dictionary<string, ParameterInfo>
        {
            { "air", new ParameterInfo { readOnly = true } },
            { "battery", new ParameterInfo { readOnly = true } },
            { "batterycurrent", new ParameterInfo { maxValue = 30 } },
            { "batterydrain", new ParameterInfo { maxValue = 1 } },
            { "batterylife", new ParameterInfo { maxValue = 128 } },
            { "batterylifemax", new ParameterInfo { maxValue = 128 } },
            { "batterylifeenabled", new ParameterInfo { maxValue = 1, type = ParameterType.Bool } },
            { "batterytimeenabled", new ParameterInfo { maxValue = 1, type = ParameterType.Bool } },
            { "batterytimeremaining", new ParameterInfo { maxValue = 3600 * 12, type = ParameterType.Int } },
            { "oxygen", new ParameterInfo { readOnly = true } },
            { "playervessel", new ParameterInfo { minValue = 1, maxValue = 4, type = ParameterType.Int } },
            { "depth", new ParameterInfo { maxValue = 12000 } },
            { "divetimeactive", new ParameterInfo { maxValue = 1, type = ParameterType.Bool } },
            { "duetimeactive", new ParameterInfo { maxValue = 1, type = ParameterType.Bool } },
            { "vessel1vis", new ParameterInfo { maxValue = 1, type = ParameterType.Bool } },
            { "vessel2vis", new ParameterInfo { maxValue = 1, type = ParameterType.Bool } },
            { "vessel3vis", new ParameterInfo { maxValue = 1, type = ParameterType.Bool } },
            { "vessel4vis", new ParameterInfo { maxValue = 1, type = ParameterType.Bool } },
            { "vessel5vis", new ParameterInfo { maxValue = 1, type = ParameterType.Bool } },
            { "vessel6vis", new ParameterInfo { maxValue = 1, type = ParameterType.Bool } },
            { "meg1vis", new ParameterInfo { maxValue = 1, type = ParameterType.Bool } },
            { "intercept1vis", new ParameterInfo { maxValue = 1, type = ParameterType.Bool } },
            { "vessel1icon", new ParameterInfo { maxValue = 1, type = ParameterType.Int } },
            { "vessel2icon", new ParameterInfo { maxValue = 1, type = ParameterType.Int } },
            { "vessel3icon", new ParameterInfo { maxValue = 1, type = ParameterType.Int } },
            { "vessel4icon", new ParameterInfo { maxValue = 1, type = ParameterType.Int } },
            { "vessel5icon", new ParameterInfo { maxValue = 1, type = ParameterType.Int } },
            { "vessel6icon", new ParameterInfo { maxValue = 1, type = ParameterType.Int } },
            { "vessel1depth", new ParameterInfo { maxValue = 12000, type = ParameterType.Int } },
            { "vessel2depth", new ParameterInfo { maxValue = 12000, type = ParameterType.Int } },
            { "vessel3depth", new ParameterInfo { maxValue = 12000, type = ParameterType.Int } },
            { "vessel4depth", new ParameterInfo { maxValue = 12000, type = ParameterType.Int } },
            { "vessel5depth", new ParameterInfo { maxValue = 12000, type = ParameterType.Int } },
            { "vessel6depth", new ParameterInfo { maxValue = 12000, type = ParameterType.Int } },
            { "meg1depth", new ParameterInfo { maxValue = 12000, type = ParameterType.Int } },
            { "intercept1depth", new ParameterInfo { maxValue = 12000, type = ParameterType.Int } },
            { "disableinput", new ParameterInfo { maxValue = 1, type = ParameterType.Bool } },
            { "isautostabilised", new ParameterInfo { maxValue = 1, type = ParameterType.Bool } },
            { "ispitchalsostabilised", new ParameterInfo { maxValue = 1, type = ParameterType.Bool } },
            { "joystickoverride", new ParameterInfo { maxValue = 1, type = ParameterType.Bool } },
            { "joystickpilot", new ParameterInfo { maxValue = 1, type = ParameterType.Bool } },
            { "inputxaxis", new ParameterInfo { minValue = -1, maxValue = 1 } },
            { "inputyaxis", new ParameterInfo { minValue = -1, maxValue = 1 } },
            { "inputzaxis", new ParameterInfo { minValue = -1, maxValue = 1 } },
            { "inputxaxis2", new ParameterInfo { minValue = -1, maxValue = 1 } },
            { "inputyaxis2", new ParameterInfo { minValue = -1, maxValue = 1 } },
            { "pitchangle", new ParameterInfo { minValue = -90, maxValue = 90 } },
            { "yawangle", new ParameterInfo { minValue = 0, maxValue = 360 } },
            { "rollangle", new ParameterInfo { minValue = -90, maxValue = 90 } },
            { "scene", new ParameterInfo { minValue = 1, maxValue = 200, type = ParameterType.Int } },
            { "shot", new ParameterInfo { minValue = 1, maxValue = 20, type = ParameterType.Int } },
            { "take", new ParameterInfo { minValue = 1, maxValue = 20, type = ParameterType.Int } },
            { "dccquadscreen0", new ParameterInfo { minValue = 0, maxValue = 20, type = ParameterType.Int } },
            { "dccquadscreen1", new ParameterInfo { minValue = 0, maxValue = 20, type = ParameterType.Int } },
            { "dccquadscreen2", new ParameterInfo { minValue = 0, maxValue = 20, type = ParameterType.Int } },
            { "dccquadscreen3", new ParameterInfo { minValue = 0, maxValue = 20, type = ParameterType.Int } },
            { "dccquadscreen4", new ParameterInfo { minValue = 0, maxValue = 20, type = ParameterType.Int } },
            { "dccfullscreen", new ParameterInfo { minValue = 0, maxValue = 1, type = ParameterType.Int } },
            { "co2", new ParameterInfo { maxValue = 5 } },
            { "co2ppm", new ParameterInfo { readOnly = true } },
            { "cabinpressurepsi", new ParameterInfo { readOnly = true } },
            { "scrubbedco2", new ParameterInfo { maxValue = 5 } },
            { "cabinpressure", new ParameterInfo { maxValue = 1.25f } },
            { "domecenter", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int } },
            { "domecornerbottomleft", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int } },
            { "domecornerbottomright", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int } },
            { "domecornertopleft", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int } },
            { "domecornertopright", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int } },
            { "domeleft", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int } },
            { "domehexbottomleft", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int } },
            { "domehexbottomright", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int } },
            { "domehextopleft", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int } },
            { "domehextopright", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int } },
            { "domeright", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int } },
            { "domesquarebottom", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int } },
            { "domesquareleft", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int } },
            { "domesquareright", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int } },
            { "domesquaretop", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int } },
            { "sonarshortfrequency", new ParameterInfo { minValue = 0, maxValue = 1000 } },
            { "sonarshortrange", new ParameterInfo { minValue = 30, maxValue = 120, type = ParameterType.Int } },
            { "sonarshortgain", new ParameterInfo { minValue = 50, maxValue = 110} },
            { "sonarshortsensitivity", new ParameterInfo { minValue = 0, maxValue = 110} },
            { "sonarlongfrequency", new ParameterInfo { minValue = 0, maxValue = 250 } },
            { "sonarlongrange", new ParameterInfo { minValue = 1000, maxValue = 6000, type = ParameterType.Int } },
            { "sonarlonggain", new ParameterInfo { minValue = 50, maxValue = 110} },
            { "sonarlongsensitivity", new ParameterInfo { minValue = 0, maxValue = 110} },
        };
        
        /** Return information about a given parameter. */
        public static ParameterInfo GetServerDataInfo(string valueName)
        {
            var key = valueName.ToLower();
            if (ParameterInfos.ContainsKey(key))
                return ParameterInfos[key];

            return DefaultParameterInfo;
        }

        /** Return the current value of a shared state value, indexed by name. */
        public static float GetServerData(string valueName, float defaultValue = Unknown)
        {
            if (!ServerObject || string.IsNullOrEmpty(valueName))
                return defaultValue;

            // Check if we're looking for a vessel state value.
            if (VesselData.IsVesselKey(valueName))
                return VesselData.GetServerData(valueName, defaultValue);

            switch (valueName.ToLower())
            {
                case "scene":
                    return ServerData.scene;
                case "shot":
                    return ServerData.shot;
                case "take":
                    return ServerData.take;
                case "depth":
                    return ServerData.depth;
                case "xpos":
                    return ServerObject.transform.position.x;
                case "zpos":
                    return ServerObject.transform.position.z;
                case "pressure":
                    return ServerData.pressure;
                case "heading":
                    return ServerData.heading;
                case "pitchangle":
                    return ServerData.pitchAngle;
                case "yawangle":
                    return ServerData.yawAngle;
                case "rollangle":
                    return ServerData.rollAngle;
                case "velocity":
                    return ServerData.velocity;
                case "floordepth":
                    return ServerData.floorDepth;
                case "floordistance":
                    return ServerData.floorDistance;
                case "divetime":
                    return ServerData.diveTime;
                case "divetimeactive":
                    return ServerData.diveTimeActive ? 1 : 0;
                case "duetime":
                    return ServerData.dueTime;
                case "duetimeactive":
                    return ServerData.dueTimeActive ? 1 : 0;
                case "watertemp":
                    return ServerData.waterTemp;
                case "battery":
                    return BatteryData.battery;
                case "batterycurrent":
                    return BatteryData.batteryCurrent;
                case "batterydrain":
                    return BatteryData.batteryDrain;
                case "batteryerrorthreshold":
                    return BatteryData.batteryErrorThreshold;
                case "batterylife":
                    return BatteryData.batteryLife;
                case "batterylifemax":
                    return BatteryData.batteryLifeMax;
                case "batterylifeenabled":
                    return BatteryData.batteryLifeEnabled ? 1 : 0;
                case "batterytemp":
                    return BatteryData.batteryTemp;
                case "batterytimeremaining":
                    return BatteryData.batteryTimeRemaining;
                case "batterytimeenabled":
                    return BatteryData.batteryTimeEnabled ? 1 : 0;
                case "b1":
                    return BatteryData.bank1;
                case "b2":
                    return BatteryData.bank2;
                case "b3":
                    return BatteryData.bank3;
                case "b4":
                    return BatteryData.bank4;
                case "b5":
                    return BatteryData.bank5;
                case "b6":
                    return BatteryData.bank6;
                case "b7":
                    return BatteryData.bank7;
                case "b1error":
                    return BatteryData.bank1Error;
                case "b2error":
                    return BatteryData.bank2Error;
                case "b3error":
                    return BatteryData.bank3Error;
                case "b4error":
                    return BatteryData.bank4Error;
                case "b5error":
                    return BatteryData.bank5Error;
                case "b6error":
                    return BatteryData.bank6Error;
                case "b7error":
                    return BatteryData.bank7Error;
                case "oxygen":
                    return OxygenData.oxygen;
                case "oxygenlitres":
                    return OxygenData.oxygenLitres;
                case "o1":
                case "oxygentank1":
                    return OxygenData.oxygenTank1;
                case "o2":
                case "oxygentank2":
                    return OxygenData.oxygenTank2;
                case "o3":
                case "oxygentank3":
                    return OxygenData.oxygenTank3;
                case "reserveoxygen":
                    return OxygenData.reserveOxygen;
                case "reserveoxygenlitres":
                    return OxygenData.reserveOxygenLitres;
                case "o4":
                case "reserveoxygentank1":
                    return OxygenData.reserveOxygenTank1;
                case "o5":
                case "reserveoxygentank2":
                    return OxygenData.reserveOxygenTank2;
                case "o6":
                case "reserveoxygentank3":
                    return OxygenData.reserveOxygenTank3;
                case "o7":
                case "reserveoxygentank4":
                    return OxygenData.reserveOxygenTank4;
                case "o8":
                case "reserveoxygentank5":
                    return OxygenData.reserveOxygenTank5;
                case "o9":
                case "reserveoxygentank6":
                    return OxygenData.reserveOxygenTank6;
                case "oxygenflow":
                    return OxygenData.oxygenFlow;
                case "air":
                    return AirData.air;
                case "airlitres":
                    return AirData.airLitres;
                case "airtank1":
                    return AirData.airTank1;
                case "airtank2":
                    return AirData.airTank2;
                case "airtank3":
                    return AirData.airTank3;
                case "airtank4":
                    return AirData.airTank4;
                case "reserveair":
                    return AirData.reserveAir;
                case "reserveairlitres":
                    return AirData.reserveAirLitres;
                case "reserveairtank1":
                    return AirData.reserveAirTank1;
                case "reserveairtank2":
                    return AirData.reserveAirTank2;
                case "reserveairtank3":
                    return AirData.reserveAirTank3;
                case "reserveairtank4":
                    return AirData.reserveAirTank4;
                case "reserveairtank5":
                    return AirData.reserveAirTank5;
                case "reserveairtank6":
                    return AirData.reserveAirTank6;
                case "reserveairtank7":
                    return AirData.reserveAirTank7;
                case "reserveairtank8":
                    return AirData.reserveAirTank8;
                case "reserveairtank9":
                    return AirData.reserveAirTank9;
                case "co2":
                    return CabinData.Co2;
                case "co2ppm":
                    return CabinData.Co2 * Conversions.PercentToPartsPerMillion;
                case "cabinpressure":
                    return CabinData.cabinPressure;
                case "cabinpressurepsi":
                    return CabinData.cabinPressure * Conversions.BarToPsi;
                case "cabinoxygen":
                    return CabinData.cabinOxygen;
                case "cabintemp":
                    return CabinData.cabinTemp;
                case "cabinhumidity":
                    return CabinData.cabinHumidity;
                case "scrubbedco2":
                    return CabinData.scrubbedCo2;
                case "scrubbedhumidity":
                    return CabinData.scrubbedHumidity;
                case "scrubbedoxygen":
                    return CabinData.scrubbedOxygen;
                case "error_bilgeleak":
                    return ErrorData.error_bilgeLeak;
                case "error_batteryleak":
                    return ErrorData.error_batteryLeak;
                case "error_electricleak":
                    return ErrorData.error_electricLeak;
                case "error_oxygenext":
                    return ErrorData.error_oxygenExt;
                case "error_vhf":
                    return ErrorData.error_vhf;
                case "error_forwardsonar":
                    return ErrorData.error_forwardSonar;
                case "error_depthsonar":
                    return ErrorData.error_depthSonar;
                case "error_doppler":
                    return ErrorData.error_doppler;
                case "error_gps":
                    return ErrorData.error_gps;
                case "error_cpu":
                    return ErrorData.error_cpu;
                case "error_vidhd":
                    return ErrorData.error_vidhd;
                case "error_datahd":
                    return ErrorData.error_datahd;
                case "error_tow":
                    return ErrorData.error_tow;
                case "error_radar":
                    return ErrorData.error_radar;
                case "error_sternlights":
                    return ErrorData.error_sternLights;
                case "error_bowlights":
                    return ErrorData.error_bowLights;
                case "error_portlights":
                    return ErrorData.error_portLights;
                case "error_bowthruster":
                    return ErrorData.error_bowThruster;
                case "error_hyrdaulicres":
                    return ErrorData.error_hyrdaulicRes;
                case "error_starboardlights":
                    return ErrorData.error_starboardLights;
                case "error_runninglights":
                    return ErrorData.error_runningLights;
                case "error_ballasttank":
                    return ErrorData.error_ballastTank;
                case "error_hydraulicpump":
                    return ErrorData.error_hydraulicPump;
                case "error_oxygenpump":
                    return ErrorData.error_oxygenPump;
                case "genericerror":
                    return ErrorData.genericerror;
                case "error_thruster_l":
                    return GliderErrorData.error_thruster_l;
                case "error_thruster_r":
                    return GliderErrorData.error_thruster_r;
                case "error_vertran_l":
                    return GliderErrorData.error_vertran_l;
                case "error_vertran_r":
                    return GliderErrorData.error_vertran_r;
                case "error_jet_l":
                    return GliderErrorData.error_jet_l;
                case "error_jet_r":
                    return GliderErrorData.error_jet_r;
                case "thruster_heat_l":
                    return GliderErrorData.thruster_heat_l;
                case "thruster_heat_r":
                    return GliderErrorData.thruster_heat_r;
                case "vertran_heat_l":
                    return GliderErrorData.vertran_heat_l;
                case "vertran_heat_r":
                    return GliderErrorData.vertran_heat_r;
                case "jet_heat_l":
                    return GliderErrorData.jet_heat_l;
                case "jet_heat_r":
                    return GliderErrorData.jet_heat_r;
                case "disableinput":
                    return SubControl.disableInput ? 1 : 0;
                case "inputxaxis":
                    return SubControl.inputXaxis;
                case "inputyaxis":
                    return SubControl.inputYaxis;
                case "inputzaxis":
                    return SubControl.inputZaxis;
                case "inputxaxis2":
                    return SubControl.inputXaxis2;
                case "inputyaxis2":
                    return SubControl.inputYaxis2;
                case "isautostabilised":
                    return SubControl.isAutoStabilised ? 1 : 0;
                case "ispitchalsostabilised":
                    return SubControl.IsPitchAlsoStabilised ? 1 : 0;
                case "joystickoverride":
                    return SubControl.JoystickOverride ? 1 : 0;
                case "joystickpilot":
                    return SubControl.JoystickPilot ? 1 : 0;
                case "acceleration":
                    return SubControl.Acceleration;
                case "yawspeed":
                    return SubControl.yawSpeed;
                case "pitchspeed":
                    return SubControl.pitchSpeed;
                case "rollspeed":
                    return SubControl.rollSpeed;
                case "maxspeed":
                    return SubControl.MaxSpeed;
                case "minspeed":
                    return SubControl.MinSpeed;
                case "verticalvelocity":
                    return ServerData.verticalVelocity;
                case "horizontalvelocity":
                    return ServerData.horizontalVelocity;
                case "crewheartrate1":
                    return CrewData.crewHeartRate1;
                case "crewheartrate2":
                    return CrewData.crewHeartRate2;
                case "crewheartrate3":
                    return CrewData.crewHeartRate3;
                case "crewheartrate4":
                    return CrewData.crewHeartRate4;
                case "crewheartrate5":
                    return CrewData.crewHeartRate5;
                case "crewheartrate6":
                    return CrewData.crewHeartRate6;
                case "crewbodytemp1":
                    return CrewData.crewBodyTemp1;
                case "crewbodytemp2":
                    return CrewData.crewBodyTemp2;
                case "crewbodytemp3":
                    return CrewData.crewBodyTemp3;
                case "crewbodytemp4":
                    return CrewData.crewBodyTemp4;
                case "crewbodytemp5":
                    return CrewData.crewBodyTemp5;
                case "crewbodytemp6":
                    return CrewData.crewBodyTemp6;
                case "playervessel":
                    return VesselData.PlayerVessel;
                case "initiatemapevent":
                    return MapData.initiateMapEvent;
                case "latitude":
                    return MapData.latitude;
                case "longitude":
                    return MapData.longitude;
                case "towwinchload":
                    return OperatingData.towWinchLoad;
                case "hydraulictemp":
                    return OperatingData.hydraulicTemp;
                case "hydraulicpressure":
                    return OperatingData.hydraulicPressure;
                case "ballastpressure":
                    return OperatingData.ballastPressure;
                case "variableballasttemp":
                    return OperatingData.variableBallastTemp;
                case "variableballastpressure":
                    return OperatingData.variableBallastPressure;
                case "commssignalstrength":
                    return OperatingData.commsSignalStrength;
                case "divertpowertothrusters":
                    return OperatingData.divertPowerToThrusters;
                case "vesselmovementenabled":
                    return VesselMovements.Enabled ? 1 : 0;
                case "timetointercept":
                    return VesselMovements.TimeToIntercept;
                case "megspeed":
                    return SonarData.MegSpeed;
                case "megturnspeed":
                    return SonarData.MegTurnSpeed;
                case "sonarlongfrequency":
                    return SonarData.LongFrequency;
                case "sonarlonggain":
                    return SonarData.LongGain;
                case "sonarlongrange":
                    return SonarData.LongRange;
                case "sonarlongsensitivity":
                    return SonarData.LongSensitivity;
                case "sonarshortfrequency":
                    return SonarData.ShortFrequency;
                case "sonarshortgain":
                    return SonarData.ShortGain;
                case "sonarshortrange":
                    return SonarData.ShortRange;
                case "sonarshortsensitivity":
                    return SonarData.ShortSensitivity;
                case "dccquadscreen0":
                    return DCCScreenData.DCCquadScreen0;
                case "dccquadscreen1":
                    return DCCScreenData.DCCquadScreen1;
                case "dccquadscreen2":
                    return DCCScreenData.DCCquadScreen2;
                case "dccquadscreen3":
                    return DCCScreenData.DCCquadScreen3;
                case "dccquadscreen4":
                    return DCCScreenData.DCCquadScreen4;
                case "dccfullscreen":
                    return DCCScreenData.DCCfullscreen;
                case "domecenter":
                    return (float)DomeData.domeCenter;
                case "domecornerbottomleft":
                    return (float)DomeData.domeCornerBottomLeft;
                case "domecornerbottomright":
                    return (float)DomeData.domeCornerBottomRight;
                case "domecornertopleft":
                    return (float)DomeData.domeCornerTopLeft;
                case "domecornertopright":
                    return (float)DomeData.domeCornerTopRight;
                case "domeleft":
                    return (float)DomeData.domeLeft;
                case "domehexbottomleft":
                    return (float)DomeData.domeHexBottomLeft;
                case "domehexbottomright":
                    return (float)DomeData.domeHexBottomRight;
                case "domehextopleft":
                    return (float)DomeData.domeHexTopLeft;
                case "domehextopright":
                    return (float)DomeData.domeHexTopRight;
                case "domeright":
                    return (float)DomeData.domeRight;
                case "domesquarebottom":
                    return (float)DomeData.domeSquareBottom;
                case "domesquareleft":
                    return (float)DomeData.domeSquareLeft;
                case "domesquareright":
                    return (float)DomeData.domeSquareRight;
                case "domesquaretop":
                    return (float) DomeData.domeSquareTop;
                default:
                    return GetDynamicValue(valueName, defaultValue);
            }
        }

        /** Return a string representation of a shared state value, indexed by name. */
        public static string GetServerDataAsText(string valueName)
        {
            if (!ServerObject)
                return "no value";

            switch (valueName.ToLower())
            {
                case "playervesselname":
                    return VesselData.PlayerVesselName;
                case "scene":
                    return ServerData.scene.ToString();
                case "shot":
                    return ServerData.shot.ToString();
                case "take":
                    return ServerData.take.ToString();
                case "depth":
                    int dInt = (int)ServerData.depth;
                    return dInt.ToString();
                case "pressure":
                    return ServerData.pressure.ToString();
                case "heading":
                    return (ServerData.heading.ToString("n1") + "°");
                case "pitchangle":
                    return (ServerData.pitchAngle.ToString("n1") + "°");
                case "yawangle":
                    return (ServerData.yawAngle.ToString("n1") + "°");
                case "rollangle":
                    return (ServerData.rollAngle.ToString("n1") + "°");
                case "velocity":
                    return ServerData.velocity.ToString("n1");
                case "floordepth":
                    return Mathf.RoundToInt(ServerData.floorDepth).ToString();
                case "floordistance":
                    return Mathf.RoundToInt(ServerData.floorDistance).ToString();
                case "divetime":
                    var diveSpan = TimeSpan.FromSeconds(ServerData.diveTime);
                    return string.Format("{0:00}:{1:00}:{2:00}", diveSpan.Hours, diveSpan.Minutes, diveSpan.Seconds);
                case "divetimeactive":
                    return ServerData.diveTimeActive.ToString();
                case "duetime":
                    var dueSpan = TimeSpan.FromSeconds(ServerData.dueTime);
                    return string.Format("{0:00}:{1:00}:{2:00}", dueSpan.Hours, dueSpan.Minutes, dueSpan.Seconds);
                case "duetimeactive":
                    return ServerData.dueTimeActive.ToString();
                case "watertemp":
                    return ServerData.waterTemp.ToString();
                case "b1":
                    return BatteryData.bank1.ToString("n1");
                case "b2":
                    return BatteryData.bank2.ToString("n1");
                case "b3":
                    return BatteryData.bank3.ToString("n1");
                case "b4":
                    return BatteryData.bank4.ToString("n1");
                case "b5":
                    return BatteryData.bank5.ToString("n1");
                case "b6":
                    return BatteryData.bank6.ToString("n1");
                case "b7":
                    return BatteryData.bank7.ToString("n1");
                case "o1":
                case "oxygentank1":
                    return OxygenData.oxygenTank1.ToString("n1");
                case "o2":
                case "oxygentank2":
                    return OxygenData.oxygenTank2.ToString("n1");
                case "o3":
                case "oxygentank3":
                    return OxygenData.oxygenTank3.ToString("n1");
                case "oxygen":
                    return (OxygenData.oxygen.ToString("n1") + "%");
                case "oxygenflow":
                    return (OxygenData.oxygenFlow.ToString("n0") + "lpm");
                case "co2":
                    return (CabinData.Co2 + "%");
                case "cabinpressure":
                    return CabinData.cabinPressure.ToString();
                case "cabinoxygen":
                    return (CabinData.cabinOxygen.ToString("n0") + "lpm");
                case "cabinhumidity":
                    return (CabinData.cabinHumidity.ToString("n1") + "%");
                case "cabintemp":
                    return CabinData.cabinTemp.ToString();
                case "battery":
                    return (BatteryData.battery.ToString("n1") + "%");
                case "batterytemp":
                    return (BatteryData.batteryTemp.ToString("n1") + "°c");
                case "batterycurrent":
                    return (BatteryData.batteryTemp.ToString("n1"));
                case "batterytimeremaining":
                    var span = TimeSpan.FromSeconds(BatteryData.batteryTimeRemaining);
                    return string.Format("{0:00}:{1:00}", span.Hours, span.Minutes);
                case "pilot":
                    return ServerData.pilot;
                case "verticalvelocity":
                    return ServerData.verticalVelocity.ToString("n1");
                case "horizontalvelocity":
                    return ServerData.horizontalVelocity.ToString("n1");
                case "inputxaxis":
                    return SubControl.inputXaxis.ToString("n1");
                case "inputyaxis":
                    return SubControl.inputYaxis.ToString("n1");
                case "inputzaxis":
                    return SubControl.inputZaxis.ToString("n1");
                case "inputxaxis2":
                    return SubControl.inputXaxis2.ToString("n1");
                case "inputyaxis2":
                    return SubControl.inputYaxis2.ToString("n1");
                case "crewheartrate1":
                    return CrewData.crewHeartRate1.ToString("n1");
                case "crewheartrate2":
                    return CrewData.crewHeartRate2.ToString("n1");
                case "crewheartrate3":
                    return CrewData.crewHeartRate3.ToString("n1");
                case "crewheartrate4":
                    return CrewData.crewHeartRate4.ToString("n1");
                case "crewheartrate5":
                    return CrewData.crewHeartRate5.ToString("n1");
                case "crewheartrate6":
                    return CrewData.crewHeartRate6.ToString("n1");
                case "crewbodytemp1":
                    return CrewData.crewBodyTemp1.ToString("n1");
                case "crewbodytemp2":
                    return CrewData.crewBodyTemp2.ToString("n1");
                case "crewbodytemp3":
                    return CrewData.crewBodyTemp3.ToString("n1");
                case "crewbodytemp4":
                    return CrewData.crewBodyTemp4.ToString("n1");
                case "crewbodytemp5":
                    return CrewData.crewBodyTemp5.ToString("n1");
                case "crewbodytemp6":
                    return CrewData.crewBodyTemp6.ToString("n1");
                case "mapeventname":
                    return MapData.mapEventName;
                case "latitude":
                    return FormatLatitude(MapData.latitude);
                case "longitude":
                    return FormatLongitude(MapData.longitude);
                case "towwinchload":
                    return OperatingData.towWinchLoad.ToString("n0");
                case "hydraulictemp":
                    return OperatingData.hydraulicTemp.ToString("n1") + "°c";
                case "hydraulicpressure":
                    return OperatingData.hydraulicPressure.ToString("n1");
                case "ballastpressure":
                    return OperatingData.ballastPressure.ToString("n1") + "°c";
                case "variableballasttemp":
                    return OperatingData.variableBallastTemp.ToString("n1") + "°c";
                case "variableballastpressure":
                    return OperatingData.variableBallastPressure.ToString("n1");
                case "commssignalstrength":
                    return OperatingData.commsSignalStrength.ToString("n1");
                case "divertpowertothrusters":
                    return OperatingData.divertPowerToThrusters.ToString("n1");
                case "thruster_heat_l":
                    return GliderErrorData.thruster_heat_l.ToString("n1") + "°c";
                case "thruster_heat_r":
                    return GliderErrorData.thruster_heat_r.ToString("n1") + "°c";
                case "vertran_heat_l":
                    return GliderErrorData.vertran_heat_l.ToString("n1") + "°c";
                case "vertran_heat_r":
                    return GliderErrorData.vertran_heat_r.ToString("n1") + "°c";
                case "thruster_l_status":
                    return (GliderErrorData.thruster_heat_l > 85) ? "WARNING" : "OK";
                case "thruster_r_status":
                    return (GliderErrorData.thruster_heat_r > 85) ? "WARNING" : "OK";
                case "vertran_l_status":
                    return (GliderErrorData.vertran_heat_l > 85) ? "WARNING" : "OK";
                case "vertran_r_status":
                    return (GliderErrorData.vertran_heat_r > 85) ? "WARNING" : "OK";
                case "jet_l_status":
                    return (GliderErrorData.jet_heat_l > 85) ? "WARNING" : "OK";
                case "jet_r_status":
                    return (GliderErrorData.jet_heat_r > 85) ? "WARNING" : "OK";
                case "vesselmovementenabled":
                    return VesselMovements.Enabled.ToString();
                case "timetointercept":
                    return VesselMovements.TimeToIntercept.ToString();
                case "megspeed":
                    return SonarData.MegSpeed.ToString("n1");
                case "megturnspeed":
                    return SonarData.MegTurnSpeed.ToString("n1");
                case "domecenter":
                    return DomeData.domeCenter.ToString();
                case "domecornerbottomleft":
                    return DomeData.domeCornerBottomLeft.ToString();
                case "domecornerbottomright":
                    return DomeData.domeCornerBottomRight.ToString();
                case "domecornertopleft":
                    return DomeData.domeCornerTopLeft.ToString();
                case "domecornertopright":
                    return DomeData.domeCornerTopRight.ToString();
                case "domeleft":
                    return DomeData.domeLeft.ToString();
                case "domehexbottomleft":
                    return DomeData.domeHexBottomLeft.ToString();
                case "domehexbottomright":
                    return DomeData.domeHexBottomRight.ToString();
                case "domehextopleft":
                    return DomeData.domeHexTopLeft.ToString();
                case "domehextopright":
                    return DomeData.domeHexTopRight.ToString();
                case "domeright":
                    return DomeData.domeRight.ToString();
                case "domesquarebottom":
                    return DomeData.domeSquareBottom.ToString();
                case "domesquareleft":
                    return DomeData.domeSquareLeft.ToString();
                case "domesquareright":
                    return DomeData.domeSquareRight.ToString();
                case "domesquaretop":
                    return DomeData.domeSquareTop.ToString();
                default:
                    var value = GetServerData(valueName);
                    return (value == Unknown) ? "no value" : value.ToString("n1");
            }
        }

        /** Set the current value of a shared boolean state value by name (only works on host). */
        public static void SetServerBool(string boolName, bool value)
        {
            if (ServerObject == null)
            {
                Debug.Log("Server object missing");
            }
            else
            {
                ServerData.OnChangeBool(boolName, value);
            }
        }

        /** Return the current value of a shared boolean state value by name. */
        public static bool GetServerBool(string boolName)
        {
            // Check if we're looking for a vessel state value.
            if (VesselData.IsVesselKey(boolName))
                return VesselData.GetServerData(boolName, Unknown) > 0;

            // Match server data key against known values.
            switch (boolName.ToLower())
            {
                case "divetimeactive":
                    return ServerData.diveTimeActive;
                case "duetimeactive":
                    return ServerData.dueTimeActive;
                case "vesselmovementenabled":
                    return VesselMovements.Enabled;
                case "disableinput":
                    return SubControl.disableInput;
                case "isautostabilised":
                    return SubControl.isAutoStabilised;
                case "ispitchalsostabilised":
                    return SubControl.IsPitchAlsoStabilised;
                case "joystickoverride":
                    return SubControl.JoystickOverride;
                case "joystickpilot":
                    return SubControl.JoystickPilot;
                default:
                    // As a last resort, interpret numeric values as booleans.
                    var value = GetServerData(boolName);
                    return !Mathf.Approximately(value, 0) && !Mathf.Approximately(value, Unknown);
            }
        }

        /** Set the current value of a shared numeric state value by name (only works on host). */
        public static void SetServerData(string valueName, float value, bool add = false)
        {
            // Check that parameter name is valid.
            if (string.IsNullOrEmpty(valueName))
                return;

            // Update the server data value.
            var key = valueName.ToLower();
            ServerData.OnValueChanged(key, value, add);
        }

        /** Set a shared server value at runtime. */
        public static void SetDynamicValue(string valueName, float newValue, ParameterInfo info)
        {
            // Check that parameter name is valid.
            if (string.IsNullOrEmpty(valueName))
                return;

            // Update the shared server data value.
            ServerData.SetDynamicValue(new ServerValue
                { key = valueName.ToLower(), value = newValue, info = info }, true);
        }

        /** Set a shared server value at runtime. */
        public static void SetDynamicValue(string valueName, bool newValue, ParameterInfo info)
            { SetDynamicValue(valueName, newValue ? 1 : 0, info); }

        /** Register the existence of a server data value. */
        public static void RegisterServerValue(string valueName, ParameterInfo info)
        {
            RegisterServerValue(new ServerValue
                { key = valueName.ToLower(), info = info });
        }

        /** Register a new server data value. */
        public static void RegisterServerValue(ServerValue value)
        {
            Parameters.Add(value.key);
            WriteableParameters.Add(value.key);
            ParameterInfos[value.key] = value.info;
        }

        /** Return a shared server value that has been defined at runtime. */
        public static float GetDynamicValue(string valueName, float defaultValue = Unknown)
        {
            return ServerData.GetDynamicValue(valueName, defaultValue);
        }

        /** Determines if a dynamic shared server value exists. */
        public static bool HasDynamicValue(string valueName)
        {
            return ServerData.HasDynamicValue(valueName);
        }

        /** Set the current value of a shared string state value by name (only works on host). */
        public static void SetServerData(string valueName, string value)
        {
            ServerData.OnValueChanged(valueName, value);
        }

        /** Set the current value of a shared numeric state value by name (works on both clients and host). */
        public static void PostServerData(string valueName, float value, bool add = false)
        {
            if (ServerData && ServerData.isServer)
                SetServerData(valueName, value, add);
            else if (LocalPlayer)
                LocalPlayer.PostServerData(valueName, value, add);
        }

        /** Set the current value of a shared string state value by name (works on both clients and host). */
        public static void PostServerData(string valueName, string value)
        {
            if (ServerData && ServerData.isServer)
                SetServerData(valueName, value);
            else if (LocalPlayer)
                LocalPlayer.PostServerData(valueName, value);
        }

        /** Initiate a physics impact to the player vessel (works on both clients and host). */
        public static void PostImpact(Vector3 impactVector)
        {
            if (ServerData && ServerData.isServer)
                ServerData.RpcImpact(impactVector);
            else if (LocalPlayer)
                LocalPlayer.PostImpact(impactVector);
        }

        /** Initiate a sonar event (works on both clients and host). */
        public static void PostSonarEvent(megEventSonar sonarEvent)
        {
            if (LocalPlayer)
                LocalPlayer.PostSonarEvent(sonarEvent);
        }

        /** Clear the sonar of all active objects (works on both clients and host). */
        public static void PostSonarClear()
        {
            if (LocalPlayer)
                LocalPlayer.PostSonarClear();
        }

        /** Post a custom camera event by name (works on both clients and host). */
        public static void PostMapCameraEvent(string eventName)
        {
            if (LocalPlayer)
                LocalPlayer.PostMapCameraEvent(eventName);
        }

        /** Post a custom camera event by supplying the target state (works on both clients and host). */
        public static void PostMapCameraState(megMapCameraEventManager.State state)
        {
            if (LocalPlayer)
                LocalPlayer.PostMapCameraState(state);
        }

        /** Post vessel movements state to the server (works on both clients and host). */
        public static void PostVesselMovementState(JSONObject json)
        {
            if (LocalPlayer)
                LocalPlayer.PostVesselMovementState(json);
        }

        /** Number of vessels that can be displayed on the map. */
        public static int GetVesselCount()
            { return VesselData.VesselCount; }

        /** Set a vessel's current position and nominal speed (1-based index). */
        public static void SetVesselData(int vessel, Vector3 position, float speed)
            { VesselData.SetState(vessel, position, speed); }

        /** Return a vessel's current depth (1-based index). */
        public static float GetVesselDepth(int vessel)
            { return VesselData.GetDepth(vessel); }

        /** Set a vessel's current depth (1-based index). */
        public static void SetVesselDepth(int vessel, float depth)
            { VesselData.SetDepth(vessel, depth); }

        /** Return a vessel's current position (1-based index). */
        public static Vector3 GetVesselPosition(int vessel)
            { return VesselData.GetPosition(vessel); }

        /** Return a vessel's velocity (1-based index). */
        public static float GetVesselVelocity(int vessel)
            { return VesselData.GetSpeed(vessel); }

        /** Return a vessel's name (1-based index). */
        public static string GetVesselName(int vessel)
            { return VesselData.GetName(vessel); }

        /** Return the player vessel's current target vessel (or 0 if there is no target). */
        public static int GetTargetVessel()
        {
            var playerVessel = GetPlayerVessel();
            if (playerVessel <= 0)
                return 0;

            var movement = GetVesselMovements().GetVesselMovement(playerVessel);
            var intercept = movement as vesselIntercept;
            var pursue = movement as vesselPursue;

            if (intercept)
                return intercept.TargetVessel;
            if (pursue)
                return pursue.TargetVessel;

            // No target vessel.
            return 0;
        }

        /** Return whether player's vessel has a current target. */
        public static bool HasTargetVessel()
        {
            return GetTargetVessel() > 0;
        }

        /** Get a bearing to target vessel, relative to player vessel. */
        public static float GetVesselBearing(int vessel)
        {
            if (vessel <= 0)
                return 0;

            var origin = GetVesselPosition(GetPlayerVessel());
            var target = GetVesselPosition(vessel);
            var heading = GetServerData("heading");
            var delta = new Vector3(target.x - origin.x, 0, target.y - origin.y);
            var spherical = new SphericalCoordinates(delta);
            var headingToTarget = (90 - spherical.polar * Mathf.Rad2Deg);
            var bearing = headingToTarget - heading;
            return Mathf.Repeat(bearing, 360);
        }

        /** Set a vessel's current position (1-based index). */
        public static void SetVesselPosition(int vessel, Vector3 p)
            { VesselData.SetPosition(vessel, p); }

        /** Set a vessel's current speed (1-based index). */
        public static void SetVesselVelocity(int vessel, float v)
            { VesselData.SetSpeed(vessel, v); }

        /** Return vessel's current position/velocity data (1-based index). */
        public static void GetVesselData(int vessel, out Vector3 position, out float velocity)
        {
            position = VesselData.GetPosition(vessel);
            velocity = VesselData.GetSpeed(vessel);
        }

        /** Return the distance between two vessels in meters. */
        public static float GetVesselDistance(int from, int to)
            { return VesselData.GetDistance(from, to); }

        /** Return a vessel's current position as a latitude/longitude pair (1-based index). */
        public static Vector2 GetVesselLatLong(int vessel)
            { return VesselData.GetLatLong(vessel); }

        /** Return the vessel movements manager. */
        public static vesselMovements GetVesselMovements()
            { return ServerObject ? ServerObject.GetComponent<vesselMovements>() : null; }

        /** Sets which vessel is controlled by the player (1-based index). */
        public static void SetPlayerVessel(int vessel)
        {
            if (VesselData)
                VesselData.PlayerVessel = vessel;
        }

        /** Returns which vessel is controlled by the player (1-based index). */
        public static int GetPlayerVessel()
            { return VesselData ? VesselData.PlayerVessel : 0; }

        /** Sets the player vessel's velocity in world space. */
        public static void SetPlayerWorldVelocity(Vector3 velocity)
        {
            if (VesselData)
                VesselData.SetPlayerWorldVelocity(velocity);
        }

        /** Return a vessel's visibility on the navigation map (1-based index). */
        public static bool GetVesselVis(int vessel)
            { return VesselData && VesselData.IsOnMap(vessel); }

        /** Sets a vessel's visibility on the navigation map (1-based index). */
        public static void SetVesselVis(int vessel, bool state)
        {
            if (VesselData)
                VesselData.SetVisible(vessel, state);
        }

        /** Set the current vessel color theme. */
        public static void SetColorTheme(megColorTheme theme)
        {
            if (ServerObject == null)
                return;

            ServerObject.GetComponent<graphicsColourHolder>().themeName = theme.name;
            ServerObject.GetComponent<graphicsColourHolder>().backgroundColor = theme.backgroundColor;
            ServerObject.GetComponent<graphicsColourHolder>().highlightColor = theme.highlightColor;
            ServerObject.GetComponent<graphicsColourHolder>().keyColor = theme.keyColor;
        }

        /** Returns the current vessel color theme. */
        public static megColorTheme GetColorTheme()
        {
            megColorTheme theme = new megColorTheme();
            if (ServerObject)
                theme = ServerObject.GetComponent<graphicsColourHolder>().theme;

            return theme;
        }

        /** Format a latitude value into a readable string (e.g. 25°20'10.5 N). */
        public static string FormatLatitude(float value, int precision = 4)
            { return FormatDegreeMinuteSecond(Mathf.Abs(value), precision) + (value >= 0 ? "N" : "S"); }

        /** Format a longitude value into a readable string (e.g. 85°20'10.5 E). */
        public static string FormatLongitude(float value, int precision = 4)
            { return FormatDegreeMinuteSecond(Mathf.Abs(value), precision) + (value >= 0 ? "E" : "W"); }

        /** Format a angular value into a readable string (e.g. 85°20'10.5). */
        public static string FormatDegreeMinuteSecond(float value, int precision = 4)
        {
            var degrees = Mathf.FloorToInt(value);
            var minutes = Mathf.FloorToInt((value * 60) % 60);
            var seconds = Mathf.Abs(value * 3600) % 60;

            return string.Format("{0}°{1}\'{2:N" + precision + "}\"", degrees, minutes, seconds);
        }

        /** Set the desired map event name (indicates a camera transition, only works on host). */
        public static void SetMapEventName(string eventName)
        {
            ServerObject.GetComponent<mapData>().mapEventName = eventName;
        }

        /** Whether player is in a glider sub. */
        public static bool IsGlider()
            { return ServerData && ServerData.isGlider; }

        /** Get an ID for the given glider screen. */
        public static int getGliderScreen(int screenID)
        {
            int outID = 0;

            if (ServerObject)
            {
                outID = ServerObject.GetComponent<glScreenData>().getScreen(screenID);
            }

            return outID;
        }

        /** Return the buttons state for a given glider button. */
        public static bool GetGliderButtonState(int buttonID)
        {
            bool buttonState = false;

            switch (buttonID)
            {
                case 0:
                    //elevation
                    buttonState = ServerObject.GetComponent<glScreenData>().mapElevation;
                    break;
                case 1:
                    //recentre
                    buttonState = ServerObject.GetComponent<glScreenData>().recentreMap;
                    break;
                case 2:
                    //labels
                    buttonState = ServerObject.GetComponent<glScreenData>().objectLabelsOn;
                    break;
            }

            return buttonState;
        }

    }
}
