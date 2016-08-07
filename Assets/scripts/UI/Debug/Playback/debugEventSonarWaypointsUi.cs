using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Meg.EventSystem;

public class debugEventSonarWaypointsUi : MonoBehaviour, IPointerClickHandler
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Remove waypoint button. */
    public Button RemoveButton;

    /** Clear waypoints button. */
    public Button ClearButton;


    [Header("Prefabs")] 

    /** Visual indicator for a sonar waypoint. */
    public debugEventSonarWaypointUi WaypointPrefab;


    [Header("Coordinates")]

    /** Sonar waypoint cartesian space. */
    public Graphic WaypointSpace;

    /** Sonar radius, in sonar space units. */
    public float SonarRadius = 90;

    /** Sonar extents in the UI (polar space). */
    public Vector2 UiAngleRange = new Vector2(-45, 45);
    public Vector2 UiRadiusRange = new Vector2(100, 200);

    /** Sonar extents in the actual display (polar space). */
    public Vector2 ActualAngleRange = new Vector2(-40, 40);
    public Vector2 ActualRadiusRange = new Vector2(60, 95);


    // Properties
    // ------------------------------------------------------------

    /** The event being represented. */
    public megEventSonar Event
    {
        get { return _event; }
        set { SetEvent(value); }
    }

    /** Whether an event is set. */
    public bool HasEvent
        { get { return _event != null; } }


    // Members
    // ------------------------------------------------------------

    /** The sonar event. */
    private megEventSonar _event;

    /** Waypoint UI elements. */
    private readonly List<debugEventSonarWaypointUi> _waypoints = new List<debugEventSonarWaypointUi>();


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        if (!WaypointSpace)
            WaypointSpace = GetComponent<Graphic>();
    }

    /** Updating. */
    private void Update()
    {
        UpdateUi();
    }

    /** Handle a click on the sonar display. */
    public void OnPointerClick(PointerEventData ped)
    {
        // Convert click coordinates to local space.
        Vector2 local;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            WaypointSpace.rectTransform, ped.position, ped.pressEventCamera, out local))
                return;

        // Handle the click event.
        OnClick(local);
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Remove the last waypoint. */
    public void RemoveLastWaypoint()
        { Event.RemoveLastWaypoint(); }

    /** Remove all waypoints. */
    public void RemoveAllWaypoints()
        { Event.RemoveAllWaypoints(); }



    // Private Methods
    // ------------------------------------------------------------

    private void SetEvent(megEventSonar value)
    {
        if (_event == value)
            return;

        _event = value;

        if (_event != null)
        {
            InitUi();
            UpdateUi();
        }
        else
            ClearUi();
    }

    private void InitUi()
    {
    }

    private void UpdateUi()
    {
        if (_event == null)
            return;

        UpdateWaypoints();

        RemoveButton.interactable = Event.hasWaypoints;
        ClearButton.interactable = Event.hasWaypoints;
    }

    private void ClearUi()
    {
    }

    /** Handle a click on the sonar display. */
    private void OnClick(Vector2 local)
    {
        // Add a waypoint to the event.
        AddWaypoint(LocalToWaypoint(local));
    }

    /** Add a waypoint to the sonar event. */
    private void AddWaypoint(Vector3 p)
    {
        Event.AddWaypoint(p);
    }

    /** Convert a point in local space to waypoint space. */
    private Vector3 LocalToWaypoint(Vector2 local)
    {
        // Get polar coordinates in local space.
        var r = local.magnitude;
        var theta = 90 - Mathf.Atan2(local.y, local.x) * Mathf.Rad2Deg;

        // Remap polar coordinates into waypoint space.
        r = Remap(r, UiRadiusRange, ActualRadiusRange);
        theta = Remap(theta, UiAngleRange, ActualAngleRange);
        return new Vector3(theta, 0, r);
    }

    /** Convert a point in waypoint space to local space. */
    private Vector2 WaypointToLocal(Vector3 p)
    {
        // Remap polar coordinates from waypoint into local space.
        var r = Remap(p.z, ActualRadiusRange, UiRadiusRange);
        var theta = Remap(p.x, ActualAngleRange, UiAngleRange);

        // Convert into cartesian space.
        var x = r * Mathf.Sin(theta * Mathf.Deg2Rad);
        var y = r * Mathf.Cos(theta * Mathf.Deg2Rad);

        return new Vector2(x, y);
    }

    /** Remap a value in the 'from' range into a value in the 'to' range. */
    private float Remap(float value, Vector2 from, Vector2 to)
    {
        var low1 = from.x;
        var high1 = from.y;
        var low2 = to.x;
        var high2 = to.y;
        return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
    }

    /** Update the waypoint collection. */
    private void UpdateWaypoints()
    {
        var index = 0;
        if (Event != null)
            foreach (var p in Event.waypoints)
            {
                var wp = GetWaypoint(index++);
                wp.transform.localPosition = WaypointToLocal(p);
                wp.Label.text = index.ToString();
            }

        for (var i = 0; i < _waypoints.Count; i++)
            _waypoints[i].gameObject.SetActive(i < index);
    }

    /** Get a waypoint graphic. */
    private debugEventSonarWaypointUi GetWaypoint(int i)
    {
        if (i >= _waypoints.Count)
        {
            var wp = Instantiate(WaypointPrefab);
            wp.transform.SetParent(WaypointSpace.transform, false);
            _waypoints.Add(wp);
        }

        return _waypoints[i];
    }

}
