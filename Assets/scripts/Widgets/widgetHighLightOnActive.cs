using UnityEngine;
using System.Collections;
using Meg.Maths;

public class widgetHighLightOnActive : MonoBehaviour
{
    public bool doAlpha = false;
    public bool doScaleX = false;
    public float maxAlpha = 1.0f;
    public float transitionTime = 1.0f;
    public float startDelay = 0.0f;
    float targetAlpha = 1.0f;
    float startAlpha = 0.0f;
    Color initialColor;
    float alphaLerpStart;
    bool isAlphaLerping = false;
    Renderer r;
    Material m;
    float startScaleX = 0.1f;
    float endScaleX = 1.0f;
    bool isScaleXLerping = false;
    float scaleLerpStart;
    public Vector3 initialScale;

    void Start ()
    {
        if (doAlpha)
        {
            r = gameObject.GetComponent<Renderer>();
            m = r.material;
            initialColor = m.GetColor("_TintColor");
        }
    }

    void OnEnable()
    {
        if (doAlpha)
        {
            //set start alpha
            startAlpha = initialColor.a;

            //set the target alpha
            targetAlpha = maxAlpha;

            //start delay
            StartCoroutine(delayStart(startDelay));
        }

        if (doScaleX)
        {
            //set start scale
            transform.localScale = new Vector3(startScaleX, initialScale.y, initialScale.z);

            //max scale
            endScaleX = initialScale.x;

            //start delay
            StartCoroutine(delayStart(startDelay));
        }
    }

    IEnumerator delayStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (doAlpha)
        {
            //start the lerp to increase alpha
            doAlphaTransition(targetAlpha, transitionTime);

            //start a co-routine to start the transition back again
            StartCoroutine(doAlphaWait(transitionTime));
        }

        if (doScaleX)
        {
            doScaleTransition();
        }
    }

    IEnumerator doAlphaWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //set the target alpha
        targetAlpha = 0.0f;

        //set the target alpha
        startAlpha = maxAlpha;

        //start the transition
        doAlphaTransition(0.0f, transitionTime);
    }

    void doScaleTransition()
    {
        isScaleXLerping = true;

        scaleLerpStart = Time.time;
    }

    void doAlphaTransition(float toAlpha, float toTime)
    {
        //start the lerp
        isAlphaLerping = true;

        //set the start time
        alphaLerpStart = Time.time;

        //set the target alpha
        targetAlpha = toAlpha;
    }

    void FixedUpdate()
    {
        if (doAlpha)
        {
            if (isAlphaLerping)
            {

                float timeSinceStarted = Time.time - alphaLerpStart;
                float percentageComplete = timeSinceStarted / transitionTime;

                //lerp alpha from start alpha to target alpha
                m.SetColor("_TintColor", new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.Lerp(startAlpha, targetAlpha, percentageComplete)));

                if (percentageComplete >= 1.0f)
                {
                    isAlphaLerping = false;
                }
            }
        }
        if (doScaleX)
        {
            if (isScaleXLerping)
            {
                float timeSinceStarted = Time.time - scaleLerpStart;
                float percentageComplete = timeSinceStarted / transitionTime;

                //make the curve an ease in type
                percentageComplete = graphicsEasing.EaseOut(percentageComplete, EasingType.Cubic);

                //lerp
                gameObject.transform.localScale = new Vector3(Mathf.Lerp(startScaleX, endScaleX, percentageComplete), gameObject.transform.localScale.y, gameObject.transform.localScale.z);

                if (percentageComplete >= 1.0f)
                {
                    isScaleXLerping = false;
                }
            }
        }
    }
}
