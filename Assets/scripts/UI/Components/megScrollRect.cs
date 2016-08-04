using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Meg.UI
{

    public class megScrollRect : UnityEngine.UI.ScrollRect, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public bool Dragging { get; private set; }

        private RectTransform _maskTransform;
        private RectTransform _rectTransform;
        private float _lastScrollPos;

        protected override void Start()
        {
            base.Start();
            _lastScrollPos = verticalNormalizedPosition;
            _rectTransform = GetComponent<RectTransform>();
            _maskTransform = content.parent.GetComponent<RectTransform>();
        }

        protected virtual void Update()
        {
            var scrollPos = verticalNormalizedPosition;
            if (!Mathf.Approximately(scrollPos, _lastScrollPos))
                Dragging = true;

            _lastScrollPos = scrollPos;
        }

        protected override void OnDisable()
        {
            Dragging = false;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            Dragging = true;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            Dragging = false;
        }

        /** Handle a click on the sonar display. */
        public void OnPointerClick(PointerEventData ped)
        {
            Dragging = true;
        }

        public void OnPointerDown(PointerEventData ped)
        {
        }

        public void OnPointerUp(PointerEventData ped)
        {
        }

        public void CenterOnItem(RectTransform target, float smoothTime = 0)
        {
            if (!target || !_rectTransform)
                return;

            // Item is here
            var itemCenterPositionInScroll = GetWorldPointInWidget(_rectTransform, GetWidgetWorldPoint(target));
            // Debug.Log("Item Anchor Pos In Scroll: " + itemCenterPositionInScroll);
            
            // But must be here
            var targetPositionInScroll = GetWorldPointInWidget(_rectTransform, GetWidgetWorldPoint(_maskTransform));
            // Debug.Log("Target Anchor Pos In Scroll: " + targetPositionInScroll);

            // So it has to move this distance
            var difference = targetPositionInScroll - itemCenterPositionInScroll;
            difference.z = 0f;

            // Clear axis data that is not enabled in the scrollrect
            if (!horizontal)
                difference.x = 0f;
            if (!vertical)
                difference.y = 0f;

            var normalizedDifference = new Vector2(
                difference.x / (content.rect.size.x - _rectTransform.rect.size.x),
                difference.y / (content.rect.size.y - _rectTransform.rect.size.y));
            // Debug.Log("Normalized difference: " + normalizedDifference);

            var newNormalizedPosition = normalizedPosition - normalizedDifference;
            // Debug.Log("New normalized position: " + newNormalizedPosition);

            if (movementType != MovementType.Unrestricted)
            {
                newNormalizedPosition.x = Mathf.Clamp01(newNormalizedPosition.x);
                newNormalizedPosition.y = Mathf.Clamp01(newNormalizedPosition.y);
                // Debug.Log("Clamped normalized position: " + newNormalizedPosition);
            }

            if (smoothTime <= 0)
                normalizedPosition = newNormalizedPosition;
            else
                DOTween.To(() => normalizedPosition, x => normalizedPosition = x, newNormalizedPosition, smoothTime);
        }

        private Vector3 GetWidgetWorldPoint(RectTransform target)
        {
            // Pivot position + item size has to be included
            var pivotOffset = new Vector3(
                (0.5f - target.pivot.x) * target.rect.size.x,
                (0.5f - target.pivot.y) * target.rect.size.y, 0f);

            var localPosition = target.localPosition + pivotOffset;
            return target.parent.TransformPoint(localPosition);
        }

        private Vector3 GetWorldPointInWidget(RectTransform target, Vector3 worldPoint)
            { return target.InverseTransformPoint(worldPoint); }

    }

}