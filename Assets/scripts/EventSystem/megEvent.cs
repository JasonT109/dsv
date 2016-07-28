using UnityEngine;
using System.Collections;
using System.Linq;

namespace Meg.EventSystem
{

    /** Possible types of timed event. */

    public enum megEventType
    {
        Value = 0,
        Physics = 1,
        Sonar = 2,
        MapCamera = 3
    }


    /** Base class for a timed event that can have an effect on the simulation. */

    [System.Serializable]
    public abstract class megEvent
    {

        // Properties
        // ------------------------------------------------------------

        /** The type of event. */
        public readonly megEventType type;

        /** Id for this event. */
        public abstract string id { get; }

        /** Optional trigger button object. */
        public GameObject trigger;

        /** Time at which to trigger the event. */
        public float triggerTime;

        /** Duration of the event. */
        public float completeTime;

        /** Current local time for event. */
        public float time { get; set; }

        /** Fraction of local time within [trigger, complete] interval. */
        public float timeFraction
            { get { return completeTime > 0 ? Mathf.Clamp01(time - triggerTime) / completeTime : 1; } }

        /** Whether the event is currently running. */
        public bool running { get; set; }

        /** Whether the event has completed. */
        public bool completed { get; set; }


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for an event. */
        protected megEvent(megEventType type)
        {
            this.type = type;
        }


        // Public Methods
        // ------------------------------------------------------------

        /** Update this event over time from an event group. */
        public void UpdateFromGroup(megEventGroup group, float t, float dt)
        {
            if (group.running)
            {
                if (!running && t >= triggerTime)
                {
                    running = true;
                    completed = false;
                    time = 0;
                    Start();
                }
                else if (running)
                    time += dt;

                if (!completed)
                    Update(time, dt);
            }
            else if (running)
            {
                running = false;
                Stop();
            }
        }


        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public virtual JSONObject Save()
        {
            var json = new JSONObject();
            json.AddField("type", type.ToString());
            json.AddField("triggerTime", triggerTime);
            json.AddField("completeTime", completeTime);
            return json;
        }

        /** Load state from JSON. */
        public virtual void Load(JSONObject json)
        {
            json.GetField(ref triggerTime, "triggerTime");
            json.GetField(ref completeTime, "completeTime");
        }


        // Protected Methods
        // ------------------------------------------------------------

        /** Start this event. */
        protected abstract void Start();

        /** Update this event internally. */
        protected abstract void Update(float t, float dt);

        /** Stop this event. */
        protected abstract void Stop();


    }

}