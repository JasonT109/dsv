namespace Meg.JSON
{
    using UnityEngine;
    using System.Collections;
    using System.IO;
    using Meg.Networking;

    public class jsonData : MonoBehaviour
    {
        private static void exportData(string filePath, string fileName, string data)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            if (!File.Exists(filePath + fileName))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath + fileName))
                {
                    sw.WriteLine(data);
                }
            }
            else
            {
                //confirm overwrite
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath + fileName))
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
                        //decide what to do with this data
                        switch (key)
                        {
                            case "vessel1Data":
                                Debug.Log("Setting vessel 1 to: " + new Vector3(j.list[0].n, j.list[1].n, j.list[2].n));
                                serverUtils.SetVesselData(1, new Vector3(j.list[0].n, j.list[1].n, j.list[2].n), j.list[3].n);
                                break;
                            case "vessel2Data":
                                serverUtils.SetVesselData(2, new Vector3(j.list[0].n, j.list[1].n, j.list[2].n), j.list[3].n);
                                break;
                            case "vessel3Data":
                                serverUtils.SetVesselData(3, new Vector3(j.list[0].n, j.list[1].n, j.list[2].n), j.list[3].n);
                                break;
                            case "vessel4Data":
                                serverUtils.SetVesselData(4, new Vector3(j.list[0].n, j.list[1].n, j.list[2].n), j.list[3].n);
                                break;
                            case "vessel5Data":
                                serverUtils.SetVesselData(5, new Vector3(j.list[0].n, j.list[1].n, j.list[2].n), j.list[3].n);
                                break;
                        }
                        break;
                    case JSONObject.Type.STRING:
                        //set server string
                        break;
                    case JSONObject.Type.NUMBER:
                        //set server float
                        serverUtils.SetServerData(key, j.n);
                        break;
                    case JSONObject.Type.BOOL:
                        //set server bool
                        break;
                    case JSONObject.Type.NULL:
                        break;
                }
            }
        }

        public static void megSaveJSONData(string path, string fileName, JSONObject saveData)
        {
            string saveText = saveData.Print();
            saveText = saveText.Replace("{", "{ " + System.Environment.NewLine);
            saveText = saveText.Replace(",", "," + System.Environment.NewLine);
            saveText = saveText.Replace("}", System.Environment.NewLine + "}");
            exportData(path, fileName, saveText);
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