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



        // Static Methods
        // ------------------------------------------------------------

        public static bool IsReady()
        {
            return ServerObject != null;
        }

        public static float GetServerData(string valueName)
        {
            float rValue = 0f;
            if (ServerObject != null)
            {
                switch (valueName)
                {
                    case "depth":
                        rValue = ServerData.depth;
                        break;
                    case "xPos":
                        rValue = ServerObject.transform.position.x;
                        break;
                    case "zPos":
                        rValue = ServerObject.transform.position.z;
                        break;
                    case "pressure":
                        rValue = ServerData.pressure;
                        break;
                    case "cabinPressure":
                        rValue = ServerData.cabinPressure;
                        break;
                    case "heading":
                        rValue = ServerData.heading;
                        break;
                    case "pitchAngle":
                        rValue = ServerData.pitchAngle;
                        break;
                    case "yawAngle":
                        rValue = ServerData.yawAngle;
                        break;
                    case "rollAngle":
                        rValue = ServerData.rollAngle;
                        break;
                    case "velocity":
                        rValue = ServerData.velocity;
                        break;
                    case "floorDistance":
                        rValue = ServerData.floorDistance;
                        break;
                    case "diveTime":
                        rValue = ServerData.diveTime;
                        break;
                    case "dueTime":
                        rValue = ServerData.dueTime;
                        break;
                    case "Co2":
                        rValue = ServerData.Co2;
                        break;
                    case "waterTemp":
                        rValue = ServerData.waterTemp;
                        break;
                    case "cabinTemp":
                        rValue = ServerData.cabinTemp;
                        break;
                    case "b1":
                        rValue = ServerData.batteries[0];
                        break;
                    case "b2":
                        rValue = ServerData.batteries[1];
                        break;
                    case "b3":
                        rValue = ServerData.batteries[2];
                        break;
                    case "b4":
                        rValue = ServerData.batteries[3];
                        break;
                    case "b5":
                        rValue = ServerData.batteries[4];
                        break;
                    case "b6":
                        rValue = ServerData.batteries[5];
                        break;
                    case "b7":
                        rValue = ServerData.batteries[6];
                        break;
                    case "o1":
                        rValue = ServerData.oxygenTanks[0];
                        break;
                    case "o2":
                        rValue = ServerData.oxygenTanks[1];
                        break;
                    case "o3":
                        rValue = ServerData.oxygenTanks[2];
                        break;
                    case "o4":
                        rValue = ServerData.oxygenTanks[3];
                        break;
                    case "o5":
                        rValue = ServerData.oxygenTanks[4];
                        break;
                    case "o6":
                        rValue = ServerData.oxygenTanks[5];
                        break;
                    case "o7":
                        rValue = ServerData.oxygenTanks[6];
                        break;
                    case "oxygen":
                        rValue = ServerData.oxygen;
                        break;
                    case "battery":
                        rValue = ServerData.battery;
                        break;
                    case "error_bilgeLeak":
                        rValue = ErrorData.error_bilgeLeak;
                        break;
                    case "error_batteryLeak":
                        rValue = ErrorData.error_batteryLeak;
                        break;
                    case "error_electricLeak":
                        rValue = ErrorData.error_electricLeak;
                        break;
                    case "error_oxygenExt":
                        rValue = ErrorData.error_oxygenExt;
                        break;
                    case "error_vhf":
                        rValue = ErrorData.error_vhf;
                        break;
                    case "error_forwardSonar":
                        rValue = ErrorData.error_forwardSonar;
                        break;
                    case "error_depthSonar":
                        rValue = ErrorData.error_depthSonar;
                        break;
                    case "error_doppler":
                        rValue = ErrorData.error_doppler;
                        break;
                    case "error_gps":
                        rValue = ErrorData.error_gps;
                        break;
                    case "error_cpu":
                        rValue = ErrorData.error_cpu;
                        break;
                    case "error_vidhd":
                        rValue = ErrorData.error_vidhd;
                        break;
                    case "error_datahd":
                        rValue = ErrorData.error_datahd;
                        break;
                    case "error_tow":
                        rValue = ErrorData.error_tow;
                        break;
                    case "error_radar":
                        rValue = ErrorData.error_radar;
                        break;
                    case "error_sternLights":
                        rValue = ErrorData.error_sternLights;
                        break;
                    case "error_bowLights":
                        rValue = ErrorData.error_bowLights;
                        break;
                    case "error_portLights":
                        rValue = ErrorData.error_portLights;
                        break;
                    case "error_bowThruster":
                        rValue = ErrorData.error_bowThruster;
                        break;
                    case "error_hyrdaulicRes":
                        rValue = ErrorData.error_hyrdaulicRes;
                        break;
                    case "error_starboardLights":
                        rValue = ErrorData.error_starboardLights;
                        break;
                    case "error_runningLights":
                        rValue = ErrorData.error_runningLights;
                        break;
                    case "error_ballastTank":
                        rValue = ErrorData.error_ballastTank;
                        break;
                    case "error_hydraulicPump":
                        rValue = ErrorData.error_hydraulicPump;
                        break;
                    case "error_oxygenPump":
                        rValue = ErrorData.error_oxygenPump;
                        break;
                    case "inputXaxis":
                        rValue = ServerData.inputXaxis;
                        break;
                    case "inputYaxis":
                        rValue = ServerData.inputYaxis;
                        break;
                    case "inputZaxis":
                        rValue = ServerData.inputZaxis;
                        break;
                    case "inputXaxis2":
                        rValue = ServerData.inputXaxis2;
                        break;
                    case "inputYaxis2":
                        rValue = ServerData.inputYaxis2;
                        break;
                    case "verticalVelocity":
                        rValue = ServerData.verticalVelocity;
                        break;
                    case "horizontalVelocity":
                        rValue = ServerData.horizontalVelocity;
                        break;
                    case "commsSignalStrength":
                        rValue = ServerData.commsSignalStrength;
                        break;
                    case "divertPowerToThrusters":
                        rValue = ServerData.divertPowerToThrusters;
                        break;
                    case "crewHeartRate1":
                        rValue = CrewData.crewHeartRate1;
                        break;
                    case "crewHeartRate2":
                        rValue = CrewData.crewHeartRate2;
                        break;
                    case "crewHeartRate3":
                        rValue = CrewData.crewHeartRate3;
                        break;
                    case "crewHeartRate4":
                        rValue = CrewData.crewHeartRate4;
                        break;
                    case "crewHeartRate5":
                        rValue = CrewData.crewHeartRate5;
                        break;
                    case "crewHeartRate6":
                        rValue = CrewData.crewHeartRate6;
                        break;
                    case "crewBodyTemp1":
                        rValue = CrewData.crewBodyTemp1;
                        break;
                    case "crewBodyTemp2":
                        rValue = CrewData.crewBodyTemp2;
                        break;
                    case "crewBodyTemp3":
                        rValue = CrewData.crewBodyTemp3;
                        break;
                    case "crewBodyTemp4":
                        rValue = CrewData.crewBodyTemp4;
                        break;
                    case "crewBodyTemp5":
                        rValue = CrewData.crewBodyTemp5;
                        break;
                    case "crewBodyTemp6":
                        rValue = CrewData.crewBodyTemp6;
                        break;
                    case "v1posX":
                        rValue = MapData.vessel1Pos.x;
                        break;
                    case "v1posY":
                        rValue = MapData.vessel1Pos.y;
                        break;
                    case "v1posZ":
                        rValue = MapData.vessel1Pos.z;
                        break;
                    case "v2posX":
                        rValue = MapData.vessel2Pos.x;
                        break;
                    case "v2posY":
                        rValue = MapData.vessel2Pos.y;
                        break;
                    case "v2posZ":
                        rValue = MapData.vessel2Pos.z;
                        break;
                    case "v3posX":
                        rValue = MapData.vessel3Pos.x;
                        break;
                    case "v3posY":
                        rValue = MapData.vessel3Pos.y;
                        break;
                    case "v3posZ":
                        rValue = MapData.vessel3Pos.z;
                        break;
                    case "v4posX":
                        rValue = MapData.vessel4Pos.x;
                        break;
                    case "v4posY":
                        rValue = MapData.vessel4Pos.y;
                        break;
                    case "v4posZ":
                        rValue = MapData.vessel4Pos.z;
                        break;
                    case "meg1posX":
                        rValue = MapData.meg1Pos.x;
                        break;
                    case "meg1posY":
                        rValue = MapData.meg1Pos.y;
                        break;
                    case "meg1posZ":
                        rValue = MapData.meg1Pos.z;
                        break;
                    case "intercept1posX":
                        rValue = MapData.intercept1Pos.x;
                        break;
                    case "intercept1posY":
                        rValue = MapData.intercept1Pos.y;
                        break;
                    case "intercept1posZ":
                        rValue = MapData.intercept1Pos.z;
                        break;
                    case "v1velocity":
                        rValue = MapData.vessel1Velocity;
                        break;
                    case "v2velocity":
                        rValue = MapData.vessel2Velocity;
                        break;
                    case "v3velocity":
                        rValue = MapData.vessel3Velocity;
                        break;
                    case "v4velocity":
                        rValue = MapData.vessel4Velocity;
                        break;
                    case "meg1velocity":
                        rValue = MapData.meg1Velocity;
                        break;
                    case "intercept1velocity":
                        rValue = MapData.intercept1Velocity;
                        break;
                    case "initiateMapEvent":
                        rValue = MapData.initiateMapEvent;
                        break;
                    case "latitude":
                        rValue = MapData.latitude;
                        break;
                    case "longitude":
                        rValue = MapData.longitude;
                        break;
                    default:
                        rValue = 50;
                        break;
                }
            }
            return rValue;
        }

        public static string GetServerDataAsText(string valueName)
        {
            string rValue = "no value";
            if (ServerObject != null)
            {
                switch (valueName)
                {
                    case "depth":
                        int dInt = (int)ServerData.depth;
                        rValue = dInt.ToString();
                        break;
                    case "pressure":
                        rValue = ServerData.pressure.ToString();
                        break;
                    case "cabinPressure":
                        rValue = ServerData.cabinPressure.ToString();
                        break;
                    case "heading":
                        rValue = (ServerData.heading.ToString("n1") + "°");
                        break;
                    case "pitchAngle":
                        rValue = (ServerData.pitchAngle.ToString("n1") + "°");
                        break;
                    case "yawAngle":
                        rValue = (ServerData.yawAngle.ToString("n1") + "°");
                        break;
                    case "rollAngle":
                        rValue = (ServerData.rollAngle.ToString("n1") + "°");
                        break;
                    case "velocity":
                        rValue = ServerData.velocity.ToString("n1");
                        break;
                    case "floorDistance":
                        int flInt = (int)ServerData.floorDistance;
                        rValue = flInt.ToString();
                        break;
                    case "diveTime":
                        var diveSpan = TimeSpan.FromSeconds(ServerData.diveTime);
                        rValue = string.Format("{0:00}:{1:00}:{2:00}", diveSpan.Hours, diveSpan.Minutes, diveSpan.Seconds);
                        break;
                    case "dueTime":
                        var dueSpan = TimeSpan.FromSeconds(ServerData.dueTime);
                        rValue = string.Format("{0:00}:{1:00}:{2:00}", dueSpan.Hours, dueSpan.Minutes, dueSpan.Seconds);
                        break;
                    case "Co2":
                        rValue = (ServerData.Co2.ToString() + "%");
                        break;
                    case "waterTemp":
                        rValue = ServerData.waterTemp.ToString();
                        break;
                    case "cabinTemp":
                        rValue = ServerData.cabinTemp.ToString();
                        break;
                    case "b1":
                        rValue = ServerData.batteries[0].ToString("n1");
                        break;
                    case "b2":
                        rValue = ServerData.batteries[1].ToString("n1");
                        break;
                    case "b3":
                        rValue = ServerData.batteries[2].ToString("n1");
                        break;
                    case "b4":
                        rValue = ServerData.batteries[3].ToString("n1");
                        break;
                    case "b5":
                        rValue = ServerData.batteries[4].ToString("n1");
                        break;
                    case "b6":
                        rValue = ServerData.batteries[5].ToString("n1");
                        break;
                    case "b7":
                        rValue = ServerData.batteries[6].ToString("n1");
                        break;
                    case "o1":
                        rValue = ServerData.oxygenTanks[0].ToString("n1");
                        break;
                    case "o2":
                        rValue = ServerData.oxygenTanks[1].ToString("n1");
                        break;
                    case "o3":
                        rValue = ServerData.oxygenTanks[2].ToString("n1");
                        break;
                    case "o4":
                        rValue = ServerData.oxygenTanks[3].ToString("n1");
                        break;
                    case "o5":
                        rValue = ServerData.oxygenTanks[4].ToString("n1");
                        break;
                    case "o6":
                        rValue = ServerData.oxygenTanks[5].ToString("n1");
                        break;
                    case "o7":
                        rValue = ServerData.oxygenTanks[6].ToString("n1");
                        break;
                    case "oxygen":
                        rValue = (ServerData.oxygen.ToString("n1") + "%");
                        break;
                    case "battery":
                        rValue = (ServerData.battery.ToString("n1") + "%");
                        break;
                    case "pilot":
                        rValue = ServerData.pilot;
                        break;
                    case "verticalVelocity":
                        rValue = ServerData.verticalVelocity.ToString("n1");
                        break;
                    case "horizontalVelocity":
                        rValue = ServerData.horizontalVelocity.ToString("n1");
                        break;
                    case "inputXaxis":
                        rValue = ServerData.inputXaxis.ToString("n1");
                        break;
                    case "inputYaxis":
                        rValue = ServerData.inputYaxis.ToString("n1");
                        break;
                    case "inputZaxis":
                        rValue = ServerData.inputZaxis.ToString("n1");
                        break;
                    case "inputXaxis2":
                        rValue = ServerData.inputXaxis2.ToString("n1");
                        break;
                    case "inputYaxis2":
                        rValue = ServerData.inputYaxis2.ToString("n1");
                        break;
                    case "commsSignalStrength":
                        rValue = ServerData.commsSignalStrength.ToString("n1");
                        break;
                    case "divertPowerToThrusters":
                        rValue = ServerData.divertPowerToThrusters.ToString("n1");
                        break;
                    case "crewHeartRate1":
                        rValue = CrewData.crewHeartRate1.ToString("n1");
                        break;
                    case "crewHeartRate2":
                        rValue = CrewData.crewHeartRate2.ToString("n1");
                        break;
                    case "crewHeartRate3":
                        rValue = CrewData.crewHeartRate3.ToString("n1");
                        break;
                    case "crewHeartRate4":
                        rValue = CrewData.crewHeartRate4.ToString("n1");
                        break;
                    case "crewHeartRate5":
                        rValue = CrewData.crewHeartRate5.ToString("n1");
                        break;
                    case "crewHeartRate6":
                        rValue = CrewData.crewHeartRate6.ToString("n1");
                        break;
                    case "crewBodyTemp1":
                        rValue = CrewData.crewBodyTemp1.ToString("n1");
                        break;
                    case "crewBodyTemp2":
                        rValue = CrewData.crewBodyTemp2.ToString("n1");
                        break;
                    case "crewBodyTemp3":
                        rValue = CrewData.crewBodyTemp3.ToString("n1");
                        break;
                    case "crewBodyTemp4":
                        rValue = CrewData.crewBodyTemp4.ToString("n1");
                        break;
                    case "crewBodyTemp5":
                        rValue = CrewData.crewBodyTemp5.ToString("n1");
                        break;
                    case "crewBodyTemp6":
                        rValue = CrewData.crewBodyTemp6.ToString("n1");
                        break;
                    case "v1depth":
                        rValue = MapData.vessel1Pos.z.ToString("n0");
                        break;
                    case "v2depth":
                        rValue = MapData.vessel2Pos.z.ToString("n0");
                        break;
                    case "v3depth":
                        rValue = MapData.vessel3Pos.z.ToString("n0");
                        break;
                    case "v4depth":
                        rValue = MapData.vessel4Pos.z.ToString("n0");
                        break;
                    case "meg1depth":
                        rValue = MapData.meg1Pos.z.ToString("n0");
                        break;
                    case "intercept1depth":
                        rValue = MapData.intercept1Pos.z.ToString("n0");
                        break;
                    case "v1velocity":
                        rValue = MapData.vessel1Velocity.ToString("n1");
                        break;
                    case "v2velocity":
                        rValue = MapData.vessel2Velocity.ToString("n1");
                        break;
                    case "v3velocity":
                        rValue = MapData.vessel3Velocity.ToString("n1");
                        break;
                    case "v4velocity":
                        rValue = MapData.vessel4Velocity.ToString("n1");
                        break;
                    case "meg1velocity":
                        rValue = MapData.meg1Velocity.ToString("n1");
                        break;
                    case "intercept1velocity":
                        rValue = MapData.intercept1Velocity.ToString("n1");
                        break;
                    case "mapEventName":
                        rValue = MapData.mapEventName;
                        break;
                    case "latitude":
                        rValue = FormatLatitude(MapData.latitude);
                        break;
                    case "longitude":
                        rValue = FormatLongitude(MapData.longitude);
                        break;
                    default:
                        rValue = "no value";
                        break;
                }
            }
            return rValue;
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
            ServerData.SetPlayerVisibility(vessel, state);
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

    }


}