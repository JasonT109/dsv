using System;
using UnityEngine;
using System.Collections;
using Meg.Graphics;

namespace Meg.Networking
{
    public class serverUtils : MonoBehaviour
    {

        // Static Properties
        // ------------------------------------------------------------

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


        // Static Methods
        // ------------------------------------------------------------

        public static bool IsReady()
        {
            return ServerObject != null;
        }

        public static float GetServerData(string valueName)
        {
            if (!ServerObject)
                return 0;

            switch (valueName)
            {
                case "depth":
                    return ServerData.depth;
                case "xPos":
                    return ServerObject.transform.position.x;
                case "zPos":
                    return ServerObject.transform.position.z;
                case "pressure":
                    return ServerData.pressure;
                case "heading":
                    return ServerData.heading;
                case "pitchAngle":
                    return ServerData.pitchAngle;
                case "yawAngle":
                    return ServerData.yawAngle;
                case "rollAngle":
                    return ServerData.rollAngle;
                case "velocity":
                    return ServerData.velocity;
                case "floorDistance":
                    return ServerData.floorDistance;
                case "diveTime":
                    return ServerData.diveTime;
                case "dueTime":
                    return ServerData.dueTime;
                case "waterTemp":
                    return ServerData.waterTemp;
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
                case "oxygenFlow":
                    return OxygenData.oxygenFlow;
                case "Co2":
                    return OxygenData.Co2;
                case "Co2Ppm":
                    return OxygenData.Co2Ppm;
                case "cabinPressure":
                    return OxygenData.cabinPressure;
                case "cabinPressurePsi":
                    return OxygenData.cabinPressurePsi;
                case "cabinOxygen":
                    return OxygenData.cabinOxygen;
                case "cabinTemp":
                    return OxygenData.cabinTemp;
                case "cabinHumidity":
                    return OxygenData.cabinHumidity;
                case "battery":
                    return BatteryData.battery;
                case "batteryTemp":
                    return BatteryData.batteryTemp;
                case "error_bilgeLeak":
                    return ErrorData.error_bilgeLeak;
                case "error_batteryLeak":
                    return ErrorData.error_batteryLeak;
                case "error_electricLeak":
                    return ErrorData.error_electricLeak;
                case "error_oxygenExt":
                    return ErrorData.error_oxygenExt;
                case "error_vhf":
                    return ErrorData.error_vhf;
                case "error_forwardSonar":
                    return ErrorData.error_forwardSonar;
                case "error_depthSonar":
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
                case "error_sternLights":
                    return ErrorData.error_sternLights;
                case "error_bowLights":
                    return ErrorData.error_bowLights;
                case "error_portLights":
                    return ErrorData.error_portLights;
                case "error_bowThruster":
                    return ErrorData.error_bowThruster;
                case "error_hyrdaulicRes":
                    return ErrorData.error_hyrdaulicRes;
                case "error_starboardLights":
                    return ErrorData.error_starboardLights;
                case "error_runningLights":
                    return ErrorData.error_runningLights;
                case "error_ballastTank":
                    return ErrorData.error_ballastTank;
                case "error_hydraulicPump":
                    return ErrorData.error_hydraulicPump;
                case "error_oxygenPump":
                    return ErrorData.error_oxygenPump;
                case "inputXaxis":
                    return ServerData.inputXaxis;
                case "inputYaxis":
                    return ServerData.inputYaxis;
                case "inputZaxis":
                    return ServerData.inputZaxis;
                case "inputXaxis2":
                    return ServerData.inputXaxis2;
                case "inputYaxis2":
                    return ServerData.inputYaxis2;
                case "verticalVelocity":
                    return ServerData.verticalVelocity;
                case "horizontalVelocity":
                    return ServerData.horizontalVelocity;
                case "crewHeartRate1":
                    return CrewData.crewHeartRate1;
                case "crewHeartRate2":
                    return CrewData.crewHeartRate2;
                case "crewHeartRate3":
                    return CrewData.crewHeartRate3;
                case "crewHeartRate4":
                    return CrewData.crewHeartRate4;
                case "crewHeartRate5":
                    return CrewData.crewHeartRate5;
                case "crewHeartRate6":
                    return CrewData.crewHeartRate6;
                case "crewBodyTemp1":
                    return CrewData.crewBodyTemp1;
                case "crewBodyTemp2":
                    return CrewData.crewBodyTemp2;
                case "crewBodyTemp3":
                    return CrewData.crewBodyTemp3;
                case "crewBodyTemp4":
                    return CrewData.crewBodyTemp4;
                case "crewBodyTemp5":
                    return CrewData.crewBodyTemp5;
                case "crewBodyTemp6":
                    return CrewData.crewBodyTemp6;
                case "v1posX":
                    return MapData.vessel1Pos.x;
                case "v1posY":
                    return MapData.vessel1Pos.y;
                case "v1posZ":
                    return MapData.vessel1Pos.z;
                case "v2posX":
                    return MapData.vessel2Pos.x;
                case "v2posY":
                    return MapData.vessel2Pos.y;
                case "v2posZ":
                    return MapData.vessel2Pos.z;
                case "v3posX":
                    return MapData.vessel3Pos.x;
                case "v3posY":
                    return MapData.vessel3Pos.y;
                case "v3posZ":
                    return MapData.vessel3Pos.z;
                case "v4posX":
                    return MapData.vessel4Pos.x;
                case "v4posY":
                    return MapData.vessel4Pos.y;
                case "v4posZ":
                    return MapData.vessel4Pos.z;
                case "meg1posX":
                    return MapData.meg1Pos.x;
                case "meg1posY":
                    return MapData.meg1Pos.y;
                case "meg1posZ":
                    return MapData.meg1Pos.z;
                case "intercept1posX":
                    return MapData.intercept1Pos.x;
                case "intercept1posY":
                    return MapData.intercept1Pos.y;
                case "intercept1posZ":
                    return MapData.intercept1Pos.z;
                case "v1velocity":
                    return MapData.vessel1Velocity;
                case "v2velocity":
                    return MapData.vessel2Velocity;
                case "v3velocity":
                    return MapData.vessel3Velocity;
                case "v4velocity":
                    return MapData.vessel4Velocity;
                case "meg1velocity":
                    return MapData.meg1Velocity;
                case "intercept1velocity":
                    return MapData.intercept1Velocity;
                case "vessel1Vis":
                    return MapData.vessel1Vis ? 1.0f : 0.0f;
                case "vessel2Vis":
                    return MapData.vessel2Vis ? 1.0f : 0.0f;
                case "vessel3Vis":
                    return MapData.vessel3Vis ? 1.0f : 0.0f;
                case "vessel4Vis":
                    return MapData.vessel4Vis ? 1.0f : 0.0f;
                case "meg1Vis":
                    return MapData.meg1Vis ? 1.0f : 0.0f;
                case "intercept1Vis":
                    return MapData.intercept1Vis ? 1.0f : 0.0f;
                case "vessel1Warning":
                    return MapData.vessel1Warning ? 1.0f : 0.0f;
                case "vessel2Warning":
                    return MapData.vessel2Warning ? 1.0f : 0.0f;
                case "vessel3Warning":
                    return MapData.vessel3Warning ? 1.0f : 0.0f;
                case "vessel4Warning":
                    return MapData.vessel4Warning ? 1.0f : 0.0f;
                case "meg1Warning":
                    return MapData.meg1Warning ? 1.0f : 0.0f;
                case "intercept1Warning":
                    return MapData.intercept1Warning ? 1.0f : 0.0f;
                case "initiateMapEvent":
                    return MapData.initiateMapEvent;
                case "latitude":
                    return MapData.latitude;
                case "longitude":
                    return MapData.longitude;
                case "towWinchLoad":
                    return OperatingData.towWinchLoad;
                case "hydraulicTemp":
                    return OperatingData.hydraulicTemp;
                case "hydraulicPressure":
                    return OperatingData.hydraulicPressure;
                case "ballastPressure":
                    return OperatingData.ballastPressure;
                case "variableBallastTemp":
                    return OperatingData.variableBallastTemp;
                case "variableBallastPressure":
                    return OperatingData.variableBallastPressure;
                case "commsSignalStrength":
                    return OperatingData.commsSignalStrength;
                case "divertPowerToThrusters":
                    return OperatingData.divertPowerToThrusters;
                default:
                    return 0;
            }
        }

