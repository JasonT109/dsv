using UnityEngine;
using System.Collections;
using System.Linq;

namespace Meg.EventSystem
{

    [System.Serializable]
    public class megEventSonar : megEvent
    {

        // Properties
        // ------------------------------------------------------------

        /** Id for this event. */
        public override string id { get { return "Sonar"; } }

        public Vector3[] waypoints;
        public bool destroyOnEnd;


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for a sonar event. */
        public megEventSonar(megEventGroup group = null) 
            : base(megEventType.Sonar, group) { }


        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public override JSONObject Save()
        {
            var json = base.Save();
            json.GetField(ref triggerTime, "triggerTime");
            var waypointsJson = new JSONObject(JSONObject.Type.ARRAY);
            foreach (var w in waypoints)
                waypointsJson.Add(w);

            json.AddField("waypoints", waypointsJson);
            json.AddField("destroyOnEnd", destroyOnEnd);

            return json;
        }

        /** Load movement state from JSON. */
        public override void Load(JSONObject json)
        {
            base.Load(json);
            var waypointsJson = json.GetField("waypoints");
            waypoints = new Vector3[waypointsJson.Count];
            for (var i = 0; i < waypointsJson.Count; i++)
            {
                var w = waypointsJson[i];
                waypoints[i] = new Vector3(w[0].f, w[1].f, w[2].f);
            }

            json.GetField(ref destroyOnEnd, "destroyOnEnd");
        }


        // Protected Methods
        // ------------------------------------------------------------

        /** Start this event. */
        protected override void Start()
        {
            Debug.Log("Sonar event.");
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