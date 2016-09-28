using UnityEngine;
using System.Collections;
using DG.Tweening;
using Meg.Maths;
using Meg.Networking;
using UnityEngine.UI;

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
    public GameObject Indicator;

    /** Rotation root (for keeping ping upright on screen. */
    public Transform Rotator;

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

    /** Depth offset used to control this ping's depth order (optional). */
    public float DepthOffset { get; set; }

    /** Color for this ping. */
    public Color Color
    {
        get
        {
            return Pings.Space == SonarPings.DisplaySpace.Sonar
                ? Vessel.ColorOnSonar
                : Vessel.ColorOnMap;
        }
    }

    /** Whether ping is visible. */
    public bool Visible
    {
        get
        {
            return Pings.Space == SonarPings.DisplaySpace.Sonar
                ? Vessel.OnSonar
                : Vessel.OnMap;
        }
    }


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

            var r = Indicator.GetComponent<MeshRenderer>();
            var g = Indicator.GetComponent<Graphic>();
            if (r)
                _indicatorColor = r.material.GetColor("_TintColor");
            else if (g)
                _indicatorColor = g.color;

            Indicator.gameObject.SetActive(false);
        }

        if (!Rotator)
            Rotator = transform;
    }

    /** Enabling. */
    private void OnEnable()
    {
        if (AutoPulseInterval > 0)
            StartCoroutine(AutoPulseRoutine(AutoPulseInterval));
    }

    /** Disabling. */
    private void OnDisable()
    {
        StopAllCoroutines();
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
            && (Vessel.OnMap || !Pings.HideIfNotOnMap)
            && (p.magnitude <= 1 || !Pings.HideIfOutOfRange)
            && (Vessel.Id != player || !Pings.HideIfPlayer);

        gameObject.SetActive(visible);
        Rotator.rotation = Quaternion.identity;

        var t = Time.time;
        if (t > _nextPositionUpdate)
        {
            transform.localPosition = Pings.VesselToPingSpace(Vessel) + new Vector3(0, 0, DepthOffset);
            _nextPositionUpdate = t + PositionUpdateInterval;
        }

        NameLabel.Text = Vessel.Name.ToUpper();
        if (t > _nextDepthUpdate && DepthLabel)
        { 
            UpdateDepth();
            _nextDepthUpdate = t + DepthUpdateInterval;
        }
    }

    /** Convert ping's position into vessel space. */
    public Vector3 ToVesselSpace()
        { return LocalToVesselSpace(Vector3.zero); }

    /** Convert a position in ping's local space into vessel space. */
    public Vector3 LocalToVesselSpace(Vector3 local)
    {
        var v = Pings.PingToVesselSpace(transform.localPosition + local);
        v.z = Vessel.Depth;
        return v;
    }

    /** Start or stop auto-pulse indicator. */
    public void AutoPulse(float interval)
    {
        if (Mathf.Approximately(AutoPulseInterval, interval))
            return;

        AutoPulseInterval = interval;
        StopAllCoroutines();
        if (AutoPulseInterval > 0)
            StartCoroutine(AutoPulseRoutine(interval));
    }

    /** Pulse this ping's visual indicator. */
    public void Pulse()
    {
        if (!Indicator)
            return;

        Indicator.gameObject.SetActive(true);
        Indicator.transform.localScale = Vector3.zero;
        Indicator.transform.DOScale(_indicatorScale, 0.5f);

        var r = Indicator.GetComponent<MeshRenderer>();
        var g = Indicator.GetComponent<CanvasGroup>();
        if (r)
        {
            r.material.SetColor("_TintColor", _indicatorColor);
            r.material.DOColor(new Color(0, 0, 0, 0), "_TintColor", 0.5f).SetDelay(0.25f);
        }
        else if (g)
        {
            g.alpha = 1;
            g.DOFade(0, 0.5f).SetDelay(0.25f);
        }
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

    /** Update the ping's depth display. */
    private void UpdateDepth()
    {
        if (Pings.RelativeDepth)
        {
            var player = VesselData.PlayerVessel;
            var delta = Vessel.Depth - VesselData.GetDepth(player);
            var rounded = graphicsMaths.roundToInterval(delta, 5);
            DepthLabel.Text = string.Format("{0}{1:n0}m", rounded > 0 ? "+" : "", rounded);
        }
        else
        {
            DepthLabel.Text = string.Format("{0:n0}m", Vessel.Depth);
        }
    }


}
