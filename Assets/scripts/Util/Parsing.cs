using UnityEngine;
using System.Collections;
using System.ComponentModel;

public static class Parsing
{

    // Static Methods
    // ------------------------------------------------------------

    /** Parse a string into the given type. */
    public static T Parse<T>(this string input)
        { return Parse(input, default(T)); }

    /** Parse a string into the given type. */
    public static T Parse<T>(this string input, T defaultValue)
    {
        if (string.IsNullOrEmpty(input))
            return defaultValue;

        var converter = TypeDescriptor.GetConverter(typeof(T));
        if (converter != null)
            return (T) converter.ConvertFromString(input);

        return defaultValue;
    }

}
