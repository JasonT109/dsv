using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class SonarRays : MonoBehaviour
{

    public SonarRangeControl RangeControl;
    public SonarGainControl GainControl;

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

    private Material _material;

    void Start()
	{
	    _material = Rays.GetComponent<Renderer>().material;
    }
	
	void Update()
	{
	    var range = (float) RangeControl.Range;
	    var r = (range - RangeControl.MinRange) / (RangeControl.MaxRange - RangeControl.MinRange);

        var lowX = Mathf.Lerp(RayTilingRangeLowX.x, RayTilingRangeLowX.y, r);
        var lowY = Mathf.Lerp(RayTilingRangeLowY.x, RayTilingRangeLowY.y, r);
        var highX = Mathf.Lerp(RayTilingRangeHighX.x, RayTilingRangeHighX.y, r);
        var highY = Mathf.Lerp(RayTilingRangeHighY.x, RayTilingRangeHighY.y, r);

        var lowScale = new Vector3(lowX, lowY, 0);
        var highScale = new Vector3(highX, highY, 0);

	    _material.SetTextureScale("_LowTexture", lowScale);
        _material.SetTextureScale("_HighTexture", highScale);

        var gain = (float)GainControl.Gain;

        var lowColor = _material.GetVector("_LowColor");
        lowColor.z = GainLowColorZ.Evaluate(gain);
        _material.SetVector("_LowColor", lowColor);

        var highColor = _material.GetVector("_HighColor");
        highColor.z = GainHighColorZ.Evaluate(gain);
        _material.SetVector("_HighColor", highColor);

	    var intensity = GainBloomIntensity.Evaluate(gain);
	    var threshold = GainBloomThreshold.Evaluate(gain);

        Bloom.bloomIntensity = intensity;
	    Bloom.bloomThreshold = threshold;
	}
}
