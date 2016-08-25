using System;
using System.Linq;
using System.Text.RegularExpressions;

public static class CommandLine
{

    // Static Members
    // ------------------------------------------------------------

    /** The commandline arguments for this execution. */
    private static string[] _args;

    /** The commandline arguments for this execution. */
    public static string Value
        { get { return Environment.CommandLine; } }

    /** The commandline arguments for this execution. */
    public static string[] Args
        { get { return _args ?? (_args = Environment.GetCommandLineArgs()); } }


    // Static Methods
    // ------------------------------------------------------------

    /** Check if a given commandline parameter exists. */
    public static bool HasParameter(string parameter)
        { return Args != null && Args.Any(arg => Regex.IsMatch(arg, @"-?" + parameter)); }

    /** Parse the commandline for a typed parameter. */
    public static T GetParameter<T>(string parameter, T defaultValue)
        { return Parsing.Parse(GetParameter(parameter), defaultValue); }

    /** Parse the commandline for a parameter. */
    public static string GetParameter(string parameter, string defaultValue = null)
    {
        if (Args == null)
            return defaultValue;

        var args = Args;
        var s = defaultValue;
        var pattern = @"-?" + parameter;
        for (var i = 0; i < args.Length; i++)
            if (Regex.IsMatch(args[i], pattern) && args.Length >= i + 2)
                s = args[i + 1];

        return s;
    }

    /** Parse the commandline for a typed parameter. */
    public static T GetParameterByRegex<T>(string pattern, T defaultValue)
        { return Parsing.Parse(GetParameterByRegex(pattern), defaultValue); }

    /** Parse the commandline for a typed parameter. */
    public static T GetParameterByRegex<T>(Regex regex, T defaultValue)
        { return Parsing.Parse(GetParameterByRegex(regex), defaultValue); }

    /** Parse the commandline for a parameter using a regex (e.g. @"-id[\s|=]+(\w+)"). */
    public static string GetParameterByRegex(string pattern, string defaultValue = null)
        { return GetParameterByRegex(new Regex(pattern), defaultValue); }

    /** Parse the commandline for a parameter using a regex. */
    public static string GetParameterByRegex(Regex regex, string defaultValue = null)
    {
        var match = regex.Match(Value);
        if (match.Success && match.Groups.Count > 1)
            return match.Groups[1].Value;

        return defaultValue;
    }


}
