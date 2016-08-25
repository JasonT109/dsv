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
        return new Bounds(transform.position, Vector3.zero);
    }

}
