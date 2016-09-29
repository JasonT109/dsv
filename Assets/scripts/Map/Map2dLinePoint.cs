using System;
using UnityEngine;
using TouchScript.Behaviors;
using TouchScript.Gestures;
using UnityEngine.UI;


public class Map2dLinePoint : MonoBehaviour
{
    public int Index { get; set; }
    public Map2dLine Parent { get; set; }

    [Header("Components")]
    public Transformer Transformer;
    public TransformGesture TransformGesture;

    
    // Members
    // ------------------------------------------------------------

    /** Graphical elements in the point. */
    private Graphic[] _graphics;

    /** Canvas group. */
    private CanvasGroup _group;


    // Unity Methods
    // ------------------------------------------------------------

    private void Awake()
    {
        if (!TransformGesture)
            TransformGesture = Transformer.GetComponent<TransformGesture>();

        if (TransformGesture)
            TransformGesture.TransformStarted += OnTransformStarted;

        _graphics = transform.GetComponentsInChildren<Graphic>(true);
        _group = GetComponent<CanvasGroup>();
    }

    private void OnTransformStarted(object sender, EventArgs e)
    {
        // transform.SetAsLastSibling();
    }

    private void OnEnable()
    {
        if (Transformer)
            Transformer.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if (Transformer)
            Transformer.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        // Clean up transformer (since we decoupled it and made it a sibling).
        if (Transformer)
            Transformer.gameObject.Cleanup();
    }

    private void UpdateWhileTransforming()
    {
        transform.localPosition = Transformer.transform.localPosition;
        Parent.SetPoint(Index, transform.localPosition);
    }


    // Public Methods
    // ------------------------------------------------------------

    public void UpdatePoint()
    {
        // Set point's properties.
        transform.localPosition = Parent.Line.Points[Index];
        _group.alpha = Parent.GetPointAlpha(Index);
        for (var i = 0; i < _graphics.Length; i++)
            _graphics[i].color = Parent.Line.Color;

        // Update transformer (if any).
        if (Transformer)
            UpdateTransformer();
    }


    // Private Methods
    // ------------------------------------------------------------

    private void UpdateTransformer()
    {
        // Only allow points to be moved when debug screen is open.
        var active = debugMapLinesUi.Instance.gameObject.activeInHierarchy;
        Transformer.gameObject.SetActive(active);
        TransformGesture.enabled = active;
        if (!active)
            return;

        // Reparent transformer to be a sibling of the ping.
        // This allows us to decouple transform interactions from the ping visuals.
        if (Transformer.transform.parent != transform.parent)
            Transformer.transform.SetParent(transform.parent);

        // Update vessel during transform interactions.
        var transforming = TransformGesture.State == Gesture.GestureState.Changed;
        if (transforming)
            UpdateWhileTransforming();
        else
        {
            Transformer.transform.localPosition = transform.localPosition;
            Transformer.transform.localScale = transform.localScale;
        }
    }

}

