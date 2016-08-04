using UnityEngine;
using UnityEngine.UI;

public class PasswordScreen : MonoBehaviour 
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]
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

    public GameObject ZhangLogo;
    public GameObject StartButtonObj;
    public GameObject PasswordRoot;

    [Header("Configuration")]
    public string Password = "EnglishBreakfast";
    public string Password2 = "EarlGrey";
    public bool IsPasswordCorrect = false;
    public bool IsPasswordPin = false;


    // Members
    // ------------------------------------------------------------

    private bool _client = true;
    private NetworkManagerCustom _manager;


    // Unity Methods
    // ------------------------------------------------------------

    private void Start() 
    {
        _manager = ObjectFinder.Find<NetworkManagerCustom>();

        var scene = _manager.Scene;
        var host = _manager.Host;
        var role = Configuration.Get("network-role", "").ToLower();

        if (role == "client")
            ToggleClient();
        else
            ToggleHost();

        HostIP.text = host;
        HostIPTextPlaceholder.text = host;

        if (scene == NetworkManagerCustom.BigSubScene)
            ToggleBigSub();
        else if (scene == NetworkManagerCustom.GliderScene)
            ToggleGlider();
        else if (scene == NetworkManagerCustom.DccScene)
            ToggleDCC();
        else
            ToggleBigSub();

        if (IsPasswordCorrect)
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

        if (_manager.AutoStart)
            StartButtonObj.SetActive(false);
	}


    // Public Methods
    // ------------------------------------------------------------

    public void ToggleHost()
    {
        _client = false;
        HostIPObject.SetActive(false);
        LocalIP.SetActive(true);
        HostButtonImg.color = Color.white;
        ClientButtonImg.color = Color.grey;
        thisIP.text = Network.player.ipAddress;
    }

    public void ToggleClient()
    {
        _client = true;
        HostIPObject.SetActive(true);
        LocalIP.SetActive(false);
        HostButtonImg.color = Color.grey;
        ClientButtonImg.color = Color.white;
    }

    public void ToggleGlider()
    {
        GliderButtonImg.color = Color.white;
        BigSubButtonImg.color = Color.grey;
        DCCButtonImg.color = Color.grey;
        _manager.Scene = NetworkManagerCustom.GliderScene;
    }

    public void ToggleBigSub()
    {
        GliderButtonImg.color = Color.grey;;
        BigSubButtonImg.color = Color.white;;
        DCCButtonImg.color = Color.grey;
        _manager.Scene = NetworkManagerCustom.BigSubScene;
    }

    public void ToggleDCC()
    {
        GliderButtonImg.color = Color.grey;;
        BigSubButtonImg.color = Color.grey;;
        DCCButtonImg.color = Color.white;
        _manager.Scene = NetworkManagerCustom.DccScene;
    }

    public void PasswordLiveInput(string value)
    {
        if (IsPasswordPin)
            PasswordInputChanged(value);
    }

    public void PasswordInputChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
            return;

        if (value == Password)
        {
            IsPasswordCorrect = true;
            if(ZhangLogo)
            {
                ZhangLogo.SetActive(true);
                PasswordRoot.SetActive(false);
            }
            StartButtonObj.SetActive(true);
        }
        else if (value == Password2)
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

    public void HostIPChanged(string value)
    {
        if (!string.IsNullOrEmpty(value))
            _manager.Host = value;
    }

    public void StartButton()
    {
        if (_client)
            _manager.StartClient();
        else
            _manager.StartServer();
    }
}
