using UnityEngine;
using System.Collections;
using Meg.Graphics;
using Meg.Networking;

public class graphicsThemedText : MonoBehaviour
{
    public widgetText Text;
    public megColorTheme.Option Color = megColorTheme.Option.Text;

    void OnEnable()
    {
        updateColor();
    }

    void Update()
    {
        updateColor();
    }

    public void updateColor()
    {
        var theme = serverUtils.GetColorTheme();

        if (!Text)
            Text = GetComponent<widgetText>();

        if (!Text)
            return;

        Text.Color = theme.GetColor(Color);
    }

}
