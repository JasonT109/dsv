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
        private Vector3 _targetScale;
        private readonly List<ITransformGesture> _gestures = new List<ITransformGesture>();

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
            var scale = _targetScale.x * gesture.DeltaScale;
            if (scale < ZoomSoftLimits.x || scale > ZoomSoftLimits.y)
            {
                gesture.Cancel(true, true);
                return;
            }

            _targetScale *= gesture.DeltaScale;

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

        #endregion


        public void Move3D(Vector3 translation, float zoom)
        {
            if (DOTween.IsTweening(transform))
                return;

            var scale = _targetScale * (1 + zoom);
            if (scale.x < ZoomSoftLimits.x)
                zoom = 0;
            if (scale.x > ZoomSoftLimits.y)
                zoom = 0;

            _targetScale *= (1 + zoom);
            _targetPosition += translation;

            var dp = _targetPosition * zoom;
            _targetPosition += dp;
        }

    }
}
