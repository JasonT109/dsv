using UnityEngine;
using System.Collections;

public class debugShipPhysics : MonoBehaviour 
{
    public GameObject StabiliseRoll;
    public GameObject FullStabilisation;

    public GameObject JoyStickSwitcher;

    public GameObject JoystickOverride;

    public sliderWidget JoystickX;
    public sliderWidget JoystickY;
    public sliderWidget JoystickThrust;
    public sliderWidget JoystickVertical;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (StabiliseRoll.GetComponent<buttonControl>().active)
        {
            GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
            if(Root)
            {
                Root.GetComponent<SubControl>().isAutoStabilised = true;
            }
        }
        else
        {
            GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
            if(Root)
            {
                Root.GetComponent<SubControl>().isAutoStabilised = false;
            }
        }

        if (FullStabilisation.GetComponent<buttonControl>().active)
        {
            StabiliseRoll.GetComponent<buttonControl>().active = true;

            GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
            if(Root)
            {
                Root.GetComponent<SubControl>().IsPitchAlsoStabilised = true;
            }
        }
        else
        {
            GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
            if(Root)
            {
                Root.GetComponent<SubControl>().IsPitchAlsoStabilised = false;
            }
        }

        if(JoyStickSwitcher.GetComponent<buttonControl>().active)
        {
            GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
            if(Root)
            {
                // Root.GetComponent<serverData>().IsJoystickSwapped = true;
                // JoyStickSwitcher.GetComponent<buttonControl>().warning = true;
            }
        }
        else
        {
            GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
            if(Root)
            {
                // Root.GetComponent<serverData>().IsJoystickSwapped = false;
                // JoyStickSwitcher.GetComponent<buttonControl>().warning = false;
            } 
        }

        if(OverrideLogic())
        {
            SliderLogic();
        }
	}

    void SliderLogic()
    {
        //if(JoystickX.valueChanged)
        {
            GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
            Root.GetComponent<SubControl>().inputXaxis = JoystickX.returnValue;
        }

        //if(JoystickY.valueChanged)
        {
            GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
            Root.GetComponent<SubControl>().inputYaxis = JoystickY.returnValue;
        }

        //if(JoystickThrust.valueChanged)
        {
            GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
            Root.GetComponent<SubControl>().inputZaxis = JoystickThrust.returnValue;
        }

        //if(JoystickVertical.valueChanged)
        {
            GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
            Root.GetComponent<SubControl>().inputYaxis2 = JoystickVertical.returnValue;
        }
    }

    bool OverrideLogic()
    {

        bool r = false;

        if (JoystickOverride.GetComponent<buttonControl>().active)
        {
            GameObject Root = GameObject.FindGameObjectWithTag("ServerData");
            if(Root)
            {
                Root.GetComponent<SubControl>().JoystickOverride = true;
                JoystickOverride.GetComponent<buttonControl>().warning = true;
                r = true;
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

        return r;
    }
}
