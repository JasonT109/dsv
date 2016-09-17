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

        private Transform cachedTransform;
        private readonly List<ITransformGesture> gestures = new List<ITransformGesture>();
        private Vector3 _lastOkPosition;
        private Vector3 _lastOkScale;
        private Vector3 _smoothPositionVelocity;
        private Vector3 _smoothScaleVelocity;

        private Vector3 _targetPosition;
        private Vector3 _targetPositionVelocity;
        private Vector3 _targetScale;
        private Vector3 _targetScaleVelocity;

        #endregion

        #region Unity methods

        private void Awake()
        {
            cachedTransform = transform;
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

                gestures.Add(transformGesture);
                transformGesture.Transformed += transformHandler;
                transformGesture.TransformStarted += transformStartedHandler;
                transformGesture.TransformCompleted += transformCompletedHandler;
            }

        }

        private void OnDisable()
        {
            for (var i = 0; i < gestures.Count; i++)
            {
                var transformGesture = gestures[i];
                transformGesture.Transformed -= transformHandler;
            }
            gestures.Clear();
        }

        /** Draw gizmos for this zone when object is selected. */
        private void OnDrawGizmosSelected()
        {
            var min = transform.TransformPoint(PanLimits.min);
            var max = transform.TransformPoint(PanLimits.max);
            var extents = (max - min) * 0.5f;
            var center = min + extents;

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(center, extents);
        }

        #endregion

        #region Event handlers

        private void transformHandler(object sender, EventArgs e)
        {
            var gesture = sender as TransformGesture;
            ApplyTransform(gesture, cachedTransform);
        }

        private void transformStartedHandler(object sender, EventArgs e)
        {
        }

        private void transformCompletedHandler(object sender, EventArgs e)
        {
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
                target.rotation = Quaternion.AngleAxis(gesture.DeltaRotation, gesture.RotationAxis)*target.rotation;

            if (gesture.DeltaPosition != Vector3.zero)
            {
                var s = _targetScale.x;
                var min = target.parent.TransformPoint(PanLimits.min * s);
                var max = target.parent.TransformPoint(PanLimits.max * s);
                var p = _targetPosition + gesture.DeltaPosition;
                _targetPosition = Vector3.Min(Vector3.Max(p, min), max);
            }

        }

        private void LateUpdate()
        {
            // Return to acceptable values.
            var scale = _targetScale.x;
            if (scale < ZoomSoftLimits.x || scale > ZoomSoftLimits.y)
                ReturnToLastOkState();

            // Smooth out pan and zoom.
            transform.localScale = Vector3.SmoothDamp(transform.localScale, 
                _targetScale, ref _targetScaleVelocity, SmoothTime);

            _targetPosition.z = transform.parent.position.z;
            transform.position = Vector3.SmoothDamp(transform.position,
                _targetPosition, ref _targetPositionVelocity, SmoothTime);
        }

        private void ReturnToLastOkState()
        {
            _targetScale = Vector3.SmoothDamp(_targetScale,
                _lastOkScale, ref _smoothScaleVelocity, ZoomSmoothTime);

            _targetPosition = Vector3.SmoothDamp(_targetPosition,
                _lastOkPosition, ref _smoothPositionVelocity, ZoomSmoothTime);
        }

        private static float RemapValue(float value, float inMin, float inMax, float outMin, float outMax)
            { return ((value - inMin) / (inMax - inMin) * (outMax - outMin)) + outMin; }


        #endregion
    }
}