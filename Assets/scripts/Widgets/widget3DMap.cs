using UnityEngine;
using System;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Hit;
using Meg.Maths;

public class widget3DMap : MonoBehaviour {

    public GameObject mapCameraRoot;
    public GameObject mapCameraPitch;
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
    public GameObject viewAngleSlider;
    public float scaleAmount;
    public float maxScroll;
    public float scaleDelta;
    public bool deactivateChildrenOnScroll = true;
    private float zoom;
    private float rotDelta;
    private TouchHit currentHit;
    private TouchHit previousHit;
    private bool pressed = false;
    private bool multiTouch = false;
    private float rotateAmount = 0f;

    float easeOutCustom(float t, float b = 0.0f, float c = 1.0f, float d = 1.0f)
    {
        float ts = (t /= d) * t;
        float tc = ts * t;
        return (b + c * (1.4025f * tc * ts + -6.7075f * ts * ts + +12.21f * tc + -10.805f * ts + 4.9f * t));
    }

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

        //set pressed to true so the update will transform the map
        pressed = true;

        //if we have more than one touch
        if (gesture.NumTouches > 1)
        {
            multiTouch = true;
            //get scale of gesture
            scaleDelta = Mathf.Clamp( gesture.DeltaScale, 0.9f, 1.1f );
            rotDelta = gesture.DeltaRotation;
        }
        else
        {
            scaleDelta = 1f;
            rotDelta = 0f;
            multiTouch = false;
        }

        if (deactivateChildrenOnScroll)
        {
            //set all children buttons to in active
            foreach (GameObject b in gameObject.GetComponentInChildren<buttonGroup>().buttons)
            {
                if (b.GetComponent<buttonControl>().active)
                {
                    b.GetComponent<buttonControl>().toggleButton(b);
                }
            }
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
        rotateAmount = 0f;

        //stop the update loop from transforming the map
        pressed = false;
    }

    // Use this for initialization
    void Start()
    {
        Terrain.activeTerrain.materialTemplate = mapMaterial;
    }

	// Update is called once per frame
	void Update () {

        //get the view angle slider value
        float viewAngle = viewAngleSlider.GetComponent<sliderWidget>().returnValue;

        mapCameraPitch.transform.localRotation = Quaternion.Euler(viewAngle, 0, 0);

        //get current zoom level from camera
        zoom = (mapCamera.transform.localPosition.z - camMinZ) / (camMaxZ - camMinZ);
        float camZ = mapCamera.transform.localPosition.z;

        //scale and pan speed should be adjusted to how far out we are zoomed, 100% speed at zoomed out, 10% speed at zoomed in
        float zoomeSpeedMultiplier = graphicsMaths.remapValue(zoom, 0f, 1f, 0.1f, 1f);

        if (pressed)
        {
            //scale or rotate the camera
            if (multiTouch)
            {
                //slowly increase the rotate amount so we don't get jittery rotations when fingers are close together
                rotateAmount = Mathf.Lerp(rotateAmount, rotDelta, Time.deltaTime * rotateSpeed);

                //apply the rotation
                mapCameraRoot.transform.Rotate(0, rotateAmount, 0);

                //scale amount is -1 to 1
                scaleAmount = graphicsMaths.remapValue(scaleDelta, 0.5f, 1.5f, -1f, 1f);

                //add the scale amount to current camera position value
                camZ += scaleAmount * (scaleSpeed * zoomeSpeedMultiplier);

                //add this to the camera pos z
                mapCamera.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(mapCamera.transform.localPosition.z, camZ, Time.deltaTime * 0.2f));

                mapCamera.transform.localPosition = new Vector3(0, 0, Mathf.Clamp(mapCamera.transform.localPosition.z, camMaxZ, camMinZ));

                //set the FOV
                float currentFOV = zoom * ((camMaxFOV - camMinFOV) + camMinFOV);
                currentFOV = Mathf.Clamp(currentFOV, camMinFOV, camMaxFOV);
                mapCamera.GetComponent<Camera>().fieldOfView = currentFOV;
            }

            //inverse scale the local position constraints so we can't scroll when zoomed out
            maxScroll = (mapCamera.transform.localPosition.z - camMinZ) / (camMaxZ - camMinZ) * (rootMaxX - -rootMaxX) + -rootMaxX;
            maxScroll = Mathf.Abs(maxScroll);

            //pan the camera
            if (!multiTouch)
            {
                //if we've not yet touched the screen the delta can't be reliably calculated
                if (previousHit.Point == Vector3.zero)
                {
                    previousHit = currentHit;
                    return;
                }

                //get difference from this touch position and last
                Vector2 touchDelta = new Vector2(previousHit.Point.x - currentHit.Point.x, previousHit.Point.y - currentHit.Point.y);

                //create a local space transform that we can transpose
                Vector3 worldPos = new Vector3(touchDelta.x * (Time.deltaTime * (scrollSpeed * zoomeSpeedMultiplier)), 0, touchDelta.y * (Time.deltaTime * (scrollSpeed * zoomeSpeedMultiplier)));

                //transpose to world space
                worldPos = mapCameraRoot.transform.TransformPoint(worldPos);

                //subtract the root position
                worldPos -= mapCameraRoot.transform.position;

                Vector3 rootPos = mapCameraRoot.transform.localPosition;

                //add the new value to root position
                rootPos += new Vector3( worldPos.x, mapCameraRoot.transform.localPosition.y, worldPos.z);

                //clamp the translation
                //rootPos = new Vector3(Mathf.Clamp(rootPos.x, -maxScroll, maxScroll), rootPos.y, Mathf.Clamp(rootPos.z, -maxScroll, maxScroll));

                mapCameraRoot.transform.localPosition = Vector3.Lerp(mapCameraRoot.transform.localPosition, rootPos, Time.deltaTime);

                //set previous hit point so we can reference it next frame to calculate the delta
                previousHit = currentHit;
            } 
        }
    }
}
