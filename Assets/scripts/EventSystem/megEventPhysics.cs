using Meg.Networking;
using UnityEngine;

namespace Meg.EventSystem
{

    /** An event that applies physics to the player vessel. */
    [System.Serializable]
    public class megEventPhysics : megEvent
    {

        // Properties
        // ------------------------------------------------------------

        /** Id for this event. */
        public override string id { get { return "Physics"; } }

        /** Direction of physics impact force. */
        public Vector3 physicsDirection;

        /** Magnitude of physics impact force. */
        public float physicsMagnitude;


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for an event. */
        public megEventPhysics() : base(megEventType.Physics) { }


        // Load / Save
        // ------------------------------------------------------------

        /** Save movement state to JSON. */
        public override JSONObject Save()
        {
            var json = base.Save();
            json.AddField("Direction", physicsDirection);
            json.AddField("Magnitude", physicsMagnitude);
            return json;
        }

        /** Load movement state from JSON. */
        public override void Load(JSONObject json)
        {
            base.Load(json);
            json.GetField(ref physicsDirection, "Direction");
            json.GetField(ref physicsMagnitude, "Magnitude");
        }


        // Protected Methods
        // ------------------------------------------------------------

        /** Start this event. */
        protected override void Start()
        {
            Debug.Log("Physics event.");
            serverUtils.ServerData.RpcImpact(physicsDirection * physicsMagnitude);
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