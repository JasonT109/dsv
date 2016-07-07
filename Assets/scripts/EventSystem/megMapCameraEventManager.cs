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
        for (int i = 0; i < mapEventList.Length; i++)
        {
            if (mapEventList[i].eventName == eventName)
            {
                //start this event
                running = true;

                //set run time to 0
                runTime = 0f;

                //set the current running event
                runningEvent = mapEventList[i];

                //set the current running event button group just in case we want to turn this off when the event is complete
                runningEventButton = mapEventList[i].trigger.GetComponent<buttonControl>();

                //set the initial states
                initialPosition = mapCameraRoot.transform.localPosition;
                initialOrientationX = mapCameraPitch.transform.localRotation.eulerAngles.x;
                initialOrientationY = mapCameraRoot.transform.localRotation.eulerAngles.y;
                initialZoom = mapCameraObject.transform.localPosition.z;
            }
        }
    }

	void Update ()
    {
        //check to see if the sever wants to start an event
        if (serverUtils.GetServerData("initiateMapEvent") == 1.0f)
        {
            //trigger this event
            triggerByName(serverUtils.GetServerDataAsText("mapEventName"));
        }

        if (!running)
        {
            //we can trigger an event
            for (int i = 0; i < mapEventList.Length; i++)
            {
                //check each button to see if it wants to trigger an event
                if (mapEventList[i].trigger.GetComponent<buttonControl>().active == true)
                {
                    //start this event
                    running = true;

                    //set run time to 0
                    runTime = 0f;

                    //set the current running event
                    runningEvent = mapEventList[i];

                    //set the current running event button group just in case we want to turn this off when the event is complete
                    runningEventButton = mapEventList[i].trigger.GetComponent<buttonControl>();

                    //set the initial states
                    initialPosition = mapCameraRoot.transform.localPosition;
                    initialOrientationX = mapCameraPitch.transform.localRotation.eulerAngles.x;
                    initialOrientationY = mapCameraRoot.transform.localRotation.eulerAngles.y;
                    initialZoom = mapCameraObject.transform.localPosition.z;
                }
            }
        }
        else
        {
            runTime += Time.deltaTime;
            if (runTime > runningEvent.completeTime)
            {
                //stop this event
                running = false;

                //turn off the trigger button
                runningEventButton.active = false;
            }
            else
            {
                //get percentage through the event
                completePercentage = runTime / runningEvent.completeTime;

                //set ease curve
                completePercentage = graphicsEasing.EaseInOut(completePercentage, EasingType.Cubic);

                //Lerp the camera root position
                mapCameraRoot.transform.localPosition = Vector3.Lerp(initialPosition, runningEvent.toPosition, completePercentage);

                //Lerp the camera y orientation
                float camY = Mathf.Lerp(initialOrientationY, runningEvent.toOrientation.y, completePercentage);

                //Lerp the camera pitch
                float camX = Mathf.Lerp(initialOrientationX, runningEvent.toOrientation.x, completePercentage);

                //set the root rotation
                mapCameraRoot.transform.rotation = Quaternion.Euler(0, camY, 0);

                //set the pitch
                mapPitchSliderButton.transform.localPosition = new Vector3(graphicsMaths.remapValue(camX, sliderMinValue, sliderMaxValue, -1, 1) , 0, mapPitchSliderButton.transform.localPosition.z);

                //Lerp the camera zoom
                mapCameraObject.transform.localPosition = Vector3.Lerp(new Vector3(0, 0, initialZoom), new Vector3(0, 0, runningEvent.toZoom), completePercentage);

            }
        }
	}
}
