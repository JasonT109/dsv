using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LiveFeedInputManager : MonoBehaviour 
{
    public int iNumCameras = 0;
    public Text ButtonText;
    //bool ButtonHeld = false;
    public Image StartImg;

    public int defaultModeIndex = 40;
    public AVProLiveCameraManager AVLiveCameraManager;
    public AVProLiveCamera[] AVLiveCameras;
    public GameObject[] AVCameraOutputs;

    [Header("Configuration")]
    public Color SelectedColor = Color.white;

    public GameObject Inputs;

	// Use this for initialization
	void Start () 
    {
        RefreshButtonText();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    void RefreshButtonText()
    {
        if(iNumCameras > 0)
        {
            if(ButtonText)
            {
                ButtonText.text = "LIVE CAMS - " + iNumCameras;
            }
        }
        else
        {
            if(ButtonText)
            {
                ButtonText.text = "LIVE CAMS - NONE";
            }
        }
    }

    public void ToggleCameras()
    {
        
            iNumCameras++;
            if(iNumCameras > 4)
            {
                iNumCameras = 0;
            }

            RefreshButtonText();
    }

    public void StartCameras()
    {
        StartImg.color = SelectedColor;
        Inputs.SetActive(true);

        for(int i = 0; i < iNumCameras; ++i)
        {
            AVLiveCameras[i]._desiredDeviceIndex = i;
            AVLiveCameras[i]._desiredModeIndex = defaultModeIndex;
            AVLiveCameras[i]._deviceSelection = AVProLiveCamera.SelectDeviceBy.Index;
            AVLiveCameras[i]._modeSelection = AVProLiveCamera.SelectModeBy.Index;
            AVLiveCameras[i].Begin();

            AVCameraOutputs[i].SetActive(true);
        }
    }

	public int getNumCams()
	{
		return (iNumCameras);
	}
}
