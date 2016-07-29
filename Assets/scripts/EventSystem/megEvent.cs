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

        /** Group that this event belongs to. */
        public megEventGroup group { get { return _group; } }

        /** File that this event belongs to. */
        public megEventFile file { get { return _group != null ? _group.file : null; } }

        /** Optional trigger button object. */
        public GameObject trigger;

        /** Time at which to trigger the event. */
        public float triggerTime;

        /** Duration of the event. */
        public float completeTime;

        /** Time at which the event ends. */
        public float endTime { get { return triggerTime + Mathf.Max(0, completeTime); } }

        /** Current local time for event. */
        public float time { get; set; }

        /** Fraction of local time within [trigger, complete] interval. */
        public float timeFraction
            { get { return completeTime > 0 ? Mathf.Clamp01(time / completeTime) : 1; } }

        /** Whether the event is currently running. */
        public bool running { get; set; }

        /** Whether the event has completed. */
        public bool completed { get; set; }

        
        // Members
        // ------------------------------------------------------------

        /** The group that this event belongs to. */
        private readonly megEventGroup _group;


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for an event. */
        protected megEvent(megEventType type, megEventGroup group = null)
        {
            this.type = type;
            _group = group;
        }


        // Public Methods
        // ------------------------------------------------------------

        /** Update this event over time from an event group. */
        public void UpdateFromGroup(float t, float dt)
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
                else if (running && !completed)
                    time += dt;

                if (running && !completed)
                    Update(time, dt);
            }
            else if (running)
            {
                running = false;
                Stop();
            }
        }

        /** Stop event from an event group. */
        public void StopFromGroup()
        {
            if (!running)
                return;

            running = false;
            completed = false;
            time = 0;

            Stop();
        }

        /** Rewind event from an event group. */
        public void RewindFromGroup()
        {
            running = false;
            completed = false;
            time = 0;

            Rewind();
        }

        /** Stop event from an event file. */
        public void StopFromFile()
        {
            if (!running)
                return;

            running = false;
            completed = false;
            time = 0;

            Stop();
        }

        /** String representation. */
        public override string ToString()
        {
            return id;
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

        /** Reset this event. */
        protected abstract void Rewind();


    }

}