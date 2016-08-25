using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Meg.Networking;
using Vectrosity;

public class debugVesselMarker : MonoBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** Scaling factor for velocity line. */
    private const float VelocityLineScale = 0.5f;

    /** Minimum velocity line length. */
    private const float VelocityLineMinLength = 10;
    
    /** Minimum velocity line length. */
    private const float VelocityLineMaxLength = 60;

    /** Interval between trail points. */
    private const float TrailInterval = 1.0f;

    /** Maximum length of trail. */
    private const int TrailLength = 60;


    // Properties
    // ------------------------------------------------------------

    /** The vessel this marker represents. */
    public int Vessel;

    /** Name of this marker. */
    public string Name;

    /** Whether this marker is selected. */
    public bool Selected;

    /** Default marker color. */
    public Color DefaultColor;

    /** Default text color. */
    public Color DefaultTextColor;

    /** Intercept line color. */
    public Color InterceptLineColor;

    /** Trail line color. */
    public Color TrailLineColor;

    /** Intercept marker. */
    public debugVesselMarker InterceptMarker;


    // Computed Properties
    // ------------------------------------------------------------

    /** Whether this marker represents the player vessel. */
    public bool IsPlayer
    { get { return Vessel == serverUtils.GetPlayerVessel(); } }

    /** Whether this marker represents a visible vessel. */
    public bool IsVesselVisible
    { get { return serverUtils.GetVesselVis(Vessel); } }

    /** Returns the vessel movements module. */
    private vesselMovements Movements
        { get { return serverUtils.GetVesselMovements(); } }

    /** Returns the marked vessel's movement, if any. */
    private vesselMovement Movement
        { get { return Movements.GetVesselMovement(Vessel); } }


    // Members
    // ------------------------------------------------------------

    /** Marker material. */
    private MeshRenderer _marker;

    /** Text mesh for this marker. */
    private TextMesh _text;

    /** Text renderer for this marker. */
    private Renderer _textRenderer;

    /** Interception line. */
    private VectorLine _interceptLine;

    /** Heading line. */
    private VectorLine _headingLine;

    /** Trail line. */
    private VectorLine _trailLine;

    /** Trail colors. */
    private readonly List<Color32> _trailColors = new List<Color32>();

    /** Timestamp for next trail point. */
    private float _nextTrailTime;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start ()
    {
        _marker = GetComponent<MeshRenderer>();
        _text = GetComponentInChildren<TextMesh>();
        _textRenderer = _text.GetComponent<Renderer>();
    }

    /** Enabling. */
    private void OnEnable()
    {
        _interceptLine = VectorLine.SetLine(InterceptLineColor, new Vector2[2]);
        _headingLine = new VectorLine("Heading", new List<Vector2>(6), 1, LineType.Discrete);
    }

    /** Disabling. */
    private void OnDisable()
    {
        VectorLine.Destroy(ref _interceptLine);
        VectorLine.Destroy(ref _headingLine);
        VectorLine.Destroy(ref _trailLine);
    }

    /** Updating. */
    private void LateUpdate()
    {
        // Update marker colors.
        var markerColor = GetMarkerColor();
        var textColor = GetTextColor();
        _marker.material.color = markerColor;
        _text.text = Name;
        _textRenderer.material.color = textColor;

        // Update interception indicator.
        _interceptLine.active = InterceptMarker != null;
        var markerPosition = ToScreen(transform.position);
        _interceptLine.points2[0] = markerPosition;
        if (InterceptMarker)
            _interceptLine.points2[1] = ToScreen(InterceptMarker.transform.position);
        _interceptLine.Draw();

        // Update heading indicator.
        var velocity = Movement ? Movement.Velocity : Vector3.zero;
        var heading = velocity * VelocityLineScale
            + velocity.normalized * VelocityLineMinLength;
        if (heading.magnitude > VelocityLineMaxLength)
            heading = heading.normalized * VelocityLineMaxLength;

        Vector2 headingEnd = markerPosition + heading;
        Vector2 perp = new Vector2(-heading.y, heading.x).normalized;
        Vector2 back = -heading.normalized;

        _headingLine.color = markerColor;
        _headingLine.active = velocity.magnitude > 0;
        _headingLine.points2[0] = markerPosition;
        _headingLine.points2[1] = headingEnd;
        _headingLine.points2[2] = headingEnd;
        _headingLine.points2[3] = headingEnd + back * 10 + perp * 5;
        _headingLine.points2[4] = headingEnd;
        _headingLine.points2[5] = headingEnd + back * 10 - perp * 5;
        _headingLine.Draw();

        // Update trail (if moving).
        if (Movement && Time.time > _nextTrailTime)
        {
            _nextTrailTime = Time.time + TrailInterval;
            if (_trailLine == null)
                _trailLine = new VectorLine("Trail", new List<Vector2>(), 1, LineType.Continuous);

            _trailLine.points2.Add(markerPosition);
            if (_trailLine.points2.Count > TrailLength)
                _trailLine.points2.RemoveAt(0);

            // Fade out trail over time;
            Color32 c = TrailLineColor;
            var nColors = _trailLine.points2.Count - 1;
            _trailColors.Clear();
            for (var i = 0; i < nColors; i++)
            {
                var a = (byte) ((i / (float) nColors) * c.a);
                _trailColors.Add(new Color32(c.r, c.g, c.b, a));
            }

            _trailLine.SetColors(_trailColors);
        }

        if (_trailLine != null)
            _trailLine.Draw();
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Reset this marker. */
    public void Reset()
    {
        // Clear the marker trail.
        if (_trailLine != null)
            _trailLine.points2.Clear();
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Return a screenspace position, given a worldspace one. */
    private Vector3 ToScreen(Vector3 world)
        { return Camera.main.WorldToScreenPoint(world); }

    /** Current marker color. */
    private Color GetMarkerColor()
    {
        var playerColor = serverUtils.GetColorTheme().highlightColor;
        var color = IsPlayer ? playerColor : DefaultColor;

        if (Selected)
            color = Color.Lerp(color, Color.white, 0.5f);

        if (!IsVesselVisible)
            color *= 0.25f;

        return color;
    }

    /** Current marker text color. */
    private Color GetTextColor()
    {
        var playerColor = serverUtils.GetColorTheme().highlightColor;

        var color = IsPlayer ? playerColor : DefaultTextColor;
        if (Selected)
            color = Color.Lerp(color, Color.white, 0.5f);

        if (!IsVesselVisible)
            color.a = 0.5f;

        return color;
    }

}
