using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public static class TransformExtensions
{

    /** Recursively search a transform's children for a named child. */
    public static Transform FindChildRecursive(this Transform transform, string name)
    {
        Transform child = null;
        foreach (Transform t in transform)
        {
            if (t.name == name)
            {
                child = t;
                return child;
            }
            if (t.childCount > 0)
            {
                child = t.FindChildRecursive(name);
                if (child)
                {
                    return child;
                }
            }
        }

        return child;
    }

    /** Recursively search a transform's children for a named child. */
    public static Transform FindChildByRegexRecursive(this Transform transform, Regex regex)
    {
        Transform child = null;
        foreach (Transform t in transform)
        {
            if (regex.IsMatch(t.name))
            {
                child = t;
                return child;
            }
            if (t.childCount > 0)
            {
                child = t.FindChildByRegexRecursive(regex);
                if (child)
                {
                    return child;
                }
            }
        }

        return child;
    }

    /** Search a transform's direct children for a given tag. */
    public static Transform FindChildByTag(this Transform transform, string tag)
    {
        foreach (Transform t in transform)
            if (t.tag == tag)
                return t;

        return null;
    }

    /** Recursively search a transform's children for a child with a given tag. */
    public static Transform FindChildByTagRecursive(this Transform transform, string tag)
    {
        Transform child = null;
        foreach (Transform t in transform)
        {
            if (t.tag == tag)
            {
                child = t;
                return child;
            }
            if (t.childCount > 0)
            {
                child = t.FindChildByTagRecursive(tag);
                if (child)
                {
                    return child;
                }
            }
        }

        return child;
    }

    /** Get a component in the transform's parents. */
    public static T GetComponentInParents<T>(this Transform transform) where T : Component
    {
        var t = transform.parent;
        while (t)
        {
            var c = t.GetComponent<T>();
            if (c)
                return c;

            t = t.parent;
        }

        return null;
    }


}
