using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
using Meg.Networking;

public class UnityToArduino : MonoBehaviour 
{
	private SerialPort port;
	public serverData Server;
	public SubControl Controls;
    public MotionBaseData MotionData;
    public GameObject MotionBaseTester;
	public string COMPort = "";

	private Quaternion motionBase;

	ArduinoManager Settings;

	private Vector3 LastMove;

	private Vector3 flat;
	private Quaternion flatQ;

    public Vector3 preMapped;

    public float slerpNerf;

	// initialization
	void Start()
	{ 
		if(GameObject.FindGameObjectWithTag("ArduinoManager"))
		{
			if(GameObject.FindGameObjectWithTag("ArduinoManager").GetComponent<ArduinoManager>())
			{
				Settings = GameObject.FindGameObjectWithTag("ArduinoManager").GetComponent<ArduinoManager>();
				COMPort = Settings.ComPort;
			
				port = new SerialPort(COMPort, 115200);
				if (!port.IsOpen)
				{
					port.Open();
					StartCoroutine(SendData());
				}
			}
		}
			
		LastMove.x = MotionData.MotionBasePitch;
		LastMove.y = MotionData.MotionBaseYaw;
		LastMove.z = MotionData.MotionBaseRoll;

		flat = new Vector3(0,0,0);
		flatQ = Quaternion.Euler(flat);

	}

	void Awake()
	{
		//if(GameObject.FindGameObjectWithTag("ArduinoManager").GetComponent<ArduinoManager>())
		//{
		//	Settings = GameObject.FindGameObjectWithTag("ArduinoManager").GetComponent<ArduinoManager>();
		//	COMPort = Settings.ComPort;
		//}

		LastMove.x = MotionData.MotionBasePitch;
		LastMove.y = MotionData.MotionBaseYaw;
		LastMove.z = MotionData.MotionBaseRoll;
	}

	IEnumerator SendData()
	{
		while (port.IsOpen)
		{
			HazardCheck();

			if(MotionData.MotionSafety)
			{
				if(!MotionData.MotionHazard)
				{
                    if (serverUtils.IsGlider())
                    {
                        SlerpWithRemap();

                    }
                    else
                    {
                        motionBase = Quaternion.Slerp(motionBase, Server.transform.rotation, Time.deltaTime * MotionData.MotionSlerpSpeed);
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
                    MotionData.MotionBasePitch = motionBase.eulerAngles.x-360f;
				}

				if(MotionData.MotionBaseRoll > 180f)
				{
                    MotionData.MotionBaseRoll = motionBase.eulerAngles.z-360f;
				}
			}
			else
			{
                MotionData.MotionBasePitch = Server.pitchAngle;
                MotionData.MotionBaseYaw = Server.yawAngle;
                MotionData.MotionBaseRoll = Server.rollAngle;
			}

			if(MotionData.MotionHazard && !MotionData.MotionSafety)
			{
				motionBase = Quaternion.Slerp(motionBase, flatQ, Time.deltaTime*0.5f);
			}

			if(MotionData.MotionBaseRoll > MotionData.MotionRollMax)
			{
                MotionData.MotionBaseRoll = MotionData.MotionRollMax;
			}

			if(MotionData.MotionBaseRoll < -MotionData.MotionRollMax)
			{
                MotionData.MotionBaseRoll = -MotionData.MotionRollMax;
			}

			if(MotionData.MotionBasePitch > MotionData.MotionPitchMax)
			{
                MotionData.MotionBasePitch = MotionData.MotionPitchMax;
			}

			if(MotionData.MotionBasePitch < -MotionData.MotionPitchMax)
			{
                MotionData.MotionBasePitch = -MotionData.MotionPitchMax;
			}

            if(MotionData.MotionBaseYaw > 180)
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


            //if(!Controls.MotionHazard)
            //{
            //	port.Write(String.Format("${0},{1},{2},{3},{4},{5}\0",
            //		(Controls.MotionBaseYaw.ToString("F3")),
            //		(Controls.MotionBasePitch.ToString("F3")),
            //		(Controls.MotionBaseRoll.ToString("F3")),
            //	
            //		(Controls.inputXaxis.ToString("F3")),
            //		(Controls.inputYaxis.ToString("F3")),
            //		(Controls.inputZaxis.ToString("F3")))
            //	); 
            //}

            if (MotionBaseTester)
            {
                Quaternion MotionBaseTestQ;
                MotionBaseTestQ = Quaternion.Euler(new Vector3(MotionData.MotionBasePitch, MotionData.MotionBaseYaw, MotionData.MotionBaseRoll));
                MotionBaseTester.transform.rotation = MotionBaseTestQ;
            }




            port.Write(String.Format("${0},{1},{2},{3},{4},{5}\0",
				(MotionData.MotionBaseYaw.ToString("F3")),
				(MotionData.MotionBasePitch.ToString("F3")),
				(MotionData.MotionBaseRoll.ToString("F3")),

				(Controls.inputXaxis.ToString("F3")),
				(Controls.inputYaxis.ToString("F3")),
				(Controls.inputZaxis.ToString("F3")))
			);



            //port.Write(String.Format("${0}\0",
            //	(Controls.MotionBasePitch.ToString("F3")))
            //);



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

	void HazardCheck()
	{
		if(Mathf.Abs(LastMove.x - MotionData.MotionBasePitch) > MotionData.MotionHazardSensitivity)
		{
            MotionData.MotionHazard = true;
		}

		//if(Mathf.Abs(LastMove.y - Controls.MotionBaseYaw) > Controls.MotionHazardSensitivity)
		//{
		//	Controls.MotionHazard = true;
		//}

		if(Mathf.Abs(LastMove.z - MotionData.MotionBaseRoll) > MotionData.MotionHazardSensitivity)
		{
            MotionData.MotionHazard = true;
		}

		LastMove.x = MotionData.MotionBasePitch;
		LastMove.y = MotionData.MotionBaseYaw;
		LastMove.z = MotionData.MotionBaseRoll;
	}

    private void SlerpWithRemap()
    {
        Controls.CalculateYawVelocity();

        MotionData.MotionBaseYaw *= MotionData.MotionYawMax;

        preMapped = Server.transform.rotation.eulerAngles;

        if (preMapped.x > 180f)
        {
            preMapped.x -= 360f;
        }

        if (preMapped.z > 180f)
        {
            preMapped.z -= 360f;
        }

        preMapped.x /= 90f;
        preMapped.z /= 180f;

        preMapped.x *= (MotionData.MotionPitchMax * 1.3f);
        preMapped.z *= (MotionData.MotionRollMax * 1.3f);
        
        Quaternion reMapped;
        reMapped = Quaternion.Euler(preMapped.x, MotionData.MotionBaseYaw, preMapped.z);

        //lerp the slerp
        //float lerpSlerp1;

        float angle = Quaternion.Angle(reMapped, motionBase);

        //slerpNerf = angle / 30f; //Mathf.Clamp(angle / 30f, 0f, 1f);
        slerpNerf = Mathf.Clamp(angle / 30f, 0.1f, 2f);
        slerpNerf = 2f - slerpNerf +2f;

        motionBase = Quaternion.Slerp(motionBase, reMapped, Time.deltaTime * (MotionData.MotionSlerpSpeed * slerpNerf));
    }
}