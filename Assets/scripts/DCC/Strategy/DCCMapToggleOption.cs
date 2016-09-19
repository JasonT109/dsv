using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Meg.Networking;

public class DCCMapToggleOption : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]
    public DCCStrategyMap StrategyMap;
    public Button Button;

    [Header("Configuration")]
    public string ServerParameter;
    public mapData.Mode MapMode;


    // Members
    // ------------------------------------------------------------

    /** Toggle indicator. */
    private Graphic _on;


    // Unity Methods
    // ------------------------------------------------------------

    private void Start()
	{
	    if (!Button)
	        Button = GetComponent<Button>();

        Button.onClick.AddListener(ToggleOption);
        _on = Button.GetComponentInChildrenNotMe<Image>(true);
        StrategyMap.OnMapModeChanged += OnMapModeChanged;

        UpdateVisibility();
        Update();
    }


    // Private Methods
    // ------------------------------------------------------------

    private void Update()
        { _on.gameObject.SetActive(serverUtils.GetServerBool(ServerParameter)); }

    private void ToggleOption()
    {
        var wasOn = serverUtils.GetServerBool(ServerParameter);
        serverUtils.PostServerData(ServerParameter, wasOn ? 0 : 1);
    }

    private void OnMapModeChanged(mapData.Mode oldmode, mapData.Mode newmode)
        { UpdateVisibility(); }

    private void UpdateVisibility()
        { gameObject.SetActive(StrategyMap.Mode == MapMode); }

}
