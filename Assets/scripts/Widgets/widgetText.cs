using UnityEngine;
using System.Collections;

/** Base class for widgets that deal with on-screen text. */

public class widgetText : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    /** Whether the widget has a text rendering component. */
    public bool HasRenderer
    {
        get { return TextMesh || DynamicText; }
    }

    /** The current text value. */
    public string Text
    {
        get
        {
            if (TextMesh)
                return TextMesh.text;
            else if (DynamicText)
                return DynamicText.GetText();
            else
                return "";
        }

        set
        {
            if (TextMesh)
                TextMesh.text = value;
            else if (DynamicText)
                DynamicText.SetText(value);
        }
    }

    /** Text color. */
    public Color Color
    {
        get
        {
            if (TextMesh)
                return TextMesh.color;
            else if (DynamicText)
                return DynamicText.color;
            else
                return Color.white;
        }

        set
        {
            if (TextMesh)
                TextMesh.color = value;
            else if (DynamicText)
                DynamicText.color = value;
        }
    }


    // Members
    // ------------------------------------------------------------

    /** Text mesh (if used). */
    public TextMesh TextMesh
        { get; protected set; }

    /** Dynamic text (if used). */
    public DynamicText DynamicText
        { get; protected set; }


    // Unity Methods
    // ------------------------------------------------------------

    protected virtual void Awake()
    {
        // Look for text components on the 
        if (!TextMesh)
            TextMesh = GetComponent<TextMesh>();
        if (!DynamicText && !TextMesh)
            DynamicText = GetComponent<DynamicText>();
    }
	
}
