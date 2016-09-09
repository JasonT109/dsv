using UnityEngine;
using System.Collections;
using Meg.Networking;
using UnityStandardAssets.ImageEffects;

public class SonarRays : MonoBehaviour
{
    public SonarData.Type Type = SonarData.Type.ShortRange;

    public GameObject Rays;
    public Bloom Bloom;

    public Vector2 RayTilingRangeLowX;
    public Vector2 RayTilingRangeLowY;
    public Vector2 RayTilingRangeHighX;
    public Vector2 RayTilingRangeHighY;

    public AnimationCurve GainLowColorZ;
    public AnimationCurve GainHighColorZ;
    public AnimationCurve GainBloomIntensity;
    public AnimationCurve GainBloomThreshold;

    public AnimationCurve SensitivityBloomIntensity;
    public AnimationCurve SensitivityBloomThreshold;

    public Vector2 WobbleIntensity = Vector2.zero;
    public Vector2 WobbleRate = Vector2.one;

    private Material _material;
    private SonarData.Config _config;

    void Start()
	{
        if (Rays)
	        _material = Rays.GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (!serverUtils.IsReady())
            return;

        if (_material)
            UpdateMaterial();

        if (Bloom)
            UpdateBloom();
	}

    void UpdateMaterial()
    {
        if (_config == null)
            _config = serverUtils.SonarData.GetConfigForType(Type);

        var range = _config.Range;
        var gain = _config.Gain;
        var r = (range - _config.MinRange) / (_config.MaxRange - _config.MinRange);

        var lowX = Mathf.Lerp(RayTilingRangeLowX.x, RayTilingRangeLowX.y, r);
        var lowY = Mathf.Lerp(RayTilingRangeLowY.x, RayTilingRangeLowY.y, r);
        var highX = Mathf.Lerp(RayTilingRangeHighX.x, RayTilingRangeHighX.y, r);
        var highY = Mathf.Lerp(RayTilingRangeHighY.x, RayTilingRangeHighY.y, r);

        var lowScale = new Vector3(lowX, lowY, 0);
        var highScale = new Vector3(highX, highY, 0);

        _material.SetTextureScale("_LowTexture", lowScale);
        _material.SetTextureScale("_HighTexture", highScale);

        var wobbleIntensity = Mathf.Lerp(WobbleIntensity.x, WobbleIntensity.y, r);
        var wobbleRate = Mathf.Lerp(WobbleRate.x, WobbleRate.y, r);
        var wobble = Mathf.Sin(Time.time * wobbleRate) * wobbleIntensity;

        var lowColor = _material.GetVector("_LowColor");
        lowColor.z = GainLowColorZ.Evaluate(gain) * (1 + wobble);
        _material.SetVector("_LowColor", lowColor);

        var highColor = _material.GetVector("_HighColor");
        highColor.z = GainHighColorZ.Evaluate(gain) * (1 + wobble);
        _material.SetVector("_HighColor", highColor);
    }

    void UpdateBloom()
    {
        var sensitivity = _config.Sensitivity;
        var intensity = SensitivityBloomIntensity.Evaluate(sensitivity);
        var threshold = SensitivityBloomThreshold.Evaluate(sensitivity);

        Bloom.bloomIntensity = intensity;
        Bloom.bloomThreshold = threshold;
    }
}
