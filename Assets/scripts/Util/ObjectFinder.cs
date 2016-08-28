using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Object = UnityEngine.Object;

public class ObjectFinder
{

    // Static Members
    // ------------------------------------------------------------

    /** Lookup table to accelerate Locate() calls. */
    private static readonly Dictionary<Type, Component> ComponentLookup = new Dictionary<Type, Component>();

    /** Lookup table to accelerate repeated LocateUi() calls. */
    private static readonly Dictionary<string, GameObject> UiObjectLookup = new Dictionary<string, GameObject>();

    /** Lookup table to accelerate repeated LocateUiByTag() calls. */
    private static readonly Dictionary<string, GameObject> UiTagLookup = new Dictionary<string, GameObject>();


    // Static Methods
    // ------------------------------------------------------------

    /** Locate all instances of a component in the scene (including inactive objects). */
    public static IEnumerable<T> FindAll<T>() where T : Component
        { return Resources.FindObjectsOfTypeAll<T>(); }

    /** Locate the first instance of a component in the scene. */
    public static T Find<T>() where T : Component
    {
        // Check if we have the object cached already.
        var type = typeof (T);
        Component result;
        if (ComponentLookup.TryGetValue(type, out result) && result)
            return result as T;

        // First, try finding an active object in the scene.
        result = Object.FindObjectOfType<T>();
        
        // If that fails, fall back to a way that works for inactive objects.
        if (!result)
            result = FindAll<T>().FirstOrDefault();

        // If we located the object, store a lookup entry for it.
        if (result)
            ComponentLookup[type] = result;

        // Return the located object.
        return (T) result;
    }

    /** Locate the first instance of a game object containing the given component type in the scene. */
    public static GameObject FindGameObject<T>() where T : Component
    {
        var component = Find<T>();
        return component ? component.gameObject : null;
    }

    /** Locate a UI gameobject by tag, optionally including inactive objects in the search. */
    public static GameObject FindUiByTag(string tag, string startingPath = null)
    {
        if (string.IsNullOrEmpty(tag))
            return null;

        // Check if we have the object locally cached.
        GameObject result;
        if (UiTagLookup.TryGetValue(tag, out result) && result)
            return result;

        // Try to use built-in tag method if possible.
        result = GameObject.FindWithTag(tag);
        if (result)
        {
            UiTagLookup[tag] = result;
            return result;
        }

        // Determine where to start the search.
        var ui = Camera.main.transform;
        if (!string.IsNullOrEmpty(startingPath))
        {
            var from = ui.Find(startingPath);
            if (from)
                ui = from;
        }

        // Search in the given subtree for a matching tag.
        var t = ui.FindChildByTagRecursive(tag);
        if (!t)
            return result;

        // Update the lookup table.
        result = t.gameObject;
        UiTagLookup[tag] = result;
        return result;
    }

    /** 
     * Locate a gameobject in the UI hierarchy. 
     * Very expensive - don't call this on a per-frame basis.
     * You can optionally specify an starting path in the UI scene such as 'Screens/Screen_Navigation'.
     * If the starting path doesn't exist, search will look through the whole UI subtree.
     */
    public static GameObject FindUiByName(string name, string startingPath = null)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        GameObject result;
        if (UiObjectLookup.TryGetValue(name, out result) && result)
            return result;

        var ui = Camera.main.transform;
        if (!string.IsNullOrEmpty(startingPath))
        {
            var from = ui.Find(startingPath);
            if (from)
                ui = from;
        }

        var t = ui.FindChildRecursive(name);
        if (!t)
            return result;

        result = t.gameObject;
        UiObjectLookup[name] = result;
        return result;
    }

    /** 
     * Locate a gameobject in the UI hierarchy via regex. 
     * Very expensive - don't call this on a per-frame basis.
     */
    public static GameObject FindUiByRegex(string pattern, string startingPath = null)
    {
        if (string.IsNullOrEmpty(pattern))
            return null;

        return FindUiByRegex(new Regex(pattern), startingPath);
    }

    /** 
     * Locate a gameobject in the UI hierarchy via regex. 
     * Very expensive - don't call this on a per-frame basis.
     */
    public static GameObject FindUiByRegex(Regex regex, string startingPath = null)
    {
        if (regex == null)
            return null;

        var key = regex.ToString();
        GameObject result;
        if (UiObjectLookup.TryGetValue(key, out result) && result)
            return result;

        var ui = Camera.main.transform;
        if (!string.IsNullOrEmpty(startingPath))
        {
            var from = ui.Find(startingPath);
            if (from)
                ui = from;
        }

        var t = ui.FindChildByRegexRecursive(regex);
        if (!t)
            return result;

        result = t.gameObject;
        UiObjectLookup[key] = result;
        return result;
    }

    /** Locate the first instance of a component in an object's parents. */
    public static T FindInParents<T>(GameObject go) where T : Component
        { return go ? FindInParents<T>(go.transform) : null; }

    /** Locate the first instance of a component in an object's parents. */
    public static T FindInParents<T>(Transform t) where T : Component
    {
        while (t != null)
        {
            var c = t.GetComponent<T>();
            if (c)
                return c;

            t = t.parent;
        }

        return null;
    }

}
