using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Meg.DCC;
using Meg.EventSystem;
using Meg.Graphics;
using UnityEngine.Networking;

namespace Meg.Networking
{
    public class serverUtils : MonoBehaviour
    {

        // Constants
        // ------------------------------------------------------------

        /** The current application version. */
        public const string Version = "1.1.3";

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
                var player = ClientScene.localPlayers.FirstOrDefault();
                if (player != null)
                    return player.gameObject.GetComponent<serverPlayer>();

                return null;
            }
        }

        /** Return the local player's screen state. */
        public static screenData.State LocalScreenState
            { get { return LocalPlayer ? LocalPlayer.ScreenState : new screenData.State(); } }

        /** Return the local player's input source. */
        public static Rewired.Player LocalInput
            { get { return LocalPlayer ? LocalPlayer.Input : null; } }

        /** Return a collection of all known players. */
        public static IEnumerable<serverPlayer> GetPlayers()
        {
            return GameObject.FindGameObjectsWithTag("Player")
                .Select(go => go.GetComponent<serverPlayer>());
        }

        /** Return player with the given id. */
        public static serverPlayer GetPlayer(NetworkInstanceId id)
            { return GetPlayers().FirstOrDefault(p => p.netId == id); }

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

        /** Return the Screen data object. */
        private static screenData _screenData;
        public static screenData ScreenData
        {
            get
            {
                if (!_screenData && ServerObject)
                    _screenData = ServerObject.GetComponent<screenData>();
                return _screenData;
            }
        }

        /** Return the Popup data object. */
        private static popupData _popupData;
        public static popupData PopupData
        {
            get
            {
                if (!_popupData && ServerObject)
                    _popupData = ServerObject.GetComponent<popupData>();
                return _popupData;
            }
        }

        /** Return the Popup data object. */
        private static clientCalcValues _clientCalcValues;
        public static clientCalcValues ClientCalcValues
        {
            get
            {
                if (!_clientCalcValues && ServerObject)
                    _clientCalcValues = ServerObject.GetComponent<clientCalcValues>();
                return _clientCalcValues;
            }
        }

        /** Return the light data object. */
        private static lightData _lightData;
        public static lightData LightData
        {
            get
            {
                if (!_lightData && ServerObject)
                    _lightData = ServerObject.GetComponent<lightData>();
                return _lightData;
            }
        }

        /** Return the docking data object. */
        private static dockingData _dockingData;
        public static dockingData DockingData
        {
            get
            {
                if (!_dockingData && ServerObject)
                    _dockingData = ServerObject.GetComponent<dockingData>();
                return _dockingData;
            }
        }

        /** Return the docking data object. */
        private static glTowingData __glTowingData;
        public static glTowingData GLTowingData
        {
            get
            {
                if (!__glTowingData && ServerObject)
                    __glTowingData = ServerObject.GetComponent<glTowingData>();
                return __glTowingData;
            }
        }

        /** Return the parameter noise data object. */
        private static noiseData _noiseData;
        public static noiseData NoiseData
        {
            get
            {
                if (!_noiseData && ServerObject)
                    _noiseData = ServerObject.GetComponent<noiseData>();
                return _noiseData;
            }
        }

        /** Return the vessel movements controller. */
        private static vesselMovements _vesselMovements;
        public static vesselMovements VesselMovements
        {
            get
            {
                if (!_vesselMovements && ServerObject)
                    _vesselMovements = ServerObject.GetComponent<vesselMovements>();
                return _vesselMovements;
            }
        }

        /** Whether the server object is available for use yet. */
        public static bool IsReady()
            { return ServerObject != null; }

        /** Whether this instance is the server. */
        public static bool IsServer()
            { return ServerData && ServerData.isServer; }

        /** Whether this instance is in the debug screen. */
        public static bool IsInDebugScreen()
            { return widgetDebugButton.Instance && widgetDebugButton.Instance.IsDebug; }

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
            "acidlayer",
            "air",
            "airtank1",
            "airtank2",
            "airtank3",
            "airtank4",
            "b1",
            "b1error",
            "b2",
            "b2error",
            "b3",
            "b3error",
            "b4",
            "b4error",
            "b5",
            "b5error",
            "b6",
            "b6error",
            "b7",
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
            "bootcodeduration",
            "bootprogress",
            "cabinhumidity",
            "cabinoxygen",
            "cabinpressure",
            "cabinpressurepsi",
            "cabintemp",
            "camerabrightness",
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
            "dcccommsusesliders",
            "dcccommscontent",
            "dccvesselnameintitle",
            "depth",
            "depthdisplayed",
            "depthoverride",
            "depthoverrideamount",
            "disableinput",
            "divertpowertothrusters",
            "divetime",
            "divetimeactive",
            "docking1",
            "docking2",
            "docking3",
            "docking4",
            "dockingbuttonenabled",
            "domecenter",
            "domecornerbottomleft",
            "domecornerbottomright",
            "domecornertopleft",
            "domecornertopright",
            "domehexbottomleft",
            "domehexbottomright",
            "domehextopleft",
            "domehextopright",
            "domeleft",
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
            "error_diagnostics",
            "error_doppler",
            "error_electricleak",
            "error_forwardsonar",
            "error_gps",
            "error_grapple",
            "error_hatch",
            "error_hydraulicpump",
            "error_hyrdaulicres",
            "error_jet_l",
            "error_jet_r",
            "error_oxygenext",
            "error_oxygenpump",
            "error_panel_l",
            "error_panel_r",
            "error_portlights",
            "error_pressure",
            "error_radar",
            "error_runninglights",
            "error_starboardlights",
            "error_sternlights",
            "error_structural",
            "error_system",
            "error_thruster_l",
            "error_thruster_r",
            "error_tow",
            "error_vertran_l",
            "error_vertran_r",
            "error_vhf",
            "error_vidhd",
            "floordepth",
            "floordistance",
            "floordistancedisplayed",
            "genericerror",
            "greenscreenbrightness",
            "heading",
            "horizontalvelocity",
            "hydraulicpressure",
            "hydraulictemp",
            "initiatemapevent",
            "inputsource",
            "inputxaxis",
            "inputxaxis2",
            "inputyaxis",
            "inputyaxis2",
            "inputzaxis",
			"iscontroldecentmode",
            "iscontroldecentmodeonjoystick",
            "isautopilot",
			"iscontrolmodeoverride",
			"iscontroloverridestandard",
            "isautostabilised",
            "ispitchalsostabilised",
            "jet_heat_l",
            "jet_heat_r",
            "joystickoverride",
            "joystickpilot",
            "latitude",
            "lightarray1",
            "lightarray2",
            "lightarray3",
            "lightarray4",
            "lightarray5",
            "lightarray6",
            "lightarray7",
            "lightarray8",
            "lightarray9",
            "lightarray10",
            "longitude",
            "maxwildlife",
            "maxspeed",
            "meg1posx",
            "meg1posy",
            "meg1posz",
            "meg1velocity",
            "meg1vis",
            "megspeed",
            "megturnspeed",
            "minspeed",
			"motionbasepitch",
			"motionbaseyaw",
			"motionbaseroll",
			"motionsafety",
			"motionhazard",
			"motionhazardenabled",
			"motionslerpspeed",
			"motionhazardsensitivity",
			"motionscaleimpacts",
			"motionminimpactinterval",
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
            "pilotbuttonenabled",
            "pitchangle",
            "pitchspeed",
            "playervessel",
            "playervesselname",
            "pressure",
            "pressureoverride",
            "pressureoverrideamount",
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
            "screenglitchamount",
            "screenglitchautodecay",
            "screenglitchautodecaytime",
            "screenglitchmaxdelay",
            "scrubbedco2",
            "scrubbedhumidity",
            "scrubbedoxygen",
            "shot",
            "sonarheadingup",
            "sonarlongfrequency",
            "sonarlonggain",
            "sonarlongrange",
            "sonarlongsensitivity",
            "sonarproximity",
            "sonarshortfrequency",
            "sonarshortgain",
            "sonarshortrange",
            "sonarshortsensitivity",
            "startimagesequence",
            "take",
            "thruster_heat_l",
            "thruster_heat_r",
            "timetointercept",
            "towtargetx",
            "towtargety",
            "towtargetspeed",
            "towtargetvisible",
            "towfiringpressure",
            "towfiringpower",
            "towfiringstatus",
            "towlinespeed",
            "towlinelength",
            "towlineremaining",
            "towtargetdistance",
            "towwinchload",
            "v1depth",
            "v1posx",
            "v1posy",
            "v1posz",
            "v1velocity",
            "v2depth",
            "v2posx",
            "v2posy",
            "v2posz",
            "v2velocity",
            "v3depth",
            "v3posx",
            "v3posy",
            "v3posz",
            "v3velocity",
            "v4depth",
            "v4posx",
            "v4posy",
            "v4posz",
            "v4velocity",
            "v5depth",
            "v5posx",
            "v5posy",
            "v5posz",
            "v5velocity",
            "v6depth",
            "v6posx",
            "v6posy",
            "v6posz",
            "variableballastpressure",
            "variableballasttemp",
            "velocity",
            "version",
            "verticalvelocity",
            "vertran_heat_l",
            "vertran_heat_r",
            "vessel1depth",
            "vessel1icon",
            "vessel1vis",
            "vessel2depth",
            "vessel2icon",
            "vessel2vis",
            "vessel3depth",
            "vessel3icon",
            "vessel3vis",
            "vessel4depth",
            "vessel4icon",
            "vessel4vis",
            "vessel5icon",
            "vessel5vis",
            "vessel6depth",
            "vessel6icon",
            "vessel6vis",
            "vesselmovementenabled",
            "watertemp",
            "watertempoverride",
            "watertempoverrideamount",
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
            Bool,
            String
        }

        /** Configuration data for a parameter. */
        public class ParameterInfo
        {
            public ParameterType type = ParameterType.Float;
            public float minValue = 0;
            public float maxValue = 100;
            public bool readOnly;
            public bool hideInUi;
            public string description = "";
            public noiseData.Profile noiseProfile;

            public ParameterInfo()
            {
            }

            public ParameterInfo(ParameterType type)
            {
                this.type = type;
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
            { "acceleration", new ParameterInfo { description = "Sub's acceleration (scaling factor)."} },
            { "acidlayer", new ParameterInfo { maxValue = 1, type = ParameterType.Int, description = "Acid layer map visibility."} },
            { "air", new ParameterInfo { readOnly = true } },
            { "airtank1", new ParameterInfo { description = "Main air tank 1 capacity (%)."} },
            { "airtank2", new ParameterInfo { description = "Main air tank 2 capacity (%)."} },
            { "airtank3", new ParameterInfo { description = "Main air tank 3 capacity (%)."} },
            { "airtank4", new ParameterInfo { description = "Main air tank 4 capacity (%)."} },
            { "b1", new ParameterInfo { description = "State of charge for battery bank 1."} },
            { "b1error", new ParameterInfo { description = "Error levels for cells in battery bank 1."} },
            { "b2", new ParameterInfo { description = "State of charge for battery bank 2."} },
            { "b2error", new ParameterInfo { description = "Error levels for cells in battery bank 2."} },
            { "b3", new ParameterInfo { description = "State of charge for battery bank 3."} },
            { "b3error", new ParameterInfo { description = "Error levels for cells in battery bank 3."} },
            { "b4", new ParameterInfo { description = "State of charge for battery bank 4."} },
            { "b4error", new ParameterInfo { description = "Error levels for cells in battery bank 4."} },
            { "b5", new ParameterInfo { description = "State of charge for battery bank 5."} },
            { "b5error", new ParameterInfo { description = "Error levels for cells in battery bank 5."} },
            { "b6", new ParameterInfo { description = "State of charge for battery bank 6."} },
            { "b6error", new ParameterInfo { description = "Error levels for cells in battery bank 6."} },
            { "b7", new ParameterInfo { description = "State of charge for battery bank 7."} },
            { "b7error", new ParameterInfo { description = "Error levels for cells in battery bank 7."} },
            { "ballastpressure", new ParameterInfo { description = "Ballast air pressure (psi)." } },
            { "battery", new ParameterInfo { readOnly = true, description = "Overall battery charge level (computed on server)." } },
            { "batterycurrent", new ParameterInfo { maxValue = 30, description = "Current draw from the battery banks (amps)" } },
            { "batterydrain", new ParameterInfo { maxValue = 1, description = "Rate of battery drain (% per second)."} },
            { "batteryerrorthreshold", new ParameterInfo { description = "Battery bank errors are automatically populated when their charge level dips below this number."} },
            { "batterylife", new ParameterInfo { maxValue = 128, description = "Battery life remaining (KWh)." } },
            { "batterylifeenabled", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether battery life is automatically updated."} },
            { "batterylifemax", new ParameterInfo { maxValue = 128, description = "Battery life maximum (KWh)."} },
            { "batterytemp", new ParameterInfo { description = "Battery temperature (degrees c)." } },
            { "batterytimeenabled", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether battery time counts down."} },
            { "batterytimeremaining", new ParameterInfo { maxValue = 3600 * 12, type = ParameterType.Int, description = "Battery time remaining (seconds)." } },
            { "bootcodeduration", new ParameterInfo { minValue = 1, maxValue = 5, description = "Duration of the bootup sequence's code section."} },
            { "bootprogress", new ParameterInfo { description = "Progress for the bootup systems online sequence."} },
            { "cabinhumidity", new ParameterInfo { description = "Cabin humidity (%)."} },
            { "cabinoxygen", new ParameterInfo { description = "Cabin oxygen percentage (tops out to nominal 20.9% at 5% reserves)."} },
            { "cabinpressure", new ParameterInfo { maxValue = 1.25f, description = "Cabin pressure (in bar)." } },
            { "cabinpressurepsi", new ParameterInfo { readOnly = true, description = "Cabin pressure (derived, in psi)." } },
            { "cabintemp", new ParameterInfo { description = "Cabin temperature (degrees c)."} },
            { "camerabrightness", new ParameterInfo { maxValue = 2, description = "Overall brightness of all screens. Can be used for powering down / low power mode effects." } },
            { "co2", new ParameterInfo { maxValue = 5, description = "CO2% in cabin atmosphere." } },
            { "co2ppm", new ParameterInfo { readOnly = true, description = "CO2 in cabin atmosphere (ppm)." } },
            { "commssignalstrength", new ParameterInfo { description = "Communications signal strength (0..100%)."} },
            { "dcccommsusesliders", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether to display an alternate comms UI (sliders instead of live feed)." } },
            { "dcccommscontent", new ParameterInfo { minValue = 0, maxValue = 9, type = ParameterType.Int, description = "Contents for DCC comms screen on overhead displays." } },
            { "dccvesselnameintitle", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether to display the current player vessel name in DCC window titles." } },
            { "depth", new ParameterInfo { maxValue = 12000, description = "Current depth (m)"} },
            { "depthdisplayed", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether depth is displayed in header area."} },
            { "depthoverride", new ParameterInfo { maxValue = 12000, description = "Displayed depth if depth override is active (m)"} },
            { "depthoverrideamount", new ParameterInfo { maxValue = 1, description = "The amount of depth override to apply [0..1]."} },
            { "disableinput", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether joystick input to sub is completely disabled."} },
            { "divertpowertothrusters", new ParameterInfo { description = "Amount of power diverted from systems to thrusters (0..100%)."} },
            { "divetime", new ParameterInfo { maxValue = 3600, description = "Duration of the dive so far (s)."} },
            { "divetimeactive", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether dive time is automatically updated."} },
            { "dockingbuttonenabled", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether the docking button is enabled in Piloting screen."} },
            { "docking1", new ParameterInfo { maxValue = 1, type = ParameterType.Int, description = "Docking sequence 1."} },
            { "docking2", new ParameterInfo { maxValue = 1, type = ParameterType.Int, description = "Docking sequence 2."} },
            { "docking3", new ParameterInfo { maxValue = 1, type = ParameterType.Int, description = "Docking sequence 3."} },
            { "docking4", new ParameterInfo { maxValue = 1, type = ParameterType.Int, description = "Docking sequence 4."} },
            { "domecenter", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int, description = domeData.HudDescription } },
            { "domecornerbottomleft", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int, description = domeData.HudDescription } },
            { "domecornerbottomright", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int, description = domeData.HudDescription } },
            { "domecornertopleft", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int, description = domeData.HudDescription } },
            { "domecornertopright", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int, description = domeData.HudDescription } },
            { "domehexbottomleft", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int, description = domeData.HudDescription } },
            { "domehexbottomright", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int, description = domeData.HudDescription } },
            { "domehextopleft", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int, description = domeData.HudDescription } },
            { "domehextopright", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int, description = domeData.HudDescription } },
            { "domeleft", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int, description = domeData.HudDescription } },
            { "domeright", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int, description = domeData.HudDescription } },
            { "domesquarebottom", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int, description = domeData.HudDescription } },
            { "domesquareleft", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int, description = domeData.HudDescription } },
            { "domesquareright", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int, description = domeData.HudDescription } },
            { "domesquaretop", new ParameterInfo { minValue = 0, maxValue = 12, type = ParameterType.Int, description = domeData.HudDescription } },
            { "duetime", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "ETA / Time to Intercept (s)."} },
            { "duetimeactive", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether ETA/TTI is automatically updated over time."} },
            { "error_ballasttank", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_batteryleak", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_bilgeleak", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_bowlights", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_bowthruster", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_cpu", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_datahd", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_depthsonar", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_diagnostics", new ParameterInfo { maxValue = 1, description = "Overall error indicator for diagnostics screen (drives sub display and datafeed)." } },
            { "error_doppler", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_electricleak", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_forwardsonar", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_gps", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_grapple", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_hatch", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_hydraulicpump", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_hyrdaulicres", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_jet_l", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_jet_r", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_oxygenext", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_oxygenpump", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_panel_l", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_panel_r", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_portlights", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_pressure", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_radar", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_runninglights", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_starboardlights", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_sternlights", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_structural", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_system", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_thruster_l", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_thruster_r", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_tow", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_vertran_l", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_vertran_r", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_vhf", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "error_vidhd", new ParameterInfo { maxValue = 1, description = errorData.DefaultErrorDescription } },
            { "floordistance", new ParameterInfo { readOnly = true, description = "Distance of sub to the ocean floor (m)." } },
            { "floordistancedisplayed", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether floor distance is displayed in header area."} },
            { "floordepth", new ParameterInfo { description = "Depth of the ocean floor (from sea level) at sub's current location (m)." } },
            { "genericerror", new ParameterInfo { description = "Generic error indicator popup control."} },
            { "greenscreenbrightness", new ParameterInfo { maxValue = 1, description = "Brightness level for greenscreen elements [0..1]."} },
            { "heading", new ParameterInfo { description = "Vessel's current heading angle (degrees, same as yawAngle.)"} },
            { "horizontalvelocity", new ParameterInfo { readOnly = true, description = "Sub's current velocity in the horizontal direction (m/s)." } },
            { "hydraulicpressure", new ParameterInfo { description = "Hydraulic system pressure (psi)."} },
            { "hydraulictemp", new ParameterInfo { description = "Hydraulic system temperature (°c)."} },
            { "initiatemapevent", new ParameterInfo { description = "Used to signal a map camera angle change.", hideInUi = true } },
            { "inputsource", new ParameterInfo { minValue = 0, maxValue = 2, readOnly = true, description = "Where input is coming from (0:None, 1:Client, 2:Server)" } },
            { "inputxaxis", new ParameterInfo { minValue = -1, maxValue = 1, description = "Current value of the joystick X input axis (yaw)." } },
            { "inputxaxis2", new ParameterInfo { minValue = -1, maxValue = 1, description = "Current value of the joystick X2 input axis." } },
            { "inputyaxis", new ParameterInfo { minValue = -1, maxValue = 1, description = "Current value of the joystick Y input axis (pitch)." } },
            { "inputyaxis2", new ParameterInfo { minValue = -1, maxValue = 1, description = "Current value of the joystick Y2 input axis."  } },
            { "inputzaxis", new ParameterInfo { minValue = -1, maxValue = 1, description = "Current value of the joystick Z input axis (throttle)."} },
			{ "isautopilot", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Sub control auto pilot toggle. does nothing but change a button light." } },
			{ "iscontroldecentmode", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Controls whether sub is in auto-descent mode." } },
            { "iscontroldecentmodeonjoystick", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether joystick input can be used to toggle descent mode." } },
            { "iscontrolmodeoverride", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Sub control mode server override." } },
			{ "iscontroloverridestandard", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Sub control mode when server is overriding." } },
            { "isautostabilised", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether sub roll is automatically stabilised."} },
            { "ispitchalsostabilised", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether sub pitch is also automatically stabilised."} },
            { "joystickoverride", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether pilot input is overridden by joysticks attached to the server."} },
            { "joystickpilot", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether input updates from pilot's joysticks (turn off for manual input editing)."} },
            { "latitude", new ParameterInfo { description = "Latitude at the map's origin (+N/-S, decimal degrees)." } },
            { "lightarray1", new ParameterInfo { minValue = 0, maxValue = 3, type = ParameterType.Int, description = "Light array 1 status."} },
            { "lightarray2", new ParameterInfo { minValue = 0, maxValue = 3, type = ParameterType.Int, description = "Light array 1 status."} },
            { "lightarray3", new ParameterInfo { minValue = 0, maxValue = 3, type = ParameterType.Int, description = "Light array 1 status."} },
            { "lightarray4", new ParameterInfo { minValue = 0, maxValue = 3, type = ParameterType.Int, description = "Light array 1 status."} },
            { "lightarray5", new ParameterInfo { minValue = 0, maxValue = 3, type = ParameterType.Int, description = "Light array 1 status."} },
            { "lightarray6", new ParameterInfo { minValue = 0, maxValue = 3, type = ParameterType.Int, description = "Light array 1 status."} },
            { "lightarray7", new ParameterInfo { minValue = 0, maxValue = 3, type = ParameterType.Int, description = "Light array 1 status."} },
            { "lightarray8", new ParameterInfo { minValue = 0, maxValue = 3, type = ParameterType.Int, description = "Light array 1 status."} },
            { "lightarray9", new ParameterInfo { minValue = 0, maxValue = 3, type = ParameterType.Int, description = "Light array 1 status."} },
            { "lightarray10", new ParameterInfo { minValue = 0, maxValue = 3, type = ParameterType.Int, description = "Light array 1 status."} },
            { "longitude", new ParameterInfo { description = "Latitude at the map's origin (+E/-W, decimal degrees)." } },
            { "maxwildlife", new ParameterInfo { minValue = 0, maxValue = 30, type = ParameterType.Int, description = "Maximum number of spawned small sonar contacts."} },
            { "maxspeed", new ParameterInfo { description = "Sub's maximum speed at 100% throttle (m/s)."} },
            { "megspeed", new ParameterInfo { description = "Speed that the Meg moves in the short-range sonar display."} },
            { "megturnspeed", new ParameterInfo { description = "Speed that the Meg turns in the short-range sonar display."} },
            { "minspeed", new ParameterInfo { description = "Sub's minimum speed at 100% reverse throttle (m/s)."} },
			{ "motionbasepitch", new ParameterInfo { minValue = -90, maxValue = 90, description = "current orientation sent to the motion base"} },
			{ "motionbaseyaw", new ParameterInfo { minValue = 0, maxValue = 360, description = "current orientation sent to the motion base"} },
			{ "motionbaseroll", new ParameterInfo { minValue = -90, maxValue = 90, description = "current orientation sent to the motion base."} },
			{ "motionsafety", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether motion-base values are slerped from raw sub state (for safety)."} },
			{ "motionhazard", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether output to motion-base has reached unsafe levels."} },
			{ "motionhazardenabled", new ParameterInfo {  maxValue = 1, type = ParameterType.Bool, description = "Whether motion-base hazard detection is enabled."} },
			{ "motionslerpspeed", new ParameterInfo { maxValue = 30, description = "t multiplier for the motion base's slerp."} },
			{ "motionhazardsensitivity", new ParameterInfo { maxValue = 30, description = "Sensitivity of tripping a motion base hazard (unsafe rate of change threshold)."} },
			{ "motionscaleimpacts", new ParameterInfo { maxValue = 0.75f, description = "Scaling factor for reducing the intensity of physics impacts."} },
			{ "motionminimpactinterval", new ParameterInfo { maxValue = 5.0f, description = "min interval for impacts on the sub"} },
            { "o1", new ParameterInfo { description = "Oxygen tank 1 capacity (%) (maps to oxygenTank1)."} },
            { "o2", new ParameterInfo { description = "Oxygen tank 2 capacity (%) (maps to oxygenTank2)."} },
            { "o3", new ParameterInfo { description = "Oxygen tank 3 capacity (%) (maps to oxygenTank3)."} },
            { "o4", new ParameterInfo { description = "Oxygen tank 4 capacity (%) (legacy, maps to reserveOxygenTank1)."} },
            { "o5", new ParameterInfo { description = "Oxygen tank 5 capacity (%) (legacy, maps to reserveOxygenTank2)."} },
            { "o6", new ParameterInfo { description = "Oxygen tank 6 capacity (%) (legacy, maps to reserveOxygenTank3)."} },
            { "o7", new ParameterInfo { description = "Oxygen tank 7 capacity (%) (legacy, maps to reserveOxygenTank4)."} },
            { "o8", new ParameterInfo { description = "Oxygen tank 8 capacity (%) (legacy, maps to reserveOxygenTank5)."} },
            { "o9", new ParameterInfo { description = "Oxygen tank 9 capacity (%) (legacy, maps to reserveOxygenTank6)."} },
            { "oxygen", new ParameterInfo { readOnly = true, description = "Overall oxygen tank percentage (%)."} },
            { "oxygenflow", new ParameterInfo { description = "Flow from the oxygen tanks (liters / min)."} },
            { "oxygentank1", new ParameterInfo { description = "Oxygen main tank 1 capacity (%)."} },
            { "oxygentank2", new ParameterInfo { description = "Oxygen main tank 1 capacity (%)."} },
            { "oxygentank3", new ParameterInfo { description = "Oxygen main tank 1 capacity (%)."} },
            { "pilotbuttonenabled", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether the piloting button is enabled in Piloting screen."} },
            { "pitchangle", new ParameterInfo { minValue = -90, maxValue = 90, description = "Sub's current pitch angle (nose up/down, degrees)."} },
            { "pitchspeed", new ParameterInfo { description = "Sub's pitching speed (nose up / down)."} },
            { "playervessel", new ParameterInfo { minValue = 1, maxValue = 4, type = ParameterType.Int, description = "Which vessel is occupied by the players (crew)."} },
            { "playervesselname", new ParameterInfo { type = ParameterType.String, description = "Name of the player vessel."} },
            { "pressure", new ParameterInfo { readOnly = true, description = "Current exterior pressure (bar)"} },
            { "pressureoverride", new ParameterInfo { description = "Override value for exterior pressure (bar)"} },
            { "pressureoverrideamount", new ParameterInfo { maxValue = 1, description = "How much to override the displayed value for exterior pressure (bar)"} },
            { "reserveair", new ParameterInfo { readOnly = true, description = "Overall reserve air tank percentage (%)."} },
            { "reserveairtank1", new ParameterInfo { description = "Reserve air tank 1 capacity (%)."} },
            { "reserveairtank2", new ParameterInfo { description = "Reserve air tank 2 capacity (%)."} },
            { "reserveairtank3", new ParameterInfo { description = "Reserve air tank 3 capacity (%)."} },
            { "reserveairtank4", new ParameterInfo { description = "Reserve air tank 4 capacity (%)."} },
            { "reserveairtank5", new ParameterInfo { description = "Reserve air tank 5 capacity (%)."} },
            { "reserveairtank6", new ParameterInfo { description = "Reserve air tank 6 capacity (%)."} },
            { "reserveairtank7", new ParameterInfo { description = "Reserve air tank 7 capacity (%)."} },
            { "reserveairtank8", new ParameterInfo { description = "Reserve air tank 8 capacity (%)."} },
            { "reserveairtank9", new ParameterInfo { description = "Reserve air tank 9 capacity (%)."} },
            { "reserveoxygen", new ParameterInfo { readOnly = true, description = "Overall reserve oxygen tank percentage (%)."} },
            { "reserveoxygentank1", new ParameterInfo { description = "Oxygen reserve tank 1 capacity (%)."} },
            { "reserveoxygentank2", new ParameterInfo { description = "Oxygen reserve tank 2 capacity (%)."} },
            { "reserveoxygentank3", new ParameterInfo { description = "Oxygen reserve tank 3 capacity (%)."} },
            { "reserveoxygentank4", new ParameterInfo { description = "Oxygen reserve tank 4 capacity (%)."} },
            { "reserveoxygentank5", new ParameterInfo { description = "Oxygen reserve tank 5 capacity (%)."} },
            { "reserveoxygentank6", new ParameterInfo { description = "Oxygen reserve tank 6 capacity (%)."} },
            { "rollangle", new ParameterInfo { minValue = -90, maxValue = 90, description = "Sub's current roll angle (degrees)." } },
            { "rollspeed", new ParameterInfo { description = "Sub's rolling speed."} },
            { "scene", new ParameterInfo { minValue = 1, maxValue = 200, type = ParameterType.Int, description = "The scene currently being filmed." } },
            { "screenglitchamount", new ParameterInfo { description = "Amount of screen glitch across all screens."} },
            { "screenglitchautodecay", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether screen glitch automatically decays over time."} },
            { "screenglitchautodecaytime", new ParameterInfo { maxValue = 1, description = "Time taken for screen glitch to decay to nothing (s)."} },
            { "screenglitchmaxdelay", new ParameterInfo { maxValue = 1, description = "Maximum delay to introduce when screen glitch is in effect."} },
            { "scrubbedco2", new ParameterInfo { maxValue = 5, description = "CO2% in atmosphere after leaving the scrubber." } },
            { "scrubbedhumidity", new ParameterInfo { description = "Humidity leaving the scrubber (%)." } },
            { "scrubbedoxygen", new ParameterInfo { description = "Oxygen percentage leaving the scrubber." } },
            { "shot", new ParameterInfo { minValue = 1, maxValue = 20, type = ParameterType.Int, description = "Shot number within the current scene." } },
            { "sonarheadingup", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether sonar is in Heading Up or North Up mode."} },
            { "sonarlongfrequency", new ParameterInfo { minValue = 0, maxValue = 250, description = "Frequency setting for 360 (long-range) sonar. (kHz)" } },
            { "sonarlonggain", new ParameterInfo { minValue = 50, maxValue = 110, description = "Gain setting for 360 (long-range) sonar. (%)" } },
            { "sonarlongrange", new ParameterInfo { minValue = 1000, maxValue = 6000, type = ParameterType.Int, description = "Range setting for 360 (long-range) sonar (m)." } },
            { "sonarlongsensitivity", new ParameterInfo { minValue = 0, maxValue = 110, description = "Sensitivity setting for 360 (long-range) sonar. (%)" } },
            { "sonarproximity", new ParameterInfo { maxValue = 1, description = "Proximity alert for long range sonar (0 = full alert)." } },
            { "sonarshortfrequency", new ParameterInfo { minValue = 0, maxValue = 1000, description = "Frequency setting for front-scanning (short-range) sonar (kHz)." } },
            { "sonarshortgain", new ParameterInfo { minValue = 50, maxValue = 110, description = "Gain setting for front-scanning (short-range) sonar (%)."} },
            { "sonarshortrange", new ParameterInfo { minValue = 30, maxValue = 120, type = ParameterType.Int, description = "Range setting for front-scanning (short-range) sonar (m)." } },
            { "sonarshortsensitivity", new ParameterInfo { minValue = 0, maxValue = 110, description = "Sensitivity setting for front-scanning (short-range) sonar (%)."} },
            { "startimagesequence", new ParameterInfo { minValue = 0, maxValue = 20, type = ParameterType.Int, description = "Starts an image sequence playing."} },
            { "take", new ParameterInfo { minValue = 1, maxValue = 20, type = ParameterType.Int, description = "Take number for the current shot." } },
            { "timetointercept", new ParameterInfo { description = "Time to Intercept (used by vessel interception logic, drives dueTime when simulation is active."} },
            { "towtargetx", new ParameterInfo { minValue = 0, maxValue = 1, description = "Tow target screen x position from bottom left."} },
            { "towtargety", new ParameterInfo { minValue = 0, maxValue = 1, description = "Tow target screen y position from bottom left."} },
            { "towtargetspeed", new ParameterInfo { minValue = 0, maxValue = 1, description = "Tow target movement speed."} },
            { "towtargetvisible", new ParameterInfo { minValue = 0, maxValue = 1, type = ParameterType.Bool, description = "Tow target visibilty."} },
            { "towfiringpressure", new ParameterInfo { minValue = 0, maxValue = 12000, description = "Tow firing pressure."} },
            { "towfiringpower", new ParameterInfo { minValue = 0, maxValue = 100, description = "Tow firing power as percentage."} },
            { "towfiringstatus", new ParameterInfo { minValue = 0, maxValue = 3, type = ParameterType.Int, description = "Tow firing status: 0 = ready, 1 = acquiring, 2 = locked, 3 = fired."} },
            { "towlinespeed", new ParameterInfo { minValue = 0, maxValue = 1000, description = "Line speed in m/s."} },
            { "towlinelength", new ParameterInfo { minValue = 0, maxValue = 1000, description = "Total tow line length in metres."} },
            { "towlineremaining", new ParameterInfo { minValue = 0, maxValue = 1000, description = "Line remaining in metres."} },
            { "towtargetdistance", new ParameterInfo { minValue = 0, maxValue = 100, description = "Tow target distance in metres."} },
            { "towwinchload", new ParameterInfo { description = "Tow winch load (kg)."} },
            { "variableballastpressure", new ParameterInfo { description = "Variable ballast pressure (psi)."} },
            { "variableballasttemp", new ParameterInfo { description = "Variable ballast temp (°c)."} },
            { "velocity", new ParameterInfo { description = "Sub's current speed (m/s)." } },
            { "version", new ParameterInfo { readOnly = true, description = "Current application version." } },
            { "verticalvelocity", new ParameterInfo { readOnly = true, description = "Sub's current velocity in the vertical direction (m/s)." } },
            { "vesselmovementenabled", new ParameterInfo { maxValue = 1, type = ParameterType.Bool, description = "Whether vessel movement simulation is enabled during playback." } },
            { "watertemp", new ParameterInfo { readOnly = true, description = "Exterior water temperature (computed, degrees C)."} },
            { "watertempoverride", new ParameterInfo { description = "Override value for water temperature (C)"} },
            { "watertempoverrideamount", new ParameterInfo { maxValue = 1, description = "How much to override the displayed value for water temperature (C)"} },
            { "xpos", new ParameterInfo { description = "Sub's X coordinate (m) (Note that this is in the XZ plane, where Y is up/down.)" } },
            { "yawangle", new ParameterInfo { minValue = 0, maxValue = 360, description = "Sub's current yaw angle (heading, degrees)." } },
            { "yawspeed", new ParameterInfo { description = "Sub's yawing speed (heading change)."} },
            { "zpos", new ParameterInfo { description = "Sub's Z coordinate (m) (Note that this is in the XZ plane, where Y is up/down.)" } },
        };
        
        /** Return information about a given parameter. */
        public static ParameterInfo GetServerDataInfo(string valueName)
        {
            var key = valueName.ToLower();
            if (ParameterInfos.ContainsKey(key))
                return ParameterInfos[key];

            return DefaultParameterInfo;
        }

        /** Return the current (possibly noisy) value of a shared state value, indexed by name. */
        public static float GetServerData(string valueName, float defaultValue = Unknown)
        {
            return GetServerDataRaw(valueName, defaultValue);

            /*
            // Bypass 'noisy data' logic for now, as we're concerned it might
            // be causing some stability issues.
             
            if (!ServerObject || string.IsNullOrEmpty(valueName))
                return defaultValue;

            var value = GetServerDataRaw(valueName, defaultValue);
            var noise = NoiseData.Sample(valueName);

            return value + noise;
            */
        }

        /** Return the current value of a shared state value, indexed by name. */
        public static float GetServerDataRaw(string valueName, float defaultValue = Unknown)
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
                    return ServerData.GetDepth();
                case "depthdisplayed":
                    return ServerData.depthDisplayed ? 1 : 0;
                case "depthoverride":
                    return ServerData.depthOverride;
                case "depthoverrideamount":
                    return ServerData.depthOverrideAmount;
                case "floordistancedisplayed":
                    return ServerData.floorDistanceDisplayed ? 1 : 0;
                case "xpos":
                    return ServerObject.transform.position.x;
                case "zpos":
                    return ServerObject.transform.position.z;
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
                case "pressure":
                    return ClientCalcValues.pressureResult;
                case "watertemp":
                    return ClientCalcValues.waterTempResult;
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
                case "pressureoverride":
                    return CabinData.pressureOverride;
                case "pressureoverrideamount":
                    return CabinData.pressureOverrideAmount;
                case "watertempoverride":
                    return CabinData.waterTempOverride;
                case "watertempoverrideamount":
                    return CabinData.waterTempOverrideAmount;
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
                case "error_diagnostics":
                    return ErrorData.error_diagnostics;
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
                case "error_hatch":
                    return ErrorData.error_hatch;
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
                case "error_panel_l":
                    return GliderErrorData.error_panel_l;
                case "error_panel_r":
                    return GliderErrorData.error_panel_r;
                case "error_pressure":
                    return GliderErrorData.error_pressure;
                case "error_structural":
                    return GliderErrorData.error_structural;
                case "error_grapple":
                    return GliderErrorData.error_grapple;
                case "error_system":
                    return GliderErrorData.error_system;
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
                case "inputsource":
                    return ServerData.inputSource;
				case "isautopilot":
					return SubControl.isAutoPilot ? 1 : 0;
				case "iscontroldecentmode":
					return SubControl.isControlDecentMode ? 1 : 0;
				case "iscontrolmodeoverride":
					return SubControl.isControlModeOverride ? 1 : 0;
				case "iscontroloverridestandard":
					return SubControl.isControlOverrideStandard ? 1 : 0;
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
				case "isAutoPilot":
					return SubControl.isAutoPilot ? 1 : 0;
                case "isControlDecentMode":
                    return SubControl.isControlDecentMode ? 1 : 0;
                case "iscontroldecentmodeonjoystick":
                    return ServerData.isControlDecentModeOnJoystick ? 1 : 0;
                case "isControlModeOverride":
                    return SubControl.isControlModeOverride ? 1 : 0;
                case "isControlOverrideStandard":
                    return SubControl.isControlOverrideStandard ? 1 : 0;
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
                case "dockingbuttonenabled":
                    return OperatingData.dockingButtonEnabled ? 1 : 0;
                case "pilotbuttonenabled":
                    return OperatingData.pilotButtonEnabled ? 1 : 0;
                case "vesselmovementenabled":
                    return VesselMovements.Enabled ? 1 : 0;
                case "timetointercept":
                    return VesselMovements.TimeToIntercept;
                case "megspeed":
                    return SonarData.MegSpeed;
                case "megturnspeed":
                    return SonarData.MegTurnSpeed;
				case "motionsafety":
					return SubControl.MotionSafety ? 1 : 0;
				case "motionhazard":
					return SubControl.MotionHazard ? 1 : 0;
				case "motionhazardenabled":
					return SubControl.MotionHazardEnabled ? 1 : 0;
				case "motionbasepitch":
					return SubControl.MotionBasePitch;
				case "motionbaseyaw":
					return SubControl.MotionBaseYaw;
				case "motionbaseroll":
					return SubControl.MotionBaseRoll;
				case "motionslerpspeed":
					return SubControl.MotionSlerpSpeed;
				case "motionscaleimpacts":
					return SubControl.MotionScaleImpacts;
				case "motionminimpactinterval":
					return SubControl.MotionMinImpactInterval;
				case "motionhazardsensitivity":
					return SubControl.MotionHazardSensitivity;
                case "sonarheadingup":
                    return SonarData.HeadingUp ? 1 : 0;
                case "sonarlongfrequency":
                    return SonarData.LongFrequency;
                case "sonarlonggain":
                    return SonarData.LongGain;
                case "sonarlongrange":
                    return SonarData.LongRange;
                case "sonarlongsensitivity":
                    return SonarData.LongSensitivity;
                case "sonarproximity":
                    return SonarData.Proximity;
                case "sonarshortfrequency":
                    return SonarData.ShortFrequency;
                case "sonarshortgain":
                    return SonarData.ShortGain;
                case "sonarshortrange":
                    return SonarData.ShortRange;
                case "sonarshortsensitivity":
                    return SonarData.ShortSensitivity;
                case "dcccommscontent":
                    return DCCScreenData.DCCcommsContent;
                case "dccvesselnameintitle":
                    return DCCScreenData.DCCvesselNameInTitle ? 1 : 0;
                case "dcccommsusesliders":
                    return DCCScreenData.DCCcommsUseSliders ? 1 : 0;
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
                case "screenglitchamount":
                    return ScreenData.screenGlitch;
                case "screenglitchautodecay":
                    return ScreenData.screenGlitchAutoDecay ? 1 : 0;
                case "screenglitchautodecaytime":
                    return ScreenData.screenGlitchAutoDecayTime;
                case "screenglitchmaxdelay":
                    return ScreenData.screenGlitchMaxDelay;
                case "camerabrightness":
                    return ScreenData.cameraBrightness;
                case "startimagesequence":
                    return ScreenData.startImageSequence;
                case "greenscreenbrightness":
                    return ScreenData.greenScreenBrightness;
                case "acidlayer":
                    return MapData.acidLayer;
                case "lightarray1":
                    return LightData.lightArray1;
                case "lightarray2":
                    return LightData.lightArray2;
                case "lightarray3":
                    return LightData.lightArray3;
                case "lightarray4":
                    return LightData.lightArray4;
                case "lightarray5":
                    return LightData.lightArray5;
                case "lightarray6":
                    return LightData.lightArray6;
                case "lightarray7":
                    return LightData.lightArray7;
                case "lightarray8":
                    return LightData.lightArray8;
                case "lightarray9":
                    return LightData.lightArray9;
                case "lightarray10":
                    return LightData.lightArray10;
                case "maxwildlife":
                    return SonarData.MaxWildlife;
                case "docking1":
                    return DockingData.docking1;
                case "docking2":
                    return DockingData.docking2;
                case "docking3":
                    return DockingData.docking3;
                case "docking4":
                    return DockingData.docking4;
                case "bootcodeduration":
                    return PopupData.bootCodeDuration;
                case "bootprogress":
                    return PopupData.bootProgress;
                case "towtargetx":
                    return GLTowingData.towTargetX;
                case "towtargety":
                    return GLTowingData.towTargetY;
                case "towtargetspeed":
                    return GLTowingData.towTargetSpeed;
                case "towtargetvisible":
                    return GLTowingData.towTargetVisible ? 1 : 0;
                case "towfiringpressure":
                    return GLTowingData.towFiringPressure;
                case "towfiringpower":
                    return GLTowingData.towFiringPower;
                case "towfiringstatus":
                    return GLTowingData.towFiringStatus;
                case "towlinespeed":
                    return GLTowingData.towLineSpeed;
                case "towlinelength":
                    return GLTowingData.towLineLength;
                case "towlineremaining":
                    return GLTowingData.towLineRemaining;
                case "towtargetdistance":
                    return GLTowingData.towTargetDistance;
                default:
                    return GetDynamicValue(valueName, defaultValue);
            }
        }

        /** Return a string representation of a shared state value, indexed by name. */
        public static string GetServerDataAsText(string valueName)
        {
            if (string.IsNullOrEmpty(valueName))
                return "";

            if (valueName == "version")
                return Version;

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
                    var dInt = (int) ServerData.GetDepth();
                    return dInt.ToString();
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
				case "motionbasepitch":
					return SubControl.MotionBasePitch.ToString("n1");
				case "motionbaseyaw":
					return SubControl.MotionBaseYaw.ToString("n1");
				case "motionbaseroll":
					return SubControl.MotionBaseRoll.ToString("n1");
				case "motionslerpspeed":
					return SubControl.MotionSlerpSpeed.ToString("n1");
				case "motionhazardsensitivity":
					return SubControl.MotionHazardSensitivity.ToString("n1");
				case "motionscaleimpacts":
					return SubControl.MotionScaleImpacts.ToString("n1");
				case "motionminimpactinterval":
					return SubControl.MotionMinImpactInterval.ToString("n1");
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
                case "version":
                    return Version;
                default:
                    var value = GetServerDataRaw(valueName);
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
        public static bool GetServerBool(string boolName, bool defaultValue = false)
        {
            // Check whether server object is ready.
            if (ServerObject == null)
                return defaultValue;

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
				case "isautopilot":
					return SubControl.isAutoPilot;
				case "iscontroldecentmode":
					return SubControl.isControlDecentMode;
                case "iscontroldecentmodeonjoystick":
                    return ServerData.isControlDecentModeOnJoystick;
                case "iscontrolmodeoverride":
					return SubControl.isControlModeOverride;
				case "iscontroloverridestandard":
					return SubControl.isControlOverrideStandard;
                case "isautostabilised":
                    return SubControl.isAutoStabilised;
                case "ispitchalsostabilised":
                    return SubControl.IsPitchAlsoStabilised;
                case "joystickoverride":
                    return SubControl.JoystickOverride;
                case "joystickpilot":
                    return SubControl.JoystickPilot;
                case "screenglitchautodecay":
                    return ScreenData.screenGlitchAutoDecay;
				case "motionsafety":
					return SubControl.MotionSafety;
				case "motionhazard":
					return SubControl.MotionHazard;
				case "motionhazardenabled":
					return SubControl.MotionHazardEnabled;
                case "dccvesselnameintitle":
                    return DCCScreenData.DCCvesselNameInTitle;
                case "dcccommsusesliders":
                    return DCCScreenData.DCCcommsUseSliders;
                case "sonarheadingup":
                    return SonarData.HeadingUp;
                default:
                    // As a last resort, interpret numeric values as booleans.
                    var value = GetServerData(boolName, defaultValue ? 1 : 0);
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

        /** Add a popup (works on both clients and host). */
        public static void PostAddPopup(popupData.Popup popup)
        {
            if (LocalPlayer)
                LocalPlayer.PostAddPopup(popup);
        }

        /** Toggle a popup on or off. */
        public static void PostTogglePopup(popupData.Popup popup)
        {
            if (LocalPlayer)
                LocalPlayer.PostTogglePopup(popup);
        }

        /** Remove a popup (works on both clients and host). */
        public static void PostRemovePopup(popupData.Popup popup)
        {
            if (LocalPlayer)
                LocalPlayer.PostRemovePopup(popup);
        }

        /** Clear all popups (works on both clients and host). */
        public static void PostClearPopups()
        {
            if (LocalPlayer)
                LocalPlayer.PostClearPopups();
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

        /** Post vessel movement type to the server. */
        public static void PostVesselMovementType(int id, string type)
        {
            if (LocalPlayer)
                LocalPlayer.PostVesselMovementType(id, type);
        }

        /** Post vessel's name to the server. */
        public static void PostVesselName(int id, string name)
        {
            if (LocalPlayer)
                LocalPlayer.PostVesselName(id, name);
        }

        /** Post vessel's depth to the server. */
        public static void PostVesselDepth(int id, float depth)
        {
            if (LocalPlayer)
                LocalPlayer.PostVesselDepth(id, depth);
        }

        /** Post vessel's icon to the server. */
        public static void PostVesselIcon(int id, vesselData.Icon icon)
        {
            if (LocalPlayer)
                LocalPlayer.PostVesselIcon(id, icon);
        }

        /** Post vessel map visibility to the server. */
        public static void PostVesselOnMap(int id, bool value)
        {
            if (LocalPlayer)
                LocalPlayer.PostVesselOnMap(id, value);
        }

        /** Post vessel sonar visibility to the server. */
        public static void PostVesselOnSonar(int id, bool value)
        {
            if (LocalPlayer)
                LocalPlayer.PostVesselOnSonar(id, value);
        }

        /** Post vessel movements state to the server (works on both clients and host). */
        public static void PostVesselMovementState(JSONObject json)
        {
            if (LocalPlayer)
                LocalPlayer.PostVesselMovementState(json);
        }

        /** Add a vessel to the simulation. */
        public static void PostAddVessel(vesselData.Vessel vessel)
        {
            if (LocalPlayer)
                LocalPlayer.PostAddVessel(vessel);
        }

        /** Remove the last vessel from the simulation. */
        public static void PostRemoveLastVessel()
        {
            if (LocalPlayer)
                LocalPlayer.PostRemoveLastVessel();
        }

        /** Clear extra vessels from the simulation. */
        public static void PostClearExtraVessels()
        {
            if (LocalPlayer)
                LocalPlayer.PostClearExtraVessels();
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

            var movement = VesselMovements.GetVesselMovement(playerVessel);
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

        /** Set a vessel's current position (1-based index). */
        public static void PostVesselPosition(int vessel, Vector3 p)
        {
            if (LocalPlayer)
                LocalPlayer.PostVesselPosition(vessel, p);
        }

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

        /** Sets which vessel is controlled by the player (1-based index). */
        public static void SetPlayerVessel(int vessel)
        {
            if (VesselData)
                VesselData.SetPlayerVessel(vessel);
        }

        /** Returns which vessel is controlled by the player (1-based index). */
        public static int GetPlayerVessel()
            { return VesselData ? VesselData.PlayerVessel : 0; }

        /** Returns which vessel is controlled by the player (1-based index). */
        public static string GetPlayerVesselName()
            { return VesselData ? VesselData.GetVessel(VesselData.PlayerVessel).Name : vesselData.Unknown; }

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

        /** Return the current movement mode (if any) for a vessel. */
        public static vesselMovement GetVesselMovement(int vessel)
            { return VesselMovements ? VesselMovements.GetVesselMovement(vessel) : null; }

        /** Return the current player vessel movement mode (if any). */
        public static vesselMovement GetPlayerVesselMovement()
            { return VesselMovements ? VesselMovements.GetPlayerVesselMovement() : null; }

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
            { return NetworkManagerCustom.IsInGlider; }

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

        /** Post screen state for the given player. */
        public static void PostScreenState(NetworkInstanceId playerId, screenData.State state)
        {
            if (LocalPlayer)
                LocalPlayer.PostScreenState(playerId, state);
        }

        /** Post screen state for this player. */
        public static void PostScreenStateContent(NetworkInstanceId playerId, screenData.Content content)
        {
            if (LocalPlayer)
                LocalPlayer.PostScreenStateContent(playerId, content);
        }

        /** Return content ID for the specified DCC screen. */
        public static DCCWindow.contentID GetScreenContent(DCCScreenID._screenID id, int stationId)
        {
            if (!DCCScreenData)
                return 0;

            return DCCScreenData.GetScreenContent(id, stationId);
        }

        /** Post content for the specified DCC screen. */
        public static void PostScreenContent(DCCScreenID._screenID id, DCCWindow.contentID value, int stationId)
        {
            if (LocalPlayer)
                LocalPlayer.PostScreenContent(id, value, stationId);
        }

        /** Post content for the specified quad screen. */
        public static void PostQuadContent(DCCScreenContentPositions.positionID id, DCCWindow.contentID value, int stationId)
        {
            if (LocalPlayer)
                LocalPlayer.PostQuadContent(id, value, stationId);
        }

        /** Return content ID for the specified DCC quad screen. */
        public static DCCWindow.contentID GetQuadContent(DCCScreenContentPositions.positionID id, int stationId)
        {
            if (!DCCScreenData)
                return 0;

            return DCCScreenData.GetQuadContent(id, stationId);
        }

        /** Post cycle state for the specified quad screen. */
        public static void PostQuadCycle(int value, int stationId)
        {
            if (LocalPlayer)
                LocalPlayer.PostQuadCycle(value, stationId);
        }

        /** Return cycle state for the specified DCC quad screen. */
        public static int GetQuadCycle(int stationId)
        {
            if (!DCCScreenData)
                return 0;

            return DCCScreenData.GetQuadCycle(stationId);
        }

        /** Post cycle state for the specified quad screen. */
        public static void PostQuadFullScreen(int value, int stationId)
        {
            if (LocalPlayer)
                LocalPlayer.PostQuadFullScreen(value, stationId);
        }

        /** Return cycle state for the specified DCC quad screen. */
        public static int GetQuadFullScreen(int stationId)
        {
            if (!DCCScreenData)
                return 0;

            return DCCScreenData.GetQuadFullScreen(stationId);
        }

        /** Add noise to a server parameter. */
        public static void PostAddNoise(string parameter, noiseData.Profile profile)
        {
            if (LocalPlayer)
                LocalPlayer.PostAddNoise(parameter, profile);
        }

        /** Remove noise from a server parameter. */
        public static void PostRemoveNoise(string parameter)
        {
            if (LocalPlayer)
                LocalPlayer.PostRemoveNoise(parameter);
        }

        /** Return whether a parameter has noise. */
        public static bool HasNoise(string parameter)
        {
            if (NoiseData)
                return NoiseData.HasNoise(parameter);

            return false;
        }

        /** Expand out substitution values in a string, e.g. '{player-id}'. */
        public static string Expanded(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            // First, expand out configuration values.
            value = Configuration.Expanded(value);

            // Expand out local player values.
            if (LocalPlayer)
                value = value.Replace("{player-id}", LocalPlayer.Id);

            // Lastly, Look for parameter references (e.g. '{o2}') and expand them.
            var matches = Regex.Matches(value, @"{([\w-]+)}");
            foreach (Match match in matches)
            {
                var id = match.Groups[1].Value.ToLower();
                if (Parameters.Contains(id))
                    value = value.Replace("{" + id + "}", GetServerDataAsText(id));
            }

            return value;
        }
    }
}
