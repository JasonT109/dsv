using UnityEngine;
using System.Collections;
using Meg.Networking;

public class ROVLiveCameras : MonoBehaviour
{
    public AVProLiveCameraUGUIComponent[] AVCameraOutputs;
    public LiveCameraOutputFinder Outputs;

    public int iCurrentMain = 0;

    // Use this for initialization
    void Start ()
    {
	    
	}

    void OnEnable()
    {
        if(Outputs.isLive)
        {
            AVCameraOutputs[0].m_liveCamera = Outputs.GetOutput(iCurrentMain);
            AVCameraOutputs[1].m_liveCamera = Outputs.GetOutput(0);
            AVCameraOutputs[2].m_liveCamera = Outputs.GetOutput(1);
            AVCameraOutputs[3].m_liveCamera = Outputs.GetOutput(2);
            AVCameraOutputs[4].m_liveCamera = Outputs.GetOutput(3);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!serverUtils.IsReady())
        {
            return;
        }

        if (serverUtils.SubControl.isControlDecentMode && serverUtils.OSRov.ROVCameraState == 1)
        {
            serverUtils.SubControl.isControlDecentMode = false;
            iCurrentMain++;
            if(iCurrentMain > 3)
            {
                iCurrentMain = 0;
            }
            AVCameraOutputs[0].m_liveCamera = Outputs.GetOutput(iCurrentMain);

        }
	}
}
