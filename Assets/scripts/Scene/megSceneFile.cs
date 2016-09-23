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

        /** Default format for scene filenames. */
        public const string DefaultSaveFormat = @"{save-folder}/Scene_{1:000}/{0}_Scene_{1:000}_{2:000}_{3:000}_UTC_{4:yyyy.MM.dd}_{4:HH.mm.ss.f}.json";

        /** Default format for autosave filenames. */
        public const string DefaultAutoSaveFormat = @"{auto-save-folder}/Scene_{1:000}/{5}/{0}_Scene_{1:000}_{2:000}_{3:000}_UTC_{4:yyyy.MM.dd}_{4:HH.mm.ss.f}_{5}.json";


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

        /** Save out scene state to a scene file. */
        public static void SaveScene()
            { SaveToFile(GetSceneSaveFilename()); }

        /** Save out scene state to an autosave file. */
        public static void AutoSave(string suffix)
            { SaveToFile(GetAutoSaveFilename(suffix)); }

        /** Save out scene state to file. */
        public static void Save(string format, string suffix)
            { SaveToFile(GetSaveFilename(format, suffix)); }

        /** Return a save filename for the current scene state. */
        public static string GetSceneSaveFilename(string suffix = "")
            { return GetSaveFilename(Configuration.Get("save-format", DefaultSaveFormat), suffix); }

        /** Return a autosave filename for the current scene state. */
        public static string GetAutoSaveFilename(string suffix)
            { return GetSaveFilename(Configuration.Get("auto-save-format", DefaultAutoSaveFormat), suffix); }

        /** Return a save filename for the current scene state. */
        public static string GetSaveFilename(string format, string suffix = "")
        {
            // Expand out any config paths in the save file format string.
            format = Configuration.ExpandedPath(format);

            // Get player vessel info.  If name is empty, use the vessel number instead.
            var vessel = serverUtils.GetServerDataAsText("playerVesselName");
            if (string.IsNullOrEmpty(vessel))
                vessel = string.Format("Vessel_{0:00}", serverUtils.GetPlayerVessel());

            // Prepend an underscore to the suffix if one is supplied.
            if (!string.IsNullOrEmpty(suffix))
                suffix = "_" + suffix;

            // Gather all information required to determine the save filename.
            var scene = serverUtils.GetServerData("scene");
            var shot = serverUtils.GetServerData("shot");
            var take = serverUtils.GetServerData("take");
            var utc = DateTime.UtcNow;
            var path = string.Format(format, vessel, scene, shot, take, utc, suffix);

            return path;
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
            json.AddField("utc", string.Format("{0:yyyy/MM/dd hh:mm:ss.f}", DateTime.UtcNow));
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
                try
                    { json.AddField(parameter, serverUtils.GetServerDataRaw(parameter)); }
                catch (Exception ex)
                    { Debug.LogWarning("megSceneFile.SaveParameters(): Failed to save parameter '" + parameter + "' - " + ex); }

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
            return serverUtils.VesselMovements.Save();
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
                    try
                        { serverUtils.SetServerData(key, value); }
                    catch (Exception ex)
                        { Debug.LogWarning("megSceneFile.LoadParameters(): Failed to set parameter '" + key + "' to " + value + ":- " + ex); }
        }

        /** Load vessel states from JSON. */
        public void LoadVessels(JSONObject json)
        {
            serverUtils.VesselData.Load(json);
        }

        /** Load vessel movements from JSON. */
        private void LoadMovements(JSONObject json)
        {
            serverUtils.VesselMovements.Load(json);
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
