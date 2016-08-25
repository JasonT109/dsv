using UnityEngine;
using System.Collections;

public class PopupTarget : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]
    
    /** Id for this popup target. */
    public string Id;

    /** Optional bounding collider. */
    public Collider BoundingCollider;

    /** Optional bounding renderer. */
    public Renderer BoundingRenderer;

    /** Bounds for this popup target. */
    public Bounds Bounds
        { get { return GetBounds(); } }


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

        // If that fails, look for a collider or renderer component.
        if (!BoundingCollider)
            BoundingCollider = GetComponent<Collider>();
        if (BoundingCollider)
            return BoundingCollider.bounds;

        if (!BoundingRenderer)
            BoundingRenderer = GetComponent<Renderer>();
        if (BoundingRenderer)
            return BoundingRenderer.bounds;

        // Lastly, fall back to a child collider or renderer.
        if (!BoundingCollider)
            BoundingCollider = GetComponentInChildren<Collider>();
        if (BoundingCollider)
            return BoundingCollider.bounds;

        if (!BoundingRenderer)
            BoundingRenderer = GetComponentInChildren<Renderer>();
        if (BoundingRenderer)
            return BoundingRenderer.bounds;

        // Worst case - return empty bounds.
        return new Bounds(transform.position, Vector3.zero);
    }

}
