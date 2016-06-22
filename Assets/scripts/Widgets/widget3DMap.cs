using UnityEngine;
using System;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Hit;
using Meg.Maths;

public class widget3DMap : MonoBehaviour {

    public GameObject mapCameraRoot;
    public GameObject mapCamera;
    public Material mapMaterial;
    public float rootMaxX = 225f;
    public float rootMaxZ = 225f;
    public float camMaxZ = -400f;
    public float camMinZ = -160f;
    public float camMaxFOV = 50f;
    public float camMinFOV = 22f;
    public float mapMaxScale = 10f;
    public float mapMinScale = 1;
    public float mapMaxOffset = -9f;
    public float scrollSpeed = 1.0f;
    public float rotateSpeed = 0.2f;
    public float scaleSpeed = 0.2f;

    public float maxScroll;
    private float scaleDelta;
    private Vector3 posDelta;
    private float rotDelta;
    private TouchHit currentHit;
    private TouchHit previousHit;
    private bool pressed = false;
    private bool multiTouch = false;
    
    public Vector4 matPosScale;

    private void OnEnable()
    {
        GetComponent<PressGesture>().Pressed += pressedHandler;
        GetComponent<ReleaseGesture>().Released += releaseHandler;
        GetComponent<TransformGesture>().Transformed += transformHandler;
    }

    private void OnDisable()
    {
        GetComponent<PressGesture>().Pressed -= pressedHandler;
        GetComponent<ReleaseGesture>().Released -= releaseHandler;
        GetComponent<TransformGesture>().Transformed -= transformHandler;
    }

    private void pressedHandler(object sender, EventArgs e)
    {
        var gesture = sender as PressGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);
    }

    private void transformHandler(object sender, EventArgs e)
    {
        var gesture = sender as TransformGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        //make sure we are hitting this gameObject
        if (hit.Transform != gameObject.transform)
        {
            return;
        }

        //set the current hit data so we can read the position the touch is at
        currentHit = hit;

        //set pressed to true so the update will transform the texture co-ords
        pressed = true;

        //if we have more than one touch
        if (gesture.NumTouches > 1)
        {
            multiTouch = true;
            //get scale of gesture
            scaleDelta = gesture.DeltaScale;
            posDelta = gesture.DeltaPosition;
            rotDelta = gesture.DeltaRotation;
        }
        else
        {
            scaleDelta = 1f;
            rotDelta = 0f;
            multiTouch = false;
        }
    }

    private void releaseHandler(object sender, EventArgs e)
    {
        var gesture = sender as ReleaseGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        //reset previous hit
        previousHit = new TouchHit();
        scaleDelta = 1f;

        //stop the update loop from transforming the texture
        pressed = false;

    }

    // Use this for initialization
    void Start()
    {
        Terrain.activeTerrain.materialTemplate = mapMaterial;
    }

	// Update is called once per frame
	void Update () {

        //get current zoom level from camera
        float zoom = (mapCamera.transform.localPosition.z - camMinZ) / (camMaxZ - camMinZ);
        float camZ = mapCamera.transform.localPosition.z;

        if (pressed)
        {
            matPosScale = mapMaterial.GetVector("_PosScale");

            //get difference from this touch position and last
            Vector2 touchDelta = new Vector2(previousHit.Point.x - currentHit.Point.x, previousHit.Point.y - currentHit.Point.y);

            float rotateAmount = 1f;
            rotateAmount /= rotDelta;

            //apply the rotation
            mapCameraRoot.transform.Rotate(0, rotDelta * rotateSpeed, 0);

            //scale amount
            float scaleAmount = 1f;
            scaleAmount = scaleDelta * (Time.deltaTime * scaleSpeed);

            //divide scale amount by the current delta
            if (scaleDelta < 1)
            {
                scaleAmount = -scaleAmount;
                zoom += scaleAmount;
                zoom = Mathf.Clamp(zoom, 0, 1);
            }

            if (scaleDelta > 1)
            {
                zoom += scaleAmount;
                zoom = Mathf.Clamp(zoom, 0, 1);
            }

            camZ = (zoom * (camMaxZ - camMinZ)) + camMinZ;

            //add this to the camera pos z
            mapCamera.transform.localPosition = new Vector3(0, 0, camZ);
            //mapCamera.transform.localPosition = new Vector3(0, 0, Mathf.Clamp(mapCamera.transform.localPosition.z, camMinZ, camMaxZ));

            //inverse scale the local position constraints so we can't scroll when zoomed out
            maxScroll = (mapCamera.transform.localPosition.z - camMinZ) / (camMaxZ - camMinZ) * (rootMaxX - -rootMaxX) + -rootMaxX;
            maxScroll = Mathf.Abs(maxScroll);

            //pan the camera
            if (previousHit.Point != Vector3.zero && !multiTouch)
            {
                //create a local space transform that we can transpose
                Vector3 worldPos = new Vector3(touchDelta.x * (Time.deltaTime * scrollSpeed), 0, touchDelta.y * (Time.deltaTime * scrollSpeed));

                worldPos = mapCameraRoot.transform.TransformPoint(worldPos);

                worldPos -= mapCameraRoot.transform.position;

                mapCameraRoot.transform.localPosition += new Vector3( worldPos.x, mapCameraRoot.transform.localPosition.y, worldPos.z);
                mapCameraRoot.transform.localPosition = new Vector3(Mathf.Clamp(mapCameraRoot.transform.localPosition.x, -maxScroll, maxScroll), mapCameraRoot.transform.localPosition.y, Mathf.Clamp(mapCameraRoot.transform.localPosition.z, -maxScroll, maxScroll));
            }
            previousHit = currentHit;
        }

        //invert zoom and apply to map clipping UV
        matPosScale.z = ((1 - graphicsEasing.EaseOut(zoom, EasingType.Quadratic)) * (mapMaxScale - mapMinScale)) + mapMinScale;
        //matPosScale.w = matPosScale.z;

        //set the clipping UV position
        matPosScale.x = -((mapCameraRoot.transform.localPosition.z - -rootMaxX) / (rootMaxX - -rootMaxX) * (mapMaxOffset - -mapMaxOffset) + -mapMaxOffset) * (1 - zoom);
        matPosScale.y = -((mapCameraRoot.transform.localPosition.x - -rootMaxX) / (rootMaxX - -rootMaxX) * (mapMaxOffset - -mapMaxOffset) + -mapMaxOffset) * (1 - zoom);

        //set material data
        mapMaterial.SetVector("_PosScale", matPosScale);

        //clamp the translation so we can't pan off the map
        mapCameraRoot.transform.localPosition = new Vector3(Mathf.Clamp(mapCameraRoot.transform.localPosition.x, -(rootMaxX * (1 - zoom)), rootMaxX * (1 - zoom)), mapCameraRoot.transform.localPosition.y, Mathf.Clamp(mapCameraRoot.transform.localPosition.z, -(rootMaxZ * (1 - zoom)), rootMaxZ * (1 - zoom)));

        //set the FOV
        float currentFOV = zoom * ((camMaxFOV - camMinFOV) + camMinFOV);
        currentFOV = Mathf.Clamp(currentFOV, camMinFOV, camMaxFOV);
        mapCamera.GetComponent<Camera>().fieldOfView = currentFOV;

        Debug.Log("Zoom level = " + zoom + " Map scale = " + matPosScale.z + " Camera position: " + camZ + " FOV = " + currentFOV);
    }
}
