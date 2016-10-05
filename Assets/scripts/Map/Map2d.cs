using UnityEngine;
using DG.Tweening;
using Meg.Networking;
using TouchScript.Behaviors;
using TouchScript.Gestures;
using TouchScript.Gestures.Base;

public class Map2d : Singleton<Map2d>
{

    public Transform Root;

    public float Pan3DSpeed = 750f;
    public float Pan3DSmoothTime = 0.2f;
    public float Zoom3DSpeed = 5;
    public float Zoom3DSmoothTime = 0.2f;
    public AnimationCurve ZoomResponse;


	private void Awake()
	    { SetInstance(this); }

    private TransformGesture _gesture;
    private TransformerConstrained _transformer;

    private Vector3 _translation;
    private Vector3 _translationVelocity;

    private float _zoom;
    private float _zoomVelocity;

    private void Start()
    {
        _gesture = Root.GetComponent<TransformGesture>();
        _transformer = Root.GetComponent<TransformerConstrained>();
    }

    private void Update()
    {
        var mapData = serverUtils.MapData;
        if (!mapData)
            return;

        TransformGestureBase.TransformType type = 0;
        if (mapData.mapCanPan)
            type = type | TransformGestureBase.TransformType.Translation;
        if (mapData.mapCanZoom)
            type = type | TransformGestureBase.TransformType.Scaling;
        // if (mapData.mapCanRotate)
        //    type = type | TransformGestureBase.TransformType.Rotation;

        // Allow zooming with the mouse wheel.
        var dz = Input.GetAxis("MouseAxis3");
        if (!Mathf.Approximately(dz, 0))
            MouseWheelZoom(dz);

        _gesture.Type = type;
    }

    public void TriggerEventFromState(megMapCameraEventManager.State state)
    {
        // Cancel any existing camera animations.
        if (DOTween.IsTweening(Root))
            DOTween.Kill(Root);

        // Interrupt active transform gesture (if any).
        _gesture.Cancel();

        var target = state.toPosition;
        target.z = 0;

        var zoom = Mathf.Clamp(state.toZoom, 
            _transformer.ZoomSoftLimits.x, 
            _transformer.ZoomSoftLimits.y);

        var duration = state.completeTime;
        Root.DOScale(zoom, duration).SetEase(Ease.OutSine);
        Root.DOLocalMove(target, duration).SetEase(Ease.OutSine);
    }

    public bool Capture(ref megMapCameraEventManager.State state)
    {
        state.toPosition = Root.localPosition;
        state.toOrientation = Vector3.zero;
        state.toZoom = Root.localScale.x;
        state.is2d = true;

        return true;
    }

    private void MouseWheelZoom(float dz)
    {
        if (DOTween.IsTweening(Root))
            return;

        var state = new megMapCameraEventManager.State();
        var world = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
        var target = Root.InverseTransformPoint(world);
        target.z = 0;

        var dZoom = (1 + dz * 0.5f);
        state.toZoom = Root.localScale.x * dZoom;
        if (state.toZoom > _transformer.ZoomSoftLimits.y)
            return;
        if (state.toZoom < _transformer.ZoomSoftLimits.x)
            return;

        state.toPosition = -target * state.toZoom;
        state.toOrientation = Vector3.zero;
        state.is2d = true;
        state.completeTime = 0.25f;

        TriggerEventFromState(state);
    }

}
