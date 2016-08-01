using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/** 
 * Sets a Button or Toggle icon to semitransparent when disabled. 
 * Attach this script to the button/toggle (or its icon.)
 */

public class SelectableIconAlpha : MonoBehaviour
{

    private const float EnabledAlpha = 1.0f;
    private const float DisabledAlpha = 0.3f;

    public Selectable Selectable;
    public Image Icon;

    private void Start()
    {
        // Locate the button component.
        if (!Selectable)
            Selectable = GetComponentInChildren<Selectable>();
        if (!Selectable)
            Selectable = GetComponentInParent<Selectable>();
        if (!Selectable)
            return;

        // Look for an icon in the button's children.
        if (!Icon)
        {
            var n = Selectable.transform.childCount;
            for (var i = 0; i < n; i++)
            {
                var t = Selectable.transform.GetChild(i);
                if (!t.gameObject.activeSelf)
                    continue;

                Icon = t.GetComponentInChildren<Image>();
                if (Icon)
                    break;
            }
        }

        Update();
    }
	
	private void Update()
    {
        if (!Selectable || !Icon)
            return;

	    var c = Icon.color;
	    var interactible = Selectable.IsInteractable();
        if (interactible && c.a < EnabledAlpha)
            Icon.color = new Color(c.r, c.g, c.b, EnabledAlpha);
        else if (!interactible && c.a >= EnabledAlpha)
            Icon.color = new Color(c.r, c.g, c.b, DisabledAlpha);
    }

}
