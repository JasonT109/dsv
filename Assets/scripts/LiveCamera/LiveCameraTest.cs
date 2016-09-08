using UnityEngine;
using System.Collections;

public class LiveCameraTest : MonoBehaviour 
{
	public LiveCameraOutputFinder Outputs;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown("space"))
		{
			this.GetComponent<AVProLiveCameraUGUIComponent>().m_liveCamera = Outputs.GetOutput(0);
			//Outputs.
		}
	}
}
