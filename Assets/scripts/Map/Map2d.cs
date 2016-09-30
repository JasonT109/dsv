using UnityEngine;
using System.Collections;
using DG.Tweening;
using Meg.EventSystem;
using Meg.Networking;
using TouchScript.Behaviors;
using TouchScript.Gestures;
using TouchScript.Gestures.Base;

public class Map2d : Singleton<Map2d>
{

    public Transform Root;

	private void Awake()
	    { SetInstance(this); }

    private TransformGesture _gesture;
    private TransformerConstrained _transformer;

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

}
