using UnityEngine;
using System.Collections;
using Meg.EventSystem;
using Meg.Networking;
using Meg.Maths;

public class megMapCameraEventManager : Singleton<megMapCameraEventManager>
{
    //map camera events are local and do not need to be sync'd with the server
    //they can however be triggered by the server
    //they are typically used for tracking an object on the map


    // Structures
    // ------------------------------------------------------------

    public struct State
    {
        public Vector3 toPosition;
        public Vector3 toOrientation;
        public float toZoom;
        public float completeTime;
    }


    // Properties
    // ------------------------------------------------------------

    public megEventMapCamera[] mapEventList;
    public GameObject mapCameraRoot;
    public GameObject mapCameraPitch;
    public GameObject mapCameraObject;
    public GameObject mapPitchSliderButton;

    public float runTime;
    public bool running;
    public float completePercentage;

    public bool isContourModeActive { get; set; }


    private megEventMapCamera runningEvent;
    private buttonControl runningEventButton;
    private Vector3 initialPosition;
    private float initialOrientationX;
    private float initialOrientationY;
    private float initialZoom;

    private widget3DMap _map;

    private bool _started;

    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Awake()
    {
        // Immediately update the camera when manager is started.
        UpdateMap();
        _started = true;
    }

    /** Enabling. */
    private void OnEnable()
    {
        // Immediately update the camera when manager is enabled.
        if (_started)
            UpdateMap();
    }

