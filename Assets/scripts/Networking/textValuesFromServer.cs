using System;
using UnityEngine;
using UnityEngine.Networking;
using Meg.Networking;

public class textValuesFromServer : widgetText
{
    public string[] linkDataStrings;
    public float scale = 1;
    public float updateTick = 0.2f;
    private float nextUpdate = 0;

    public string format = "";

    private float[] _values;

    private void Start()
    {
        _values = new float[linkDataStrings.Length];
    }

    private void Update()
    {
        if (Time.time < nextUpdate)
            return;

        nextUpdate = Time.time + updateTick;

        // Apply baseline value formatting.
        for (var i = 0; i < linkDataStrings.Length; i++)
        {
            var linkDataString = linkDataStrings[i];
            _values[i] = serverUtils.GetServerData(linkDataString) * scale;
        }

        // Update formatted text.
        try
        {
            switch (_values.Length)
            {
                case 0:
                    Text = string.Format(format);
                    break;
                case 1:
                    Text = string.Format(format, _values[0]);
                    break;
                case 2:
                    Text = string.Format(format, _values[0], _values[1]);
                    break;
                case 3:
                    Text = string.Format(format, _values[0], _values[1], _values[2]);
                    break;
                case 4:
                    Text = string.Format(format, _values[0], _values[1], _values[2], _values[3]);
                    break;
                default:
                    Text = string.Format(format);
                    break;
            }
        }
        catch (Exception)
        {
            // Consume any errors silently as this is non-critical functionality.
        }
    }

}
