using UnityEngine;
using System.Collections;

public class SonarRays : MonoBehaviour
{

    public SonarRangeControl RangeControl;
    public SonarGainControl GainControl;

    public GameObject Rays;

    public Vector2 RayTilingRangeLowX;
    public Vector2 RayTilingRangeLowY;
    public Vector2 RayTilingRangeHighX;
    public Vector2 RayTilingRangeHighY;

    public AnimationCurve GainLowColorZ;
    public AnimationCurve GainHighColorZ;

    private Material _material;

    private const float RangeSmoothTime = 0.0f;
    private const float GainSmoothTime = 0.0f;

    private Vector3 _lowScale;
    private Vector3 _lowScaleVelocity;

    private Vector3 _highScale;
    private Vector3 _highScaleVelocity;

    private float _lowColorZ;
    private float _lowColorZVelocity;

    private float _highColorZ;
    private float _highColorZVelocity;

    void Start()
	{
	    _material = Rays.GetComponent<Renderer>().material;

        _lowScale = new Vector3(RayTilingRangeLowX.x, RayTilingRangeLowY.x);
        _highScale = new Vector3(RayTilingRangeHighX.x, RayTilingRangeHighY.x);
        _lowColorZ = _material.GetVector("_LowColor").z;
        _highColorZ = _material.GetVector("_HighColor").z;
    }
	
	void Update()
	{
	    var range = (float) RangeControl.Range;
	    var r = (range - RangeControl.MinRange) / (RangeControl.MaxRange - RangeControl.MinRange);

        var lowX = Mathf.Lerp(RayTilingRangeLowX.x, RayTilingRangeLowX.y, r);
        var lowY = Mathf.Lerp(RayTilingRangeLowY.x, RayTilingRangeLowY.y, r);
        var highX = Mathf.Lerp(RayTilingRangeHighX.x, RayTilingRangeHighX.y, r);
        var highY = Mathf.Lerp(RayTilingRangeHighY.x, RayTilingRangeHighY.y, r);

        var lowTarget = new Vector3(lowX, lowY, 0);
        var highTarget = new Vector3(highX, highY, 0);

	    _lowScale = Vector3.SmoothDamp(_lowScale, lowTarget, ref _lowScaleVelocity, RangeSmoothTime);
        _highScale = Vector3.SmoothDamp(_highScale, highTarget, ref _highScaleVelocity, RangeSmoothTime);

        _material.SetTextureScale("_LowTexture", _lowScale);
        _material.SetTextureScale("_HighTexture", _highScale);

        var gain = (float)GainControl.Gain;

        var lowColor = _material.GetVector("_LowColor");
        var lowColorTarget = GainLowColorZ.Evaluate(gain);
        _lowColorZ = Mathf.SmoothDamp(_lowColorZ, lowColorTarget, ref _lowColorZVelocity, GainSmoothTime);
	    lowColor.z = _lowColorZ;
        _material.SetVector("_LowColor", lowColor);

        var highColor = _material.GetVector("_HighColor");
        var highColorTarget = GainHighColorZ.Evaluate(gain);
        _highColorZ = Mathf.SmoothDamp(_highColorZ, highColorTarget, ref _highColorZVelocity, GainSmoothTime);
        highColor.z = _highColorZ;
        _material.SetVector("_HighColor", highColor);

    }
}
