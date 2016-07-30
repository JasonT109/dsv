using System;
using System.Collections;
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

        /** Whether file can load more groups. */
        public bool canLoad { get { return true; } } // !playing; } }

        /** Whether file can be cleared. */
        public bool canClear { get { return !empty && !playing; } }

        /** The selected event (if any). */
        public megEvent selectedEvent { get; set; }


        // Members
        // ------------------------------------------------------------

        /** Loaded files. */
        private HashSet<string> _loadedFiles = new HashSet<string>();


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

            // Stop all events in the file in reverse time order.
            // This ensures server values are correctly reset.
            var ordered = events.OrderByDescending(e => e.triggerTime);
            foreach (var e in ordered)
                e.StopFromFile();

            // Stop each group.
            for (var i = 0; i < groups.Count; i++)
                groups[i].Stop();

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
            if (_loadedFiles.Contains(path))
                return;

            var text = File.ReadAllText(path);
            var json = new JSONObject(text);
            Load(json);

            _loadedFiles.Add(path);
        }

    }

}