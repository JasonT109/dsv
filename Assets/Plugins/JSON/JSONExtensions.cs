using UnityEngine;
using System.Text.RegularExpressions;

public static class JSONExtensions
{

    public static Vector3 toVector3(this JSONObject json)
    {
        if (!json.IsArray || json.Count < 3)
            return Vector3.zero;

        return new Vector3(json[0].f, json[1].f, json[2].f);
    }

    public static void Add(this JSONObject json, Vector3 v)
    {
        var o = new JSONObject(JSONObject.Type.ARRAY);
        o.Add(v.x);
        o.Add(v.y);
        o.Add(v.z);
        json.Add(o);
    }

    public static void AddField(this JSONObject json, string field, Vector3 v)
    {
        var o = new JSONObject(JSONObject.Type.ARRAY);
        o.Add(v.x);
        o.Add(v.y);
        o.Add(v.z);
        json.AddField(field, o);
    }

    public static bool GetField(this JSONObject json, ref Vector3 v, string field)
    {
        var o = json[field];
        if (!o)
            return false;

        v.x = o[0].f;
        v.y = o[1].f;
        v.z = o[2].f;
        return true;
    }

    public static void Add(this JSONObject json, Vector2 v)
    {
        var o = new JSONObject(JSONObject.Type.ARRAY);
        o.Add(v.x);
        o.Add(v.y);
        json.Add(o);
    }

    public static void AddField(this JSONObject json, string field, Vector2 v)
    {
        var o = new JSONObject(JSONObject.Type.ARRAY);
        o.Add(v.x);
        o.Add(v.y);
        json.AddField(field, o);
    }

    public static bool GetField(this JSONObject json, ref Vector2 v, string field)
    {
        var o = json[field];
        if (!o)
            return false;

        v.x = o[0].f;
        v.y = o[1].f;
        return true;
    }

    public static void Add(this JSONObject json, Color c)
    {
        var o = new JSONObject(JSONObject.Type.ARRAY);
        o.Add(c.r);
        o.Add(c.g);
        o.Add(c.b);
        o.Add(c.a);
        json.Add(o);
    }

    public static void AddField(this JSONObject json, string field, Color c)
    {
        var o = new JSONObject(JSONObject.Type.ARRAY);
        o.Add(c.r);
        o.Add(c.g);
        o.Add(c.b);
        o.Add(c.a);
        json.AddField(field, o);
    }

    public static bool GetField(this JSONObject json, ref Color c, string field)
    {
        var o = json[field];
        if (!o)
            return false;

        if (o.IsArray && o.Count >= 3)
        {
            c.r = o[0].f;
            c.g = o[1].f;
            c.b = o[2].f;
            if (o.Count > 3)
                c.a = o[3].f;
        }
        else if (o.IsString)
        {
            var match = Regex.Match(o.str, @"^(#|0x)?([0-9A-F]{6,8})$", RegexOptions.IgnoreCase);
            if (match.Success)
                return ColorUtility.TryParseHtmlString("#" + match.Groups[2].Value, out c);
            else
                return false;
        }

        return true;
    }

}
