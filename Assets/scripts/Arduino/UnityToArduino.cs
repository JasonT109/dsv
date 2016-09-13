using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
using Meg.Networking;

public class UnityToArduino : Singleton<UnityToArduino>
{
    /** Interval between port connection attempts. */
    private const float PortUpdateInterval = 1;

    /** Timeout for retrying ports that fail to open. */
    private const float PortTimeoutInterval = 10;

    private SerialPort port;
    public GameObject MotionBaseTester;

    private Quaternion motionBase;

    private Vector3 LastMove;

    private Vector3 flat;
    private Quaternion flatQ;

    public Vector3 preMapped;

    public float slerpNerf;

    private float _nextPortUpdate;

    private static MotionBaseData MotionData
        { get { return serverUtils.MotionBaseData; } }

    private static serverData Server
        { get { return serverUtils.ServerData; } }

    private static SubControl Controls
        { get { return serverUtils.SubControl; } }


    // initialization
    void Start()
    {
        UpdateComPort();

        flat = new Vector3(0, 0, 0);
        flatQ = Quaternion.Euler(flat);

        StartCoroutine(UpdateMotionBaseRoutine());
    }

    void Update()
    {
        if (Time.time < _nextPortUpdate)
            return;

        UpdateComPort();
        _nextPortUpdate = Time.time + PortUpdateInterval;
    }

    IEnumerator UpdateMotionBaseRoutine()
    {
        // Wait until server is started up.
        while (!serverUtils.IsServer())
            yield return 0;

        // Motion base update loop.
        while (gameObject.activeSelf)
        {
            HazardCheck();

            if (MotionData.MotionSafety)
            {
                if (!MotionData.MotionHazard)
                {
                    if (serverUtils.IsGlider())
                    {
                        SlerpWithRemap();

                    }
                    else
                    {
                        motionBase = Quaternion.Slerp(motionBase, Controls.GetMotionBaseQuaternion(), Time.deltaTime * MotionData.MotionSlerpSpeed);
                    }
                }
                else
                {
                    motionBase = Quaternion.Slerp(motionBase, flatQ, Time.deltaTime * 0.5f);
                }

                MotionData.MotionBasePitch = motionBase.eulerAngles.x;
                MotionData.MotionBaseYaw = motionBase.eulerAngles.y;
                MotionData.MotionBaseRoll = motionBase.eulerAngles.z;

                if (MotionData.MotionBasePitch > 180f)
                {
                    MotionData.MotionBasePitch = motionBase.eulerAngles.x - 360f;
                }

                if (MotionData.MotionBaseRoll > 180f)
                {
                    MotionData.MotionBaseRoll = motionBase.eulerAngles.z - 360f;
                }
            }
            else
            {
                MotionData.MotionBasePitch = Server.pitchAngle;
                MotionData.MotionBaseYaw = Server.yawAngle;
                MotionData.MotionBaseRoll = Server.rollAngle;
            }

            if (MotionData.MotionHazard && !MotionData.MotionSafety)
            {
                motionBase = Quaternion.Slerp(motionBase, flatQ, Time.deltaTime * 0.5f);
            }

            if (MotionData.MotionBaseRoll > MotionData.MotionRollMax)
            {
                MotionData.MotionBaseRoll = MotionData.MotionRollMax;
            }

            if (MotionData.MotionBaseRoll < MotionData.MotionRollMin)
            {
                MotionData.MotionBaseRoll = MotionData.MotionRollMin;
            }

            if (MotionData.MotionBasePitch > MotionData.MotionPitchMax)
            {
                MotionData.MotionBasePitch = MotionData.MotionPitchMax;
            }

            if (MotionData.MotionBasePitch < MotionData.MotionPitchMin)
            {
                MotionData.MotionBasePitch = MotionData.MotionPitchMin;
            }

            if (MotionData.MotionBaseYaw > 180)
            {
                MotionData.MotionBaseYaw -= 360f;
            }

            if (MotionData.MotionBaseYaw > MotionData.MotionYawMax)
            {
                MotionData.MotionBaseYaw = MotionData.MotionYawMax;
            }

            if (MotionData.MotionBaseYaw < -MotionData.MotionYawMax)
            {
                MotionData.MotionBaseYaw = -MotionData.MotionYawMax;
            }

            if (MotionBaseTester)
            {
                Quaternion MotionBaseTestQ;
                MotionBaseTestQ = Quaternion.Euler(new Vector3(MotionData.MotionBasePitch, MotionData.MotionBaseYaw, MotionData.MotionBaseRoll));
                MotionBaseTester.transform.rotation = MotionBaseTestQ;
            }

            if (port != null && port.IsOpen)
            {
                try
                {
                    port.Write(String.Format("${0},{1},{2},{3},{4},{5}\0",
                        (MotionData.MotionBaseYaw.ToString("F3")),
                        (MotionData.MotionBasePitch.ToString("F3")),
                        (MotionData.MotionBaseRoll.ToString("F3")),

                        (Controls.inputXaxis.ToString("F3")),
                        (Controls.inputYaxis.ToString("F3")),
                        (Controls.inputZaxis.ToString("F3")))
                        );
                }
                catch (Exception)
                {
                    Debug.LogWarning("Failed to write to COM port - closing the port.");
                    if (port != null)
                        port.Close();

                    port = null;
                }
            }


            //yield return new WaitForSeconds(0.016f);
            yield return new WaitForSeconds(0.0083f);
        }
    }

