using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Meg.Networking;

namespace Meg.EventSystem
{

    /** A file containing groups of timed events. */

    [System.Serializable]
    public class megEventFile
    {

        // Properties
        // ------------------------------------------------------------

        /** The event groups managed by this file. */
        public List<megEventGroup> groups = new List<megEventGroup>();

        /** All events in the file. */
        public IEnumerable<megEvent> events { get { return groups.SelectMany(g => g.events); } }

        /** Current local time. */
        public float time { get; set; }

        /** Whether file is currently running. */
        public bool running { get; set; }

        /** Whether file is currently paused. */
        public bool paused { get; set; }

        /** Whether file is complete. */
        public bool completed { get; set; }

        /** Whether file is playing back at the moment (running and active). */
        public bool playing { get { return running && !paused; } }

        /** Local time at which the file ends. */
        public float endTime { get { return empty ? 0 : groups.Max(e => e.endTime); } }

        /** Whether file is empty. */
        public bool empty { get { return groups.Count == 0; } }

        /** Whether file can be played. */
        public bool canPlay { get { return !empty; } }

        /** Whether file can be rewound. */
        public bool canRewind { get { return !empty && !playing && running; } }

        /** Whether file can be added to. */
        public bool canAdd { get { return !playing; } }

        /** Whether file can be removed from. */
        public bool canRemove { get { return !empty && !playing; } }

        /** Whether file can be cleared. */
        public bool canClear { get { return !empty && !playing; } }

        /** Whether file can be saved at the moment. */
        public bool canSave { get { return !empty && !playing; } }

        /** The selected event group (if any). */
        public megEventGroup selectedGroup { get; set; }

        /** The selected event (if any). */
        public megEvent selectedEvent { get; set; }


        // Structures
        // ------------------------------------------------------------

        /** Tracking data for a server value. */
        private struct ServerValue
        {
            public float time;
            public float initial;
        }


        // Members
        // ------------------------------------------------------------

        /** Tracking data for server values. */
        private readonly Dictionary<string, ServerValue> _values = new Dictionary<string, ServerValue>();


        // Public Methods
        // ------------------------------------------------------------

        /** Play this event file. */
        public void Play()
        {
            Start();
        }

        /** Start this event file. */
        public void Start()
        {
            // Allow file to be restarted once complete.
            if (running && completed)
                Stop();

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

            // Register with event manager for updates.
            megEventManager.Instance.AddFile(this);
        }

        /** Update this event file as time passes. */
        public void Update(float t, float dt)
        {
            if (paused)
                return;

            if (running)
                time += dt;

            for (var i = 0; i < groups.Count; i++)
                groups[i].Update(time, dt);

            if (running && !completed)
                completed = groups.All(g => g.completed);
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

            // Stop each group.
            for (var i = 0; i < groups.Count; i++)
                groups[i].Stop();

            // Restore initial values for server data.
            ResetServerData();

            time = 0;
            running = false;

            // Stop receiving updates.
            megEventManager.Instance.RemoveFile(this);
        }

        /** Rewind the file to start time. */
        public void Rewind()
        {
            var wasRunning = running;
            var wasPaused = paused;
            var wasCompleted = completed;

            Stop();

            if (wasRunning)
                Start();
            if (wasPaused || wasCompleted)
                Pause();
        }

        /** Add an group of a given type. */
        public megEventGroup AddGroup()
        {
            var group = CreateGroup();
            groups.Add(group);

            // Start the group if file is running.
            if (running)
                group.Start();

            return group;
        }

        /** Insert an group of a given type after the given group. */
        public megEventGroup InsertGroup(megEventGroup insertAfter)
        {
            var group = CreateGroup();
            var insertIndex = groups.IndexOf(insertAfter);
            if (insertIndex >= 0)
                groups.Insert(insertIndex + 1, group);
            else
                groups.Add(group);

            // Start the group if file is running.
            if (running)
                group.Start();

            return group;
        }

        /** Create an group of a given type. */
        public megEventGroup CreateGroup()
            { return new megEventGroup(this); }

        /** Remove an group from the group. */
        public void RemoveGroup(megEventGroup group)
        {
            // Ensure the group has been stopped.
            group.Stop();

            // Remove group from the file.
            groups.Remove(group);
        }

        // Server values
        // ------------------------------------------------------------

        /** Set a server float value. */
        public void PostServerData(string key, float value)
        {
            // Record initial value if this is the first time we've set it.
            // This will be used to reset the value when file playback stops.
            if (!_values.ContainsKey(key))
                _values[key] = new ServerValue
                {
                    initial = serverUtils.GetServerData(key),
                    time = time
                };

            // Set the server data value.
            serverUtils.PostServerData(key, value);
        }

        /** Set a server string value. */
        public void PostServerData(string key, string value)
            { serverUtils.PostServerData(key, value); }

        /** Return a server value. */
        public float GetServerData(string key)
            { return serverUtils.GetServerData(key); }

        /** Reset all server values to initial settings. */
        private void ResetServerData()
        {
            foreach (var e in _values)
                serverUtils.PostServerData(e.Key, e.Value.initial);
            
            _values.Clear();
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
            for (var i = 0; i < groupsJson.Count; i++)
            {
                var group = new megEventGroup(this);
                groups.Add(group);
                group.Load(groupsJson[i]);

                if (running)
                    group.Start();
            }
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

            File.WriteAllText(path, text);
        }

    }

}