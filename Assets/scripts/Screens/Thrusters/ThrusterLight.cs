using UnityEngine;
using System.Collections;

public class ThrusterLight : MonoBehaviour
{

    public widgetThrusterControl Control;
    public widgetThrusterControl.ThrusterId Thruster;

    public float MinIntensity = 0;
    public float MaxIntensity = 2.5f;

    public Light Light;

    public float SmoothTime = 0.5f;

    private float _level;
    private float _levelVelocity;

    public void Start()
    {
        if (!Light)
            Light = GetComponent<Light>();

        Light.color = Color.black;
        Light.intensity = 0;
    }

    public void Update()
    {
        var target = Mathf.Abs(Control.GetThrusterLevel(Thruster)) * 0.01f;
        _level = Mathf.SmoothDamp(_level, target, ref _levelVelocity, SmoothTime);

        Light.color = Control.LightGradient.Evaluate(_level);
        Light.intensity = Mathf.Lerp(MinIntensity, MaxIntensity, _level);
    }

}
