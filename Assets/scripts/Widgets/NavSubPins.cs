using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meg.Maths;
using Meg.Networking;

public class NavSubPins : Singleton<NavSubPins>
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** The transform in which to place pin buttons and other 2D elements. */
    public Transform VesselButtonRoot;

    /** The transform in which to place stats boxes. */
    public Transform StatsBoxRoot;


    [Header("Configuration")]

    public Vector2 ViewportSize = new Vector2(1920, 1080);
    public Vector2 imageSize = new Vector2(5, 5);
    public float lineXOffset = -0.1f;
    public bool isGliderMap = false;

    [Header("Prefabs")]

    public NavSubPin PinPrefab;

    public Camera MapCamera
        { get { return _mapCamera; } }

    // Members
    // ------------------------------------------------------------

    private readonly List<NavSubPin> _pins = new List<NavSubPin>();
    private readonly Dictionary<int, NavSubPin> _pinLookup = new Dictionary<int, NavSubPin>();
    private Camera _mapCamera;
    private Vector3 _initialScale;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        _initialScale = transform.localScale;

        // Add pre-existing pins to the manager.
        var pins = GetComponentsInChildren<NavSubPin>().ToList();
        foreach (var pin in pins)
            RegisterPin(pin);

        // Locate the map camera.
        ResolveMapCamera();

        // Check that we have everything configured properly.
        if (!VesselButtonRoot)
            Debug.LogError("NavSubPins.Start(): Missing a reference to vessel buttons root transform.");
        if (!StatsBoxRoot)
            Debug.LogError("NavSubPins.Start(): Missing a reference to stats boxes root transform.");
    }


    // Public Methods
    // ------------------------------------------------------------

    public void ToggleLabels()
    {
        for (var i = 0; i < _pins.Count; i++)
            _pins[i].ToggleLabel();
    }

    public bool HasLabels
        { get { return _pins.Exists(pin => pin.ShowLabel); } }

    /** Locate a pin by the vessel id it represents. */
    public NavSubPin GetVesselPin(int vessel)
    {
        NavSubPin pin;
        _pinLookup.TryGetValue(vessel, out pin);
        return pin;
    }

    public Vector3 ConvertToPinSpace(Vector3 position)
    {
        // Project from map camera into viewport.
        var c = _mapCamera.WorldToViewportPoint(position);

        // Convert into pin's parent viewport space.
        var extents = ViewportSize * 0.005f;
        var x = graphicsMaths.remapValue(c.x, 0, 1, -extents.x, extents.x);
        var y = graphicsMaths.remapValue(c.y, 0, 1, -extents.y, extents.y);
        var z = c.z * 0.001f;

        return new Vector3(x, y, z);
    }

    public float GetVesselFloorDistance(int vessel)
    {
        if (_pins == null || vessel < 1 || vessel >= _pins.Count)
            return 0;

        return _pins[vessel - 1].GetFloorDistance();
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Resolve map camera components. */
    private void ResolveMapCamera()
    {
        if (_mapCamera)
            return;

        var root = GameObject.Find("MapRoot");
        if (root)
            _mapCamera = root.GetComponentInChildren<Camera>();
        if (!_mapCamera && Map.Instance)
            _mapCamera = Map.Instance.Camera;
    }

    /** Create a new pin at runtime. */
    private NavSubPin CreatePin()
    {
        // Create a new pin instance.
        var pin = Instantiate(PinPrefab);
        if (!pin)
            return null;

        // Configure the pin (create UI button, height indicator, etc.)
        pin.transform.SetParent(transform, false);
        pin.Configure(_pins.Count + 1);

        // Add pin to the manager.
        RegisterPin(pin);

        return pin;
    }

    /** Register a pin with the manager. */
    private void RegisterPin(NavSubPin pin)
    {
        // Check if we already have this pin in place.
        if (!pin || _pins.Contains(pin) || _pinLookup.ContainsKey(pin.VesselId))
            return;
        
        // Add the pin to tracking data structures.
        _pins.Add(pin);
        _pinLookup[pin.VesselId] = pin;
    }

    private void Update()
    {
        // Update scaling of the pin model root to compensate for display scaling.
        // This will be the case when scaling up from 16:10 to 16:9, for example.
        UpdateScale();
    }

    private void LateUpdate()
    {
        // Update the navigation pin positions, etc.
        UpdatePins();
    }

    private void UpdateScale()
    {
        // Adjust local scale to account for camera distortion.
        var s = Camera.main.transform.localScale;
        var sx = _initialScale.x / s.x;
        var sy = _initialScale.y / s.y;
        var sz = _initialScale.z / s.z;

        transform.localScale = new Vector3(sx, sy, sz);
    }

    private void UpdatePins()
    {
        // Check if server is ready.
        if (!serverUtils.IsReady())
            return;

        // Create new pins if necessary.
        var vesselCount = serverUtils.VesselData.VesselCount;
        for (var i = _pins.Count; i < vesselCount; i++)
            CreatePin();

        // Update each pin's position, etc.
        for (var i = 0; i < _pins.Count; i++)
            _pins[i].UpdatePin();

        // Update indicators that span between pins. (e.g. intercept lines).
        for (var i = 0; i < _pins.Count; i++)
            _pins[i].UpdateIndicators();
    }

}
