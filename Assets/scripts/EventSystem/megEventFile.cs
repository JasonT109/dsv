using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Meg.Networking;
using Meg.Scene;

namespace Meg.EventSystem
{

    /** 
     * A file containing groups of timed events. 
     * 
     * An event file can 'played back' somewhat like a video.  Each Group in the file has its own timeline,
     * and plays back a collection of Events at the appropriate start time ('triggerTime'). Events optionally
     * have a duration ('completeTime'), which affects how long they take to achieve the desired effect
     * (e.g. animating a server parameter to a specified value.)
     * 
     * When playback begins, the file is responsible for capturing the initial state of the world.
     * It does this by querying various managers for state data and remembering those value.  When playback
     * is Stopped, the file then asks each manager to reset to the stored initial state, and also asks the
     * server to reset modified parameters back to their starting values.
     * 
     * The interface for editing event files centers around debugEventFileUi - see that file for more info.
     * 
     */

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
        public bool canPlay { get { return true; } }

        /** Whether file can be rewound. */
        public bool canRewind { get { return running && !playing; } }

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

        /** When playback last started. */
        public DateTime startedTimestamp;

        /** When playback last stopped. */
        public DateTime stoppedTimestamp;


        // Events
        // ------------------------------------------------------------

        /** General signature for file events. */
        public delegate void megEventFileHandler(megEventFile file);

        /** Signature for file group events. */
        public delegate void megEventFileGroupHandler(megEventFile file, megEventGroup group);

        /** Event fired when file is loaded. */
        public megEventFileHandler Loaded;

        /** Event fired when file is saved. */
        public megEventFileHandler SavedToFile;

        /** Event fired when file is cleared. */
        public megEventFileHandler Cleared;

        /** Event fired when a group is added to the file. */
        public megEventFileGroupHandler GroupAdded;

        /** Event fired when a group is removed from the file. */
        public megEventFileGroupHandler GroupRemoved;


        // Structures
        // ------------------------------------------------------------

        /** Tracking data for a triggered event. */
        private struct EventRecord<T>
        {
            public float time;
            public T value;
        }


        // Members
        // ------------------------------------------------------------

        /** Tracking data for server values. */
        private readonly Dictionary<string, EventRecord<float>> _values = new Dictionary<string, EventRecord<float>>();

        /** Triggered vessel movements. */
        private readonly Dictionary<int, EventRecord<JSONObject>> _movements = new Dictionary<int, EventRecord<JSONObject>>();

        /** Whether sonar events have been triggered. */
        private bool _sonarEventsTriggered;

        /** Whether initial camera state is known .*/
        private bool _cameraEventsTriggered;

        /** Whether vessel movements have been altered. */
        private bool _movementEventsTriggered;

        /** Initial camera state. */
        private megMapCameraEventManager.State _initialCamera;

        /** Whether initial camera state is valid. */
        private bool _initialCameraValid;


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

            // Check if file is already running.
            if (running)
                return;

            // Capture initial state.
            CaptureServerState();

            // Update file state to running.
            running = true;
            completed = false;
            paused = false;
            time = 0;

            // Initialize event groups.
            if (groups == null)
                groups = new List<megEventGroup>();

            for (var i = 0; i < groups.Count; i++)
                groups[i].Start();

            // Register with event manager for updates.
            megEventManager.Instance.StartUpdating(this);

            // Remember when playback started.
            startedTimestamp = DateTime.UtcNow;
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
            ResetServerState();

            time = 0;
            running = false;

            // Stop receiving updates.
            megEventManager.Instance.StopUpdating(this);

