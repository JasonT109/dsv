using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

/** 
 * Override link data values in children. 
 * A link data string is the name of a server data value, e.g 'depth' or 'diveTime'.
 * Makes it easier to manage replicated UI elements such as battery bank readouts.
 */

public class linkDataOverrides : MonoBehaviour
{

    /** Specification for searching and replacing within link data strings. */
    [System.Serializable]
    public struct Override
    {
        public string pattern;
        public string replacement;
    }

    /** The collection of overrides to apply. */
    [Header("Overrides")]
    public Override[] Overrides;

    [Header("Configuration")]
    public bool ApplyOnAwake = true;

    /** The set of components to affect. */
    [Header("ComponentsToAffect")]
    public bool textValueFromServer = true;
    public bool textValuesFromServer = true;
    public bool valueFromServer = true;
    public bool buttonAutoWarning = true;
    public bool enableOnServerValue = true;
    public bool widgetFillBar = true;

    /** Initialization. */
    private void Awake()
    {
        if (ApplyOnAwake)
            Apply();
    }

    /** Update all link data within this object's children. */
    public void Apply()
    {
        if (textValueFromServer)
            foreach (var c in transform.GetComponentsInChildren<textValueFromServer>(true))
                c.linkDataString = GetLinkDataString(c.linkDataString);

        if (valueFromServer)
            foreach (var c in transform.GetComponentsInChildren<valueFromServer>(true))
                c.linkDataString = GetLinkDataString(c.linkDataString);

        if (textValuesFromServer)
            foreach (var c in transform.GetComponentsInChildren<textValuesFromServer>(true))
                for (var i = 0; i < c.linkDataStrings.Length; i++)
                    c.linkDataStrings[i] = GetLinkDataString(c.linkDataStrings[i]);

        if (widgetFillBar)
            foreach (var c in transform.GetComponentsInChildren<widgetFillBar>(true))
                c.serverValue = GetLinkDataString(c.serverValue);

        if (buttonAutoWarning)
            foreach (var c in transform.GetComponentsInChildren<buttonControl>(true))
                c.autoWarningServerName = GetLinkDataString(c.autoWarningServerName);

        if (enableOnServerValue)
            foreach (var c in transform.GetComponentsInChildren<enableOnServerValue>(true))
                c.linkDataString = GetLinkDataString(c.linkDataString);
    }

    /** Return an overridden link data string. */
    private string GetLinkDataString(string linkData)
    {
        foreach (var o in Overrides)
            linkData = Regex.Replace(linkData, o.pattern, o.replacement, RegexOptions.IgnoreCase);

        return linkData;
    }


}
