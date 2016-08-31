using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class debugDisplaySettingsUi : MonoBehaviour
{

    public InputField StationIdInput;

    private void OnEnable()
    {
        StationIdInput.text = DCCScreenData.StationId.ToString();
        StationIdInput.onEndEdit.AddListener(UpdateStationId);
    }

    public void UpdateStationId(string value)
    {
        Debug.Log("debugDisplaySettingsUi.UpdateStationId() - Updating station id to: " + value);
        DCCScreenData.SetStationId(value);

        // Station id might have been clamped to a valid id.
        StationIdInput.text = DCCScreenData.StationId.ToString();
    }

}
