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
            // Capture the full state of all vessels (movements, position, visibility).
            vesselMovements = serverUtils.VesselMovements.SaveFullState();
        }

        /** String representation. */
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(triggerLabel))
                return triggerLabel;

            return "Vessels State";
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

            if (_initialState == null)
                _initialState = serverUtils.VesselMovements.SaveFullState();

            serverUtils.PostVesselMovementsState(vesselMovements);
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
            // Restore initial vessel movements state (including position and visibility).
            if (_initialState != null)
                serverUtils.PostVesselMovementsState(_initialState);
        }

    }

}