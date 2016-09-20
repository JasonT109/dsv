using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class RadialSlider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    bool isPointerDown = false;
    public float value;
    public Camera RayCam;
    public bool isTouch = false;
    

    public Vector2 DebugTouchPos;
    public float debugDist;
    public bool TouchDetected = false;

    //public GraphicRaycaster ray;

    // Called when the pointer enters our GUI component.
    // Start tracking the mouse
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine("TrackPointer");
    }

    // Called when the pointer exits our GUI component.
    // Stop tracking the mouse
    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine("TrackPointer");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        //Debug.Log("mousedown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        //Debug.Log("mousedown");
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            TouchDetected = true;

            Vector2 localPos; // Mouse position  
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.GetTouch(0).position, RayCam, out localPos);

            var Dist = Vector2.Distance(localPos, new Vector2(transform.localPosition.x, transform.localPosition.y));
            debugDist = Dist;
            if (Dist < 1f && Dist > 0.3f)
            {
                isPointerDown = true;
                isTouch = true;
            }
            else
            {
                isPointerDown = false;
                isTouch = false;
            }
        }
        else
        {
            TouchDetected = false;
        }
    }

    // mainloop
    IEnumerator TrackPointer()
    {
        var ray = GetComponentInParent<GraphicRaycaster>();
        //var input = FindObjectOfType<StandaloneInputModule>();

        //if (input != null)//ray != null)// && input != null)
        {
            while (Application.isPlaying)
            {

                // TODO: if mousebutton down
                if (isPointerDown)
                {

                    Vector2 localPos; // Mouse position  
                    if(isTouch)
                    {
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.GetTouch(0).position, RayCam, out localPos);
                        DebugTouchPos = Input.GetTouch(0).position;
                    }
                    else
                    {
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, ray.eventCamera, out localPos);
                    }

                    // local pos is the mouse position.
                    float angle = (Mathf.Atan2(-localPos.y, localPos.x) * 180f / Mathf.PI + 180f) / 360f;

                    angle += 0.5f;

                    if(angle > 1f)
                    {
                        angle -= 1f;
                    }

                    bool DoNothing = false;

                    if(value > 0.8f)
                    {
                        //value can only go up from here or down gradually, not down suddenly
                        if(angle < 0.2f)
                        {
                            //do nothing. new angle is too small
                            DoNothing = true;
                        }
                        else
                        {
                            
                        }
                    }

                    if (value < 0.2f)
                    {
                        //value can only go down from here or up gradually, not up suddenly
                        if (angle > 0.8f)
                        {
                            //do nothing. new angle is too large
                            DoNothing = true;
                        }
                        else
                        {
                             
                        }
                    }

                    if (!DoNothing)
                    {
                        GetComponent<Image>().fillAmount = angle;
                        value = angle;
                        if(value > 0.96f)
                        {
                            value = 1f;
                        }
                    }
                }

                yield return 0;
            }
        }
    }





}
