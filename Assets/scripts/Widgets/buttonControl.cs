using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Hit;
using Meg.Networking;
using Meg.Graphics;

public class buttonControl : MonoBehaviour
{
    [Header("State")]
    public bool pressed = false;
    public bool doublePressed = false;
    public bool active = false;
    public bool disabled = false;
    public bool warning = false;
    public bool toggleType = false;
    public bool canToggleOff = false;
    public bool changed = false;
    public float minDoublePressTime = 0.4f;
    public bool requiresDoublePress = false;

    [Header("Appearance")]
    public Color[] colorTheme;
    public float Brightness = 1;

    [Header("Groups")]
    public GameObject buttonGroup;
    public GameObject visGroup;

    [Header("Warnings")]
    public float warningFlashSpeed = 1.0f;
    public float warningFlashPause = 1.0f;
    public bool autoWarning = false;
    public float autoWarningValue = 0f;
    public string autoWarningServerName = "depth";
    public bool autoWarningGreaterThan = false;
    public bool useUniversalSync = false;

    [Header("Server")]
    public bool observeServerState;
    public string serverName = "depth";
    public string serverType = "int";

    [Header("Animation")]
    public bool AnimateOnPress;

    [Header("Glider")]
    public bool gliderButton = false;
    public GameObject gliderButtonOnMesh;

    [Header("DCC")]
    public bool DCCButton = false;
    public bool DCCQuadButton = false;
    public GameObject DCCQuadMenu;
    public float pressTime = 0.2f;
    public float pressedScale = 0.95f;

    public delegate void buttonEventHandler();
    public event buttonEventHandler onPressed;
    public event buttonEventHandler onReleased;

    private GameObject syncNode;
    private megColorTheme colorScheme;
    private Renderer r;
    private Material m;
    private float timeIndex = 0.0f;
    private float pressTimer;
    private GameObject colourThemeObj;
    private float pressDelay = 0.05f;
    private bool canPress = true;
    public widgetHighLightOnActive transition;
    private int frame = 0;
    private float doublePressTime = 0f;
    private bool doublePressCheck = false;

    void Start()
    {
        r = GetComponent<Renderer>();
        m = r.material;
        m.color = GetThemeColor(3);
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

        if (gliderButton || DCCButton)
        {
            transition = gliderButtonOnMesh.GetComponent<widgetHighLightOnActive>();
        }

        if (useUniversalSync)
        {
            syncNode = GameObject.FindWithTag("WarningSync");
        }
    }

    public void RemotePress()
    {
        Debug.Log("Remote pressed this object: " + gameObject);
        if (!disabled && canPress)
        {
            //prevent accidental double pressing of button
            canPress = false;
            StartCoroutine(waitRelease(pressDelay));

            //set pressed state and change color
            pressed = true;
            m.color = GetThemeColor(4);

            //broadcast that this button has changed state
            changed = true;
            frame = 0;
            StartCoroutine(waitOneFrame());

            if (AnimateOnPress)
                transform.DOPunchScale(transform.localScale * 0.05f, 0.1f);

            if (onPressed != null)
                onPressed();
        }
    }

