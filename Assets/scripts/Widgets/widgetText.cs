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

            if (ShrinkToFit)
                UpdateScaleToFit();
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

    /** Whether to scale text down if it gets too large. */
    public bool ShrinkToFit;

    /** Text mesh (if used). */
    public TextMesh TextMesh
        { get; protected set; }

    /** Dynamic text (if used). */
    public DynamicText DynamicText
        { get; protected set; }


    // Members
    // ------------------------------------------------------------

    /** Collider, used for computing shrink to fit sizing. */
    private Collider _collider;

    /** Max width and height for the text. */
    private Vector2 _maxSize;

    /** Initial scale for the text. */
    private Vector3 _initialScale;


    // Unity Methods
    // ------------------------------------------------------------

    protected virtual void Awake()
    {
        // Look for text components.
        if (!TextMesh)
            TextMesh = GetComponentInChildren<TextMesh>();
        if (!DynamicText && !TextMesh)
            DynamicText = GetComponentInChildren<DynamicText>();

        // Initialize sizing behaviour.
        InitializeSizing();
    }

    /** Initialize sizing information. */
    private void InitializeSizing()
    {
        if (!_collider)
            _collider = GetComponent<Collider>();
        if (!_collider)
            return;

        var min = transform.InverseTransformPoint(_collider.bounds.min);
        var max = transform.InverseTransformPoint(_collider.bounds.max);
        _maxSize = max - min;
        _initialScale = transform.localScale;
    }

    /** Update the local scale of text component to ensure it stays within the attached collider. */
    protected virtual void UpdateScaleToFit()
    {
        // Only works with dynamic text for now.
        // TODO: Add support for TextMesh.
        if (!_collider || !DynamicText)
            return;

        var b = DynamicText.bounds;

        Vector2 sizes = b.extents * 2;
        sizes.x /= _maxSize.x;
        sizes.y /= _maxSize.y;

        var scale = Mathf.Clamp01(1 / Mathf.Max(sizes.x, sizes.y));
        transform.localScale = _initialScale * scale;
    }

}
