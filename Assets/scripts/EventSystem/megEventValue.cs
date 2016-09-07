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
        public string serverParam = "";

        /** Value to apply to server data. */
        public float serverValue;

        /** Optional initial value. */
        public float initialValue;

        /** Whether to apply initial value when event starts. */
        public bool applyInitialValue;


        // Members
        // ------------------------------------------------------------

        /** Initial value, used when interpolating. */
        private float _initialValue;

        /** Current value. */
        private float _value = float.MinValue;


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for an event. */
        public megEventValue(megEventGroup group = null) : base(megEventType.Value, group) { }


        // Public Methods
        // ------------------------------------------------------------

        /** String representation. */
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(triggerLabel))
                return triggerLabel;
            if (string.IsNullOrEmpty(serverParam))
                return base.ToString();

            var value = serverUtils.GetServerData(serverParam);
            if (applyInitialValue)
                return string.Format("{0}: {1:N1}->{2:N1} ({3:N1})", serverParam, initialValue, serverValue, value);
            
            return string.Format("{0}: {1:N1} ({2:N1})", serverParam, serverValue, value);
        }

        /** Execute this event's effect, regardless of timing. */
        public override void Execute()
        {
            base.Execute();
            PostServerData(serverParam, serverValue);
        }


        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public override JSONObject Save()
        {
            var json = base.Save();
            json.AddField("serverParam", serverParam);
            json.AddField("serverValue", serverValue);

            if (!Mathf.Approximately(initialValue, 0))
                json.AddField("initialValue", initialValue);

            return json;
        }

        /** Load state from JSON. */
        public override void Load(JSONObject json)
        {
            base.Load(json);
            json.GetField(ref serverParam, "serverParam");
            json.GetField(ref serverValue, "serverValue");
            json.GetField(ref initialValue, "initialValue");
        }


        // Protected Methods
        // ------------------------------------------------------------

        /** Start this event. */
        protected override void Start()
        {
            // Get initial value.
            _initialValue = applyInitialValue 
                ? initialValue 
                : serverUtils.GetServerDataRaw(serverParam);

            // Optionally apply a specified initial value.
            if (applyInitialValue)
            {
                _value = initialValue;
                PostServerData(serverParam, _value);
            }

            // Apply final value immediately if needed.
            if (completeTime <= 0)
            {
                _value = serverValue;
                PostServerData(serverParam, _value);
                completed = true;
            }
        }

        /** Update this event internally. */
        protected override void Update(float t, float dt)
        {
            // Interpolate towards final value over time.
            _value = Mathf.Lerp(_initialValue, serverValue, timeFraction);
            PostServerData(serverParam, _value);

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
