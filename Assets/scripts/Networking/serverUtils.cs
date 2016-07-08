﻿using UnityEngine;
using System.Collections;

namespace Meg.Networking
{
    public class serverUtils : MonoBehaviour
    {
        public static float GetServerData(string valueName)
        {
            GameObject serverObject = GameObject.FindWithTag("ServerData");
            float rValue = 0f;
            if (serverObject != null)
            {
                switch (valueName)
                {
                    case "depth":
                        rValue = serverObject.GetComponent<serverData>().depth;
                        break;
                    case "xPos":
                        rValue = serverObject.transform.position.x;
                        break;
                    case "zPos":
                        rValue = serverObject.transform.position.z;
                        break;
                    case "pressure":
                        rValue = serverObject.GetComponent<serverData>().pressure;
                        break;
                    case "heading":
                        rValue = serverObject.GetComponent<serverData>().heading;
                        break;
                    case "pitchAngle":
                        rValue = serverObject.GetComponent<serverData>().pitchAngle;
                        break;
                    case "yawAngle":
                        rValue = serverObject.GetComponent<serverData>().yawAngle;
                        break;
                    case "rollAngle":
                        rValue = serverObject.GetComponent<serverData>().rollAngle;
                        break;
                    case "velocity":
                        rValue = serverObject.GetComponent<serverData>().velocity;
                        break;
                    case "floorDistance":
                        rValue = serverObject.GetComponent<serverData>().floorDistance;
                        break;
                    case "Co2":
                        rValue = serverObject.GetComponent<serverData>().Co2;
                        break;
                    case "waterTemp":
                        rValue = serverObject.GetComponent<serverData>().waterTemp;
                        break;
                    case "b1":
                        rValue = serverObject.GetComponent<serverData>().batteries[0];
                        break;
                    case "b2":
                        rValue = serverObject.GetComponent<serverData>().batteries[1];
                        break;
                    case "b3":
                        rValue = serverObject.GetComponent<serverData>().batteries[2];
                        break;
                    case "b4":
                        rValue = serverObject.GetComponent<serverData>().batteries[3];
                        break;
                    case "b5":
                        rValue = serverObject.GetComponent<serverData>().batteries[4];
                        break;
                    case "b6":
                        rValue = serverObject.GetComponent<serverData>().batteries[5];
                        break;
                    case "b7":
                        rValue = serverObject.GetComponent<serverData>().batteries[6];
                        break;
                    case "o1":
                        rValue = serverObject.GetComponent<serverData>().oxygenTanks[0];
                        break;
                    case "o2":
                        rValue = serverObject.GetComponent<serverData>().oxygenTanks[1];
                        break;
                    case "o3":
                        rValue = serverObject.GetComponent<serverData>().oxygenTanks[2];
                        break;
                    case "o4":
                        rValue = serverObject.GetComponent<serverData>().oxygenTanks[3];
                        break;
                    case "o5":
                        rValue = serverObject.GetComponent<serverData>().oxygenTanks[4];
                        break;
                    case "o6":
                        rValue = serverObject.GetComponent<serverData>().oxygenTanks[5];
                        break;
                    case "o7":
                        rValue = serverObject.GetComponent<serverData>().oxygenTanks[6];
                        break;
                    case "oxygen":
                        rValue = serverObject.GetComponent<serverData>().oxygen;
                        break;
                    case "battery":
                        rValue = serverObject.GetComponent<serverData>().battery;
                        break;
                    case "error_bilgeLeak":
                        rValue = serverObject.GetComponent<errorData>().error_bilgeLeak;
                        break;
                    case "error_batteryLeak":
                        rValue = serverObject.GetComponent<errorData>().error_batteryLeak;
                        break;
                    case "error_electricLeak":
                        rValue = serverObject.GetComponent<errorData>().error_electricLeak;
                        break;
                    case "error_oxygenExt":
                        rValue = serverObject.GetComponent<errorData>().error_oxygenExt;
                        break;
                    case "error_vhf":
                        rValue = serverObject.GetComponent<errorData>().error_vhf;
                        break;
                    case "error_forwardSonar":
                        rValue = serverObject.GetComponent<errorData>().error_forwardSonar;
                        break;
                    case "error_depthSonar":
                        rValue = serverObject.GetComponent<errorData>().error_depthSonar;
                        break;
                    case "error_doppler":
                        rValue = serverObject.GetComponent<errorData>().error_doppler;
                        break;
                    case "error_gps":
                        rValue = serverObject.GetComponent<errorData>().error_gps;
                        break;
                    case "error_cpu":
                        rValue = serverObject.GetComponent<errorData>().error_cpu;
                        break;
                    case "error_vidhd":
                        rValue = serverObject.GetComponent<errorData>().error_vidhd;
                        break;
                    case "error_datahd":
                        rValue = serverObject.GetComponent<errorData>().error_datahd;
                        break;
                    case "error_tow":
                        rValue = serverObject.GetComponent<errorData>().error_tow;
                        break;
                    case "error_radar":
                        rValue = serverObject.GetComponent<errorData>().error_radar;
                        break;
                    case "error_sternLights":
                        rValue = serverObject.GetComponent<errorData>().error_sternLights;
                        break;
                    case "error_bowLights":
                        rValue = serverObject.GetComponent<errorData>().error_bowLights;
                        break;
                    case "error_portLights":
                        rValue = serverObject.GetComponent<errorData>().error_portLights;
                        break;
                    case "error_bowThruster":
                        rValue = serverObject.GetComponent<errorData>().error_bowThruster;
                        break;
                    case "error_hyrdaulicRes":
                        rValue = serverObject.GetComponent<errorData>().error_hyrdaulicRes;
                        break;
                    case "error_starboardLights":
                        rValue = serverObject.GetComponent<errorData>().error_starboardLights;
                        break;
                    case "error_runningLights":
                        rValue = serverObject.GetComponent<errorData>().error_runningLights;
                        break;
                    case "error_ballastTank":
                        rValue = serverObject.GetComponent<errorData>().error_ballastTank;
                        break;
                    case "error_hydraulicPump":
                        rValue = serverObject.GetComponent<errorData>().error_hydraulicPump;
                        break;
                    case "error_oxygenPump":
                        rValue = serverObject.GetComponent<errorData>().error_oxygenPump;
                        break;
                    case "inputXaxis":
                        rValue = serverObject.GetComponent<serverData>().inputXaxis;
                        break;
                    case "inputYaxis":
                        rValue = serverObject.GetComponent<serverData>().inputYaxis;
                        break;
                    case "inputZaxis":
                        rValue = serverObject.GetComponent<serverData>().inputZaxis;
                        break;
                    case "inputXaxis2":
                        rValue = serverObject.GetComponent<serverData>().inputXaxis2;
                        break;
                    case "inputYaxis2":
                        rValue = serverObject.GetComponent<serverData>().inputYaxis2;
                        break;
                    case "verticalVelocity":
                        rValue = serverObject.GetComponent<serverData>().verticalVelocity;
                        break;
                    case "horizontalVelocity":
                        rValue = serverObject.GetComponent<serverData>().horizontalVelocity;
                        break;
                    case "dueTimeHours":
                        rValue = serverObject.GetComponent<serverData>().dueTimeHours;
                        break;
                    case "dueTimeMins":
                        rValue = serverObject.GetComponent<serverData>().dueTimeMins;
                        break;
                    case "dueTimeSecs":
                        rValue = serverObject.GetComponent<serverData>().dueTimeSecs;
                        break;
                    case "diveTimeHours":
                        rValue = serverObject.GetComponent<serverData>().diveTimeHours;
                        break;
                    case "diveTimeMins":
                        rValue = serverObject.GetComponent<serverData>().diveTimeMins;
                        break;
                    case "diveTimeSecs":
                        rValue = serverObject.GetComponent<serverData>().diveTimeSecs;
                        break;
                    case "crewHeartRate1":
                        rValue = serverObject.GetComponent<crewData>().crewHeartRate1;
                        break;
                    case "crewHeartRate2":
                        rValue = serverObject.GetComponent<crewData>().crewHeartRate2;
                        break;
                    case "crewHeartRate3":
                        rValue = serverObject.GetComponent<crewData>().crewHeartRate3;
                        break;
                    case "crewHeartRate4":
                        rValue = serverObject.GetComponent<crewData>().crewHeartRate4;
                        break;
                    case "crewHeartRate5":
                        rValue = serverObject.GetComponent<crewData>().crewHeartRate5;
                        break;
                    case "crewHeartRate6":
                        rValue = serverObject.GetComponent<crewData>().crewHeartRate6;
                        break;
                    case "crewBodyTemp1":
                        rValue = serverObject.GetComponent<crewData>().crewBodyTemp1;
                        break;
                    case "crewBodyTemp2":
                        rValue = serverObject.GetComponent<crewData>().crewBodyTemp2;
                        break;
                    case "crewBodyTemp3":
                        rValue = serverObject.GetComponent<crewData>().crewBodyTemp3;
                        break;
                    case "crewBodyTemp4":
                        rValue = serverObject.GetComponent<crewData>().crewBodyTemp4;
                        break;
                    case "crewBodyTemp5":
                        rValue = serverObject.GetComponent<crewData>().crewBodyTemp5;
                        break;
                    case "crewBodyTemp6":
                        rValue = serverObject.GetComponent<crewData>().crewBodyTemp6;
                        break;
                    case "v1posX":
                        rValue = serverObject.GetComponent<mapData>().vessel1Pos.x;
                        break;
                    case "v1posY":
                        rValue = serverObject.GetComponent<mapData>().vessel1Pos.y;
                        break;
                    case "v1posZ":
                        rValue = serverObject.GetComponent<mapData>().vessel1Pos.z;
                        break;
                    case "v2posX":
                        rValue = serverObject.GetComponent<mapData>().vessel2Pos.x;
                        break;
                    case "v2posY":
                        rValue = serverObject.GetComponent<mapData>().vessel2Pos.y;
                        break;
                    case "v2posZ":
                        rValue = serverObject.GetComponent<mapData>().vessel2Pos.z;
                        break;
                    case "v3posX":
                        rValue = serverObject.GetComponent<mapData>().vessel3Pos.x;
                        break;
                    case "v3posY":
                        rValue = serverObject.GetComponent<mapData>().vessel3Pos.y;
                        break;
                    case "v3posZ":
                        rValue = serverObject.GetComponent<mapData>().vessel3Pos.z;
                        break;
                    case "v4posX":
                        rValue = serverObject.GetComponent<mapData>().vessel4Pos.x;
                        break;
                    case "v4posY":
                        rValue = serverObject.GetComponent<mapData>().vessel4Pos.y;
                        break;
                    case "v4posZ":
                        rValue = serverObject.GetComponent<mapData>().vessel4Pos.z;
                        break;
                    case "meg1posX":
                        rValue = serverObject.GetComponent<mapData>().meg1Pos.x;
                        break;
                    case "meg1posY":
                        rValue = serverObject.GetComponent<mapData>().meg1Pos.y;
                        break;
                    case "meg1posZ":
                        rValue = serverObject.GetComponent<mapData>().meg1Pos.z;
                        break;
                    case "v1velocity":
                        rValue = serverObject.GetComponent<mapData>().vessel1Velocity;
                        break;
                    case "v2velocity":
                        rValue = serverObject.GetComponent<mapData>().vessel2Velocity;
                        break;
                    case "v3velocity":
                        rValue = serverObject.GetComponent<mapData>().vessel3Velocity;
                        break;
                    case "v4velocity":
                        rValue = serverObject.GetComponent<mapData>().vessel4Velocity;
                        break;
                    case "meg1velocity":
                        rValue = serverObject.GetComponent<mapData>().meg1Velocity;
                        break;
                    case "initiateMapEvent":
                        rValue = serverObject.GetComponent<mapData>().initiateMapEvent;
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
            GameObject serverObject = GameObject.FindWithTag("ServerData");
            string rValue = "no value";
            if (serverObject != null)
            {
                switch (valueName)
                {
                    case "depth":
                        int dInt = (int)serverObject.GetComponent<serverData>().depth;
                        rValue = dInt.ToString();
                        break;
                    case "pressure":
                        rValue = serverObject.GetComponent<serverData>().pressure.ToString();
                        break;
                    case "heading":
                        rValue = (serverObject.GetComponent<serverData>().heading.ToString("n1") + "°");
                        break;
                    case "pitchAngle":
                        rValue = (serverObject.GetComponent<serverData>().pitchAngle.ToString("n1") + "°");
                        break;
                    case "yawAngle":
                        rValue = (serverObject.GetComponent<serverData>().yawAngle.ToString("n1") + "°");
                        break;
                    case "rollAngle":
                        rValue = (serverObject.GetComponent<serverData>().rollAngle.ToString("n1") + "°");
                        break;
                    case "velocity":
                        rValue = serverObject.GetComponent<serverData>().velocity.ToString("n1");
                        break;
                    case "floorDistance":
                        int flInt = (int)serverObject.GetComponent<serverData>().floorDistance;
                        rValue = flInt.ToString();
                        break;
                    case "Co2":
                        rValue = serverObject.GetComponent<serverData>().Co2.ToString();
                        break;
                    case "waterTemp":
                        rValue = serverObject.GetComponent<serverData>().waterTemp.ToString();
                        break;
                    case "b1":
                        rValue = serverObject.GetComponent<serverData>().batteries[0].ToString("n1");
                        break;
                    case "b2":
                        rValue = serverObject.GetComponent<serverData>().batteries[1].ToString("n1");
                        break;
                    case "b3":
                        rValue = serverObject.GetComponent<serverData>().batteries[2].ToString("n1");
                        break;
                    case "b4":
                        rValue = serverObject.GetComponent<serverData>().batteries[3].ToString("n1");
                        break;
                    case "b5":
                        rValue = serverObject.GetComponent<serverData>().batteries[4].ToString("n1");
                        break;
                    case "b6":
                        rValue = serverObject.GetComponent<serverData>().batteries[5].ToString("n1");
                        break;
                    case "b7":
                        rValue = serverObject.GetComponent<serverData>().batteries[6].ToString("n1");
                        break;
                    case "o1":
                        rValue = serverObject.GetComponent<serverData>().oxygenTanks[0].ToString("n1");
                        break;
                    case "o2":
                        rValue = serverObject.GetComponent<serverData>().oxygenTanks[1].ToString("n1");
                        break;
                    case "o3":
                        rValue = serverObject.GetComponent<serverData>().oxygenTanks[2].ToString("n1");
                        break;
                    case "o4":
                        rValue = serverObject.GetComponent<serverData>().oxygenTanks[3].ToString("n1");
                        break;
                    case "o5":
                        rValue = serverObject.GetComponent<serverData>().oxygenTanks[4].ToString("n1");
                        break;
                    case "o6":
                        rValue = serverObject.GetComponent<serverData>().oxygenTanks[5].ToString("n1");
                        break;
                    case "o7":
                        rValue = serverObject.GetComponent<serverData>().oxygenTanks[6].ToString("n1");
                        break;
                    case "oxygen":
                        rValue = (serverObject.GetComponent<serverData>().oxygen.ToString("n1") + "%");
                        break;
                    case "battery":
                        rValue = (serverObject.GetComponent<serverData>().battery.ToString("n1") + "%");
                        break;
                    case "pilot":
                        rValue = serverObject.GetComponent<serverData>().pilot;
                        break;
                    case "verticalVelocity":
                        rValue = serverObject.GetComponent<serverData>().verticalVelocity.ToString("n1");
                        break;
                    case "horizontalVelocity":
                        rValue = serverObject.GetComponent<serverData>().horizontalVelocity.ToString("n1");
                        break;
                    case "dueTimeHours":
                        rValue = serverObject.GetComponent<serverData>().dueTimeHours.ToString();
                        break;
                    case "dueTimeMins":
                        rValue = serverObject.GetComponent<serverData>().dueTimeMins.ToString();
                        break;
                    case "dueTimeSecs":
                        rValue = serverObject.GetComponent<serverData>().dueTimeSecs.ToString();
                        break;
                    case "diveTimeHours":
                        rValue = serverObject.GetComponent<serverData>().diveTimeHours.ToString();
                        break;
                    case "diveTimeMins":
                        rValue = serverObject.GetComponent<serverData>().diveTimeMins.ToString();
                        break;
                    case "diveTimeSecs":
                        rValue = serverObject.GetComponent<serverData>().diveTimeSecs.ToString();
                        break;
                    case "inputXaxis":
                        rValue = serverObject.GetComponent<serverData>().inputXaxis.ToString("n1");
                        break;
                    case "inputYaxis":
                        rValue = serverObject.GetComponent<serverData>().inputYaxis.ToString("n1");
                        break;
                    case "inputZaxis":
                        rValue = serverObject.GetComponent<serverData>().inputZaxis.ToString("n1");
                        break;
                    case "inputXaxis2":
                        rValue = serverObject.GetComponent<serverData>().inputXaxis2.ToString("n1");
                        break;
                    case "inputYaxis2":
                        rValue = serverObject.GetComponent<serverData>().inputYaxis2.ToString("n1");
                        break;
                    case "crewHeartRate1":
                        rValue = serverObject.GetComponent<crewData>().crewHeartRate1.ToString("n1");
                        break;
                    case "crewHeartRate2":
                        rValue = serverObject.GetComponent<crewData>().crewHeartRate2.ToString("n1");
                        break;
                    case "crewHeartRate3":
                        rValue = serverObject.GetComponent<crewData>().crewHeartRate3.ToString("n1");
                        break;
                    case "crewHeartRate4":
                        rValue = serverObject.GetComponent<crewData>().crewHeartRate4.ToString("n1");
                        break;
                    case "crewHeartRate5":
                        rValue = serverObject.GetComponent<crewData>().crewHeartRate5.ToString("n1");
                        break;
                    case "crewHeartRate6":
                        rValue = serverObject.GetComponent<crewData>().crewHeartRate6.ToString("n1");
                        break;
                    case "crewBodyTemp1":
                        rValue = serverObject.GetComponent<crewData>().crewBodyTemp1.ToString("n1");
                        break;
                    case "crewBodyTemp2":
                        rValue = serverObject.GetComponent<crewData>().crewBodyTemp2.ToString("n1");
                        break;
                    case "crewBodyTemp3":
                        rValue = serverObject.GetComponent<crewData>().crewBodyTemp3.ToString("n1");
                        break;
                    case "crewBodyTemp4":
                        rValue = serverObject.GetComponent<crewData>().crewBodyTemp4.ToString("n1");
                        break;
                    case "crewBodyTemp5":
                        rValue = serverObject.GetComponent<crewData>().crewBodyTemp5.ToString("n1");
                        break;
                    case "crewBodyTemp6":
                        rValue = serverObject.GetComponent<crewData>().crewBodyTemp6.ToString("n1");
                        break;
                    case "v1depth":
                        rValue = serverObject.GetComponent<mapData>().vessel1Pos.z.ToString("n0");
                        break;
                    case "v2depth":
                        rValue = serverObject.GetComponent<mapData>().vessel2Pos.z.ToString("n0");
                        break;
                    case "v3depth":
                        rValue = serverObject.GetComponent<mapData>().vessel3Pos.z.ToString("n0");
                        break;
                    case "v4depth":
                        rValue = serverObject.GetComponent<mapData>().vessel4Pos.z.ToString("n0");
                        break;
                    case "meg1depth":
                        rValue = serverObject.GetComponent<mapData>().meg1Pos.z.ToString("n0");
                        break;
                    case "v1velocity":
                        rValue = serverObject.GetComponent<mapData>().vessel1Velocity.ToString("n1");
                        break;
                    case "v2velocity":
                        rValue = serverObject.GetComponent<mapData>().vessel2Velocity.ToString("n1");
                        break;
                    case "v3velocity":
                        rValue = serverObject.GetComponent<mapData>().vessel3Velocity.ToString("n1");
                        break;
                    case "v4velocity":
                        rValue = serverObject.GetComponent<mapData>().vessel4Velocity.ToString("n1");
                        break;
                    case "meg1velocity":
                        rValue = serverObject.GetComponent<mapData>().meg1Velocity.ToString("n1");
                        break;
                    case "mapEventName":
                        rValue = serverObject.GetComponent<mapData>().mapEventName;
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
            GameObject serverObject = GameObject.FindWithTag("ServerData");

            serverObject.GetComponent<serverData>().OnChangeBool(boolName, value);
        }

        public static void SetServerData(string valueName, float value)
        {
            GameObject serverObject = GameObject.FindWithTag("ServerData");

            serverObject.GetComponent<serverData>().OnValueChanged(valueName, value);
        }

        public static void SetBatteryData(int bank, float value)
        {
            GameObject serverObject = GameObject.FindWithTag("ServerData");

            serverObject.GetComponent<serverData>().OnBatterySliderChanged(bank, value);
        }

        public static void SetOxygenData(int bank, float value)
        {
            GameObject serverObject = GameObject.FindWithTag("ServerData");

            serverObject.GetComponent<serverData>().OnOxygenSliderChanged(bank, value);
        }

        public static void SetVesselData(int vessel, Vector3 pos, float vesselVelocity)
        {
            GameObject serverObject = GameObject.FindWithTag("ServerData");

            serverObject.GetComponent<serverData>().OnVesselDataChanged(vessel, pos, vesselVelocity);
        }

        public static void SetPlayerVessel(int vessel)
        {
            GameObject serverObject = GameObject.FindWithTag("ServerData");

            Debug.Log("Setting player vessel to: " + vessel);
            serverObject.GetComponent<serverData>().SetPlayerVessel(vessel);
        }

        public static int GetPlayerVessel()
        {
            GameObject serverObject = GameObject.FindWithTag("ServerData");

            return serverObject.GetComponent<mapData>().playerVessel;
        }

        public static bool GetVesselVis(int vessel)
        {
            GameObject serverObject = GameObject.FindWithTag("ServerData");

            bool vesselVis = true;

            switch (vessel)
            {
                case 1:
                    vesselVis = serverObject.GetComponent<mapData>().vessel1Vis;
                    break;
                case 2:
                    vesselVis = serverObject.GetComponent<mapData>().vessel2Vis;
                    break;
                case 3:
                    vesselVis = serverObject.GetComponent<mapData>().vessel3Vis;
                    break;
                case 4:
                    vesselVis = serverObject.GetComponent<mapData>().vessel4Vis;
                    break;
                case 5:
                    vesselVis = serverObject.GetComponent<mapData>().meg1Vis;
                    break;
            }

            return vesselVis;
        }

        public static void SetVesselVis(int vessel, bool state)
        {
            GameObject serverObject = GameObject.FindWithTag("ServerData");

            serverObject.GetComponent<serverData>().SetPlayerVisibility(vessel, state);
        }
    }
}