        public static string GetServerDataAsText(string valueName)
        {
            if (!ServerObject)
                return "no value";

            switch (valueName)
            {
                case "depth":
                    int dInt = (int)ServerData.depth;
                    return dInt.ToString();
                case "pressure":
                    return ServerData.pressure.ToString();
                case "heading":
                    return (ServerData.heading.ToString("n1") + "°");
                case "pitchAngle":
                    return (ServerData.pitchAngle.ToString("n1") + "°");
                case "yawAngle":
                    return (ServerData.yawAngle.ToString("n1") + "°");
                case "rollAngle":
                    return (ServerData.rollAngle.ToString("n1") + "°");
                case "velocity":
                    return ServerData.velocity.ToString("n1");
                case "floorDistance":
                    int flInt = (int)ServerData.floorDistance;
                    return flInt.ToString();
                case "diveTime":
                    var diveSpan = TimeSpan.FromSeconds(ServerData.diveTime);
                    return string.Format("{0:00}:{1:00}:{2:00}", diveSpan.Hours, diveSpan.Minutes, diveSpan.Seconds);
                case "dueTime":
                    var dueSpan = TimeSpan.FromSeconds(ServerData.dueTime);
                    return string.Format("{0:00}:{1:00}:{2:00}", dueSpan.Hours, dueSpan.Minutes, dueSpan.Seconds);
                case "waterTemp":
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
                case "oxygenFlow":
                    return (OxygenData.oxygenFlow.ToString("n0") + "lpm");
                case "Co2":
                    return (OxygenData.Co2.ToString() + "%");
                case "Co2Ppm":
                    return OxygenData.Co2Ppm.ToString("n1") + "ppm";
                case "cabinPressure":
                    return OxygenData.cabinPressure.ToString();
                case "cabinPressurePsi":
                    return OxygenData.cabinPressurePsi.ToString("n1");
                case "cabinOxygen":
                    return (OxygenData.cabinOxygen.ToString("n0") + "lpm");
                case "cabinHumidity":
                    return (OxygenData.cabinHumidity.ToString("n1") + "%");
                case "cabinTemp":
                    return OxygenData.cabinTemp.ToString();
                case "battery":
                    return (BatteryData.battery.ToString("n1") + "%");
                case "batteryTemp":
                    return (BatteryData.batteryTemp.ToString("n1") + "°c");
                case "pilot":
                    return ServerData.pilot;
                case "verticalVelocity":
                    return ServerData.verticalVelocity.ToString("n1");
                case "horizontalVelocity":
                    return ServerData.horizontalVelocity.ToString("n1");
                case "inputXaxis":
                    return ServerData.inputXaxis.ToString("n1");
                case "inputYaxis":
                    return ServerData.inputYaxis.ToString("n1");
                case "inputZaxis":
                    return ServerData.inputZaxis.ToString("n1");
                case "inputXaxis2":
                    return ServerData.inputXaxis2.ToString("n1");
                case "inputYaxis2":
                    return ServerData.inputYaxis2.ToString("n1");
                case "crewHeartRate1":
                    return CrewData.crewHeartRate1.ToString("n1");
                case "crewHeartRate2":
                    return CrewData.crewHeartRate2.ToString("n1");
                case "crewHeartRate3":
                    return CrewData.crewHeartRate3.ToString("n1");
                case "crewHeartRate4":
                    return CrewData.crewHeartRate4.ToString("n1");
                case "crewHeartRate5":
                    return CrewData.crewHeartRate5.ToString("n1");
                case "crewHeartRate6":
                    return CrewData.crewHeartRate6.ToString("n1");
                case "crewBodyTemp1":
                    return CrewData.crewBodyTemp1.ToString("n1");
                case "crewBodyTemp2":
                    return CrewData.crewBodyTemp2.ToString("n1");
                case "crewBodyTemp3":
                    return CrewData.crewBodyTemp3.ToString("n1");
                case "crewBodyTemp4":
                    return CrewData.crewBodyTemp4.ToString("n1");
                case "crewBodyTemp5":
                    return CrewData.crewBodyTemp5.ToString("n1");
                case "crewBodyTemp6":
                    return CrewData.crewBodyTemp6.ToString("n1");
                case "v1depth":
                    return MapData.vessel1Pos.z.ToString("n0");
                case "v2depth":
                    return MapData.vessel2Pos.z.ToString("n0");
                case "v3depth":
                    return MapData.vessel3Pos.z.ToString("n0");
                case "v4depth":
                    return MapData.vessel4Pos.z.ToString("n0");
                case "meg1depth":
                    return MapData.meg1Pos.z.ToString("n0");
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
                case "meg1velocity":
                    return MapData.meg1Velocity.ToString("n1");
                case "intercept1velocity":
                    return MapData.intercept1Velocity.ToString("n1");
                case "mapEventName":
                    return MapData.mapEventName;
                case "latitude":
                    return FormatLatitude(MapData.latitude);
                case "longitude":
                    return FormatLongitude(MapData.longitude);
                case "towWinchLoad":
                    return OperatingData.towWinchLoad.ToString("n0");
                case "hydraulicTemp":
                    return OperatingData.hydraulicTemp.ToString("n1") + "°c";
                case "hydraulicPressure":
                    return OperatingData.hydraulicPressure.ToString("n1");
                case "ballastPressure":
                    return OperatingData.ballastPressure.ToString("n1") + "°c";
                case "variableBallastTemp":
                    return OperatingData.variableBallastTemp.ToString("n1") + "°c";
                case "variableBallastPressure":
                    return OperatingData.variableBallastPressure.ToString("n1");
                case "commsSignalStrength":
                    return OperatingData.commsSignalStrength.ToString("n1");
                case "divertPowerToThrusters":
                    return OperatingData.divertPowerToThrusters.ToString("n1");
                default:
                    return "no value";
            }
        }

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

