using UnityEngine;
using System.Collections;
using Meg.Networking;

public class widgetWarningSwapTexture : MonoBehaviour
{

    public string[] serverVars;
    public Texture swapTexture;
    public float warningLevel;
    public bool greaterThanLevel;

    private float[] serverVarValues;
    private Renderer r;
    private Material m;
    private Texture originalTexture;

    void SetTexture()
    {
        serverVarValues = new float[serverVars.Length];
        for (int i = 0; i < serverVars.Length; i++)
        {
            serverVarValues[i] = serverUtils.GetServerData(serverVars[i]);
        }

        if (greaterThanLevel)
        {
            float currentWarningLevel = Mathf.Max(serverVarValues);

            if (currentWarningLevel > warningLevel)
                m.mainTexture = swapTexture;
            else
                m.mainTexture = originalTexture;
        }
        else
        {
            float currentWarningLevel = Mathf.Min(serverVarValues);

            if (currentWarningLevel <= warningLevel)
                m.mainTexture = swapTexture;
            else
                m.mainTexture = originalTexture;
        }
    }

    void OnEnable()
    {
        if (!r)
            r = gameObject.GetComponent<Renderer>();
        if (!m)
            m = r.material;
        if (!originalTexture)
            originalTexture = m.mainTexture;
        SetTexture();
    }

    // Use this for initialization
    void Start ()
    {
        r = gameObject.GetComponent<Renderer>();
        m = r.material;
        originalTexture = m.mainTexture;
	}
	
	// Update is called once per frame
	void Update ()
    {
        SetTexture();
    }
}
