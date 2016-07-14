using UnityEngine;
using System.Collections;
using Meg.Maths;

public class graphicsAnimateTransform : MonoBehaviour
{
    public bool[] animateAxis = new bool[9] { false, false, false, false, false, false, false, false, false };
    public float[] animateTime = new float[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    public float[] onDelayTime = new float[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public float[] offDelayTime = new float[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public float[] toValue = new float[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    public float[] fromValue = new float[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    public GameObject triggerButton;

    private bool canPress = true;
    private bool on = false;
    private float[] runningTime = new float[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private bool[] lerping = new bool[9] { false, false, false, false, false, false, false, false, false };
    private float[] from = new float[9];
    private float[] to = new float[9];

    public void triggerOn ()
    {
        //Debug.Log("Triggering on animated lerp...");

        from = fromValue;
        to = toValue;

        on = true;
	    for (int i = 0; i < animateAxis.Length; i++)
        {
            if (animateAxis[i])
            {
                //lerp this value from a to b over time, after delay
                StartCoroutine(lerpDelay(onDelayTime[i], i));
            }
        }
	}

    public void triggerOff()
    {
        //Debug.Log("Triggering off animated lerp...");

        from = toValue;
        to = fromValue;

        on = false;
        for (int i = 0; i < animateAxis.Length; i++)
        {
            if (animateAxis[i])
            {
                //lerp this value from a to b over time, after delay
                StartCoroutine(lerpDelay(offDelayTime[i], i));
            }
        }
    }

    IEnumerator lerpDelay(float delay, int index)
    {
        yield return new WaitForSeconds(delay);
        runningTime[index] = 0;
        lerping[index] = true;
    }

    IEnumerator wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canPress = true;

    }

    void Update()
    {
        if (triggerButton.GetComponent<buttonControl>().pressed && canPress)
        {
            canPress = false;
            StartCoroutine(wait(0.4f));

            if (!on)
            {
                triggerOn();
            }
            else
            {
                triggerOff();
            }
        }

        for (int i = 0; i < lerping.Length; i++)
        {
            if (lerping[i])
            {
                runningTime[i] += Time.deltaTime;
                float pComplete = runningTime[i] / animateTime[i];

                pComplete = graphicsEasing.EaseOut(pComplete, EasingType.Cubic);

                switch (i)
                {
                    case 0:
                        transform.localPosition = new Vector3(Mathf.Lerp(from[i], to[i], pComplete), transform.localPosition.y, transform.localPosition.z);
                        break;
                    case 1:
                        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(from[i], to[i], pComplete), transform.localPosition.z);
                        break;
                    case 2:
                        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Lerp(from[i], to[i], pComplete));
                        break;
                    case 6:
                        transform.localScale = new Vector3(Mathf.Lerp(from[i], to[i], pComplete), transform.localScale.y, transform.localScale.z);
                        break;
                    case 7:
                        transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(from[i], to[i], pComplete), transform.localScale.z);
                        break;
                    case 8:
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Lerp(from[i], to[i], pComplete));
                        break;
                }

                if (runningTime[i] > animateTime[i])
                {
                    lerping[i] = false;
                }
            }
        }
    }
}
