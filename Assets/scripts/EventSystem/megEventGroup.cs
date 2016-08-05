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

        /** Whether group should pause when it completes. */
        public bool pauseOnComplete { get; set; }

        /** Whether group is minimized. */
        public bool minimized { get; set; }

        /** Whether group's timeline is hidden. */
        public bool hideTimeline { get; set; }

        /** Whether group's trigger buttons are hidden. */
        public bool hideTriggers { get; set; }

        /** Whether group can be looped. */
        public bool canLoop { get; set; }

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
        public void Start(float t = 0)
        {
            if (running)
                return;

            time = t;
            running = true;
            completed = false;

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

            if (completed && pauseOnComplete)
                paused = true;
            if (completed && (looping || pauseOnComplete))
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
            foreach (var e in events)
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

        /** Add an event of a given type. */
        public megEvent AddEvent(megEventType type)
        {
            var e = CreateEvent(type);
            events.Add(e);
            return e;
        }

        /** Insert an event of a given type after the given event. */
        public megEvent InsertEvent(megEventType type, megEvent insertAfter)
        {
            var e = CreateEvent(type);
            var insertIndex = events.IndexOf(insertAfter);
            if (insertIndex >= 0)
                events.Insert(insertIndex + 1, e);
            else
                events.Add(e);

            return e;
        }

        /** Create an event of a given type. */
        public megEvent CreateEvent(megEventType type)
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
                case megEventType.VesselMovement:
                    return new megEventVesselMovement(this);
                default:
                    return new megEventValue(this);
            }
        }

        /** Remove an event from the group. */
        public void RemoveEvent(megEvent e)
        {
            events.Remove(e);
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

            if (paused)
                json.AddField("paused", paused);
            if (looping)
                json.AddField("looping", looping);
            if (canLoop)
                json.AddField("canLoop", canLoop);
            if (minimized)
                json.AddField("minimized", minimized);
            if (pauseOnComplete)
                json.AddField("pauseOnComplete", pauseOnComplete);
            if (hideTimeline)
                json.AddField("hideTimeline", hideTimeline);
            if (hideTriggers)
                json.AddField("hideTriggers", hideTriggers);

            json.AddField("events", eventsJson);

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
            bool jsonCanLoop;
            if (json.GetField(out jsonCanLoop, "canLoop", false))
                canLoop = jsonCanLoop;
            bool jsonMinimized;
            if (json.GetField(out jsonMinimized, "minimized", false))
                minimized = jsonMinimized;
            bool jsonPauseOnComplete;
            if (json.GetField(out jsonPauseOnComplete, "pauseOnComplete", false))
                pauseOnComplete = jsonPauseOnComplete;
            bool jsonHideTimeline;
            if (json.GetField(out jsonHideTimeline, "hideTimeline", false))
                hideTimeline = jsonHideTimeline;
            bool jsonHideTriggers;
            if (json.GetField(out jsonHideTriggers, "hideTriggers", false))
                hideTriggers = jsonHideTriggers;

            var eventsJson = json.GetField("events");
            events.Clear();
            for (var i = 0; i < eventsJson.Count; i++)
            {
                var s = eventsJson[i]["type"].str;
                var type = (megEventType) Enum.Parse(typeof(megEventType), s, true);
                var e = AddEvent(type);
                e.Load(eventsJson[i]);
            }
        }

    }

}