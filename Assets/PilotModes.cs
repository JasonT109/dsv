﻿using UnityEngine;
using System.Collections;
using Meg.Networking;

public class PilotModes : MonoBehaviour 
{
	public buttonControl AutoPilotButton;
	public buttonControl DescentModeButton;

	public GameObject DecentVis;
	public GameObject AutoPilotVis;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(DescentModeButton.active && !(serverUtils.GetServerBool("iscontroldecentmode")))
		{
			//serverUtils.SetServerBool("iscontroldecentmode", true);
			DescentModeButton.active = false;
			DecentVis.SetActive(false);
		}
		else if(!(DescentModeButton.active) && (serverUtils.GetServerBool("iscontroldecentmode")))
		{
			//serverUtils.SetServerBool("iscontroldecentmode", false);
			DescentModeButton.active = true;
			DecentVis.SetActive(true);
		}

		if(AutoPilotButton.active && !(serverUtils.GetServerBool("isautopilot")))
		{
			//serverUtils.SetServerBool("iscontroldecentmode", true);
			AutoPilotButton.active = false;
			AutoPilotVis.SetActive(false);
		}
		else if(!(AutoPilotButton.active) && (serverUtils.GetServerBool("isautopilot")))
		{
			//serverUtils.SetServerBool("iscontroldecentmode", false);
			AutoPilotButton.active = true;
			AutoPilotVis.SetActive(true);
		}


	}

	public void ToggleAutoPilot()
	{
		if(serverUtils.GetServerBool("isautopilot"))
		{
			serverUtils.SetServerBool("isautopilot", false);
			AutoPilotVis.SetActive(false);
		}
		else
		{
			serverUtils.SetServerBool("isautopilot", true);
			AutoPilotVis.SetActive(true);
		}
	}

	public void ToggleDecentMode()
	{
		if(serverUtils.GetServerBool("iscontroldecentmode"))
		{
			serverUtils.SetServerBool("iscontroldecentmode", false);
			DecentVis.SetActive(false);
		}
		else
		{
			serverUtils.SetServerBool("iscontroldecentmode", true);
			DecentVis.SetActive(true);
		}
	}

}
