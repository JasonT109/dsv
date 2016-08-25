using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class OnClickUi : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{

    public UnityEvent onClick;
    public UnityEvent onPress;
    public UnityEvent onRelease;

    private void Start()
    {
        if (onClick == null)
            onClick = new UnityEvent();
        if (onPress == null)
            onPress = new UnityEvent();
        if (onRelease == null)
            onRelease = new UnityEvent();
    }

    public void OnPointerClick(PointerEventData ped)
    {
        if (onClick != null)
            onClick.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onPress != null)
            onPress.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onRelease != null)
            onRelease.Invoke();
    }
}
