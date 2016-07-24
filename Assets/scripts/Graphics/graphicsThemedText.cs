using UnityEngine;
using System.Collections;
using Meg.Graphics;
using Meg.Networking;

public class graphicsThemedText : MonoBehaviour
{
    public widgetText Text;
    public megColorTheme.Option Color = megColorTheme.Option.Text;

    private DynamicText _dynamicText;
    private TextMesh _textMesh;

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

        if (Text)
        {
            // Set color on the text widget.
            Text.Color = theme.GetColor(Color);
        }
        else
        {
            // Fall back to dynamic text if there is no widget.
            if (!_dynamicText && !_textMesh)
                _dynamicText = GetComponent<DynamicText>();
            if (_dynamicText)
            {
                _dynamicText.color = theme.GetColor(Color);
                return;
            }

            // Fall back to text mesh as a last resort.
            if (!_dynamicText && !_textMesh)
                _textMesh = GetComponent<TextMesh>();
            if (_textMesh)
            {
                _textMesh.color = theme.GetColor(Color);
                return;
            }
        }
    }


}
