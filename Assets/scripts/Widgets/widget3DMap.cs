using UnityEngine;
using System;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Hit;
using Meg.Maths;

public class widget3DMap : MonoBehaviour {

    [Header("Map Components")]
    public GameObject mapCameraRoot;
    public GameObject mapCameraPitch;
    public GameObject mapCamera;

    [Header("Components")]
    public GameObject viewAngleSlider;
    public buttonControl button3dMapping;
    public buttonControl buttonContourMapping;
    public TiltShift tilt;

    [Header("Configuration")]
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
    public float scaleAmount;
    public float maxScroll;
    public float scaleDelta;
    public bool deactivateChildrenOnScroll = true;

    [Header("Terrain")]
    public Material TerrainMaterial2d;
    public Material TerrainMaterial3d;
    public float TerrainTransitionRange = 30;

    [Header("Water")]
    public GameObject Water;
    public Color WaterColor2d;
    public Color WaterColor3d;

    private float zoom;
    private float rotDelta;
    private TouchHit currentHit;
    private TouchHit previousHit;
    private bool pressed = false;
    private bool multiTouch = false;
    private float rotateAmount = 0f;

    private bool _terrainInitialized;
    private float _terrain3DAmount = 1;
    private float _terrain3DVelocity = 0;

    private Color _loColor2D;
    private Color _hiColor2D;
    private Color _lineColor2D;
    private Color _lineColorHi2D;
    private Color _fresnelColor2D;
    private Color _ambient2D;
    private float _levels2D;
    private float _gradient2D;
    private float _lineDetail2D;

    private Color _loColor3D;
    private Color _hiColor3D;
    private Color _lineColor3D;
    private Color _lineColorHi3D;
    private Color _fresnelColor3D;
    private Color _ambient3D;
    private float _levels3D;
    private float _gradient3D;
    private float _lineDetail3D;

    private Material _waterMaterial;


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

        UpdateZoom();
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

    void UpdateZoom()
    {
        // Get current zoom level from camera
        zoom = (mapCamera.transform.localPosition.z - camMinZ) / (camMaxZ - camMinZ);
        var camZ = mapCamera.transform.localPosition.z;
        if (tilt)
            tilt.focalPoint = Math.Abs(camZ);

        // Set the camera's FOV.
        float currentFOV = zoom * ((camMaxFOV - camMinFOV) + camMinFOV);
        currentFOV = Mathf.Clamp(currentFOV, camMinFOV, camMaxFOV);
        mapCamera.GetComponent<Camera>().fieldOfView = currentFOV;
    }

