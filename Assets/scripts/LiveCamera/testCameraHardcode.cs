using UnityEngine;
using System.Collections;

public class testCameraHardcode : MonoBehaviour 
{
	public AVProLiveCamera _liveCamera;
	public AVProLiveCameraManager _liveCameraManager;
	//public GUISkin _guiSkin;
	// Use this for initialization
	void Start () 
	{
		
		_liveCamera._desiredDeviceIndex = 8;
		_liveCamera._desiredModeIndex = 20;
		_liveCamera._deviceSelection = AVProLiveCamera.SelectDeviceBy.Index;
		_liveCamera._modeSelection = AVProLiveCamera.SelectModeBy.Index;
		_liveCamera.Begin();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
