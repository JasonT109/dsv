using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Meg.EventSystem
{

    /** A file containing groups of timed events. */

    [System.Serializable]
    public class megEventFile
    {

        // Properties
        // ------------------------------------------------------------

        /** The events managed by this group. */
        public List<megEventGroup> groups = new List<megEventGroup>();

        /** Current local time. */
        public float time { get; set; }

        /** Whether file is currently running. */
        public bool running { get; set; }

        /** Whether file is currently paused. */
        public bool paused { get; set; }

        /** Whether file  is complete. */
        public bool completed { get; set; }


        // Public Methods
        // ------------------------------------------------------------

        /** Start this event group. */
        public void Start()
        {
            if (running)
                return;

            running = true;
            completed = false;
            paused = false;
            time = 0;

            if (groups == null)
                groups = new List<megEventGroup>();

            for (var i = 0; i < groups.Count; i++)
                groups[i].Start();
        }

        /** Update this event group as time passes. */
        public void Update(float t, float dt)
        {
            if (paused)
                return;

            if (running)
            {
                time += dt;
                completed = groups.All(e => e.completed);
            }

            for (var i = 0; i < groups.Count; i++)
                groups[i].Update(time, dt);
        }

        /** Pause playback on the file. */
        public void Pause()
        {
            paused = true;
        }

        /** Resume playback on the file. */
        public void Resume()
        {
            paused = false;
        }

        /** Stop this event group. */
        public void Stop()
        {
            if (!running)
                return;

            for (var i = 0; i < groups.Count; i++)
                groups[i].Stop();

            time = 0;
            running = false;
        }


        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public virtual JSONObject Save()
        {
            var json = new JSONObject();
            var groupsJson = new JSONObject(JSONObject.Type.ARRAY);
            foreach (var g in groups)
                groupsJson.Add(g.Save());

            json.AddField("groups", groupsJson);

            return json;
        }

        /** Load state from JSON. */
        public virtual void Load(JSONObject json)
        {
            // Load in value events.
            var groupsJson = json.GetField("groups");
            groups.Clear();
            for (var i = 0; i < groupsJson.Count; i++)
            {
                groups.Add(new megEventGroup());
                groups[i].Load(groupsJson[i]);
            }
        }

        /** Load state from a JSON file. */
        public virtual void LoadFromFile(string path)
        {
            try
            {
                var text = File.ReadAllText(path);
                var json = new JSONObject(text);
                Load(json);
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to load event file: " + path + ", error: " + ex);
            }
        }

    }

}