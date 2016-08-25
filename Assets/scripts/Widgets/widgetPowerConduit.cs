using UnityEngine;
using System.Collections;

public class widgetPowerConduit : MonoBehaviour
{
    /** The conduit renderer. */
    public graphicsConduit Conduit;

    /** Color of conduit when it's inactive. */
    public Color InactiveColor;

    /** Color of conduit when it's active. */
    public Color ActiveColor;

    /** Speed of conduit at max power (texture space). */
    public float MaxScrollSpeed = 10;

    /** Conduit activity level [0..100]. */
    public float Value;

    /** Animate y offset instead of x*/
    public bool animateYOffset = false;

    /** Conduit's renderer. */
    private MeshRenderer _renderer;

    /** Conduit's texture offset. */
    private Vector2 _offset;

    /** Initialization. */
	private void Start ()
	{
	    if (!Conduit)
	        Conduit = GetComponent<graphicsConduit>();

        if (Conduit)
	        _renderer = Conduit.GetComponent<MeshRenderer>();
        else
            _renderer = GetComponent<MeshRenderer>();
    }
	
    /** Updating. */
	private void Update()
	{
        if (animateYOffset)
        {
            _offset.y -= GetScrollSpeed(Value) * Time.deltaTime;
        }
        else
        {
            _offset.x -= GetScrollSpeed(Value) * Time.deltaTime;
        }
        
        _renderer.material.SetColor("_TintColor", GetConduitColor(Value));
        _renderer.material.SetTextureOffset("_MainTex", _offset);
    }

    /** Return a scrolling speed based on activity level. */
    private float GetScrollSpeed(float value)
    {
        return MaxScrollSpeed * Mathf.Clamp01(value * 0.01f);
    }

    /** Returns the conduit's current color based on activity level. */
    private Color GetConduitColor(float value)
    {
        var f = Mathf.Clamp01(value * 0.01f);
        return Color.Lerp(InactiveColor, ActiveColor, f);
    }

}
