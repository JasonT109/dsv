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

        public const float DefaultCompleteTime = 2f;


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
        public bool is2d;

        public bool is3d
            { get { return !is2d; } }


        // Private Properties
        // ------------------------------------------------------------

        /** The 3d camera event manager. */
        private megMapCameraEventManager Camera3d
            { get { return megMapCameraEventManager.Instance; } }

        /** The 2d camera event manager. */
        private Map2d Camera2d
            { get { return Map2d.Instance; } }


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

        public void Capture3d()
        {
            // Check that manager exists.
            if (!Camera3d)
                return;

            // Capture current camera state.
            var state = new megMapCameraEventManager.State { completeTime = DefaultCompleteTime };
            Camera3d.Capture(ref state);

            // Apply state to the event.
            SetState(state);
        }

        public void Capture2d()
        {
            // Check that manager exists.
            if (!Camera2d)
                return;

            // Capture current camera state.
            var state = new megMapCameraEventManager.State { completeTime = DefaultCompleteTime };
            Camera2d.Capture(ref state);

            // Apply state to the event.
            SetState(state);
        }

        /** String representation. */
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(triggerLabel))
                return triggerLabel;

            if (!string.IsNullOrEmpty(eventName))
                return string.Format("3D Map: {0}", eventName);

            return string.Format("{0} Map: {1}", is2d ? "2D" : "3D", toPosition);
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
            is2d = state.is2d;
        }

        /** Return event as a map camera state. */
        public megMapCameraEventManager.State GetState()
        {
            return new megMapCameraEventManager.State
            {
                toOrientation = toOrientation,
                toPosition = toPosition,
                toZoom = toZoom,
                completeTime = completeTime,
                is2d = is2d
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
                json.AddField("is2d", is2d);
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
            json.GetField(ref is2d, "is2d");
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
