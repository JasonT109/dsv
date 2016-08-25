using UnityEngine;
using System.Collections;

public class SonarSweeper : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        var ping = other.GetComponent<SonarPing>();
        if (ping)
            ping.Pulse();
    }
    
}
