using UnityEngine;
using System.Collections;

public class LiveFeedOutputComms : MonoBehaviour 
{
	public LiveCameraOutputFinder Outputs;
    public DCCCameraLiveFeed[] LiveFeeds;

    private AVProLiveCamera GetFeedOutput(int _feedID)
    {
        AVProLiveCamera liveCamera = null;

        int numCams = Outputs.getNumCams();
        if (numCams > _feedID)
            liveCamera = Outputs.GetOutput(_feedID);

        return liveCamera;
    }

	void Start () 
	{
		if (Outputs && Outputs.isLive)
		{
            for (int i = 0; i < LiveFeeds.Length; i++)
            {
                AVProLiveCamera cam = GetFeedOutput(LiveFeeds[i].LiveFeedID);
                if (cam)
                    LiveFeeds[i].CameraMesh._liveCamera = Outputs.GetOutput(i);
            }
		}
	}
}
