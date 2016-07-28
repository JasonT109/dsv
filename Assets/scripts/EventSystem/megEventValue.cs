using UnityEngine;
using Meg.Networking;

namespace Meg.EventSystem
{

    /** An event that manipulates a server data value. */
    [System.Serializable]
    public class megEventValue : megEvent
    {

        // Properties
        // ------------------------------------------------------------

        /** The server data value to manipulate. */
        public string serverParam;

        /** Value to apply to server data. */
        public float serverValue;

        /** Id for this event. */
        public override string id { get { return serverParam; } }


        // Members
        // ------------------------------------------------------------

        /** Initial value, used when interpolating. */
        private float _initialValue;


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for an event. */
        public megEventValue() : base(megEventType.Value) { }


        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public override JSONObject Save()
        {
            var json = base.Save();
            json.AddField("serverParam", serverParam);
            json.AddField("serverValue", serverValue);
            return json;
        }

        /** Load state from JSON. */
        public override void Load(JSONObject json)
        {
            base.Load(json);
            json.GetField(ref serverParam, "serverParam");
            json.GetField(ref serverValue, "serverValue");
        }


        // Protected Methods
        // ------------------------------------------------------------

        /** Start this event. */
        protected override void Start()
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

        /** Update this event internally. */
        protected override void Update(float t, float dt)
        {
            // Interpolate towards final value over time.
            var value = Mathf.Lerp(_initialValue, serverValue, timeFraction);
            serverUtils.SetServerData(serverParam, value);

            // Check if final value has been reached.
            if (timeFraction >= 1)
                completed = true;
        }

        /** Stop this event. */
        protected override void Stop()
        {
        }

    }
}