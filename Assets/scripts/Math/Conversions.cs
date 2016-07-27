using UnityEngine;
using System.Collections;

public class Conversions
{

    // Percentage
    // ------------------------------------------------------------

    public const float PercentToPartsPerMillion = 10000;
    public const float PartsPerMillionToPercent = 1 / PercentToPartsPerMillion;


    // Speed
    // ------------------------------------------------------------

    public const float KphToKnots = 0.5399568f;
    public const float KnotsToKph = 1 / KphToKnots;

    public const float MetresPerSecondToKph = 3.6f;
    public const float KphToMetresPerSecond = 1 / MetresPerSecondToKph;

    public const float MetresPerSecondToKnots = 1.943846f;
    public const float KnotsToMetresPerSecond = 1 / MetresPerSecondToKnots;

    public const float MetresPerSecondToMetersPerMin = 60;
    public const float MetersPerMinToMetresPerSecond = 1 / MetresPerSecondToMetersPerMin;


    // Distance
    // ------------------------------------------------------------

    public const double EarthRadius = 6378137.0;


    // Pressure
    // ------------------------------------------------------------

    public const float BarToPsi = 14.5038f;
    public const float PsiToBar = 1 / BarToPsi;

}
