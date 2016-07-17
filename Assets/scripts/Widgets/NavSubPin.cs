using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Meg.Networking;
using Vectrosity;


public class NavSubPin : MonoBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** Intercept line width. */
    private const float InterceptLineWidth = 3;

    /** Interval between trail points. */
    private const float TrailInterval = 1.0f;

    /** Maximum length of trail. */
    private const int TrailLength = 30;

    /** Trail line width. */
    private const float TrailLineWidth = 3;


    // Properties
    // ------------------------------------------------------------

    /** Vessel that this pin represents. */
    public int VesselId;

    /** Visual indicator for height above ocean floor. */
    public GameObject vesselHeightIndicator;

    /** Pressable button for this pin. */
    public GameObject vesselButton;

    /** 3D representation of the vessel. */
    public GameObject vesselModel;

    /** Intercept line color. */
    public Color InterceptLineColor;

    /** Distance to ocean floor. */
    public float Distance;


    // Private Properties
    // ------------------------------------------------------------

    private float LineXOffset
    { get { return _manager.lineXOffset; } }

    private Vector2 ImageSize
    { get { return _manager.imageSize; } }

    
    // Members
    // ------------------------------------------------------------

    /** The pin manager. */
    private NavSubPins _manager;

    /** Raycast result, used to locate ocean floor. */
    private RaycastHit _hit;

    /** Interception line. */
    private VectorLine _interceptLine;

    /** Trail line. */
    private VectorLine _trailLine;

    /** Trail colors. */
    private readonly List<Color32> _trailColors = new List<Color32>();

    /** Timestamp for next trail point. */
    private float _nextTrailTime;

    /** List of previous positions. */
    private readonly  List<Vector3> _history = new List<Vector3>();

    /** Last known movement mode for this pin. */
    private vesselMovement _movement;

    /** Vessel button control. */
    private buttonControl _vesselButtonControl;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        _manager = GetComponentInParent<NavSubPins>();
        _vesselButtonControl = vesselButton.GetComponentInChildren<buttonControl>();
    }

    /** Enabling. */
    private void OnEnable()
    {
        _interceptLine = VectorLine.SetLine(InterceptLineColor, new Vector3[2]);
        _interceptLine.lineWidth = InterceptLineWidth;
    }

    /** Disabling. */
    private void OnDisable()
    {
        VectorLine.Destroy(ref _interceptLine);
        VectorLine.Destroy(ref _trailLine);
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Updating. */
    public void UpdatePin()
    {
        // Update pin visibility.
        var visible = serverUtils.GetVesselVis(VesselId);
        vesselButton.SetActive(visible);
        vesselHeightIndicator.SetActive(visible);

        // Get vessel's server position and apply that to the vessel model.
        Vector3 position = new Vector3();
        float velocity;
        serverUtils.GetVesselData(VesselId, out position, out velocity);
        vesselModel.transform.localPosition = ConvertVesselCoords(position);

        // Get position in map space and position button there.
        var mapPos = ConvertToMapSpace(vesselModel.transform.position);
        vesselButton.transform.localPosition = mapPos;

        // Cast a ray down to the terrain from the original position.
        if (Physics.Raycast(vesselModel.transform.position, -Vector3.up, out _hit))
            Distance = _hit.distance;

        // Update height indicator.
        if (Distance > 0)
        {
            // Set the position of the height indicators to be at ground level
            var groundPos = ConvertToMapSpace(_hit.point);
            vesselHeightIndicator.transform.localPosition = groundPos;

            // Set the x position to be exactly the same as button plus offset
            vesselHeightIndicator.transform.localPosition =
                new Vector3(mapPos.x + LineXOffset,
                    vesselHeightIndicator.transform.localPosition.y,
                    vesselHeightIndicator.transform.localPosition.z + 1);
        }

        // Update the height indicator's length.
        float vesselHeight = mapPos.y - vesselHeightIndicator.transform.localPosition.y;
        vesselHeightIndicator.GetComponent<graphicsSlicedMesh>().Height = vesselHeight;
        vesselHeightIndicator.GetComponent<Renderer>()
            .material.SetTextureScale("_MainTex", new Vector2(1, 4 * vesselHeight));

        // Update map icon with a direction indicator.
        UpdateMapIconDirection();
        
    }

    /** Update indicator lines for this vessel pin. */
    public void UpdateIndicators()
    {
        var movement = serverUtils.GetVesselMovements().GetVesselMovement(VesselId);
        var intercept = movement as vesselIntercept;

        // Update the interception indicator (if any).
        UpdateInterceptIndicator(intercept);

        // Update the trail indicator.
        UpdateTrail(movement);

        // Remember movement mode.
        _movement = movement;
    }


    // Private Methods
    // ------------------------------------------------------------

    private void UpdateMapIconDirection()
    {
        // 0 no direction, 1 left, 2 upleft, 3 up, 4 up right, 5 right, 6 down right, 7 down, 8 down left
        int mapIconDirection = 0;
        var local = vesselButton.transform.localPosition;
        var x = ImageSize.x;
        var y = ImageSize.y;

        // Check to see if child is at edge of the map.
        if (local.x == -x && local.y == -y)
            mapIconDirection = 8;
        else if (local.x == -x && local.y == y)
            mapIconDirection = 2;
        else if (local.x == -x)
            mapIconDirection = 1;

        if (local.x == x && local.y == -y)
            mapIconDirection = 6;
        else if (local.x == x && local.y == y)
            mapIconDirection = 4;
        else if (local.x == x)
            mapIconDirection = 5;

        if (local.y == y && local.x != x && local.x != -x)
            mapIconDirection = 3;
        if (local.y == -y && local.x != x && local.x != -x)
            mapIconDirection = 7;

        // Set the orientation of the child to indicate the direction.
        vesselButton.GetComponent<graphicsMapIcon>().atBounds = mapIconDirection != 0;
        vesselButton.GetComponent<graphicsMapIcon>().direction = mapIconDirection;
    }

    /** Convert a vessel's position into 2D map space. */
    private Vector2 ConvertToMapSpace(Vector3 p)
        { return _manager.ConvertToMapSpace(p); }

    /** Convert a vessel's position into 3D map space. */
    private Vector3 ConvertVesselCoords(Vector3 p)
        { return _manager.ConvertVesselCoords(p); }

    /** Update the interception indicator for this vessel (if intercepting). */
    private void UpdateInterceptIndicator(vesselIntercept intercept)
    {
        // Check if this vessel is performing an interception.
        _interceptLine.active = intercept != null;
        if (intercept == null)
            return;

        // Locate interception pin.
        var interceptPin = _manager.GetVesselPin(intercept.TargetVessel);

        // Get interception locations.
        var from = vesselButton.transform.position;
        var to = interceptPin.vesselButton.transform.position;

        // Update interception indicator.
        _interceptLine.points3[0] = new Vector3(from.x, from.y, from.z + 2);
        _interceptLine.points3[1] = new Vector3(to.x, to.y, to.z + 2);
        _interceptLine.Draw3D();
    }

    /** Update the trail indicator for this pin. */
    private void UpdateTrail(vesselMovement movement)
    {
        // Clear history when movement changes.
        if (movement != _movement)
            _history.Clear();

        // Update trail (if moving).
        if (movement && Time.time > _nextTrailTime)
        {
            _nextTrailTime = Time.time + TrailInterval;
            _history.Add(vesselModel.transform.position);
            if (_history.Count > TrailLength)
                _history.RemoveAt(0);
        }

        // Check if we need to update the trail.
        if (_history.Count == 0)
            return;

        // Ensure there is a trail line.
        if (_trailLine == null)
            _trailLine = new VectorLine("Trail", new List<Vector3>(), 1, LineType.Continuous);

        // Populate trail points for this frame.
        _trailLine.points3.Clear();
        for (var i = 0; i < _history.Count; i++)
            _trailLine.points3.Add(GetTrailPoint(_history[i]));
        _trailLine.points3.Add(GetTrailPoint(vesselModel.transform.position));

        // Update trail colors;
        Color32 c = _vesselButtonControl.colorTheme[3];
        var nColors = _trailLine.points3.Count - 1;
        _trailColors.Clear();
        for (var i = 0; i < nColors; i++)
        {
            var a = (byte) ((i / (float) nColors) * c.a * 0.5f);
            _trailColors.Add(new Color32(c.r, c.g, c.b, a));
        }
        _trailLine.SetColors(_trailColors);

        // Draw trail.
        _trailLine.lineWidth = TrailLineWidth;
        _trailLine.Draw3D();
    }

    /** Convert a trail point into map screen space for this frame. */
    private Vector3 GetTrailPoint(Vector3 p)
    {
        // Locate the point in current map screen space.
        var space = vesselButton.transform.parent;
        var point = space.TransformPoint(ConvertToMapSpace(p));

        // Push point back in Z so it appears beneath other UI elements.
        point.z += 5;

        return point;
    }

}
