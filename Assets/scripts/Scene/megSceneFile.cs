using System;
using UnityEngine;
using System.IO;
using Meg.EventSystem;
using Meg.Networking;

namespace Meg.Scene
{

    public class megSceneFile
    {
        // Constants
        // ------------------------------------------------------------

        /** Default folder for autosave files. */
        public const string DefaultAutoSaveFolder = @"C:/Meg/Autosave/";

        /** Default format for autosave filenames. */
        public const string DefaultAutoSaveFormat = @"{0}/Scene_{2:000}/{6}/{1}_{2:000}_{3:00}_{4:00}_{5:dd.MM.yyyy}_{5:HH.mm.ss.f}_{6}.json";


        // Load / Save
        // ------------------------------------------------------------

        /** Load state from a JSON file. */
        public static void LoadFromFile(string path)
        {
            var file = new megSceneFile();
            var text = File.ReadAllText(path);
            var json = new JSONObject(text);
            file.Load(json);
        }

        /** Save state to a JSON file. */
        public static void SaveToFile(string path)
        {
            var info = new FileInfo(path);
            var folder = info.DirectoryName;
            if (folder != null && !Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var file = new megSceneFile();
            var json = file.Save();
            var text = json.Print(true);

            File.WriteAllText(path, text);
        }

        /** Whether autosaving is enabled. */
        public static bool AutoSaveEnabled()
            { return Configuration.Get("auto-save-enabled", true); }

        /** Save out scene state to an autosave file. */
        public static void AutoSave(string suffix)
        {
            // Gather all information required to determine the auto-save filename.
            var folder = Configuration.Get("auto-save-folder", DefaultAutoSaveFolder);
            var format = Configuration.Get("auto-save-format", DefaultAutoSaveFormat);
            var vessel = serverUtils.GetServerDataAsText("playerVesselName");
            var scene = serverUtils.GetServerData("scene");
            var shot = serverUtils.GetServerData("shot");
            var take = serverUtils.GetServerData("take");
            var utc = DateTime.UtcNow;
            var path = string.Format(format, folder, vessel, scene, shot, take, utc, suffix);

            // Save file to nominated path.
            SaveToFile(path);
        }

        /** Load state from JSON. */
        public void Load(JSONObject json)
        {
            var parameters = json.GetField("parameters");
            if (parameters)
                LoadParameters(parameters);

            var vessels = json.GetField("vessels");
            if (vessels)
                LoadVessels(vessels);

            var movements = json.GetField("movements");
            if (movements)
                LoadMovements(movements);

            var events = json.GetField("events");
            if (events)
                LoadEvents(events);
        }

        /** Save state to JSON. */
        public JSONObject Save()
        {
            var json = new JSONObject();
            json.AddField("metadata", SaveMetadata());
            json.AddField("parameters", SaveParameters());
            json.AddField("vessels", SaveVessels());
            json.AddField("movements", SaveMovements());
            json.AddField("events", SaveEvents());

            return json;
        }


        // Save Methods
        // ------------------------------------------------------------

        /** Save file metadata to JSON. */
        private JSONObject SaveMetadata()
        {
            var json = new JSONObject();
            json.AddField("utc", string.Format("{0:dd/MM/yy hh:mm:ss.f}", DateTime.UtcNow));
            json.AddField("scene", serverUtils.GetServerData("scene"));
            json.AddField("shot", serverUtils.GetServerData("shot"));
            json.AddField("take", serverUtils.GetServerData("take"));
            json.AddField("playervessel", serverUtils.GetServerData("playervessel"));
            json.AddField("playervesselname", serverUtils.GetServerDataAsText("playervesselname"));

            return json;
        }

        /** Save server parameter state to JSON. */
        private JSONObject SaveParameters()
        {
            var json = new JSONObject();
            foreach (var parameter in serverUtils.WriteableParameters)
                json.AddField(parameter, serverUtils.GetServerData(parameter));

            return json;
        }

        /** Save state of all vessels to JSON. */
        private JSONObject SaveVessels()
        {
            return serverUtils.VesselData.Save();
        }

        /** Save state of vessel movements to JSON. */
        private JSONObject SaveMovements()
        {
            return serverUtils.GetVesselMovements().Save();
        }

        /** Save current events to JSON. */
        private JSONObject SaveEvents()
        {
            var file = megEventManager.Instance.File;
            if (file != null)
                return file.Save();

            return new JSONObject();
        }


        // Load Methods
        // ------------------------------------------------------------

        /** Load vessel states from JSON. */
        public void LoadParameters(JSONObject json)
        {
            var value = 0.0f;
            foreach (var key in json.keys)
                if (json.GetField(ref value, key))
                    serverUtils.SetServerData(key, value);
        }

        /** Load vessel states from JSON. */
        public void LoadVessels(JSONObject json)
        {
            serverUtils.VesselData.Load(json);
        }

        /** Load vessel movements from JSON. */
        private void LoadMovements(JSONObject json)
        {
            serverUtils.GetVesselMovements().Load(json);
        }

        /** Load current events from JSON. */
        private void LoadEvents(JSONObject json)
        {
            var file = megEventManager.Instance.File;
            if (file != null)
            {
                file.Clear();
                file.Load(json);
            }
        }

    }

}