    void OnDestroy()
    {
        try
        {
            if (port != null)
                port.Close();
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e);
        }
    }

    private void HazardCheck()
    {
        if (Mathf.Abs(LastMove.x - MotionData.MotionBasePitch) > MotionData.MotionHazardSensitivity)
        {
            MotionData.MotionHazard = true;
        }

        if (Mathf.Abs(LastMove.z - MotionData.MotionBaseRoll) > MotionData.MotionHazardSensitivity)
        {
            MotionData.MotionHazard = true;
            Debug.Log("Hazard: Too much movement");
        }

        LastMove.x = MotionData.MotionBasePitch;
        LastMove.y = MotionData.MotionBaseYaw;
        LastMove.z = MotionData.MotionBaseRoll;
    }

    private void SlerpWithRemap()
    {
        Controls.CalculateYawVelocity();

        MotionData.MotionBaseYaw *= MotionData.MotionYawMax;

        preMapped = Controls.GetMotionBaseQuaternion().eulerAngles;

        if (preMapped.x > 180f)
        {
            preMapped.x -= 360f;
        }

        if (preMapped.z > 180f)
        {
            preMapped.z -= 360f;
        }

        preMapped.x /= Controls.MaxGliderAngle;
        preMapped.z /= Controls.MaxGliderAngle;

        //if(preMapped.x > 0)
        //{
        //    preMapped.x *= (MotionData.MotionPitchMax);
        //}
        //else
        //{
        //    preMapped.x *= (Mathf.Abs(MotionData.MotionPitchMin));
        //}
        //
        //if (preMapped.z > 0)
        //{
        //    preMapped.z *= (MotionData.MotionRollMax);
        //}
        //else
        //{
        //    preMapped.z *= (Mathf.Abs(MotionData.MotionRollMin));
        //}
        var maxAngle = Mathf.Max(serverUtils.MotionBaseData.MotionPitchMax,
            Mathf.Abs(serverUtils.MotionBaseData.MotionPitchMin),
            serverUtils.MotionBaseData.MotionRollMax,
            Mathf.Abs(serverUtils.MotionBaseData.MotionRollMin));

        preMapped.x *= (maxAngle);
        preMapped.z *= (maxAngle);

        Quaternion reMapped;
        reMapped = Quaternion.Euler(preMapped.x, MotionData.MotionBaseYaw, preMapped.z);

        //lerp the slerp
        //float lerpSlerp1;

        float angle = Quaternion.Angle(reMapped, motionBase);

        slerpNerf = angle / 30f; //Mathf.Clamp(angle / 30f, 0f, 1f);
        //slerpNerf = Mathf.Clamp(angle / 30f, 0.5f, 20f);
        //slerpNerf = 20f - slerpNerf +20f;

        motionBase = Quaternion.Slerp(motionBase, reMapped, Time.deltaTime * (MotionData.MotionSlerpSpeed / (slerpNerf + 0.1f)));
    }


    /** Update the current serial port based on settings. */
    private void UpdateComPort()
    {
        // Only connect to a COM port on the host.
        if (!serverUtils.IsServer())
            return;

        // Determine which port we want to connect to.
        var portId = serverUtils.MotionBaseData.MotionComPort;
        var portName = portId > 0 ? string.Format("COM{0}",
            serverUtils.MotionBaseData.MotionComPort) : "";

        // Close old port if it's out of date.
        if (port != null && port.PortName != portName)
        {
            port.Close();
            port = null;
        }

        // Open a fresh port if needed.
        if (port == null && !string.IsNullOrEmpty(portName))
        {
            try
            {
                port = new SerialPort(portName, 115200);
                port.Open();
                StartCoroutine(PortTimeoutRoutine(portName));
            }
            catch (Exception)
            {
                Debug.LogWarning("Failed to open COM port: " + portName);
                if (port != null)
                    port.Close();

                port = null;
            }
        }

        // Update COM port status.
        serverUtils.MotionBaseData.MotionComPortOpen = port != null && port.IsOpen;
    }

    /** Routine to check for and clean up timed-out serial ports. */
    private IEnumerator PortTimeoutRoutine(string portName)
    {
        // Wait a little while, then clean up port if it failed to open.
        yield return new WaitForSeconds(PortTimeoutInterval);
        if (port == null || port.IsOpen || port.PortName != portName)
            yield break;

        port = null;
    }

}