using UnityEngine;
using System.Collections;
using Vectrosity;
using Meg.Maths;
using Meg.Networking;

public class graphicsLineGraph : MonoBehaviour
{
    public VectorObject2D line;
    public int numPoints;
    public float graphMax = 19f;
    public float graphWidth = 14.6f;
    public float updateSpeed = 1.0f;
    public float speed = 1.0f;
    public float maxHeight = 10.0f;
    public float minHeight = 5.0f;
    private float[] graphHeights;
    private float tickTime;
    // private float timeIndex = 0.0f;

    public bool useGaugeAverage = false;
    public digital_gauge[] gauges;
    public float gaugeAverage;

    public bool useServerValue = false;
    public string linkDataString;

    public float valueMin = 0f;
    public float valueMax = 100f;

    public bool doWobble = false;
    public float valueWobble = 0.0f;
    public float wobbleSpeed = 0.0f;

    public bool doNoise = false;
    public SmoothNoise noise;

    private float index;
    private float wobble = 1.0f;
    private float wobbleAccumulate = 0.0f;

    void Start()
    {
        if (doNoise)
            noise.Start();

        graphHeights = new float[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            if (useGaugeAverage)
                graphHeights[i] = GetInitGaugeAverage() + Random.Range(-1, 1);
            else if (useServerValue)
                graphHeights[i] = GetServerValue() + Random.Range(-1, 1);
            else
                graphHeights[i] = Random.Range(minHeight, maxHeight);

            line.vectorLine.points2[i] = new Vector2((graphWidth / numPoints) * i, graphHeights[i]);
        }

        line.vectorLine.Draw();
    }

    float GetInitGaugeAverage()
    {
        float gAvg = 0;
        for (int g = 0; g < gauges.Length; g++)
        {
            gAvg += gauges[g].staticValue;
        }

        if (gAvg != 0 && gauges.Length != 0)
            gAvg = gAvg / gauges.Length;

        return graphicsMaths.remapValue(gAvg, valueMin, valueMax, 0, graphMax);
    }

    float GetGaugeAverage()
    {
        float gAvg = 0;
        for (int g = 0; g < gauges.Length; g++)
        {
            gAvg += gauges[g].value;
            gAvg += Random.Range(-1, 1);
        }

        if (gAvg != 0 && gauges.Length != 0)
            gAvg = gAvg / gauges.Length;

        return graphicsMaths.remapValue(gAvg, valueMin, valueMax, 0, graphMax);
    }

    float GetServerValue()
    {
        var value = serverUtils.GetServerData(linkDataString);
        return graphicsMaths.remapValue(value, valueMin, valueMax, 0, graphMax);
    }

    void Update()
    {
        if (doNoise)
            noise.Update();

        if (Time.time > tickTime)
        {
            // Move previous points back one step.
            System.Array.Copy(graphHeights, 1, graphHeights, 0, numPoints - 1);

            // Sample the current graph value.
            var value = Random.Range(minHeight, maxHeight);
            if (useGaugeAverage)
            { 
                gaugeAverage = GetGaugeAverage();
                value = gaugeAverage;
            }
            else if (useServerValue)
                value = GetServerValue();

            // Apply wobble to new value.
            if (doWobble)
            {
                index += Time.deltaTime;
                wobble = valueWobble * Mathf.Sin(wobbleSpeed * index);
                wobbleAccumulate += wobble * 0.1f;
                value += graphicsMaths.remapValue(wobbleAccumulate, valueMin, valueMax, 0, graphMax);
            }

            // Apply noise to new value.
            value += graphicsMaths.remapValue(noise.Value, valueMin, valueMax, 0, graphMax);

            // Clamp the new value to legal graph range.
            value = Mathf.Clamp(value, 0, graphMax);

            // Add the newest value to the graph.
            graphHeights[numPoints - 1] = value;
            for (int i = 0; i < numPoints; i++)
                line.vectorLine.points2[i] = new Vector2((graphWidth / numPoints) * i, Mathf.Clamp(graphHeights[i], 0, graphMax));

            // Draw the updated graph line.
            line.vectorLine.Draw();
            tickTime = Time.time + updateSpeed;
        }
    }
}
