using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    private const float TrailLineWidth = 5;

    /** Pin position smoothing time. */
    private const float SmoothTime = 0.2f;


    // Properties
    // ------------------------------------------------------------

    [Header("Configuration")]

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

    /** Whether to display pin's label. */
    public bool ShowLabel = true;

    /** Whether to orient pin to face direction of movement. */
    public bool OrientToHeading;


    [Header("Prefabs")]

    /** Prefab for a vessel height indicator. */
    public GameObject HeightIndicatorPrefab;

    /** Prefab for a vessel pin button. */
    public GameObject ButtonPrefab;

    /** Stats box prefab. */
    public GameObject StatsBoxPrefab;


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

    /** Map camera. */
    private Camera _mapCamera;

    /** Icon. */
    private graphicsMapIcon _icon;

    /** Current position, smoothed. */
    private Vector3 _position;

    /** Current position smoothing velocity. */
    private Vector3 _velocity;

    /** The stats box for this pin. */
    private GameObject _statsBox;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        // If this pin is already set up for a vessel, configure it.
        // Otherwise, assume that pin manager will do that explicitly.
        if (VesselId > 0)
            Configure(VesselId);
    }

    /** Enabling. */
    private void OnEnable()
    {
    }

    /** Disabling. */
    private void OnDisable()
    {
        VectorLine.Destroy(ref _interceptLine);
        VectorLine.Destroy(ref _trailLine);

        _position = Vector3.zero;
        _velocity = Vector3.zero;
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Configure a pin. */

    public void Configure(int id)
    {
        // Update pin's id.
        VesselId = id;

        // Try to get hold of the pin manager.
        _manager = NavSubPins.Instance;

        // Complain if manager lookup failed.
        if (_manager == null)
        {
            Debug.LogError("Failed to locate nav pin manager.");
            return;
        }

        // Get hold of all dependencies and objects we need.
        if (!_mapCamera)
            _mapCamera = GameObject.Find("MapRoot").GetComponentInChildren<Camera>();

        if (!vesselModel)
            vesselModel = gameObject;

        if (!vesselButton)
        {
            vesselButton = Instantiate(ButtonPrefab);
            vesselButton.transform.SetParent(_manager.VesselButtonRoot, false);
        }

        if (!vesselHeightIndicator)
        {
            vesselHeightIndicator = Instantiate(HeightIndicatorPrefab);
            vesselHeightIndicator.transform.SetParent(_manager.VesselButtonRoot, false);
        }

        if (vesselButton && !_vesselButtonControl)
        {
            _vesselButtonControl = vesselButton.GetComponentInChildren<buttonControl>();
            _vesselButtonControl.buttonGroup = _manager.VesselButtonRoot.gameObject;
            _vesselButtonControl.buttonGroup.GetComponent<buttonGroup>()
                .Add(_vesselButtonControl.gameObject);
        }

        if (!_icon)
            _icon = vesselButton.GetComponent<graphicsMapIcon>();

        // Set up button handler for stats boxes.
        _vesselButtonControl.onPress.RemoveListener(OnButtonPressed);
        _vesselButtonControl.onPress.AddListener(OnButtonPressed);

        /*
        // Create a stats box if needed.
        if (VesselId <= vesselData.BaseVesselCount)
        {
            _statsBox = CreateStatsBox();
            _vesselButtonControl.visGroup = _statsBox;
        }
        */
    }

    /** Handle the map pin button being pressed. */
    private void OnButtonPressed()
    {
        if (!_statsBox)
        {
            _statsBox = CreateStatsBox();
            _vesselButtonControl.visGroup = _statsBox;
        }

        _statsBox.gameObject.SetActive(true);
    }

    /** Toggle pin's label. */
    public void ToggleLabel()
    {
        // Toggle label visibility.
        ShowLabel = !ShowLabel;

        // Fade the label in/out.
        var label = _icon.label;
        var c = label.color;
        var to = new Color(c.r, c.g, c.g, ShowLabel ? 1 : 0);
        label.DOKill();
        DOTween.To(() => label.color, x => label.color = x, to, 0.25f);

        // Fade the label's backdrop in/out.
        if (label.transform.childCount <= 0)
            return;

        var backdrop = label.transform.GetChild(0).GetComponentInChildren<MeshRenderer>();
        if (!backdrop)
            return;

        backdrop.DOKill();
        backdrop.material.DOFade(ShowLabel ? 0.5f : 0, "_TintColor", 0.25f);
    }

    /** Updating. */
    public void UpdatePin()
    {
        // Check if pin is configured correctly.
        if (!_manager)
            return;

        // Update pin visibility.
        var visible = serverUtils.GetVesselVis(VesselId);
        vesselButton.SetActive(visible);
        vesselHeightIndicator.SetActive(visible);

        // Get vessel's server position and apply that to the vessel model.
        var position = serverUtils.GetVesselPosition(VesselId);

        // Apply smoothing to the position.
        if (Mathf.Approximately(_position.sqrMagnitude, 0))
            _position = position;
        else
            _position = Vector3.SmoothDamp(_position, position, ref _velocity, SmoothTime);

        vesselModel.transform.localPosition = ConvertVesselCoords(_position);

        // Get position in map space and position button there.
        var mapPos = ConvertToMapScreenSpace(vesselModel.transform.position);
        vesselButton.transform.localPosition = mapPos;

        // Cast a ray down to the terrain from the original position.
        if (Physics.Raycast(vesselModel.transform.position, -Vector3.up, out _hit))
            Distance = _hit.distance;

        // Update height indicator.
        if (Distance > 0)
        {
            // Set the position of the height indicators to be at ground level
            var groundPos = ConvertToMapScreenSpace(_hit.point);
            vesselHeightIndicator.transform.localPosition = groundPos;

            // Set the x position to be exactly the same as button plus offset
            vesselHeightIndicator.transform.localPosition =
                new Vector3(mapPos.x + LineXOffset,
                    vesselHeightIndicator.transform.localPosition.y,
                    vesselHeightIndicator.transform.localPosition.z + 0.1f);
        }

        // Update the height indicator's length.
        float vesselHeight = mapPos.y - vesselHeightIndicator.transform.localPosition.y;
        vesselHeightIndicator.GetComponent<graphicsSlicedMesh>().Height = vesselHeight;
        vesselHeightIndicator.GetComponent<Renderer>()
            .material.SetTextureScale("_MainTex", new Vector2(1, 4 * vesselHeight));

        // Update pin colors. 
        UpdateColor();

        // Update map icon with a direction indicator.
        UpdateMapIcon();
        
    }

    /** Update indicator lines for this vessel pin. */
    public void UpdateIndicators()
    {
        // Check if pin is configured correctly.
        if (!_manager)
            return;

        // Set the correct camera for lines in nav screen.
        VectorLine.SetCamera3D(_mapCamera);

        var movement = serverUtils.GetVesselMovement(VesselId);
        var intercept = movement as vesselIntercept;

        // Update the interception indicator (if any).
        UpdateInterceptIndicator(intercept);

        // Update the trail indicator.
        UpdateTrail(movement);

        // Update button orientation.
        if (OrientToHeading)
            UpdateButtonOrientation(movement);

        // Remember movement mode.
        _movement = movement;
    }

    /** Return the floor distance for this vessel. */
    public float GetFloorDistance()
        { return Distance; }


    // Private Methods
    // ------------------------------------------------------------

    private void UpdateColor()
    {
        var color = serverUtils.VesselData.GetColorOnMap(VesselId);
        var theme = _vesselButtonControl.colorTheme;

        theme[1] = color;
        theme[2] = HSBColor.FromColor(color).Brighten(0.75f).ToColor();
        theme[3] = color;
        theme[4] = color;
    }

    private void UpdateMapIcon()
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
        _icon.VesselId = VesselId;
        _icon.UpdateIcon(mapIconDirection != 0, mapIconDirection);
    }

    /** Convert a vessel's position into map screenspace. */
    private Vector3 ConvertToMapScreenSpace(Vector3 p)
        { return _manager.ConvertToMapScreenSpace(p); }

    /** Convert a vessel's position into 3D map space. */
    private Vector3 ConvertVesselCoords(Vector3 p)
        { return _manager.ConvertVesselCoords(p); }

    /** Convert a vessel's position into 2D screen space. */
    private Vector3 ConvertVesselToScreenSpace(Vector3 p)
    {
        var map = ConvertToMapScreenSpace(p);
        var world = vesselButton.transform.parent.TransformPoint(map);
        return Camera.main.WorldToScreenPoint(world);
    }

    /** Update the interception indicator for this vessel (if intercepting). */
    private void UpdateInterceptIndicator(vesselIntercept intercept)
    {
        // Ensure intercept line exists.
        if (_interceptLine == null)
        { 
            _interceptLine = VectorLine.SetLine(InterceptLineColor, new Vector3[2]);
            _interceptLine.lineWidth = InterceptLineWidth;
            _interceptLine.layer = gameObject.layer;
        }

        // Check if we should draw the intercept indicator.
        var visible = serverUtils.GetVesselVis(VesselId);
        var interceptIsVisible = serverUtils.GetVesselVis(vesselData.InterceptId);
        _interceptLine.active = intercept != null && visible && interceptIsVisible;
        if (!_interceptLine.active)
            return;

        // Locate interception pin.
        var interceptPin = _manager.GetVesselPin(intercept.TargetVessel);

        // Get interception locations.
        var from = vesselModel.transform.position;
        var to = interceptPin.vesselModel.transform.position;

        // Update interception indicator.
        _interceptLine.points3[0] = new Vector3(from.x, from.y, from.z);
        _interceptLine.points3[1] = new Vector3(to.x, to.y, to.z);
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

        // Ensure there is a trail line.
        if (_trailLine == null)
        {
            _trailLine = new VectorLine("Trail", new List<Vector3>(), 1, LineType.Continuous);
            _trailLine.lineWidth = TrailLineWidth;
            _trailLine.layer = gameObject.layer;
        }

        // Check if we should draw the trail.
        var visible = serverUtils.GetVesselVis(VesselId) && movement;
        _trailLine.active = visible && _history.Count > 0;
        if (!_trailLine.active)
            return;

        // Populate trail points for this frame.
        _trailLine.points3.Clear();
        for (var i = 0; i < _history.Count; i++)
            _trailLine.points3.Add(_history[i]);
        _trailLine.points3.Add(vesselModel.transform.position);

        // Update trail colors;
        Color32 c = _vesselButtonControl.GetThemeColor(3);
        var nColors = _trailLine.points3.Count - 1;
        _trailColors.Clear();
        for (var i = 0; i < nColors; i++)
        {
            var a = (byte) ((i / (float) nColors) * c.a * 0.25f);
            _trailColors.Add(new Color32(c.r, c.g, c.b, a));
        }
        _trailLine.SetColors(_trailColors);

        // Draw trail.
        _trailLine.Draw3D();
    }

    /** Update the orientation of the button to face towards movement direction. */
    private void UpdateButtonOrientation(vesselMovement movement)
    {
        // If not moving, orient upwards.
        if (!movement || movement.Velocity.magnitude < 0.01f)
            return;

        // Get direction of movement in screen space.
        var p = vesselModel.transform.position;
        var v = movement.WorldVelocity;
        var from = ConvertVesselToScreenSpace(p);
        var to = ConvertVesselToScreenSpace(p + v);
        var delta = to - from;

        var angle = -(90 + Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg);
        _icon.button.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    /** Create a stats box for this pin. */
    private GameObject CreateStatsBox()
    {
        var statsBox = Instantiate(StatsBoxPrefab);
        statsBox.transform.SetParent(_manager.StatsBoxRoot, false);

        // Set up server data to use the correct vessel id.
        var overrides = statsBox.GetComponent<linkDataOverrides>();
        overrides.Overrides[0].replacement = string.Format("vessel{0}", VesselId);
        overrides.Apply();

        // Ensure box follows the pin button.
        var transition = statsBox.GetComponent<widgetTransition>();
        transition.parentObject = vesselButton;

        // Disable the box so it's initially hidden.
        statsBox.gameObject.SetActive(false);

        return statsBox;
    }

}
