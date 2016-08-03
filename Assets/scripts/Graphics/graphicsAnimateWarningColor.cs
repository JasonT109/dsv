using UnityEngine;
using System.Collections;
using Meg.Networking;

public class graphicsAnimateWarningColor : MonoBehaviour {

    public Color warningColor;
    public bool warning;
    public float warningFlashSpeed = 1.0f;
    public float warningFlashPause = 1.0f;
    public bool autoWarning = false;
    public float autoWarningValue = 0f;
    public string autoWarningServerName = "depth";
    public bool autoWarningGreaterThan = false;
    public bool useUniversalSync = true;
    public bool usingTintColor = false;

    private GameObject syncNode;
    private float timeIndex;
    private Renderer r;
    private Material m;
    private Color originalColor;

    void SetColor()
    {
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
            if (!useUniversalSync || !syncNode)
            {
                timeIndex += Time.deltaTime * warningFlashSpeed;
                float sinWave = Mathf.Sin(timeIndex);
                if (timeIndex > 1.0f)
                {
                    timeIndex = warningFlashPause;
                }

                if (usingTintColor)
                    m.SetColor("_TintColor", Color.Lerp(warningColor, originalColor, Mathf.Sin(sinWave)));
                else
                    m.SetColor("_Color", Color.Lerp(warningColor, originalColor, Mathf.Sin(sinWave)));
            }
            else
            {
                float sinWave = Mathf.Sin(syncNode.GetComponent<widgetWarningFlashSync>().timeIndex);
                if (usingTintColor)
                    m.SetColor("_TintColor", Color.Lerp(warningColor, originalColor, Mathf.Sin(sinWave)));
                else
                    m.SetColor("_Color", Color.Lerp(warningColor, originalColor, Mathf.Sin(sinWave)));
            }

        }
        else
        {
            if (usingTintColor)
                m.SetColor("_TintColor", originalColor);
            else
                m.SetColor("_Color", originalColor);
        }
    }


    void OnEnable()
    {
        r = gameObject.GetComponent<Renderer>();
        m = r.material;

        if (usingTintColor)
            originalColor = m.GetColor("_TintColor");
        else
            originalColor = m.GetColor("_Color");

        if (useUniversalSync)
        {
            syncNode = GameObject.FindWithTag("WarningSync");
        }

        SetColor();
    }

    // Use this for initialization
    void Start ()
    {
        r = gameObject.GetComponent<Renderer>();
        m = r.material;
        
        if (usingTintColor)
            originalColor = m.GetColor("_TintColor");
        else
            originalColor = m.GetColor("_Color");

        if (useUniversalSync)
        {
            syncNode = GameObject.FindWithTag("WarningSync");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        SetColor();
    }
}
