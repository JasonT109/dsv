using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

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

    public Image GliderButtonImg;
    public Image BigSubButtonImg;
    public Image DCCButtonImg;
    int ID = 0; // ID of system, in this case it's set to Glider

    //public NetworkManagerCustom CustomNetMan;
    NetworkManagerCustom CustomNetMan;
    public NetworkManager manager;

    public GameObject ZhangLogo;
    public GameObject StartButtonObj;

    public string Password = "EnglishBreakfast";
    public string Password2 = "EarlGrey";

    public bool IsPasswordCorrect = false;
    public bool IsPasswordPin = false;

    public GameObject PasswordRoot;

    public string GliderLevel = "screen_gliders";
    public string BigSubLevel = "screen_01";
    public string DCCLevel = "screen_gliders";


	// Use this for initialization
	void Start () 
    {
        CustomNetMan = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManagerCustom>();

        ToggleHost();
        HostIP.text = Network.player.ipAddress;
        HostIPTextPlaceholder.text = Network.player.ipAddress;

        CustomNetMan.connectionInfo = Network.player.ipAddress;
        CustomNetMan.connectionList[0] = Network.player.ipAddress;
     
        ToggleBigSub();

        if(IsPasswordCorrect)
        {
            ZhangLogo.SetActive(true);
            StartButtonObj.SetActive(true);
            PasswordRoot.SetActive(false);
        }
        else
        {
            ZhangLogo.SetActive(false);
            StartButtonObj.SetActive(false);
            PasswordRoot.SetActive(true);
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

    public void ToggleGlider()
    {
        ID = 0;
        GliderButtonImg.color = Color.white;
        BigSubButtonImg.color = Color.grey;
        DCCButtonImg.color = Color.grey;
        manager.onlineScene = GliderLevel;

    }

    public void ToggleBigSub()
    {
        ID = 2;
        GliderButtonImg.color = Color.grey;;
        BigSubButtonImg.color = Color.white;;
        DCCButtonImg.color = Color.grey;
        manager.onlineScene = BigSubLevel;
    }

    public void ToggleDCC()
    {
        ID = 3;
        GliderButtonImg.color = Color.grey;;
        BigSubButtonImg.color = Color.grey;;
        DCCButtonImg.color = Color.white;
        manager.onlineScene = DCCLevel;
    }

    public void PasswordLiveInput(string _PasswordInput)
    {
        if(IsPasswordPin)
        {
            PasswordInputChanged(_PasswordInput);
        }
    }

    public void PasswordInputChanged(string _PasswordInput)
    {
        if(_PasswordInput != "")
        {
            if(_PasswordInput == Password)
            {
                IsPasswordCorrect = true;
                if(ZhangLogo)
                {
                    ZhangLogo.SetActive(true);
                    PasswordRoot.SetActive(false);
                }
                StartButtonObj.SetActive(true);
            }
            else if(_PasswordInput == Password2)
            {
                IsPasswordCorrect = true;
                if(ZhangLogo)
                {
                    ZhangLogo.SetActive(true);
                    PasswordRoot.SetActive(false);
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
    }

    public void HostIPChanged(string _HostIP)
    {
        if(_HostIP != "")
        {
            CustomNetMan.connectionInfo = _HostIP;
            CustomNetMan.connectionList[0] = _HostIP;
        }
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