    /** Update map camera events. */
    private void Update()
    {
        // Ensure that camera exists.
        ResolveMapCamera();
        if (!mapCameraRoot)
            return;

        // Check to see if the server wants to start an event
        if (Mathf.Approximately(serverUtils.GetServerData("initiateMapEvent"), 1))
        {
            var eventName = serverUtils.GetServerDataAsText("mapEventName");
            if (runningEvent == null || runningEvent.eventName != eventName)
                triggerByName(eventName);
        }

        // Trigger camera events on button presses.
        for (var i = 0; i < mapEventList.Length; i++)
        {
            var trigger = mapEventList[i].trigger;
            if (trigger && trigger.GetComponent<buttonControl>().pressed)
                StartRunningEvent(mapEventList[i]);
        }

        // Update the current running event (if any).
        if (runningEvent != null)
        {
            runTime += Time.deltaTime;
            if (runTime > runningEvent.completeTime)
                StopRunningEvent();
            else
                UpdateRunningEvent();
        }
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Trigger a registered camera event by name. */
    public void triggerByName(string eventName)
    {
        for (var i = 0; i < mapEventList.Length; i++)
        {
            if (mapEventList[i].eventName == eventName)
            {
                StartRunningEvent(mapEventList[i]);

                // Toggle the event's associated button on if desired.
                var b = runningEventButton;
                if (b != null && b.toggleType && !b.active)
                    b.RemoteToggle();
            }
        }
    }

    /** Trigger a custom camera state. */
    public void triggerEventFromState(State state)
    {
        // Ignore state events while in contour mode.
        if (isContourModeActive)
            return;

        var e = new megEventMapCamera(state);
        StartRunningEvent(e);
    }

    /** Capture the map camera's current state into a camera event.  */
    public bool Capture(ref State state)
    {
        if (!mapCameraRoot)
            return false;

        state.toPosition = mapCameraRoot.transform.localPosition;

        var camY = mapCameraRoot.transform.rotation.eulerAngles.y;
        var camX = mapPitchSliderButton.GetComponent<sliderWidget>().returnValue;
        state.toOrientation = new Vector3(camX, camY);

        var camZ = mapCameraObject.transform.localPosition.z;
        state.toZoom = camZ;
        return true;
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Resolve map camera components. */
    private void ResolveMapCamera()
    {
        if (mapCameraRoot)
            return;

        var navMap = NavMapCamera.Instance;
        if (!navMap)
            return;

        mapCameraRoot = navMap.Root.gameObject;
        mapCameraPitch = navMap.Pitch.gameObject;
        mapCameraObject = navMap.Camera.gameObject;
    }

    /** Start running the given camera event. */
    private void StartRunningEvent(megEventMapCamera e)
    {
        // Check if we are allowed to interrupt the current event.
        if (runningEvent != null && e.priority < runningEvent.priority)
            return;

        // Ensure that map camera exists.
        ResolveMapCamera();
        if (!mapCameraRoot)
            return;

        // start this event
        running = true;

        // set run time to 0
        runTime = 0f;

        // set the current running event
        runningEvent = e;

        // set the current running event button group just in case we want to turn this off when the event is complete
        if (e.trigger)
            runningEventButton = e.trigger.GetComponent<buttonControl>();

        //set the initial states
        initialPosition = mapCameraRoot.transform.localPosition;
        initialOrientationX = mapCameraPitch.transform.localRotation.eulerAngles.x;
        initialOrientationY = mapCameraRoot.transform.localRotation.eulerAngles.y;
        initialZoom = mapCameraObject.transform.localPosition.z;

        //if to object get that position in map space
        if (runningEvent.goToObject)
        {
            var p = runningEvent.toObject.transform.position;
            runningEvent.toPosition = mapCameraRoot.transform.parent.InverseTransformPoint(p);
            runningEvent.toOrientation = new Vector3(initialOrientationX, initialOrientationY, 0);
            runningEvent.toZoom = initialZoom;
        }

        if (runningEvent.goToPlayerVessel)
        {
            var playerVessel = serverUtils.GetPlayerVessel();
            var p = NavSubPins.Instance.GetVesselPin(playerVessel).transform.position;
            runningEvent.toPosition = mapCameraRoot.transform.parent.InverseTransformPoint(p);
            runningEvent.toOrientation = new Vector3(initialOrientationX, initialOrientationY, 0);
            runningEvent.toZoom = initialZoom;
        }
    }

    /** Update the current camera event. */
    private void UpdateRunningEvent()
    {
        // Get percentage through the event
        completePercentage = runTime / runningEvent.completeTime;

        // Set ease curve
        completePercentage = graphicsEasing.EaseInOut(completePercentage, EasingType.Cubic);

        // Lerp the camera root position
        mapCameraRoot.transform.localPosition = Vector3.Lerp(initialPosition, runningEvent.toPosition, completePercentage);

        // Lerp the camera y orientation
        var currentY = Mathf.Repeat(initialOrientationY, 360);
        var targetY = Mathf.Repeat(runningEvent.toOrientation.y, 360);
        var camY = Mathf.LerpAngle(currentY, targetY, completePercentage);

        // Lerp the camera pitch
        var camX = Mathf.Lerp(initialOrientationX, runningEvent.toOrientation.x, completePercentage);

        // Set the root rotation
        mapCameraRoot.transform.rotation = Quaternion.Euler(0, camY, 0);

        // Set the pitch
        mapPitchSliderButton.GetComponent<sliderWidget>().SetValue(camX);

        // Lerp the camera zoom
        mapCameraObject.transform.localPosition = Vector3.Lerp(new Vector3(0, 0, initialZoom), new Vector3(0, 0, runningEvent.toZoom), completePercentage);
    }

    /** Stop running the current camera event. */
    private void StopRunningEvent()
    {
        // Ensure camera is in final state.
        if (runningEvent != null && gameObject.activeInHierarchy)
            SetState(runningEvent.GetState());

        //stop this event
        running = false;
        runningEvent = null;
        runTime = 0f;

        //stop server initiated event
        serverUtils.SetServerData("initiateMapEvent", 0);

        // turn off the trigger button
        if (runningEventButton && !runningEventButton.buttonGroup)
            runningEventButton.active = false;
    }

    /** Set a given state on the camera. */
    private void SetState(State state)
    {
        mapCameraRoot.transform.localPosition = state.toPosition;
        var camY = Mathf.Repeat(state.toOrientation.y, 360);
        var camX = state.toOrientation.x;

        mapCameraRoot.transform.rotation = Quaternion.Euler(0, camY, 0);

        mapPitchSliderButton.GetComponent<sliderWidget>().SetValue(camX);

        mapCameraObject.transform.localPosition = new Vector3(0, 0, state.toZoom);
    }

    /** Update the map immediately. */
    private void UpdateMap()
    {
        // Update the current camera event state.
        Update();

        // Attempt to update the map immediately.
        if (!_map)
            _map = GetComponent<widget3DMap>();

        if (_map)
            _map.UpdateMap();
    }

}
