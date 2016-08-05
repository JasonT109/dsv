using UnityEngine;
using System.Collections;
using System.Linq;
using Meg.Networking;

namespace Meg.EventSystem
{

    [System.Serializable]
    public class megEventSonar : megEvent
    {

        // Properties
        // ------------------------------------------------------------

        public string eventName = "";
        public Vector3[] waypoints = { };
        public bool destroyOnEnd = true;

        /** Whether any sonar waypoints are defined. */
        public bool hasWaypoints { get { return waypoints != null && waypoints.Length > 0; } }


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for a sonar event. */
        public megEventSonar(megEventGroup group = null) 
            : base(megEventType.Sonar, group) { }


        // Public Methods
        // ------------------------------------------------------------

        /** Add a waypoint. */
        public void AddWaypoint(Vector3 p)
        {
            var list = waypoints.ToList();
            list.Add(p);
            waypoints = list.ToArray();
        }

        /** Remove the last waypoint. */
        public void RemoveLastWaypoint()
        {
            var n = waypoints.Length;
            if (n <= 0)
                return;

            var list = waypoints.ToList();
            list.RemoveAt(n - 1);
            waypoints = list.ToArray();
        }

        /** Remove all waypoints. */
        public void RemoveAllWaypoints()
        {
            waypoints = new Vector3[] { };
        }


        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public override JSONObject Save()
        {
            var json = base.Save();
            json.AddField("eventName", eventName);
            json.AddField("destroyOnEnd", destroyOnEnd);

            if (waypoints != null && waypoints.Length > 0)
            {
                var waypointsJson = new JSONObject(JSONObject.Type.ARRAY);
                foreach (var w in waypoints)
                    waypointsJson.Add(w);

                json.AddField("waypoints", waypointsJson);
            }

            return json;
        }

        /** Load movement state from JSON. */
        public override void Load(JSONObject json)
        {
            base.Load(json);
            json.GetField(ref eventName, "eventName");
            json.GetField(ref destroyOnEnd, "destroyOnEnd");

            var waypointsJson = json.GetField("waypoints");
            if (waypointsJson != null)
            {
                var n = waypointsJson.Count;
                waypoints = new Vector3[n];
                for (var i = 0; i < n; i++)
                {
                    var w = waypointsJson[i];
                    waypoints[i] = new Vector3(w[0].f, w[1].f, w[2].f);
                }
            }
        }

        /** String representation. */
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(triggerLabel))
                return triggerLabel;

            if (!string.IsNullOrEmpty(eventName))
                return string.Format("Sonar: {0}", eventName);

            return base.ToString();
        }


        // Protected Methods
        // ------------------------------------------------------------

        /** Start this event. */
        protected override void Start()
        {
            file.PostSonarEvent(this);
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
            file.PostSonarClear();
        }

    }

}