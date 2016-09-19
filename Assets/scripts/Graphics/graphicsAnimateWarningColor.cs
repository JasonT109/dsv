using UnityEngine;
using System.Collections;
using Meg.Networking;

public class graphicsAnimateWarningColor : MonoBehaviour {
    [Header ("State:")]
    public bool warning;

    [Header ("Configuration:")]
    public Color warningColor;
    public float warningFlashSpeed = 1.0f;
    public float warningFlashPause = 1.0f;
    public bool useUniversalSync = true;
    public bool usingTintColor = false;
    public bool isDynamicText = false;
    public float phase = 0f;

    [Header("Optional shader colors to adjust:")]
    public string[] shaderColorNames;

    [Header ("Server Configuration:")]
    public bool autoWarning = false;
    public float autoWarningValue = 0f;
    public string autoWarningServerName = "depth";
    public bool autoWarningGreaterThan = false;
    public int[] materialIndex;

    private Color[] shaderColorOrigValues;
    private Color[] shaderColorLerpValues;
    private GameObject syncNode;
    private float timeIndex;
    private Renderer r;
    private Material m;
    private Color originalColor;
    private DynamicText dt;
    private bool prevState;
    private bool stateChanged = true;

    void SetColor()
    {
        if (warning != prevState)
            stateChanged = true;

        Color c = Color.black;

        if (autoWarning)
        {
            if (autoWarningGreaterThan)
            {
                if (serverUtils.GetServerData(autoWarningServerName) > autoWarningValue)
                    warning = true;
                else
                    warning = false;
            }
            else
            {
                if (serverUtils.GetServerData(autoWarningServerName) <= autoWarningValue)
                    warning = true;
                else
                    warning = false;
            }
        }

        if (warning)
        {
            prevState = warning;
            stateChanged = true;
            if (!useUniversalSync || !syncNode)
            {
                timeIndex += Time.deltaTime * warningFlashSpeed;
                float sinWave = Mathf.Sin(timeIndex);

                if (timeIndex > 1.0f)
                    timeIndex = warningFlashPause;

                if (shaderColorNames.Length > 0)
                    for (int i = 0; i < shaderColorNames.Length; i++)
                        shaderColorLerpValues[i] = Color.Lerp(warningColor, shaderColorOrigValues[i], Mathf.Sin(sinWave));
                else
                    c = Color.Lerp(warningColor, originalColor, Mathf.Sin(sinWave));


            }
            else
            {
                float sinWave = Mathf.Sin(syncNode.GetComponent<widgetWarningFlashSync>().timeIndex + phase);

                if (shaderColorNames.Length > 0)
                    for (int i = 0; i < shaderColorNames.Length; i++)
                        shaderColorLerpValues[i] = Color.Lerp(warningColor, shaderColorOrigValues[i], Mathf.Sin(sinWave));
                else
                    c = Color.Lerp(warningColor, originalColor, Mathf.Sin(sinWave));

            }
        }
        else
        {
            prevState = warning;
            c = originalColor;
            if (shaderColorNames.Length > 0)
                for (int i = 0; i < shaderColorNames.Length; i++)
                    shaderColorLerpValues[i] = shaderColorOrigValues[i];
        }


        if (stateChanged)
        {
            stateChanged = false;

            if (isDynamicText)
                dt.color = c;
            else if (usingTintColor)
                m.SetColor("_TintColor", c);
            else if (shaderColorNames.Length > 0)
                for (int i = 0; i < shaderColorNames.Length; i++)
                    m.SetColor(shaderColorNames[i], shaderColorLerpValues[i]);
            else
                m.SetColor("_Color", c);
        }

    }

    void ResetColor(string colorName, Color value)
    {
        m.SetColor(colorName, value);
    }

    void OnEnable()
    {
        if (useUniversalSync)
            syncNode = GameObject.FindWithTag("WarningSync");
    }

    void Start ()
    {
        if (isDynamicText)
        {
            dt = GetComponent<DynamicText>();
            originalColor = dt.color;
        }
        else
        {
            r = gameObject.GetComponent<Renderer>();
            m = r.material;

            if (shaderColorNames.Length > 0)
            {
                shaderColorOrigValues = new Color[shaderColorNames.Length];
                shaderColorLerpValues = new Color[shaderColorNames.Length];
                for (int i = 0; i < shaderColorNames.Length; i++)
                    shaderColorOrigValues[i] = m.GetColor(shaderColorNames[i]);
            }
            else if (usingTintColor)
                originalColor = m.GetColor("_TintColor");
            else
                originalColor = m.GetColor("_Color");
        }
        timeIndex += phase;
        if (useUniversalSync)
            syncNode = GameObject.FindWithTag("WarningSync");
    }


	void Update ()
    {
        if (materialIndex.Length > 0 && !isDynamicText)
        {
            for (int i = 0; i < materialIndex.Length; i++)
            {
                m = r.materials[materialIndex[i]];
                SetColor();
            }
        }
        else
        {
            SetColor();
        }
    }
}
