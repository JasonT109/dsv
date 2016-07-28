using UnityEngine;

public static class JSONExtensions
{

    public static Vector3 toVector(this JSONObject json)
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

    public static void GetField(this JSONObject json, ref Vector3 v, string field)
    {
        JSONObject o = json[field];
        v.x = o[0].f;
        v.y = o[1].f;
        v.z = o[2].f;
    }


}
