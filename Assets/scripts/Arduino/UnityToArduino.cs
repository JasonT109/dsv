using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;

public class UnityToArduino : MonoBehaviour 
{
	private SerialPort port;
	public serverData Server;
	public SubControl Controls;
	public string COMPort = "COM6";

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


			port.Write(String.Format("${0},{1},{2},{3},{4},{5}\0",
				(Server.yawAngle.ToString("F3")),
				(Server.pitchAngle.ToString("F3")),
				(Server.rollAngle.ToString("F3")),

				(Controls.inputXaxis.ToString("F3")),
				(Controls.inputYaxis.ToString("F3")),
				(Controls.inputZaxis.ToString("F3")))
			); 
				

			yield return new WaitForSeconds(0.0083f);
		}
	} 

	void OnDestroy()
	{ 
		port.Close(); 
	}
}