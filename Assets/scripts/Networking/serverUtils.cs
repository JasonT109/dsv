using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
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

        /** Return the map data object (contains shared vessel state values.) */
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

        /** Return the map data object (contains shared crew state values.) */
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

        /** Return the map data object (contains shared oxygen state values.) */
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

        /** Return the map data object (contains shared battery state values.) */
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

        /** Return the map data object (contains shared operating state values.) */
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

        /** Return the vessel movements controller. */
        public static vesselMovements VesselMovements
        {
            get { return GetVesselMovements(); }
        }


        // Static Methods
        // ------------------------------------------------------------

        /** Whether the server object is available for use yet. */
        public static bool IsReady()
        {
            return ServerObject != null;
        }

        /** The set of all server data parameters that can be set or read. */
        public static readonly HashSet<string> Parameters = new HashSet<string>
        {
            "b1",
            "b2",
            "b3",
            "b4",
            "b5",
            "b6",
            "b7",
            "ballastpressure",
            "battery",
            "batterytemp",
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
            "depth",
            "disableinput",
            "divertpowertothrusters",
            "divetime",
            "divetimeactive",
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
            "intercept1warning",
            "jet_heat_l",
            "jet_heat_r",
            "latitude",
            "longitude",
            "meg1posx",
            "meg1posy",
            "meg1posz",
            "meg1velocity",
            "meg1vis",
            "meg1warning",
            "megspeed",
            "megturnspeed",
            "o1",
            "o2",
            "o3",
            "o4",
            "o5",
            "o6",
            "o7",
            "oxygen",
            "oxygenflow",
            "pitchangle",
            "pressure",
            "rollangle",
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
            "vessel1vis",
            "vessel1warning",
            "vessel2vis",
            "vessel2warning",
            "vessel3vis",
            "vessel3warning",
            "vessel4vis",
            "vessel4warning",
            "vessel5vis",
            "vessel5warning",
            "vessel6vis",
            "vessel6warning",
            "vesselmovementsactive",
            "watertemp",
            "xpos",
            "yawangle",
            "zpos",
        };


        /** Return the current value of a shared state value, indexed by name. */
        public static float GetServerData(string valueName)
        {
            if (!ServerObject)
                return Unknown;

            switch (valueName.ToLower())
            {
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
                case "disableinput":
                    return ServerData.disableInput ? 1 : 0;
                case "b1":
                    return ServerData.batteries[0];
                case "b2":
                    return ServerData.batteries[1];
                case "b3":
                    return ServerData.batteries[2];
                case "b4":
                    return ServerData.batteries[3];
                case "b5":
                    return ServerData.batteries[4];
                case "b6":
                    return ServerData.batteries[5];
                case "b7":
                    return ServerData.batteries[6];
                case "o1":
                    return ServerData.oxygenTanks[0];
                case "o2":
                    return ServerData.oxygenTanks[1];
                case "o3":
                    return ServerData.oxygenTanks[2];
                case "o4":
                    return ServerData.oxygenTanks[3];
                case "o5":
                    return ServerData.oxygenTanks[4];
                case "o6":
                    return ServerData.oxygenTanks[5];
                case "o7":
                    return ServerData.oxygenTanks[6];
                case "oxygen":
                    return OxygenData.oxygen;
                case "oxygenflow":
                    return OxygenData.oxygenFlow;
                case "co2":
                    return OxygenData.Co2;
                case "co2ppm":
                    return OxygenData.Co2Ppm;
                case "cabinpressure":
                    return OxygenData.cabinPressure;
                case "cabinpressurepsi":
                    return OxygenData.cabinPressurePsi;
                case "cabinoxygen":
                    return OxygenData.cabinOxygen;
                case "cabintemp":
                    return OxygenData.cabinTemp;
                case "cabinhumidity":
                    return OxygenData.cabinHumidity;
                case "battery":
                    return BatteryData.battery;
                case "batterytemp":
                    return BatteryData.batteryTemp;
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
                case "inputxaxis":
                    return ServerData.inputXaxis;
                case "inputyaxis":
                    return ServerData.inputYaxis;
                case "inputzaxis":
                    return ServerData.inputZaxis;
                case "inputxaxis2":
                    return ServerData.inputXaxis2;
                case "inputyaxis2":
                    return ServerData.inputYaxis2;
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
                case "v1posx":
                    return MapData.vessel1Pos.x;
                case "v1posy":
                    return MapData.vessel1Pos.y;
                case "v1posz":
                case "v1depth":
                    return MapData.vessel1Pos.z;
                case "v2posx":
                    return MapData.vessel2Pos.x;
                case "v2posy":
                    return MapData.vessel2Pos.y;
                case "v2posz":
                case "v2depth":
                    return MapData.vessel2Pos.z;
                case "v3posx":
                    return MapData.vessel3Pos.x;
                case "v3posy":
                    return MapData.vessel3Pos.y;
                case "v3posz":
                case "v3depth":
                    return MapData.vessel3Pos.z;
                case "v4posx":
                    return MapData.vessel4Pos.x;
                case "v4posy":
                    return MapData.vessel4Pos.y;
                case "v4posz":
                case "v4depth":
                    return MapData.vessel4Pos.z;
                case "v5posx":
                case "meg1posx":
                    return MapData.meg1Pos.x;
                case "v5posy":
                case "meg1posy":
                    return MapData.meg1Pos.y;
                case "v5posz":
                case "v5depth":
                case "meg1posz":
                    return MapData.meg1Pos.z;
                case "v6posx":
                case "intercept1posx":
                    return MapData.intercept1Pos.x;
                case "v6posy":
                case "intercept1posy":
                    return MapData.intercept1Pos.y;
                case "v6posz":
                case "v6depth":
                case "intercept1posz":
                    return MapData.intercept1Pos.z;
                case "v1velocity":
                    return MapData.vessel1Velocity;
                case "v2velocity":
                    return MapData.vessel2Velocity;
                case "v3velocity":
                    return MapData.vessel3Velocity;
                case "v4velocity":
                    return MapData.vessel4Velocity;
                case "v5velocity":
                case "meg1velocity":
                    return MapData.meg1Velocity;
                case "v6velocity":
                case "intercept1velocity":
                    return MapData.intercept1Velocity;
                case "vessel1vis":
                    return MapData.vessel1Vis ? 1.0f : 0.0f;
                case "vessel2vis":
                    return MapData.vessel2Vis ? 1.0f : 0.0f;
                case "vessel3vis":
                    return MapData.vessel3Vis ? 1.0f : 0.0f;
                case "vessel4vis":
                    return MapData.vessel4Vis ? 1.0f : 0.0f;
                case "vessel5vis":
                case "meg1vis":
                    return MapData.meg1Vis ? 1.0f : 0.0f;
                case "vessel6vis":
                case "intercept1vis":
                    return MapData.intercept1Vis ? 1.0f : 0.0f;
                case "vessel1warning":
                    return MapData.vessel1Warning ? 1.0f : 0.0f;
                case "vessel2warning":
                    return MapData.vessel2Warning ? 1.0f : 0.0f;
                case "vessel3warning":
                    return MapData.vessel3Warning ? 1.0f : 0.0f;
                case "vessel4warning":
                    return MapData.vessel4Warning ? 1.0f : 0.0f;
                case "vessel5warning":
                case "meg1warning":
                    return MapData.meg1Warning ? 1.0f : 0.0f;
                case "vessel6warning":
                case "intercept1warning":
                    return MapData.intercept1Warning ? 1.0f : 0.0f;
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
                case "vesselmovementsactive":
                    return VesselMovements.Active ? 1 : 0;
                case "timetointercept":
                    return VesselMovements.TimeToIntercept;
                case "megspeed":
                    return SonarData.MegSpeed;
                case "megturnspeed":
                    return SonarData.MegTurnSpeed;
                default:
                    return Unknown;
            }
        }

        /** Return a string representation of a shared state value, indexed by name. */
        public static string GetServerDataAsText(string valueName)
        {
            if (!ServerObject)
                return "no value";

            switch (valueName.ToLower())
            {
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
                    return ServerData.batteries[0].ToString("n1");
                case "b2":
                    return ServerData.batteries[1].ToString("n1");
                case "b3":
                    return ServerData.batteries[2].ToString("n1");
                case "b4":
                    return ServerData.batteries[3].ToString("n1");
                case "b5":
                    return ServerData.batteries[4].ToString("n1");
                case "b6":
                    return ServerData.batteries[5].ToString("n1");
                case "b7":
                    return ServerData.batteries[6].ToString("n1");
                case "o1":
                    return ServerData.oxygenTanks[0].ToString("n1");
                case "o2":
                    return ServerData.oxygenTanks[1].ToString("n1");
                case "o3":
                    return ServerData.oxygenTanks[2].ToString("n1");
                case "o4":
                    return ServerData.oxygenTanks[3].ToString("n1");
                case "o5":
                    return ServerData.oxygenTanks[4].ToString("n1");
                case "o6":
                    return ServerData.oxygenTanks[5].ToString("n1");
                case "o7":
                    return ServerData.oxygenTanks[6].ToString("n1");
                case "oxygen":
                    return (OxygenData.oxygen.ToString("n1") + "%");
                case "oxygenflow":
                    return (OxygenData.oxygenFlow.ToString("n0") + "lpm");
                case "co2":
                    return (OxygenData.Co2.ToString() + "%");
                case "co2ppm":
                    return OxygenData.Co2Ppm.ToString("n1") + "ppm";
                case "cabinpressure":
                    return OxygenData.cabinPressure.ToString();
                case "cabinpressurepsi":
                    return OxygenData.cabinPressurePsi.ToString("n1");
                case "cabinoxygen":
                    return (OxygenData.cabinOxygen.ToString("n0") + "lpm");
                case "cabinhumidity":
                    return (OxygenData.cabinHumidity.ToString("n1") + "%");
                case "cabintemp":
                    return OxygenData.cabinTemp.ToString();
                case "battery":
                    return (BatteryData.battery.ToString("n1") + "%");
                case "batterytemp":
                    return (BatteryData.batteryTemp.ToString("n1") + "°c");
                case "pilot":
                    return ServerData.pilot;
                case "verticalvelocity":
                    return ServerData.verticalVelocity.ToString("n1");
                case "horizontalvelocity":
                    return ServerData.horizontalVelocity.ToString("n1");
                case "inputxaxis":
                    return ServerData.inputXaxis.ToString("n1");
                case "inputyaxis":
                    return ServerData.inputYaxis.ToString("n1");
                case "inputzaxis":
                    return ServerData.inputZaxis.ToString("n1");
                case "inputxaxis2":
                    return ServerData.inputXaxis2.ToString("n1");
                case "inputyaxis2":
                    return ServerData.inputYaxis2.ToString("n1");
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
                case "v1depth":
                    return MapData.vessel1Pos.z.ToString("n0");
                case "v2depth":
                    return MapData.vessel2Pos.z.ToString("n0");
                case "v3depth":
                    return MapData.vessel3Pos.z.ToString("n0");
                case "v4depth":
                    return MapData.vessel4Pos.z.ToString("n0");
                case "v5depth":
                case "meg1depth":
                    return MapData.meg1Pos.z.ToString("n0");
                case "v6depth":
                case "intercept1depth":
                    return MapData.intercept1Pos.z.ToString("n0");
                case "v1velocity":
                    return MapData.vessel1Velocity.ToString("n1");
                case "v2velocity":
                    return MapData.vessel2Velocity.ToString("n1");
                case "v3velocity":
                    return MapData.vessel3Velocity.ToString("n1");
                case "v4velocity":
                    return MapData.vessel4Velocity.ToString("n1");
                case "v5velocity":
                case "meg1velocity":
                    return MapData.meg1Velocity.ToString("n1");
                case "v6velocity":
                case "intercept1velocity":
                    return MapData.intercept1Velocity.ToString("n1");
                case "mapeventname":
                    return MapData.mapEventName;
                case "vessel1vis":
                    return MapData.vessel1Vis.ToString();
                case "vessel2vis":
                    return MapData.vessel2Vis.ToString();
                case "vessel3vis":
                    return MapData.vessel3Vis.ToString();
                case "vessel4vis":
                    return MapData.vessel4Vis.ToString();
                case "vessel5vis":
                case "meg1vis":
                    return MapData.meg1Vis.ToString();
                case "vessel6vis":
                case "intercept1vis":
                    return MapData.intercept1Vis.ToString();
                case "vessel1warning":
                    return MapData.vessel1Warning.ToString();
                case "vessel2warning":
                    return MapData.vessel2Warning.ToString();
                case "vessel3warning":
                    return MapData.vessel3Warning.ToString();
                case "vessel4warning":
                    return MapData.vessel4Warning.ToString();
                case "vessel5warning":
                case "meg1warning":
                    return MapData.meg1Warning.ToString();
                case "vessel6warning":
                case "intercept1warning":
                    return MapData.intercept1Warning.ToString();
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
                case "vesselmovementsactive":
                    return VesselMovements.Active.ToString();
                case "timetointercept":
                    return VesselMovements.TimeToIntercept.ToString();
                case "megspeed":
                    return SonarData.MegSpeed.ToString("n1");
                case "megturnspeed":
                    return SonarData.MegTurnSpeed.ToString("n1");
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

        /** Set the current value of a shared numeric state value by name (only works on host). */
        public static void SetServerData(string valueName, float value)
        {
            ServerData.OnValueChanged(valueName, value);
        }

        /** Set the current value of a shared string state value by name (only works on host). */
        public static void SetServerData(string valueName, string value)
        {
            ServerData.OnValueChanged(valueName, value);
        }

        /** Set the current value of a shared numeric state value by name (works on both clients and host). */
        public static void PostServerData(string valueName, float value)
        {
            if (ServerData && ServerData.isServer)
                SetServerData(valueName, value);
            else if (LocalPlayer)
                LocalPlayer.PostServerData(valueName, value);
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
        public static void PostSonarClear(megEventSonar sonarEvent)
        {
            if (LocalPlayer)
                LocalPlayer.PostSonarClear(sonarEvent);
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
        public static void PostVesselMovementsState(JSONObject json)
        {
            if (LocalPlayer)
                LocalPlayer.PostVesselMovementsState(json);
        }

        /** Set a battery bank value (only works on host). */
        public static void SetBatteryData(int bank, float value)
        {
            ServerData.OnBatterySliderChanged(bank, value);
        }

        /** Set an oxygen bank value (only works on host). */
        public static void SetOxygenData(int bank, float value)
        {
            ServerData.OnOxygenSliderChanged(bank, value);
        }

        /** Number of vessels that can be displayed on the map. */
        public static int GetVesselCount()
        {
            return mapData.VesselCount;
        }

        /** Set a vessel's current position and nominal speed (1-based index). */
        public static void SetVesselData(int vessel, Vector3 pos, float vesselVelocity)
        {
            ServerData.OnVesselDataChanged(vessel, pos, vesselVelocity);
        }

        /** Return a vessel's current depth (1-based index). */
        public static float GetVesselDepth(int vessel)
        {
            return GetVesselData(vessel)[2];
        }

        /** Set a vessel's current depth (1-based index). */
        public static void SetVesselDepth(int vessel, float depth)
        {
            Vector3 position;
            float velocity;

            GetVesselData(vessel, out position, out velocity);
            position.z = depth;
            SetVesselData(vessel, position, velocity);
        }

        /** Return a vessel's current position (1-based index). */
        public static Vector3 GetVesselPosition(int vessel)
        {
            var data = GetVesselData(vessel);
            return new Vector3(data[0], data[1], data[2]);
        }

        /** Return a vessel's velocity (1-based index). */
        public static float GetVesselVelocity(int vessel)
        {
            var data = GetVesselData(vessel);
            return data[3];
        }

        /** Return the player vessel's current target vessel (or 0 if there is no target). */
        public static int GetTargetVessel()
        {
            var playerVessel = serverUtils.GetPlayerVessel();
            if (playerVessel <= 0)
                return 0;

            var movement = serverUtils.GetVesselMovements().GetVesselMovement(playerVessel);
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
        {
            Vector3 position;
            float velocity;
            GetVesselData(vessel, out position, out velocity);
            SetVesselData(vessel, p, velocity);
        }

        /** Set a vessel's current speed (1-based index). */
        public static void SetVesselVelocity(int vessel, float v)
        {
            Vector3 position;
            float velocity;
            GetVesselData(vessel, out position, out velocity);
            SetVesselData(vessel, position, v);
        }

        /** Return vessel's current position/velocity data (1-based index). */
        public static void GetVesselData(int vessel, out Vector3 position, out float velocity)
        {
            var data = GetVesselData(vessel);
            position = new Vector3(data[0], data[1], data[2]);
            velocity = data[3];
        }

        /** Return the distance between two vessels in meters. */
        public static float GetVesselDistance(int from, int to)
        {
            var a = GetVesselPosition(from); a.x *= 1000; a.y *= 1000;
            var b = GetVesselPosition(to); b.x *= 1000; b.y *= 1000;
            return Vector3.Distance(a, b);
        }

        /** Return a vessel's current position as a latitude/longitude pair (1-based index). */
        public static Vector2 GetVesselLatLong(int vessel)
        {
            var position = GetVesselPosition(vessel);
            var dx = position.x * 1000;
            var dy = position.y * 1000;

            double latitude = GetServerData("latitude");
            double longitude = GetServerData("longitude");

            latitude = latitude + (dy / Conversions.EarthRadius) * (180 / Math.PI);
            longitude = longitude + (dx / Conversions.EarthRadius) * (180 / Math.PI) / Math.Cos(latitude * Math.PI / 180);

            return new Vector2((float) longitude, (float) latitude);
        }

        /** Return a vessel's current state as am [x,y,z,speed] tuple (1-based index). */
        public static float[] GetVesselData(int vessel)
        {
            //get vessels map space position

            float[] vesselData = new float[4];

            switch (vessel)
            {
                case 1:
                    vesselData[0] = MapData.vessel1Pos.x;
                    vesselData[1] = MapData.vessel1Pos.y;
                    vesselData[2] = MapData.vessel1Pos.z;
                    vesselData[3] = MapData.vessel1Velocity;
                    break;
                case 2:
                    vesselData[0] = MapData.vessel2Pos.x;
                    vesselData[1] = MapData.vessel2Pos.y;
                    vesselData[2] = MapData.vessel2Pos.z;
                    vesselData[3] = MapData.vessel2Velocity;
                    break;
                case 3:
                    vesselData[0] = MapData.vessel3Pos.x;
                    vesselData[1] = MapData.vessel3Pos.y;
                    vesselData[2] = MapData.vessel3Pos.z;
                    vesselData[3] = MapData.vessel3Velocity;
                    break;
                case 4:
                    vesselData[0] = MapData.vessel4Pos.x;
                    vesselData[1] = MapData.vessel4Pos.y;
                    vesselData[2] = MapData.vessel4Pos.z;
                    vesselData[3] = MapData.vessel4Velocity;
                    break;
                case 5:
                    vesselData[0] = MapData.meg1Pos.x;
                    vesselData[1] = MapData.meg1Pos.y;
                    vesselData[2] = MapData.meg1Pos.z;
                    vesselData[3] = MapData.meg1Velocity;
                    break;
                case 6:
                    vesselData[0] = MapData.intercept1Pos.x;
                    vesselData[1] = MapData.intercept1Pos.y;
                    vesselData[2] = MapData.intercept1Pos.z;
                    vesselData[3] = MapData.intercept1Velocity;
                    break;
            }

            return vesselData;
        }

        /** Return the vessel movements manager. */
        public static vesselMovements GetVesselMovements()
        {
            return ServerObject ? ServerObject.GetComponent<vesselMovements>() : null;
        }

        /** Sets which vessel is controlled by the player (1-based index). */
        public static void SetPlayerVessel(int vessel)
        {
            //Debug.Log("Setting player vessel to: " + vessel);
            if (ServerObject != null)
                ServerData.SetPlayerVessel(vessel);
        }

        /** Returns which vessel is controlled by the player (1-based index). */
        public static int GetPlayerVessel()
        {
            if (!ServerObject)
                return 0;

            return MapData.playerVessel;
        }

        /** Sets the player vessel's velocity in world space. */
        public static void SetPlayerWorldVelocity(Vector3 velocity)
        {
            if (ServerObject != null)
                ServerData.SetPlayerWorldVelocity(velocity);
        }

        /** Return a vessel's visibility on the navigation map (1-based index). */
        public static bool GetVesselVis(int vessel)
        {
            if (ServerObject == null)
                return false;

            bool vesselVis = true;

            switch (vessel)
            {
                case 1:
                    vesselVis = MapData.vessel1Vis;
                    break;
                case 2:
                    vesselVis = MapData.vessel2Vis;
                    break;
                case 3:
                    vesselVis = MapData.vessel3Vis;
                    break;
                case 4:
                    vesselVis = MapData.vessel4Vis;
                    break;
                case 5:
                    vesselVis = MapData.meg1Vis;
                    break;
                case 6:
                    vesselVis = MapData.intercept1Vis;
                    break;
            }

            return vesselVis;
        }

        /** Sets a vessel's visibility on the navigation map (1-based index). */
        public static void SetVesselVis(int vessel, bool state)
        {
            ServerData.SetVesselVis(vessel, state);
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
