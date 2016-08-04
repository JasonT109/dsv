using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PasswordScreen : MonoBehaviour 
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    public CanvasGroup ConnectRoot;

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

    public GameObject StartButtonObj;
    public Text StartButtonText;


    [Header("Configuraiton")]

    public Color SelectedColor = Color.white;
    public Color UnselectedColor = Color.grey;

    public float DefaultFadeDelay = 1.5f;
    public float AutoStartFadeDelay = 5.0f;


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

        if (role == "host" || role == "server")
            ToggleHost();
        else
            ToggleClient();

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

        var isAutoStarting = _manager.AutoStart;
        if (isAutoStarting)
            StartButtonObj.SetActive(false);

        // Fade in the UI after a brief delay.
        var delay = isAutoStarting ? AutoStartFadeDelay : DefaultFadeDelay;
        ConnectRoot.DOFade(0.0f, 1.0f).From().SetDelay(delay);
        ConnectRoot.transform.DOScale(0.0f, 0.5f).From().SetDelay(delay);
    }


    // Public Methods
    // ------------------------------------------------------------

    public void ToggleHost()
    {
        _client = false;
        HostIPObject.SetActive(false);
        LocalIP.SetActive(true);
        HostButtonImg.color = SelectedColor;
        ClientButtonImg.color = UnselectedColor;
        thisIP.text = Network.player.ipAddress;
        StartButtonText.text = "START HOST";
    }

    public void ToggleClient()
    {
        _client = true;
        HostIPObject.SetActive(true);
        LocalIP.SetActive(false);
        HostButtonImg.color = UnselectedColor;
        ClientButtonImg.color = SelectedColor;
        StartButtonText.text = "CONNECT TO HOST";
    }

    public void ToggleGlider()
    {
        GliderButtonImg.color = SelectedColor;
        BigSubButtonImg.color = UnselectedColor;
        DCCButtonImg.color = UnselectedColor;
        _manager.SetScene(NetworkManagerCustom.GliderScene);
    }

    public void ToggleBigSub()
    {
        GliderButtonImg.color = UnselectedColor;
        BigSubButtonImg.color = SelectedColor;
        DCCButtonImg.color = UnselectedColor;
        _manager.SetScene(NetworkManagerCustom.BigSubScene);
    }

    public void ToggleDCC()
    {
        GliderButtonImg.color = UnselectedColor;
        BigSubButtonImg.color = UnselectedColor;
        DCCButtonImg.color = SelectedColor;
        _manager.SetScene(NetworkManagerCustom.DccScene);
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
