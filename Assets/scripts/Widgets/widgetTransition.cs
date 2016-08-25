using UnityEngine;
using System.Collections;

public class widgetTransition : MonoBehaviour {

    public float transitionTime = 15.0f;
    public bool scaleOnEnable = true;
    public bool repositionChildren = false;
    public float childOffsetX = 5f;
    public float childOffsetY = 5f;
    public GameObject parentObject;
    private Transform c;
    private float startTime = 0f;

    private bool doTransition = false;
    private Vector3 initialScale;
    private Vector3 startScale = new Vector3(0.1f, 0.1f, 0.1f);

    void Awake ()
    {
        initialScale = transform.localScale;
    }

    void Start ()
    {
        if (!parentObject)
            return;

        //create a new transform for the child object, this we can then offset to make the box visually more appealing
        if (repositionChildren)
        {
            c = transform.GetChild(0);

            //set the childs initial position
            if (parentObject.transform.localPosition.x > 0)
            {
                c.localPosition = new Vector3(-childOffsetX, c.localPosition.y, c.localPosition.z);
            }
            else
            {
                c.localPosition = new Vector3(childOffsetX, c.localPosition.y, c.localPosition.z);
            }

            if (parentObject.transform.localPosition.y < 0)
            {
                c.localPosition = new Vector3(c.localPosition.x, childOffsetY, c.localPosition.z);
            }
            else
            {
                c.localPosition = new Vector3(c.localPosition.x, -childOffsetY, c.localPosition.z);
            }
        }
    }

    void OnEnable()
    {
        if (!parentObject)
            return;
        
        c = transform.GetChild(0);

        if (scaleOnEnable)
        {
            doTransition = true;
            startTime = 0;
            transform.localScale = startScale;

            //set the childs initial position
            if (parentObject.transform.localPosition.x > 0)
            {
                c.localPosition = new Vector3(-childOffsetX, c.localPosition.y, c.localPosition.z);
            }
            else
            {
                c.localPosition = new Vector3(childOffsetX, c.localPosition.y, c.localPosition.z);
            }

            if (parentObject.transform.localPosition.y < 0)
            {
                c.localPosition = new Vector3(c.localPosition.x, childOffsetY, c.localPosition.z);
            }
            else
            {
                c.localPosition = new Vector3(c.localPosition.x, -childOffsetY, c.localPosition.z);
            }
        }
    }

    void Update ()
    {
        if (!parentObject)
            return;

        //set the root position to them same as our spawning button
        transform.position = parentObject.transform.position;

        if (doTransition)
        {
            startTime += Time.deltaTime;
            if (scaleOnEnable)
            {
                float percentDone = startTime / transitionTime;

                transform.localScale = Vector3.Lerp(startScale, initialScale, percentDone);

                //set the childs initial position
                if (repositionChildren)
                {
                    if (parentObject.transform.localPosition.x > 0)
                    {
                        c.localPosition = new Vector3(-childOffsetX, c.localPosition.y, c.localPosition.z);
                    }
                    else
                    {
                        c.localPosition = new Vector3(childOffsetX, c.localPosition.y, c.localPosition.z);
                    }

                    if (parentObject.transform.localPosition.y < 0)
                    {
                        c.localPosition = new Vector3(c.localPosition.x, childOffsetY, c.localPosition.z);
                    }
                    else
                    {
                        c.localPosition = new Vector3(c.localPosition.x, -childOffsetY, c.localPosition.z);
                    }
                }

                //stop when transition time reached
                if (startTime > transitionTime)
                {
                    doTransition = false;
                }
            }
        }

        //lerp the childs position
        if (repositionChildren)
        {
            Vector3 t = new Vector3();

            if (parentObject.transform.localPosition.x > 0)
            {
                t = new Vector3(-childOffsetX, c.localPosition.y, c.localPosition.z);
            }
            else
            {
                t = new Vector3(childOffsetX, c.localPosition.y, c.localPosition.z);
            }

            if (parentObject.transform.localPosition.y < 0)
            {
                t = new Vector3(t.x, childOffsetY, t.z);
            }
            else
            {
                t = new Vector3(t.x, -childOffsetY, t.z);
            }

            //c.localPosition = Vector3.Lerp(c.localPosition, t, Time.deltaTime * transitionSpeed);
            c.localPosition = t;
        }
    }
}
