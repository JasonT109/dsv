using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Meg.UI
{

    public class megScrollRect : UnityEngine.UI.ScrollRect, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public bool Dragging;

        private float _lastScrollPos;

        private void Start()
        {
            _lastScrollPos = verticalNormalizedPosition;
        }

        private void Update()
        {
            var scrollPos = verticalNormalizedPosition;
            if (!Mathf.Approximately(scrollPos, _lastScrollPos))
                Dragging = true;

            _lastScrollPos = scrollPos;
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

    }

}