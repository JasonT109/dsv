using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Meg.EventSystem
{

    /** A group of timed events. */

    [System.Serializable]
    public class megEventGroup
    {

        // Properties
        // ------------------------------------------------------------

        /** ID of this event group. */
        public string id = "Group";

        /** Object used to trigger this group. */
        public GameObject trigger;

        /** The events managed by this group. */
        public List<megEvent> events = new List<megEvent>();

        /** TODO: Remove this once transition to megEvent is complete. */
        public megEventObject[] eventObjects;

        /** The file that this group belongs to. */
        public megEventFile file { get { return _file; } }
    
        /** Current local time for event group. */
        public float time { get; set; }

        /** Whether group is currently running. */
        public bool running { get; set; }

        /** Whether group is currently paused. */
        public bool paused { get; set; }

        /** Whether group is currently set to loop. */
        public bool looping { get; set; }

        /** Whether group is complete. */
        public bool completed { get; set; }

        /** Whether group is minimized. */
        public bool minimized { get; set; }

        /** Whether group is empty. */
        public bool empty { get { return events.Count == 0; } }

        /** Local time at which the group ends. */
        public float endTime { get { return empty ? 0 : events.Max(e => e.endTime); } }


        // Members
        // ------------------------------------------------------------

        /** The file that this group belongs to. */
        private readonly megEventFile _file;


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor. */
        public megEventGroup(megEventFile file = null)
        {
            _file = file;
        }


        // Public Methods
        // ------------------------------------------------------------

        /** Start this event group. */
        public void Start()
        {
            if (running)
                return;

            running = true;
            completed = false;
            time = 0;

            // Initialize event list if needed.
            if (events == null)
                events = new List<megEvent>();
            if (events.Count == 0 && eventObjects != null)
                events.AddRange(eventObjects);
        }

        /** Update this event group as time passes. */
        public void Update(float t, float dt)
        {
            if (paused)
                return;

            if (running && !completed)
                time += dt;

            for (var i = 0; i < events.Count; i++)
                events[i].UpdateFromGroup(time, dt);

            if (running && !completed)
            {
                var finished = events.All(e => e.completed);
                if (finished && !looping)
                    completed = true;
                else if (finished && looping)
                    Rewind();
            }

            if (completed && looping)
                Rewind();
        }

        /** Set group's paused state. */
        public void SetPaused(bool value)
        {
            paused = value;
        }

        /** Set group's looping state. */
        public void SetLooping(bool value)
        {
            looping = value;
        }

        /** Pause playback on the group. */
        public void Pause()
        {
            SetPaused(true);
        }

        /** Resume playback on the group. */
        public void Resume()
        {
            SetPaused(false);
        }

        /** Stop this event group. */
        public void Stop()
        {
            if (!running)
                return;

            running = false;

            // Stop events in reverse start time order.
            // This ensures server values are correctly reset.
            var ordered = events.OrderByDescending(e => e.triggerTime);
            foreach (var e in ordered)
                e.StopFromGroup();
        }

        /** Rewind the group to start time. */
        public void Rewind()
        {
            completed = false;
            time = 0;
            foreach (var e in events)
                e.RewindFromGroup();

            if (!_file.playing)
                Stop();
        }


        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public virtual JSONObject Save()
        {
            var json = new JSONObject();
            var eventsJson = new JSONObject(JSONObject.Type.ARRAY);
            var events = this.events;
            foreach (var e in events)
                eventsJson.Add(e.Save());

            json.AddField("id", id);
            json.AddField("paused", paused);
            json.AddField("events", eventsJson);
            json.AddField("looping", looping);

            return json;
        }

        /** Load state from JSON. */
        public virtual void Load(JSONObject json)
        {
            // Load in value events.
            json.GetField(ref id, "id");
            bool jsonPaused;
            if (json.GetField(out jsonPaused, "paused", false))
                paused = jsonPaused;

            bool jsonLooping;
            if (json.GetField(out jsonLooping, "looping", false))
                looping = jsonLooping;

            bool jsonMinimized;
            if (json.GetField(out jsonMinimized, "minimized", false))
                minimized = jsonMinimized;

            var eventsJson = json.GetField("events");
            events.Clear();
            for (var i = 0; i < eventsJson.Count; i++)
            {
                var s = eventsJson[i]["type"].str;
                var type = (megEventType) Enum.Parse(typeof(megEventType), s, true);
                events.Add(CreateEvent(type));
                events[i].Load(eventsJson[i]);
            }
        }


        // Private Methods
        // ------------------------------------------------------------

        /** Create an event of a given type. */
        private megEvent CreateEvent(megEventType type)
        {
            switch (type)
            {
                case megEventType.Value:
                    return new megEventValue(this);
                case megEventType.Physics:
                    return new megEventPhysics(this);
                case megEventType.Sonar:
                    return new megEventSonar(this);
                case megEventType.MapCamera:
                    return new megEventMapCamera(this);
                default:
                    return new megEventValue(this);
            }
        }

    }

}