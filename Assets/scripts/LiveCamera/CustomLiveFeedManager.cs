using UnityEngine;
using System.Collections;

public class CustomLiveFeedManager : MonoBehaviour 
{
	public AVProLiveCamera[] _liveCameras;
	//int[] CameraIndexData;

	public int defaultDeviceIndex = 8;
	public int defaultModeIndex = 31;

	int CurrentCamera = 0;

	public AVProLiveCameraManager _liveCameraManager;

	// Use this for initialization
	void Start () 
	{
		//uncomment to have cameras setup and begin playing on start
		//this sets the first camera to slot 1, second to slot 2 etc...
        /*
		for (int i = 0; i < _liveCameras.Length; ++i) 
		{
			_liveCameras [i]._desiredDeviceIndex = ConvertDeviceSlotToIndex(i);
			_liveCameras [i]._desiredModeIndex = defaultModeIndex;
			_liveCameras [i]._deviceSelection = AVProLiveCamera.SelectDeviceBy.Index;
			_liveCameras [i]._modeSelection = AVProLiveCamera.SelectModeBy.Index;
			_liveCameras [i].Begin ();
		}*/
	}

	// Update is called once per frame
	void Update () 
	{

	}

	//this function sets the slot
	public void SetSlot(int _Slot)
	{
		_liveCameras [CurrentCamera]._desiredDeviceIndex = ConvertDeviceSlotToIndex(_Slot);
		_liveCameras [CurrentCamera]._deviceSelection = AVProLiveCamera.SelectDeviceBy.Index;
		//uncomment if this is intended to take effect during this function call
		_liveCameras [CurrentCamera].Begin ();
	}

	public void SetCamera(int _Index)
	{
		CurrentCamera = _Index;
	}

	public void SetMode(int _Index)
	{
		_liveCameras [CurrentCamera]._desiredModeIndex = _Index;
		_liveCameras [CurrentCamera]._modeSelection = AVProLiveCamera.SelectModeBy.Index;
		//uncomment if this is intended to take effect during this function call
		_liveCameras [CurrentCamera].Begin ();
	}

	public void Begin()
	{
		_liveCameras [CurrentCamera].Begin ();
	}

	//this function takes in the slot number 
	//(physical SDI output from card in a logical order, left to right), 
	//and returns the device index
	int ConvertDeviceSlotToIndex(int _Slot)
	{
		switch (_Slot) 
		{
		case 0:
			{
				return(9);
			}
			//break;
		case 1:
			{
				return(11);
			}
			//break;
		case 2:
			{
				return(14);
			}
			//break;
		case 3:
			{
				return(10);
			}
			//break;
		case 4:
			{
				return(8);
			}
			//break;
		case 5:
			{
				return(15);
			}
			//break;
		case 6:
			{
				return(13);
			}
			//break;
		case 7:
			{
				return(12);
			}
			//break;
		default:
			{
				return(-1);
			}
		}
	}
}
