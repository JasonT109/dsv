using UnityEngine;
using System.Collections;

public class LiveCameraOutputFinder : MonoBehaviour 
{
	LiveFeedInputManager LiveFeeds;
	public bool isLive = false;

	int iNumCams;

	// Use this for initialization
	void Start () 
	{
		if(GameObject.FindGameObjectWithTag("LiveFeedManager"))
		{
			if(GameObject.FindGameObjectWithTag("LiveFeedManager").GetComponent<LiveFeedInputManager>())
			{
				LiveFeeds = GameObject.FindGameObjectWithTag("LiveFeedManager").GetComponent<LiveFeedInputManager>();
				isLive = true;
			}
		}
	}

	void Awake()
	{
		if(GameObject.FindGameObjectWithTag("LiveFeedManager"))
		{
			if(GameObject.FindGameObjectWithTag("LiveFeedManager").GetComponent<LiveFeedInputManager>())
			{
				LiveFeeds = GameObject.FindGameObjectWithTag("LiveFeedManager").GetComponent<LiveFeedInputManager>();
				isLive = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public AVProLiveCamera GetOutput(int _OutPut)
	{
		if(!LiveFeeds)
		{
			if(GameObject.FindGameObjectWithTag("LiveFeedManager"))
			{
				if(GameObject.FindGameObjectWithTag("LiveFeedManager").GetComponent<LiveFeedInputManager>())
				{
					LiveFeeds = GameObject.FindGameObjectWithTag("LiveFeedManager").GetComponent<LiveFeedInputManager>();
					isLive = true;
				}
			}
		}
		
		if(!LiveFeeds)
		{
			//return(this.GetComponent<AVProLiveCamera>());
			return(null);
		}
	
		return(LiveFeeds.AVLiveCameras[_OutPut]);
	}

	public int getNumCams()
	{
		return(GameObject.FindGameObjectWithTag("LiveFeedManager").GetComponent<LiveFeedInputManager>().getNumCams());
	}
}
