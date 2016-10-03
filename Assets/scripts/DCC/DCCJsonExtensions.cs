using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class DCCJsonExtensions
{

    public static bool GetField(this JSONObject json, ref DCCWindow.contentID id, string field)
    {
        var name = "none";
        var result = json.GetField(ref name, field);
        try
            { id = DCCWindow.ContentForName(name); }
        catch (Exception)
            { result = false; }

        return result;
    }

}
