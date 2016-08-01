using UnityEngine;
using System.Collections;
using System.Linq;
using Meg.Networking;

namespace Meg.EventSystem
{

    [System.Serializable]
    public class megEventMapCamera : megEvent
    {

        // Constants
        // ------------------------------------------------------------

        /** Duration for setting map camera event syncvars. */
        private const float EventBroadcastDuration = 1.0f;


        // Properties
        // ------------------------------------------------------------

        public string eventName = "";
        public Vector3 toPosition;
        public Vector3 toOrientation;
        public float toZoom;
        public GameObject toObject;
        public bool goToObject;
        public bool goToPlayerVessel;
        public bool triggeredByServer;
        public int serverTriggerID;
        public int priority;

        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for a sonar event. */
        public megEventMapCamera(megEventGroup group = null) 
            : base(megEventType.MapCamera, group) { }


        // Public Methods
        // ------------------------------------------------------------

        /** Capture current map camera state. */
        public void Capture()
        {
            // TODO: Implement.
        }

        /** String representation. */
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(triggerLabel))
                return triggerLabel;

            if (!string.IsNullOrEmpty(eventName))
                return string.Format("Map: {0}", eventName);

            return base.ToString();
        }


        // Load / Save
        // ------------------------------------------------------------

        /** Save movement state to JSON. */
        public override JSONObject Save()
        {
            var json = base.Save();
            json.AddField("eventName", eventName);
            return json;
        }

        /** Load movement state from JSON. */
        public override void Load(JSONObject json)
        {
            base.Load(json);
            json.GetField(ref eventName, "eventName");
        }


        // Protected Methods
        // ------------------------------------------------------------

        /** Start this event. */
        protected override void Start()
        {
            TriggerMapCameraEvent(eventName);
        }

        /** Update this event internally. */
        protected override void Update(float t, float dt)
        {
            // Allow time for all clients to react to the map event syncvar.
            if (time < EventBroadcastDuration)
                return;

            ResetMapCameraEvent();

            // Check if final value has been reached.
            if (timeFraction >= 1)
                completed = true;
        }

        /** Rewind this event. */
        protected override void Rewind()
        {
            ResetMapCameraEvent();
        }

        /** Stop this event. */
        protected override void Stop()
        {
            ResetMapCameraEvent();
        }


        // Private Methods
        // ------------------------------------------------------------

        /** Trigger a map camera event via syncvars. */
        private void TriggerMapCameraEvent(string name)
        {
            // TODO: Turn this into an RPC.
            Debug.Log("Map camera event: " + this.name);
            PostServerData("mapEventName", eventName);
            PostServerData("initiateMapEvent", 1.0f);
        }

        /** Reset map camera event syncvars. */
        private void ResetMapCameraEvent()
        {
            PostServerData("mapEventName", "");
            PostServerData("initiateMapEvent", 0);
        }

    }

}