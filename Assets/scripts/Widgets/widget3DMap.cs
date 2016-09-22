using UnityEngine;
using System;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Hit;
using Meg.Maths;
using Meg.Networking;

public class widget3DMap : MonoBehaviour {

    [Header("Map Components")]
    public GameObject mapCameraRoot;
    public GameObject mapCameraPitch;
    public GameObject mapCamera;

    [Header("Components")]
    public GameObject viewAngleSlider;
    public TiltShift tilt;
    public buttonControl button3dMapping;
    public buttonControl buttonContourMapping;

    [Header("Configuration")]
    public float rootMaxX = 225f;
    public float rootMaxZ = 225f;
    public float camMaxZ = -400f;
    public float camMinZ = -160f;
    public float camMaxFOV = 50f;
    public float camMinFOV = 22f;
    public float camMinOrthoSize = 100;
    public float camMaxOrthoSize = 300;
    public float mapMaxScale = 10f;
    public float mapMinScale = 1;
    public float mapMaxOffset = -9f;
    public float scrollSpeed = 1.0f;
    public float rotateSpeed = 0.2f;
    public float pitchSpeed = 30f;
    public float scaleSpeed = 0.2f;
    public float scaleAmount;
    public float maxScroll;
    public float scaleDelta;
    public bool deactivateChildrenOnScroll = true;
    public bool snapToPlayerVessel;
    public float CameraSnapSmoothTime = 0.5f;
    public float CameraSnapDelay = 5.0f;

    [Header("Terrain")]
    public Material TerrainMaterial2d;
    public Material TerrainMaterial3d;
    public float TerrainTransitionRange = 30;

    [Header("Water")]
    public GameObject Water;
    public Color WaterColor2d;
    public Color WaterColor3d;

    [Header("Acid")]
    public GameObject Acid;
    public Color AcidColor2d;
    public Color AcidColor3d;

    public bool Is3D
        { get { return !IsTopDown; } }

    public bool IsTopDown
        { get { return megMapCameraEventManager.Instance.IsTopDown; } }

    private const float MaxRotationDelta = 10f;
    private const float MinRotationDistance = 0;
    private const float MaxRotationDistance = 200;
    private const float RotationDistanceRange = MaxRotationDistance - MinRotationDistance;

    private float zoom;
    private float rotDelta;
    private TouchHit currentHit;
    private TouchHit previousHit;
    private bool pressed = false;
    private int touches;
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
    private Material _acidMaterial;

    private float _waterAlpha = 1;
    private float _waterSmoothVelocity;
    private float _acidAlpha;
    private float _acidSmoothVelocity;
    private float _acidMaxRefraction;

    private Vector3 _cameraSmoothVelocity;
    private float _lastPressTime;

    private PressGesture _pressGesture;
    private ReleaseGesture _releaseGesture;
    private TransformGesture _transformGesture;

    float easeOutCustom(float t, float b = 0.0f, float c = 1.0f, float d = 1.0f)
    {
        float ts = (t /= d) * t;
        float tc = ts * t;
        return (b + c * (1.4025f * tc * ts + -6.7075f * ts * ts + +12.21f * tc + -10.805f * ts + 4.9f * t));
    }

    private void OnEnable()
    {
        _pressGesture = GetComponent<PressGesture>();
        _releaseGesture = GetComponent<ReleaseGesture>();
        _transformGesture = GetComponent<TransformGesture>();

        _pressGesture.Pressed += pressedHandler;
        _releaseGesture.Released += releaseHandler;
        _transformGesture.Transformed += transformHandler;

        UpdateZoom();
    }

