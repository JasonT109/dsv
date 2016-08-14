using UnityEngine;
using System.Collections;
using DG.Tweening;
using Meg.Maths;
using Meg.Networking;

public class SonarPing : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Label for this ping. */
    public widgetText NameLabel;

    /** Label for depth information. */
    public widgetText DepthLabel;

    /** Ping indicator. */
    public Renderer Indicator;

    /** Interval for position updates. */
    public float PositionUpdateInterval = 0;

    /** Interval for depth updates. */
    public float DepthUpdateInterval = 0;

    /** Auto-pulse interval. */
    public float AutoPulseInterval = 0;

    /** Parent sonar ping manager. */
    public SonarPings Pings { get; set; }

    /** Vessel represented by this ping. */
    public vesselData.Vessel Vessel { get; set; }


    // Private Properties
    // ------------------------------------------------------------

    /** Vessel data. */
    private vesselData VesselData
        { get { return serverUtils.VesselData; } }



    // Members
    // ------------------------------------------------------------

    /** Timestamp for next position update. */
    private float _nextPositionUpdate;

    /** Timestamp for next depth update. */
    private float _nextDepthUpdate;

    /** Indicator's normal color. */
    private Color _indicatorColor;

    /** Indicator's normal scale. */
    private Vector3 _indicatorScale;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Awake()
    {
        if (Indicator)
        {
            _indicatorScale = Indicator.transform.localScale;
            _indicatorColor = Indicator.material.GetColor("_TintColor");
            Indicator.gameObject.SetActive(false);
        }
    }

    /** Enabling. */
    private void OnEnable()
    {
        if (AutoPulseInterval > 0)
            StartCoroutine(AutoPulseRoutine(AutoPulseInterval));
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Configure ping. */
    public void Configure(SonarPings pings)
    {
        Pings = pings;
        transform.SetParent(Pings.Root, false);
    }

    /** Updating. */
    public void Refresh()
    {
        var p = VesselData.GetSonarPosition(Vessel.Id, Pings.Type);
        var player = VesselData.PlayerVessel;
        var visible = (Vessel.OnSonar || !Pings.HideIfNotOnSonar)
            && (p.magnitude <= 1 || !Pings.HideIfOutOfRange)
            && (Vessel.Id != player || !Pings.HideIfPlayer);

        gameObject.SetActive(visible);
        transform.rotation = Quaternion.identity;

        var t = Time.time;
        if (t > _nextPositionUpdate)
        {
            transform.localPosition = Pings.VesselToPingSpace(Vessel);
            _nextPositionUpdate = t + PositionUpdateInterval;
        }

        NameLabel.Text = Vessel.Name.ToUpper();

        if (t > _nextDepthUpdate)
        {
            var delta = Vessel.Depth - VesselData.GetDepth(player);
            var rounded = graphicsMaths.roundToInterval(delta, 5);
            DepthLabel.Text = string.Format("{0}{1:n0}m", rounded > 0 ? "+" : "", rounded);
            _nextDepthUpdate = t + DepthUpdateInterval;
        }
    }

    /** Convert ping's position into vessel space. */
    public Vector3 ToVesselSpace()
        { return LocalToVesselSpace(Vector3.zero); }

    /** Convert a position in ping's local space into vessel space. */
    public Vector3 LocalToVesselSpace(Vector3 local)
        { return Pings.PingToVesselSpace(transform.localPosition + local); }

    /** Pulse this ping's visual indicator. */
    public void Pulse()
    {
        if (!Indicator)
            return;

        Indicator.gameObject.SetActive(true);
        Indicator.transform.localScale = Vector3.zero;
        Indicator.transform.DOScale(_indicatorScale, 0.5f);
        Indicator.material.SetColor("_TintColor", _indicatorColor);
        Indicator.material.DOColor(new Color(0,0,0,0), "_TintColor", 0.5f).SetDelay(0.25f);
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Automatic pulsing routine. */
    private IEnumerator AutoPulseRoutine(float interval)
    {
        var wait = new WaitForSeconds(interval);
        while (gameObject.activeSelf)
        {
            Pulse();
            yield return wait;
        }
        
    }


}
