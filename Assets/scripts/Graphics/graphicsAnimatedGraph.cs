using UnityEngine;
using System.Collections;
using System.Linq;
using Meg.Networking;
using Meg.Maths;

public class graphicsAnimatedGraph : MonoBehaviour
{

    [Header ("Appearance")]
    public GameObject[] graphBars;
    public float updateSpeed = 1.0f;
    public float speed = 1.0f;
    public float maxHeight = 10.0f;
    public float minHeight = 5.0f;
    public float noise = 0.2f;
    public bool useSineWave;
    public bool fadeOut;

    [Header ("Server")]
    public bool switchOnWarning = false;
    public string warningParam;
    public float warningValue;
    public float warningMaxHeight = 10.0f;
    public float warningMinHeight = 5.0f;

    [HideInInspector]
    public float[] graphHeights;

    private float minH;
    private float maxH;
    private int n;
    private float tickTime;
    private float timeIndex = 0.0f;
    private bool switched = false;

    void Start ()
    {
        n = graphBars.Length;
        graphHeights = new float[n];
        minH = minHeight;
        maxH = maxHeight;

        for (int i = 0; i < n; i++)
        {
            if (useSineWave)
            {
                graphBars[i].GetComponent<graphicsSlicedMesh>().Height = graphHeights[i] = minH;
            }
            else
            {
                graphBars[i].GetComponent<graphicsSlicedMesh>().Height = graphHeights[i] = Random.Range(minH, maxH);
            }

            if (fadeOut)
            {
                float _ColorLerpValue = graphicsMaths.remapValue(i, 0, n, 0, 1);
                Renderer _Renderer = graphBars[i].GetComponent<Renderer>();
                Color _OrigColor = _Renderer.material.color;
                _Renderer.material.color = Color.Lerp(_OrigColor, new Color(0,0,0,0), 1 - _ColorLerpValue);
            }
        }
        tickTime = Time.time + updateSpeed;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (switchOnWarning)
        {
            if (serverUtils.GetServerData(warningParam) <= warningValue && !switched)
            {
                minH = warningMinHeight;
                maxH = warningMaxHeight;
                switched = true;
            }
            else if (serverUtils.GetServerData(warningParam) > warningValue && switched)
            {
                minH = minHeight;
                maxH = maxHeight;
                switched = false;
            }
        }

        if (useSineWave)
        {
            //time increment
            if (Time.time > tickTime)
            {
                timeIndex += Time.deltaTime * speed;
                float sinWave = Mathf.Sin(timeIndex);
                System.Array.Copy(graphHeights, 1, graphHeights, 0, n - 1);
                graphHeights[n - 1] = (((sinWave + 1 + minH) * maxH) * 0.5f) + Random.Range(0, noise);
                for (int i = 0; i < n; i++)
                {
                    graphBars[i].GetComponent<graphicsSlicedMesh>().Height = graphHeights[i];
                }
                tickTime = Time.time + updateSpeed;
            }
        }
        else
        {
            //time increment
            if (Time.time > tickTime)
            {
                System.Array.Copy(graphHeights, 1, graphHeights, 0, n - 1);
                graphHeights[n - 1] = Random.Range(minH, maxH);
                for (int i = 0; i < n; i++)
                {
                    graphBars[i].GetComponent<graphicsSlicedMesh>().Height = graphHeights[i];
                }
                tickTime = Time.time + updateSpeed;
            }
        }
	}
}
