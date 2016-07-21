using UnityEngine;
using System.Collections;
using Meg.Networking;

public class InstrumentsTextValues : MonoBehaviour
{
    public widgetText ScientificDataValues;
    public widgetText OperatingDataValues;
    public widgetText AcousticNavigationDataValues;

    public float updateTick = 0.2f;
    private float nextUpdate = 0;

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
        ScientificDataValues.Text = GetScientificValues();
        OperatingDataValues.Text = GetOperatingDataValues();
        AcousticNavigationDataValues.Text = GetAcousticNavigationDataValues();
    }

    private string GetScientificValues()
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

    private string GetOperatingDataValues()
    {
        /*
        // TODO: Implement.
        var hydraulicPressure = 0;
        var ballastAirPressure = 0;
        var variableBallastTemp = 0;
        var variableBallastPressure = 0;

        return string.Format("{0:N0}", hydraulicPressure) + "\n"
               + string.Format("{0:N0}", ballastAirPressure) + "\n"
               + string.Format("{0:N0}", variableBallastTemp) + "\n"
               + string.Format("{0:N0}", variableBallastPressure) + "\n";
        */

        return "-" + "\n"
               + "-" + "\n"
               + "-" + "\n"
               + "-" + "\n";
    }

    private string GetAcousticNavigationDataValues()
    {
        // TODO: Implement.
        var playerVessel = serverUtils.GetPlayerVessel();
        var playerPos = serverUtils.GetVesselPosition(playerVessel);
        var latlong = serverUtils.GetVesselLatLong(playerVessel);
        var playerX = playerPos.x * 1000;
        var playerY = playerPos.y * 1000;
        var targetVessel = serverUtils.GetTargetVessel();
        var hasTarget = targetVessel > 0;
        var targetPos = serverUtils.GetVesselPosition(targetVessel);
        var targetRange = serverUtils.GetVesselDistance(playerVessel, targetVessel);
        var targetX = targetPos.x * 1000;
        var targetY = targetPos.y * 1000;
        var targetBearing = serverUtils.GetVesselBearing(targetVessel);

        // Avoid bearing readout flip-flopping from 360 to 0 when intercepting.
        if (targetBearing > 359)
            targetBearing = 0;

        return string.Format("{0:N0}", playerX) + "\n"
            + string.Format("{0:N0}", playerY) + "\n"
            + serverUtils.FormatLatitude(latlong.y, 1) + "\n"
            + serverUtils.FormatLongitude(latlong.x, 1) + "\n"
            + (hasTarget ? string.Format("{0:N0}", targetBearing) : "-") + "\n"
            + (hasTarget ? string.Format("{0:N0}", targetRange) : "-") + "\n"
            + (hasTarget ? string.Format("{0:N0}", targetX) : "-") + "\n"
            + (hasTarget ? string.Format("{0:N0}", targetY) : "-") + "\n";
    }

}