            // Remember when playback stopped.
            stoppedTimestamp = DateTime.UtcNow;
        }

        /** Rewind the file to start time. */
        public void Rewind()
        {
            if (!running)
                return;

            // Stop playback if needed.
            if (paused || completed)
                Stop();
            else
                time = 0;

            // Rewind each group.
            for (var i = 0; i < groups.Count; i++)
                groups[i].Rewind();
        }

        /** Add an group of a given type. */
        public megEventGroup AddGroup()
        {
            var group = CreateGroup();
            groups.Add(group);

            // Start the group if file is running.
            if (running)
                group.Start();

            if (GroupAdded != null)
                GroupAdded(this, group);

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

            if (GroupAdded != null)
                GroupAdded(this, group);

            return group;
        }

        /** Create an group of a given type. */
        public megEventGroup CreateGroup()
            { return new megEventGroup(this); }

        /** Remove an group from the group. */
        public void RemoveGroup(megEventGroup group)
        {
            group.Stop();
            groups.Remove(group);

            if (selectedGroup == group)
                selectedGroup = null;
            if (selectedEvent != null && selectedEvent.group == group)
                selectedEvent = null;

            if (GroupRemoved != null)
                GroupRemoved(this, group);
        }

        /** Clear the file of all groups. */
        public void Clear()
        {
            Stop();
            groups.Clear();

            selectedGroup = null;
            selectedEvent = null;

            if (Cleared != null)
                Cleared(this);
        }


        // Server State
        // ------------------------------------------------------------

        /** Set a server float value. */
        public void PostServerData(string key, float value)
        {
            // Check that key is valid.
            if (string.IsNullOrEmpty(key))
                return;

            // Force key to lowercase to avoid issues with varying capitalization.
            key = key.ToLower();

            // Record initial value if this is the first time we've set it.
            // This will be used to reset the value when file playback stops.
            if (!_values.ContainsKey(key))
                _values[key] = new EventRecord<float>
                    { time = time, value = serverUtils.GetServerData(key) };

            // Set the server data value.
            serverUtils.PostServerData(key, value);
        }

        /** Return a server value. */
        public float GetServerData(string key)
            { return serverUtils.GetServerData(key); }

        /** Set a server string value. */
        public void PostServerData(string key, string value)
            { serverUtils.PostServerData(key, value); }

        /** Post vessel movement state to the server (works on both clients and host). */
        public void PostVesselMovementState(int vessel, JSONObject json)
        {
            // Record initial value if this is the first time we've set it.
            if (!_movements.ContainsKey(vessel))
                _movements[vessel] = new EventRecord<JSONObject>
                    { time = time, value = serverUtils.VesselMovements.SaveVessel(vessel) };

            _movementEventsTriggered = true;
            serverUtils.PostVesselMovementState(json);
        }

        /** Post a sonar event to the server. */
        public void PostSonarEvent(megEventSonar sonar)
        {
            _sonarEventsTriggered = true;
            serverUtils.PostSonarEvent(sonar);
        }

        /** Post a sonar clear to the server. */
        public void PostSonarClear()
            { serverUtils.PostSonarClear(); }

        /** Post a custom camera event by name. */
        public void PostMapCameraEvent(string eventName)
        {
            _cameraEventsTriggered = true;
            serverUtils.PostMapCameraEvent(eventName);
        }

        /** Post a custom camera event by supplying the target state. */
        public void PostMapCameraState(megMapCameraEventManager.State state)
        {
            _cameraEventsTriggered = true;
            serverUtils.PostMapCameraState(state);
        }

        
        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public virtual JSONObject Save()
        {
            var json = new JSONObject();
            json.AddField("metadata", SaveMetadata());

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

        /** Save event file metadata to JSON. */
        private JSONObject SaveMetadata()
        {
            var json = new JSONObject();
            json.AddField("utc", string.Format("{0:dd/MM/yy hh:mm:ss.f}", DateTime.UtcNow));
            json.AddField("startedTimestamp", string.Format("{0:dd/MM/yy hh:mm:ss.f}", startedTimestamp));
            json.AddField("stoppedTimestamp", string.Format("{0:dd/MM/yy hh:mm:ss.f}", stoppedTimestamp));
            return json;
        }


        // Private Methods
        // ------------------------------------------------------------

        /** Capture initial server state. */
        private void CaptureServerState()
        {
            // Save out initial scene state to file.
            if (megSceneFile.AutoSaveEnabled())
                megSceneFile.AutoSave("Start");

            // Capture initial camera state.
            _initialCameraValid = MapCamera && MapCamera.Capture(ref _initialCamera);

            // Capture initial vessel states.
            serverUtils.VesselMovements.CaptureInitialState();
            serverUtils.VesselData.CaptureInitialState();
        }

        /** Reset server state to initial settings. */
        private void ResetServerState()
        {
            // Save out final scene state to file.
            if (megSceneFile.AutoSaveEnabled())
                megSceneFile.AutoSave("Stop");
    
            // Reset data values from events.
            foreach (var e in _values)
                serverUtils.PostServerData(e.Key, e.Value.value);

            if (_movementEventsTriggered)
                foreach (var e in _movements)
                    serverUtils.PostVesselMovementState(e.Value.value);

            if (_sonarEventsTriggered)
                serverUtils.PostSonarClear();

            if (_cameraEventsTriggered && _initialCameraValid)
                serverUtils.PostMapCameraState(_initialCamera);

            // Reset vessels to their original state.
            // Only do this on the server, though.
            if (serverUtils.IsServer())
            {
                serverUtils.VesselMovements.ResetToInitialState();
                serverUtils.VesselData.ResetToInitialState();
            }

            // Clear all tracking data.
            _values.Clear();
            _movements.Clear();
            _sonarEventsTriggered = false;
            _cameraEventsTriggered = false;
            _movementEventsTriggered = false;
        }

        /** The camera event manager. */
        private megMapCameraEventManager MapCamera
            { get { return megEventManager.Instance.MapCamera; } }

    }
}
