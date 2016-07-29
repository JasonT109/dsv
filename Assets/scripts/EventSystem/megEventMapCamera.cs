using UnityEngine;
using System.Collections;
using System.Linq;

namespace Meg.EventSystem
{

    [System.Serializable]
    public class megEventMapCamera : megEvent
    {

        // Properties
        // ------------------------------------------------------------

        /** Id for this event. */
        public override string id { get { return eventName; } }

        public string eventName;
        public Vector3 toPosition;
        public Vector3 toOrientation;
        public float toZoom;
        public GameObject toObject;
        public bool goToObject;
        public bool goToPlayerVessel;
        public int priority;


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for a sonar event. */
        public megEventMapCamera(megEventGroup group = null) 
            : base(megEventType.MapCamera, group) { }


        // Load / Save
        // ------------------------------------------------------------

        /** Save movement state to JSON. */
        public override JSONObject Save()
        {
            var json = base.Save();
            json.AddField("eventName", eventName);
            json.AddField("toPosition", toPosition);
            json.AddField("toOrientation", toOrientation);
            json.AddField("toZoom", toZoom);
            // json.AddField("toObject", toObject);
            json.AddField("goToObject", goToObject);
            json.AddField("goToPlayerVessel", goToPlayerVessel);
            json.AddField("priority", priority);
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
            // json.GetField(ref toObject, "toObject");
            json.GetField(ref goToObject, "goToObject");
            json.GetField(ref goToPlayerVessel, "goToPlayerVessel");
            json.GetField(ref priority, "priority");
        }


        // Protected Methods
        // ------------------------------------------------------------

        /** Start this event. */
        protected override void Start()
        {
            Debug.Log("Map camera event: " + id);
            completed = true;
        }

        /** Update this event internally. */
        protected override void Update(float t, float dt)
        {
        }

        /** Stop this event. */
        protected override void Stop()
        {
        }


    }

}