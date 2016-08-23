using UnityEngine;
using System.Collections;

public class LiveFeedOutputComms : MonoBehaviour 
{

	public AVProLiveCameraMeshApply[] CameraMesh;
	public LiveCameraOutputFinder Outputs;

	public GameObject[] LivePlane;
	public GameObject[] FakePlane;

	// Use this for initialization
	void Start () 
	{
		//CameraMesh[0]._liveCamera = Outputs.GetOutput(0);
		if(Outputs.isLive)
		{
			for(int i = 0; i < 4; ++i)
			{
				if(i < Outputs.getNumCams())
				{
					CameraMesh[i]._liveCamera = Outputs.GetOutput(i);
					LivePlane[i].SetActive(true);
					FakePlane[i].SetActive(false);
				}
				else
				{
					LivePlane[i].SetActive(false);
					FakePlane[i].SetActive(true);
				}
			}

			//works
			//CameraMesh[4]._liveCamera = Outputs.GetOutput(0);		

			//CameraMesh[4]._liveCamera = Outputs.GetOutput(0);
			//LivePlane[4].SetActive(true);
			//FakePlane[4].SetActive(false);
			//
			//if(Outputs.getNumCams() > 1)
			//{
			//	CameraMesh[5]._liveCamera = Outputs.GetOutput(1);
			//	LivePlane[5].SetActive(true);
			//	FakePlane[5].SetActive(false);
			//}
			//else
			//{
			//	LivePlane[5].SetActive(false);
			//	FakePlane[5].SetActive(true);
			//}
		
			//for(int i = 4; i < 6; ++i)
			//{
			//	if(i < Outputs.getNumCams())
			//	{
			//		CameraMesh[i]._liveCamera = Outputs.GetOutput(i-4);
			//		LivePlane[i].SetActive(true);
			//		FakePlane[i].SetActive(false);
			//	}
			//	else
			//	{
			//		LivePlane[i].SetActive(false);
			//		FakePlane[i].SetActive(true);
			//	}
			//}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
