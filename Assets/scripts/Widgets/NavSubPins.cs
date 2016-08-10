using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Meg.Maths;
using Meg.Networking;

public class NavSubPins : Singleton<NavSubPins>
{

    public float seaFloor = 10994f;
    public float heightMul = 2.0f;
    public float MapSize;
    public Vector2 imageSize = new Vector2(5, 5);
    public float distanceScale = 0.01f;
    public float lineXOffset = -0.1f;

    private NavSubPin[] _pins;
    private readonly Dictionary<int, NavSubPin> _pinLookup = new Dictionary<int, NavSubPin>();
    private Camera _mapCamera;

    private Vector3 _initialScale;

    private void Start()
    {
        _initialScale = transform.localScale;
        _pins = GetComponentsInChildren<NavSubPin>();
        foreach (var pin in _pins)
            _pinLookup[pin.VesselId] = pin;

        var mapRoot = GameObject.Find("MapRoot");
        _mapCamera = mapRoot.GetComponentInChildren<Camera>(true);
    }

    private void Update()
    {
        // Update scaling of the pin model root to compensate for display scaling.
        // This will be the case when scaling up from 16:10 to 16:9, for example.
        UpdateScale();
    }

    private void LateUpdate()
    {
        UpdatePins();
    }

    private void UpdateScale()
    {
        var s = Camera.main.transform.localScale;
        var sx = _initialScale.x / s.x;
        var sy = _initialScale.y / s.y;
        var sz = _initialScale.z / s.z;

        transform.localScale = new Vector3(sx, sy, sz);
    }

    private void UpdatePins()
    {
        for (var i = 0; i < _pins.Length; i++)
            _pins[i].UpdatePin();

        for (var i = 0; i < _pins.Length; i++)
            _pins[i].UpdateIndicators();
    }

    public void ToggleLabels()
    {
        for (var i = 0; i < _pins.Length; i++)
            _pins[i].ToggleLabel();
    }

    public NavSubPin GetVesselPin(int vessel)
        { return _pinLookup[vessel]; }

    public Vector2 ConvertToMap2DSpace(Vector3 position)
    {
        // Project from map camera into viewport.
        var c = _mapCamera.WorldToViewportPoint(position);

        // Convert into 2D map space.
        Vector2 p;
        p.x = Mathf.Clamp((c.x * 10.0f) - 5.0f, -imageSize.x, imageSize.x);
        p.y = Mathf.Clamp((c.y * 10.0f) - 5.0f, -imageSize.y, imageSize.y);

        return p;
    }

    public Vector3 ConvertVesselCoords(Vector3 position)
    {
        Vector3 p;

        //"normalise" the coordinate system used in the crew data map
        //hardcoded with the size of the crew map size
        p.x = (position.x + 5.0f) / 10.0f;
        p.y = ((seaFloor - position.z) / 1000.0f) * heightMul;
        p.z = (position.y + 5.0f) / 10.0f;

        //convert the normalised coordinates to map to the nav map
        //hardcoded with a displacement to the nav map
        p.x = p.x * MapSize + 130.0f;
        p.z = p.z * MapSize + -25.0f;

        return(p);
    }

    public float GetVesselFloorDistance(int vessel)
    {
        if (_pins == null|| vessel < 1 || vessel >= _pins.Length)
            return 0;

        return _pins[vessel - 1].GetFloorDistance();
    }

}