    private void OnDisable()
    {
        _pressGesture.Pressed -= pressedHandler;
        _releaseGesture.Released -= releaseHandler;
        _transformGesture.Transformed -= transformHandler;
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
        touches = gesture.NumTouches;
        if (touches == 2)
        {
            // Limit scale delta to reasonable values.
            scaleDelta = Mathf.Clamp(gesture.DeltaScale, 0.9f, 1.1f);

            // Reduce rotation rate as fingers get close together.
            var t = _transformGesture.ActiveTouches;
            var distance = Vector3.Distance(t[0].Position, t[1].Position);
            var rotScale = Mathf.Clamp01((distance - MinRotationDistance) / RotationDistanceRange);
            rotDelta = Mathf.Clamp(gesture.DeltaRotation * rotScale, -MaxRotationDelta, MaxRotationDelta);
        }
        else
        {
            scaleDelta = 1f;
            rotDelta = 0f;
        }

        if (deactivateChildrenOnScroll)
        {
            //set all children buttons to in active
            var buttonGroup = gameObject.GetComponentInChildren<buttonGroup>();
            foreach (var b in buttonGroup.buttons)
            {
                if (!b)
                    continue;

                var button = b.GetComponent<buttonControl>();
                if (button && button.active)
                    button.toggleButton(b);
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
        // Try to resolve map camera components.
        ResolveMapCamera();

        // Get current zoom level from camera
        zoom = (mapCamera.transform.localPosition.z - camMinZ) / (camMaxZ - camMinZ);
        var camZ = mapCamera.transform.localPosition.z;

        // Update tilt-shift effect.
        if (tilt)
            tilt.focalPoint = Math.Abs(camZ);

        // Set the camera's FOV.
        float currentFOV = zoom * ((camMaxFOV - camMinFOV) + camMinFOV);
        currentFOV = Mathf.Clamp(currentFOV, camMinFOV, camMaxFOV);
        mapCamera.GetComponent<Camera>().fieldOfView = currentFOV;
    }

    private void ResolveMapCamera()
    {
        if (mapCameraRoot)
            return;

        var map = Map.Instance;
        if (!map)
            return;

        mapCameraRoot = map.CameraRoot.gameObject;
        mapCameraPitch = map.CameraPitch.gameObject;
        mapCamera = map.Camera.gameObject;
        tilt = map.Camera.GetComponent<TiltShift>();
    }

    public void UpdateMap()
    {
        // Try to resolve map camera components.
        ResolveMapCamera();

        // Ensure that camera exists.
        if (!mapCameraRoot)
            return;

        // Cap delta time to avoid jumps at low framerates.
        const float maxDeltaTime = 1 / 50.0f;
        var dt = Mathf.Min(maxDeltaTime, Time.deltaTime);

        // Get the view angle slider value.
        var slider = viewAngleSlider.GetComponent<sliderWidget>();
        var viewAngle = Mathf.Clamp(slider.returnValue, slider.minValue, slider.maxValue);
        mapCameraPitch.transform.localRotation = Quaternion.Euler(viewAngle, 0, 0);

        // Enable or disable slider based on current camera mapping mode (3d / contours).
        var interactive = serverUtils.GetServerBool("mapInteractive");
        slider.SetInputEnabled(Is3D && interactive);
        slider.SetVisible(Is3D && interactive);

        // Update terrain material based on view angle.
        UpdateTerrain();

        // Update camera zoom level.
        UpdateZoom();

        // Scale and pan speed should be adjusted to how far out we are zoomed, 100% speed at zoomed out, 10% speed at zoomed in
        float zoomeSpeedMultiplier = graphicsMaths.remapValue(zoom, 0f, 1f, 0.1f, 1f);

        if (pressed && interactive)
        {
            _lastPressTime = Time.time;

            // Get difference from this touch position and last.
            // If we've not yet touched the screen the delta can't be reliably calculated.
            if (previousHit.Point == Vector3.zero)
                { previousHit = currentHit; return; }

            var touchDelta = new Vector2(previousHit.Point.x - currentHit.Point.x, previousHit.Point.y - currentHit.Point.y);
            previousHit = currentHit;

            //scale or rotate the camera
            if (touches == 2)
            {
                //slowly increase the rotate amount so we don't get jittery rotations when fingers are close together
                rotateAmount = Mathf.Lerp(rotateAmount, rotDelta, dt * rotateSpeed);

                //apply the rotation
                mapCameraRoot.transform.Rotate(0, rotateAmount, 0);

                //scale amount is -1 to 1
                scaleAmount = graphicsMaths.remapValue(scaleDelta, 0.5f, 1.5f, -1f, 1f);

                var cam = mapCamera.GetComponent<Camera>();
                if (cam && cam.orthographic)
                {
                    var camSize = cam.orthographicSize;
                    camSize -= scaleAmount * (scaleSpeed * zoomeSpeedMultiplier);
                    cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, camSize, dt * 0.05f);
                    cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, camMinOrthoSize, camMaxOrthoSize);
                }
                else
                { 
                    // Add scale amount to current camera position value
                    var camZ = mapCamera.transform.localPosition.z;
                    camZ += scaleAmount * (scaleSpeed * zoomeSpeedMultiplier);

                    // Add this to the camera pos z
                    mapCamera.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(mapCamera.transform.localPosition.z, camZ, dt * 0.2f));
                    mapCamera.transform.localPosition = new Vector3(0, 0, Mathf.Clamp(mapCamera.transform.localPosition.z, camMaxZ, camMinZ));
                }
            }

            if (touches >= 3 && !IsTopDown)
            {
                // Adjust view angle as gesture moves up and down the screen.
                viewAngle = Mathf.Clamp(viewAngle + touchDelta.y * pitchSpeed * Time.deltaTime, slider.minValue, slider.maxValue);
                slider.SetValue(viewAngle);
                mapCameraPitch.transform.localRotation = Quaternion.Euler(viewAngle, 0, 0);
            }

            //inverse scale the local position constraints so we can't scroll when zoomed out
            maxScroll = (mapCamera.transform.localPosition.z - camMinZ) / (camMaxZ - camMinZ) * (rootMaxX - -rootMaxX) + -rootMaxX;
            maxScroll = Mathf.Abs(maxScroll);

            //pan the camera
            if (touches == 1)
            {
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
            }

        }

