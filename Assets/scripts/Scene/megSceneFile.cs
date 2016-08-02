using UnityEngine;
using System.IO;
using Meg.EventSystem;
using Meg.Networking;

namespace Meg.Scene
{

    public class megSceneFile
    {

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
            json.AddField("parameters", SaveParameters());
            json.AddField("vessels", SaveVessels());
            json.AddField("movements", SaveMovements());
            json.AddField("events", SaveEvents());

            return json;
        }


        // Save Methods
        // ------------------------------------------------------------

        /** Save server parameter state to JSON. */
        private JSONObject SaveParameters()
        {
            var json = new JSONObject();
            foreach (var parameter in serverUtils.Parameters)
                json.AddField(parameter, serverUtils.GetServerData(parameter));

            return json;
        }

        /** Save state of all vessels to JSON. */
        private JSONObject SaveVessels()
        {
            var json = new JSONObject();
            var n = serverUtils.GetVesselCount();
            for (var vessel = 1; vessel <= n; vessel++)
                json.AddField(vessel.ToString(), SaveVessel(vessel));

            return json;
        }

        /** Save a vessel's state to JSON. */
        private JSONObject SaveVessel(int vessel)
        {
            var json = new JSONObject();
            json.AddField("position", serverUtils.GetVesselPosition(vessel));
            json.AddField("velocity", serverUtils.GetVesselVelocity(vessel));
            json.AddField("visible", serverUtils.GetVesselVis(vessel));

            return json;
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
            for (var i = 0; i < json.Count; i++)
                LoadVessel(i + 1, json[i]);
        }

        /** Load vessel state from JSON. */
        private void LoadVessel(int vessel, JSONObject json)
        {
            var position = serverUtils.GetVesselPosition(vessel);
            var velocity = serverUtils.GetVesselVelocity(vessel);
            var visible = serverUtils.GetVesselVis(vessel);

            json.GetField(ref position, "position");
            json.GetField(ref velocity, "velocity");
            json.GetField(ref visible, "visible");

            serverUtils.SetVesselData(vessel, position, velocity);

            // If this is the player vessel, reset its world velocity.
            if (velocity <= 0 && vessel == serverUtils.GetPlayerVessel())
                serverUtils.SetPlayerWorldVelocity(Vector3.zero);
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