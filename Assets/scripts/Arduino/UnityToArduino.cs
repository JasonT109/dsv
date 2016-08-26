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
			

	}

	void Awake()
	{
		//if(GameObject.FindGameObjectWithTag("ArduinoManager").GetComponent<ArduinoManager>())
		//{
		//	Settings = GameObject.FindGameObjectWithTag("ArduinoManager").GetComponent<ArduinoManager>();
		//	COMPort = Settings.ComPort;
		//}
	}

	IEnumerator SendData()
	{
		while (port.IsOpen)
		{
			motionBase = Quaternion.Slerp(motionBase, Server.transform.rotation, Time.deltaTime*2f);
			Controls.MotionBasePitch = motionBase.eulerAngles.x;
			Controls.MotionBaseYaw = motionBase.eulerAngles.y;
			Controls.MotionBaseRoll = motionBase.eulerAngles.z;

			if(Controls.MotionBasePitch > 180f)
			{
				Controls.MotionBasePitch = motionBase.eulerAngles.x-360f;
			}

			if(Controls.MotionBaseYaw > 180f)
			{
				Controls.MotionBaseYaw = motionBase.eulerAngles.y-360f;
			}

			if(Controls.MotionBaseRoll > 180f)
			{
				Controls.MotionBaseRoll = motionBase.eulerAngles.z-360f;
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

			port.Write(String.Format("${0},{1},{2},{3},{4},{5}\0",
				(Controls.MotionBaseYaw.ToString("F3")),
				(Controls.MotionBasePitch.ToString("F3")),
				(Controls.MotionBaseRoll.ToString("F3")),
			
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
}