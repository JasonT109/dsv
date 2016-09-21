using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Hit;
using Meg.Networking;
using Meg.Graphics;
using UnityEngine.Events;

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
    public bool useSharedTheme = true;
    public float Brightness = 1;
    public DynamicText optionalLabel;
    public string labelInactiveText;
    public string labelActiveText;
    public Color labelInactiveColor = Color.grey;
    public Color labelActiveColor = Color.white;

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
    public bool autoWarningInheritFromVisGroup;
    public autoWarningCondition[] autoWarningConditions;
    public bool useUniversalSync = false;

    [Serializable]
    public struct autoWarningCondition
    {
        public float value;
        public string serverName;
        public bool greaterThan;
    }

    [Header("Server")]
    public bool observeServerState;
    public string serverName = "depth";
    public string serverType = "int";
    public string serverEnableKey = "";

    [Header("Animation")]
    public bool AnimateOnPress;
    public float colorTweenDuration = 0;
    public widgetHighLightOnActive transition;

    [Header("Glider")]
    public bool gliderButton = false;
    public GameObject gliderButtonOnMesh;

    [Header("DCC")]
    public bool DCCButton = false;
    public bool DCCQuadButton = false;
    public GameObject DCCQuadMenu;
    public float pressTime = 0.2f;
    public float pressedScale = 0.95f;

    [Header("Events")]
    public UnityEvent onPress;
    public UnityEvent onRelease;
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
    private int frame = 0;
    public float doublePressTime = 0f;
    private bool doublePressCheck = false;

    private buttonControl[] _autoWarningsInVisGroup;

    private buttonGroup _group;
    public buttonGroup Group
    {
        get
        {
            if (!_group)
                _group = buttonGroup.GetComponent<buttonGroup>();

            return _group;
        }
    }

    public Color Color
    {
        get { return m ? m.color : Color.white; }
        set { SetColor(value); }
    }

    void Start()
    {
        r = GetComponent<Renderer>();
        m = r.material;
        SetColor(GetThemeColor(3), false);
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
            Color = GetThemeColor(4);

            //broadcast that this button has changed state
            changed = true;
            frame = 0;
            StartCoroutine(waitOneFrame());

            if (AnimateOnPress)
                transform.DOPunchScale(transform.localScale * 0.05f, 0.1f);

            if (onPressed != null)
                onPressed();

            if (onPress != null)
                onPress.Invoke();
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
                    Color = GetThemeColor(1);
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

            if (onRelease != null)
                onRelease.Invoke();
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
            Color = GetThemeColor(4);

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
            Color = GetThemeColor(4);

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

            if (onPress != null)
                onPress.Invoke();
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
                if ((!requiresDoublePress) && (!active || canToggleOff))
                {
                    var bGroupScript = buttonGroup.GetComponent<buttonGroup>();
                    bGroupScript.toggleButtons(gameObject);
                }
                else
                {
                    pressed = false;
                    Color = GetThemeColor(1);
                }
            }
            else
            {
                if (!requiresDoublePress)
                    toggleButton(gameObject);
            }

            //broadcast that this button has changed state
            changed = true;
            frame = 0;
            waitOneFrame();

            if (onReleased != null)
                onReleased();

            if (onRelease != null)
                onRelease.Invoke();
        }
    }

    public void setButtonActive(bool value)
        { toggleButton(value ? gameObject : null, true); }

    public void toggleButton(GameObject b, bool forceOn = false)
    {
        if (b == gameObject) //quick check to see if this is the button being pressed, this function can be called by a button group
        {
            if (toggleType && (!active || forceOn))
            {
                pressed = false;
                active = true;

                if (m)
                    Color = GetThemeColor(1);
            }
            else
            {
                pressed = false;
                active = false;

                if (m)
                    Color = GetThemeColor(3);
            }
        }
        else
        {
            pressed = false;
            active = false;

            if (m)
                Color = GetThemeColor(3);
        }
        toggleVisGroup();
    }

    public void toggleVisGroup()
    {
        if (visGroup && !active && visGroup.activeSelf)
            visGroup.SetActive(false);
        else if (visGroup && active && !visGroup.activeSelf)
            visGroup.SetActive(true);
    }

    public void updateColor()
    {
        if (useSharedTheme)
        {
            colorScheme = serverUtils.GetColorTheme();
            colorTheme[1] = colorScheme.keyColor;
        }

        if (active && !warning && !pressed)
            Color = GetThemeColor(1);
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
        if (!string.IsNullOrEmpty(serverEnableKey))
            disabled = !serverUtils.GetServerBool(serverEnableKey, true);

        if (optionalLabel)
        {
            if (!active)
            {
                optionalLabel.SetText(labelInactiveText);
                optionalLabel.color = labelInactiveColor;
            }
            else
            {
                optionalLabel.SetText(labelActiveText);
                optionalLabel.color = labelActiveColor;
            }
        }

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
            warning = IsAutoWarningActive();

        if (warning)
        {
            float sinWave;
            if (!useUniversalSync || !syncNode)
            {
                timeIndex += Time.deltaTime * warningFlashSpeed;
                sinWave = Mathf.Sin(timeIndex);
                if (timeIndex > 1.0f)
                    timeIndex = warningFlashPause;

                SetColor(Color.Lerp(GetThemeColor(0), GetThemeColor(active ? 1 : 3), Mathf.Sin(sinWave)), false);
            }
            else
            {
                sinWave = Mathf.Sin(syncNode.GetComponent<widgetWarningFlashSync>().timeIndex);
                SetColor(Color.Lerp(GetThemeColor(0), GetThemeColor(active ? 1 : 3), Mathf.Sin(sinWave)), false);
            }
        }
        else
        {
            if (!pressed && !active)
                SetColor(GetThemeColor(3), false);
        }
    }

    private bool IsAutoWarningActive()
    {
        // Evaluate primary autowarning condition.
        bool on = false;

        if (!string.IsNullOrEmpty(autoWarningServerName))
            on = IsWarningConditionMet(autoWarningServerName, autoWarningGreaterThan, autoWarningValue);

        // Evaluate additional autowarning conditions.
        for (var i = 0; i < autoWarningConditions.Length; i++)
            on = on || IsWarningConditionMet(autoWarningConditions[i]);

        // Optionally check if any warnings are active in vis group.
        if (autoWarningInheritFromVisGroup)
            on = on || HasWarningInVisgroup();

        return on;
    }

    private bool HasWarningInVisgroup()
    {
        if (!visGroup)
            return false;

        if (_autoWarningsInVisGroup == null)
            _autoWarningsInVisGroup = visGroup.transform
                .GetComponentsInChildren<buttonControl>(true)
                .Where(b => b.autoWarning)
                .ToArray();

        return _autoWarningsInVisGroup
            .Any(b => b.IsAutoWarningActive());
    }

    private bool IsWarningConditionMet(autoWarningCondition c)
        { return IsWarningConditionMet(c.serverName, c.greaterThan, c.value); }

    private bool IsWarningConditionMet(string key, bool greaterThan, float value)
    {
        var current = serverUtils.GetServerData(key);
        if (greaterThan)
            return current > value;

        return current <= value;
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
        //if (doublePressed)
            //doublePressed = false;
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
        Color = GetThemeColor(3);
    }

    public Tweener SetColor(Color c, bool tweened = true, float duration = 0)
    {
        if (!m)
            return null;

        if (duration <= 0)
            duration = colorTweenDuration;
       
        m.DOKill();

        if (tweened)
        {
            if (m.HasProperty("_Color"))
                return m.DOColor(c, "_Color", duration);
            if (m.HasProperty("_TintColor"))
                return m.DOColor(c, "_TintColor", duration);
        }
        else
        {
            if (m.HasProperty("_Color"))
                m.SetColor("_Color", c);
            if (m.HasProperty("_TintColor"))
                m.SetColor("_TintColor", c);
        }

        return null;
    }
}
