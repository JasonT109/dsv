using UnityEngine;
using System.Collections;
using Vectrosity;
using Meg.Maths;

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
    private float timeIndex = 0.0f;

    public bool useGaugeAverage = false;
    public digital_gauge[] gauges;
    public float gaugeAverage;

    void Start()
    {
        graphHeights = new float[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            if (useGaugeAverage)
            {
                graphHeights[i] = GetInitGaugeAverage();
                graphHeights[i] += Random.Range(-1, 1);
            }
            else
            {
                graphHeights[i] = Random.Range(minHeight, maxHeight);
            }
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

        return graphicsMaths.remapValue(gAvg, 0, 100, 0, graphMax);
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

        return graphicsMaths.remapValue(gAvg, 0, 100, 0, graphMax);
    }

    void Update()
    {
        if (Time.time > tickTime)
        {
            System.Array.Copy(graphHeights, 1, graphHeights, 0, numPoints - 1);

            if (useGaugeAverage)
            {
                gaugeAverage = GetGaugeAverage();
                graphHeights[numPoints - 1] = gaugeAverage;
                for (int i = 0; i < numPoints; i++)
                {
                    line.vectorLine.points2[i] = new Vector2((graphWidth / numPoints) * i, Mathf.Clamp(graphHeights[i], 0, graphMax));
                }
            }
            else
            {
                graphHeights[numPoints - 1] = Random.Range(minHeight, maxHeight);
                for (int i = 0; i < numPoints; i++)
                {
                    line.vectorLine.points2[i] = new Vector2((graphWidth / numPoints) * i, Mathf.Clamp(graphHeights[i], 0, graphMax));
                }
            }
            tickTime = Time.time + updateSpeed;
            line.vectorLine.Draw();
        }
    }
}
