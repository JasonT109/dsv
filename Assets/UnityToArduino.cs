using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;

public class UnityToArduino : MonoBehaviour 
{
    private SerialPort port;
    public serverData Server;
    public string COMPort = "COM6";

    // initialization
    void Start()
    { 
        port = new SerialPort(COMPort, 9600);
        if (!port.IsOpen)
        {
            port.Open();
            StartCoroutine(SendData());
        }
    }

    IEnumerator SendData()
    {
        while (port.IsOpen)
        {
            port.Write(String.Format("{0}, {1}, {2}\0",
                (int)(Server.yawAngle),
                (int)(Server.pitchAngle),
                (int)(Server.rollAngle))
            ); 
            yield return new WaitForSeconds(0.1f);
        }
    } 

    void OnDestroy()
    { 
        port.Close(); 
    }
}