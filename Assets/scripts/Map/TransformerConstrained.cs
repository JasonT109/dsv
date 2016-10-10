/*
 * @author Valentin Simonov / http://va.lent.in/
 */

using System;
using System.Collections.Generic;
using DG.Tweening;
using Meg.Networking;
using TouchScript.Gestures;
using UnityEngine;

namespace TouchScript.Behaviors
{
    [AddComponentMenu("TouchScript/Behaviors/Transformer (Constrained)")]
    public class TransformerConstrained : MonoBehaviour
    {
        #region Public properties

        /** Bounds of the panning zone (local space). */
        public Bounds PanLimits;

        /** Zoom limits. */
        public Vector2 ZoomSoftLimits = new Vector2(0, float.MaxValue);

        /** Zoom delta limits. */
        public Vector2 ZoomDeltaLimits = new Vector2(0.9f, 1.1f);

        #endregion

        #region Private variables

        private Transform _cachedTransform;
        private Vector3 _targetPosition;
        private float _targetScale;
        private readonly List<ITransformGesture> _gestures = new List<ITransformGesture>();

        private Vector3 _deltaPosition;
        private float _deltaScale = 1;

        private Vector3 _translation;
        private Vector3 _translationVelocity;

        private float _zoom;
        private float _zoomVelocity;


        #endregion

        #region Unity methods

        private void Awake()
        {
            _cachedTransform = transform;
            _targetPosition = transform.position;
            _targetScale = transform.localScale.x;
        }

        private void OnEnable()
        {
            var g = GetComponents<Gesture>();
            for (var i = 0; i < g.Length; i++)
            {
                var transformGesture = g[i] as ITransformGesture;
                if (transformGesture == null) continue;

                _gestures.Add(transformGesture);
                transformGesture.TransformStarted += transformStartedHandler;
                transformGesture.Transformed += transformHandler;
                transformGesture.TransformCompleted += transformCompleteHandler;
            }

        }

        private void OnDisable()
        {
            for (var i = 0; i < _gestures.Count; i++)
            {
                var transformGesture = _gestures[i];
                transformGesture.Transformed -= transformHandler;
                transformGesture.TransformCompleted -= transformCompleteHandler;
            }
            _gestures.Clear();
        }

        #endregion

        #region Event handlers

        private void transformStartedHandler(object sender, EventArgs e)
        {
            _deltaPosition = Vector3.zero;
            _translation = Vector3.zero; ;
            _deltaScale = 1;
            _zoom = 1;
            _translationVelocity = Vector3.zero;
            _zoomVelocity = 0;
        }

        private void transformHandler(object sender, EventArgs e)
        {
            var gesture = sender as TransformGesture;
            ApplyTransform(gesture, _cachedTransform);
        }

        private void transformCompleteHandler(object sender, EventArgs e)
        {
        }

        /// <inheritdoc />
        private void ApplyTransform(TransformGesture gesture, Transform target)
        {
            _deltaPosition = gesture.DeltaPosition;
            _translation = gesture.DeltaPosition;
            _deltaScale = gesture.DeltaScale;
        }

        private void LateUpdate()
        {
            // Smooth out panning / scaling gesture deltas.
            var smoothTime = serverUtils.GetServerData("mapSmoothTime", 0.1f);
            _translation = Vector3.SmoothDamp(_translation, _deltaPosition, ref _translationVelocity, smoothTime);
            _zoom = Mathf.SmoothDamp(_zoom, _deltaScale, ref _zoomVelocity, smoothTime);
            _deltaPosition = Vector3.zero;
            _deltaScale = 1;

            // Respect tween values.
            if (DOTween.IsTweening(transform))
            {
                _targetScale = transform.localScale.x;
                _targetPosition = transform.position;
            }
            else
                PanAndZoom(_translation, _zoom, smoothTime);

            // Apply scaling.
            transform.localScale = Vector3.one * _targetScale;

            // Constrain panning to respect bounds.
            transform.position = _targetPosition;
            var min = PanLimits.min * _targetScale;
            var max = PanLimits.max * _targetScale;
            var afterLocal = transform.localPosition;
            var constrainedLocal = Vector3.Min(Vector3.Max(afterLocal, min), max);
            transform.localPosition = constrainedLocal;
            _targetPosition = transform.position;

        }

        #endregion


        private void PanAndZoom(Vector3 translation, float zoom, float smoothTime)
        {
            var scale = _targetScale * zoom;
            var zooming = !Mathf.Approximately(zoom, 1);
            if (zooming && scale < ZoomSoftLimits.x)
            {
                zoom = 1;
                translation = Vector3.zero;
            }
            else if (zooming && scale > ZoomSoftLimits.y)
            {
                zoom = 1;
                translation = Vector3.zero;
            }

            _targetScale *= zoom;

            if (smoothTime <= 0)
            {
                // When no smoothing is applied, allow simultaneous pan/zoom.
                _targetPosition += translation;
            }
            else
            {
                // When smoothing is applied, only apply pan if not zooming.
                // When zooming, keep the current focal point in center of screen.
                if (zooming)
                {
                    var dp = _targetPosition * (zoom - 1);
                    _targetPosition += dp;
                }
                else
                    _targetPosition += translation;
            }
        }

    }
}
