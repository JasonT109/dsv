using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ObjectFinder
{

    // Static Members
    // ------------------------------------------------------------

    /** Lookup table to accelerate Locate() calls. */
    private static readonly Dictionary<Type, Component> ComponentLookup = new Dictionary<Type, Component>();

    /** Lookup table to accelerate LocateUiObject() calls. */
    private static readonly Dictionary<string, GameObject> UiObjectLookup = new Dictionary<string, GameObject>();


    // Static Methods
    // ------------------------------------------------------------

    /** Locate all instances of a component in the scene (including inactive objects). */
    public static IEnumerable<T> LocateAll<T>() where T : Component
        { return Resources.FindObjectsOfTypeAll<T>(); }

    /** Locate the first instance of a component in the scene. */
    public static T Locate<T>() where T : Component
    {
        Component result;
        if (ComponentLookup.TryGetValue(typeof(T), out result) && result)
            return result as T;

        result = LocateAll<T>().FirstOrDefault();
        if (result)
            ComponentLookup[typeof(T)] = result;

        return (T) result;
    }

    /** Locate the first instance of a game object containing the given component type in the scene. */
    public static GameObject LocateGameObject<T>() where T : Component
    {
        var component = Locate<T>();
        return component ? component.gameObject : null;
    }

    /** 
     * Locate a gameobject in the UI hierarchy. 
     * Very expensive - don't call this on a per-frame basis.
     * You can optionally specify an starting path in the UI scene such as 'Screens/Screen_Navigation'.
     * If the starting path doesn't exist, search will look through the whole UI subtree.
     */
    public static GameObject LocateUiByName(string name, string startingPath = null)
    {
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
    public static GameObject LocateUiByRegex(string pattern, string startingPath = null)
    {
        return LocateUiByRegex(new Regex(pattern), startingPath);
    }

    /** 
     * Locate a gameobject in the UI hierarchy via regex. 
     * Very expensive - don't call this on a per-frame basis.
     */
    public static GameObject LocateUiByRegex(Regex regex, string startingPath = null)
    {
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

        var t = ui.FindChildRecursiveRegex(regex);
        if (!t)
            return result;

        result = t.gameObject;
        UiObjectLookup[key] = result;
        return result;
    }

}
