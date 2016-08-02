using System;
using System.Collections;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Hit;
using Meg.Networking;
using Meg.EventSystem;
using Meg.SonarEvent;

public class megSonarGetTouch : MonoBehaviour {

    public bool pressed = false;
    public Vector3 hitLocation;
    public bool recording = false;
    public GameObject marker;
    public int markerId = 0;
    public GameObject recordButton;
    public GameObject playButton;
    public GameObject customButton1;
    public Vector3[] wp1;
    public GameObject customButton2;
    public Vector3[] wp2;
    public GameObject customButton3;
    public Vector3[] wp3;
    public GameObject customButton4;
    public Vector3[] wp4;
    
    private int activeSlot = 1;
    private Vector3[] tempVecArray;
    private bool playing = false;
    private megEventSonar playEvent;
    private bool canPress = true;

    private void OnEnable()
    {
        GetComponent<PressGesture>().Pressed += pressedHandler;
        GetComponent<ReleaseGesture>().Released += releaseHandler;
    }

    private void OnDisable()
    {
        GetComponent<PressGesture>().Pressed -= pressedHandler;
        GetComponent<ReleaseGesture>().Released -= releaseHandler;
    }

    private void pressedHandler(object sender, EventArgs e)
    {
        var gesture = sender as PressGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

    }

    private void releaseHandler(object sender, EventArgs e)
    {
        var gesture = sender as ReleaseGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        //call spawnMarker
        spawnMarker(hit.Point);
    }

    IEnumerator waitPress(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        pressed = false;
    }

    private void spawnMarker(Vector3 worldPos)
    {
        if (marker && recording)
        {
            //convert to local space
            hitLocation = gameObject.transform.InverseTransformPoint(worldPos);

            //spawn a visual waypoint
            GameObject wp = (GameObject)Instantiate(marker, new Vector3(0, 0, 0), Quaternion.identity);
            TextMesh t = wp.GetComponentInChildren<TextMesh>();

            //set the ID of the marker
            t.text = markerId.ToString();

            //parent to this object
            wp.transform.parent = gameObject.transform;

            //set the wp local position
            wp.transform.localPosition = new Vector3(hitLocation.x, hitLocation.y, hitLocation.z);

            //re-order for the 2d co-ordinate system and remove the y component from hitlocation as it isn't needed
            hitLocation = new Vector3(hitLocation.x, 0, hitLocation.y);

            //record the way point
            if (markerId == 0)
            {
                tempVecArray = new Vector3[1];
                tempVecArray[markerId] = hitLocation;
            }
            else
            {
                //copy the existing temp array to this
                Vector3[] newArray = new Vector3[markerId + 1];
                Array.Copy(tempVecArray, newArray, markerId);

                //add the new data
                newArray[markerId] = hitLocation;

                //re-declare and copy back to the temp array
                tempVecArray = new Vector3[markerId + 1];
                Array.Copy(newArray, tempVecArray, markerId + 1);
            }

            switch (activeSlot)
            {
                case 1:
                    wp1 = new Vector3[tempVecArray.Length];
                    Array.Copy(tempVecArray, wp1, tempVecArray.Length);
                    break;
                case 2:
                    wp2 = new Vector3[tempVecArray.Length];
                    Array.Copy(tempVecArray, wp2, tempVecArray.Length);
                    break;
                case 3:
                    wp3 = new Vector3[tempVecArray.Length];
                    Array.Copy(tempVecArray, wp3, tempVecArray.Length);
                    break;
                case 4:
                    wp4 = new Vector3[tempVecArray.Length];
                    Array.Copy(tempVecArray, wp4, tempVecArray.Length);
                    break;
            }

            //increment the ID
            markerId++;

            Debug.Log("Hit object here: " + hitLocation);
        }
        else
        {
            Debug.Log("No marker or not recording.");
        }
    }
	

	void Update ()
    {
        //allow recording of waypoints
        if (recordButton.GetComponent<buttonControl>().active && !recording)
        {
            recording = true;

            //get previously used markers and destroy them
            GameObject[] oldMarkers = GameObject.FindGameObjectsWithTag("Marker");
            foreach (GameObject g in oldMarkers)
            {
                Destroy(g);
            }
        }

        //don't allow recording
        if (!recordButton.GetComponent<buttonControl>().active)
        {
            recording = false;
            markerId = 0;
        }

        //attempt to play an event
        if (playButton.GetComponent<buttonControl>().active && !playing)
        {
            playEvent = new megEventSonar();
            switch (activeSlot)
            {
                case 1:
                    playEvent.waypoints = wp1;
                    break;
                case 2:
                    playEvent.waypoints = wp2;
                    break;
                case 3:
                    playEvent.waypoints = wp3;
                    break;
                case 4:
                    playEvent.waypoints = wp4;
                    break;
            }

            //set event to auto destroy
            playEvent.destroyOnEnd = true;

            //start the event
            if (playEvent.waypoints.Length > 0)
            {
                var sonarManager = gameObject.GetComponent<megSonarEventManager>();
                sonarManager.megPlayMegSonarEvent(playEvent);
                sonarManager.megSetVisualMarkers(playEvent);
            }

            //make sure that this event is only done once
            playing = true;
        }

        //allow playing again when button is de-activated
        if (!playButton.GetComponent<buttonControl>().active)
        {
            playing = false;
        }

        //get the active custom button to record to
        if (customButton1.GetComponent<buttonControl>().active)
        {
            activeSlot = 1;
        }
        if (customButton2.GetComponent<buttonControl>().active)
        {
            activeSlot = 2;
        }
        if (customButton3.GetComponent<buttonControl>().active)
        {
            activeSlot = 3;
        }
        if (customButton4.GetComponent<buttonControl>().active)
        {
            activeSlot = 4;
        }

        //update the markers when we switch custom buttons
        if (customButton1.GetComponent<buttonControl>().pressed && canPress)
        {
            playEvent = new megEventSonar();
            playEvent.waypoints = wp1;
            gameObject.GetComponent<megSonarEventManager>().megSetVisualMarkers(playEvent);
            canPress = false;
            StartCoroutine(wait(0.1f));
        }
        if (customButton2.GetComponent<buttonControl>().pressed && canPress)
        {
            playEvent = new megEventSonar();
            playEvent.waypoints = wp2;
            gameObject.GetComponent<megSonarEventManager>().megSetVisualMarkers(playEvent);
            canPress = false;
            StartCoroutine(wait(0.1f));
        }
        if (customButton3.GetComponent<buttonControl>().pressed && canPress)
        {
            playEvent = new megEventSonar();
            playEvent.waypoints = wp3;
            gameObject.GetComponent<megSonarEventManager>().megSetVisualMarkers(playEvent);
            canPress = false;
            StartCoroutine(wait(0.1f));
        }
        if (customButton4.GetComponent<buttonControl>().pressed && canPress)
        {
            playEvent = new megEventSonar();
            playEvent.waypoints = wp4;
            gameObject.GetComponent<megSonarEventManager>().megSetVisualMarkers(playEvent);
            canPress = false;
            StartCoroutine(wait(0.1f));
        }
    }

    IEnumerator wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canPress = true;
    }
}
