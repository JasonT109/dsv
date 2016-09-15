using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LiveFeedInputManager : MonoBehaviour 
{
    [Header("Configuration")]
    public int iNumCameras = 0;
    public Text ButtonText;
    public Image StartImg;
    public int defaultModeIndex = 40;
    public AVProLiveCameraManager AVLiveCameraManager;
    public AVProLiveCamera[] AVLiveCameras = new AVProLiveCamera[4];
    public GameObject[] AVCameraOutputs;
    public Color SelectedColor = Color.white;
    public GameObject Inputs;

    [Header("Button Appearance")]
    public GameObject button1hilight;
    public GameObject button2hilight;
    public GameObject button3hilight;
    public GameObject button4hilight;

    void Start () 
    {
        RefreshButtonText();
	}

    void RefreshButtonText()
    {
        if(iNumCameras > 0)
        {
            if(ButtonText)
                ButtonText.text = "LIVE CAMS - " + iNumCameras;
        }
        else
        {
            if(ButtonText)
                ButtonText.text = "LIVE CAMS - NONE";
        }
    }

    public void ToggleButtonState(int id, bool state)
    {
        switch (id)
        {
            case 0:
                button1hilight.SetActive(state);
                break;
            case 1:
                button1hilight.SetActive(state);
                break;
            case 2:
                button1hilight.SetActive(state);
                break;
            case 3:
                button1hilight.SetActive(state);
                break;
        }
    }

    public void ToggleCamera(int id)
    {
        if (!AVCameraOutputs[id].activeInHierarchy)
        {
            iNumCameras++;
            AVLiveCameras[id]._desiredDeviceIndex = id;
            AVLiveCameras[id]._desiredModeIndex = defaultModeIndex;
            AVLiveCameras[id]._deviceSelection = AVProLiveCamera.SelectDeviceBy.Index;
            AVLiveCameras[id]._modeSelection = AVProLiveCamera.SelectModeBy.Index;
            AVLiveCameras[id].Begin();

            AVCameraOutputs[id].SetActive(true);
            ToggleButtonState(id, true);
        }
        else
        {
            iNumCameras--;
            AVLiveCameras[id].Close(AVLiveCameras[id].Device);
            AVCameraOutputs[id].SetActive(false);
            ToggleButtonState(id, false);
        }

        if (iNumCameras > 0)
            Inputs.SetActive(true);
    }

    public void ToggleCameras()
    {
            iNumCameras++;
            if(iNumCameras > 4)
                iNumCameras = 0;

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
