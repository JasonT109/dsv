using System.Collections.Generic;
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
    public Image MMBButtonImg;
    public Image EvacButtonImg;
    public Image StrategyButtonImg;
    public Image ROVButtonImg;

    public GameObject StartButtonObj;
    public Text StartButtonText;

    public Graphic InitialSpinner;
    public Graphic ConnectingSpinner;

    public CanvasGroup StationRoot;
    public InputField StationIdInput;
    public Text StationName;


    [Header("Configuration")]

    public Color SelectedColor = Color.white;
    public Color UnselectedColor = Color.grey;

    public float DefaultFadeDelay = 1.5f;
    public float AutoStartFadeDelay = 5.0f;


    // Members
    // ------------------------------------------------------------

    private bool _client = true;
    private NetworkManagerCustom _manager;
    private readonly List<Image> _sceneButtonImages = new List<Image>();


    // Unity Methods
    // ------------------------------------------------------------

    private void Start() 
    {
        Debug.Log("PasswordScreen.Start()");
        _manager = ObjectFinder.Find<NetworkManagerCustom>();

        _sceneButtonImages.Add(GliderButtonImg);
        _sceneButtonImages.Add(BigSubButtonImg);
        _sceneButtonImages.Add(DCCButtonImg);
        _sceneButtonImages.Add(MMBButtonImg);
        _sceneButtonImages.Add(EvacButtonImg);
        _sceneButtonImages.Add(StrategyButtonImg);
        _sceneButtonImages.Add(ROVButtonImg);

        var scene = PlayerPrefs.GetString("DefaultScene", _manager.Scene);
        var host = PlayerPrefs.GetString("DefaultHost", _manager.Host);
        var stationId = PlayerPrefs.GetString("DefaultStationId", "0");

        var role = Configuration.Get("network-role", "").ToLower();
        if (role == "host" || role == "server")
            ToggleHost();
        else
            ToggleClient();

        HostIP.text = host;
        HostIPTextPlaceholder.text = host;
        _manager.Host = host;

        if (scene == NetworkManagerCustom.BigSubScene)
            ToggleBigSub();
        else if (scene == NetworkManagerCustom.GliderScene)
            ToggleGlider();
        else if (scene == NetworkManagerCustom.DccScene)
            ToggleDCC();
        else if (scene == NetworkManagerCustom.MmbScene)
            ToggleMMB();
        else if (scene == NetworkManagerCustom.EvacScene)
            ToggleEvac();
        else if (scene == NetworkManagerCustom.StrategyTableScene)
            ToggleStrategy();
        else if (scene == NetworkManagerCustom.RovScene)
            ToggleROV();
        else
            ToggleBigSub();

        // Initialize DCC station id.
        UpdateStationId(stationId);
        StationIdInput.text = DCCScreenData.StationId.ToString();
        StationIdInput.onEndEdit.AddListener(UpdateStationId);
        StationName.text = DCCScreenData.GetStationName(DCCScreenData.StationId);

        var isAutoStarting = _manager.AutoStart;
        if (isAutoStarting)
            StartButtonObj.SetActive(false);

        // Fade in the UI after a brief delay.
        var delay = isAutoStarting ? AutoStartFadeDelay : DefaultFadeDelay;
        ConnectRoot.DOFade(0.0f, 1.0f).From().SetDelay(delay);
        ConnectRoot.transform.DOScale(0.0f, 0.5f).From().SetDelay(delay);
        InitialSpinner.gameObject.SetActive(true);
        InitialSpinner.DOFade(0.0f, 0.25f).SetDelay(delay);

        ConnectingSpinner.gameObject.SetActive(false);
    }


    // Public Methods
    // ------------------------------------------------------------

    public void ToggleHost()
    {
        Debug.Log("PasswordScreen.ToggleHost() - Configuring UI for Host mode.");
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
        Debug.Log("PasswordScreen.ToggleClient() - Configuring UI for Client mode.");
        _client = true;
        HostIPObject.SetActive(true);
        LocalIP.SetActive(false);
        HostButtonImg.color = UnselectedColor;
        ClientButtonImg.color = SelectedColor;
        StartButtonText.text = "CONNECT TO HOST";
    }

    public void ToggleGlider()
    {
        Debug.Log("PasswordScreen.ToggleGlider() - Configuring UI for Glider mode.");
        ResetSceneSelection();
        GliderButtonImg.color = SelectedColor;
        SetScene(NetworkManagerCustom.GliderScene);
    }

    public void ToggleBigSub()
    {
        Debug.Log("PasswordScreen.ToggleBigSub() - Configuring UI for main sub mode.");
        ResetSceneSelection();
        BigSubButtonImg.color = SelectedColor;
        SetScene(NetworkManagerCustom.BigSubScene);
    }

    public void ToggleDCC()
    {
        Debug.Log("PasswordScreen.ToggleDCC() - Configuring UI for DCC mode.");
        ResetSceneSelection();
        DCCButtonImg.color = SelectedColor;
        StationRoot.gameObject.SetActive(true);
        SetScene(NetworkManagerCustom.DccScene);
    }

    public void ToggleMMB()
    {
        Debug.Log("PasswordScreen.ToggleBigSub() - Configuring UI for Medical Bay (MMB) mode.");
        ResetSceneSelection();
        MMBButtonImg.color = SelectedColor;
        SetScene(NetworkManagerCustom.MmbScene);
    }

    public void ToggleEvac()
    {
        Debug.Log("PasswordScreen.ToggleEvac() - Configuring UI for Evac Ship.");
        ResetSceneSelection();
        EvacButtonImg.color = SelectedColor;
        SetScene(NetworkManagerCustom.EvacScene);
    }

    public void ToggleStrategy()
    {
        Debug.Log("PasswordScreen.ToggleStrategy() - Configuring UI for Strategy Table.");
        ResetSceneSelection();
        StrategyButtonImg.color = SelectedColor;
        SetScene(NetworkManagerCustom.StrategyTableScene);
    }

    public void ToggleROV()
    {
        Debug.Log("PasswordScreen.ToggleROV() - Configuring UI for ROV.");
        ResetSceneSelection();
        ROVButtonImg.color = SelectedColor;
        SetScene(NetworkManagerCustom.RovScene);
    }

    private void ResetSceneSelection()
    {
        StationRoot.gameObject.SetActive(false);
        foreach (var image in _sceneButtonImages)
            image.color = UnselectedColor;
    }

    public void UpdateStationId(string value)
    {
        Debug.Log("PasswordScreen.UpdateStationId() - Updating station id to: " + value);
        DCCScreenData.SetInitialStationId(value);

        // Station id might have been clamped to a valid id.
        StationIdInput.text = DCCScreenData.StationId.ToString();
        StationName.text = DCCScreenData.GetStationName(DCCScreenData.StationId);
    }

    public void PreviousStation()
    {
        DCCScreenData.SetInitialStationId(DCCScreenData.StationId - 1);
        StationIdInput.text = DCCScreenData.StationId.ToString();
        StationName.text = DCCScreenData.GetStationName(DCCScreenData.StationId);
    }

    public void NextStation()
    {
        DCCScreenData.SetInitialStationId(DCCScreenData.StationId + 1);
        StationIdInput.text = DCCScreenData.StationId.ToString();
        StationName.text = DCCScreenData.GetStationName(DCCScreenData.StationId);
    }

    public void HostIPChanged(string value)
    {
        Debug.Log("IP Address changed: " + value);
        if (!string.IsNullOrEmpty(value))
            _manager.Host = value;
    }

    public void StartButton()
    {
        var mode = _client ? "client" : "server";
        Debug.Log(string.Format("PasswordScreen.StartButton() - Starting up in {0} mode.", mode));

        // Save out selections so we can set them as defaults next time around.
        PlayerPrefs.SetString("DefaultScene", _manager.Scene);
        PlayerPrefs.SetString("DefaultHost", _manager.Host);
        PlayerPrefs.SetString("DefaultStationId", DCCScreenData.StationId.ToString());

        if (_client)
            _manager.StartClient();
        else
            _manager.StartServer();

        ConnectingSpinner.gameObject.SetActive(true);
        ConnectingSpinner.DOKill();
        ConnectingSpinner.DOFade(0.0f, 0.25f).From();
    }

    private void SetScene(string scene)
    {
        _manager.SetScene(scene);
    }

}
