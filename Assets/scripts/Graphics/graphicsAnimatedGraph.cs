﻿using UnityEngine;
using System.Collections;
using System.Linq;
using Meg.Networking;

public class graphicsAnimatedGraph : MonoBehaviour {
    public GameObject[] graphBars;
    public float updateSpeed = 1.0f;
    public float speed = 1.0f;
    public float maxHeight = 10.0f;
    public float minHeight = 5.0f;
    public float noise = 0.2f;
    public bool useSineWave;
    public bool switchOnWarning = false;
    public string warningParam;
    public float warningValue;
    public float warningMaxHeight = 10.0f;
    public float warningMinHeight = 5.0f;

    private float minH;
    private float maxH;
    public float[] graphHeights;
    private int n;
    private float tickTime;
    private float timeIndex = 0.0f;
    private bool switched = false;

    void Start () {
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
