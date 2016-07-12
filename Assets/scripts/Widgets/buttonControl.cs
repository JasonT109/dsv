using System;
using System.Collections;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Hit;
using Meg.Networking;
using Meg.Graphics;

public class buttonControl : MonoBehaviour
{
    public Color[] colorTheme;
    public bool gliderButton = false;
    public GameObject gliderButtonOnMesh;
    public bool pressed = false;
    public bool active = false;
    public bool disabled = false;
    public bool warning = false;
    public bool toggleType = false;
    public bool canToggleOff = false;
    public GameObject buttonGroup;
    public GameObject visGroup;
    public float warningFlashSpeed = 1.0f;
    public float warningFlashPause = 1.0f;
    public bool autoWarning = false;
    public float autoWarningValue = 0f;
    public string autoWarningServerName = "depth";
    public bool autoWarningGreaterThan = false;
    public string serverName = "depth";
    public string serverType = "int";

    private megColorTheme colorScheme;
    private Renderer r;
    private Material m;
    private float timeIndex = 0.0f;
    private GameObject colourThemeObj;
    private float pressDelay = 0.1f;
    private bool canPress = true;

    void Start()
    {
        r = GetComponent<Renderer>();
        m = r.material;
        m.color = colorTheme[3];
        toggleVisGroup();

        if (active)
        {
            active = false;
            if (buttonGroup)
            {
                var bGroupScript = buttonGroup.GetComponent<buttonGroup>();
                bGroupScript.toggleButtons(gameObject);
            }
            else
            {
                toggleButton(gameObject);
            }
        }
    }

    private void OnEnable()
    {
        GetComponent<TapGesture>().Tapped += tappedHandler;
        GetComponent<PressGesture>().Pressed += pressedHandler;
        GetComponent<ReleaseGesture>().Released += releaseHandler;
    }

    private void OnDisable()
    {
        GetComponent<TapGesture>().Tapped -= tappedHandler;
        GetComponent<PressGesture>().Pressed -= pressedHandler;
        GetComponent<ReleaseGesture>().Released -= releaseHandler;
    }

    private void tappedHandler(object sender, EventArgs e)
    {
        var gesture = sender as TapGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        if (!disabled && !toggleType)
        {
            //Debug.Log("tapped this object: " + gameObject);
            pressed = true;
            m.color = colorTheme[4];
            StartCoroutine(waitPress(0.1f));
        }
    }

    private void pressedHandler(object sender, EventArgs e)
    {
        var gesture = sender as PressGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        //Debug.Log("Pressed this object: " + gameObject);
        if (!disabled && canPress)
        {
            canPress = false;
            StartCoroutine(waitRelease(pressDelay));
            pressed = true;
            m.color = colorTheme[4];
        }
    }

    private void releaseHandler(object sender, EventArgs e)
    {
        var gesture = sender as ReleaseGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        //Debug.Log("Released on this object: " + gameObject);
        if (!disabled)
        {
            if (buttonGroup)
            {
                if (!active || canToggleOff)
                {
                    var bGroupScript = buttonGroup.GetComponent<buttonGroup>();
                    bGroupScript.toggleButtons(gameObject);
                }
                else
                {
                    pressed = false;
                    m.color = colorTheme[1];
                }
            }
            else
            {
                toggleButton(gameObject);
            }
        }
    }

    public void toggleButton(GameObject b)
    {
        if (b == gameObject) //quick check to see if this is the button being pressed, this function can be called by a button group
        {
            if (toggleType && !active)
            {
                pressed = false;
                active = true;
                m.color = colorTheme[1];
            }
            else
            {
                pressed = false;
                active = false;
                m.color = colorTheme[3];
            }
        }
        else
        {
            pressed = false;
            active = false;
            if (m)
            {
                m.color = colorTheme[3];
            }
        }
        toggleVisGroup();
    }

    public void toggleVisGroup()
    {
        if (visGroup && !active)
        {
            visGroup.SetActive(false);
        }
        else if (visGroup && active)
        {
            visGroup.SetActive(true);
        }
    }

    public void updateColor()
    {
        colorScheme = serverUtils.GetColorTheme();
        colorTheme[1] = colorScheme.keyColor;

        if (active && !warning && !pressed)
        {
            m.color = colorTheme[1];
        }
    }

    void Update()
    {
        if (gliderButton)
        {
            if (pressed || active)
            {
                gliderButtonOnMesh.SetActive(true);
            }
            else
            {
                gliderButtonOnMesh.SetActive(false);
            }
        }

        updateColor();

        if (autoWarning)
        {
            if (autoWarningGreaterThan)
            {
                //check if server value is higher than warning value
                if (serverUtils.GetServerData(autoWarningServerName) > autoWarningValue)
                {
                    warning = true;
                }
                else
                {
                    warning = false;
                }

            }
            else
            {
                //check if server value is lower than warning value
                if (serverUtils.GetServerData(autoWarningServerName) <= autoWarningValue)
                {
                    warning = true;
                }
                else
                {
                    warning = false;
                }
            }
        }
        if (warning)
        {
            timeIndex += Time.deltaTime * warningFlashSpeed;
            float sinWave = Mathf.Sin(timeIndex);
            if (timeIndex > 1.0f)
            {
                timeIndex = warningFlashPause;
            }
            if (active)
            {
                m.color = Color.Lerp(colorTheme[0], colorTheme[1], Mathf.Sin(sinWave));
            }
            else
            {
                m.color = Color.Lerp(colorTheme[0], colorTheme[3], Mathf.Sin(sinWave));
            }
        }
        else
        {
            if (!pressed && !active)
            {
                m.color = colorTheme[3];
            }
        }
    }

    IEnumerator waitToDestroy(float waitTime, GameObject g)
    {
        yield return new WaitForSeconds(waitTime);
        //destroy thing
        Destroy(g);
    }

    IEnumerator waitRelease(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canPress = true;
    }

    IEnumerator waitPress(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        pressed = false;
        m.color = colorTheme[3];
    }
}
