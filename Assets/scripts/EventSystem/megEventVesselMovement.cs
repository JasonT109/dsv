using Meg.Networking;

namespace Meg.EventSystem
{

    [System.Serializable]
    public class megEventVesselMovement : megEvent
    {

        // Properties
        // ------------------------------------------------------------

        public int Vessel = 1;
        public string Type = vesselMovements.NoType;
        public float Speed;
        public bool AutoSpeed;
        public int TargetVessel = 1;
        public float Heading;
        public float DiveAngle;
        public float Period = 60;


        // Derived Properties
        // ------------------------------------------------------------

        public bool IsNone { get { return Type == vesselMovements.NoType; } }
        public bool IsIntercept { get { return Type == vesselMovements.InterceptType; } }
        public bool IsPursue { get { return Type == vesselMovements.PursueType; } }
        public bool IsHolding { get { return Type == vesselMovements.HoldingType; } }
        public bool IsSetVector { get { return Type == vesselMovements.SetVectorType; } }


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for a vessel movement event. */
        public megEventVesselMovement(megEventGroup group = null)
            : base(megEventType.VesselMovement, group)
        {
        }


        // Public Methods
        // ------------------------------------------------------------

        /** Execute this event's effect, regardless of timing. */
        public override void Execute()
        {
            base.Execute();

            var json = SaveVesselMovement();
            file.PostVesselMovementState(Vessel, json);
        }

        /** Capture current vessel simulation state. */
        public void Capture()
        {
            // Save movement state for this vessel.
            var json = serverUtils.VesselMovements.SaveVessel(Vessel);
            if (!json.IsNull)
                LoadVesselMovement(json);
        }

        /** String representation. */
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(triggerLabel))
                return triggerLabel;

            var vesselName = serverUtils.VesselData.GetDebugName(Vessel);
            var movementType = Type;
            if (IsPursue)
            {
                var targetName = serverUtils.VesselData.GetDebugName(TargetVessel);
                movementType = string.Format("{0} {1}", Type, targetName);
            }

            return string.Format("{0}: {1}, {2:n1}m/s", vesselName, movementType, Speed);
        }


        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public override JSONObject Save()
        {
            var json = base.Save();
            var movementJson = SaveVesselMovement();
            json.AddField("movement", movementJson);
            return json;
        }

        /** Load movement state from JSON. */
        public override void Load(JSONObject json)
        {
            base.Load(json);
            var movementJson = json.GetField("movement");
            if (movementJson)
                LoadVesselMovement(movementJson);
        }

        /** Save state to vessel movement JSON. */
        private JSONObject SaveVesselMovement()
        {
            var json = new JSONObject();
            json.AddField("Vessel", Vessel);
            json.AddField("Type", Type);
            if (!IsNone)
                json.AddField("Speed", Speed);

            if (IsIntercept)
                json.AddField("AutoSpeed", AutoSpeed);
            else if (IsPursue)
                json.AddField("TargetVessel", TargetVessel);
            else if (IsHolding)
                json.AddField("Period", Period);
            else if (IsSetVector)
            {
                json.AddField("Heading", Heading);
                json.AddField("DiveAngle", DiveAngle);
            }

            return json;
        }

        /** Load state from vessel movement JSON. */
        private void LoadVesselMovement(JSONObject json)
        {
            json.GetField(ref Vessel, "Vessel");
            json.GetField(ref Type, "Type");
            if (!IsNone)
                json.GetField(ref Speed, "Speed");

            if (IsIntercept)
                json.GetField(ref AutoSpeed, "AutoSpeed");
            else if (IsPursue)
                json.GetField(ref TargetVessel, "TargetVessel");
            else if (IsHolding)
                json.GetField(ref Period, "Period");
            else if (IsSetVector)
            {
                json.GetField(ref Heading, "Heading");
                json.GetField(ref DiveAngle, "DiveAngle");
            }
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
