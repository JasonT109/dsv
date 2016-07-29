using UnityEngine;
using Meg.Networking;

namespace Meg.EventSystem
{

    /** Base class for an event. */
    [System.Serializable]
    public class megEventObject : megEvent
    {

        // Properties
        // ------------------------------------------------------------

        public Vector3 physicsDirection;
        public float phyicsMagnitude;
        public string serverParam;
        public float serverValue;

        /** Id for this event. */
        public override string id { get { return serverParam; } }

        public bool physicsEvent
        { get { return type == megEventType.Physics; } }

        public bool valueEvent
        { get { return type == megEventType.Value; } }


        // Members
        // ------------------------------------------------------------

        /** Initial value, used when interpolating. */
        private float _initialValue;


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for an event. */
        public megEventObject(megEventType type, megEventGroup group = null) 
            : base(type, group) { }


        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public override JSONObject Save()
        {
            var json = base.Save();
            json.AddField("physicsDirection", physicsDirection);
            json.AddField("phyicsMagnitude", phyicsMagnitude);
            json.AddField("serverParam", serverParam);
            json.AddField("serverValue", serverValue);
            return json;
        }

        /** Load state from JSON. */
        public override void Load(JSONObject json)
        {
            base.Load(json);
            json.GetField(ref physicsDirection, "physicsDirection");
            json.GetField(ref phyicsMagnitude, "phyicsMagnitude");
            json.GetField(ref serverParam, "serverParam");
            json.GetField(ref serverValue, "serverValue");
        }


        // Protected Methods
        // ------------------------------------------------------------

        /** Start this event. */
        protected override void Start()
        {
            if (physicsEvent)
            {
                Debug.Log("Physics event.");
                serverUtils.ServerData.RpcImpact(physicsDirection*phyicsMagnitude);
                completed = true;
            }
            else if (valueEvent)
            {
                // Get initial value.
                _initialValue = serverUtils.GetServerData(serverParam);

                // Apply final value immediately if needed.
                if (completeTime <= 0)
                {
                    serverUtils.SetServerData(serverParam, serverValue);
                    completed = true;
                }
            }
        }

        /** Update this event internally. */
        protected override void Update(float t, float dt)
        {
            if (valueEvent)
            {
                // Interpolate towards final value over time.
                var value = Mathf.Lerp(_initialValue, serverValue, timeFraction);
                serverUtils.SetServerData(serverParam, value);

                // Check if final value has been reached.
                if (timeFraction >= 1)
                    completed = true;
            }
        }

        /** Stop this event. */
        protected override void Stop()
        {
        }

    }

}