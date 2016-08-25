using UnityEngine;
using System.Collections;
using System.Linq;
using Meg.Networking;

namespace Meg.EventSystem
{

    [System.Serializable]
    public class megEventPopup : megEvent
    {

        // Properties
        // ------------------------------------------------------------

        public string Title = "WARNING";
        public string Target = "";
        public Vector3 Position = Vector3.zero;
        public popupData.Icon Icon = popupData.Icon.Exclamation;


        // Private Properties
        // ------------------------------------------------------------

        /** Popup configuration data. */
        private popupData.Popup Popup
        {
            get
            {
                return new popupData.Popup
                {
                    Title = Title,
                    Target = Target,
                    Position = Position,
                    Icon = Icon
                };
            }
        }


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for a sonar event. */
        public megEventPopup(megEventGroup group = null)
            : base(megEventType.Popup, group) { }


        // Public Methods
        // ------------------------------------------------------------

        /** Execute this event's effect, regardless of timing. */
        public override void Execute()
        {
            base.Execute();
            file.PostAddPopup(Popup);
        }


        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public override JSONObject Save()
        {
            var json = base.Save();
            json.AddField("Title", Title);
            json.AddField("Target", Target);
            json.AddField("Position", Position);
            json.AddField("Icon", (int) Icon);

            return json;
        }

        /** Load movement state from JSON. */
        public override void Load(JSONObject json)
        {
            base.Load(json);
            json.GetField(ref Title, "Title");
            json.GetField(ref Target, "Target");
            json.GetField(ref Position, "Position");

            var icon = 0;
            json.GetField(ref icon, "Icon");
            Icon = (popupData.Icon) icon;
        }

        /** String representation. */
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(triggerLabel))
                return triggerLabel;

            if (!string.IsNullOrEmpty(Title))
                return string.Format("Popup: '{0}'", Title);

            return base.ToString();
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
            if (timeFraction < 1 || completed)
                return;

            // Event is complete.
            completed = true;

            // If event has a duration, hide the popup when it completes.
            if (completeTime > 0)
                file.PostRemovePopup(Popup);
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
