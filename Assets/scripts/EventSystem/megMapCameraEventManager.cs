using UnityEngine;
using System.Collections;
using Meg.EventSystem;
using Meg.Networking;
using Meg.Maths;

public class megMapCameraEventManager : MonoBehaviour
{
    //map camera events are local and do not need to be sync'd with the server
    //they can however be triggered by the server
    //they are typically used for tracking an object on the map

    public megEventMapCamera[] mapEventList;
    public GameObject mapCameraRoot;
    public GameObject mapCameraPitch;
    public GameObject mapCameraObject;
    public GameObject mapPitchSliderButton;
    public GameObject[] vessels;

    public float runTime;
    public bool running;
    public float completePercentage;
    private megEventMapCamera runningEvent;
    private buttonControl runningEventButton;
    private Vector3 initialPosition;
    private float initialOrientationX;
    private float initialOrientationY;
    private float initialZoom;
    private float sliderMinValue;
    private float sliderMaxValue;
    

    void Start()
    {
        sliderMinValue = mapPitchSliderButton.GetComponent<sliderWidget>().minValue;
        sliderMaxValue = mapPitchSliderButton.GetComponent<sliderWidget>().maxValue;
    }

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

	private void Update()
    {
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
            if (mapEventList[i].trigger.GetComponent<buttonControl>().pressed)
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

    /** Start running the given camera event. */
    private void StartRunningEvent(megEventMapCamera e)
    {
        // Check if we are allowed to interrupt the current event.
        if (runningEvent != null && e.priority < runningEvent.priority)
            return;

        // start this event
        running = true;

        // set run time to 0
        runTime = 0f;

        // set the current running event
        runningEvent = e;

        // set the current running event button group just in case we want to turn this off when the event is complete
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
            var p = vessels[playerVessel - 1].transform.position;
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
        mapPitchSliderButton.transform.localPosition = new Vector3(graphicsMaths.remapValue(camX, sliderMinValue, sliderMaxValue, -1, 1), 0, mapPitchSliderButton.transform.localPosition.z);

        // Lerp the camera zoom
        mapCameraObject.transform.localPosition = Vector3.Lerp(new Vector3(0, 0, initialZoom), new Vector3(0, 0, runningEvent.toZoom), completePercentage);
    }

    /** Stop running the current camera event. */
    private void StopRunningEvent()
    {
        //stop this event
        running = false;
        runningEvent = null;
        runTime = 0f;

        //stop server initiated event
        serverUtils.SetServerData("initiateMapEvent", 0);

        // turn off the trigger button
        if (!runningEventButton.buttonGroup)
            runningEventButton.active = false;
    }

}
