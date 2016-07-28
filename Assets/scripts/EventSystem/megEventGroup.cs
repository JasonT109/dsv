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
            if (!running)
            {
                // Trigger the group when a button is pressed.
                if (trigger && trigger.GetComponent<buttonControl>().active)
                    Start();
            }
            else if (running)
            {
                // Update current time.
                time += dt;

                // Check if all events are complete.
                completed = events.All(e => e.completed);

                // Check if we should stop the group.
                if (trigger && !trigger.GetComponent<buttonControl>().active)
                    Stop();
            }

            // Update each event.
            for (var i = 0; i < events.Count; i++)
                events[i].UpdateFromGroup(this, time, dt);
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

        /** Save movement state to JSON. */
        public virtual JSONObject Save()
        {
            var json = new JSONObject();
            var eventsJson = new JSONObject(JSONObject.Type.ARRAY);
            var events = this.events;
            foreach (var e in events)
                eventsJson.Add(e.Save());

            json.AddField("Events", eventsJson);

            return json;
        }

        /** Load movement state from JSON. */
        public virtual void Load(JSONObject json)
        {
            // Load in value events.
            var eventsJson = json.GetField("Events");
            events.Clear();
            for (var i = 0; i < eventsJson.Count; i++)
            {
                var type = (megEventType) eventsJson[i]["type"].i;
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