    public void RemoteToggle()
    {
        Debug.Log("Toggled this object: " + gameObject);
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
                    m.color = GetThemeColor(1);
                }
            }
            else
            {
                toggleButton(gameObject);
            }

            //broadcast that this button has changed state
            changed = true;
            frame = 0;
            waitOneFrame();

            if (onReleased != null)
                onReleased();
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

    /**tapped removed as functionality appears superfluous */
    private void tappedHandler(object sender, EventArgs e)
    {
        var gesture = sender as TapGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        /*
        if (!disabled && !toggleType && canPress)
        {
            //Debug.Log("tapped this object: " + gameObject);
            //prevent accidental double pressing of button
            canPress = false;
            StartCoroutine(waitRelease(pressDelay));

            //set pressed state and change color
            pressed = true;
            m.color = GetThemeColor(4);

            //broadcast that this button has changed state
            changed = true;
            frame = 0;
            waitOneFrame();

            if (onPressed != null)
                onPressed();
        }
        */
    }

    private void pressedHandler(object sender, EventArgs e)
    {
        var gesture = sender as PressGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        if (doublePressTime != 0)
        {
            doublePressed = true;
            StartCoroutine(disableDoublePress(0.05f));
        }

        if (!disabled && canPress)
        {
            if (!doublePressed)
            {
                doublePressCheck = true;
                StartCoroutine(waitDoublePress(minDoublePressTime));
            }

            //prevent accidental double pressing of button
            canPress = false;
            StartCoroutine(waitRelease(pressDelay));

            //set pressed state and change color
            pressed = true;
            m.color = GetThemeColor(4);

            //broadcast that this button has changed state
            changed = true;
            frame = 0;
            StartCoroutine(waitOneFrame());

            if (DCCQuadButton)
                transform.DOScale(transform.localScale * pressedScale, 0.1f);

            if (AnimateOnPress)
                transform.DOPunchScale(transform.localScale * 0.05f, 0.1f);

            if (onPressed != null)
                onPressed();
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
                if (!requiresDoublePress && (!active || canToggleOff))
                {
                    var bGroupScript = buttonGroup.GetComponent<buttonGroup>();
                    bGroupScript.toggleButtons(gameObject);
                }
                else
                {
                    pressed = false;
                    m.color = GetThemeColor(1);
                }
                if (requiresDoublePress && doublePressed)
                {
                    toggleButton(gameObject);
                }
            }
            else
            {
                toggleButton(gameObject);
            }

            //broadcast that this button has changed state
            changed = true;
            frame = 0;
            waitOneFrame();

            if (onReleased != null)
                onReleased();
        }
    }

    public void toggleButton(GameObject b, bool forceOn = false)
    {
        if (b == gameObject) //quick check to see if this is the button being pressed, this function can be called by a button group
        {
            if (toggleType && (!active || forceOn))
            {
                pressed = false;
                active = true;

                if (m)
                    m.color = GetThemeColor(1);
            }
            else
            {
                pressed = false;
                active = false;

                if (m)
                    m.color = GetThemeColor(3);
            }
        }
        else
        {
            pressed = false;
            active = false;

            if (m)
                m.color = GetThemeColor(3);
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
            m.color = GetThemeColor(1);
        }
    }

    public Color GetThemeColor(int i)
    {
        if (Mathf.Approximately(Brightness, 1))
            return colorTheme[i];
        else
        {
            var hsb = HSBColor.FromColor(colorTheme[i]);
            hsb.b *= Brightness;
            return hsb.ToColor();
        }
    }

    void Update()
    {
        if (doublePressCheck)
            doublePressTime += Time.deltaTime;
        else
            doublePressTime = 0;

        if (pressed)
            pressTimer += Time.deltaTime;
        else
            pressTimer = 0;

        if (changed)
            frame += 1;

        if (gliderButton || DCCButton)
        {
            if (pressed || active)
            {
                gliderButtonOnMesh.SetActive(true);
            }
            else
            {
                transition = gliderButtonOnMesh.GetComponent<widgetHighLightOnActive>();
                if (transition)
                {
                    if (!transition.isScaleXLerping)
                    {
                        gliderButtonOnMesh.SetActive(false);
                    }
                }
                else
                {
                    gliderButtonOnMesh.SetActive(false);
                }
            }
        }

        if (DCCQuadButton)
        {
            if (pressTimer > pressTime && pressed)
            {
                
                DCCQuadMenu.SetActive(true);
            }
            else
            {
                DCCQuadMenu.SetActive(false);
                transform.localScale = Vector3.one;
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
            float sinWave;
            if (!useUniversalSync || !syncNode)
            {
                timeIndex += Time.deltaTime * warningFlashSpeed;
                sinWave = Mathf.Sin(timeIndex);
                if (timeIndex > 1.0f)
                {
                    timeIndex = warningFlashPause;
                }

                if (active)
                {
                    m.SetColor("_TintColor", Color.Lerp(GetThemeColor(0), GetThemeColor(1), Mathf.Sin(sinWave)));
                    //m.SetColor("_MainColor", Color.Lerp(GetThemeColor(0), GetThemeColor(1), Mathf.Sin(sinWave)));
                    m.color = Color.Lerp(GetThemeColor(0), GetThemeColor(1), Mathf.Sin(sinWave));
                }
                else
                {
                    m.SetColor("_TintColor", Color.Lerp(GetThemeColor(0), GetThemeColor(3), Mathf.Sin(sinWave)));
                    //m.SetColor("_MainColor", Color.Lerp(GetThemeColor(0), GetThemeColor(3), Mathf.Sin(sinWave)));
                    m.color = Color.Lerp(GetThemeColor(0), GetThemeColor(3), Mathf.Sin(sinWave));
                }
            }
            else
            {
                sinWave = Mathf.Sin(syncNode.GetComponent<widgetWarningFlashSync>().timeIndex);

                if (active)
                {
                    m.SetColor("_TintColor", Color.Lerp(GetThemeColor(0), GetThemeColor(1), Mathf.Sin(sinWave)));
                    //m.SetColor("_MainColor", Color.Lerp(GetThemeColor(0), GetThemeColor(1), Mathf.Sin(sinWave)));
                    m.color = Color.Lerp(GetThemeColor(0), GetThemeColor(1), Mathf.Sin(sinWave));
                }
                else
                {
                    m.SetColor("_TintColor", Color.Lerp(GetThemeColor(0), GetThemeColor(3), Mathf.Sin(sinWave)));
                    //m.SetColor("_MainColor", Color.Lerp(GetThemeColor(0), GetThemeColor(3), Mathf.Sin(sinWave)));
                    m.color = Color.Lerp(GetThemeColor(0), GetThemeColor(3), Mathf.Sin(sinWave));
                }
            }
        }
        else
        {
            if (!pressed && !active)
            {
                m.SetColor("_TintColor", GetThemeColor(3));
                m.color = GetThemeColor(3);
            }
        }
    }

    IEnumerator waitDoublePress(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        doublePressCheck = false;
        doublePressTime = 0;
    }

    IEnumerator disableDoublePress(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (doublePressed)
            doublePressed = false;
    }

    IEnumerator waitOneFrame()
    {
        yield return new WaitWhile(() => frame < 1);
        changed = false;
        if (doublePressed)
            doublePressed = false;
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
        changed = false;
    }

    IEnumerator waitPress(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        pressed = false;
        m.color = GetThemeColor(3);
    }
}
