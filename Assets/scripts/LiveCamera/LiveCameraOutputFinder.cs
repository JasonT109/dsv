using UnityEngine;
using System.Collections;

public class LiveCameraOutputFinder : MonoBehaviour 
{
    public bool _isLive;
    public bool isLive
    {
        get
        {
            EnableCameras();
            return _isLive;
        }
        set
        {
            _isLive = value;
        }
    }

    private LiveFeedInputManager LiveFeeds;
    private int iNumCams;

    public void EnableCameras()
    {
        LiveFeedInputManager livefeedmanager = ObjectFinder.Find<LiveFeedInputManager>();

        if (livefeedmanager)
        {
            LiveFeeds = livefeedmanager;
            if (LiveFeeds.getNumCams() > 0)
                isLive = true;
            else
                Debug.Log("AVPro live feed manager found but no cameras active!");
        }
        else
            Debug.Log("No AVPro live feed manager found!");
    }

    public AVProLiveCamera GetOutput(int _OutPut)
    {
        EnableCameras();

        if (!LiveFeeds)
            return (null);

        return (LiveFeeds.AVLiveCameras[_OutPut]);
    }

    public int getNumCams()
    {
        EnableCameras();

        if (!LiveFeeds)
            return 0;

        return LiveFeeds.getNumCams();
    }

    void Start () 
	{
        EnableCameras();
	}

	void Awake()
	{
        EnableCameras();
    }
}
