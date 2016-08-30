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
        MapCamera = 3,
        VesselMovement = 4,
        Popup = 5
    }


    /** Base class for a timed event that can have an effect on the simulation. */

    [System.Serializable]
    public abstract class megEvent
    {

        // Properties
        // ------------------------------------------------------------

        /** The type of event. */
        public readonly megEventType type;

        /** Group that this event belongs to. */
        public megEventGroup group { get { return _group; } }

        /** File that this event belongs to. */
        public megEventFile file { get { return _group != null ? _group.file : null; } }

        /** Optional trigger button object. */
        public GameObject trigger;

        /** Optional trigger button label. */
        public string triggerLabel;

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

        /** Whether event should be manually triggered by a button. */
        public bool hasTrigger { get { return !string.IsNullOrEmpty(triggerLabel); } }

        /** Whether event is selected. */
        public bool selected { get { return group != null && group.file.selectedEvent == this; } }

        /** Whether event is minimized. */
        public bool minimized { get { return !selected; } }

        /** Id for this event. */
        public virtual string name
            { get { return ToString(); } }


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

        /** Manually trigger this event. */
        public void Trigger()
        {
            // Check that the event can be manually triggered in principle.
            if (!hasTrigger)
                return;

            // Check if we can trigger the event right now.
            if (!file.playing || !group.running || group.paused)
                return;

            // Start up the event.
            running = true;
            completed = false;
            time = 0;
            TriggerExecute();
        }

        /** Execute trigger effect. */
        public virtual void TriggerExecute()
        {
            Start();
        }

        /** Execute this event's effect, regardless of timing. */
        public virtual void Execute()
        {
        }

        /** Update this event over time from an event group. */
        public void UpdateFromGroup(float t, float dt)
        {
            if (group.running)
            {
                if (!running && t >= triggerTime && !hasTrigger)
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

        /** String representation. */
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(triggerLabel))
                return triggerLabel;

            return type.ToString();
        }


        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public virtual JSONObject Save()
        {
            var json = new JSONObject();
            json.AddField("type", type.ToString());
            if (triggerTime > 0)
                json.AddField("triggerTime", triggerTime);
            if (completeTime > 0)
                json.AddField("completeTime", completeTime);
            if (hasTrigger)
                json.AddField("triggerLabel", triggerLabel);

            return json;
        }

        /** Load state from JSON. */
        public virtual void Load(JSONObject json)
        {
            json.GetField(ref triggerTime, "triggerTime");
            json.GetField(ref completeTime, "completeTime");
            json.GetField(ref triggerLabel, "triggerLabel");
        }


        // Server values
        // ------------------------------------------------------------

        /** Set a server value. */
        protected void PostServerData(string key, float value)
            { file.PostServerData(key, value); }

        /** Set a server value. */
        protected void PostServerData(string key, string value)
            { file.PostServerData(key, value); }

        /** Return a server value. */
        public float GetServerData(string key)
            { return file.GetServerData(key); }


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
