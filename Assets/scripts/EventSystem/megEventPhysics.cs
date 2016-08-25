using Meg.Networking;
using UnityEngine;

namespace Meg.EventSystem
{

    /** An event that applies physics to the player vessel. */
    [System.Serializable]
    public class megEventPhysics : megEvent
    {

        // Properties
        // ------------------------------------------------------------

        /** Direction of physics impact force. */
        public Vector3 physicsDirection;

        /** Magnitude of physics impact force. */
        public float physicsMagnitude;


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for an event. */
        public megEventPhysics(megEventGroup group = null) 
            : base(megEventType.Physics, group) { }


        // Public Methods
        // ------------------------------------------------------------

        /** Execute this event's effect, regardless of timing. */
        public override void Execute()
        {
            base.Execute();
            serverUtils.PostImpact(physicsDirection * physicsMagnitude);
        }

        /** String representation. */
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(triggerLabel))
                return triggerLabel;

            return string.Format("Impact: d={0}, m={1:N1}", physicsDirection, physicsMagnitude);
        }


        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public override JSONObject Save()
        {
            var json = base.Save();
            json.AddField("physicsDirection", physicsDirection);
            json.AddField("physicsMagnitude", physicsMagnitude);
            return json;
        }

        /** Load state from JSON. */
        public override void Load(JSONObject json)
        {
            base.Load(json);
            json.GetField(ref physicsDirection, "physicsDirection");
            json.GetField(ref physicsMagnitude, "physicsMagnitude");
        }


        // Protected Methods
        // ------------------------------------------------------------

        /** Start this event. */
        protected override void Start()
        {
            Execute();
        }

        /** Update this event internally. */
        protected override void Update(float t, float dt)
        {
            // Check if final value has been reached.
            if (timeFraction >= 1)
                completed = true;
        }

        /** Rewind this event. */
        protected override void Rewind()
        {
        }

        /** Stop this event. */
        protected override void Stop()
        {
        }


    }

}
