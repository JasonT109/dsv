using UnityEngine;
using UnityEngine.Networking;
using Meg.Networking;

public class textValueFromServer : widgetText
{
    public string linkDataString = "depth";
    public float scale = 1;
    public float updateTick = 0.2f;
    private float nextUpdate = 0;

    public bool upperCase;

    public string format = "";
    
    [System.Serializable]
    public struct ValueRange
    {
        public string Id;
        public Vector2 Range;
        public Color Color;
    }

    public ValueRange[] Ranges;

    private void Update()
    {
        if (Time.time < nextUpdate)
            return;

        nextUpdate = Time.time + updateTick;

        // Apply baseline value formatting.
        var value = serverUtils.GetServerData(linkDataString) * scale;
        if (string.IsNullOrEmpty(format))
            Text = serverUtils.GetServerDataAsText(linkDataString);
        else
            Text = string.Format(format, value);

        if (upperCase)
            Text = Text.ToUpper();

        // Apply custom formatting for value ranges (if any).
        for (var i = 0; i < Ranges.Length; i++)
        {
            var range = Ranges[i];
            if (value < range.Range.x || value > range.Range.y)
                continue;

            Color = range.Color;

            if (!string.IsNullOrEmpty(range.Id))
                Text = string.Format(range.Id, value);
        }

    }

}
