using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class debugDCCScreensUi : MonoBehaviour
{

    public InputField StationIdInput;
    public Text StationName;

    public Transform StationContainer;
    public debugDCCStationUi StationUiPrefab;

    private void Start()
    {
        PopulateStations();
    }

    private void OnEnable()
    {
        StationIdInput.text = DCCScreenData.StationId.ToString();
        StationIdInput.onEndEdit.AddListener(UpdateStationId);
        StationName.text = DCCScreenData.GetStationName(DCCScreenData.StationId);
    }

    public void UpdateStationId(string value)
    {
        Debug.Log("debugDisplaySettingsUi.UpdateStationId() - Updating station id to: " + value);
        DCCScreenData.SetStationId(value);

        // Station id might have been clamped to a valid id.
        StationIdInput.text = DCCScreenData.StationId.ToString();
        StationName.text = DCCScreenData.GetStationName(DCCScreenData.StationId);
    }

    public void PreviousStation()
    {
        DCCScreenData.SetStationId(DCCScreenData.StationId - 1);
        StationIdInput.text = DCCScreenData.StationId.ToString();
        StationName.text = DCCScreenData.GetStationName(DCCScreenData.StationId);
    }

    public void NextStation()
    {
        DCCScreenData.SetStationId(DCCScreenData.StationId + 1);
        StationIdInput.text = DCCScreenData.StationId.ToString();
        StationName.text = DCCScreenData.GetStationName(DCCScreenData.StationId);
    }

    private void PopulateStations()
    {
        for (var i = 0; i < 5; i++)
            AddStation(i);
    }

    private void AddStation(int stationId)
    {
        var ui = Instantiate(StationUiPrefab);
        ui.transform.SetParent(StationContainer, false);
        ui.StationId = stationId;
    }

}