    public void UpdateMap()
    {
        // Cap delta time to avoid jumps at low framerates.
        const float maxDeltaTime = 1 / 50.0f;
        var dt = Mathf.Min(maxDeltaTime, Time.deltaTime);

        //get the view angle slider value
        var slider = viewAngleSlider.GetComponent<sliderWidget>();
        var viewAngle = Mathf.Clamp(slider.returnValue, slider.minValue, slider.maxValue);
        mapCameraPitch.transform.localRotation = Quaternion.Euler(viewAngle, 0, 0);

        // Enable or disable slider based on current camera mapping mode (3d / contours).
        var is3D = !button3dMapping || button3dMapping.active;
        slider.SetInputEnabled(is3D);
        slider.SetVisible(is3D);

        // Update terrain material based on view angle.
        UpdateTerrain();

        // Update camera zoom level.
        UpdateZoom();

        // Scale and pan speed should be adjusted to how far out we are zoomed, 100% speed at zoomed out, 10% speed at zoomed in
        float zoomeSpeedMultiplier = graphicsMaths.remapValue(zoom, 0f, 1f, 0.1f, 1f);

        if (pressed)
        {
            //scale or rotate the camera
            if (multiTouch)
            {
                //slowly increase the rotate amount so we don't get jittery rotations when fingers are close together
                rotateAmount = Mathf.Lerp(rotateAmount, rotDelta, dt * rotateSpeed);

                //apply the rotation
                mapCameraRoot.transform.Rotate(0, rotateAmount, 0);

                //scale amount is -1 to 1
                scaleAmount = graphicsMaths.remapValue(scaleDelta, 0.5f, 1.5f, -1f, 1f);

                // Add scale amount to current camera position value
                var camZ = mapCamera.transform.localPosition.z;
                camZ += scaleAmount * (scaleSpeed * zoomeSpeedMultiplier);

                // Add this to the camera pos z
                mapCamera.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(mapCamera.transform.localPosition.z, camZ, dt * 0.2f));
                mapCamera.transform.localPosition = new Vector3(0, 0, Mathf.Clamp(mapCamera.transform.localPosition.z, camMaxZ, camMinZ));
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

                //z translate is normalised view angle * touchDelta.y
                float zTrans = graphicsMaths.remapValue(viewAngle, viewAngleSlider.GetComponent<sliderWidget>().minValue, viewAngleSlider.GetComponent<sliderWidget>().maxValue, 0, 1) * touchDelta.y;

                //y translate is 1 - normalised view angle * touchDelta.y
                float yTrans = (1 - graphicsMaths.remapValue(viewAngle, viewAngleSlider.GetComponent<sliderWidget>().minValue, viewAngleSlider.GetComponent<sliderWidget>().maxValue, 0, 1)) * touchDelta.y;

                //create a local space transform that we can transpose
                Vector3 worldPos = new Vector3(touchDelta.x * (dt * (scrollSpeed * zoomeSpeedMultiplier)), yTrans * (dt * (scrollSpeed * zoomeSpeedMultiplier)), zTrans * (dt * (scrollSpeed * zoomeSpeedMultiplier)));

                //transpose to world space
                worldPos = mapCameraRoot.transform.TransformPoint(worldPos);

                //subtract the root position
                worldPos -= mapCameraRoot.transform.position;

                Vector3 rootPos = mapCameraRoot.transform.localPosition;

                //add the new value to root position
                rootPos += new Vector3(worldPos.x, worldPos.y, worldPos.z);

                //clamp the translation
                //rootPos = new Vector3(Mathf.Clamp(rootPos.x, -maxScroll, maxScroll), rootPos.y, Mathf.Clamp(rootPos.z, -maxScroll, maxScroll));

                mapCameraRoot.transform.localPosition = Vector3.Lerp(mapCameraRoot.transform.localPosition, rootPos, dt);

                //set previous hit point so we can reference it next frame to calculate the delta
                previousHit = currentHit;
            }
        }
    }

    void Start()
    {
        tilt = mapCamera.GetComponent<TiltShift>();
        UpdateTerrain();
    }

	void Update()
    {
        UpdateMap();
    }

    private void UpdateTerrain()
    {
        var terrain = Terrain.activeTerrain;
        if (!terrain)
            return;

        if (!_terrainInitialized)
            InitTerrain();

        if (!TerrainMaterial2d || !TerrainMaterial3d)
            return;

        var slider = viewAngleSlider.GetComponent<sliderWidget>();
        var viewAngle = Mathf.Clamp(slider.returnValue, slider.minValue, slider.maxValue);

        var transitionStart = slider.maxValue - TerrainTransitionRange;
        var target3DAmount = 1 - Mathf.Clamp01((viewAngle - transitionStart) / TerrainTransitionRange);

        // Don't allow crossfade in 3d mode (unless transitioning).
        var cameraEventManager = GetComponent<megMapCameraEventManager>();
        var force3D = button3dMapping.active && !cameraEventManager.running;
        if (force3D)
            target3DAmount = 1;

        // Smooth terrain 3d fade amount over time.
        _terrain3DAmount = Mathf.SmoothDamp(_terrain3DAmount, target3DAmount, ref _terrain3DVelocity, 0.5f);

        var t = _terrain3DAmount;
        var m = terrain.materialTemplate;
        m.SetColor("_LoColor", Color.Lerp(_loColor2D, _loColor3D, t));
        m.SetColor("_HiColor", Color.Lerp(_hiColor2D, _hiColor3D, t));
        m.SetColor("_LineColor", Color.Lerp(_lineColor2D, _lineColor3D, t));
        m.SetColor("_LineColorHi", Color.Lerp(_lineColorHi2D, _lineColorHi3D, t));
        m.SetColor("_Ambient", Color.Lerp(_ambient2D, _ambient3D, t));
        m.SetColor("_FresnelColor", Color.Lerp(_fresnelColor2D, _fresnelColor3D, t));
        m.SetFloat("_Levels", Mathf.Lerp(_levels2D, _levels3D, t));
        m.SetFloat("_Gradient", Mathf.Lerp(_gradient2D, _gradient3D, t));
        m.SetFloat("_LineDetail", Mathf.Lerp(_lineDetail2D, _lineDetail3D, t));

        _waterMaterial.SetColor("_MainColor", Color.Lerp(WaterColor2d, WaterColor3d, t));
    }

    private void InitTerrain()
    {
        var terrain = Terrain.activeTerrain;
        if (!TerrainMaterial2d || !TerrainMaterial3d)
            return;

        _loColor2D = TerrainMaterial2d.GetColor("_LoColor");
        _hiColor2D = TerrainMaterial2d.GetColor("_HiColor");
        _lineColor2D = TerrainMaterial2d.GetColor("_LineColor");
        _lineColorHi2D = TerrainMaterial2d.GetColor("_LineColorHi");
        _ambient2D = TerrainMaterial2d.GetColor("_Ambient");
        _fresnelColor2D = TerrainMaterial2d.GetColor("_FresnelColor");
        _levels2D = TerrainMaterial2d.GetFloat("_Levels");
        _gradient2D = TerrainMaterial2d.GetFloat("_Gradient");
        _lineDetail2D = TerrainMaterial2d.GetFloat("_LineDetail");

        _loColor3D = TerrainMaterial3d.GetColor("_LoColor");
        _hiColor3D = TerrainMaterial3d.GetColor("_HiColor");
        _lineColor3D = TerrainMaterial3d.GetColor("_LineColor");
        _lineColorHi3D = TerrainMaterial3d.GetColor("_LineColorHi");
        _ambient3D = TerrainMaterial3d.GetColor("_Ambient");
        _fresnelColor3D = TerrainMaterial3d.GetColor("_FresnelColor");
        _levels3D = TerrainMaterial3d.GetFloat("_Levels");
        _gradient3D = TerrainMaterial3d.GetFloat("_Gradient");
        _lineDetail3D = TerrainMaterial3d.GetFloat("_LineDetail");

        terrain.materialTemplate = new Material(terrain.materialTemplate);
        terrain.basemapDistance = 10000;

        _waterMaterial = Water.GetComponent<Renderer>().material;

        _terrainInitialized = true;
    }
}
