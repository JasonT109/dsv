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
                        switch (key)
                        {
                            case "vesselMovements":
                                serverUtils.GetVesselMovements().Load(j);
                                break;
                            default:
                                importData(j);
                                break;
                        }
                        break;
                    case JSONObject.Type.ARRAY:
                        switch (key)
                        {
                            case "vessel1Data":
                                importVesselData(1, j);
                                break;
                            case "vessel2Data":
                                importVesselData(2, j);
                                break;
                            case "vessel3Data":
                                importVesselData(3, j);
                                break;
                            case "vessel4Data":
                                importVesselData(4, j);
                                break;
                            case "vessel5Data":
                                importVesselData(5, j);
                                break;
                            case "vessel6Data":
                                importVesselData(6, j);
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

        /** Import vessel state from JSON. */
        private static void importVesselData(int vessel, JSONObject j)
        {
            var pos = new Vector3(j.list[0].n, j.list[1].n, j.list[2].n);
            serverUtils.SetVesselData(vessel, pos, j.list[3].n);

            if (j.list.Count > 4)
                serverUtils.SetVesselVis(vessel, j.list[4].b);
        }

        public static void megSaveJSONData(string path, string fileName, JSONObject saveData)
        {
            // Convert JSON object to string, with pretty-printing.
            string saveText = saveData.Print(true);
            exportData(path, fileName, saveText);
        }

        public static void megLoadJSONData(string path)
        {
            string encodedString = File.ReadAllText(path);
            JSONObject j = new JSONObject(encodedString);
            importData(j);
        }
    }
}