using UnityEngine;
using System.Collections;

public class testGui : MonoBehaviour 
{
	public GameObject Camera;
	public GameObject Slot;
	public GameObject Mode;

	//reference set in editor to the CustomLiveFeedManager
	public GameObject LiveFeed;

	//this function takes the data set in the GUI, 
	//sends it to the CustomLiveFeedManager 
	//and begins the camera
	public void Go()
	{
		int CamIndex = Camera.GetComponent<LiveFeedGUI> ().Index;
		int SlotNum = Slot.GetComponent<LiveFeedGUI> ().Index;
		int ModeIndex = Mode.GetComponent<LiveFeedGUI> ().Index;

		LiveFeed.GetComponent<CustomLiveFeedManager> ().SetCamera (CamIndex);
		LiveFeed.GetComponent<CustomLiveFeedManager> ().SetSlot (SlotNum);
		LiveFeed.GetComponent<CustomLiveFeedManager> ().SetMode (ModeIndex + 1);
		//mode has +1 as it ranges from 0 - 33
		//camIndex and SlotNum range from 1-8

		LiveFeed.GetComponent<CustomLiveFeedManager> ().Begin ();
	}
}
