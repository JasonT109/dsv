using UnityEngine;
using System.IO;
using Meg.Scene;

namespace Meg.JSON
{

    public class jsonData : MonoBehaviour
    {

        public static void megSaveJSONData(string path, string fileName, JSONObject json)
        {
            var text = json.Print(true);
            if (path != null && !Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllText(path + fileName, text);
        }

        public static void megLoadJSONData(string path)
        {
            megSceneFile.LoadFromFile(path);
        }
    }

}