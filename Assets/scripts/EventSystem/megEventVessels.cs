using UnityEngine;
using System.Collections;
using System.Linq;
using Meg.Networking;

namespace Meg.EventSystem
{

    [System.Serializable]
    public class megEventVessels : megEvent
    {

        // Properties
        // ------------------------------------------------------------

        public JSONObject vesselMovements;


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for a sonar event. */
        public megEventVessels(megEventGroup group = null)
            : base(megEventType.Vessels, group) { }


        // Members
        // ------------------------------------------------------------

        /** Initial state of the vessel movements simulation. */
        private JSONObject _initialState;


        // Public Methods
        // ------------------------------------------------------------

        /** Capture current vessel simulation state. */
        public void Capture()
        {
            vesselMovements = serverUtils.VesselMovements.SaveWithState();
        }


        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public override JSONObject Save()
        {
            var json = base.Save();
            json.AddField("vesselMovements", vesselMovements);

            return json;
        }

        /** Load movement state from JSON. */
        public override void Load(JSONObject json)
        {
            base.Load(json);
            vesselMovements = json.GetField("vesselMovements");
        }


        // Protected Methods
        // ------------------------------------------------------------

        /** Start this event. */
        protected override void Start()
        {
            if (vesselMovements == null)
                return;

            _initialState = serverUtils.VesselMovements.Save();
            serverUtils.VesselMovements.Load(vesselMovements);
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
            if (_initialState != null)
                serverUtils.VesselMovements.Load(_initialState);
        }

    }

}