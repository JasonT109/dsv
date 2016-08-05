using UnityEngine;
using System.Collections;
using System.Linq;
using Meg.Networking;

namespace Meg.EventSystem
{

    [System.Serializable]
    public class megEventMapCamera : megEvent
    {

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


        // Private Properties
        // ------------------------------------------------------------

        /** The camera event manager. */
        private megMapCameraEventManager Manager
            { get { return megEventManager.Instance.MapCamera; } }

        /** Whether camera event has a custom state applied. */
        private bool hasState
            { get { return toPosition.sqrMagnitude > 0 || toOrientation.sqrMagnitude > 0 || toZoom != 0; } }


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for a map camera event. */
        public megEventMapCamera(megEventGroup group = null) 
            : base(megEventType.MapCamera, group) { }

        /** Constructor for a map camera event, taking a custom state. */
        public megEventMapCamera(megMapCameraEventManager.State state)
            : base(megEventType.MapCamera)
        {
            SetState(state);
        }


        // Public Methods
        // ------------------------------------------------------------

        /** Execute this event's effect, regardless of timing. */
        public override void Execute()
        {
            base.Execute();

            if (hasState)
                file.PostMapCameraState(GetState());
            else
                file.PostMapCameraEvent(eventName);
        }

        /** Capture current map camera state. */
        public void Capture()
        {
            // Check that manager exists.
            if (!Manager)
                return;

            // Capture current camera state.
            var state = new megMapCameraEventManager.State();
            Manager.Capture(ref state);

            // Apply state to the event.
            SetState(state);
        }

        /** String representation. */
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(triggerLabel))
                return triggerLabel;

            if (!string.IsNullOrEmpty(eventName))
                return string.Format("Map: {0}", eventName);

            return string.Format("Map: {0}", toPosition);
        }

        /** Apply a custom camera state to this event. */
        public void SetState(megMapCameraEventManager.State state)
        {
            eventName = "";
            completeTime = state.completeTime;
            toPosition = state.toPosition;
            toOrientation = state.toOrientation;
            toZoom = state.toZoom;
            toObject = null;
            goToObject = false;
            goToPlayerVessel = false;
            triggeredByServer = false;
            serverTriggerID = 0;
            priority = 0;
        }

        /** Return event as a map camera state. */
        public megMapCameraEventManager.State GetState()
        {
            return new megMapCameraEventManager.State
            {
                toOrientation = toOrientation,
                toPosition = toPosition,
                toZoom = toZoom,
                completeTime = completeTime
            };
        }


        // Load / Save
        // ------------------------------------------------------------

        /** Save movement state to JSON. */
        public override JSONObject Save()
        {
            var json = base.Save();

            if (!string.IsNullOrEmpty(eventName))
                json.AddField("eventName", eventName);
            else
            {
                json.AddField("toPosition", toPosition);
                json.AddField("toOrientation", toOrientation);
                json.AddField("toZoom", toZoom);
            }

            return json;
        }

        /** Load movement state from JSON. */
        public override void Load(JSONObject json)
        {
            base.Load(json);

            json.GetField(ref eventName, "eventName");
            json.GetField(ref toPosition, "toPosition");
            json.GetField(ref toOrientation, "toOrientation");
            json.GetField(ref toZoom, "toZoom");
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
