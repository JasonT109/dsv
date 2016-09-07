using UnityEngine;
using System.Collections;

public class ArduinoManager : MonoBehaviour 
{
	public string ComPort;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void setPort(string _port)
	{
		ComPort = _port;
	}
}
