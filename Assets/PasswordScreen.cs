using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PasswordScreen : MonoBehaviour 
{
    bool IsClient = true;
    public GameObject HostIPObject;
    public GameObject LocalIP;

    public Image ClientButtonImg;
    public Image HostButtonImg;

    public Text thisIP;
    public Text HostIP;
    public Text HostIPTextPlaceholder;

    //public NetworkManagerCustom CustomNetMan;
    NetworkManagerCustom CustomNetMan;

    public GameObject ZhangLogo;
    public GameObject StartButtonObj;

    public string Password = "EnglishBreakfast";

    public bool IsPasswordCorrect = false;


	// Use this for initialization
	void Start () 
    {
        CustomNetMan = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManagerCustom>();

        ToggleClient();
        HostIP.text = Network.player.ipAddress;
        HostIPTextPlaceholder.text = Network.player.ipAddress;

        CustomNetMan.connectionInfo = Network.player.ipAddress;
        CustomNetMan.connectionList[0] = Network.player.ipAddress;

        if(IsPasswordCorrect)
        {
            ZhangLogo.SetActive(true);
            StartButtonObj.SetActive(true);
        }
        else
        {
            ZhangLogo.SetActive(false);
            StartButtonObj.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    public void ToggleHost()
    {
        IsClient = false;
        HostIPObject.SetActive(false);
        LocalIP.SetActive(true);
        HostButtonImg.color = Color.white;
        ClientButtonImg.color = Color.grey;
        thisIP.text = Network.player.ipAddress;
    }

    public void ToggleClient()
    {
        IsClient = true;
        HostIPObject.SetActive(true);
        LocalIP.SetActive(false);
        HostButtonImg.color = Color.grey;
        ClientButtonImg.color = Color.white;
    }

    public void PasswordInputChanged(string _PasswordInput)
    {
        if(_PasswordInput == Password)
        {
            IsPasswordCorrect = true;
            if(ZhangLogo)
            {
                ZhangLogo.SetActive(true);
            }
            StartButtonObj.SetActive(true);
        }
        else
        {
            IsPasswordCorrect = false;
            ZhangLogo.SetActive(false);
            StartButtonObj.SetActive(false);
        }
    }

    public void HostIPChanged(string _HostIP)
    {
        CustomNetMan.connectionInfo = _HostIP;
        CustomNetMan.connectionList[0] = _HostIP;
    }

    public void StartButton()
    {
        if(IsClient)
        {
            CustomNetMan.StartClient();
        }
        else
        {
            CustomNetMan.StartServer();
        }
    }
}