        // Snap camera to player vessel if desired.
        if (snapToPlayerVessel && NavSubPins.Instance)
            UpdateCameraSnapping();
    }

    void Start()
    {
        tilt = mapCamera.GetComponent<TiltShift>();
        UpdateTerrain();
    }

	void Update()
    {
        UpdateMap();
	    UpdateButtons();
    }

    private void UpdateButtons()
    {
        var isTopDown = serverUtils.GetServerBool("mapTopDown");
        if (button3dMapping)
            button3dMapping.active = !isTopDown;
        if (buttonContourMapping)
            buttonContourMapping.active = isTopDown;
    }

    private void UpdateCameraSnapping()
    {
        if (Time.time < (_lastPressTime + CameraSnapDelay))
            return;

        if (megMapCameraEventManager.Instance.running)
            return;

        var player = serverUtils.GetPlayerVessel();
        var pin = NavSubPins.Instance.GetVesselPin(player);
        if (!pin)
            return;

        var p = pin.transform.position;
        var c = mapCameraRoot.transform.position;
        var target = new Vector3(p.x, c.y, p.z);

        mapCameraRoot.transform.position = Vector3.SmoothDamp(c,
            target, ref _cameraSmoothVelocity, CameraSnapSmoothTime);
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
        var force3D = Is3D && !cameraEventManager.running;
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

        // Update water visibility.
        var waterColor = Color.Lerp(WaterColor2d, WaterColor3d, t);
        var waterTargetAlpha = serverUtils.GetServerData("waterLayer", 1);
        _waterAlpha = Mathf.SmoothDamp(_waterAlpha, waterTargetAlpha, ref _waterSmoothVelocity, 0.25f);
        waterColor.a *= _waterAlpha;
        if (_waterMaterial)
            _waterMaterial.SetColor("_MainColor", waterColor);
        if (Water)
            Water.gameObject.SetActive(_waterAlpha > 0.01f);

        // Update acid visibility.
        var acidColor = Color.Lerp(AcidColor2d, AcidColor3d, t);
        var acidTargetAlpha = serverUtils.GetServerData("acidLayer", 0);
        _acidAlpha = Mathf.SmoothDamp(_acidAlpha, acidTargetAlpha, ref _acidSmoothVelocity, 0.25f);
        if (_acidMaterial)
        {
            acidColor.a *= _acidAlpha;
            _acidMaterial.SetColor("_FogColor", acidColor);
            _acidMaterial.SetFloat("_Opacity", _acidAlpha);
            _acidMaterial.SetFloat("_RefractionAmount", _acidMaxRefraction * _acidAlpha);
        }
        if (Acid)
            Acid.gameObject.SetActive(_acidAlpha > 0.01f);
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

        if (!Water && Map.Instance)
            Water = Map.Instance.Water;
        if (Water)
            _waterMaterial = Water.GetComponent<Renderer>().material;

        if (!Acid && Map.Instance)
            Acid = Map.Instance.Acid;
        if (Acid)
        {
            _acidMaterial = Acid.GetComponent<Renderer>().material;
            _acidMaxRefraction = _acidMaterial.GetFloat("_RefractionAmount");
        }

        _terrainInitialized = true;
    }

    /** Toggle the acid layer. */
    public void ToggleAcidLayer()
    {
        var wasOn = serverUtils.GetServerBool("acidlayer");
        serverUtils.PostServerData("acidlayer", wasOn ? 0 : 1);
    }

    /** Toggle the water layer. */
    public void ToggleWaterLayer()
    {
        var wasOn = serverUtils.GetServerBool("waterlayer");
        serverUtils.PostServerData("waterlayer", wasOn ? 0 : 1);
    }

}
