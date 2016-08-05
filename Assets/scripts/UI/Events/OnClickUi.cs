using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class OnClickUi : MonoBehaviour, IPointerClickHandler
{

    public UnityEvent onClick;

    private void Start()
    {
        if (onClick == null)
            onClick = new UnityEvent();

    }

    public void OnPointerClick(PointerEventData ped)
    {
        if (onClick != null)
            onClick.Invoke();
    }

}
