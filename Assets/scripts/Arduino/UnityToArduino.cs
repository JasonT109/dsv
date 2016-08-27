using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;

public class UnityToArduino : MonoBehaviour 
{
	private SerialPort port;
	public serverData Server;
	public SubControl Controls;
	public string COMPort = "";

	private Quaternion motionBase;

	ArduinoManager Settings;

	private Vector3 LastMove;

	float time = 0.0f;

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
			
		LastMove.x = Controls.MotionBasePitch;
		LastMove.y = Controls.MotionBaseYaw;
		LastMove.z = Controls.MotionBaseRoll;

	}

	void Awake()
	{
		//if(GameObject.FindGameObjectWithTag("ArduinoManager").GetComponent<ArduinoManager>())
		//{
		//	Settings = GameObject.FindGameObjectWithTag("ArduinoManager").GetComponent<ArduinoManager>();
		//	COMPort = Settings.ComPort;
		//}

		LastMove.x = Controls.MotionBasePitch;
		LastMove.y = Controls.MotionBaseYaw;
		LastMove.z = Controls.MotionBaseRoll;
	}

	IEnumerator SendData()
	{
		while (port.IsOpen)
		{
			if(Controls.MotionSafety)
			{
				motionBase = Quaternion.Slerp(motionBase, Server.transform.rotation, Time.deltaTime*Controls.MotionSlerpSpeed);
				Controls.MotionBasePitch = motionBase.eulerAngles.x;
				Controls.MotionBaseYaw = motionBase.eulerAngles.y;
				Controls.MotionBaseRoll = motionBase.eulerAngles.z;

				if(Controls.MotionBasePitch > 180f)
				{
					Controls.MotionBasePitch = motionBase.eulerAngles.x-360f;
				}

				if(Controls.MotionBaseRoll > 180f)
				{
					Controls.MotionBaseRoll = motionBase.eulerAngles.z-360f;
				}
			}
			else
			{
				Controls.MotionBasePitch = Server.pitchAngle;
				Controls.MotionBaseYaw = Server.yawAngle;
				Controls.MotionBaseRoll = Server.rollAngle;
			}

			if(Controls.MotionBaseRoll > 33f)
			{
				Controls.MotionBaseRoll = 33f;
			}

			if(Controls.MotionBaseRoll < -33f)
			{
				Controls.MotionBaseRoll = -33f;
			}

			if(Controls.MotionBasePitch > 37f)
			{
				Controls.MotionBasePitch = 37f;
			}

			if(Controls.MotionBasePitch < -37f)
			{
				Controls.MotionBasePitch = -37f;
			}
				
			HazardCheck();

			if(!Controls.MotionHazard)
			{
				port.Write(String.Format("${0},{1},{2},{3},{4},{5}\0",
					(Controls.MotionBaseYaw.ToString("F3")),
					(Controls.MotionBasePitch.ToString("F3")),
					(Controls.MotionBaseRoll.ToString("F3")),
				
					(Controls.inputXaxis.ToString("F3")),
					(Controls.inputYaxis.ToString("F3")),
					(Controls.inputZaxis.ToString("F3")))
				); 
			}

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
            port.Close();
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e);
        }
    }

	void HazardCheck()
	{
		if(Mathf.Abs(LastMove.x - Controls.MotionBasePitch) > Controls.MotionHazardSensitivity)
		{
			Controls.MotionHazard = true;
		}

		//if(Mathf.Abs(LastMove.y - Controls.MotionBaseYaw) > Controls.MotionHazardSensitivity)
		//{
		//	Controls.MotionHazard = true;
		//}

		if(Mathf.Abs(LastMove.z - Controls.MotionBaseRoll) > Controls.MotionHazardSensitivity)
		{
			Controls.MotionHazard = true;
		}

		LastMove.x = Controls.MotionBasePitch;
		LastMove.y = Controls.MotionBaseYaw;
		LastMove.z = Controls.MotionBaseRoll;
	}
}