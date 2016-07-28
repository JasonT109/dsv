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

        /** Current local time for event group. */
        public float time { get; set; }

        /** Whether group is currently running. */
        public bool running { get; set; }

        /** Whether file is currently paused. */
        public bool paused { get; set; }

        /** Whether group is complete. */
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
            // if (trigger)
            //    paused = !trigger.GetComponent<buttonControl>().active;

            if (paused)
                return;

            if (running)
            {
                // Update current time.
                time += dt;

                // Check if all events are complete.
                completed = events.All(e => e.completed);
            }

            // Update each event.
            for (var i = 0; i < events.Count; i++)
                events[i].UpdateFromGroup(this, time, dt);
        }

        /** Pause playback on the group. */
        public void Pause()
        {
            paused = true;
        }

        /** Resume playback on the group. */
        public void Resume()
        {
            paused = false;
        }

        /** Stop this event group. */
        public void Stop()
        {
            if (!running)
                return;

            time = 0;
            running = false;
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

            return json;
        }

        /** Load state from JSON. */
        public virtual void Load(JSONObject json)
        {
            // Load in value events.
            json.GetField(ref id, "id");
            paused = json.GetField("paused").b;
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
                    return new megEventValue();
                case megEventType.Physics:
                    return new megEventPhysics();
                case megEventType.Sonar:
                    return new megEventSonar();
                case megEventType.MapCamera:
                    return new megEventMapCamera();
                default:
                    return new megEventValue();
            }
        }

    }

}