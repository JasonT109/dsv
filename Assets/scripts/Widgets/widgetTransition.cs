using UnityEngine;
using System.Collections;

public class widgetTransition : MonoBehaviour {

    public float transitionSpeed = 15.0f;
    public bool scaleOnEnable = true;
    private bool doTransition = false;
    private Vector3 initialScale;

	// Use this for initialization
	void Awake ()
    {
        initialScale = transform.localScale;
    }

    void OnEnable()
    {
        if (scaleOnEnable)
        {
            doTransition = true;
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    // Update is called once per frame
    void Update ()
    {
	    if (doTransition)
        {
            if (scaleOnEnable)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, initialScale, Time.deltaTime * transitionSpeed);
                if (transform.localScale.x > (initialScale.x * 0.98))
                {
                    transform.localScale = initialScale;
                    doTransition = false;
                }
            }
        }
	}
}
