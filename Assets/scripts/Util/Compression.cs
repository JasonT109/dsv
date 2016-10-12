using UnityEngine;
using System.Collections;
using System.IO;
using Unity.IO.Compression;

public static class Compression
{

    public static byte[] CompressJson(JSONObject json)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
            using (var writer = new StreamWriter(gzipStream))
            {
                writer.Write(json.Print(true));
            }

            return memoryStream.ToArray();
        }
    }

    public static JSONObject DecompressJson(byte[] data)
    {
        using (var memoryStream = new MemoryStream(data))
        {
            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            using (var reader = new StreamReader(gzipStream))
            {
                var text = reader.ReadToEnd();
                var json = new JSONObject(text);
                return json;
            }
        }
    }

}
