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
    public static Transform FindChildRecursiveRegex(this Transform transform, Regex regex)
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
                child = t.FindChildRecursiveRegex(regex);
                if (child)
                {
                    return child;
                }
            }
        }

        return child;
    }

}
