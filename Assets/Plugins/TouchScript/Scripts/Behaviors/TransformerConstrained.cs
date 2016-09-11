/*
 * @author Valentin Simonov / http://va.lent.in/
 */

using System;
using System.Collections.Generic;
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
        public Vector2 ZoomLimits = new Vector2(0, float.MaxValue);


        #endregion

        #region Private variables

        private Transform cachedTransform;
        private List<ITransformGesture> gestures = new List<ITransformGesture>();

        #endregion

        #region Unity methods

        private void Awake()
        {
            cachedTransform = transform;
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

        /// <inheritdoc />
        private void ApplyTransform(TransformGesture gesture, Transform target)
        {
            if (!Mathf.Approximately(gesture.DeltaScale, 1f))
            {
                var scale = target.localScale * gesture.DeltaScale;
                target.localScale = new Vector3(
                    Mathf.Clamp(scale.x, ZoomLimits.x, ZoomLimits.y),
                    Mathf.Clamp(scale.y, ZoomLimits.x, ZoomLimits.y),
                    scale.z);

                if (!Mathf.Approximately(scale.x, target.localScale.x))
                    return;

                if (!Mathf.Approximately(scale.y, target.localScale.y))
                    return;
            }

            if (!Mathf.Approximately(gesture.DeltaRotation, 0f))
                target.rotation = Quaternion.AngleAxis(gesture.DeltaRotation, gesture.RotationAxis) * target.rotation;

            if (gesture.DeltaPosition != Vector3.zero)
            {
                var s = target.localScale.x;
                var min = target.parent.TransformPoint(PanLimits.min * s);
                var max = target.parent.TransformPoint(PanLimits.max * s);

                var p = target.position + gesture.DeltaPosition;
                target.position = Vector3.Min(Vector3.Max(p, min), max);
            }

        }

        #endregion
    }
}