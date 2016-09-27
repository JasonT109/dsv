using UnityEngine;
using System.Collections;
using Meg.Networking;

public class widgetGliderDescentValues : MonoBehaviour
{

    public widgetThrusterControl thrusterControl;
    public widgetText flowText;
    public widgetText powerText;
    public widgetText stabilizerText;
    public widgetText angleText;
    public widgetText descentText;
    public float maxFlowRate = 640f;
    private float[] sidePower = new float[2];
    public float updateTick = 0.2f;
    private float updateTime = 0.2f;
    public ParticleSystem p1;
    public ParticleSystem p2;
    public float particleBaseSpeed = 1;

    private ParticleSystem.EmissionModule p1Emission;
    private ParticleSystem.EmissionModule p2Emission;

    void updateValues()
    {
        if (!thrusterControl)
            thrusterControl = ObjectFinder.Find<widgetThrusterControl>();

        float powerValue = thrusterControl.GetThrusterLevel(widgetThrusterControl.ThrusterId.MainShared);
        powerText.Text = Mathf.Abs(powerValue).ToString("N0");

        float velocity = serverUtils.GetServerData("velocity");
        descentText.Text = velocity.ToString("N0");

        sidePower[0] = thrusterControl.GetThrusterLevel(widgetThrusterControl.ThrusterId.SideL1);
        sidePower[1] = thrusterControl.GetThrusterLevel(widgetThrusterControl.ThrusterId.SideR1);

        float stabPower = Mathf.Abs(Mathf.Max(sidePower));
        stabilizerText.Text = stabPower.ToString("N0");

        float pitchAngle = serverUtils.GetServerData("pitchAngle");
        angleText.Text = pitchAngle.ToString("N0") + "°";

        float flowRate = maxFlowRate * powerValue;
        UpdateParticles(flowRate, powerValue);

        flowText.Text = flowRate.ToString("N2");
    }

    private void UpdateParticles(float flowRate, float powerValue)
    {
        if (!p1 || !p2)
            return;

        if (powerValue != 0)
        {
            //flowRate += Mathf.Sin(0.01f * Time.time);
            p1Emission.rate = new ParticleSystem.MinMaxCurve(100);
            p2Emission.rate = new ParticleSystem.MinMaxCurve(100);
            p1.startSpeed = flowRate * particleBaseSpeed;
            p1.startSpeed = flowRate * particleBaseSpeed;
            p2.startSpeed = flowRate * particleBaseSpeed;
            p2.startSpeed = flowRate * particleBaseSpeed;
        }
        else
        {
            p1Emission.rate = new ParticleSystem.MinMaxCurve(0);
            p2Emission.rate = new ParticleSystem.MinMaxCurve(0);
        }
    }

    void OnEnable()
    {
        p1Emission = p1.emission;
        p2Emission = p2.emission;
        updateValues();
    }

    void Start()
    {
        p1Emission = p1.emission;
        p2Emission = p2.emission;
    }

	// Update is called once per frame
	void Update ()
    {
        if (Time.time > updateTime)
        {
            updateTime = Time.time + updateTick;
            updateValues();
        }
    }
}
