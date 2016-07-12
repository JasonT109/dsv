namespace Meg.JSON
{
    using UnityEngine;
    using System.Collections;
    using System.IO;
    using Meg.Networking;

    public class jsonData : MonoBehaviour
    {
        private static void exportData(string filePath, string data)
        {
            if (!File.Exists(filePath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(data);
                }
            }
            else
            {
                //confirm overwrite
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(data);
                }
            }
        }

        private static void importData(JSONObject obj)
        {
            for (int i = 0; i < obj.list.Count; i++)
            {
                string key = (string)obj.keys[i];
                JSONObject j = (JSONObject)obj.list[i];

                switch (j.type)
                {
                    case JSONObject.Type.OBJECT:
                        importData(j);
                        break;
                    case JSONObject.Type.ARRAY:
                        //do something else with this
                        Debug.Log("Key name: " + key + " array");
                        break;
                    case JSONObject.Type.STRING:
                        Debug.Log("Key name: " + key + " String value: " + j.str);
                        //set server string
                        break;
                    case JSONObject.Type.NUMBER:
                        Debug.Log("Key name: " + key + " Float value: " + j.n);
                        //set server float
                        serverUtils.SetServerData(key, j.n);
                        break;
                    case JSONObject.Type.BOOL:
                        Debug.Log("Key name: " + key + " Bool value: " + j.b);
                        //set server bool
                        break;
                    case JSONObject.Type.NULL:
                        Debug.Log("Key name: " + key + " is NULL.");
                        break;
                }
            }
        }

        public static void megSaveJSONData(string path, JSONObject saveData)
        {
            string saveText = saveData.Print();
            saveText = saveText.Replace("{", "{ " + System.Environment.NewLine);
            saveText = saveText.Replace(",", "," + System.Environment.NewLine);
            saveText = saveText.Replace("}", System.Environment.NewLine + "}");
            exportData(path, saveText);
            //Debug.Log(saveText);
        }

        public static void megLoadJSONData(string path)
        {
            string encodedString = "";
            JSONObject j;

            // Open the file to read from. 
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    encodedString += s;
                }
            }

            j = new JSONObject(encodedString);
            importData(j);
        }
    }
}