        public static void SetServerData(string valueName, float value)
        {
            ServerData.OnValueChanged(valueName, value);
        }

        public static void SetServerData(string valueName, string value)
        {
            ServerData.OnValueChanged(valueName, value);
        }

        public static void SetBatteryData(int bank, float value)
        {
            ServerData.OnBatterySliderChanged(bank, value);
        }

        public static void SetOxygenData(int bank, float value)
        {
            ServerData.OnOxygenSliderChanged(bank, value);
        }

        public static void SetVesselData(int vessel, Vector3 pos, float vesselVelocity)
        {
            ServerData.OnVesselDataChanged(vessel, pos, vesselVelocity);
        }

        public static float GetVesselDepth(int vessel)
        {
            return GetVesselData(vessel)[2];
        }

        public static void SetVesselDepth(int vessel, float depth)
        {
            Vector3 position;
            float velocity;

            GetVesselData(vessel, out position, out velocity);
            position.z = depth;
            SetVesselData(vessel, position, velocity);
        }

        public static Vector3 GetVesselPosition(int vessel)
        {
            var data = GetVesselData(vessel);
            return new Vector3(data[0], data[1], data[2]);
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

        public static void SetVesselPosition(int vessel, Vector3 p)
        {
            Vector3 position;
            float velocity;
            GetVesselData(vessel, out position, out velocity);
            SetVesselData(vessel, p, velocity);
        }

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

        public static vesselMovements GetVesselMovements()
        {
            return ServerObject ? ServerObject.GetComponent<vesselMovements>() : null;
        }

        public static void SetPlayerVessel(int vessel)
        {
            //Debug.Log("Setting player vessel to: " + vessel);
            if (ServerObject != null)
                ServerData.SetPlayerVessel(vessel);
        }

        public static int GetPlayerVessel()
        {
            if (!ServerObject)
                return 0;

            return MapData.playerVessel;
        }

        public static void SetPlayerWorldVelocity(Vector3 velocity)
        {
            if (ServerObject != null)
                ServerData.SetPlayerWorldVelocity(velocity);
        }

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

        public static void SetVesselVis(int vessel, bool state)
        {
            ServerData.SetVesselVis(vessel, state);
        }

        public static void SetColorTheme(megColorTheme theme)
        {
            if (ServerObject == null)
                return;

            ServerObject.GetComponent<graphicsColourHolder>().themeName = theme.name;
            ServerObject.GetComponent<graphicsColourHolder>().backgroundColor = theme.backgroundColor;
            ServerObject.GetComponent<graphicsColourHolder>().highlightColor = theme.highlightColor;
            ServerObject.GetComponent<graphicsColourHolder>().keyColor = theme.keyColor;
        }

        public static megColorTheme GetColorTheme()
        {
            megColorTheme theme = new megColorTheme();
            if (ServerObject)
                theme = ServerObject.GetComponent<graphicsColourHolder>().theme;

            return theme;
        }

        public static int getGliderScreen(int screenID)
        {
            int outID = 0;

            if (ServerObject)
            {
                outID = ServerObject.GetComponent<glScreenData>().getScreen(screenID);
            }

            return outID;
        }

        public static string FormatLatitude(float value, int precision = 4)
            { return FormatDegreeMinuteSecond(Mathf.Abs(value), precision) + (value >= 0 ? "N" : "S"); }

        public static string FormatLongitude(float value, int precision = 4)
            { return FormatDegreeMinuteSecond(Mathf.Abs(value), precision) + (value >= 0 ? "E" : "W"); }

        public static string FormatDegreeMinuteSecond(float value, int precision = 4)
        {
            var degrees = Mathf.FloorToInt(value);
            var minutes = Mathf.FloorToInt((value * 60) % 60);
            var seconds = Mathf.Abs(value * 3600) % 60;

            return string.Format("{0}°{1}\'{2:N" + precision + "}\"", degrees, minutes, seconds);
        }

        public static bool GetGliderButtonState(int buttonID)
        {
            bool buttonState = false;

            switch (buttonID)
            {
                case 0:
                    buttonState = ServerObject.GetComponent<glScreenData>().mapElevation;
                    break;
                case 1:
                    buttonState = ServerObject.GetComponent<glScreenData>().recentreMap;
                    break;
                case 2:
                    buttonState = ServerObject.GetComponent<glScreenData>().objectLabelsOn;
                    break;
            }

            return buttonState;
        }

    }
}