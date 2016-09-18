using UnityEngine;
using System.Collections;
using Meg.Networking;

public class PopupTarget : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]
    
    /** Id for this popup target. */
    public string Id;

    /** Optional offset (local-space). */
    public Vector3 Offset;

    /** Optional bounding collider. */
    public Collider BoundingCollider;

    /** Optional bounding renderer. */
    public Renderer BoundingRenderer;

    /** Bounds for this popup target. */
    public Bounds Bounds
        { get { return GetBounds(); } }

    /** Popup shared data. */
    public popupData PopupData { get { return serverUtils.PopupData; } }


    // Unity Methods
    // ------------------------------------------------------------

    /** Enabling. */
    private void OnEnable()
    {
        if (string.IsNullOrEmpty(Id))
            Id = name;

        if (PopupData)
            PopupData.RegisterTarget(this);
    }

    /** Disabling. */
    private void OnDisable()
    {
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Determine the popup's current worldspace bounds. */
    private Bounds GetBounds()
    {
        // Try to resolve bounds using preset collider or renderer.
        if (BoundingCollider)
            return BoundingCollider.bounds;
        if (BoundingRenderer)
            return BoundingRenderer.bounds;

        // If no bounds are specified, return empty bounds.
        var origin = transform.TransformPoint(Offset);
        return new Bounds(origin, Vector3.zero);
    }

}
