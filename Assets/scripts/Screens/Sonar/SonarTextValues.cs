using UnityEngine;
using Meg.Networking;

public class SonarTextValues : MonoBehaviour
{
    public widgetText DataValues;
    public float updateTick = 0.2f;

    private float nextUpdate;

    void Start()
    {
        nextUpdate = Time.time;
        UpdateValues();
    }

    void OnEnable()
    {
        UpdateValues();
    }

    void Update()
    {
        if (Time.time > nextUpdate)
            UpdateValues();
    }

    private void UpdateValues()
    {
        DataValues.Text = GetDataValues();
    }

    private string GetDataValues()
    {
        var precisionDepth = serverUtils.GetServerData("depth");
        var nonPrecisionDepth = Mathf.RoundToInt(serverUtils.GetServerData("depth") / 10) * 10;
        var altitude = serverUtils.GetServerData("floorDistance");
        var medianAltitude = serverUtils.GetServerData("floorDistance");
        var verticalRate = serverUtils.GetServerData("verticalVelocity") * Conversions.MetresPerSecondToMetersPerMin;
        var heading = serverUtils.GetServerData("heading");
        var pitch = serverUtils.GetServerData("pitchAngle");
        var roll = serverUtils.GetServerData("rollAngle");
        var waterTemp = clientCalcValues.Instance.waterTempResult;

        return string.Format("{0:N0}", precisionDepth) + "\n"
               + string.Format("{0:N0}", nonPrecisionDepth) + "\n"
               + string.Format("{0:N0}", altitude) + "\n"
               + string.Format("{0:N0}", medianAltitude) + "\n"
               + string.Format("{0:N0}", verticalRate) + "\n"
               + string.Format("{0:N0}", heading) + "\n"
               + string.Format("{0:N0}", pitch) + "\n"
               + string.Format("{0:N0}", roll) + "\n"
               + string.Format("{0:N0}", waterTemp) + "\n";
    }

}
