using UnityEngine;
using System.Collections;

public class VirtualJoystickOverride : MonoBehaviour 
{
    public GameObject JoystickOverride;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (JoystickOverride.GetComponent<buttonControl>().active)
        {
            GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
            if(Root)
            {
                Root.GetComponent<SubControl>().JoystickOverride = true;
                JoystickOverride.GetComponent<buttonControl>().warning = true;
            }
        }
        else
        {
            GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
            if(Root)
            {
                Root.GetComponent<SubControl>().JoystickOverride = false;
                JoystickOverride.GetComponent<buttonControl>().warning = false;
            }
        }
	}
}
