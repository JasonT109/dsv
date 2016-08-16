using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Meg.Networking;

namespace Meg.Parameters
{

    /** A file containing parameters of timed parameters. */

    [System.Serializable]
    public class megParameterFile
    {

        // Properties
        // ------------------------------------------------------------

        /** The parameters managed by this file. */
        public List<megParameterGroup> groups = new List<megParameterGroup>();

        /** Whether file is empty. */
        public bool empty { get { return groups.Count == 0; } }

        /** Whether file can be added to. */
        public bool canAdd { get { return true; } }

        /** Whether file can be removed from. */
        public bool canRemove { get { return !empty; } }

        /** Whether file can be cleared. */
        public bool canClear { get { return !empty; } }

        /** Whether file can be saved at the moment. */
        public bool canSave { get { return !empty; } }

        /** The selected parameter group (if any). */
        public megParameterGroup selectedGroup { get; set; }

        /** The selected parameter (if any). */
        public megParameter selectedParameter { get; set; }


        // Parameters
        // ------------------------------------------------------------

        /** General signature for file parameters. */
        public delegate void megParameterFileHandler(megParameterFile file);

        /** Parameter fired when file is loaded. */
        public megParameterFileHandler Loaded;

        /** Parameter fired when file is saved. */
        public megParameterFileHandler SavedToFile;

        /** Parameter fired when file is cleared. */
        public megParameterFileHandler Cleared;


        // Structures
        // ------------------------------------------------------------

        /** Tracking data for an applied parameter. */
        private struct ParameterRecord<T>
        {
            public float time;
            public T value;
        }


        // Members
        // ------------------------------------------------------------

        /** Tracking data for server values. */
        private readonly Dictionary<string, ParameterRecord<float>> _values = new Dictionary<string, ParameterRecord<float>>();


        // Public Methods
        // ------------------------------------------------------------

        /** Add an group of a given type. */
        public megParameterGroup AddGroup()
        {
            var group = CreateGroup();
            groups.Add(group);
            return group;
        }

        /** Insert an group of a given type after the given group. */
        public megParameterGroup InsertGroup(megParameterGroup insertAfter)
        {
            var group = CreateGroup();
            var insertIndex = groups.IndexOf(insertAfter);
            if (insertIndex >= 0)
                groups.Insert(insertIndex + 1, group);
            else
                groups.Add(group);

            return group;
        }

        /** Create an group of a given type. */
        public megParameterGroup CreateGroup()
        { return new megParameterGroup(this); }

        /** Remove an group from the group. */
        public void RemoveGroup(megParameterGroup group)
        {
            groups.Remove(group);

            if (group == selectedGroup)
                selectedGroup = null;
        }

        /** Clear the file of all groups. */
        public void Clear()
        {
            groups.Clear();

            selectedGroup = null;
            selectedParameter = null;

            if (Cleared != null)
                Cleared(this);
        }


        // Server State
        // ------------------------------------------------------------

        /** Set a server float value. */
        public void PostServerData(string key, float value, bool add)
        {
            // Record initial value if this is the first time we've set it.
            // This will be used to reset the value when file playback stops.
            if (!_values.ContainsKey(key))
                _values[key] = new ParameterRecord<float>
                    { time = Time.time, value = serverUtils.GetServerData(key) };

            // Set the server data value.
            serverUtils.PostServerData(key, value, add);
        }

        /** Return a server value. */
        public float GetServerData(string key)
            { return serverUtils.GetServerData(key); }

        /** Set a server string value. */
        public void PostServerData(string key, string value)
            { serverUtils.PostServerData(key, value); }

        /** Set a server boolean value. */
        public void PostServerData(string key, bool value)
            { serverUtils.PostServerData(key, value ? 1 : 0); }


        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public virtual JSONObject Save()
        {
            var json = new JSONObject();
            var parametersJson = new JSONObject(JSONObject.Type.ARRAY);
            foreach (var g in groups)
                parametersJson.Add(g.Save());

            json.AddField("groups", parametersJson);
            return json;
        }

        /** Load state from JSON. */
        public virtual void Load(JSONObject json)
        {
            // Load in value parameters.
            var parametersJson = json.GetField("groups");
            for (var i = 0; i < parametersJson.Count; i++)
            {
                var group = AddGroup();
                group.Load(parametersJson[i]);
            }

            if (Loaded != null)
                Loaded(this);
        }

        /** Load state from a JSON file. */
        public virtual void LoadFromFile(string path)
        {
            var text = File.ReadAllText(path);
            var json = new JSONObject(text);
            Load(json);
        }

        /** Save state to a JSON file. */
        public virtual void SaveToFile(string path)
        {
            var json = Save();
            var text = json.Print(true);

            var info = new FileInfo(path);
            var folder = info.DirectoryName;
            if (folder != null && !Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            File.WriteAllText(path, text);

            if (SavedToFile != null)
                SavedToFile(this);
        }


        // Private Methods
        // ------------------------------------------------------------

        /** Reset server state to initial settings. */
        private void ResetServerState()
        {
            // Reset data values from parameters.
            foreach (var e in _values)
                serverUtils.PostServerData(e.Key, e.Value.value);

            // Clear all tracking data.
            _values.Clear();
        }

    }
}
