using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;

public class UnityToArduino : MonoBehaviour 
{
	private SerialPort port;
	public serverData Server;
	public SubControl Controls;
    public MotionBaseData MotionData;
	public string COMPort = "";

	private Quaternion motionBase;

	ArduinoManager Settings;

	private Vector3 LastMove;

	private Vector3 flat;
	private Quaternion flatQ;

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
					motionBase = Quaternion.Slerp(motionBase, Server.transform.rotation, Time.deltaTime* MotionData.MotionSlerpSpeed);
				}
				else
				{
					motionBase = Quaternion.Slerp(motionBase, flatQ, Time.deltaTime*0.5f);
				}

                MotionData.MotionBasePitch = motionBase.eulerAngles.x;
                MotionData.MotionBaseYaw = motionBase.eulerAngles.y;
                MotionData.MotionBaseRoll = motionBase.eulerAngles.z;

				if(MotionData.MotionBasePitch > 180f)
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

			if(MotionData.MotionBaseRoll > 33f)
			{
                MotionData.MotionBaseRoll = 33f;
			}

			if(MotionData.MotionBaseRoll < -33f)
			{
                MotionData.MotionBaseRoll = -33f;
			}

			if(MotionData.MotionBasePitch > 37f)
			{
                MotionData.MotionBasePitch = 37f;
			}

			if(MotionData.MotionBasePitch < -37f)
			{
                MotionData.MotionBasePitch = -37f;
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
}