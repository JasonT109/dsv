/*
 * @author Valentin Simonov / http://va.lent.in/
 */

using System;
using System.Collections.Generic;
using DG.Tweening;
using TouchScript.Gestures;
using UnityEngine;

namespace TouchScript.Behaviors
{
    [AddComponentMenu("TouchScript/Behaviors/Transformer (Constrained)")]
    public class TransformerConstrained : MonoBehaviour
    {
        #region Public properties

        /** Smooth time for transform. */
        public float SmoothTime = 0.25f;

        /** Bounds of the panning zone (local space). */
        public Bounds PanLimits;

        /** Zoom limits. */
        public Vector2 ZoomSoftLimits = new Vector2(0, float.MaxValue);
        public float ZoomSmoothTime = 1;


        #endregion

        #region Private variables

        private Transform _cachedTransform;
        private readonly List<ITransformGesture> _gestures = new List<ITransformGesture>();
        private Vector3 _lastOkPosition;
        private Vector3 _lastOkScale;
        private Vector3 _smoothPositionVelocity;
        private Vector3 _smoothScaleVelocity;
        private Vector3 _targetPosition;
        private Vector3 _targetScale;

        #endregion

        #region Unity methods

        private void Awake()
        {
            _cachedTransform = transform;
            _targetPosition = transform.position;
            _targetScale = transform.localScale;
        }

        private void OnEnable()
        {
            var g = GetComponents<Gesture>();
            for (var i = 0; i < g.Length; i++)
            {
                var transformGesture = g[i] as ITransformGesture;
                if (transformGesture == null) continue;

                _gestures.Add(transformGesture);
                transformGesture.Transformed += transformHandler;
            }

        }

        private void OnDisable()
        {
            for (var i = 0; i < _gestures.Count; i++)
            {
                var transformGesture = _gestures[i];
                transformGesture.Transformed -= transformHandler;
            }
            _gestures.Clear();
        }

        #endregion

        #region Event handlers

        private void transformHandler(object sender, EventArgs e)
        {
            var gesture = sender as TransformGesture;
            ApplyTransform(gesture, _cachedTransform);
        }

        /// <inheritdoc />
        private void ApplyTransform(TransformGesture gesture, Transform target)
        {
            _targetScale *= gesture.DeltaScale;

            // Remember last acceptable values so we can reset to them.
            var scale = _targetScale.x;
            if (scale > ZoomSoftLimits.x && scale < ZoomSoftLimits.y)
            {
                _lastOkScale = _targetScale;
                _lastOkPosition = _targetPosition;
            }
            else if (scale < ZoomSoftLimits.x)
            {
                _targetScale = new Vector3(ZoomSoftLimits.x, ZoomSoftLimits.x, _targetScale.z);
                return;
            }

            if (!Mathf.Approximately(gesture.DeltaRotation, 0f))
                target.rotation = Quaternion.AngleAxis(gesture.DeltaRotation, gesture.RotationAxis) * target.rotation;

            if (gesture.DeltaPosition != Vector3.zero)
                _targetPosition += gesture.DeltaPosition;
        }

        private void LateUpdate()
        {
            // Respect tween values.
            if (DOTween.IsTweening(transform))
            {
                _targetScale = transform.localScale;
                _targetPosition = transform.position;
            }

            // Return to acceptable zoom values when needed.
            var scale = _targetScale.x;
            if (scale < ZoomSoftLimits.x || scale > ZoomSoftLimits.y)
                ReturnToLastOkState();

            // Apply scaling.
            transform.localScale = _targetScale;

            // Constrain panning.
            transform.position = _targetPosition;
            var min = PanLimits.min * _targetScale.x;
            var max = PanLimits.max * _targetScale.x;
            var afterLocal = transform.localPosition;
            var constrainedLocal = Vector3.Min(Vector3.Max(afterLocal, min), max);
            transform.localPosition = constrainedLocal;
            _targetPosition = transform.position;
        }

        private void ReturnToLastOkState()
        {
            _targetScale = Vector3.SmoothDamp(_targetScale,
                _lastOkScale, ref _smoothScaleVelocity, ZoomSmoothTime);

            _targetPosition = Vector3.SmoothDamp(_targetPosition,
                _lastOkPosition, ref _smoothPositionVelocity, ZoomSmoothTime);
        }


        #endregion
    }
}
